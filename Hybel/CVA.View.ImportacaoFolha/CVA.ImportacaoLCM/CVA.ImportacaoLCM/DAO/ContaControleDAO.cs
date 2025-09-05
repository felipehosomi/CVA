//using log4net;
using CVA.ImportacaoLCM.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.DAO
{
    public class ContaControleDAO
    {
        //private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static List<string> BuscaContaControle(string contaDominio)
        {
            var Log = new LogErro();
            List<string> cControle = new List<string>();
            string sql = string.Empty;

            try
            {

                SAPbobsCOM.Recordset rst = (SAPbobsCOM.Recordset)SBOApp.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                sql = string.Format((SBOApp.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB) ? SQL.SQL.ContaControlePN_Hana: SQL.SQL.ContaControlePN, contaDominio.ToString().Trim());
                rst.DoQuery(sql);

                if (rst.RecordCount > 0)
                {
                    cControle.Add((string)rst.Fields.Item(0).Value);

                    cControle.Add((string)rst.Fields.Item(1).Value);
                }
                else
                {
                    sql = string.Format((SBOApp.oCompany.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB) ?SQL.SQL.ContaContabil_Hana: SQL.SQL.ContaContabil, contaDominio.ToString().Trim());
                    rst.DoQuery(sql);
                    if (rst.RecordCount > 0)
                    {
                        cControle.Add((string)rst.Fields.Item(0).Value);

                    }
                }
                


                return cControle;

            }
            catch (Exception ex)
            {
                //log.Fatal("Erro Fatal: " + ex.Message + "Data Log: " + DateTime.Now);
                return cControle;
                throw;
            }

        }

    }
}
