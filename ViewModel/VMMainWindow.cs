using Backend;
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
        
        private String errorMsg;
        private HabiticaClient client;
        private ICommand fetchCommand;
        private ICommand sendCommand;
        private ICommand createCommand;
        private ICommand deleteCommand;
        public VMMainWindow()
        {
            TodoList = new ObservableCollection<VMHabiticaTodo>();
            FetchCommand = new UserCommand(new Action<object>(FetchTodos));
            CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
            DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
            SendCommand = new UserCommand(new Action<object>(SendTodos));
            GetHabiticaClientInstance();
        }

        private void GetHabiticaClientInstance()
        {
            ErrorMsg = "";
            try
            {
                client = HabiticaClient.GetInstance();
                
            }
            catch (NoCredentialsException e)
            {
                ErrorMsg = "No User Credentials found. Please use the Options menu to input your credentials";
            }
        }

        private async void SendTodos(object obj)
        {
            GetHabiticaClientInstance();
            try
            {
                await client.SaveTodo(((VMHabiticaTodo)obj).Todo);
            }
            catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException)
            {
                ErrorMsg = e.Message;
            }
            

        }

        private async void DeleteTodo(object obj)
        {
            GetHabiticaClientInstance();
            try
            {
                await client.DeleteTodo(((VMHabiticaTodo)obj).Todo);
            }
            catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException)
            {
                ErrorMsg = e.Message;
            }

        }

        private async void CreateNewTodo(object obj)
        {
            GetHabiticaClientInstance();
            try
            {
                TodoList.Add(new VMHabiticaTodo(await client.CreateNewTodo("new Todo")));
            }
            catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException)
            {
                ErrorMsg = e.Message;
            }

        }

        
        private async void FetchTodos(object o)
        {
            GetHabiticaClientInstance();
            TodoList.Clear();
            try
            {
                IList<HabiticaTodo> templist = await client.GetTodos();
                foreach (HabiticaTodo h in templist)
                {
                    todoList.Add(new VMHabiticaTodo(h));
                }
            }
            catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException)
            {
                ErrorMsg = e.Message;
            }

        }
        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
        public ICommand FetchCommand { get => fetchCommand; set => fetchCommand = value; }
       

        public ICommand CreateCommand { get => createCommand; set => createCommand = value; }
        public ICommand DeleteCommand { get => deleteCommand; set => deleteCommand = value; }
        public ICommand SendCommand { get => sendCommand; set => sendCommand = value; }
        public string ErrorMsg { get => errorMsg; set => errorMsg = value; }
    }
}
