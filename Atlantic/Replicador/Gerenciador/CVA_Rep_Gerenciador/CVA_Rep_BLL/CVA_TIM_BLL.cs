using System.Linq;
using CVA_Rep_DAL;

namespace CVA_Rep_BLL
{
    public class CVA_TIM_BLL : IBLL<CVA_TIM>
    {
        private readonly ICVA_TIM_Repository _repository;

        public CVA_TIM_BLL()
        {
            _repository = new CVA_TIM_Repository();
        }

        public IQueryable<CVA_TIM> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_TIM GetByName(string name)
        {
            return null;
        }

        public CVA_TIM GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_TIM toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public void Update(CVA_TIM toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_TIM toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }
    }
}