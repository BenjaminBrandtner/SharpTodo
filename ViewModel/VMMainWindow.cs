﻿using Backend;
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

        private String errorMessage;
        private String successMessage;
        private String message;
        private HabiticaClient client;
        private ICommand fetchCommand;
        private ICommand saveCommand;
        private ICommand createCommand;
        private ICommand deleteCommand;
        private ICommand checkOffCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public VMMainWindow()
        {
            TodoList = new ObservableCollection<VMHabiticaTodo>();
            FetchCommand = new UserCommand(new Action<object>(FetchTodos));
            CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
            DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
            SaveCommand = new UserCommand(new Action<object>(SaveTodos));
            CheckOffCommand = new UserCommand(new Action<object>(CheckOffTodo));
        }

        private async void CheckOffTodo(object obj)
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

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private async void SaveTodos(object obj)
        {
            HabiticaClient client = HabiticaClient.GetInstance();
            try
            {
                await client.SaveTodo(((VMHabiticaTodo)obj).Todo);
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        private async void DeleteTodo(object obj)
        {
            HabiticaClient client = HabiticaClient.GetInstance();
            try
            {
                await client.DeleteTodo(((VMHabiticaTodo)obj).Todo);
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        private async void CreateNewTodo(object obj)
        {
            HabiticaClient client = HabiticaClient.GetInstance();
            try
            {
                TodoList.Add(new VMHabiticaTodo(await client.CreateNewTodo("new Todo")));
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

        public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
        public ICommand FetchCommand { get => fetchCommand; set => fetchCommand = value; }
        public ICommand CreateCommand { get => createCommand; set => createCommand = value; }
        public ICommand DeleteCommand { get => deleteCommand; set => deleteCommand = value; }
        public ICommand SaveCommand { get => saveCommand; set => saveCommand = value; }
        public ICommand CheckOffCommand { get => checkOffCommand; set => checkOffCommand = value; }
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
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Message"));
            }
        }

    }
}
