using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    public interface ITodoCollection
    {
        Task create(String title);
        Task create(String title, String notes);

        Task checkOffTodo(Todo todo);
        Task uncheckTodo(Todo todo);
        Task serializeTodo(Todo todo);

        Task checkOffIndex(int index);
        Task uncheckIndex(int index);
        Task serializeIndex(int index);

        Task serializeAllTodos();
        Task deserializeAllTodos();
    }
}
