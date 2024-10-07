namespace ToDoAPI.Models
{
    public class ToDoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? AssginedTo {  get; set; }
        public string CreatedBy {  get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDateTime {  get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
