using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    class Program
    {
        static void Main(string[] args)
        {
            HabiticaClient c = HabiticaClient.getInstance();
            Task<IList<HabiticaTodo>> t1 = c.getTodos();

            Console.WriteLine("Working...");
            t1.Wait();
            IList<HabiticaTodo> todolist = t1.Result;
            Console.WriteLine("Done.");

            foreach(HabiticaTodo ht in todolist)
            {
                Console.WriteLine(ht);
            }

            Task<HabiticaTodo> t2 = c.loadTodo(todolist[2]);
            t2.Wait();
            HabiticaTodo ht2 = t2.Result;
            Console.WriteLine(ht2);
        }
    }
}
