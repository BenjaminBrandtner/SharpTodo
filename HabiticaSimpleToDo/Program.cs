using System;

namespace HabiticaSimpleToDo
{
    class Program
    {
        private static HabiticaTodoCollection todoCollection;

        static void Main(string[] args)
        {
            todoCollection = new HabiticaTodoCollection();
            todoCollection.deserializeAllTodos();
            //todoCollection.create("Success!");

            Console.WriteLine("Working...");
            Console.ReadKey();
        }
    }
}
