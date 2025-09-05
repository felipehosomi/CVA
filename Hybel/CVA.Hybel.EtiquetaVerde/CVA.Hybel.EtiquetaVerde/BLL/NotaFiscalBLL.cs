using CVA.Hybel.EtiquetaVerde.DAO;
using CVA.Hybel.EtiquetaVerde.HELPER;
using CVA.Hybel.EtiquetaVerde.MODEL;
using System;
using System.Collections.Generic;

namespace CVA.Hybel.EtiquetaVerde.BLL
{
    public class NotaFiscalBLL
    {
        public static List<ItemModel> GetList(int notaFiscal)
        {
            string sql = String.Format(SQL.Etiqueta_Get, notaFiscal);
            return new SqlHelper().FillModelListAccordingToSql<ItemModel>(sql);
        }
    }
}