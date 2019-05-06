using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    public interface ITodoCollection
    {
        //Eventuell müssen manche dieser Methoden statt void Tasks zurückgeben?
        void create(String title);
        void create(String title, String notes);

        void checkOffTodo(Todo todo);
        void uncheckTodo(Todo todo);
        void serializeTodo(Todo todo);

        void checkOffIndex(int index);
        void uncheckIndex(int index);
        void serializeIndex(int index);

        void serializeAllTodos();
        void deserializeAllTodos();
    }
}
