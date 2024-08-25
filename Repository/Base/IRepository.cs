namespace BookReaders.Repository.Base
{
    public interface IRepository<T> where T : class
    {

        IQueryable<T> GetAll();

        void Add(T entity);
        void Update(T entity);
        void DeleteOne(T entity);

        T GetById(int id);
    
    }
}
