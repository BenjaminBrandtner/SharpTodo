namespace Backend
{
    public abstract class ChecklistItem
    {
        private readonly string id;
        private string text;
        private bool completed;

        public string Id => id;
        public string Text { get => text; set => text = value; }
        public bool Completed { get => completed; set => completed = value; }
    }
}