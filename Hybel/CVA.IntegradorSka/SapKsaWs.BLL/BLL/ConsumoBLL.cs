using CVA.AddOn.Common;
using DelimitedDataHelper.Tab;
using log4net;
using SAPbobsCOM;
using SapKsaWs.BLL.HELPER;
using SapKsaWs.BLL.MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace SapKsaWs.BLL
{
    public class ConsumoBLL
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ConsumoBLL()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void GeraArquivoConsumo()
        {
            string filePath = ConfigurationManager.AppSettings["ArquivoConsumo"];
            string update = DAO.MES.Log_Update;
            SqlHelper helperSka = new SqlHelper("ska");
            SqlHelper helperSap = new SqlHelper("sap");

            Logger.Debug("Buscando dados com Registro de Tempo OK");
            // Busca os que o registro de tempo estão OK
            List<SSPExportProdLogModel> listTempoOK = helperSka.FillModelList<SSPExportProdLogModel>(DAO.MES.Log_GetTempoOK);
            Logger.Debug("Dados encontrados: " + listTempoOK.Count);
            foreach (var itemTempoOk in listTempoOK)
            {
                Logger.Debug($"Buscando dados de consumo - OP {itemTempoOk.OP.Trim()}, {itemTempoOk.BelPosId.Trim()}, POS_ID: {itemTempoOk.PosId.Trim()}");
                List<ConsumoOPModel> listConsumo = helperSap.FillModelList<ConsumoOPModel>(String.Format(DAO.BEAS.Consumo_GetByPosicao, itemTempoOk.IdMes, itemTempoOk.BelPosId.Trim(), itemTempoOk.PosId.Trim()));

                StatusLogEnum status = StatusLogEnum.EmProcessamento;
                if (listConsumo.Count == 0) //Se não tiver nenhum item a ser consumido, atualizado o status na tabela de log
                {
                    Logger.Debug($"Nenhum item a ser consumido - OP {itemTempoOk.OP.Trim()}, {itemTempoOk.BelPosId.Trim()}, POS_ID: {itemTempoOk.PosId.Trim()}");
                    int ultimaPosicao = Convert.ToInt32(helperSap.ExecuteScalar(String.Format(DAO.BEAS.Consumo_GetUltimaPosicao, itemTempoOk.OP, itemTempoOk.BelPosId, itemTempoOk.PosId)));

                    string updateSql;
                    if (ultimaPosicao == 1)
                    {
                        Logger.Debug($"Última posição, atualizando OP {itemTempoOk.OP.Trim()} para gerar acabado");
                        status = StatusLogEnum.GerarAcabado;
                        updateSql = String.Format(update, (int)status, "", itemTempoOk.IdMes, (int)TipoLogEnum.Consumo);
                    }
                    else
                    {
                        string msg = String.Empty;

                        //if (itemTempoOk.Refugo > 0)
                        //{
                        //    msg = refugoBLL.GeraRefugo(itemTempoOk);
                        //}
                        if (String.IsNullOrEmpty(msg))
                        {
                            msg = $"OP {itemTempoOk.OP.Trim()} sem item a ser consumido. BELPOS_ID: {itemTempoOk.BelPosId.Trim()}, POS_ID: {itemTempoOk.PosId.Trim()}";
                            status = StatusLogEnum.SemConsumoMaterialNaPosicao;
                        }
                        else
                        {
                            status = StatusLogEnum.VerificarObservacao;
                        }

                        updateSql = String.Format(update, (int)status, msg.Replace(",", ""), itemTempoOk.IdMes, (int)TipoLogEnum.Consumo);
                    }

                    helperSka.ExecuteNonQuery(updateSql);
                    continue;
                }

                if (listConsumo.Any(l => l.Quantity == 0))
                {
                    status = StatusLogEnum.QuantidadeZero;
                    helperSka.ExecuteNonQuery(String.Format(update, (int)status, "", itemTempoOk.IdMes, (int)TipoLogEnum.Consumo));
                    continue;
                }

                decimal refugo = Convert.ToDecimal(helperSka.ExecuteScalar(String.Format(DAO.MES.Refugo_Get, itemTempoOk.OP, itemTempoOk.BelPosId, itemTempoOk.PosId)));

                string itensSemEstoque = String.Empty;
                // Verifica se possui quantidade em estoque
                foreach (var itemConsumo in listConsumo)
                {
                    itemConsumo.Quantity -= refugo;
                    decimal emEstoque = Convert.ToDecimal(helperSap.ExecuteScalar(String.Format(DAO.SBO.Item_GetEstoque, itemConsumo.Itemcode, itemConsumo.Warehouse)));
                    if (emEstoque < itemConsumo.Quantity)
                    {
                        itensSemEstoque += ", " + itemConsumo.Itemcode;
                    }
                }
                if (!String.IsNullOrEmpty(itensSemEstoque))
                {
                    itensSemEstoque = itensSemEstoque.Substring(2);
                    itensSemEstoque = "Itens sem estoque para consumo: " + itensSemEstoque;
                    
                    status = StatusLogEnum.QuantidadeEstoqueInsuficiente;
                    string updateSql = String.Format(update, (int)status, itensSemEstoque, itemTempoOk.IdMes, (int)TipoLogEnum.Consumo);
                    helperSka.ExecuteNonQuery(updateSql);
                    updateSql = String.Format(update, (int)status, itensSemEstoque, itemTempoOk.IdMes, (int)TipoLogEnum.ProdutoAcabado);
                    helperSka.ExecuteNonQuery(updateSql);
                }

                if (status == StatusLogEnum.EmProcessamento)
                {
                    Logger.Debug($"Gerando arquivo Consumo - OP: {itemTempoOk.OP}, BELPOS_ID: {itemTempoOk.BelPosId}, POS_ID: {itemTempoOk.BelPosId}");
                    List<ConsumoOPModel> listCalculado = new List<ConsumoOPModel>();

                    IEnumerable<IGrouping<string, ConsumoOPModel>> listByItemCode = listConsumo.GroupBy(l => l.Itemcode);
                    foreach (var itemByItemCode in listByItemCode)
                    {
                        // Se buscou lote do sistema, calcula a quantidade correta no model
                        if (itemByItemCode.ElementAt(0).BatchQuantity != 0)
                        {
                            decimal totalQuantity = itemByItemCode.ElementAt(0).Quantity;
                            foreach (var item in itemByItemCode)
                            {
                                if (totalQuantity == 0)
                                {
                                    break;
                                }

                                ConsumoOPModel modelCalculado = item;
                                if (modelCalculado.BatchQuantity > totalQuantity)
                                {
                                    modelCalculado.Quantity = totalQuantity;
                                    totalQuantity = 0;
                                }
                                else
                                {
                                    modelCalculado.Quantity = modelCalculado.BatchQuantity;
                                    totalQuantity -= modelCalculado.BatchQuantity;
                                }

                                listCalculado.Add(item);
                            }
                        }
                        else
                        {
                            listCalculado.AddRange(itemByItemCode);
                        }
                    }
                   
                    string fileName = $"Consumo_OP_{itemTempoOk.OP.Trim()}_{itemTempoOk.BelPosId.Trim()}_{itemTempoOk.PosId.Trim()}_{itemTempoOk.IdMes}_{DateTime.Now.ToString("ddMMyyyy_HHmmssfff")}.txt";
                    TabDelimitedDataWriter.WriteToTabDelimitedFile(listCalculado, Path.Combine(filePath, fileName));

                    string msg = String.Empty;
                    //if (itemTempoOk.Refugo > 0)
                    //{
                    //    msg = refugoBLL.GeraRefugo(itemTempoOk);
                    //}
                    if (!String.IsNullOrEmpty(msg))
                    {
                        status = StatusLogEnum.VerificarObservacao;
                    }
                    
                    helperSka.ExecuteNonQuery(String.Format(update, (int)status, msg.Replace(",", ""), itemTempoOk.IdMes, (int)TipoLogEnum.Consumo));
                }
            }
            helperSka.Dispose();
            helperSap.Dispose();
        }

       

        public void VerificaSaidaInsumos()
        {
            Logger.Debug("Verificando saída de insumos");

            string update = DAO.MES.Log_Update;
            string updateSql;
            var helperSap = new SqlHelper("sap");
            var helperSka = new SqlHelper("ska");

            List<SSPExportProdLogModel> modelList = helperSap.FillModelList<SSPExportProdLogModel>(DAO.BEAS.Consumo_GetSaidas);
            Logger.Debug("Dados encontrados: " + modelList.Count);

            foreach (var item in modelList)
            {
                StatusLogEnum status;
                if (!String.IsNullOrEmpty(item.Erro))
                {
                    status = StatusLogEnum.VerificarObservacao;
                }
                else
                {
                    item.Erro = String.Empty;
                    int ultimaPosicao = Convert.ToInt32(helperSap.ExecuteScalar(String.Format(DAO.BEAS.Consumo_GetUltimaPosicao, item.OP, item.BelPosId, item.PosId)));
                    if (ultimaPosicao == 1)
                    {
                        Logger.Debug($"Última posição, atualizando OP {item.OP} para gerar acabado");
                        status = StatusLogEnum.GerarAcabado;
                    }
                    else
                    {
                        updateSql = String.Format(update, (int)StatusLogEnum.SemEntradaAcabado, "Posição sem entrada de produto acabado", item.IdMes, (int)TipoLogEnum.ProdutoAcabado);
                        helperSka.ExecuteNonQuery(updateSql);
                        status = StatusLogEnum.ImportadoComSucesso;
                    }
                }

                updateSql = String.Format(update, (int)status, item.Erro.Replace("'", ""), item.IdMes, (int)TipoLogEnum.Consumo);
                helperSka.ExecuteNonQuery(updateSql);
            }

            helperSap.Dispose();
            helperSka.Dispose();
        }
    }
}
