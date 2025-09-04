using PackIndicator.Controllers;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Xml;

namespace PackIndicator.Views
{
    [FormAttribute("133", "Views/Invoice.b1f")]
    class Invoice : SystemFormBase
    {
        public Invoice()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddAfter += new DataAddAfterHandler(this.Form_DataAddAfter);

        }

        private void OnCustomInitialize()
        {

        }

        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            if (!pVal.ActionSuccess) return;

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(pVal.ObjectKey);
            // Obtém o número interno (DocEntry) do documento de marketing
            var docEntry = int.Parse(xmlDocument.GetElementsByTagName("DocEntry")[0].InnerXml);
            var invoice = (Documents)CommonController.Company.GetBusinessObject(BoObjectTypes.oInvoices);

            invoice.GetByKey(docEntry);
            
            if (!InvoiceController.IsDefaultCustomer(invoice.CardCode)) return;

            var businessPartner = (BusinessPartners)CommonController.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            businessPartner.GetByKey(invoice.CardCode);

            if (String.IsNullOrEmpty(businessPartner.UserFields.Fields.Item("U_CVA_CNPJ").Value.ToString())) return;

            invoice.TaxExtension.TaxId0 = businessPartner.UserFields.Fields.Item("U_CVA_CNPJ").Value.ToString();
            invoice.Update();

            if (CommonController.Company.GetLastErrorCode() != 0)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(String.Format(CommonController.Company.GetLastErrorDescription()), BoMessageTime.bmt_Short);
            }
        }
    }
}
