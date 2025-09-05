using AUXILIAR;
using DAO.Resources;
using MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class OrdemDAO
    {
        public List<OrdemProducaoModel> GetOPList(OPFiltroModel model)
        {
            SqlHelper sqlHelper = new SqlHelper();
            string sql = SQL.OP_GetRange;

            if (model.DataDe.HasValue)
            {
                sql += $" AND AUPT.BELDAT >= CAST('{model.DataDe.Value.ToString("yyyyMMdd")}' AS DATETIME) ";
            }
            if (model.DataAte.HasValue)
            {
                sql += $" AND AUPT.BELDAT <= CAST('{model.DataDe.Value.ToString("yyyyMMdd")}' AS DATETIME) ";
            }
            sql += $" AND AUPT.BELNR_ID BETWEEN '{model.OPDe}' AND '{model.OPAte}' ";
            if (!model.Reimpressao)
            {
                sql += $" AND  ISNULL(POS.UDF1, '') <> 'ETIQUETA IMPRESSA' ";
            }

            sql += " ORDER BY AUPT.BELNR_ID ";

            List<OrdemProducaoModel> list = sqlHelper.FillModelListAccordingToSql<OrdemProducaoModel>(sql);
            return list;
        }
    }
}
