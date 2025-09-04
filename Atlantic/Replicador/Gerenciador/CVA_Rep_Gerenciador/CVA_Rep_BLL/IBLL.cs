using System.Linq;

namespace CVA_Rep_BLL
{
    public interface IBLL<T>
    {
        IQueryable<T> GetAll();
        T GetByName(string name);
        T GetById(int id);
        void Add(T toAdd);
        void Update(T toUpdate);
        void Delete(T toDelete);
    }
}