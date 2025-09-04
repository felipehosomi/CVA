using CVA.View.EmailAutorizacao.MODEL;
using CVA.View.EmailAutorizacao.SERVICE.Resource;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAutorizacao.SERVICE
{
    public class PedidoCompraDAO
    {
        BusinessOneDAO BusinessDAO { get; set; }

        public PedidoCompraDAO(BusinessOneDAO businessOneDAO)
        {
            BusinessDAO = businessOneDAO;
        }

        public List<EmailMessageModel> RetrieveEmailList(int docNum, double docTotal)
        {
            List<EmailMessageModel> emailList = new List<EmailMessageModel>();

            string sql = String.Format(Query.Draft_GetPurchaseOrder, docNum, docTotal.ToString().Replace(",", "."), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmm"), BusinessDAO.GetCurrentUser());
            emailList = BusinessDAO.ExecuteSqlForList<EmailMessageModel>(sql);

            // Se não encontrou tenta voltar 1 minuto, pois pode ter passado
            if (emailList.Count == 0)
            {
                sql = String.Format(Query.Draft_GetPurchaseOrder, docNum, docTotal.ToString().Replace(",", "."), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.AddMinutes(-1).ToString("HHmm"), BusinessDAO.GetCurrentUser());
                emailList = BusinessDAO.ExecuteSqlForList<EmailMessageModel>(sql);
            }

            return emailList;
        }
    }
}
