namespace ToDoAPI.Models
{
    public class ListUser
    {
        public string UserId { get; set; }
        public Users User { get; set; }
        public string ToDoListId { get; set; }
        public ToDoList ToDoList { get; set; }
    }
}
