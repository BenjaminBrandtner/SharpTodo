using HabiticaSimpleToDo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class VMMainWindow
    {
        private ObservableCollection<VMHabiticaTodo> todoList;
        private String username;
        private String password;
        private HabiticaClient client;
        private ICommand fetchCommand;
        private ICommand sendCommand;
        private ICommand createCommand;
        //private ICommand deleteCommand;
        public VMMainWindow()
        {
            TodoList = new ObservableCollection<VMHabiticaTodo>();
            Username = "bla";
            Password = "test";
            FetchCommand = new UserCommand(new Action<object>(FetchTodos));
            CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
            //DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
            SendCommand = new UserCommand(new Action<object>(SendTodos));
            client = HabiticaClient.GetInstance();
        }

        private async void SendTodos(object obj)
        {
            await client.SaveTodo(((VMHabiticaTodo)obj).Todo);
        }

        //private async void DeleteTodo(object obj)
        //{
        //    await client.deleteTodo(((VMHabiticaTodo)obj).Todo);
        //}

        private async void CreateNewTodo(object obj)
        {
            
            TodoList.Add(new VMHabiticaTodo(await client.CreateNewTodo("new Todo", "")));
        }

        
        private async void FetchTodos(object o)
        {
            TodoList.Clear();
            Console.WriteLine("test");
            IList<HabiticaTodo> templist=await client.GetTodos();
            foreach(HabiticaTodo h in templist)
            {
                todoList.Add(new VMHabiticaTodo(h));
            }
        }
        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
        public ICommand FetchCommand { get => fetchCommand; set => fetchCommand = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public ICommand CreateCommand { get => createCommand; set => createCommand = value; }
        //public ICommand DeleteCommand { get => deleteCommand; set => deleteCommand = value; }
        public ICommand SendCommand { get => sendCommand; set => sendCommand = value; }
    }
}
