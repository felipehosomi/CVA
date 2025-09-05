using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments.DAL
{
    public class ProductionOrderDAO
    {
        private B1Connection ConnectionFrom;
        private B1Connection ConnectionTo;

        public ProductionOrderDAO(B1Connection connectionFrom, B1Connection connectionTo)
        {
            this.ConnectionFrom = connectionFrom;
            this.ConnectionTo = connectionTo;
        }

        public void DoCopy()
        {
            Console.WriteLine($"Buscando OP's");
            Recordset rstDocFrom = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset rstUpdate = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            rstDocFrom.DoQuery("SELECT DocEntry FROM OWOR WHERE ISNULL(U_CVA_Imported, 0) = 0 AND PostDate >= '2017-01-01'");

            Console.WriteLine("Registros encontrados: " + rstDocFrom.RecordCount);

            while (!rstDocFrom.EoF)
            {
                ProductionOrders poFrom = (ProductionOrders)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oProductionOrders);
                ProductionOrders poTo = (ProductionOrders)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oProductionOrders);
                string msg = String.Empty;
                try
                {
                    poFrom.GetByKey((int)rstDocFrom.Fields.Item(0).Value);
                    poTo.ItemNo ="123";
                    poTo.PostingDate = poFrom.PostingDate;
                    poTo.DueDate = poFrom.DueDate;
                    poTo.ClosingDate = poFrom.ClosingDate;
                    poTo.PlannedQuantity = poFrom.PlannedQuantity;
                    poTo.Warehouse = poFrom.Warehouse;
                    poTo.ProductionOrderOrigin = poFrom.ProductionOrderOrigin;
                    //poTo.Lines.UserFields.Fields.Item("U_CVA_DocEntryFrom").Value = poFrom.AbsoluteEntry;

                    for (int i = 0; i < poFrom.Lines.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(poTo.Lines.ItemNo))
                        {
                            poTo.Lines.Add();
                        }
                        poFrom.Lines.SetCurrentLine(i);
                        poTo.Lines.ItemNo = poFrom.Lines.ItemNo;
                        //poTo.Lines.ItemType = poFrom.Lines.ItemType;
                        poTo.Lines.BaseQuantity = poFrom.Lines.BaseQuantity;
                        poTo.Lines.PlannedQuantity = poFrom.Lines.PlannedQuantity;
                        poTo.Lines.ProductionOrderIssueType = poFrom.Lines.ProductionOrderIssueType;
                    }
                    ConnectionTo.oCompany.StartTransaction();

                    if (poTo.Add() != 0)
                    {
                        msg = $"Erro ao inserir: " + ConnectionTo.oCompany.GetLastErrorDescription();
                    }
                    else
                    {
                        int poToDocEntry = Convert.ToInt32(ConnectionTo.oCompany.GetNewObjectKey());
                        if (poFrom.ProductionOrderStatus == BoProductionOrderStatusEnum.boposReleased || poFrom.ProductionOrderStatus == BoProductionOrderStatusEnum.boposClosed)
                        {
                            poTo.GetByKey(poToDocEntry);
                            poTo.ProductionOrderStatus = BoProductionOrderStatusEnum.boposReleased;
                            if (poTo.Update() != 0)
                            {
                                msg = $"Erro ao liberar: " + ConnectionTo.oCompany.GetLastErrorDescription();
                            }
                            else
                            {
                                if (poFrom.Lines.ProductionOrderIssueType == BoIssueMethod.im_Manual)
                                {
                                    msg = this.GenerateInventoryExit(poFrom.AbsoluteEntry, poToDocEntry);
                                }
                                if (String.IsNullOrEmpty(msg))
                                {
                                    msg = this.GenerateInventoryEntry(poFrom.AbsoluteEntry, poToDocEntry);
                                }
                            }
                        }
                        if (poFrom.ProductionOrderStatus == BoProductionOrderStatusEnum.boposClosed)
                        {
                            poTo.GetByKey(Convert.ToInt32(ConnectionTo.oCompany.GetNewObjectKey()));
                            poTo.ProductionOrderStatus = BoProductionOrderStatusEnum.boposClosed;
                            if (poTo.Update() != 0)
                            {
                                msg = $"Erro ao fechar: " + ConnectionTo.oCompany.GetLastErrorDescription();
                            }
                        }
                        if (ConnectionTo.oCompany.InTransaction && String.IsNullOrEmpty(msg))
                        {
                            ConnectionTo.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            rstUpdate.DoQuery("UPDATE OWOR SET U_CVA_Imported = 1 WHERE DocEntry = " + poFrom.AbsoluteEntry);
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = "Erro geral: " + ex.Message;
                }
                finally
                {
                    if (!String.IsNullOrEmpty(msg))
                    {
                        msg = $"OP: {poFrom.AbsoluteEntry} - {msg}";
                        Console.WriteLine(msg);
                        msg = String.Empty;
                    }

                    Marshal.ReleaseComObject(poTo);
                    poTo = null;

                    Marshal.ReleaseComObject(poFrom);
                    poFrom = null;

                    rstDocFrom.MoveNext();
                }
            }
        }

        public string GenerateInventoryExit(int docEntryFrom, int docEntryTo)
        {
            Documents invGenExitFrom = (Documents)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenExit);
            Documents invGenExitTo = (Documents)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenExit);

            Recordset rst = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            rst.DoQuery("SELECT DocEntry FROM IGE1 WHERE BaseType = 202 AND BaseEntry = " + docEntryFrom);

            if (rst.RecordCount > 0)
            {
                try
                {
                    invGenExitFrom.GetByKey((int)rst.Fields.Item(0).Value);
                    invGenExitTo.DocDate = invGenExitFrom.DocDate;
                    invGenExitTo.BPL_IDAssignedToInvoice = invGenExitFrom.BPL_IDAssignedToInvoice;

                    invGenExitTo.DocType = BoDocumentTypes.dDocument_Items;

                    for (int i = 0; i < invGenExitFrom.Lines.Count; i++)
                    {
                        if (invGenExitTo.Lines.BaseEntry != 0)
                        {
                            invGenExitTo.Lines.Add();
                        }
                        invGenExitFrom.Lines.SetCurrentLine(i);
                        invGenExitTo.Lines.BaseType = (int)BoObjectTypes.oProductionOrders;
                        invGenExitTo.Lines.BaseEntry = docEntryTo;
                        invGenExitTo.Lines.BaseLine = invGenExitFrom.Lines.BaseLine;
                        invGenExitTo.Lines.WarehouseCode = invGenExitFrom.Lines.WarehouseCode;
                        invGenExitTo.Lines.Quantity = invGenExitFrom.Lines.Quantity;

                        if (invGenExitFrom.Lines.BatchNumbers.Quantity > 0)
                        {
                            for (int j = 0; j < invGenExitFrom.Lines.BatchNumbers.Count; j++)
                            {
                                if (!String.IsNullOrEmpty(invGenExitTo.Lines.BatchNumbers.BatchNumber))
                                {
                                    invGenExitTo.Lines.BatchNumbers.Add();
                                }

                                invGenExitFrom.Lines.BatchNumbers.SetCurrentLine(j);
                                invGenExitTo.Lines.BatchNumbers.BatchNumber = invGenExitFrom.Lines.BatchNumbers.BatchNumber;
                                invGenExitTo.Lines.BatchNumbers.Quantity = invGenExitFrom.Lines.BatchNumbers.Quantity;
                            }
                        }

                        if (invGenExitFrom.Lines.SerialNumbers.Quantity > 0)
                        {
                            for (int j = 0; j < invGenExitFrom.Lines.SerialNumbers.Count; j++)
                            {
                                if (!String.IsNullOrEmpty(invGenExitTo.Lines.SerialNumbers.InternalSerialNumber))
                                {
                                    invGenExitTo.Lines.SerialNumbers.Add();
                                }

                                invGenExitFrom.Lines.SerialNumbers.SetCurrentLine(j);
                                invGenExitTo.Lines.SerialNumbers.InternalSerialNumber = invGenExitFrom.Lines.SerialNumbers.InternalSerialNumber;
                                invGenExitTo.Lines.SerialNumbers.Quantity = invGenExitFrom.Lines.SerialNumbers.Quantity;
                            }
                        }
                    }

                    if (invGenExitTo.Add() != 0)
                    {
                        return "Erro ao gerar saída de insumos: " + ConnectionTo.oCompany.GetLastErrorDescription();
                    }
                }
                catch (Exception e)
                {
                    return "Erro geral ao gerar saída de insumos: " + e.Message;
                }
                finally
                {
                    Marshal.ReleaseComObject(invGenExitFrom);
                    Marshal.ReleaseComObject(invGenExitTo);
                    Marshal.ReleaseComObject(rst);

                    invGenExitFrom = null;
                    invGenExitTo = null;
                    rst = null;
                }
            }
            return String.Empty;
        }

        public string GenerateInventoryEntry(int docEntryFrom, int docEntryTo)
        {
            Documents invGenEntrFrom = (Documents)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);
            Documents invGenEntryTo = (Documents)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oInventoryGenEntry);

            Recordset rst = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            rst.DoQuery("SELECT DocEntry FROM IGN1 WHERE BaseType = 202 AND BaseEntry = " + docEntryFrom);
            if (rst.RecordCount > 0)
            {
                invGenEntrFrom.GetByKey((int)rst.Fields.Item(0).Value);
                invGenEntryTo.DocDate = invGenEntrFrom.DocDate;
                invGenEntryTo.BPL_IDAssignedToInvoice = invGenEntrFrom.BPL_IDAssignedToInvoice;

                invGenEntryTo.DocType = BoDocumentTypes.dDocument_Items;

                for (int i = 0; i < invGenEntrFrom.Lines.Count; i++)
                {
                    if (invGenEntryTo.Lines.BaseEntry != 0)
                    {
                        invGenEntryTo.Lines.Add();
                    }

                    invGenEntryTo.Lines.BaseType = (int)BoObjectTypes.oProductionOrders;
                    invGenEntryTo.Lines.BaseEntry = docEntryTo;
                    invGenEntryTo.Lines.BaseLine = invGenEntrFrom.Lines.BaseLine;
                    invGenEntryTo.Lines.WarehouseCode = invGenEntrFrom.Lines.WarehouseCode;
                    invGenEntryTo.Lines.Quantity = invGenEntrFrom.Lines.Quantity;

                    if (invGenEntrFrom.Lines.BatchNumbers.Quantity > 0)
                    {
                        for (int j = 0; j < invGenEntrFrom.Lines.BatchNumbers.Count; j++)
                        {
                            if (!String.IsNullOrEmpty(invGenEntryTo.Lines.BatchNumbers.BatchNumber))
                            {
                                invGenEntryTo.Lines.BatchNumbers.Add();
                            }

                            invGenEntrFrom.Lines.BatchNumbers.SetCurrentLine(j);
                            invGenEntryTo.Lines.BatchNumbers.BatchNumber = invGenEntrFrom.Lines.BatchNumbers.BatchNumber;
                            invGenEntryTo.Lines.BatchNumbers.Quantity = invGenEntrFrom.Lines.BatchNumbers.Quantity;
                        }
                    }

                    if (invGenEntrFrom.Lines.SerialNumbers.Quantity > 0)
                    {
                        for (int j = 0; j < invGenEntrFrom.Lines.SerialNumbers.Count; j++)
                        {
                            if (!String.IsNullOrEmpty(invGenEntryTo.Lines.SerialNumbers.InternalSerialNumber))
                            {
                                invGenEntryTo.Lines.SerialNumbers.Add();
                            }

                            invGenEntrFrom.Lines.SerialNumbers.SetCurrentLine(j);
                            invGenEntryTo.Lines.SerialNumbers.InternalSerialNumber = invGenEntrFrom.Lines.SerialNumbers.InternalSerialNumber;
                            invGenEntryTo.Lines.SerialNumbers.Quantity = invGenEntrFrom.Lines.SerialNumbers.Quantity;
                        }
                    }
                }

                if (invGenEntryTo.Add() != 0)
                {
                    return "Erro ao gerar entrada de produto acabado: " + ConnectionTo.oCompany.GetLastErrorDescription();
                }
            }
            return String.Empty;
        }
    }
}
