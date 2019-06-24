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
        private HabiticaTodoCollection todocollection;
        private ICommand fetchCommand;
        private ICommand sendCommand;
        private ICommand createCommand;
        private ICommand deleteCommand;
        public VMMainWindow()
        {
            TodoList = new ObservableCollection<VMHabiticaTodo>();
            Username = "bla";
            Password = "test";
            FetchCommand = new UserCommand(new Action<object>(FetchTodos));
            CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
            DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
            SendCommand = new UserCommand(new Action<object>(SendTodos));
            todocollection = new HabiticaTodoCollection();
        }

        private void SendTodos(object obj)
        {
            throw new NotImplementedException();
        }

        private void DeleteTodo(object obj)
        {
            throw new NotImplementedException();
        }

        private void CreateNewTodo(object obj)
        {
            throw new NotImplementedException();
        }

        
        private async void FetchTodos(object o)
        {
            Console.WriteLine("test");
            await todocollection.deserializeAllTodos();
            //TodoList = new ObservableCollection<VMHabiticaTodo>(((List<HabiticaTodo>)todocollection.TodoList));
        }
        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
        public ICommand FetchCommand { get => fetchCommand; set => fetchCommand = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public ICommand CreateCommand { get => createCommand; set => createCommand = value; }
        public ICommand DeleteCommand { get => deleteCommand; set => deleteCommand = value; }
        public ICommand SendCommand { get => sendCommand; set => sendCommand = value; }
    }
}
