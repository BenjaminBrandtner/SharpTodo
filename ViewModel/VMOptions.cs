using System;
using System.ComponentModel;
using System.Windows.Input;
using Backend;

namespace ViewModel
{
	public class VMOptions
	{
		private readonly VMMainWindow parentWindow;

		private readonly ConfigManager configManager;
		private readonly dynamic config;

		public VMOptions(VMMainWindow vmMainWindow)
		{
			this.parentWindow = vmMainWindow;

			configManager = new ConfigManager();
			config = configManager.Read();

			ValidateCommand = new UserCommand(ValidateCredentials);
		}

		//Properties
		public VMMainWindow ParentWindow => parentWindow;
		public string UserID { get => config.UserID; set => config.UserID = value; }
		public string ApiToken { get => config.ApiToken; set => config.ApiToken = value; }
		//Events
		public ICommand ValidateCommand { get; set; }

		private async void ValidateCredentials(object obj)
		{
			parentWindow.ErrorMessage = "";
			parentWindow.SuccessMessage = "";
			parentWindow.Busy = true;

			try
			{
				HabiticaClient client = HabiticaClient.GetInstance(config);
				await client.TestConnection();

				parentWindow.SuccessMessage = "Credentials validated successfully";

				configManager.Write(config);
			}
			catch (NoCredentialsException)
			{
				parentWindow.ErrorMessage = "One of the required fields is empty";
			}
			catch (WrongCredentialsException e)
			{
				parentWindow.ErrorMessage = e.Message;
			}
			catch (UnsuccessfulException e)
			{
				parentWindow.ErrorMessage = e.Message;
			}
			finally
			{
				parentWindow.Busy = false;
			}
		}
	}
}
