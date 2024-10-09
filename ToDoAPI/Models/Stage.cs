namespace ToDoAPI.Models
{
    public class Stage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool isLast { get; set; }
        public bool isFirst { get; set; }
        public bool Deleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public string ListId { get; set; }
        public virtual ToDoList List { get; set; }
    }

    public class StagePayload
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public bool isLast { get; set; }
        public bool isFirst { get; set; }
        public string ListId { get; set; }
    }
}