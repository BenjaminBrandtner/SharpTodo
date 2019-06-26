using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Backend
{
    internal class HabiticaSerializer
    {
        private readonly JsonSerializer serializer;

        public HabiticaSerializer()
        {
            serializer = new JsonSerializer
            {
            };
        }

        internal string SerializeTodo(HabiticaTodo todo)
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

        internal HabiticaTodo DeserializeTodo(string json)
        {
            JToken data = ParseResponseData(json);

            HabiticaTodo habiticaTodo = data.ToObject<HabiticaTodo>(serializer);

            return habiticaTodo;
        }

        internal IList<HabiticaTodo> DeserializeTodos(string json)
        {
            IList<HabiticaTodo> habiticaTodoList = new List<HabiticaTodo>();

            JToken data = ParseResponseData(json);

            foreach (JToken todo in data)
            {
                habiticaTodoList.Add(todo.ToObject<HabiticaTodo>(serializer));
            }

            return habiticaTodoList;
        }

        internal string createBasicTodo(string title)
        {
            JObject todo = new JObject();
            todo.Add("text", JToken.FromObject(title));
            todo.Add("type", JToken.FromObject("todo"));

            return todo.ToString();
        }

        internal JToken ParseResponseData(string json)
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
