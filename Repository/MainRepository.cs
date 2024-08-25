using BookReaders.Data;
using BookReaders.Models;
using BookReaders.Repository.Base;

namespace BookReaders.Repository
{
    public class MainRepository<T> : IRepository<T> where T : class
    {
       
        private readonly AppDbContext _DbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MainRepository(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this._DbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }



        public void Add(T entity)
        { 
            _DbContext.Add<T>(entity);
            _DbContext.SaveChanges();
        }



        public void DeleteOne(T entity)
        {
            _DbContext.Remove<T>(entity);
            _DbContext.SaveChanges();
        }

        public IQueryable<T> GetAll()
        {
            return _DbContext.Set<T>();
        }

        public T GetById(int id)
        {
            return _DbContext.Find<T>(id);
        }

   

        public void Update(T entity)
        {
            _DbContext.Update<T>(entity);
            _DbContext.SaveChanges();
        }
     
    }
}
