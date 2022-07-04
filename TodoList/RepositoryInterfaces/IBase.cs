namespace TodoList.RepositoryInterfaces
{
    public interface IBase<T>
    {
        string Create(T obj);
        T Retrieve(string key);
        void Update(T obj);
        void Delete(string key);
    }
}
