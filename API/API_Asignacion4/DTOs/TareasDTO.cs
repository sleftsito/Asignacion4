using System.Text.Json.Serialization;
public class TareasDTO
{
    [JsonInclude]
    public List<TareaDTO> tareasDTO = new List<TareaDTO>();
}