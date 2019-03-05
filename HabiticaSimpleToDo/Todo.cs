using System;
using System.Collections.Generic;
using System.Text;

namespace HabiticaSimpleToDo
{
    public abstract class Todo
    {
        private String id;
        private String text;
        private String notes;
        private float value;
        private bool completed;
        private float priority;
        private DateTime date;

        //TODO: Rename text to title and date to dueDate and find out how to deserialize correctly

        private List<HabiticaChecklistItem> checklist;
        private bool collapseChecklist;

        private DateTime createdAt;
        private DateTime updatedAt;

        public string Id { get => id; set => id = value; }
        public string Text { get => text; set => text = value; }
        public string Notes { get => notes; set => notes = value; }
        public float Value { get => value; set => this.value = value; }
        public bool Completed { get => completed; set => completed = value; }
        public float Priority { get => priority; set => priority = value; }
        public DateTime Date { get => date; set => date = value; }
        public bool CollapseChecklist { get => collapseChecklist; set => collapseChecklist = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
        public List<HabiticaChecklistItem> Checklist { get => checklist; set => checklist = value; }

        public override string ToString()
        {
            StringBuilder todoString = new StringBuilder();

            if(completed)
            {
                todoString.Append("[X] ");
            }
            else
            {
                todoString.Append("[ ] ");
            }
                       
            todoString.Append(text);

            if(!(string.IsNullOrWhiteSpace(notes)))
            {
                todoString.Append(" - ");
                todoString.Append(notes);
            }

            return todoString.ToString();
        }
    }
}