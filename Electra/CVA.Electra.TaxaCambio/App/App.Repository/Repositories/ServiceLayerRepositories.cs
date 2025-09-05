using App.Repository.Generic;
using App.Repository.Interfaces;
using App.Repository.Model;
using App.Repository.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace App.Repository.Repositories
{
    public enum CallType
    {
        ServiceLayer,
        OData,
        XSJS
    }

    public class ServiceLayerRepositories<TEntity> : IServiceLayerRepository<TEntity> where TEntity : class
    {

        private string _url;
        private CallType _callType;

        public ServiceLayerRepositories(string url, CallType callType = CallType.ServiceLayer)
        {
            this._url = url;
            this._callType = callType;
        }

        TEntity IServiceLayerRepository<TEntity>.Get(string filter)
        {
            try
            {


                ConnectionCacheService.CheckConnection();

                if (_callType == CallType.ServiceLayer)
                {
                    var json = new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<JObject>(_callType);

                    try
                    {
                        foreach (var ret in json.Values())
                            if (ret.Path == "value")
                                if (ret.HasValues && ret.Count() > 0)
                                {
                                    return ret[0].ToObject<TEntity>();
                                }

                        return json.ToObject<TEntity>();
                    }
                    catch
                    {
                        return null;
                    }

                    //if (!string.IsNullOrEmpty(filter) && (filter.StartsWith("?$") || filter.StartsWith("(")))
                    //return new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<TEntity>(_callType);
                }
                else
                {

                    if (_callType == CallType.XSJS)
                    {
                        return new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<TEntity>(_callType);
                    }
                    else
                    {
                        var json = new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<JObject>(_callType);

                        foreach (var d in json.Values())
                            foreach (var ret in d.Values())
                                if (ret.Path == "d.results")
                                    if (ret.HasValues && ret.Count() > 0)
                                    {
                                        return ret[0].ToObject<TEntity>();
                                    }
                    }

                }

                return null;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        IEnumerable<TEntity> IServiceLayerRepository<TEntity>.GetAll([Optional] string filter)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                if (_callType == CallType.ServiceLayer)
                {
                    //if (!string.IsNullOrEmpty(filter) && filter.StartsWith("?$"))
                    //    return new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<IEnumerable<TEntity>>(_callType);

                    var json = new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<JObject>(_callType);

                    try
                    {
                        foreach (var ret in json.Values())
                            if (ret.Path == "value")
                            {
                                if (ret.HasValues && ret.Count() > 0)
                                {
                                    return ret.ToObject<IEnumerable<TEntity>>();
                                }

                            }


                        return json.ToObject<IEnumerable<TEntity>>();
                    }
                    catch
                    {
                        return null;
                    }

                }
                else
                {
                    if (_callType == CallType.XSJS)
                    {
                        return new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<IEnumerable<TEntity>>(_callType);
                    }
                    else
                    {
                        var json = new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<JObject>(_callType);

                        foreach (var d in json.Values())
                            foreach (var ret in d.Values())
                                if (ret.Path == "d.results")
                                    if (ret.HasValues && ret.Count() > 0)
                                    {
                                        return ret.ToObject<IEnumerable<TEntity>>();
                                    }
                    }
                }

                return null;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        TEntity IServiceLayerRepository<TEntity>.Add(TEntity entity, bool sendDefaultValues)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                return new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}").Post<TEntity>(entity, sendDefaultValues);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        void IServiceLayerRepository<TEntity>.AddWithNoReturn(TEntity entity, bool sendDefaultValues)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}").PostWithNoReturn<TEntity>(entity, sendDefaultValues);
            }
            catch (System.Exception)
            {

                throw;
            }
        }




        TEntity IServiceLayerRepository<TEntity>.Edit(TEntity entity, string id, bool sendDefaultValues)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}('{id}')").Patch<TEntity>(entity, sendDefaultValues);
                return entity;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        TEntity IServiceLayerRepository<TEntity>.Edit(TEntity entity, int id, bool sendDefaultValues)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}({id})").Patch<TEntity>(entity, sendDefaultValues);
                return entity;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        void IServiceLayerRepository<TEntity>.Cancel(string id)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}('{id}')/Cancel").Post<TEntity>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        void IServiceLayerRepository<TEntity>.Cancel(int id)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}({id})/Cancel").Post<TEntity>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        void IServiceLayerRepository<TEntity>.Close(string id)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}('{id}')/Close").Post<TEntity>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        void IServiceLayerRepository<TEntity>.Close(int id)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}({id})/Close").Post<TEntity>();
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        void IServiceLayerRepository<TEntity>.Delete(TEntity entity, string id)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}('{id}')").Delete<TEntity>(entity);
            }
            catch (System.Exception)
            {

                throw;
            }

        }

        void IServiceLayerRepository<TEntity>.DeleteAll(string filter, string fieldKeyName)
        {
            try
            {
                ConnectionCacheService.CheckConnection();

                var json = new Uri(string.Format("{0}/{1}{2}", ConnectionCacheService.GetServiceLayerUrl(_callType), _url, filter)).Get<JObject>(_callType);

                foreach (var ret in json.Values())
                    if (ret.Path == "value")
                        if (ret.HasValues && ret.Count() > 0)
                        {
                            var myListObject = ret.ToObject<IEnumerable<TEntity>>();
                            foreach (var myObject in myListObject)
                            {
                                System.Reflection.PropertyInfo pi = myObject.GetType().GetProperty(fieldKeyName);
                                string id = (string)(pi.GetValue(myObject, null));

                                new Uri($"{ConnectionCacheService.GetServiceLayerUrl(_callType)}/{_url}('{id}')").Delete<TEntity>(myObject);

                            }
                        }
                return;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: descartar estado gerenciado (objetos gerenciados).
                }

                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~BasicServices() {
        //   // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
