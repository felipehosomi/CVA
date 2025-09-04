using System.Linq;
using CVA_Rep_DAL;

namespace CVA_Rep_BLL
{
    public class CVA_FUNC_BLL : IBLL<CVA_FUNC>
    {
        private readonly ICVA_FUNC_Repository _repository;

        public CVA_FUNC_BLL()
        {
            _repository = new CVA_FUNC_Repository();
        }

        public IQueryable<CVA_FUNC> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_FUNC GetByName(string name)
        {
            return _repository.GetAll().FirstOrDefault(p => p.FUNC.Equals(name));
        }

        public CVA_FUNC GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_FUNC toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public void Update(CVA_FUNC toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_FUNC toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }
    }
}