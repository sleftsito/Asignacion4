using System.Text.Json.Serialization;
public class Tareas
{
    [JsonInclude]
    public List<ModeloTarea> tareas;
}