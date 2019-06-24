using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace HabiticaSimpleToDo
{
    class HabiticaSerializer
    {
        private static HabiticaSerializer instance;
        private readonly JsonSerializer serializer;

        public static HabiticaSerializer GetInstance()
        {
            if (instance == null)
            {
                instance = new HabiticaSerializer();
            }

            return instance;
        }

        private HabiticaSerializer()
        {
            serializer = new JsonSerializer
            {
            };
        }

        public String SerializeTodo(HabiticaTodo todo)
        {
            //Remove properties that can't be changed through UpdateTask of the API
            JObject jTodo = JObject.FromObject(todo);
            jTodo["Id"].Parent.Remove();
            jTodo["Completed"].Parent.Remove();
            jTodo["Checklist"].Parent.Remove();
            jTodo["CreatedAt"].Parent.Remove();
            jTodo["UpdatedAt"].Parent.Remove();

            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, jTodo);
                return stringWriter.ToString();
            }
        }

        public HabiticaTodo DeserializeTodo(string json)
        {
            JToken data = ParseResponseData(json);

            HabiticaTodo habiticaTodo = data.ToObject<HabiticaTodo>(serializer);

            return habiticaTodo;
        }

        public IList<HabiticaTodo> DeserializeTodos(string json)
        {
            IList<HabiticaTodo> habiticaTodoList = new List<HabiticaTodo>();

            JToken data = ParseResponseData(json);

            foreach (JToken todo in data)
            {
                habiticaTodoList.Add(todo.ToObject<HabiticaTodo>(serializer));
            }

            return habiticaTodoList;
        }

        //TODO: Write methods to parse response of Score Task and Score Checklist Item

        public JToken ParseResponseData(string json)
        {
            /* The first key of the response is always "success".
             * If the request was successful, the response will contain the key "data"
             * If it was unsuccessful, the response will contain the keys "error" and "message"
             */

            JObject habiticaResponse = JObject.Parse(json);
            bool success = (bool)habiticaResponse["success"];

            if (!success)
            {
                String message = habiticaResponse["message"].ToString();

                if (message.Equals("There is no account that uses those credentials."))
                {
                    throw new WrongCredentialsException(message);
                }
                else
                {
                    throw new UnsuccessfulException("Habitica recieved the request, but operation was unsuccessful: " + message);
                }
            }

            return habiticaResponse["data"];
        }
    }
}
