using System.Linq;
using CVA_Rep_DAL;

namespace CVA_Rep_BLL
{
    public class CVA_OBJ_BLL : IBLL<CVA_OBJ>
    {
        private readonly ICVA_OBJ_Repository _repository;

        public CVA_OBJ_BLL()
        {
            _repository = new CVA_OBJ_Repository();
        }

        public IQueryable<CVA_OBJ> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_OBJ GetByName(string name)
        {
            return _repository.GetAll().FirstOrDefault(p => p.OBJ.Equals(name));
        }

        public CVA_OBJ GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_OBJ toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public void Update(CVA_OBJ toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_OBJ toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }
    }
}