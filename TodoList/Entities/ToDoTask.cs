
namespace TodoList.Entities
{
    public class ToDoTask:EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public User AssignedUser { get; set; }
    }
}
