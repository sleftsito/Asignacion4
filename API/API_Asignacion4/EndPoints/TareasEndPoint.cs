public static class TareasEndPoint
{
    public static void ObtenerEndPoints(this WebApplication app)
    {
        //Post
        app.MapPost("/CrearTarea", (TareaDTO nuevaTarea) => {
            TareaDTO tarea = new(
                nuevaTarea.Titulo,
                nuevaTarea.Descripcion,
                nuevaTarea.FCreacion,
                nuevaTarea.FLimite,
                nuevaTarea.Estado
            );
            int ID = 0;
            if(PersistenciaJson.ObtenerTareas().Count > 0)
                ID = PersistenciaJson.ObtenerTareas()[PersistenciaJson.ObtenerTareas().Count - 1].Id + 1; 
            ModeloTarea TareaModelo = new (){
                Id = ID,
                //No se necesita sumarle 1. Por que el Count igual siempre tiene un valor 
                // justo 1 mas grande que el ultimo elemento
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FCreacion = tarea.FCreacion,  
                FLimite = tarea.FLimite,  
                Estado = tarea.Estado
            };
            PersistenciaJson.GuardarTarea(TareaModelo);
            tarea.SetId(TareaModelo.Id);
            return Results.Ok(tarea);
        });

        //Get
        app.MapGet("/Tareas", () =>
        {
            Tareas tareas = new Tareas();
            tareas.tareas = PersistenciaJson.ObtenerTareas();
            if(tareas.tareas != null)
                return Results.Ok(tareas.tareas);
            return Results.NoContent();
        });
        app.MapGet("/Tareas/{Completada}", (bool Completada) =>
        {
            TareasDTO tareasCompletas = new TareasDTO();
            TareasDTO tareasImcompletas = new TareasDTO();
            
            Tareas tareas = new Tareas();
            tareas.tareas = PersistenciaJson.ObtenerTareas();
            if(tareas.tareas == null)
                return Results.NoContent();
            for(int i = 0; i < tareas.tareas.Count; i++)
            {
                TareaDTO tareadto = new TareaDTO(
                    tareas.tareas[i].Titulo,
                    tareas.tareas[i].Descripcion,
                    tareas.tareas[i].FCreacion,
                    tareas.tareas[i].FLimite,
                    tareas.tareas[i].Estado
                );
                tareadto.SetId(tareas.tareas[i].Id);
                if(tareadto.Estado == true)
                    tareasCompletas.tareasDTO.Add(tareadto);
                else
                    tareasImcompletas.tareasDTO.Add(tareadto);     
            }
            if (Completada)
            {
                if(tareasCompletas.tareasDTO.Count > 0)
                    return Results.Ok(tareasCompletas);
                else
                    return Results.Ok(new List<TareaDTO>());
            }
            else if(tareasImcompletas.tareasDTO.Count > 0)
                    return Results.Ok(tareasImcompletas);
                else
                    return Results.Ok(new List<TareaDTO>());    
            
        });

        app.MapGet("/Tarea/{id}", (int id) => 
        {
            Tareas tareas = new Tareas();
            tareas.tareas = PersistenciaJson.ObtenerTareas();
            
            if(tareas.tareas == null)
                return Results.NoContent();
            for(int i = 0; i < tareas.tareas.Count; i++)
            {
                if(tareas.tareas[i].Id == id)
                    return Results.Ok(tareas.tareas[i]);
            }
            return Results.NotFound(); 
        });

        //Put  
        app.MapPut("/Put/{id}", (int id, TareaDTO cambioTarea) => 
        {
            Tareas tareas = new Tareas(); 
            tareas.tareas = PersistenciaJson.ObtenerTareas();

            for(int i = 0; i < tareas.tareas.Count; i++)
            {
                if(tareas.tareas[i].Id == id)
                {
                    tareas.tareas[i].Titulo = cambioTarea.Titulo;
                    tareas.tareas[i].Descripcion = cambioTarea.Descripcion;
                    tareas.tareas[i].FCreacion = cambioTarea.FCreacion;
                    tareas.tareas[i].FLimite = cambioTarea.FLimite;
                    tareas.tareas[i].Estado = cambioTarea.Estado;
                    PersistenciaJson.EditarTarea(tareas);
                    return Results.NoContent(); //Parece ser la convencion regresar nada en un put
                }
            }
            return Results.NotFound();
        });
        //Patch
        app.MapPatch("/Patch/{id}/{completada}", (int id, bool completada) =>
        {
            Tareas tareas = new Tareas(); 
            tareas.tareas = PersistenciaJson.ObtenerTareas();

            for(int i = 0; i < tareas.tareas.Count; i++)
            {
                if(tareas.tareas[i].Id == id)
                {
                    tareas.tareas[i].Estado = completada;
                    PersistenciaJson.EditarTarea(tareas);
                    return Results.NoContent();//Parece ser la convencion regresar nada en un put
                }
            }
            return Results.NotFound();
        });

        //Delete
        app.MapDelete("/{id}", (int id) =>
        {
            Tareas tareas = new Tareas(); 
            tareas.tareas = PersistenciaJson.ObtenerTareas();

            for(int i = 0; i < tareas.tareas.Count; i++)
            {
                if(tareas.tareas[i].Id == id)
                {
                    PersistenciaJson.EliminarTarea(tareas.tareas[i]);
                    break;
                }
            }  
            return Results.NoContent(); //Parece ser la convencion regresar nada en un patch
        });
    }
}