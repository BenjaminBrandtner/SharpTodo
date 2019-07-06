using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class HabiticaClient : HttpClient
    {
        private static HabiticaClient instance;
        private readonly HabiticaSerializer serializer;
        private readonly dynamic config;

        public dynamic Config { get => config; }

        public static HabiticaClient GetInstance()
        {
            if (instance == null)
            {
                instance = new HabiticaClient(new ConfigManager().Read());
            }

            return instance;
        }

        /// <summary>
        /// Creates and returns a new instance with the given config.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static HabiticaClient GetInstance(dynamic config)
        {
            instance = new HabiticaClient(config);

            return instance;
        }

        private HabiticaClient(dynamic config)
        {
            this.config = config;

            SetDefaultHeaders();
            BaseAddress = new Uri("https://habitica.com/api/v3/");
            Timeout = TimeSpan.FromSeconds(20);

            serializer = new HabiticaSerializer();
        }

        private void SetDefaultHeaders()
        {
            if (String.IsNullOrWhiteSpace(config.UserID)
                || String.IsNullOrWhiteSpace(config.ApiToken))
            {
                instance = null;
                throw new NoCredentialsException();
            }

            DefaultRequestHeaders.Add("x-api-user", config.UserID);
            DefaultRequestHeaders.Add("x-api-key", config.ApiToken);
            DefaultRequestHeaders.Add("x-client", Properties.settings.Default.xClient);
        }

        /// <summary>
        /// Test if this client can connect and use the Habitica-API.
        /// Might throw NoCredentialsException, WrongCredentialsException or WebException
        /// </summary>
        public async Task TestConnection()
        {
            string url = "tasks/user?type=todos";

            HttpResponseMessage response = await GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            serializer.ParseResponseData(json);
        }

        public async Task<HabiticaTodo> CreateNewTodo(string title)
        {
            string url = "tasks/user";

            string jsonOut = serializer.createBasicTodo(title);
            StringContent requestContent = new StringContent(jsonOut, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await PostAsync(url, requestContent);
            string jsonIn = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return serializer.DeserializeTodo(jsonIn);
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

        public async Task DeleteTodo(HabiticaTodo todo)
        {
            string url = "tasks/" + todo.Id;

            HttpResponseMessage response = await DeleteAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            response.Dispose();

            serializer.ParseResponseData(json);
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