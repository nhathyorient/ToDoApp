using TodoList.RepositoryInterfaces;

namespace TodoList.Entities
{
    public class Project:IBase<EntityBase>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public User Owner { get; set; }
        public List<User> AssignedUsers { get; set; }
        public List<ToDoTask> ToDoTasks { get; set; }
        public string Create(EntityBase obj)
        {
            return "";
        }
        public EntityBase Retrieve(string key)
        {
            return new EntityBase();
        }
        public void Update(EntityBase obj)
        {

        }
        public void Delete(string key)
        {

        }
    }
}
