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
            //getTodosTest();
            //checkOffTodoTest();
            getTodoTest();
        }

        private static void checkOffTodoTest()
        {
            HabiticaHttpClient c = HabiticaHttpClient.getInstance();
            Task checkOffTodo = c.checkOffTodo("05061a0b-9952-4f3e-aa55-b7ccd6d4cc31");

            Console.WriteLine("Warten auf Antwort...");

            checkOffTodo.Wait();
        }

        private static void getTodoTest()
        {
            HabiticaHttpClient c = HabiticaHttpClient.getInstance();
            Task<HabiticaTodo> getTodo = c.getTodo("05061a0b-9952-4f3e-aa55-b7ccd6d4cc31");

            Console.WriteLine("Warten auf Antwort...");

            getTodo.Wait();

            Console.WriteLine(getTodo.Result);
        }

        private static void getTodosTest()
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
