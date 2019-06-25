using Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class VMOptions
    {
        private String userID;
        private String apiKey;
        private HabiticaClient client;
        private String errorMsg;
        private ICommand validateCommand;
        public VMOptions()
        {            
            ValidateCommand = new UserCommand(new Action<object>(ValidateCredentials));
        }

        private async void ValidateCredentials(object obj)
        {

            Backend.Properties.settings.Default.userID = UserID;
            Backend.Properties.settings.Default.apiToken = ApiKey;
            Backend.Properties.settings.Default.Save();
            try
            {
               
                client = HabiticaClient.GetInstance();
                IList<HabiticaTodo> temp = await client.GetTodos();
                //if(temp.Count>=1)
                //{
                //    ErrorMsg = "Credentials validated successfully";
                //}
                
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
        public string ErrorMsg { get => errorMsg; set => errorMsg = value; }
        public string UserID { get => userID; set => userID = value; }
        public string ApiKey { get => apiKey; set => apiKey = value; }
    }
}
