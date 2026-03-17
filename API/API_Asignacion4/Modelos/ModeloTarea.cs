public class ModeloTarea
{
    public int Id {get; set;}
    public required string Titulo {get; set;}
    public string Descripcion {get; set;}
    public required DateOnly FCreacion {get; set;}
    public required DateOnly FLimite {get; set;}
    public required bool Estado {get; set;}
}