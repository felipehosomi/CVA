using System.Linq;

namespace CVA_Rep_DAL
{
    /// <summary>
    ///     Interface genérica para acesso aos repositórios do EF.
    /// </summary>
    /// <typeparam name="T">Objeto ou tabela do EF</typeparam>
    public interface IGenericDataRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        IQueryable<T> GetAll();
        T Get(object key);
        void SaveChanges();
    }
}