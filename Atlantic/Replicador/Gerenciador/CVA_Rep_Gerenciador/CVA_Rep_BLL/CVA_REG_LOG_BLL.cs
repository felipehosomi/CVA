using System.Linq;
using CVA_Rep_DAL;

namespace CVA_Rep_BLL
{
    public class CVA_REG_LOG_BLL : IBLL<CVA_REG_LOG>
    {
        private readonly ICVA_REG_LOG_Repository _repository;

        public CVA_REG_LOG_BLL()
        {
            _repository = new CVA_REG_LOG_Repository();
        }

        public IQueryable<CVA_REG_LOG> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_REG_LOG GetByName(string name)
        {
            return _repository.GetAll().FirstOrDefault(p => p.MSG.Equals(name));
        }

        public CVA_REG_LOG GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_REG_LOG toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public void Update(CVA_REG_LOG toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_REG_LOG toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }
    }
}