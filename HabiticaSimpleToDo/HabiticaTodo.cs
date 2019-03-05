using System;
using System.Collections.Generic;
using System.Text;

namespace HabiticaSimpleToDo
{
    public class HabiticaTodo : Todo
    {
        private HabiticaTodo()
        {

        }
    }

    internal class Priority
    {
        public const float Trivial = 0.1f;
        public const float Easy = 1;
        public const float Normal = 1.5f;
        public const float Hard = 2;
    }

}
