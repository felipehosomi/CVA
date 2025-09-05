using CVA.View.CRCP.Model;
using System;

namespace CVA.View.CRCP.BLL
{
    public class CrcpBLL
    {
        public static string GetSql(CrcpFiltroModel model)
        {
            string sql = String.Empty;
            string sproc = String.Empty;
            if (model.TipoRelatorio.StartsWith("P"))
            {
                sproc = "spc_CVA_RelatorioCP";
            }
            else
            {
                sproc = "spc_CVA_RelatorioCR";
            }
            if (String.IsNullOrEmpty(model.CardCode))
            {
                model.CardCode = "NULL";
            }
            else
            {
                model.CardCode = $"'{model.CardCode}'";
            }
            if (String.IsNullOrEmpty(model.NrRefPN))
            {
                model.NrRefPN = "NULL";
            }
            else
            {
                model.NrRefPN = $"'{model.NrRefPN}'";
            }
            if (String.IsNullOrEmpty(model.GrupoPN) || model.GrupoPN == "0")
            {
                model.GrupoPN = "NULL";
            }
            if (String.IsNullOrEmpty(model.StatusCobranca) || model.StatusCobranca == "0")
            {
                model.StatusCobranca = "NULL";
            }

            sql = $@"DECLARE @DataDe DATETIME
                    DECLARE @DataAte DATETIME

                    SET @DataDe = CAST('{model.DataDe.ToString("yyyyMMdd")}' AS DATETIME)
                    SET @DataAte = CAST('{model.DataAte.ToString("yyyyMMdd")}' AS DATETIME)

                    EXEC [{sproc}] '{model.TipoData}', @DataDe, @DataAte, {model.CardCode}, {model.GrupoPN}, {model.NrRefPN}, {model.StatusCobranca}, '{model.Observacoes}', '{model.TipoRelatorio.Substring(1)}'";

            return sql;
        }
    }
}
