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
            this.Todo = todo;
        }

        public string Text { get => Todo.Text; set => Todo.Text = value; }
        public string Notes { get => Todo.Notes; set => Todo.Notes = value; }
        public HabiticaTodo Todo { get => todo; set => todo = value; }

        public override string ToString()
        {
            return Todo.ToString();
        }
    }
}
