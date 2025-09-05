using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.CVA_DOC_FRETE;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using CVA.AddOn.Hub.Logic.MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class PedidoCompraBLL
    {
        public string InsereDocsFrete(int docEntry, DataTable table)
        {
            if (table.Rows.Count == 0)
            {
                return String.Empty;
            }

            DocFreteDAO docFreteDAO = new DocFreteDAO();
            FreteBLL freteBLL = new FreteBLL();

            string msg = String.Empty;

            Documents poDoc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oPurchaseOrders);
            
            try
            {
                poDoc.GetByKey(docEntry);
                double docTotal = poDoc.DocTotal;
                List<DocumentItemModel> itemList = new List<DocumentItemModel>();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    // Se já possui o code é porque já existe
                    if (!String.IsNullOrEmpty(table.GetValue("Code", i).ToString().Trim()))
                    {
                        continue;
                    }
                    
                    UserTable userTable = SBOApp.Company.UserTables.Item("CVA_DOC_FRETE");

                    try
                    {
                        string nextCode = (docFreteDAO.GetLastCode() + 1).ToString().PadLeft(10, '0');
                        if (table.GetValue("Tipo Origem", i).ToString() == "")
                        {
                            continue;
                        }

                        userTable.Code = nextCode;
                        userTable.Name = nextCode;
                        userTable.UserFields.Fields.Item("U_PO_DocEntry").Value = docEntry;
                        userTable.UserFields.Fields.Item("U_DocType").Value = table.GetValue("Tipo Origem", i);
                        userTable.UserFields.Fields.Item("U_DocNum").Value = table.GetValue("Chave Origem", i);
                        userTable.UserFields.Fields.Item("U_DocEntry").Value = table.GetValue("DocEntry", i);
                        userTable.UserFields.Fields.Item("U_Serial").Value = table.GetValue("Serial", i);

                        if (!SBOApp.Company.InTransaction)
                        {
                            SBOApp.Company.StartTransaction();
                        }

                        if (userTable.Add() != 0)
                        {
                            throw new Exception(SBOApp.Company.GetLastErrorDescription());
                        }
                        itemList.AddRange(freteBLL.GetItems(Convert.ToInt32(table.GetValue("Tipo Origem", i)), Convert.ToInt32(table.GetValue("Chave Origem", i))));
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(userTable);
                        userTable = null;
                    }
                }

                if (itemList.Any(i => !String.IsNullOrEmpty(i.OcrCode2)))
                {
                    freteBLL.ExecutaRateio(ref poDoc, itemList);

                    double lineTotal = 0;
                    for (int i = 0; i < poDoc.Lines.Count; i++)
                    {
                        poDoc.Lines.SetCurrentLine(i);
                        lineTotal += poDoc.Lines.UnitPrice;
                    }

                    // Arredondamento
                    if (lineTotal != docTotal)
                    {
                        poDoc.Lines.SetCurrentLine(0);
                        poDoc.Lines.UnitPrice = poDoc.Lines.UnitPrice + (docTotal - lineTotal);
                    }
                }

                if (SBOApp.Company.InTransaction)
                {
                    poDoc.UserFields.Fields.Item("U_CVA_Rateio_Frete").Value = "Y";
                    if (poDoc.Update() != 0)
                    {
                        msg = SBOApp.Company.GetLastErrorDescription();
                    }
                    else
                    {
                        SBOApp.Company.EndTransaction(BoWfTransOpt.wf_Commit);
                    }
                }
            }
            catch (Exception ex)
            {
                if (SBOApp.Company.InTransaction)
                {
                    SBOApp.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }

                msg = ex.Message;
            }
            finally
            {
                if (SBOApp.Company.InTransaction)
                {
                    SBOApp.Company.EndTransaction(BoWfTransOpt.wf_Commit);
                }
                Marshal.ReleaseComObject(poDoc);
                poDoc = null;
            }

            return msg;
        }

        public string VerificaExistente(int docType, int docEntry)
        {
            string sql = Query.DocFrete_GetPODocNum;
            sql = String.Format(sql, docType, docEntry);

            object foundDocNum = CrudController.ExecuteScalar(sql);
            if (foundDocNum != null)
            {
                return "Documento já existente no pedido de compra " + foundDocNum.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        public static void SetMotivoCancelamento(int docEntry, string motivoCancelamento)
        {
            CrudController.ExecuteNonQuery(String.Format(Query.PedidoCompra_UpdateMotivoCancelamento, motivoCancelamento, docEntry));
        }
    }
}
