using CVA.Core.TransportLCM.BLL;
using CVA.Core.TransportLCM.BLL.BaseConciliadora;
using CVA.Core.TransportLCM.HELPER;
using CVA.Core.TransportLCM.MODEL.BaseConciliadora;
using Dover.Framework.Form;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;

namespace CVA.Core.TransportLCM.VIEW.Controller
{
    [Form(B1Forms.NotaFiscalSaida, "CVA.Core.TransportLCM.VIEW.Form.EmptyFormPartial.srf")]
    public class NotaFiscalSaidaView : DoverSystemFormBase
    {
        private SAPbouiCOM.Application _application { get; set; }
        private InvoiceBLL _InvoiceBLL { get; set; }

        public NotaFiscalSaidaView(SAPbouiCOM.Application application, InvoiceBLL invoiceBLL)
        {
            _application = application;
            _InvoiceBLL = invoiceBLL;
        }

        protected override void OnFormDataAddAfter(ref BusinessObjectInfo pVal)
        {
            //if(pVal.ActionSuccess)
            //    this.UpdateEmailField();
        }

        protected override void OnFormDataUpdateAfter(ref BusinessObjectInfo pVal)
        {
            //if(pVal.ActionSuccess)
            //    this.UpdateEmailField();
        }

        private void UpdateEmailField()
        {
            DBDataSource dts = this.UIAPIRawForm.DataSources.DBDataSources.Item("OINV");
            string email = dts.GetValue("U_EmailEnvDanfe", dts.Offset).Trim();
            if (String.IsNullOrEmpty(email))
            {
                int docEntry = Convert.ToInt32(dts.GetValue("DocEntry", dts.Offset));
                string error = _InvoiceBLL.UpdateEmailField(docEntry);
                if (!String.IsNullOrEmpty(error))
                {
                    _application.MessageBox("Erro ao setar e-mail para envio automático: " + error);
                }
            }
        }
    }
}
