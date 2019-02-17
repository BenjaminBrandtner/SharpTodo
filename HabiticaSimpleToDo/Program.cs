using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    class Program
    {
        static void Main(string[] args)
        {
            HabiticaHttpClient c = HabiticaHttpClient.getInstance();

            Task<IList<HabiticaTodo>> getTodos = c.getTodos();

            Console.WriteLine("Warten auf Antwort...");

            getTodos.Wait();

            foreach (HabiticaTodo todo in getTodos.Result)
            {
                Console.WriteLine(todo);
            }
        }
    }
}
