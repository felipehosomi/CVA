using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Xml;
using EDB_Solution.Controller;

namespace EDB_Solution.Views
{
    [FormAttribute("139", "Views/SaleOrder.b1f")]
    class SaleOrder : SystemFormBase
    {
        public SaleOrder()
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
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataUpdateAfter += new DataUpdateAfterHandler(this.Form_DataUpdateAfter);

        }

        private void OnCustomInitialize()
        {

        }

        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            try
            {
                if (!pVal.ActionSuccess) return;

                Application.SBO_Application.StatusBar.SetText("Realizando a seleção automática dos lotes.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(pVal.ObjectKey);
                // Obtém o número interno (DocEntry) do documento de marketing
                var docEntry = int.Parse(xmlDocument.GetElementsByTagName("DocEntry")[0].InnerXml);
                var document = (SAPbobsCOM.Documents)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                var item = (SAPbobsCOM.Items)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);

                document.GetByKey(docEntry);

                for (var i = 0; i < document.Lines.Count; i++)
                {
                    document.Lines.SetCurrentLine(i);

                    item.GetByKey(document.Lines.ItemCode);

                    if (item.ManageBatchNumbers == SAPbobsCOM.BoYesNoEnum.tYES)
                    {
                        document.Lines.BatchNumbers.BaseLineNumber = document.Lines.LineNum;
                        document.Lines.BatchNumbers.BatchNumber = "1";
                        document.Lines.BatchNumbers.Quantity = document.Lines.Quantity;
                        document.Lines.BatchNumbers.Add();
                    }
                }

                document.Update();

                if (CommonController.Company.GetLastErrorCode() != 0)
                {
                    Application.SBO_Application.StatusBar.SetText(String.Format("Seleção automática dos lotes não ocorrida: {0}", CommonController.Company.GetLastErrorDescription()), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        private void Form_DataUpdateAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            try
            {
                if (!pVal.ActionSuccess) return;

                Application.SBO_Application.StatusBar.SetText("Realizando a seleção automática dos lotes.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(pVal.ObjectKey);
                // Obtém o número interno (DocEntry) do documento de marketing
                var docEntry = int.Parse(xmlDocument.GetElementsByTagName("DocEntry")[0].InnerXml);
                var document = (SAPbobsCOM.Documents)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                var item = (SAPbobsCOM.Items)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);

                document.GetByKey(docEntry);

                for (var i = 0; i < document.Lines.Count; i++)
                {
                    document.Lines.SetCurrentLine(i);

                    item.GetByKey(document.Lines.ItemCode);

                    if (item.ManageBatchNumbers == SAPbobsCOM.BoYesNoEnum.tYES)
                    {
                        document.Lines.BatchNumbers.BaseLineNumber = document.Lines.LineNum;
                        document.Lines.BatchNumbers.BatchNumber = "1";
                        document.Lines.BatchNumbers.Quantity = document.Lines.Quantity;
                        document.Lines.BatchNumbers.Add();
                    }
                }

                document.Update();

                if (CommonController.Company.GetLastErrorCode() != 0)
                {
                    Application.SBO_Application.StatusBar.SetText(String.Format("Seleção automática dos lotes não ocorrida: {0}", CommonController.Company.GetLastErrorDescription()), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
