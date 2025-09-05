using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using App.Repository.Interfaces;
using App.Repository.Repositories;
using System.Net;
using System.IO;
using App.Repository.Exception;
using Newtonsoft.Json;
using App.Repository.Generic;

namespace App.ApplicationServices.ServiceLayer
{
    public class UserTableService
    {
        public UserTableService()
        {
        }

        public List<T> GetAll<T>(string filter) where T : class
        {
            try
            {
                var name = typeof(T).Name;
                IServiceLayerRepository<T> _repository = new ServiceLayerRepositories<T>(name);
                return (List<T>)_repository.GetAll(filter);
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void DeleteAll<T>(string filter, string fieldKeyName) where T : class
        {
            try
            {
                var name = typeof(T).Name;
                IServiceLayerRepository<T> _repository = new ServiceLayerRepositories<T>(name);
                _repository.DeleteAll(filter, fieldKeyName);

            }
            catch (Exception)
            {

                throw;
            }
        }

        internal void SaveList<T>(object oList) where T : class
        {
            int qtdError = 0;
            while (true)
            {
                try
                {
                    var name = typeof(T).Name;
                    IServiceLayerRepository<T> _repository = new ServiceLayerRepositories<T>(name);
                    var myList = ((IEnumerable<T>)oList).Cast<object>().ToList();

                    foreach (var myObject in myList)
                    {
                        System.Reflection.PropertyInfo pi = myObject.GetType().GetProperty("Code");
                        string code = (string)(pi.GetValue(myObject, null));
                        bool exist = false;
                        try
                        {
                            exist = _repository.Get($"?$filter = Code eq '{code}'") == null ? false : true;
                        }
                        catch (WebException e)
                        {
                            using (WebResponse response = e.Response)
                            {
                                HttpWebResponse httpResponse = (HttpWebResponse)response;
                                if (httpResponse.StatusCode != HttpStatusCode.NotFound)
                                {
                                    throw;
                                }
                            }
                        }
                        //if (string.IsNullOrEmpty(code))
                        if (!exist)
                        {
                            //code = GetCode(_repository);
                            //pi.SetValue(myObject, code);
                            //pi = myObject.GetType().GetProperty("Name");
                            //pi.SetValue(myObject, code);
                            _repository.Add((T)Convert.ChangeType(myObject, typeof(T)), true);
                        }
                        else
                        {
                            _repository.Edit((T)Convert.ChangeType(myObject, typeof(T)), code, true);
                        }

                    }
                    break;
                }
                catch (WebException e)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    using (WebResponse response = e.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse.StatusCode == HttpStatusCode.BadGateway)
                        {
                            qtdError++;
                            if (qtdError > 10)
                            {
                                throw new Exception(httpResponse.StatusDescription);
                            }
                            continue;
                        }
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), httpResponse.GetEncoding()))
                        {
                            var ret = JsonConvert.DeserializeObject<ErrorServiceLayerException>(reader.ReadToEnd());
                            stringBuilder.Append($"Erro: {httpResponse.StatusCode}");
                            stringBuilder.Append(" Text: " + ret.error.message.value);
                        }
                    }
                    throw new Exception(stringBuilder.ToString());

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private string GetCode<T>(IServiceLayerRepository<T> repository) where T : class
        {
            try
            {
                //não funciona conversão
                //var obj = repository.Get("?$apply=aggregate(Code with max as Code)");

                //if (obj is null)
                //{
                //    return "1";
                //}
                //else
                //{
                //    PropertyInfo pi = obj.GetType().GetProperty("Code");
                //    string code = (string)(pi.GetValue(obj, null));
                //    return Convert.ToString(Convert.ToInt32(code) + 1);
                //}
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetCode<T>() where T : class
        {
            try
            {
                var name = typeof(T).Name;
                IServiceLayerRepository<T> _repository = new ServiceLayerRepositories<T>(name);
                var obj = _repository.Get("?$apply=aggregate(Code with max as Code)");

                if (obj == null)
                {
                    return "1";
                }
                else
                {
                    PropertyInfo pi = obj.GetType().GetProperty("Code");
                    string code = (string)(pi.GetValue(obj, null));
                    return Convert.ToString(Convert.ToInt32(code) + 1);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
