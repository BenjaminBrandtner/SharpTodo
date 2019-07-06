using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

using Backend;

namespace ViewModel
{
	public class VMMainWindow : INotifyPropertyChanged
	{
		private ObservableCollection<VMHabiticaTodo> todoList;

		public event PropertyChangedEventHandler PropertyChanged;

		private String errorMessage;
		private String successMessage;
		private String message;
		private bool busy;

		private ICommand fetchCommand;
		private ICommand saveCommand;
		private ICommand loadCommand;
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
			LoadCommand = new UserCommand(new Action<object>(LoadTodos));
			CheckOffCommand = new UserCommand(new Action<object>(ChangeTodoCompletionStatus));

			Busy = false;

			FetchTodos(null);
		}

		public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
		public void OnPropertyChanged(PropertyChangedEventArgs e) { PropertyChanged?.Invoke(this, e); }
		public bool Busy
		{
			get => busy;
            set { busy = value; OnPropertyChanged(new PropertyChangedEventArgs("Busy")); }
        }
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
		public ICommand LoadCommand { get => loadCommand; set => loadCommand = value; }

		private async void ChangeTodoCompletionStatus(object obj)
		{
			Busy = true;

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
			finally
			{
				Busy = false;
			}
		}

		private async void SaveTodos(object obj)
		{
			Busy = true;

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
			finally
			{
				Busy = false;
			}
		}

		private async void LoadTodos(object obj)
		{
			Busy = true;

			VMHabiticaTodo oldVmTodo = (VMHabiticaTodo)obj;
			int index = TodoList.IndexOf(oldVmTodo);

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();
				TodoList.Insert(index, new VMHabiticaTodo(await client.LoadTodo(oldVmTodo.Todo)));

				TodoList.Remove(oldVmTodo);
			}
			catch (Exception e)
			{
				handleException(e);
			}
			finally
			{
				Busy = false;
			}
		}

		private async void DeleteTodo(object obj)
		{
			Busy = true;

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
			finally
			{
				Busy = false;
			}
		}

		private async void CreateNewTodo(object obj)
		{
			Busy = true;

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();
				TodoList.Insert(0, new VMHabiticaTodo(await client.CreateNewTodo("new Todo")));
			}
			catch (Exception e)
			{
				handleException(e);
			}
			finally
			{
				Busy = false;
			}
		}

		private async void FetchTodos(object o)
		{
			Busy = true;

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
			finally
			{
				Busy = false;
			}
		}

		private void handleException(Exception e)
		{
			switch (e)
			{
				case NoCredentialsException _:
					ErrorMessage = "No Credentials found. Please input your User ID and API Token in the options menu.";
					break;
				case WrongCredentialsException _:
					ErrorMessage = "User ID or API Token are incorrect.";
					break;
				case UnsuccessfulException _:
					ErrorMessage = "Habitica recieved the request, but something went wrong: \n" + e.Message;
					break;
				case WebException _:
					ErrorMessage = e.Message;
					break;
				case TaskCanceledException _:
					ErrorMessage = "The request timed out, maybe Habitica is unreachable at the moment.";
					break;
				default:
					throw (e);
			}
		}
	}
}
