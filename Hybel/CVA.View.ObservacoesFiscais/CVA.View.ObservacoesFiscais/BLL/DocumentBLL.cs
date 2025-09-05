using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Util;
using CVA.View.ObservacoesFiscais.DAO.Resources;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ObservacoesFiscais.BLL
{
    public class DocumentBLL
    {
        public static string UpdateObs(int docEntry, int objType)
        {
            string table = ObjectTypeUtil.GetDocumentTable((AddOn.Common.Enums.ObjectTypeEnum)objType);
            string sql = String.Format(SQL.Document_GetObservacao, table, docEntry);
            string obs = CrudController.ExecuteScalar(sql).ToString();

            //sql = String.Format(SQL.GetOBSIniciais, docEntry, objType);
            //var sretorno = CrudController.ExecuteScalar(sql);
            //string obsIniciais = string.Empty;
            //if (sretorno!=null)
            //{
            //    obsIniciais = sretorno.ToString();
            //}
            //string obsIniciais =  CrudController.ExecuteScalar(sql).ToString();
            //if (!string.IsNullOrEmpty( obsIniciais ))
            //{
            //    obsIniciais = "Baseado em Pedido: " + obsIniciais;
            //}
            Documents doc = SBOApp.Company.GetBusinessObject((BoObjectTypes)objType) as Documents;
            try
            {
                doc.GetByKey(docEntry);
                doc.ClosingRemarks = obs;
                //if (!string.IsNullOrEmpty(obsIniciais))
                //{
                //    doc.OpeningRemarks = obsIniciais;
                //}
                
                if (doc.Update() != 0)
                {
                    return SBOApp.Company.GetLastErrorDescription();
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(doc);
                doc = null;
            }
        }
    }
}
