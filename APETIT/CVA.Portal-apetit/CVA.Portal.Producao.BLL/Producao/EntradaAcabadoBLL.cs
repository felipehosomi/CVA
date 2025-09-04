using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model.Producao;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Producao
{
    public class EntradaAcabadoBLL : BaseBLL
    {
        public async Task<string> GeraEntradaAcabado(ProducaoModel model)
        {
           if(Static.Config.ServerType == BoDataServerTypes.dst_HANADB)
            {
                return await this.GeraEntradaAcabadoSL(model);
            }
           else
            {
                return await this.GeraEntradaAcabadoDI(model);
            }
        }

        public async Task<string> GeraEntradaAcabadoSL(ProducaoModel model)
        {
            DocumentoMarketingModel entradaModel = new DocumentoMarketingModel();
            entradaModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                object filial = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, model.DocEntry));
                if (filial != null)
                {
                    entradaModel.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.ToString());
                }
            }
            catch { }

            entradaModel.DocumentLines = new List<Documentline>();
            Documentline itemModel = new Documentline();
            itemModel.BaseEntry = model.DocEntry;
            itemModel.BaseType = 202;
            itemModel.Quantity = model.Quantidade;
            entradaModel.DocumentLines.Add(itemModel);

            string tipoControle = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("Item_GetTipoControle"), BaseBLL.Database, model.ItemCode)).ToString();
            if (tipoControle == "L")
            {
                entradaModel.DocumentLines[0].BatchNumbers = new List<Batchnumber>();

                Batchnumber batch = new Batchnumber();
                batch.BatchNumber = model.NrOP.ToString();
                batch.Quantity = model.Quantidade;
                batch.ManufacturingDate = DateTime.Today;
                batch.ManufacturerSerialNumber = model.NrOP.ToString();

                entradaModel.DocumentLines[0].BatchNumbers.Add(batch);
            }
            else if (tipoControle == "S")
            {
                entradaModel.DocumentLines[0].SerialNumbers = new List<Serialnumber>();

                for (int i = 0; i < model.Quantidade; i++)
                {
                    Serialnumber serial = new Serialnumber();
                    serial.InternalSerialNumber = model.NrOP.ToString() + (i + 1).ToString();
                    serial.ManufacturerSerialNumber = model.NrOP.ToString() + (i + 1).ToString();
                    serial.ManufactureDate = DateTime.Today;
                    serial.Quantity = 1;

                    entradaModel.DocumentLines[0].SerialNumbers.Add(serial);
                }
            }

            ServiceLayerUtil sl = new ServiceLayerUtil();
            string retorno = await sl.PostAsync<DocumentoMarketingModel>("InventoryGenEntries", entradaModel);
            if (!String.IsNullOrEmpty(retorno))
            {
                retorno = "Erro ao gerar entrada do produto acabado: " + retorno;
            }
            else if (tipoControle != "N")
            {
                PedidoVendaBLL pedidoVendaBLL = new PedidoVendaBLL();
                retorno = await pedidoVendaBLL.InserirLoteSL(model.NrOP, model.DocEntryPedido, model.ItemCode, tipoControle, model.Quantidade);
            }
            return retorno;
        }

        public async Task<string> GeraEntradaAcabadoDI(ProducaoModel model)
        {
            Documents doc = SBOConnectionBLL.Company.GetBusinessObject(BoObjectTypes.oInventoryGenEntry) as Documents;
            try
            {
                doc.DocDate = DateTime.Today;
                try
                {
                    object filial = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, model.DocEntry));
                    if (filial != null)
                    {
                        doc.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.ToString());
                    }
                }
                catch { }
                
                doc.Lines.BaseEntry = model.DocEntry;
                doc.Lines.BaseType = 202;
                doc.Lines.Quantity = model.Quantidade;

                string tipoControle = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("Item_GetTipoControle"), BaseBLL.Database, model.ItemCode)).ToString();
                if (tipoControle == "L")
                {
                    doc.Lines.BatchNumbers.BatchNumber = model.NrOP.ToString();
                    doc.Lines.BatchNumbers.ManufacturerSerialNumber = model.NrOP.ToString();
                    doc.Lines.BatchNumbers.Quantity = model.Quantidade;
                    doc.Lines.BatchNumbers.ManufacturingDate = DateTime.Today;
                }
                else if (tipoControle == "S")
                {
                    for (int i = 0; i < model.Quantidade; i++)
                    {
                        if (i > 0)
                        {
                            doc.Lines.SerialNumbers.Add();
                        }
                        doc.Lines.SerialNumbers.InternalSerialNumber = model.NrOP.ToString() + (i + 1).ToString();
                        doc.Lines.SerialNumbers.ManufacturerSerialNumber = model.NrOP.ToString() + (i + 1).ToString();
                        doc.Lines.SerialNumbers.ManufactureDate = DateTime.Today;
                    }
                }
                string retorno = String.Empty;
                if (doc.Add() != 0)
                {
                    retorno = "Erro ao gerar entrada do produto acabado: "  + SBOConnectionBLL.Company.GetLastErrorDescription();
                }
                else if (tipoControle != "N")
                {
                    PedidoVendaBLL pedidoVendaBLL = new PedidoVendaBLL();
                    retorno = await pedidoVendaBLL.InserirLoteDI(model.NrOP, model.DocEntryPedido, model.ItemCode, tipoControle, model.Quantidade);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return "Erro geral ao gerar entrada do produto acabado: " + ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(doc);
                doc = null;
            }
        }

        public async Task<string> GeraEntradaAcabadoApontamentoHR(ApontamentoHRModel model)
        {
            var entradaModel = new DocumentoMarketingModel();
            entradaModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                object filial = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, model.OPDocEntry));
                if (filial != null)
                {
                    entradaModel.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.ToString());
                }
            }
            catch { }

            entradaModel.DocumentLines = new List<Documentline>();
            Documentline itemModel = new Documentline();
            itemModel.BaseEntry = model.OPDocEntry;
            itemModel.BaseType = 202; // OP
            itemModel.Quantity = model.OkQuantity;
            entradaModel.DocumentLines.Add(itemModel);

            var sl = new ServiceLayerUtil();
            string retorno = await sl.PostAsync("InventoryGenEntries", entradaModel);
            if (!string.IsNullOrEmpty(retorno))
            {
                retorno = "Erro ao gerar entrada do produto acabado: " + retorno;
            }

            return retorno;
        }

        public async Task<string> GeraEntradaAcabadoOP(ProducaoModel model)
        {
            var entradaModel = new DocumentoMarketingModel();
            entradaModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
            try
            {
                object filial = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, model.DocEntry));
                if (filial != null)
                {
                    entradaModel.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.ToString());
                }
            }
            catch { }

            entradaModel.DocumentLines = new List<Documentline>();
            Documentline itemModel = new Documentline();
            itemModel.BaseEntry = model.DocEntry;
            itemModel.BaseType = 202; // OP
            itemModel.Quantity = model.QuantidadeApontada;
            entradaModel.DocumentLines.Add(itemModel);

            var sl = new ServiceLayerUtil();
            string retorno = await sl.PostAsync("InventoryGenEntries", entradaModel);
            if (!string.IsNullOrEmpty(retorno))
            {
                retorno = "Erro ao gerar entrada do produto acabado: " + retorno;
            }

            return retorno;
        }
    }
}
