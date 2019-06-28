using Backend;
using System;

namespace ViewModel
{
    public class VMHabiticaTodo
    {
        private HabiticaTodo todo;

        public VMHabiticaTodo(HabiticaTodo todo)
        {
            this.Todo = todo;
        }

        public string Text { get => Todo.Title; set => Todo.Title = value; }
        public string Notes { get => Todo.Notes; set => Todo.Notes = value; }

        public Boolean Completed { get => Todo.Completed; set => Todo.Completed = value }

        public DateTime? DueDate { get => Todo.DueDate; set => Todo.DueDate = value; }
        public HabiticaTodo Todo { get => todo; set => todo = value; }

        public override string ToString()
        {
            return Todo.ToString();
        }
    }
}
