using Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class VMOptions :INotifyPropertyChanged
    {
        private String userID;
        private String apiKey;
        private HabiticaClient client;
        private String errorMsg;
        private ICommand validateCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public VMOptions()
        {            
            ValidateCommand = new UserCommand(new Action<object>(ValidateCredentials));
        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        private async void ValidateCredentials(object obj)
        {

            Backend.Properties.settings.Default.userID = UserID;
            Backend.Properties.settings.Default.apiToken = ApiKey;
            
            try
            {
               
                client = HabiticaClient.GetInstance();
                IList<HabiticaTodo> temp = await client.GetTodos();
                ErrorMsg = "Credentials validated successfully";
                Backend.Properties.settings.Default.Save();


            }
            catch (NoCredentialsException e)
            {
                ErrorMsg = "One of the required Fields is empty";
            }
            catch(Exception  e) when(e is UnsuccessfulException || e is WrongCredentialsException)
            {
                ErrorMsg = e.Message;
            }


        }

       

       
        public ICommand ValidateCommand { get => validateCommand; set => validateCommand = value; }
        public string ErrorMsg { get => errorMsg;
                                 set
                                 {
                                    errorMsg = value;
                                    OnPropertyChanged(new PropertyChangedEventArgs("ErrorMsg"));
                                 }    
                               }
        public string UserID { get => userID; set => userID = value; }
        public string ApiKey { get => apiKey; set => apiKey = value; }
    }
}
