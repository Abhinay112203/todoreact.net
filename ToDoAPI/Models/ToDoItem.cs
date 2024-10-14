namespace ToDoAPI.Models
{
    public class ToDoItem
    {
        public string Id { get; set; }
        public string shortId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AssginedTo { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int Order { get; set; }

        public string ListId { get; set; }
        public virtual ToDoList ToDoList { get; set; }
        public string StageId { get; set; }
        public virtual Stage Stage { get; set; }
    }
    public class ToDoItemPayload
    {
        public string Name { get; set; }
        public string ListId { get; set; }
        public string? StageId { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
    }

    public class UpdateStagePayload
    {
        public string toDoItemId { get; set; }
        public string stageId { get; set; }
        public int Order { get; set; }
    }
}
