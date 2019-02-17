using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HabiticaSimpleToDo
{
    class HabiticaSerializer
    {
        private readonly JsonSerializer ser;

        public HabiticaSerializer()
        {
            ser = new JsonSerializer();

            //Clearing the DueDate of a todo on the website will explicitly set it to null, so we need to specify
            ser.NullValueHandling = NullValueHandling.Ignore;
            //and pass the JsonSerializer to all ToObject() Methods.
        }

        public HabiticaTodo deserializeTodo(string json)
        {
            JToken data = parseResponseData(json);

            HabiticaTodo habiticaTodo = data.ToObject<HabiticaTodo>(ser);

            return habiticaTodo;
        }
        public IList<HabiticaTodo> deserializeTodos(string json)
        {
            IList<HabiticaTodo> habiticaTodoList = new List<HabiticaTodo>();

            JToken data = parseResponseData(json);

            foreach(JToken todo in data)
            {
                habiticaTodoList.Add(todo.ToObject<HabiticaTodo>(ser));
            }

            return habiticaTodoList;
        }

        private JToken parseResponseData(string json)
        {
            JObject habiticaResponse = JObject.Parse(json);
            bool success = (bool)habiticaResponse["success"];

            if(!success)
            {
                String message = habiticaResponse["message"].ToString();
                throw new Exception("Habitica recieved the request, but operation was unsuccessful: " + message);
            }

            return habiticaResponse["data"];
        }
    }
}
