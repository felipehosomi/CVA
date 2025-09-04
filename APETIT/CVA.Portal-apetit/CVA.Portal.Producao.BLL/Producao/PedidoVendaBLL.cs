using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.BLL.Estoque;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model.Producao;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Producao
{
    public class PedidoVendaBLL
    {
        public async Task<string> InserirLoteSL(int nrOP, int docEntryPedido, string item, string tipoControle, double quantidade)
        {
            try
            {
                var loteBll = new LoteSerieBLL();
                ServiceLayerUtil sl = new ServiceLayerUtil();
                if (docEntryPedido == 0)
                {
                    return String.Empty;
                }

                DocumentoMarketingModel pedidoVendaModel = await sl.GetByIDAsync<DocumentoMarketingModel>("Orders", docEntryPedido.ToString());
                if (pedidoVendaModel.DocEntry == null)
                {
                    return String.Empty;
                }

                // Bug da SL - se manter todas as linhas, ocorre erro ao atualizar o segundo nº de série/lote
                pedidoVendaModel.DocumentLines = pedidoVendaModel.DocumentLines.Where(m => m.ItemCode == item).ToList();

                Documentline line = pedidoVendaModel.DocumentLines[0];

                if (tipoControle == "L")
                {
                    if (line.BatchNumbers == null)
                    {
                        line.BatchNumbers = new List<Batchnumber>();
                    }
                    else
                    {
                        if (line.BatchNumbers.Sum(m => m.Quantity) == line.Quantity) // Se os lotes já foram preenchidos, não faz nada
                        {
                            return String.Empty;
                        }
                    }
                    Batchnumber batch = new Batchnumber();
                    batch.BaseLineNumber = line.LineNum.Value;
                    batch.BatchNumber = nrOP.ToString();
                    batch.Quantity = quantidade;
                    line.BatchNumbers.Add(batch);
                }
                else
                {
                    if (line.SerialNumbers == null)
                    {
                        line.SerialNumbers = new List<Serialnumber>();
                    }
                    else
                    {
                        if (line.SerialNumbers.Count() == line.Quantity) // Se os nrs. de série já foram preenchidos, não faz nada
                        {
                            return String.Empty;
                        }
                    }
                    for (int i = 0; i < quantidade; i++)
                    {
                        string serialNumber = nrOP + (i + 1).ToString();
                        var serieModel = loteBll.GetSerialInfo(item, serialNumber);

                        if (serieModel != null)
                        {
                            Serialnumber serial = new Serialnumber();
                            serial.BaseLineNumber = line.LineNum.Value;
                            serial.InternalSerialNumber = serieModel.Serie;
                            serial.ManufacturerSerialNumber = serieModel.Serie;
                            serial.SystemSerialNumber = serieModel.SystemSerie;
                            serial.Quantity = 1;
                            line.SerialNumbers.Add(serial);
                        }
                    }
                }

                string retorno = await sl.PatchAsync("Orders", docEntryPedido.ToString(), pedidoVendaModel);
                if (!String.IsNullOrEmpty(retorno))
                {
                    retorno = "Erro ao setar lote/série no pedido de venda: " + retorno;
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return "Erro geral ao setar lote/série no pedido de venda: " + ex.Message;
            }
        }

        public async Task<string> InserirLoteDI(int nrOP, int docEntryPedido, string item, string tipoControle, double quantidade)
        {
            try
            {
                if (docEntryPedido == 0)
                {
                    return String.Empty;
                }
                Documents doc = SBOConnectionBLL.Company.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
                if (!doc.GetByKey(docEntryPedido))
                {
                    return String.Empty;
                }

                for (int i = 0; i < doc.Lines.Count; i++)
                {
                    doc.Lines.SetCurrentLine(i);
                    if (doc.Lines.ItemCode == item)
                    {
                        if (tipoControle == "L")
                        {
                            double qtdeAtual = 0;
                            for (int j = 0; j < doc.Lines.BatchNumbers.Count; j++)
                            {
                                doc.Lines.BatchNumbers.SetCurrentLine(j);
                                qtdeAtual += doc.Lines.BatchNumbers.Quantity;
                            }

                            if (qtdeAtual == quantidade) // Se os lotes já foram preenchidos, não faz nada
                            {
                                return String.Empty;
                            }

                            int count = 0;
                            doc.Lines.BatchNumbers.SetCurrentLine(count);
                            while (doc.Lines.BatchNumbers.Quantity > 0 && doc.Lines.BatchNumbers.Count > count)
                            {
                                count++;
                            }
                            if (!String.IsNullOrEmpty(doc.Lines.BatchNumbers.BatchNumber))
                            {
                                doc.Lines.BatchNumbers.Add();
                            }

                            doc.Lines.BatchNumbers.BaseLineNumber = doc.Lines.LineNum;
                            doc.Lines.BatchNumbers.BatchNumber = nrOP.ToString();
                            doc.Lines.BatchNumbers.Quantity = quantidade;
                        }
                        else
                        {
                            if (doc.Lines.SerialNumbers.Count == quantidade && !String.IsNullOrEmpty(doc.Lines.SerialNumbers.InternalSerialNumber)) // Se os nrs. de série já foram preenchidos, não faz nada
                            {
                                return String.Empty;
                            }
                            for (int serieIndex = 0; serieIndex < quantidade; serieIndex++)
                            {
                                if (!String.IsNullOrEmpty(doc.Lines.SerialNumbers.InternalSerialNumber))
                                {
                                    doc.Lines.SerialNumbers.Add();
                                }

                                doc.Lines.SerialNumbers.BaseLineNumber = doc.Lines.LineNum;
                                doc.Lines.SerialNumbers.InternalSerialNumber = nrOP + (serieIndex + 1).ToString();
                                doc.Lines.SerialNumbers.ManufacturerSerialNumber = nrOP + (serieIndex + 1).ToString();
                                doc.Lines.Quantity = 1;
                            }
                        }
                    }
                }

                string retorno = String.Empty;
                if (doc.Update() != 0)
                {
                    retorno = "Erro ao setar lote/série no pedido de venda: " + SBOConnectionBLL.Company.GetLastErrorDescription();
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return "Erro geral ao setar lote/série no pedido de venda: " + ex.Message;
            }
        }
    }
}
