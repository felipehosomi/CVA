using System.Linq;
using CVA_Rep_DAL;

namespace CVA_Rep_BLL
{
    public class CVA_REG_BLL : IBLL<CVA_REG>
    {
        private readonly ICVA_REG_Repository _repository;

        public CVA_REG_BLL()
        {
            _repository = new CVA_REG_Repository();
        }

        public IQueryable<CVA_REG> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_REG GetByName(string name)
        {
            return _repository.GetAll().FirstOrDefault(p => p.CODE.Equals(name));
        }

        public CVA_REG GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_REG toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public bool UpdateUserPassword(CVA_REG toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
            return true;
        }

        public void Update(CVA_REG toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_REG toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }
    }
}