using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    public class HabiticaClient : HttpClient
    {
        private static HabiticaClient instance;
        private HabiticaSerializer serializer;

        public static HabiticaClient GetInstance()
        {
            if (instance == null)
            {
                instance = new HabiticaClient();
            }

            return instance;
        }

        private HabiticaClient()
        {
            BaseAddress = new Uri("https://habitica.com/api/v3/");

            serializer = HabiticaSerializer.GetInstance();

            SetDefaultHeaders();
            TestConnection();
        }

        private async void TestConnection()
        {
            string url = "status";

            HttpResponseMessage response = await GetAsync(url);

            //Todo: Create a method that checks every response for potential server unavailability
            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Couldn't connect to Habitica Server.");
            }
            response.Dispose();
        }

        private void SetDefaultHeaders()
        {
            if (String.IsNullOrEmpty(Properties.settings.Default.userID)
                || String.IsNullOrEmpty(Properties.settings.Default.apiToken))
            {
                throw new NoCredentialsException();
            }

            DefaultRequestHeaders.Add("x-api-user", Properties.settings.Default.userID);
            DefaultRequestHeaders.Add("x-api-key", Properties.settings.Default.apiToken);
            DefaultRequestHeaders.Add("x-client", Properties.settings.Default.xClient);
        }

        public async Task<HabiticaTodo> CreateNewTodo(string title, string notes)
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

            HttpResponseMessage response = await PostAsync(url, requestContent);

            string json = await response.Content.ReadAsStringAsync();

            return serializer.DeserializeTodo(json);
        }

        public async Task<IList<HabiticaTodo>> GetTodos()
        {
            string url = "tasks/user?type=todos";

            HttpResponseMessage response = await GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return serializer.DeserializeTodos(json);
        }

        public async Task<HabiticaTodo> LoadTodo(HabiticaTodo todo)
        {
            string url = "tasks/" + todo.Id;

            HttpResponseMessage response = await GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return serializer.DeserializeTodo(json);
        }

        public async Task<HabiticaTodo> SaveTodo(HabiticaTodo todo)
        {
            string url = "tasks/" + todo.Id;

            string jsonOut = serializer.SerializeTodo(todo);
            StringContent requestContent = new StringContent(jsonOut, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await PutAsync(url, requestContent);
            string jsonIn = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return serializer.DeserializeTodo(jsonIn);
        }

        public async Task CheckOffTodo(HabiticaTodo todo)
        {
            string url = "tasks/" + todo.Id + "/score/up";

            HttpResponseMessage response = await PostAsync(url, new StringContent(""));
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            serializer.ParseResponseData(json);
        }

        public async Task UncheckTodo(HabiticaTodo todo)
        {
            string url = "tasks/" + todo.Id + "/score/down";

            HttpResponseMessage response = await PostAsync(url, new StringContent(""));
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            serializer.ParseResponseData(json);
        }
    }
}