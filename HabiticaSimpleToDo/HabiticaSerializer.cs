using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HabiticaSimpleToDo
{
    class HabiticaSerializer
    {
        public HabiticaTodo deserializeTodo(string json)
        {
            JToken data = parseResponseData(json);

            HabiticaTodo todo = data.ToObject<HabiticaTodo>();

            return todo;
        }

        private JToken parseResponseData(string json)
        {
            JObject habiticaResponse = JObject.Parse(json);
            bool success = (bool)habiticaResponse["success"];

            if(!success)
            {
                String dataString = habiticaResponse["Data"].ToString();
                throw new Exception("Request was recieved, but operation was unsuccessful.\n"+dataString);
            }

            return habiticaResponse["data"];
        }
    }
}
