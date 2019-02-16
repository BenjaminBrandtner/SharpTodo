namespace HabiticaSimpleToDo
{
    internal class HabiticaChecklistItem
    {
        private readonly string id;
        private string text;
        private bool completed;

        public HabiticaChecklistItem(string id, string text, bool completed)
        {
            this.id = id;
            this.text = text;
            this.completed = completed;
        }

        public string Id => id;
        public string Text { get => text; set => text = value; }
        public bool Completed { get => completed; set => completed = value; }
    }
}