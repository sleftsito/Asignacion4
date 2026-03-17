const API_BASE = "http://127.0.0.1:5231";
const form = document.getElementById('todo-form');
const todoList = document.getElementById('todo-list');
const filterSelect = document.getElementById('status-filter');

// Cargar tareas al iniciar la página
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('todo-fcreacion').valueAsDate = new Date();
    obtenerTareas();
});

// GET: Obtener tareas (con soporte para filtros por endpoint)
async function obtenerTareas() {
    const filterValue = filterSelect.value;
    let url = `${API_BASE}/Tareas`;

    // Usar el endpoint de filtro si no es "all"
    if (filterValue !== 'all') {
        url = `${API_BASE}/Tareas/${filterValue}`;
    }

    try {
        const response = await fetch(url);
        const tasks = await response.json();
        renderTasks(tasks);
    } catch (error) {
        console.error("Error al obtener tareas:", error);
    }
}

// POST: Crear Tarea
form.addEventListener('submit', async (e) => {
    e.preventDefault();

    const newTask = {
        titulo: document.getElementById('todo-title').value,
        descripcion: document.getElementById('todo-desc').value,
        fCreacion: document.getElementById('todo-fcreacion').value,
        fLimite: document.getElementById('todo-flimite').value,
        estado: false
    };

    try {
        const response = await fetch(`${API_BASE}/CrearTarea`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newTask)
        });

        if (response.ok) {
            form.reset();
            document.getElementById('todo-fcreacion').valueAsDate = new Date();
            obtenerTareas();
        }
    } catch (error) {
        console.error("Error al crear tarea:", error);
    }
});

// PATCH: Cambiar estado (Marcar como completada)
async function completeTask(id) {
    try {
        // Según tu endpoint: PATCH /Patch/{id}/{nuevoEstado}
        const response = await fetch(`${API_BASE}/Patch/${id}/true`, {
            method: 'PATCH'
        });

        if (response.ok) {
            obtenerTareas();
        }
    } catch (error) {
        console.error("Error al actualizar tarea:", error);
    }
}

// DELETE: Eliminar tarea
async function deleteTask(id) {
    if (!confirm("¿Deseas eliminar esta tarea?")) return;

    try {
        const response = await fetch(`${API_BASE}/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            obtenerTareas();
        }
    } catch (error) {
        console.error("Error al eliminar tarea:", error);
    }
}

// Función para dibujar en el HTML
// function renderTasks(tasks) {
//     todoList.innerHTML = '';

//     // VALIDACIÓN CRÍTICA: Si 'tasks' no es un array, lo convertimos en uno
//     const taskArray = Array.isArray(tasks) ? tasks : (tasks ? [tasks] : []);

//     taskArray.forEach((task) => {
//         // ... (todo el código que ya tienes dentro del forEach)
//         const li = document.createElement('li');
//         li.className = 'task-card';
        
//         const colorClass = task.estado ? 'bg-green' : 'bg-red';
//         const taskId = task.id || task.Id;

//         li.innerHTML = `
//             <div class="status-indicator ${colorClass}"></div>
//             <div class="task-content">
//                 <h3>${task.titulo}</h3>
//                 <p>${task.descripcion}</p>
//                 <div class="task-dates">
//                     <b>Creado:</b> ${task.fCreacion} | <b>Límite:</b> ${task.fLimite}
//                 </div>
//                 <div class="task-actions">
//                     <button class="complete-btn" 
//                             onclick="completeTask(${taskId})" 
//                             ${task.estado ? 'disabled' : ''}>
//                         ${task.estado ? 'Completada' : 'Marcar como completada'}
//                     </button>
//                     <button class="delete-btn" onclick="deleteTask(${taskId})">Eliminar</button>
//                 </div>
//             </div>
//         `;
//         todoList.appendChild(li);
//     });
// }
//2
function renderTasks(data) {
    todoList.innerHTML = '';

    // 1. Normalizar los datos: 
    // Si la API devuelve { tareas: [...] }, extraemos la lista. 
    // Si devuelve el objeto directo, lo metemos en un array.
    let tasks = [];

    if (Array.isArray(data)) {
        tasks = data;
    } 
    // AGREGAMOS ESTA LÍNEA para que reconozca "tareasDTO"
    else if (data && Array.isArray(data.tareasDTO)) {
        tasks = data.tareasDTO;
    } 
    else if (data && Array.isArray(data.tareas)) {
        tasks = data.tareas;
    } 
    else if (data) {
        tasks = [data];
    }
    
    // ... resto del código (forEach, etc)

    if (tasks.length === 0 || tasks[0] === null) {
        todoList.innerHTML = '<p style="text-align:center; color:#888;">No hay tareas para mostrar.</p>';
        return;
    }

    tasks.forEach((task) => {
        const li = document.createElement('li');
        li.className = 'task-card';
        
        // 2. Resolver problema de Mayúsculas/Minúsculas (Case Insensitive)
        // Esto busca la propiedad sin importar si empieza con T o t.
        const titulo = task.titulo || task.Titulo || "Sin título";
        const descripcion = task.descripcion || task.Descripcion || "Sin descripción";
        const fCreacion = task.fCreacion || task.FCreacion || "N/A";
        const fLimite = task.fLimite || task.FLimite || "N/A";
        const estado = task.estado !== undefined ? task.estado : task.Estado;
        //const taskId = task.id || task.Id;
        const taskId = (task.id !== undefined) ? task.id : task.Id;

        const colorClass = estado ? 'bg-green' : 'bg-red';

        li.innerHTML = `
            <div class="status-indicator ${colorClass}"></div>
            <div class="task-content">
                <h3>${titulo}</h3>
                <p>${descripcion}</p>
                <div class="task-dates">
                    <b>Creado:</b> ${fCreacion} | <b>Límite:</b> ${fLimite}
                </div>
                <div class="task-actions">
                    <button class="complete-btn" 
                            onclick="completeTask(${taskId})" 
                            ${estado ? 'disabled' : ''}>
                        ${estado ? 'Completada' : 'Marcar como completada'}
                    </button>
                    <button class="delete-btn" onclick="deleteTask(${taskId})">Eliminar</button>
                </div>
                <div class="task-actions">
                    <button class="edit-btn" onclick='openEditModal(${JSON.stringify(task)})'>Editar</button>
                </div>
            </div>
        `;
        todoList.appendChild(li);
    });
}
const editModal = document.getElementById('edit-modal');
const editForm = document.getElementById('edit-form');

// Abrir modal y cargar datos
function openEditModal(task) {
    document.getElementById('edit-id').value = task.id ?? task.Id;
    document.getElementById('edit-title').value = task.titulo ?? task.Titulo;
    document.getElementById('edit-desc').value = task.descripcion ?? task.Descripcion;
    document.getElementById('edit-flimite').value = task.fLimite ?? task.FLimite;
    document.getElementById('edit-estado').checked = task.estado ?? task.Estado;
    
    // Guardamos la fecha de creación original para no perderla
    editForm.dataset.fcreacion = task.fCreacion ?? task.FCreacion;
    
    editModal.style.display = 'flex';
}

function closeEditModal() {
    editModal.style.display = 'none';
}

// Evento Submit del formulario de edición
editForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    const id = document.getElementById('edit-id').value;
    
    const updatedTask = {
        titulo: document.getElementById('edit-title').value,
        descripcion: document.getElementById('edit-desc').value,
        fCreacion: editForm.dataset.fcreacion, // Mantenemos la original
        fLimite: document.getElementById('edit-flimite').value,
        estado: document.getElementById('edit-estado').checked
    };

    try {
        const response = await fetch(`${API_BASE}/Put/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedTask)
        });

        if (response.ok) {
            closeEditModal();
            await obtenerTareas();
        }
    } catch (error) {
        console.error("Error al editar:", error);
    }
});


// Escuchar cambios en el select de filtro
filterSelect.addEventListener('change', obtenerTareas);