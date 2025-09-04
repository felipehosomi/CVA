
using CVA.Portal.Producao.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.Web.Util
{
    public class APICallUtil
    {
        string ApiUrl = System.Configuration.ConfigurationManager.AppSettings["WebApiURL"];

        /// <summary>
        /// Retorna o model de acordo com o parâmetro passado
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="param">Parâmetro(s) de busca do model</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string controller, string param, string paramSeparator = "/") where T : class
        {
            string JsonResult = "";
            try
            {
                string url = string.Format(ApiUrl + "/{0}{1}{2}", controller, paramSeparator, param);

                var client = new System.Net.Http.HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(url);

                JsonResult = response.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<T>(JsonResult);

                return rootobject;
            }
            catch (Exception e)
            {
                try
                {
                    if (!String.IsNullOrEmpty(JsonResult))
                    {
                        ErrorModel errorModel = JsonConvert.DeserializeObject<ErrorModel>(JsonResult);
                        throw new Exception(errorModel.Message, new Exception(errorModel.ExceptionMessage));
                    }
                    else throw e;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        
        /// <summary>
        /// Retorna uma lista de model de acordo com o parâmetro passado
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="param">Parâmetro(s) de busca do model</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<T>(string controller, string param = "") where T : class
        {
            string JsonResult = "";
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string url = string.Format(ApiUrl + "/{0}?type=json&{1}", controller, param);
                var response = await client.GetAsync(url);
                JsonResult = response.Content.ReadAsStringAsync().Result;
                var rootobject = JsonConvert.DeserializeObject<List<T>>(JsonResult);

                return rootobject;
            }
            catch (Exception e)
            {
                try
                {
                    if (!String.IsNullOrEmpty(JsonResult))
                    {
                        ErrorModel errorModel = JsonConvert.DeserializeObject<ErrorModel>(JsonResult);
                        throw new Exception(errorModel.Message, new Exception(errorModel.ExceptionMessage));
                    }
                    else throw e;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Atualiza o model
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="param">Parâmetros de busca do model (chaves)</param>
        /// <param name="item">Model a ser atualizado</param>
        /// <returns></returns>
        public async Task<string> PutAsync<T>(string controller, string param, T item) where T : class
        {
            var client = new HttpClient();

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(string.Format(ApiUrl + "/{0}?{1}", controller, param), content);
            if (response.IsSuccessStatusCode)
            {
                return String.Empty;
            }
            else
            {
                string retorno = await response.Content.ReadAsStringAsync();
                try
                {
                    var rootobject = JsonConvert.DeserializeObject<JsonModel>(retorno);
                    return rootobject.ExceptionMessage;
                }
                catch (Exception ex)
                {
                    return retorno;
                }
            }
        }

        /////<summary>
        /////Atualiza o model parcialmente
        /////</summary>
        //public async Task<string> PatchAsync<T>(string controller, string param, T item) where T : class
        //{
        //    var client = new HttpClient();

        //    var json = JsonConvert.SerializeObject(item);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //    HttpResponseMessage response = await client.pa
        //}

        /// <summary>
        /// Inclui model
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="item">Model a ser incluido</param>
        /// <returns></returns>
        public async Task<string> PostAsync<T>(string controller, T item) where T : class
        {
            var client = new System.Net.Http.HttpClient();

            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(string.Format(ApiUrl + "/{0}", controller), content);
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var rootobject = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());

                return rootobject.Message;
            }
        }

        /// <summary>
        /// Inclui model
        /// </summary>
        /// <typeparam name="T">Tipo do model</typeparam>
        /// <param name="controller">Nome do controller da web api</param>
        /// <param name="item">Model a ser incluido</param>
        /// <returns></returns>
        public async Task<string> PostWithFileAsync<T>(string controller, T item) where T : class
        {
            var client = new System.Net.Http.HttpClient();

            var json = JsonConvert.SerializeObject(item, new MemoryStreamJsonConverter());
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(string.Format(ApiUrl + "/{0}", controller), content);
            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var rootobject = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                return rootobject.ExceptionMessage;
            }
        }

        public async Task<string> DeleteAsync(string controller, string param)
        {
            string url = string.Format(ApiUrl + "/{0}/{1}", controller, param);

            var client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return "";
            }
            else
            {
                var rootobject = JsonConvert.DeserializeObject<JsonModel>(await response.Content.ReadAsStringAsync());
                return rootobject.ExceptionMessage;
            }
        }
    }
}