using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using EmailAutorizacao.BLL;
using EmailAutorizacao.HELPER;

namespace EmailAutorizacao.VIEW.Controller
{
    //[Form("142", "CVA.View.EmailAutorizacao.VIEW.Form.EmptyFormPartial.srf")]
    public class PedidoCompraView //: DoverSystemFormBase
    {
        public static void OnFormActivateAfter(Form oForm)
        {
            oForm.Title = "Pedido de Compra";
        }

        //protected override void OnFormDataUpdateAfter(ref BusinessObjectInfo pVal)
        //{
        //    DBDataSource dts = UIAPIRawForm.DataSources.DBDataSources.Item("ORDR");
        //    string docTotal = dts.GetValue("DocTotal", dts.Offset).Replace(".", ",");
        //    string msg = PedidoCompraBLL.SendAuthorizationEmail(Convert.ToInt32(dts.GetValue("DocNum", dts.Offset)), Convert.ToDouble(docTotal));
        //    if (!String.IsNullOrEmpty(msg))
        //    {
        //        Application.MessageBox("Erro ao enviar e-mail de autorização: " + msg);
        //    }
        //}

        public static void OnFormDataAddAfter(Form oForm)
        {
            var dts = oForm.DataSources.DBDataSources.Item("OPOR");
            var docTotal = dts.GetValue("DocTotal", dts.Offset).Replace(".", ",");

            var msg = PedidoCompraBLL.SendAuthorizationEmail(Convert.ToInt32(dts.GetValue("DocNum", dts.Offset)), Convert.ToDouble(docTotal));
            if (!string.IsNullOrEmpty(msg))
            {
                B1Connection.Instance.Application.MessageBox("Erro ao enviar e-mail de autorização: " + msg);
            }
        }
    }
}
