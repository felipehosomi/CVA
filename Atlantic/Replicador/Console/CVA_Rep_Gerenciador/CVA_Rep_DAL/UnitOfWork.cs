using System;

namespace CVA_Rep_DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly CVA_ATL_REPEntities context = new CVA_ATL_REPEntities();
        private GenericRepository<CVA_BAS> cvaBasRepository;
        private GenericRepository<CVA_FUNC> cvaFuncRepository;
        private GenericRepository<CVA_OBJ> cvaObjRepository;
        private GenericRepository<CVA_REG_LOG> cvaRegLogRepository;
        private GenericRepository<CVA_REG> cvaRegRepository;
        private GenericRepository<CVA_STU> cvaStuRepository;
        private GenericRepository<CVA_TIM> cvaTimRepository;
        private GenericRepository<CVA_EML> cvaEmlRepository;

        private bool disposed;

        public GenericRepository<CVA_BAS> CvaBasRepository
            => cvaBasRepository ?? (cvaBasRepository = new GenericRepository<CVA_BAS>(context));

        public GenericRepository<CVA_FUNC> CvaFuncRepository
            => cvaFuncRepository ?? (cvaFuncRepository = new GenericRepository<CVA_FUNC>(context));

        public GenericRepository<CVA_OBJ> CvaObjRepository
            => cvaObjRepository ?? (cvaObjRepository = new GenericRepository<CVA_OBJ>(context));

        public GenericRepository<CVA_REG_LOG> CvaRegLogRepository
            => cvaRegLogRepository ?? (cvaRegLogRepository = new GenericRepository<CVA_REG_LOG>(context));

        public GenericRepository<CVA_REG> CvaRegRepository
            => cvaRegRepository ?? (cvaRegRepository = new GenericRepository<CVA_REG>(context));

        public GenericRepository<CVA_STU> CvaStuRepository
            => cvaStuRepository ?? (cvaStuRepository = new GenericRepository<CVA_STU>(context));

        public GenericRepository<CVA_TIM> CvaTimRepository
            => cvaTimRepository ?? (cvaTimRepository = new GenericRepository<CVA_TIM>(context));

        public GenericRepository<CVA_EML> CvaEmlRepository
            => cvaEmlRepository ?? (cvaEmlRepository = new GenericRepository<CVA_EML>(context));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }
    }
}