using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace HabiticaSimpleToDo
{
    class HabiticaHttpClient : HttpClient
    {
        private static HabiticaHttpClient instance;

        public static HabiticaHttpClient getInstance()
        {
            if(instance == null)
            {
                instance = new HabiticaHttpClient();
            }

            return instance;
        }

        HabiticaHttpClient()
        {
            BaseAddress = new Uri("https://habitica.com/api/v3/");
            DefaultRequestHeaders.Add("x-api-user", Properties.settings.Default.userID);
            DefaultRequestHeaders.Add("x-api-key", Properties.settings.Default.apiToken);
        }

        public void createNewTodo(String Title, String Description)
        {
            StringBuilder jsonText = new StringBuilder();
            jsonText.Append("{");
            jsonText.Append("\"text\": \"");
            jsonText.Append(Title);
            jsonText.Append("\",");
            jsonText.Append("\"type\": ");
            jsonText.Append("\"todo\"");
            jsonText.Append("}");

            Console.WriteLine(jsonText.ToString());

            Task<HttpResponseMessage> t = PostAsync("tasks/user", new StringContent(jsonText.ToString(), Encoding.UTF8, "application/json"));

            Console.WriteLine("Press something to continue:");
            Console.ReadKey();

            HttpResponseMessage result = t.Result;
            Task<Stream> t2 = result.Content.ReadAsStreamAsync();

            Console.WriteLine("Press something to continue:");
            Console.ReadKey();

            Stream s = t2.Result;

            StreamReader sr = new StreamReader(s);
            Console.WriteLine(sr.ReadToEnd());
        }

    }
}
