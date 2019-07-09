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
		private Object currentView;

		private string errorMessage;
		private string successMessage;
		private string message;
		private bool busy;

		public event PropertyChangedEventHandler PropertyChanged;

		public VMMainWindow()
		{
			ShowOptionsCommand = new UserCommand(new Action<object>(ShowOptions));
			ShowTodoListCommand = new UserCommand(new Action<object>(ShowTodoList));

			Busy = false;
			CurrentView = new VMTodoList(this);
		}

		//Properties
		public object CurrentView
		{
			get => currentView;
			set { currentView = value; OnPropertyChanged(new PropertyChangedEventArgs("CurrentView")); }
		}
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
		public ICommand ShowOptionsCommand { get; set; }
		public ICommand ShowTodoListCommand { get; set; }

		//UI Methods
		internal void ClearMessages()
		{
			ErrorMessage = "";
			SuccessMessage = "";
			Message = "";
		}

		internal void ShowTodoList(object obj)
		{
			CurrentView = new VMTodoList(this);
		}

		internal void ShowOptions(object obj)
		{
			CurrentView = new VMOptions(this);
		}

		internal void handleException(Exception e)
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
	}
}
