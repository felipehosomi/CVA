using AUXILIAR;
using DAO.Resources;
using MODEL;
using System;

namespace DAO
{
    public class ItemDAO
    {
        //public EtiquetaModel GetEtiqueta(string itemCode, string serie)
        //{
        //    EtiquetaModel etiqueta = new EtiquetaModel();
        //    string sql = String.Empty;
        //    if (String.IsNullOrEmpty(serie))
        //    {
        //        sql = String.Format(SQL.Item_Get, itemCode);
        //    }
        //    else
        //    {
        //        sql = String.Format(SQL.Serie_Get, serie);
        //        if (!String.IsNullOrEmpty(itemCode))
        //        {
        //            sql += " AND ItemCode = '{0}' ";
        //        }
        //    }
        //    etiqueta = new SqlHelper().FillModelAccordingToSql<EtiquetaModel>(sql);

        //    return etiqueta;
        //}
    }
}
