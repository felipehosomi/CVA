using System.Data.Entity;
using System.Linq;

namespace CVA_Rep_DAL
{
    /// <summary>
    ///     Repositório genérico do EF.
    /// </summary>
    /// <typeparam name="T">Objeto ou tabela do EF.</typeparam>
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        private readonly IContext<T> _context;

        public GenericDataRepository()
        {
            _context = new Context<T>();
        }

        public GenericDataRepository(IContext<T> context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.DbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            _context.DbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            _context.DbSet.Attach(entity);
            _context.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> GetAll()
        {
            return _context.DbSet;
        }

        public T Get(object key)
        {
            return _context.DbSet.Find(key);
        }

        public void SaveChanges()
        {
            _context.DbContext.SaveChanges();
        }
    }
}