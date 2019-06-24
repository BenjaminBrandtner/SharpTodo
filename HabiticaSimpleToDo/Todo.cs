using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabiticaSimpleToDo
{
    public abstract class Todo
    {
        private String id;
        private String title;
        private String notes;
        private float value;
        private bool completed;
        private float priority;
        private DateTime date;

        private List<ChecklistItem> checklist;
        private bool collapseChecklist;

        private DateTime createdAt;
        private DateTime updatedAt;

        public string Id { get => id; set => id = value; }
        [JsonProperty("text")]
        public string Title { get => title; set => title = value; }
        [JsonProperty("notes")]
        public string Notes { get => notes; set => notes = value; }
        [JsonProperty("value")]
        public float Value { get => value; set => this.value = value; }
        public bool Completed { get => completed; set => completed = value; }
        [JsonProperty("priority")]
        public float Priority { get => priority; set => priority = value; }
        [JsonProperty("date")]
        public DateTime DueDate { get => date; set => date = value; }
        [JsonProperty("collapseChecklist")]
        public bool CollapseChecklist { get => collapseChecklist; set => collapseChecklist = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
        public List<ChecklistItem> Checklist { get => checklist; set => checklist = value; }

        public override string ToString()
        {
            StringBuilder todoString = new StringBuilder();

            if (completed)
            {
                todoString.Append("[X] ");
            }
            else
            {
                todoString.Append("[ ] ");
            }

            todoString.Append(title);

            if (!(string.IsNullOrWhiteSpace(notes)))
            {
                todoString.Append(" - ");
                todoString.Append(notes);
            }

            return todoString.ToString();
        }
    }
}