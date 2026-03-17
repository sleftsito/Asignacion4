using System.Text.Json;

public class PersistenciaJson
{
    private static string Directorio = AppContext.BaseDirectory;
    private static int d = Directorio.IndexOf("\\bin\\Debug\\net10.0\\");
    private static string directorioData = Directorio.Remove(d);
    public static void GuardarTarea(ModeloTarea Tarea)
    {
        string jsonpath = directorioData; 
        string fileName = "Tasks.json"; 
        string fullFile = Path.Combine(jsonpath, fileName);

        Tareas Tareas = new Tareas();
        Tareas.tareas = ObtenerTareas();
        Tareas.tareas.Add(Tarea);
        string jsonString = JsonSerializer.Serialize(Tareas); 
        File.WriteAllText(fullFile, jsonString);
    }
    public static List<ModeloTarea> ObtenerTareas()
    {
        string jsonpath = directorioData;
        string fileName = "Tasks.json";
        string fullFile = Path.Combine(jsonpath, fileName);
        try
        {
            string jsonString = File.ReadAllText(fullFile);
            Tareas? Tareas;
            Tareas = JsonSerializer.Deserialize<Tareas>(jsonString);
            if(Tareas != null)
                return Tareas.tareas;
            return new List<ModeloTarea>();
        }catch(Exception ex)
        {
            return new List<ModeloTarea>();
        }
    }
    public static void EditarTarea(Tareas tareas)
    {
        string jsonpath = directorioData; 
        string fileName = "Tasks.json"; 
        string fullFile = Path.Combine(jsonpath, fileName);

        string jsonString = JsonSerializer.Serialize(tareas); 
        File.WriteAllText(fullFile, jsonString);
    }
    public static void EliminarTarea(ModeloTarea eliminarTarea)
    {
        string jsonpath = directorioData; 
        string fileName = "Tasks.json"; 
        string fullFile = Path.Combine(jsonpath, fileName);

        Tareas tareas = new Tareas();
        tareas.tareas = ObtenerTareas();
        tareas.tareas.RemoveAll(t => t.Id == eliminarTarea.Id);
 
        string jsonString = JsonSerializer.Serialize(tareas); 
        File.WriteAllText(fullFile, jsonString);
    }
    private static void EliminarTodasTareas(Tareas tareas)
    {
        tareas.tareas = ObtenerTareas();
        for(int i = 0; i < tareas.tareas.Count; i++)
        {
            tareas.tareas.Remove(tareas.tareas[i]);
        }

        string jsonpath = directorioData; 
        string fileName = "Tasks.json"; 
        string fullFile = Path.Combine(jsonpath, fileName);
        string jsonString = JsonSerializer.Serialize(tareas); 
        File.WriteAllText(fullFile, jsonString);
    }
}