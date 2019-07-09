using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Backend;

namespace ViewModel
{
	public class VMTodoList
	{
		private readonly VMMainWindow parentWindow;

		public VMTodoList(VMMainWindow vmMainWindow)
		{
			this.parentWindow = vmMainWindow;

			TodoList = new ObservableCollection<VMHabiticaTodo>();
			FetchCommand = new UserCommand(new Action<object>(FetchTodos));
			CreateCommand = new UserCommand(new Action<object>(CreateNewTodo));
			DeleteCommand = new UserCommand(new Action<object>(DeleteTodo));
			SaveCommand = new UserCommand(new Action<object>(SaveTodo));
			LoadCommand = new UserCommand(new Action<object>(LoadTodo));
			CheckOffCommand = new UserCommand(new Action<object>(ChangeTodoCompletionStatus));

			FetchTodos(null);
		}

		//Properties
		public ObservableCollection<VMHabiticaTodo> TodoList { get; set; }
		//Commands
		public ICommand FetchCommand { get; set; }
		public ICommand CreateCommand { get; set; }
		public ICommand DeleteCommand { get; set; }
		public ICommand SaveCommand { get; set; }
		public ICommand CheckOffCommand { get; set; }
		public ICommand LoadCommand { get; set; }

		//Todo Methods
		private async void ChangeTodoCompletionStatus(object obj)
		{
			parentWindow.Busy = true;
			parentWindow.ClearMessages();

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
				parentWindow.handleException(e);
			}
			finally
			{
				parentWindow.Busy = false;
			}
		}

		private async void SaveTodo(object obj)
		{
			parentWindow.Busy = true;
			parentWindow.ClearMessages();

			VMHabiticaTodo vmTodo = (VMHabiticaTodo)obj;

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();
				await client.SaveTodo(vmTodo.Todo);
				parentWindow.SuccessMessage = "Changes saved";
			}
			catch (Exception e)
			{
				parentWindow.handleException(e);
			}
			finally
			{
				parentWindow.Busy = false;
			}
		}

		private async void LoadTodo(object obj)
		{
			parentWindow.Busy = true;
			parentWindow.ClearMessages();

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
				parentWindow.handleException(e);
			}
			finally
			{
				parentWindow.Busy = false;
			}
		}

		private async void DeleteTodo(object obj)
		{
			parentWindow.Busy = true;
			parentWindow.ClearMessages();

			VMHabiticaTodo vmTodo = (VMHabiticaTodo)obj;

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();
				await client.DeleteTodo(vmTodo.Todo);

				TodoList.Remove(vmTodo);
			}
			catch (Exception e)
			{
				parentWindow.handleException(e);
			}
			finally
			{
				parentWindow.Busy = false;
			}
		}

		private async void CreateNewTodo(object obj)
		{
			parentWindow.Busy = true;
			parentWindow.ClearMessages();

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();
				TodoList.Insert(0, new VMHabiticaTodo(await client.CreateNewTodo("new Todo")));
			}
			catch (Exception e)
			{
				parentWindow.handleException(e);
			}
			finally
			{
				parentWindow.Busy = false;
			}
		}

		private async void FetchTodos(object o)
		{
			parentWindow.Busy = true;
			parentWindow.ClearMessages();

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance();

				IList<HabiticaTodo> templist = await client.GetTodos();

				TodoList.Clear();

				foreach (HabiticaTodo h in templist)
				{
					TodoList.Add(new VMHabiticaTodo(h));
				}
			}
			catch (Exception e)
			{
				parentWindow.handleException(e);
			}
			finally
			{
				parentWindow.Busy = false;
			}
		}
	}
}
