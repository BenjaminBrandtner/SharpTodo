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

            ser = new HabiticaSerializer();

            setDefaultHeaders();
        }

        private void setDefaultHeaders()
        {
            //TODO: If UserID/apiKey aren't found, throw NotLoggedIn-Exception
            DefaultRequestHeaders.Add("x-api-user", Properties.settings.Default.userID);
            DefaultRequestHeaders.Add("x-api-key", Properties.settings.Default.apiToken);
        }
        public async Task<HabiticaTodo> createNewTodo(string title, string notes)
        {
            string url = "tasks/user";

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

            string json = await response.Content.ReadAsStringAsync();

            return ser.deserializeTodo(json);
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
