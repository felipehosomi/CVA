
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.DAO
{
    public class CentroCustoDAO
    {
        //private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


     
        public static string BuscaCentroCusto(string ccDominio)
        {
            string cCusto = string.Empty;
            int centroCusto = Convert.ToInt32(ccDominio.Trim());
            try
            {
                SAPbobsCOM.Recordset rst = (SAPbobsCOM.Recordset)SBOApp.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string sql = string.Format((SBOApp.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)? SQL.SQL.CentroCusto_Hana : SQL.SQL.CentroCusto, centroCusto);
                rst.DoQuery(sql);

                if (rst.RecordCount > 0)
                {
                    cCusto = (string)rst.Fields.Item(0).Value;
                }



                return cCusto;

            }
            catch (Exception ex)
            {
                //log.Fatal("Erro Fatal: " + ex.Message + "Data Log: " + DateTime.Now);
                return cCusto;
                throw;
            }

        }
    }
}
