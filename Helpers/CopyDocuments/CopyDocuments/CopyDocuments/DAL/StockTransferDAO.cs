using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments.DAL
{
    public class StockTransferDAO
    {
        private B1Connection ConnectionFrom;
        private B1Connection ConnectionTo;

        public StockTransferDAO(B1Connection connectionFrom, B1Connection connectionTo)
        {
            this.ConnectionFrom = connectionFrom;
            this.ConnectionTo = connectionTo;
        }

        public void DoCopy()
        {
            Program.Logger.Info("Buscando Transferências de Estoque");
            Recordset rstDocFrom = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset rstUpdate = (Recordset)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            rstDocFrom.DoQuery("SELECT DocEntry FROM OWTR WHERE ISNULL(U_CVA_Imported, 0) = 0 AND DocDate > CAST('20170301' AS DATETIME) AND DocDate < CAST('20170331' AS DATETIME)");

            Program.Logger.Info("Registros encontrados: " + rstDocFrom.RecordCount);

            while (!rstDocFrom.EoF)
            {
                StockTransfer stockFrom = (StockTransfer)ConnectionFrom.oCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);
                stockFrom.GetByKey((int)rstDocFrom.Fields.Item(0).Value);

                StockTransfer stockTo = (StockTransfer)ConnectionTo.oCompany.GetBusinessObject(BoObjectTypes.oStockTransfer);
                stockTo.CardCode = stockFrom.CardCode;
                stockTo.Address = stockFrom.Address;
                stockTo.DocDate = DateTime.Today;
                stockTo.DueDate = DateTime.Today;
                stockTo.TaxDate = DateTime.Today;

                //stockTo.DocDate = stockFrom.DocDate;
                //stockTo.DueDate = stockFrom.DueDate;
                //stockTo.TaxDate = stockFrom.TaxDate;
                stockTo.FromWarehouse = stockFrom.FromWarehouse;
                stockTo.ToWarehouse = stockFrom.ToWarehouse;
                stockTo.PriceList = stockFrom.PriceList;
                
                //stockTo.Lines.UserFields.Fields.Item("U_CVA_DocEntryFrom").Value = stockFrom.DocEntry;

                for (int i = 0; i < stockFrom.Lines.Count; i++)
                {
                    if (!String.IsNullOrEmpty(stockTo.Lines.ItemCode))
                    {
                        stockTo.Lines.Add();
                    }
                    stockFrom.Lines.SetCurrentLine(i);

                    stockTo.Lines.ItemCode = stockFrom.Lines.ItemCode;
                    stockTo.Lines.Quantity = stockFrom.Lines.Quantity;
                    stockTo.Lines.FromWarehouseCode = stockFrom.Lines.FromWarehouseCode;
                    stockTo.Lines.WarehouseCode = stockFrom.Lines.WarehouseCode;
                    stockTo.Lines.Price = stockFrom.Lines.Price;

                    if (stockFrom.Lines.BatchNumbers.Quantity > 0)
                    {
                        for (int j = 0; j < stockFrom.Lines.BatchNumbers.Count; j++)
                        {
                            if (!String.IsNullOrEmpty(stockTo.Lines.BatchNumbers.BatchNumber))
                            {
                                stockTo.Lines.BatchNumbers.Add();
                            }

                            stockFrom.Lines.BatchNumbers.SetCurrentLine(j);
                            stockTo.Lines.BatchNumbers.BatchNumber = stockFrom.Lines.BatchNumbers.BatchNumber;
                            stockTo.Lines.BatchNumbers.Quantity = stockFrom.Lines.BatchNumbers.Quantity;
                        }
                    }

                    if (stockFrom.Lines.SerialNumbers.Quantity > 0)
                    {
                        for (int j = 0; j < stockFrom.Lines.SerialNumbers.Count; j++)
                        {
                            if (!String.IsNullOrEmpty(stockTo.Lines.SerialNumbers.ManufacturerSerialNumber))
                            {
                                stockTo.Lines.SerialNumbers.Add();
                            }

                            stockFrom.Lines.SerialNumbers.SetCurrentLine(j);
                            stockTo.Lines.SerialNumbers.ManufacturerSerialNumber = stockFrom.Lines.SerialNumbers.ManufacturerSerialNumber;
                            stockTo.Lines.SerialNumbers.Quantity = stockFrom.Lines.SerialNumbers.Quantity;
                        }
                    }
                }

                if (stockTo.Add() != 0)
                {
                    Program.Logger.Info($"Transferência estoque: {stockFrom.DocEntry} - {ConnectionTo.oCompany.GetLastErrorDescription()}");
                }
                else
                {
                    rstUpdate.DoQuery("UPDATE OWTR SET U_CVA_Imported = 1 WHERE DocEntry = " + stockFrom.DocEntry);
                }

                Marshal.ReleaseComObject(stockFrom);
                stockFrom = null;

                Marshal.ReleaseComObject(stockTo);
                stockTo = null;

                rstDocFrom.MoveNext();
            }
        }

    }
}
