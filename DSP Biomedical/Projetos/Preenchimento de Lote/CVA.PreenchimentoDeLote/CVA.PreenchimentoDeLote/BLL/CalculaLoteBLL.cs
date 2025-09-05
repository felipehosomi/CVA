using CVA.PreenchimentoDeLote.DAO;
using CVA.PreenchimentoDeLote.MODEL;
using CVA.PreenchimentoDeLote.VIEW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.PreenchimentoDeLote.BLL
{
    public class CalculaLoteBLL
    {

        #region Atributos

        public CalculaLoteDAO _CalculaLoteDAO { get; set; }
        public CalculaLoteView _CalculaLoteView { get; set; }
        public SAPbobsCOM.Company _company { get; set; }
        public SAPbouiCOM.Application _application { get; set; }

        #endregion

        #region Construtor

        public CalculaLoteBLL()
        {

            _CalculaLoteDAO = new CalculaLoteDAO();
        }

        #endregion

        public NextLote GetNextLote()
        {

            var result = _CalculaLoteDAO.GetNextLote();


            var model = new NextLote()
            {
                code = result.Rows[0]["code"].ToString(),               
            };

            return model;
        }

        //public NewLote GetNewLote(string novolote)
        //{
            

        //    var result = _CalculaLoteDAO.GetNewLote(novolote);


        //    var model = new NewLote()
        //    {
        //        newlote = result.Rows[0]["code"].ToString(),
        //    };

        //    return model;
        //}
    }
}
