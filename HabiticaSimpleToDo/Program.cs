using System;
using System.IO;
using System.Net;

namespace HabiticaSimpleToDo
{
    class Program
    {
        static void Main(string[] args)
        {
            String url = "https://habitica.com/api/v3/tasks/user?type=todos";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["x-api-user"] = Properties.settings.Default.userID;
            request.Headers["x-api-key"] = Properties.settings.Default.apiToken;

            try
            {
                //Send the Request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //Read some headers from response
                Console.WriteLine("Status: " + response.StatusCode);
                Console.WriteLine("ContentType: " + response.ContentType);

                //Output data
                //TODO: Parse json instead of just printing it to console.
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                Console.WriteLine(reader.ReadToEnd());

                response.Close();
            }
            catch (WebException)
            {
                throw;
            }
        }
    }
}
