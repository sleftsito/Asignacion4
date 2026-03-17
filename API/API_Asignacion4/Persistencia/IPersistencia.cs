public interface IPersistencia
{
    void GuardarTareas(ModeloTarea Tarea);
    List<ModeloTarea> Tareas();
}