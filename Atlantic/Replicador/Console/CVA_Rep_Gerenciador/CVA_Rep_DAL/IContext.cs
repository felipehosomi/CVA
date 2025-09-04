using System;
using System.Data.Entity;

namespace CVA_Rep_DAL
{
    public interface IContext<T> : IDisposable where T : class
    {
        DbContext DbContext { get; }
        IDbSet<T> DbSet { get; }
    }
}