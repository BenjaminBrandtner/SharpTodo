using Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ViewModel
{
    public class VMMainWindow : INotifyPropertyChanged
    {
        private ObservableCollection<VMHabiticaTodo> todoList;

        private String errorMessage;
        private String successMessage;
        private HabiticaClient client;
        private ICommand fetchCommand;
        private ICommand saveCommand;
        private ICommand createCommand;
        private ICommand deleteCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public VMMainWindow()
        {
            TodoList = new ObservableCollection<VMHabiticaTodo>();
            FetchCommand = new UserCommand(new Action<object>(FetchTodos));
            CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
            DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
            SaveCommand = new UserCommand(new Action<object>(SaveTodos));
            ErrorMessage = "";
            GetHabiticaClientInstance();
        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void GetHabiticaClientInstance()
        {
            ErrorMessage = "";
            try
            {
                client = HabiticaClient.GetInstance();
            }
            catch (NoCredentialsException)
            {
                ErrorMessage = "No User Credentials found. Please use the Options menu to input your credentials";
            }
        }

        private async void SaveTodos(object obj)
        {
            GetHabiticaClientInstance();
            if (client != null)
            {
                try
                {
                    await client.SaveTodo(((VMHabiticaTodo)obj).Todo);
                }
                catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException || e is WebException)
                {
                    ErrorMessage = e.Message;
                }
            }
        }

        private async void DeleteTodo(object obj)
        {
            GetHabiticaClientInstance();
            if (client != null)
            {
                try
                {
                    await client.DeleteTodo(((VMHabiticaTodo)obj).Todo);
                }
                catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException || e is WebException)
                {
                    ErrorMessage = e.Message;
                }
            }
        }

        private async void CreateNewTodo(object obj)
        {
            GetHabiticaClientInstance();
            if (client != null)
            {
                try
                {
                    TodoList.Add(new VMHabiticaTodo(await client.CreateNewTodo("new Todo")));
                }
                catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException || e is WebException)
                {
                    ErrorMessage = e.Message;
                }
            }
        }

        private async void FetchTodos(object o)
        {
            GetHabiticaClientInstance();
            TodoList.Clear();
            if (client != null)
            {
                try
                {
                    IList<HabiticaTodo> templist = await client.GetTodos();
                    foreach (HabiticaTodo h in templist)
                    {
                        todoList.Add(new VMHabiticaTodo(h));
                    }
                }
                catch (Exception e) when (e is WrongCredentialsException || e is UnsuccessfulException || e is WebException)
                {
                    ErrorMessage = e.Message;
                }
            }
        }

        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
        public ICommand FetchCommand { get => fetchCommand; set => fetchCommand = value; }
        public ICommand CreateCommand { get => createCommand; set => createCommand = value; }
        public ICommand DeleteCommand { get => deleteCommand; set => deleteCommand = value; }
        public ICommand SaveCommand { get => saveCommand; set => saveCommand = value; }
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ErrorMessage"));
            }
        }

        public string SuccessMessage
        {
            get => successMessage;
            set
            {
                successMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SuccessMessage"));
            }
        }
    }
}
