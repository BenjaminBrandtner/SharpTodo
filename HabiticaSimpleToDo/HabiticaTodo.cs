using System;
using System.Collections.Generic;

namespace HabiticaSimpleToDo
{
    class HabiticaTodo : ITodo
    {
        private String id;
        private String text;
        private String notes;
        private float value;
        private bool completed;
        private float priority;
        private DateTime date;

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

        public void checkOff()
        {
            completed = true;
        }
        public override String ToString()
        {
            return text + " (" + notes + ")";
        }
    }

    public class Priority
    {
        public const float Trivial = 0.1f;
        public const float Easy = 1;
        public const float Normal = 1.5f;
        public const float Hard = 2;
    }

}
