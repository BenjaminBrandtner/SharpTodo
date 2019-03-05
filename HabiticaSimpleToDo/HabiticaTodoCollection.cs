using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    public class HabiticaTodoCollection : ITodoCollection
    {
        public void checkOffIndex(int index)
        {
            throw new NotImplementedException();
        }

        public void checkOffTodo(Todo todo)
        {
            throw new NotImplementedException();
        }

        public void create(string title)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<HabiticaTodo>> deserializeAllTodos()
        {
            //A quick and dirty test method
            HabiticaHttpClient client = HabiticaHttpClient.getInstance();
            return await client.getTodos();
        }

        public void serializeIndex(int index)
        {
            throw new NotImplementedException();
        }

        public void serializeTodo(Todo todo)
        {
            throw new NotImplementedException();
        }

        public void uncheckIndex(int index)
        {
            throw new NotImplementedException();
        }

        public void uncheckTodo(Todo todo)
        {
            throw new NotImplementedException();
        }
    }
}
