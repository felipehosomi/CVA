using Dover.Framework.Form;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using CVA.View.EmailAutorizacao.BLL;

namespace CVA.View.EmailAutorizacao.VIEW.Controller
{
    [Form("142", "CVA.View.EmailAutorizacao.VIEW.Form.EmptyFormPartial.srf")]
    public class PedidoCompraView : DoverSystemFormBase
    {
        private SAPbouiCOM.Application Application { get; set; }
        private PedidoCompraBLL PedidoCompraBLL { get; set; }

        public PedidoCompraView(SAPbouiCOM.Application application, PedidoCompraBLL pedidoCompraBLL)
        {
            Application = application;
            PedidoCompraBLL = pedidoCompraBLL;
        }

        protected override void OnFormActivateAfter(SBOItemEventArg pVal)
        {
            UIAPIRawForm.Title = "Pedido de Compra";
        }

        protected override void OnFormDataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            DBDataSource dts = UIAPIRawForm.DataSources.DBDataSources.Item("OPOR");
            string docTotal = dts.GetValue("DocTotal", dts.Offset).Replace(".", ",");

            string msg = PedidoCompraBLL.SendAuthorizationEmail(Convert.ToInt32(dts.GetValue("DocNum", dts.Offset)), Convert.ToDouble(docTotal));
            if (!String.IsNullOrEmpty(msg))
            {
                Application.MessageBox("Erro ao enviar e-mail de autorização: " + msg);
            }
        }

        protected override void OnFormDataAddAfter(ref BusinessObjectInfo pVal)
        {
            DBDataSource dts = UIAPIRawForm.DataSources.DBDataSources.Item("OPOR");
            string docTotal = dts.GetValue("DocTotal", dts.Offset).Replace(".", ",");
            
            string msg = PedidoCompraBLL.SendAuthorizationEmail(Convert.ToInt32(dts.GetValue("DocNum", dts.Offset)), Convert.ToDouble(docTotal));
            if (!String.IsNullOrEmpty(msg))
            {
                Application.MessageBox("Erro ao enviar e-mail de autorização: " + msg);
            }
        }
    }
}
