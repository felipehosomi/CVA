using System.Linq;
using CVA_Rep_DAL;
using System.Collections.Generic;

namespace CVA_Rep_BLL
{
    public class CVA_STU_BLL : IBLL<CVA_STU>
    {
        private readonly ICVA_STU_Repository _repository;

        public CVA_STU_BLL()
        {
            _repository = new CVA_STU_Repository();
        }

        public IQueryable<CVA_STU> GetAll()
        {
            return _repository.GetAll();
        }

        public CVA_STU GetByName(string name)
        {
            return _repository.GetAll().FirstOrDefault(p => p.STU.Equals(name));
        }

        public CVA_STU GetById(int id)
        {
            return _repository.GetSingle(id);
        }

        public void Add(CVA_STU toAdd)
        {
            _repository.Add(toAdd);
            _repository.SaveChanges();
        }

        public void Update(CVA_STU toUpdate)
        {
            _repository.Update(toUpdate);
            _repository.SaveChanges();
        }

        public void Delete(CVA_STU toDelete)
        {
            _repository.Remove(toDelete);
            _repository.SaveChanges();
        }

        public Dictionary<string,int> GetDatabaseTypes()
        {
            var bases = new Dictionary<string, int>
            {
                {"MSSQL", 1},
                {"DB2", 2},
                {"SYBASE", 3},
                {"MSSQL 2005", 4},
                {"MAXDB", 5},
                {"MSSQL 2008", 6},
                {"MSSQL 2012", 7},
                {"MSSQL 2014", 8},
                {"HANA DB", 9}
            };

            return bases;
        }
    }
}