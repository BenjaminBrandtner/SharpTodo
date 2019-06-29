using Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace ViewModel
{
    public class VMMainWindow : INotifyPropertyChanged
    {
        private ObservableCollection<VMHabiticaTodo> todoList;

        public event PropertyChangedEventHandler PropertyChanged;

        private String errorMessage;
        private String successMessage;
        private String message;

        private ICommand fetchCommand;
        private ICommand saveCommand;
        private ICommand createCommand;
        private ICommand deleteCommand;
        private ICommand checkOffCommand;

        public VMMainWindow()
        {
            TodoList = new ObservableCollection<VMHabiticaTodo>();
            FetchCommand = new UserCommand(new Action<object>(FetchTodos));
            CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
            DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
            SaveCommand = new UserCommand(new Action<object>(SaveTodos));
            CheckOffCommand = new UserCommand(new Action<object>(ChangeTodoCompletionStatus));

            FetchTodos(null);
        }

        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
        public void OnPropertyChanged(PropertyChangedEventArgs e) { PropertyChanged?.Invoke(this, e); }
        public string ErrorMessage
        {
            get => errorMessage;
            set { errorMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("ErrorMessage")); }
        }
        public string SuccessMessage
        {
            get => successMessage;
            set { successMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("SuccessMessage")); }
        }
        public string Message
        {
            get => message;
            set { message = value; OnPropertyChanged(new PropertyChangedEventArgs("Message")); }
        }
        public ICommand FetchCommand { get => fetchCommand; set => fetchCommand = value; }
        public ICommand CreateCommand { get => createCommand; set => createCommand = value; }
        public ICommand DeleteCommand { get => deleteCommand; set => deleteCommand = value; }
        public ICommand SaveCommand { get => saveCommand; set => saveCommand = value; }
        public ICommand CheckOffCommand { get => checkOffCommand; set => checkOffCommand = value; }

        private async void ChangeTodoCompletionStatus(object obj)
        {
            VMHabiticaTodo todo = (VMHabiticaTodo)obj;
            try
            {
                HabiticaClient client = HabiticaClient.GetInstance();

                if (!todo.Completed)
                {
                    await client.CheckOffTodo(todo.Todo);
                    todo.Completed = true;
                }
                else
                {
                    await client.UncheckTodo(todo.Todo);
                    todo.Completed = false;
                }
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        private async void SaveTodos(object obj)
        {
            VMHabiticaTodo vmTodo = (VMHabiticaTodo)obj;

            try
            {
                HabiticaClient client = HabiticaClient.GetInstance();
                await client.SaveTodo(vmTodo.Todo);
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        private async void DeleteTodo(object obj)
        {
            VMHabiticaTodo vmTodo = (VMHabiticaTodo)obj;

            try
            {
                HabiticaClient client = HabiticaClient.GetInstance();
                await client.DeleteTodo(vmTodo.Todo);

                TodoList.Remove(vmTodo);
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        private async void CreateNewTodo(object obj)
        {
            try
            {
                HabiticaClient client = HabiticaClient.GetInstance();
                TodoList.Insert(0,new VMHabiticaTodo(await client.CreateNewTodo("new Todo")));
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        private async void FetchTodos(object o)
        {
            try
            {
                HabiticaClient client = HabiticaClient.GetInstance();

                IList<HabiticaTodo> templist = await client.GetTodos();

                TodoList.Clear();

                foreach (HabiticaTodo h in templist)
                {
                    todoList.Add(new VMHabiticaTodo(h));
                }
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        private void handleException(Exception e)
        {
            ErrorMessage = e.Message;
        }
    }
}
