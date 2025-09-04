using System.Linq;
using CVA_Rep_DAL;

namespace CVA_Rep_BLL
{
    public class CVA_TIP_REG_BLL : IBLL<CVA_TIP_REG>
    {
        private readonly ICVA_TIP_REG_Repository _repository;

        public CVA_TIP_REG_BLL()
        {
            _repository = new CVA_TIP_REG_Repository();
        }

        public IQueryable<CVA_TIP_REG> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_TIP_REG GetByName(string name)
        {
            return _repository.GetAll().FirstOrDefault(p => p.DSCR.Equals(name));
        }

        public CVA_TIP_REG GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_TIP_REG toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public void Update(CVA_TIP_REG toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_TIP_REG toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }
    }
}