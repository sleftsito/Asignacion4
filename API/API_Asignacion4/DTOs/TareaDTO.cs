using System.Text.Json.Serialization;
public class TareaDTO
{
    [JsonInclude]
    public int Id;
    [JsonInclude]
    public string Titulo;
    [JsonInclude]
    public string Descripcion;
    [JsonInclude]
    public DateOnly FCreacion;
    [JsonInclude]
    public DateOnly FLimite;
    [JsonInclude]
    public bool Estado;

    public TareaDTO(string tituloI, string descripcionI, DateOnly FCreacionI, DateOnly FLimiteI, bool estadoI)
    {
        this.Titulo = tituloI;
        this.Descripcion = descripcionI;
        this.FCreacion = FCreacionI;
        this.FLimite = FLimiteI;
        this.Estado = estadoI;
    }
    public TareaDTO()
    {
        
    }
    public void SetId(int IdI)
    {
        this.Id = IdI;
    }
}
    