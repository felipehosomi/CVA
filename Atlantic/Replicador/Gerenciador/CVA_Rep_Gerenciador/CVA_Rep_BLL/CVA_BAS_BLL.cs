using System.Linq;
using CVA_Rep_DAL;

namespace CVA_Rep_BLL
{
    public class CVA_BAS_BLL : IBLL<CVA_BAS>
    {
        private readonly ICVA_BAS_Repository _repository;

        public CVA_BAS_BLL()
        {
            _repository = new CVA_BAS_Repository();
        }

        public IQueryable<CVA_BAS> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_BAS GetByName(string name)
        {
            return _repository.GetAll().FirstOrDefault(p => p.COMP.Equals(name));
        }

        public CVA_BAS GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_BAS toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public void Update(CVA_BAS toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_BAS toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }
    }
}