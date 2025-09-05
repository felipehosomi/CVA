using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using CVA.AddOn.Hub.Logic.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CVA.Hub.BLL
{
    public class DocumentoBLL
    {
        private CodigoImpostoBLL CodigoImpostoBLL { get; set; }

        public DocumentoBLL()
        {
            CodigoImpostoBLL = new CodigoImpostoBLL();
        }

        public DocumentoModel GetDocEntryAndSerial(int docType, int docNum)
        {
            string sql = Query.Documento_GetDocEntryAndSerial;
            string tableName = String.Empty;
            switch ((BoObjectTypes)docType)
            {
                case BoObjectTypes.oOrders:
                    tableName = "ORDR";
                    break;
                case BoObjectTypes.oDeliveryNotes:
                    tableName = "ODLN";
                    break;
                case BoObjectTypes.oInvoices:
                    tableName = "OINV";
                    break;
                case BoObjectTypes.oCreditNotes:
                    tableName = "ORIN";
                    break;
                case BoObjectTypes.oPurchaseOrders:
                    tableName = "OPOR";
                    break;
                case BoObjectTypes.oPurchaseDeliveryNotes:
                    tableName = "OPDN";
                    break;
                case BoObjectTypes.oPurchaseInvoices:
                    tableName = "OPCH";
                    break;
                case BoObjectTypes.oPurchaseCreditNotes:
                    tableName = "ORPC";
                    break;
            }

            sql = String.Format(sql, tableName, docNum);

            DocumentoModel foundDoc = new CrudController().FillModelAccordingToSql<DocumentoModel>(sql);
            return foundDoc;
        }

        public void UpdateObsPN(int docEntry)
        {
            Documents doc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oOrders);
            try
            {
                doc.GetByKey(docEntry);

                string sqlObsPN = String.Format(Query.ParceiroNegocio_GetObsFaturamento, doc.CardCode);
                string obsPN = CrudController.ExecuteScalar(sqlObsPN).ToString();
                if (!String.IsNullOrEmpty(obsPN))
                {
                    doc.ClosingRemarks += obsPN;
                    if (doc.Update() != 0)
                    {
                        throw new Exception(SBOApp.Company.GetLastErrorDescription());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.ReleaseComObject(doc);
                doc = null;
            }
        }

        public void UpdateObs(int docEntry, int objType)
        {
            List<string> codImpostoList = new List<string>();

            Documents doc = (Documents)SBOApp.Company.GetBusinessObject((BoObjectTypes)objType);
            try
            {
                doc.GetByKey(docEntry);


                for (int i = 0; i < doc.Lines.Count; i++)
                {
                    doc.Lines.SetCurrentLine(i);
                    if (!String.IsNullOrEmpty(doc.Lines.TaxCode) && !codImpostoList.Contains(doc.Lines.TaxCode))
                    {
                        codImpostoList.Add(doc.Lines.TaxCode);
                    }
                }

                List<string> obsNFList = CodigoImpostoBLL.GetObsNF(codImpostoList);
                string openingRemarks = String.Empty;
                foreach (var item in obsNFList)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        if (!openingRemarks.ToLower().Contains(item.ToLower()))
                        {
                            if (!String.IsNullOrEmpty(openingRemarks))
                            {
                                openingRemarks += '\r';
                            }
                            openingRemarks += item;
                        }
                    }
                }

                if (openingRemarks != doc.OpeningRemarks)
                {
                    doc.OpeningRemarks = openingRemarks;
                    if (doc.Update() != 0)
                    {
                        throw new Exception(SBOApp.Company.GetLastErrorDescription());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.ReleaseComObject(doc);
                doc = null;
            }
        }
    }
}
