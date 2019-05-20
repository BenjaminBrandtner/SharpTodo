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
        private readonly JsonSerializer serializer;

        public HabiticaSerializer()
        {
            serializer = new JsonSerializer
            {
                //Clearing the DueDate of a todo on the website will set it to null, so we need to specify
                NullValueHandling = NullValueHandling.Ignore
                //and pass the JsonSerializer to all ToObject() Methods.
            };
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

            foreach(JToken todo in data)
            {
                habiticaTodoList.Add(todo.ToObject<HabiticaTodo>(serializer));
            }

            return habiticaTodoList;
        }

        //TODO: Write methods to parse response of Score Task and Score Checklist Item

        public JToken ParseResponseData(string json)
        {
            JObject habiticaResponse = JObject.Parse(json);
            bool success = (bool)habiticaResponse["success"];

            if(!success)
            {
                String message = habiticaResponse["message"].ToString();
                throw new UnsuccessfulException("Habitica recieved the request, but operation was unsuccessful: " + message);
            }

            return habiticaResponse["data"];
        }
    }
}
