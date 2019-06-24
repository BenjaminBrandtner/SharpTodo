using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HabiticaSimpleToDo;

namespace ViewModel
{
    public class VMHabiticaTodo
    {
        private HabiticaTodo todo;

        public VMHabiticaTodo(HabiticaTodo todo)
        {
            this.todo = todo;
        }

        public string Text { get => todo.Text; set => todo.Text = value; }
        public string Notes { get => todo.Notes; set => todo.Notes = value; }

        public override string ToString()
        {
            return todo.ToString();
        }
    }
}
