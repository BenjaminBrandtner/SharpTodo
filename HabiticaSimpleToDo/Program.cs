using System;
using System.IO;
using System.Net;
using System.Text;

namespace HabiticaSimpleToDo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (HabiticaHttpClient c = HabiticaHttpClient.getInstance())
            {
                //c.createNewTodo("Httpclient success", "i did it");
                c.getTodos();

                Console.WriteLine("Press something to continue:");
                Console.ReadKey();
            }
        }
    }
}
