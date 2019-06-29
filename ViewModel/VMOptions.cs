using Backend;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ViewModel
{
    public class VMOptions : INotifyPropertyChanged
    {
        private ConfigManager configManager;
        private dynamic config;

        private string errorMessage;
        private string successMessage;
        private ICommand validateCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public VMOptions()
        {
            configManager = new ConfigManager();
            config = configManager.Read();

            ValidateCommand = new UserCommand(new Action<object>(ValidateCredentials));
        }

        public ICommand ValidateCommand { get => validateCommand; set => validateCommand = value; }
        public string UserID { get => config.UserID; set => config.UserID = value; }
        public string ApiToken { get => config.ApiToken; set => config.ApiToken = value; }
        public string ErrorMessage
        {
            get => errorMessage;
            set { errorMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("ErrorMessage")); }
        }
        public string SuccessMessage
        {
            get => successMessage; set
            { successMessage = value; OnPropertyChanged(new PropertyChangedEventArgs("SuccessMessage")); }
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private async void ValidateCredentials(object obj)
        {
            ErrorMessage = "";
            SuccessMessage = "";

            try
            {
                HabiticaClient client = HabiticaClient.GetInstance(config);
                await client.GetTodos(); //TODO: replace with Method TestCredentials()

                SuccessMessage = "Credentials validated successfully";

                configManager.Write(config);
            }
            catch (NoCredentialsException)
            {
                ErrorMessage = "One of the required Fields is empty";
            }
            catch (WrongCredentialsException e)
            {
                ErrorMessage = e.Message;
            }
            catch (UnsuccessfulException e)
            {
                ErrorMessage = e.Message;
            }
        }
    }
}
