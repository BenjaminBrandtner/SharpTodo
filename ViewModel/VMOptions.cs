using Backend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace ViewModel
{
    public class VMOptions : INotifyPropertyChanged
    {
        private ConfigManager configManager;
        private dynamic config;

        private string errorMsg;
        private ICommand validateCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public VMOptions()
        {
            configManager = new ConfigManager();
            config = configManager.Read();

            ValidateCommand = new UserCommand(new Action<object>(ValidateCredentials));
        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private async void ValidateCredentials(object obj)
        {
            try
            {
                HabiticaClient client = HabiticaClient.GetInstance(config);
                await client.GetTodos(); //TODO: replace with Method TestCredentials()

                ErrorMsg = "Credentials validated successfully";

                configManager.Write(config);
            }
            catch (NoCredentialsException)
            {
                ErrorMsg = "One of the required Fields is empty";
            }
            catch (WrongCredentialsException e)
            {
                ErrorMsg = e.Message;
            }
            catch (UnsuccessfulException e)
            {
                ErrorMsg = e.Message;
            }
        }

        public ICommand ValidateCommand { get => validateCommand; set => validateCommand = value; }
        public string ErrorMsg
        {
            get => errorMsg;
            set
            {
                errorMsg = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ErrorMsg"));
            }
        }
        public string UserID { get => config.UserID; set => config.UserID = value; }
        public string ApiToken { get => config.ApiToken; set => config.ApiToken = value; }
    }
}
