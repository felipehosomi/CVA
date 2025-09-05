using AUXILIAR;
using DAO.Resources;
using MODEL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DAO
{
    public class EtiquetaDAO
    {
        public List<EtiquetaModel> GetEtiquetaList(int ordem)
        {
            string sql = SQL.Etiqueta_Get;
            sql += " WHERE FTHAUPT.BELNR_ID = " + ordem;
            List<EtiquetaModel> list = new SqlHelper().FillModelListAccordingToSql<EtiquetaModel>(sql);
            return list;
        }

        public EtiquetaModel GetEtiqueta(string itemCode, string serie)
        {
            string sql;
            if (!String.IsNullOrEmpty(serie) && String.IsNullOrEmpty(itemCode))
            {
                sql = SQL.Etiqueta_Get;
                sql += String.Format(" WHERE RESERVIERUNG.ITEM = '{0}' ", serie);

                List<EtiquetaModel> list = new SqlHelper().FillModelListAccordingToSql<EtiquetaModel>(sql);
                if (list.Count == 0)
                {
                    sql = String.Format(SQL.ItemSerie_Get, itemCode, serie);
                    list = new SqlHelper().FillModelListAccordingToSql<EtiquetaModel>(sql);
                }
                if (list.Count == 0)
                {
                    return new EtiquetaModel();
                }
                return list[0];
            }
            else
            {
                sql = String.Format(SQL.Item_Get, itemCode);
                EtiquetaModel etiqueta = new SqlHelper().FillModelAccordingToSql<EtiquetaModel>(sql);
                return etiqueta;
            }
        }
    }
}
