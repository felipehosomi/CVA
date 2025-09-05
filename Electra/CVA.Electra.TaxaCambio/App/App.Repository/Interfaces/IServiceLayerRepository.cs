using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Repository.Interfaces
{

    public interface IServiceLayerRepository<T> : IDisposable where T : class
    {
        T Get(string filter);

        IEnumerable<T> GetAll([Optional]string filter);

        T Add(T entity, bool sendDefaultValues = true);
        void AddWithNoReturn(T entity, bool sendDefaultValues = true); 

        T Edit(T entity, string id, bool sendDefaultValues = true);
        T Edit(T entity, int id, bool sendDefaultValues = true);

        void Cancel(int id);
        void Cancel(string id);

        void Close(int id);
        void Close(string id);

        void Delete(T entity, string id);
        void DeleteAll(string filter, string fieldKeyName);
    }

    
}
