var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("http://localhost:5231");
// 1. Definir la política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()   // Permite peticiones desde cualquier lugar
              .AllowAnyMethod()   // Permite GET, POST, PUT, DELETE, PATCH
              .AllowAnyHeader();  // Permite cualquier encabezado (como Content-Type)
    });
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    var url = "http://127.0.0.1:5500/index.html"; // Tu URL del Frontend
    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
    {
        FileName = url,
        UseShellExecute = true
    });
}
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
        context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        context.Response.StatusCode = 204;
        return;
    }
    await next();
});
app.ObtenerEndPoints();
// 2. Habilitar la política (Debe ir antes de MapControllers o MapEndpoints)
app.UseCors("PermitirTodo");

app.Run();
