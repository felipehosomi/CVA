using System.Configuration;
using System.Data.Entity;

namespace CVA_Rep_DAL
{
    public class Context<T> : IContext<T> where T : class
    {
        public Context()
        {
            DbContext = new DbContext(ConfigurationManager.ConnectionStrings["CVA_ATL_REPEntities"].ConnectionString);
            DbSet = DbContext.Set<T>();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        public DbContext DbContext { get; }
        public IDbSet<T> DbSet { get; }
    }
}