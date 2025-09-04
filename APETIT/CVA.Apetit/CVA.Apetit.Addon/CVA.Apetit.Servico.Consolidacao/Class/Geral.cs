using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;

namespace CVA.Apetit.Servico.Consolidacao.Class
{
    public class Geral
    {
        //==================================================================================================================================//
        static public bool PlanejamentoCarregado(string tabela, string code)
        //==================================================================================================================================//
        {
            bool achou = false;
            string sql;
            int cont;

            try
            {
                if (tabela == "@CVA_CAR_OP")
                    sql = string.Format(@"SELECT COUNT(1) FROM ""{0}"" WHERE ""U_ID_PLAN1"" = {1} ", tabela, code);
                else if (tabela == "@CVA_CAR_PLAN")
                    sql = string.Format(@"SELECT COUNT(1) FROM ""{0}"" WHERE ""U_Code"" = {1} ", tabela, code);
                else
                    sql = string.Format(@"SELECT COUNT(1) FROM ""{0}"" WHERE ""Code"" = {1} ", tabela, code);
                cont = Convert.ToInt32(Class.Conexao.ExecuteSqlScalar(sql).ToString());

                if (cont > 0)
                    achou = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return achou;
        }


        //==================================================================================================================================//
        static public int RetornaNextCode(string tabela)
        //==================================================================================================================================//
        {
            int retorno = 0;
            string sql;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"select ifnull(max(""Code""), 0) + 1 AS ""Code"" FROM ""{0}"" ", tabela);
                oRec.DoQuery(sql);
                retorno = Convert.ToInt32(oRec.Fields.Item("Code").Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }

        //==================================================================================================================================//
        static public string RetornaCodDepPadrao(string filial)
        //==================================================================================================================================//
        {
            string sql, cod = "";

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT IFNULL(""DflWhs"", '') AS ""DflWhs"" FROM ""OBPL"" WHERE ""BPLId"" = {0} ", filial);
                oRec2.DoQuery(sql);

                if (oRec2.RecordCount > 0)
                {
                    cod = oRec2.Fields.Item("DflWhs").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cod;
        }

        //==================================================================================================================================//
        static public string RetornaCodFilialCD(string filial)
        //==================================================================================================================================//
        {
            string sql, cod = "";

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"
SELECT IFNULL(""U_BPLId_CD"", '') AS ""U_BPLId_CD""
FROM ""@CVA_CAR_CONFIG""
WHERE ""U_BPLId"" = {0}
", filial);
                oRec2.DoQuery(sql);

                if (oRec2.RecordCount > 0)
                {
                    cod = oRec2.Fields.Item("U_BPLId_CD").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return cod;
        }






























    }
}
