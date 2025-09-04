using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.BLL.Util;
using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model.Producao;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CVA.Portal.Producao.BLL.Producao
{
    public class SaidaInsumosBLL : BaseBLL
    {
        public async Task<string> ExecutaSaidaInsumos(ProducaoModel model)
        {
            if (Static.Config.ServerType == BoDataServerTypes.dst_HANADB)
            {
                return await this.ExecutaSaidaInsumosSL(model);
            }
            else
            {
                return await this.ExecutaSaidaInsumosDI(model);
            }
        }

        public List<SelectListItem> GetTiposSaida()
        {
            try
            {
                var modal = DAO.FillListFromCommand<SelectListItem>(string.Format(Commands.Resource.GetString("GetTiposSaida"), Database));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ExecutaSaidaInsumosSL(ProducaoModel model)
        {
            model.Estrutura = model.Estrutura.Where(m => m.QtdeRealizada > 0 && m.MetodoBaixa != "B").ToList();

            ServiceLayerUtil sl = new ServiceLayerUtil();
            await sl.Login();
            string retorno = String.Empty;

            if (model.Estrutura.Count > 0)
            {
                DocumentoMarketingModel saidaInsumosModel = new DocumentoMarketingModel();
                saidaInsumosModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");

                try
                {
                    object filial = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, model.DocEntry));
                    if (filial != null)
                    {
                        saidaInsumosModel.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.ToString());
                    }
                }
                catch { }

                saidaInsumosModel.DocumentLines = new List<Documentline>();

                int baseLine = 0;
                foreach (var item in model.Estrutura)
                {
                    Documentline lineModel = new Documentline();
                    lineModel.BaseEntry = item.DocEntry;
                    lineModel.BaseLine = item.LineNum;
                    lineModel.BaseType = 202; // OP
                    lineModel.Quantity = item.QtdeRealizada;

                    if (item.ControlePorLote == "Y")
                    {
                        lineModel.BatchNumbers = new List<Batchnumber>();
                        item.LoteSerie = item.LoteSerie.Where(m => m.Quantidade > 0).ToList();
                        foreach (var loteSerie in item.LoteSerie)
                        {
                            Batchnumber batchModel = new Batchnumber();
                            batchModel.BatchNumber = loteSerie.Lote;
                            batchModel.Quantity = loteSerie.Quantidade;
                            batchModel.BaseLineNumber = baseLine;
                            lineModel.BatchNumbers.Add(batchModel);
                        }
                    }
                    else if (item.ControlePorSerie == "Y")
                    {
                        lineModel.SerialNumbers = new List<Serialnumber>();
                        item.LoteSerie = item.LoteSerie.Where(m => m.Selecionado).ToList();
                        foreach (var loteSerie in item.LoteSerie)
                        {
                            Serialnumber serialModel = new Serialnumber();
                            serialModel.InternalSerialNumber = loteSerie.Serie;
                            serialModel.SystemSerialNumber = loteSerie.SystemSerie;
                            serialModel.Quantity = 1;
                            serialModel.BaseLineNumber = baseLine;

                            lineModel.SerialNumbers.Add(serialModel);
                        }
                    }

                    baseLine++;
                    saidaInsumosModel.DocumentLines.Add(lineModel);
                }

                Logger.Log($"Iniciando saída de insumos para OP {model.NrOP}");

                retorno = await sl.PostAsync<DocumentoMarketingModel>("InventoryGenExits", saidaInsumosModel);
            }

            if (string.IsNullOrEmpty(retorno))
            {
                Logger.Log($"Saída de insumos realizada com sucesso! OP: {model.NrOP}");
                string status = model.Concluir ? "C" : "P";
                string update = String.Format(Commands.Resource.GetString("Etapa_UpdateStatus"), BaseBLL.Database, model.DocEntry, model.CodEtapa, status, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                DAO.ExecuteNonQuery(update);
            }

            if (!string.IsNullOrEmpty(retorno))
            {
                Logger.Log($"Erro ao gerar saída de insumos para OP {model.NrOP}: {retorno}");
                retorno = "Erro ao gerar saída de insumos: " + retorno;
            }
            else if (model.Concluir)
            {
                // Se não tiver mais nenhuma etapa, gera o acabado e fecha a OP
                if (!DAO.HasRows(String.Format(Commands.Resource.GetString("Etapa_GetEmAberto"), BaseBLL.Database, model.DocEntry)))
                {
                    EntradaAcabadoBLL entradaAcabadoBLL = new EntradaAcabadoBLL();
                    Logger.Log($"Iniciando entrada do acabado para OP {model.NrOP}");
                    retorno = await entradaAcabadoBLL.GeraEntradaAcabado(model);
                    if (string.IsNullOrEmpty(retorno))
                    {
                        Logger.Log($"Iniciando entrada do acabado para OP {model.NrOP}");
                        ProducaoBLL producaoBLL = new ProducaoBLL();
                        retorno = await producaoBLL.FechaOP(model);
                    } else
                    {
                        Logger.Log($"Erro ao gerar entrada do acabado para OP {model.NrOP}: {retorno}");
                    }
                }
            }

            await sl.Logout();

            if (!String.IsNullOrEmpty(retorno))
            {
                Logger.Log($"Erro ao gerar saída de insumos para OP {model.NrOP}: {retorno}");
                throw new Exception(retorno);
            }
            else
            {
                Logger.Log($"Atualizando a quantidade apontada para OP {model.NrOP}");
                DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("Apontamento_UpsertQtdeApontada"), BaseBLL.Database, model.DocEntry, model.Etapa, model.Quantidade.ToString().Replace(",", ".")));
                Logger.Log($"Quantidade apontada atualizada com sucesso para OP {model.NrOP}");
            }

            return retorno;
        }

        public async Task<string> ExecutaSaidaInsumosDI(ProducaoModel model)
        {
            model.Estrutura = model.Estrutura.Where(m => m.QtdeRealizada > 0).ToList();

            string retorno = String.Empty;

            if (model.Estrutura.Count > 0)
            {
                Documents doc = SBOConnectionBLL.Company.GetBusinessObject(BoObjectTypes.oInventoryGenExit) as Documents;
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

                foreach (var item in model.Estrutura)
                {
                    if (doc.Lines.BaseEntry != 0)
                    {
                        doc.Lines.Add();
                    }

                    doc.Lines.BaseEntry = item.DocEntry;
                    doc.Lines.BaseLine = item.LineNum;
                    doc.Lines.BaseType = 202; // OP
                    doc.Lines.Quantity = item.QtdeRealizada;

                    if (item.ControlePorLote == "Y")
                    {
                        if (!String.IsNullOrEmpty(doc.Lines.BatchNumbers.BatchNumber))
                        {
                            doc.Lines.BatchNumbers.Add();
                        }
                        item.LoteSerie = item.LoteSerie.Where(m => m.Quantidade > 0).ToList();
                        foreach (var loteSerie in item.LoteSerie)
                        {
                            if (!String.IsNullOrEmpty(doc.Lines.BatchNumbers.BatchNumber))
                            {
                                doc.Lines.BatchNumbers.Add();
                            }

                            doc.Lines.BatchNumbers.BatchNumber = loteSerie.Lote;
                            doc.Lines.BatchNumbers.Quantity = loteSerie.Quantidade;
                        }
                    }
                    else if (item.ControlePorSerie == "Y")
                    {
                        if (!String.IsNullOrEmpty(doc.Lines.SerialNumbers.InternalSerialNumber))
                        {
                            doc.Lines.SerialNumbers.Add();
                        }
                        item.LoteSerie = item.LoteSerie.Where(m => m.Selecionado).ToList();
                        foreach (var loteSerie in item.LoteSerie)
                        {
                            doc.Lines.SerialNumbers.InternalSerialNumber = loteSerie.Serie;
                            doc.Lines.SerialNumbers.Quantity = 1;
                        }
                    }
                }

                if (doc.Add() != 0)
                {
                    retorno = SBOConnectionBLL.Company.GetLastErrorDescription();
                }
            }

            string status = model.Concluir ? "C" : "P";
            string update = String.Format(Commands.Resource.GetString("Etapa_UpdateStatus"), BaseBLL.Database, model.DocEntry, model.CodEtapa, status, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            DAO.ExecuteNonQuery(update);

            if (!String.IsNullOrEmpty(retorno))
            {
                retorno = "Erro ao gerar saída de insumos: " + retorno;
            }
            else if (model.Concluir)
            {
                // Se não tiver mais nenhuma etapa, gera o acabado e fecha a OP
                if (!DAO.HasRows(String.Format(Commands.Resource.GetString("Etapa_GetEmAberto"), BaseBLL.Database, model.DocEntry)))
                {
                    EntradaAcabadoBLL entradaAcabadoBLL = new EntradaAcabadoBLL();
                    retorno = await entradaAcabadoBLL.GeraEntradaAcabado(model);
                    if (String.IsNullOrEmpty(retorno))
                    {
                        ProducaoBLL producaoBLL = new ProducaoBLL();
                        retorno = await producaoBLL.FechaOP(model);
                    }
                }
            }

            if (!String.IsNullOrEmpty(retorno))
            {
                throw new Exception(retorno);
            }
            else
            {
                DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("Apontamento_UpsertQtdeApontada"), BaseBLL.Database, model.DocEntry, model.Etapa, model.QtdeRealizada.ToString().Replace(",", "."), 0));
                //DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("FichaInspecao_UpdateSeqFinalizada"), BaseBLL.Database, docEntry, etapa));
            }

            return retorno;
        }

        public async Task<string> ExecutaSaidaRecurso(ApontamentoHRModel model)
        {
            var producaoBLL = new ProducaoBLL();
            var sl = new ServiceLayerUtil();
            await sl.Login();
            string retorno = string.Empty;

            var saidaInsumosModel = new DocumentoMarketingModel();
            saidaInsumosModel.DocumentLines = new List<Documentline>();
            saidaInsumosModel.Comments = model.Obs;

            try
            {
                object filial = DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, model.OPDocEntry));
                if (filial != null)
                {
                    saidaInsumosModel.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.ToString());
                }
            }
            catch { }

            var lineModel = new Documentline();
            lineModel.BaseEntry = model.OPDocEntry;
            lineModel.BaseLine = model.RecursoLineNum;
            lineModel.BaseType = 202; // OP
            lineModel.Quantity = model.Duration;

            if (!string.IsNullOrEmpty(model.OcrCode))
                lineModel.OcrCode = model.OcrCode;

            saidaInsumosModel.DocumentLines.Add(lineModel);
            retorno = await sl.PostAsync("InventoryGenExits", saidaInsumosModel);

            if (!string.IsNullOrEmpty(retorno))
            {
                throw new Exception(retorno);
            }

            // Verifica se o apontamento é para o último recurso da etapa para dar a saída de material ou não
            if (model.OkQuantity > 0 && producaoBLL.IsLastResource(model.OPDocEntry, model.RecursoLineNum, model.CodEtapa))
            {
                retorno = await ExecutaSaidaMP(model);

                if (!string.IsNullOrEmpty(retorno))
                {
                    throw new Exception(retorno);
                }
            }

            // Verifica se o apontamento é para o último recurso da OP para dar a entrada no acabado ou não
            if (model.OkQuantity > 0 && producaoBLL.IsLastResource(model.OPDocEntry, model.RecursoLineNum))
            {
                retorno = await new EntradaAcabadoBLL().GeraEntradaAcabadoApontamentoHR(model);

                if (!string.IsNullOrEmpty(retorno))
                {
                    throw new Exception(retorno);
                }
            }

            return retorno;
        }

        public async Task<string> ExecutaSaidaMP(ApontamentoHRModel model)
        {
            string retorno = string.Empty;
            var itens = new ProducaoBLL().GetItensEstoque(model.OPDocEntry, model.CodEtapa);

            if (itens != null && itens.Count > 0)
            {
                var sl = new ServiceLayerUtil();

                var saidaInsumosModel = new DocumentoMarketingModel();
                saidaInsumosModel.DocumentLines = new List<Documentline>();
                saidaInsumosModel.Comments = model.Obs;

                try
                {
                    object filial = DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, model.OPDocEntry));
                    if (filial != null)
                    {
                        saidaInsumosModel.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.ToString());
                    }
                }
                catch { }

                foreach (var item in itens)
                {
                    var lineModel = new Documentline();
                    lineModel.BaseEntry = model.OPDocEntry;
                    lineModel.BaseLine = item.LineNum;
                    lineModel.BaseType = 202; // OP
                    lineModel.Quantity = item.BaseQty * model.OkQuantity;

                    if (!string.IsNullOrEmpty(model.OcrCode))
                        lineModel.OcrCode = model.OcrCode;

                    saidaInsumosModel.DocumentLines.Add(lineModel);
                }

                retorno = await sl.PostAsync("InventoryGenExits", saidaInsumosModel);

                if (!string.IsNullOrEmpty(retorno))
                {
                    retorno = "Erro ao gerar a baixa da matéria prima: " + retorno;
                }
            }

            return retorno;
        }

        public async Task<string> ExecutaSaidaEtapa(List<ProducaoModel> model)
        {
            string retorno = string.Empty;
            var producaoBLL = new ProducaoBLL();
            var sl = new ServiceLayerUtil();
            await sl.Login();
            int bpl = 0;

            // Validação no último recurso da OP, para que não seja possível apontar a mais do que o produzido nas etapas anteriores
            foreach (var op in model)
            {
                if (producaoBLL.IsLastResource(op.DocEntry, op.RecursoLineNum))
                {
                    var qtdInferiorList = producaoBLL.GetApontamentosQuantidadeInferior(op.DocEntry, op.QuantidadeApontada / op.QuantidadeBase);

                    if (qtdInferiorList != null && qtdInferiorList.Count > 0)
                    {
                        retorno = $"Não foi possível realizar o apontamento da ordem {op.NrOP} na etapa {op.Etapa} com a quantidade {op.QuantidadeApontada}. As seguintes etapas dessa OP não atingiram a quantidade produzida necessária:\n";

                        foreach (var item in qtdInferiorList)
                            retorno += $"\n{item.Etapa} - produzido: {item.Quantidade}";

                        throw new Exception(retorno);
                    }
                }
            }

            foreach (var op in model)
            {
                try
                {
                    object filial = DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("OP_GetFilialPadrao"), BaseBLL.Database, op.DocEntry));
                    if (filial != null)
                        bpl = Convert.ToInt32(filial.ToString());
                }
                catch { }

                var linhas = producaoBLL.GetItensEstoqueRecursos(op.DocEntry, op.CodEtapa);
                var saidaInsumosModel = new DocumentoMarketingModel();
                saidaInsumosModel.DocumentLines = new List<Documentline>();
                saidaInsumosModel.Comments = $"Saída lançada pelo portal.\nOP: {op.NrOP}\nEtapa: {op.Etapa}";
                saidaInsumosModel.BPL_IDAssignedToInvoice = bpl;

                foreach (var linha in linhas)
                {
                    var lineModel = new Documentline();
                    lineModel.BaseEntry = op.DocEntry;
                    lineModel.BaseLine = linha.LineNum;
                    lineModel.BaseType = 202; // OP
                    lineModel.Quantity = op.QuantidadeApontada * linha.BaseQty;

                    if (!string.IsNullOrEmpty(linha.OcrCode))
                        lineModel.OcrCode = linha.OcrCode;

                    saidaInsumosModel.DocumentLines.Add(lineModel);
                }

                retorno = await sl.PostAsync("InventoryGenExits", saidaInsumosModel);

                if (!string.IsNullOrEmpty(retorno))
                {
                    retorno = "Erro ao gerar saída de insumos: " + retorno;
                }
                // Verifica se o apontamento é para o último recurso da OP para dar a entrada no acabado ou não
                else if (producaoBLL.IsLastResource(op.DocEntry, op.RecursoLineNum))
                {
                    retorno = await new EntradaAcabadoBLL().GeraEntradaAcabadoOP(op);

                    if (!string.IsNullOrEmpty(retorno))
                    {
                        retorno = "Erro ao gerar entrada de acabado: " + retorno;
                    }
                    else
                    {
                        // Verifica se a produção já foi concluída para fechar a OP
                        var opModel = producaoBLL.GetDadosOP(op.NrOP, op.CodEtapa.ToString());

                        if (opModel.NrOP != 0 && opModel.Quantidade <= 0)
                            retorno = await producaoBLL.FechaOP(opModel);
                    }
                }

                if (!string.IsNullOrEmpty(retorno))
                    throw new Exception(retorno);
            }

            return retorno;
        }
    }
}
