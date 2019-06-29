using Backend;
using System;

namespace ViewModel
{
    public class VMHabiticaTodo
    {
        private readonly HabiticaTodo todo;

        public VMHabiticaTodo(HabiticaTodo todo)
        {
            this.todo = todo;
        }

        public string Title { get => Todo.Title; set => Todo.Title = value; }
        public string Notes { get => Todo.Notes; set => Todo.Notes = value; }
        public bool Completed { get => Todo.Completed; set => Todo.Completed = value; }
        public DateTime? DueDate { get => Todo.DueDate; set => Todo.DueDate = value; }
        public HabiticaTodo Todo { get => todo; }
        public override string ToString()
        {
            return Todo.ToString();
        }
    }
}
