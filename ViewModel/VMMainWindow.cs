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

		private String errorMessage;
		private String successMessage;
		private String message;
		private bool busy;

		public event PropertyChangedEventHandler PropertyChanged;

		public VMMainWindow()
		{
			TodoList = new ObservableCollection<VMHabiticaTodo>();
			FetchCommand = new UserCommand(new Action<object>(FetchTodos));
			CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
			DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
			SaveCommand = new UserCommand(new Action<object>(SaveTodo));
			LoadCommand = new UserCommand(new Action<object>(LoadTodo));
			CheckOffCommand = new UserCommand(new Action<object>(ChangeTodoCompletionStatus));
			ShowOptionsCommand = new UserCommand(new Action<object>(ShowOptions));
			ShowTodoListCommand = new UserCommand(new Action<object>(ShowTodoList));

			Busy = false;

			FetchTodos(null);
		}

		//Properties
		public ObservableCollection<VMHabiticaTodo> TodoList { get => todoList; set => todoList = value; }
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
		//Events
		public void OnPropertyChanged(PropertyChangedEventArgs e) { PropertyChanged?.Invoke(this, e); }
		//Commands
		public ICommand FetchCommand { get; set; }
		public ICommand CreateCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand SaveCommand { get; set; }
		public ICommand CheckOffCommand { get; set; }
		public ICommand LoadCommand { get; set; }
		public ICommand ShowOptionsCommand { get; set; }
		public ICommand ShowTodoListCommand { get; set; }

		//UI Methods
		private void ClearMessages()
		{
			ErrorMessage = "";
			SuccessMessage = "";
			Message = "";
		}

		private void ShowTodoList(object obj)
		{
			throw new NotImplementedException();
		}

		private void ShowOptions(object obj)
		{
			throw new NotImplementedException();
		}

		private void handleException(Exception e)
		{
			switch (e)
			{
				case NoCredentialsException _:
					ErrorMessage = "No Credentials found. Please input your User ID and API Token in the options menu, then reload.";
					break;
				case WrongCredentialsException _:
					ErrorMessage = "User ID or API Token are incorrect.";
					break;
				case UnsuccessfulException _:
					ErrorMessage = "Habitica recieved the request, but something went wrong: \n" + e.Message;
					break;
				case InvalidTodoException _:
					ErrorMessage = e.Message;
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

		//Todo Methods
		private async void ChangeTodoCompletionStatus(object obj)
		{
			Busy = true;
			ClearMessages();

			VMHabiticaTodo vmTodo = (VMHabiticaTodo)obj;
			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();

				if (!vmTodo.Completed)
				{
					await client.CheckOffTodo(vmTodo.Todo);
					vmTodo.Completed = true;
				}
				else
				{
					await client.UncheckTodo(vmTodo.Todo);
					vmTodo.Completed = false;
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

		private async void SaveTodo(object obj)
		{
			Busy = true;
			ClearMessages();

			VMHabiticaTodo vmTodo = (VMHabiticaTodo)obj;

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();
				await client.SaveTodo(vmTodo.Todo);
				SuccessMessage = "Changes saved";
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

		private async void LoadTodo(object obj)
		{
			Busy = true;
			ClearMessages();

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
			ClearMessages();

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
			ClearMessages();

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
			ClearMessages();

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

	}
}
