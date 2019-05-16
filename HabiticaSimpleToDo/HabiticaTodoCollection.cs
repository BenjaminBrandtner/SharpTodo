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

        public IList<HabiticaTodo> TodoList { get => todoList; }

        //Interface Methods
        public async Task checkOffIndex(int index)
        {
            throw new NotImplementedException();
        }

        public async Task checkOffTodo(Todo todo)
        {
            throw new NotImplementedException();
        }

        public async Task create(string title)
        {
            client.createNewTodo(title, "");
            //TODO: add the result to todoList, once this method returns a result
        }

        public async Task create(string title, string notes)
        {
            client.createNewTodo(title, notes);
            //TODO: add the result to todoList, once this method returns a result
        }

        public async Task deserializeAllTodos()
        {
            todoList = await client.getTodos();
        }

        public async Task serializeAllTodos()
        {
            throw new NotImplementedException();
        }

        public async Task serializeIndex(int index)
        {
            throw new NotImplementedException();
        }

        public async Task serializeTodo(Todo todo)
        {
            throw new NotImplementedException();
        }

        public async Task uncheckIndex(int index)
        {
            throw new NotImplementedException();
        }

        public async Task uncheckTodo(Todo todo)
        {
            throw new NotImplementedException();
        }
    }
}
