using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace HabiticaSimpleToDo
{
    class Program
    {
        static void Main(string[] args)
        {
            //newTodo();
            getTodos();
        }

        private static void newTodo()
        {
            String url = "https://habitica.com/api/v3/tasks/user";
            String newTodoRequest = "type=todo&text=Success";
            //or
            //String newTodoRequestJson = "{ \"text\": \"json success\", \"type\": \"todo\" }";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["x-api-user"] = Properties.settings.Default.userID;
            request.Headers["x-api-key"] = Properties.settings.Default.apiToken;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //or
            //request.ContentType = "application/json";

            //Write Request Data
            Stream requestStream = request.GetRequestStream();
            StreamWriter writer = new StreamWriter(requestStream, Encoding.UTF8);
            writer.Write(newTodoRequest);
            writer.Close();

            try
            {
                //Send the Request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                printResponseInfo(response);
                parseResponseData(response);

                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("(In Exception) Status: " + e.Status);

                Stream responseStream = e.Response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());
            }
        }

        private static void parseResponseData(HttpWebResponse response)
        {
            //TODO: Parse json instead of just printing it to console.
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            Console.WriteLine(reader.ReadToEnd());
            reader.Close();
        }

        private static void printResponseInfo(HttpWebResponse response)
        {
            //Read some headers from response
            Console.WriteLine("Resonse Status: " + response.StatusCode);
            Console.WriteLine("Response ContentType: " + response.ContentType);
            Console.WriteLine("All Headers:");
            Console.WriteLine(response.Headers);


        }

        private static void getTodos()
        {
            //One Todo
            //String url = "https://habitica.com/api/v3/tasks/9072949e-e3ca-402f-ae3c-e50434117693";

            //All Todos
            String url = "https://habitica.com/api/v3/tasks/user?type=todos/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["x-api-user"] = Properties.settings.Default.userID;
            request.Headers["x-api-key"] = Properties.settings.Default.apiToken;

            try
            {
                //Send the Request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //Read some headers from response
                //Console.WriteLine("Status: " + response.StatusCode);
                //Console.WriteLine("ContentType: " + response.ContentType);

                //Output data
                //TODO: Parse json instead of just printing it to console.
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string json = reader.ReadToEnd();
                response.Close();

                HabiticaSerializer hs = new HabiticaSerializer();
                HabiticaTodo todo = hs.deserializeTodo(json);

                Console.WriteLine(todo);
            }
            catch (WebException)
            {
                throw;
            }
        }

    }
}
