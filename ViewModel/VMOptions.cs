using System;
using System.ComponentModel;
using System.Windows.Input;
using Backend;

namespace ViewModel
{
	public class VMOptions : INotifyPropertyChanged
	{
		private readonly ConfigManager configManager;
		private readonly dynamic config;

		private string errorMessage;
		private string successMessage;
		private bool busy;

		public event PropertyChangedEventHandler PropertyChanged;

		public VMOptions()
		{
			configManager = new ConfigManager();
			config = configManager.Read();

			ValidateCommand = new UserCommand(ValidateCredentials);
		}

		//Properties
		public string UserID { get => config.UserID; set => config.UserID = value; }
		public string ApiToken { get => config.ApiToken; set => config.ApiToken = value; }
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
		public bool Busy
		{
			get => busy;
			set { busy = value; OnPropertyChanged(new PropertyChangedEventArgs("Busy")); }
		}
		//Events
		public void OnPropertyChanged(PropertyChangedEventArgs e) { PropertyChanged?.Invoke(this, e); }
		//Commands
		public ICommand ValidateCommand { get; set; }


		private async void ValidateCredentials(object obj)
		{
			ErrorMessage = "";
			SuccessMessage = "";
			Busy = true;

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance(config);
				await client.TestConnection();

				SuccessMessage = "Credentials validated successfully";

				configManager.Write(config);
			}
			catch (NoCredentialsException)
			{
				ErrorMessage = "One of the required fields is empty";
			}
			catch (WrongCredentialsException e)
			{
				ErrorMessage = e.Message;
			}
			catch (UnsuccessfulException e)
			{
				ErrorMessage = e.Message;
			}
			finally
			{
				Busy = false;
			}
		}
	}
}
