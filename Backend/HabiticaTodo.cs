using System.Collections.Generic;

namespace Backend
{
    public class HabiticaTodo : Todo
    {
        private List<HabiticaChecklistItem> checklist;
        private HabiticaTodo()
        {

        }

        public new List<HabiticaChecklistItem> Checklist { get => checklist; set => checklist = value; }
    }

    internal class Priority
    {
        public const float Trivial = 0.1f;
        public const float Easy = 1;
        public const float Normal = 1.5f;
        public const float Hard = 2;
    }

}
