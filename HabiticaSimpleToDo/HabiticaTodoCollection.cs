using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    public class HabiticaTodoCollection : ITodoCollection
    {
        private HabiticaHttpClient client;
        private IList<HabiticaTodo> todoList;

        public HabiticaTodoCollection()
        {
            client = HabiticaHttpClient.getInstance();
            todoList = new List<HabiticaTodo>();
        }

        //Interface Methods
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
            client.createNewTodo(title, "");
            //add the result to todoList, once this method returns a result
        }

        public void create(string title, string notes)
        {
            client.createNewTodo(title, notes);
            //add the result to todoList, once this method returns a result
        }

        public async void deserializeAllTodos()
        {
            todoList = await client.getTodos();
        }

        public void serializeAllTodos()
        {
            throw new NotImplementedException();
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
