using System;
using System.Threading.Tasks;

namespace HabiticaSimpleToDo
{
    class Program
    {
        private static HabiticaTodoCollection todoCollection;

        static void Main(string[] args)
        {
            todoCollection = new HabiticaTodoCollection();
            //Task t = todoCollection.deserializeAllTodos();
            Task t = todoCollection.create("Success!");

            Console.WriteLine("Working...");
            t.Wait();
            Console.WriteLine("Done. Press anything to exit.");
            Console.ReadKey();
        }
    }
}
