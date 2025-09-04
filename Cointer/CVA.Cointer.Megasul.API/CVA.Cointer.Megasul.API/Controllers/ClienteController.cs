using CVA.Cointer.Megasul.API.BLL;
using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using CVA.Cointer.Megasul.API.Models.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CVA.Cointer.Megasul.API.Controllers
{
    public class ClienteController : ApiController
    {
        HanaDAO dao = new HanaDAO();

        public ClienteModel Get(int pagina, int quantidade_registros, string data_de = null, string cnpj_cpf = null, string codigo_sap = null)
        {
            ClienteModel clienteModel = new ClienteModel();
            try
            {
                string sql = Hana.Cliente_Get;
                string where = String.Empty;

                if (!String.IsNullOrEmpty(data_de))
                {
                    DateTime date;
                    if (DateTime.TryParseExact(data_de, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                    {
                        where += $" AND (OCRD.\"CreateDate\" >= '{date.ToString("yyyyMMdd")}' OR OCRD.\"UpdateDate\" >= '{date.ToString("yyyyMMdd")}') ";
                    }
                    else
                    {
                        var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent(string.Format("data_de deve estar no formato dd/mm/aaaa")),
                            ReasonPhrase = "Formato do campo 'data_de' inválido"
                        };
                        throw new HttpResponseException(resp);
                    }
                }
                if (!String.IsNullOrEmpty(cnpj_cpf))
                {
                    where += $" AND (REPLACE(REPLACE(REPLACE(\"TaxId0\", '.', ''), '/', ''), '-', '') = '{cnpj_cpf}' OR REPLACE(REPLACE(\"TaxId4\", '.', ''), '-', '') = '{cnpj_cpf}') ";
                }
                if (!String.IsNullOrEmpty(codigo_sap))
                {
                    where += $" AND OCRD.\"CardCode\" = '{codigo_sap}' ";
                }

                sql = String.Format(sql, where);

                sql += $" limit {quantidade_registros} offset {quantidade_registros * (pagina - 1)} ";

                clienteModel.clientes = dao.FillListFromCommand<Cliente>(sql);
                clienteModel.quantidade_registros = clienteModel.clientes.Count;
                if (clienteModel.clientes.Count > 0)
                {
                    clienteModel.quantidade_registros_total = clienteModel.clientes[0].TotalRecords;
                }
            }
            catch (Exception ex)
            {
                clienteModel.Error = ex.Message;
            }
            
            return clienteModel;
        }

        public ClienteResponseModel Post(ClienteModel clienteModel)
        {
            ClienteResponseModel clienteResponseModel = new ClienteResponseModel();
            ClienteBLL clienteBLL = new ClienteBLL();
            return clienteBLL.Insert(clienteModel);
        }

        [HttpPut]
        public ClienteResponseModel Put(ClienteModel clienteModel)
        {
            ClienteResponseModel clienteResponseModel = new ClienteResponseModel();
            ClienteBLL clienteBLL = new ClienteBLL();
            return clienteBLL.Update(clienteModel);
        }
    }
}
