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
        private HabiticaSerializer ser;

        public static HabiticaHttpClient getInstance()
        {
            if(instance == null)
            {
                instance = new HabiticaHttpClient();
            }

            return instance;
        }

        private HabiticaHttpClient()
        {
            BaseAddress = new Uri("https://habitica.com/api/v3/");
            DefaultRequestHeaders.Add("x-api-user", Properties.settings.Default.userID);
            DefaultRequestHeaders.Add("x-api-key", Properties.settings.Default.apiToken);

            ser = new HabiticaSerializer();
        }

        public async void createNewTodo(string title, string notes)
        {
            Uri url = new Uri("tasks/user");

            //TODO: Stringbuilder ersetzen wenn json eingebunden ist
            StringBuilder jsonText = new StringBuilder();
            jsonText.Append("{");
            jsonText.Append("\"text\": \"");
            jsonText.Append(title);
            jsonText.Append("\",");
            jsonText.Append("\"type\": ");
            jsonText.Append("\"todo\"");
            jsonText.Append("}");
            StringContent requestContent = new StringContent(jsonText.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await PostAsync(url,requestContent);

            Stream stream = await response.Content.ReadAsStreamAsync();

            //TODO: parse Json and return result instead of writing to console
            Console.WriteLine(new StreamReader(stream).ReadToEnd());
        }

        public async Task<IList<HabiticaTodo>> getTodos()
        {
            string url = "tasks/user?type=todos";
            
            HttpResponseMessage response = await GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return ser.deserializeTodos(json);
        }
        public async Task<HabiticaTodo> getTodo(string id)
        {
            string url = "tasks/" + id;
            
            HttpResponseMessage response = await GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return ser.deserializeTodo(json);
        }

        public async Task checkOffTodo(string id)
        {
            string url = "tasks/" + id + "/score/up";

            HttpResponseMessage response = await PostAsync(url, new StringContent(""));
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            ser.parseResponseData(json);
        }
        public async Task uncheckTodo(string id)
        {
            string url = "tasks/" + id + "/score/down";

            HttpResponseMessage response = await PostAsync(url, new StringContent(""));
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            ser.parseResponseData(json);
        }
    }
}
