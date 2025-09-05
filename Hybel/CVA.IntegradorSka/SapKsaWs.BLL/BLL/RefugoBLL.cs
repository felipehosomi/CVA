using CVA.AddOn.Common;
using log4net;
using SAPbobsCOM;
using SapKsaWs.BLL.HELPER;
using SapKsaWs.BLL.MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SapKsaWs.BLL
{
    public class RefugoBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RefugoBLL()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public string GeraRefugo(SSPExportProdLogModel model)
        {
            Documents entradaMercadoria = SBOApp.Company.GetBusinessObject(BoObjectTypes.oInventoryGenEntry) as Documents;

            try
            {
                entradaMercadoria.DocDate = DateTime.Today;
                entradaMercadoria.BPL_IDAssignedToInvoice = 1;
                entradaMercadoria.Comments = $"Refugo - OP: {model.OP.Trim()}, BEL_POSID: {model.BelPosId.Trim()}, POS_ID: {model.PosId.Trim()}";
                entradaMercadoria.DocType = BoDocumentTypes.dDocument_Items;
                entradaMercadoria.Lines.ItemCode = model.ItemCode.Trim();
                entradaMercadoria.Lines.Quantity = model.Refugo;

                SBOApp.Company.StartTransaction();

                if (entradaMercadoria.Add() != 0)
                {
                    return "Erro ao gerar entrada do refugo: " + SBOApp.Company.GetLastErrorDescription();
                }
                else
                {
                    model.DocEntryEntrada = Convert.ToInt32(SBOApp.Company.GetNewObjectKey());
                    string msg = this.GeraTransferencia(model);
                    if (!String.IsNullOrEmpty(msg))
                    {
                        return msg;
                    }
                }
            }
            catch (Exception ex)
            {
                if (SBOApp.Company.InTransaction)
                {
                    SBOApp.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                return "Erro geral ao gerar refugo: " + ex.Message;
            }
            finally
            {
                if (SBOApp.Company.InTransaction)
                {
                    SBOApp.Company.EndTransaction(BoWfTransOpt.wf_Commit);
                }

                Marshal.ReleaseComObject(entradaMercadoria);
                entradaMercadoria = null;
            }

            return String.Empty;
        }

        public string GeraTransferencia(SSPExportProdLogModel model)
        {
            Documents entradaMercadoria = SBOApp.Company.GetBusinessObject(BoObjectTypes.oInventoryGenEntry) as Documents;
            StockTransfer transferencia = SBOApp.Company.GetBusinessObject(BoObjectTypes.oStockTransfer) as StockTransfer;

            try
            {
                entradaMercadoria.GetByKey(model.DocEntryEntrada);

                transferencia.DocDate = DateTime.Today;
                transferencia.FromWarehouse = entradaMercadoria.Lines.WarehouseCode;
                transferencia.ToWarehouse = ConfigurationManager.AppSettings["DepositoRefugo"];
                transferencia.Comments = $"Refugo - OP: {model.OP.Trim()}, BEL_POSID: {model.BelPosId.Trim()}, POS_ID: {model.PosId.Trim()}";

                transferencia.Lines.ItemCode = entradaMercadoria.Lines.ItemCode.Trim();
                transferencia.Lines.Quantity = model.Refugo;

                if (!String.IsNullOrEmpty(entradaMercadoria.Lines.BatchNumbers.BatchNumber))
                {
                    for (int i = 0; i < entradaMercadoria.Lines.BatchNumbers.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(transferencia.Lines.BatchNumbers.BatchNumber))
                        {
                            transferencia.Lines.BatchNumbers.Add();
                        }

                        transferencia.Lines.BatchNumbers.BatchNumber = entradaMercadoria.Lines.BatchNumbers.BatchNumber;
                        if (model.Refugo > entradaMercadoria.Lines.BatchNumbers.Quantity)
                        {
                            transferencia.Lines.BatchNumbers.Quantity = entradaMercadoria.Lines.BatchNumbers.Quantity;
                            model.Refugo -= (int)entradaMercadoria.Lines.BatchNumbers.Quantity;
                        }
                        else
                        {
                            transferencia.Lines.BatchNumbers.Quantity = model.Refugo;
                            model.Refugo = 0;
                        }
                        if (model.Refugo == 0)
                        {
                            break;
                        }
                    }
                }

                if (!String.IsNullOrEmpty(entradaMercadoria.Lines.SerialNumbers.InternalSerialNumber))
                {
                    for (int i = 0; i < entradaMercadoria.Lines.SerialNumbers.Count; i++)
                    {
                        entradaMercadoria.Lines.SerialNumbers.SetCurrentLine(i);
                        if (!String.IsNullOrEmpty(transferencia.Lines.SerialNumbers.InternalSerialNumber))
                        {
                            transferencia.Lines.SerialNumbers.Add();
                        }
                        transferencia.Lines.SerialNumbers.InternalSerialNumber = entradaMercadoria.Lines.SerialNumbers.InternalSerialNumber;
                        transferencia.Lines.SerialNumbers.Quantity = 1;
                        model.Refugo--;
                        if (model.Refugo == 0)
                        {
                            break;
                        }
                    }
                }

                if (transferencia.Add() != 0)
                {
                    return "Erro ao gerar transferência do refugo: " + SBOApp.Company.GetLastErrorDescription();
                }
            }
            catch (Exception ex)
            {
                if (SBOApp.Company.InTransaction)
                {
                    SBOApp.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                return "Erro ao geral ao transferir refugo: " + ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(entradaMercadoria);
                Marshal.ReleaseComObject(transferencia);

                entradaMercadoria = null;
                transferencia = null;
            }
            return String.Empty;
        }
    }
}
