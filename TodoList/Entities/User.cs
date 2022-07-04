namespace TodoList.Entities
{
    public class User:EntityBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Project> Projects { get; set; }
        public List<ToDoTask> ToDoTasks { get; set; }
    }
}
