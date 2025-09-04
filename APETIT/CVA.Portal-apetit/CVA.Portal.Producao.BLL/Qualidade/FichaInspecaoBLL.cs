using CVA.Portal.Producao.BLL.Producao;
using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class FichaInspecaoBLL : BaseBLL
    {
        FichaInspecaoItemBLL FichaInspecaoItemBLL = new FichaInspecaoItemBLL();

        public FichaInspecaoBLL()
        {
            DAO.TableName = "@CVA_FICHA_INSPECAO";
        }

        #region GetList
        public List<FichaInspecaoModel> GetList(DateTime? startDate, DateTime? endDate, string tipoDoc, int? nf = null, string codEtapa = null)
        {
            var modeloFichaBLL = new ModeloFichaBLL();
            var etapaItinerarioBLL = new EtapaItinerarioBLL();

            string where = string.Empty;

            if (!startDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.MaxValue;
            }

            where += $"\"U_DataInsp\" BETWEEN '{startDate.Value.ToString("yyyy-MM-dd")}' AND '{endDate.Value.ToString("yyyy-MM-dd")}'";

            if (nf.HasValue)
            {
                where += $" AND \"U_NrNF\" = {nf}";
            }

            if (!string.IsNullOrEmpty(codEtapa))
            {
                where += $" AND LOWER(\"U_CodEtapa\") = '{codEtapa}'";
            }

            where += " AND (\"U_StatusLote\" = 'A' OR \"U_StatusLote\" = 'R')"; // apenas aprovadas ou reprovadas
            where += $" AND \"U_TipoDoc\" = '{tipoDoc}'";

            var inspecaoList = DAO.RetrieveModelList<FichaInspecaoModel>(where);

            if (inspecaoList != null)
            {
                var especList = new TipoEspecificacaoBLL().Get();

                foreach (var inspecao in inspecaoList)
                {
                    inspecao.DataInspStr = inspecao.DataInsp.ToString("yyyy-MM-dd");
                    inspecao.SequenciaParcial = inspecao.Sequencia;
                    inspecao.QtdeSeq = inspecao.Quantidade;
                    inspecao.ItemList = FichaInspecaoItemBLL.GetByFicha(inspecao.Code);

                    if (inspecao.ItemList != null)
                    {
                        foreach (var item in inspecao.ItemList)
                        {
                            var tipoEspec = especList.FirstOrDefault(m => m.Code == item.CodEspec);
                            if (tipoEspec != null)
                            {
                                item.DescEspec = tipoEspec.Descricao;
                            }
                        }
                    }

                    var groupedByAmostra = inspecao.ItemList.GroupBy(m => m.IdAmostra);

                    if (groupedByAmostra.Count() > 1)
                    {
                        inspecao.AmostraList = new List<FichaInspecaoAmostraModel>();

                        foreach (var item in groupedByAmostra)
                        {
                            var amostraModel = new FichaInspecaoAmostraModel();
                            amostraModel.IdAmostra = item.Key;
                            amostraModel.ItemList = new List<FichaInspecaoItemModel>();
                            amostraModel.ItemList.AddRange(item);
                            inspecao.AmostraList.Add(amostraModel);
                        }
                        inspecao.ItemList = null;
                    }

                    var etapaItinerarioModel = etapaItinerarioBLL.GetByCode(inspecao.CodEtapa);
                    var modeloFichaModel = modeloFichaBLL.Get(inspecao.CodModelo);
                    inspecao.DescEtapa = etapaItinerarioModel.Desc;
                    inspecao.DescModelo = modeloFichaModel.Descricao;
                }

                if (tipoDoc == "OP")
                {
                    inspecaoList = inspecaoList
                        .OrderByDescending(x => x.StatusLote)
                        .ThenBy(x => x.DocNum)
                        .ToList();
                }
                else
                {
                    inspecaoList = inspecaoList
                        .OrderByDescending(x => x.StatusLote)
                        .ThenBy(x => x.NrNF)
                        .ToList();
                }
            }

            return inspecaoList;
        }
        #endregion

        #region Get
        public FichaInspecaoModel Get(string code)
        {
            ModeloFichaBLL modeloFichaBLL = new ModeloFichaBLL();
            EtapaItinerarioBLL etapaItinerarioBLL = new EtapaItinerarioBLL();
            TipoEspecificacaoBLL tipoEspecificacaoBLL = new TipoEspecificacaoBLL();
            List<TipoEspecificacaoModel> especList = tipoEspecificacaoBLL.Get();

            FichaInspecaoModel model = DAO.RetrieveModel<FichaInspecaoModel>($"\"Code\" = '{code}'");
            model.DataInspStr = model.DataInsp.ToString("yyyy-MM-dd");
            model.SequenciaParcial = model.Sequencia;
            model.QtdeSeq = model.Quantidade;

            model.ItemList = FichaInspecaoItemBLL.GetByFicha(model.Code);

            foreach (var item in model.ItemList)
            {
                TipoEspecificacaoModel tipoEspec = especList.FirstOrDefault(m => m.Code == item.CodEspec);
                if (tipoEspec != null)
                {
                    item.DescEspec = tipoEspec.Descricao;
                }
            }

            IEnumerable<IGrouping<int, FichaInspecaoItemModel>> groupedByAmostra = model.ItemList.GroupBy(m => m.IdAmostra);

            if (groupedByAmostra.Count() > 1)
            {
                model.AmostraList = new List<FichaInspecaoAmostraModel>();

                foreach (var item in groupedByAmostra)
                {
                    FichaInspecaoAmostraModel amostraModel = new FichaInspecaoAmostraModel();
                    amostraModel.IdAmostra = item.Key;
                    amostraModel.ItemList = new List<FichaInspecaoItemModel>();
                    amostraModel.ItemList.AddRange(item);
                    model.AmostraList.Add(amostraModel);
                }
                model.ItemList = null;
            }

            EtapaItinerarioModel etapaItinerarioModel = etapaItinerarioBLL.GetByCode(model.CodEtapa);
            ModeloFichaModel modeloFichaModel = modeloFichaBLL.Get(model.CodModelo);
            model.DescEtapa = etapaItinerarioModel.Desc;
            model.DescModelo = modeloFichaModel.Descricao;

            return model;
        }
        #endregion

        #region GetByOPItemEtapa
        public FichaInspecaoModel GetByOPItemEtapa(int docEntryOP, string codItem, string codEtapa, int modal)
        {
            //FichaInspecaoModel model = DAO.RetrieveModel<FichaInspecaoModel>($"\"U_DocEntry\" = '{docEntryOP}' AND \"U_TipoDoc\" = 'OP' AND \"U_CodEtapa\" = '{codEtapa}'", "\"U_Sequencia\"");
            FichaInspecaoModel model = this.GetByItemEtapa(codItem, codEtapa, false);
            double sequenciaFinalizada = this.GetQtdeApontada(docEntryOP, codEtapa, modal);
            model.QtdeAnalisada = 1;
            model.Sequencia = Convert.ToInt32(DAO.GetNextCode("@CVA_FICHA_INSPECAO", "U_Sequencia", $"\"U_DocEntry\" = '{docEntryOP}' AND \"U_TipoDoc\" = 'OP' AND \"U_CodEtapa\" = '{codEtapa}'"));

            if (modal == 1)
            {
                model.SequenciaParcial = (int)sequenciaFinalizada + 1;
            }
            else
            {
                model.SequenciaParcial = model.Sequencia;
                model.QtdeSeq = sequenciaFinalizada;
            }

            return model;
        }

        public double GetQtdeApontada(int docEntryOP, string codEtapa, int finalizado)
        {
            return Convert.ToDouble(DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("Apontamento_GetQtde"), BaseBLL.Database, docEntryOP, codEtapa, finalizado)));
        }

        #endregion

        #region GetByItemEtapa
        public FichaInspecaoModel GetByItemEtapa(string codItem, string codEtapa, bool considerarAmostra = true)
        {
            ModeloFichaBLL modeloFichaBLL = new ModeloFichaBLL();
            FichaProdutoBLL fichaProdutoBLL = new FichaProdutoBLL();
            EtapaItinerarioBLL etapaItinerarioBLL = new EtapaItinerarioBLL();
            TipoEspecificacaoBLL tipoEspecificacaoBLL = new TipoEspecificacaoBLL();
            List<TipoEspecificacaoModel> especList = tipoEspecificacaoBLL.Get();

            FichaInspecaoModel model = new FichaInspecaoModel();

            FichaProdutoModel fichaProdutoModel = fichaProdutoBLL.GetByItemEtapa(codItem, codEtapa);
            EtapaItinerarioModel etapaItinerarioModel = etapaItinerarioBLL.GetByCode(fichaProdutoModel.CodEtapa);

            ModeloFichaModel modeloFichaModel = modeloFichaBLL.Get(fichaProdutoModel.CodModelo);

            model.QtdeAmostra = modeloFichaModel.QtdeAmostra;
            model.CodEtapa = fichaProdutoModel.CodEtapa;
            model.DescEtapa = etapaItinerarioModel.Desc;
            model.CodModelo = modeloFichaModel.Code;
            model.DescModelo = modeloFichaModel.Descricao;
            model.DataInsp = DateTime.Today;

            if (model.QtdeAmostra == 0 || !considerarAmostra)
            {
                model.QtdeAmostra = 1;
            }

            if (model.QtdeAmostra == 1)
            {
                model.ItemList = new List<FichaInspecaoItemModel>();
                foreach (var modeloItem in modeloFichaModel.ItemList)
                {
                    FichaInspecaoItemModel itemModel = new FichaInspecaoItemModel();
                    itemModel.IdAmostra = 1;
                    itemModel.CodFicha = modeloItem.Code;
                    itemModel.Amostragem = modeloItem.Amostragem;
                    itemModel.Analise = modeloItem.Analise;
                    itemModel.CodEspec = modeloItem.CodEspec;
                    //itemModel.Link = modeloItem.Link;
                    itemModel.Metodo = modeloItem.Metodo;
                    itemModel.Observacao = modeloItem.Observacao;
                    itemModel.PadraoAte = modeloItem.PadraoAte;
                    itemModel.PadraoDe = modeloItem.PadraoDe;
                    itemModel.VlrNominal = modeloItem.VlrNominal;
                    itemModel.TipoCampo = modeloItem.TipoCampo;

                    TipoEspecificacaoModel tipoEspec = especList.FirstOrDefault(m => m.Code == itemModel.CodEspec);
                    if (tipoEspec != null)
                    {
                        itemModel.DescEspec = tipoEspec.Descricao;
                    }
                    model.ItemList.Add(itemModel);
                }
            }
            else
            {
                model.AmostraList = new List<FichaInspecaoAmostraModel>();
                for (int i = 0; i < model.QtdeAmostra; i++)
                {
                    FichaInspecaoAmostraModel amostraModel = new FichaInspecaoAmostraModel();
                    amostraModel.IdAmostra = i + 1;
                    amostraModel.ItemList = new List<FichaInspecaoItemModel>();

                    foreach (var modeloItem in modeloFichaModel.ItemList)
                    {
                        FichaInspecaoItemModel itemModel = new FichaInspecaoItemModel();
                        itemModel.IdAmostra = i + 1;
                        itemModel.CodFicha = modeloItem.Code;
                        itemModel.Amostragem = modeloItem.Amostragem;
                        itemModel.Analise = modeloItem.Analise;
                        itemModel.CodEspec = modeloItem.CodEspec;
                        //itemModel.Link = modeloItem.Link;
                        itemModel.Metodo = modeloItem.Metodo;
                        itemModel.Observacao = modeloItem.Observacao;
                        itemModel.PadraoAte = modeloItem.PadraoAte;
                        itemModel.PadraoDe = modeloItem.PadraoDe;
                        itemModel.VlrNominal = modeloItem.VlrNominal;
                        itemModel.TipoCampo = modeloItem.TipoCampo;

                        TipoEspecificacaoModel tipoEspec = especList.FirstOrDefault(m => m.Code == itemModel.CodEspec);
                        if (tipoEspec != null)
                        {
                            itemModel.DescEspec = tipoEspec.Descricao;
                        }
                        amostraModel.ItemList.Add(itemModel);
                    }
                    model.AmostraList.Add(amostraModel);
                }
            }

            return model;
        }
        #endregion

        public void Create(FichaInspecaoModel model)
        {
            DAO.Model = model;
            model.Ano = DateTime.Today.Year;
            model.ID = Convert.ToInt32(DAO.GetNextCode("@CVA_FICHA_INSPECAO", "U_ID", "\"U_Ano\" = " + DateTime.Today.Year));
            model.Code = DateTime.Today.Year + model.ID.ToString().PadLeft(6, '0');

            DAO.CreateModel();
            FichaInspecaoItemBLL.Create(model);
            DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("Apontamento_UpsertQtdeCQ"), BaseBLL.Database, model.DocEntry, model.DescEtapa, 1));
        }

        public void Update(FichaInspecaoModel model)
        {
            DAO.Model = model;
            DAO.UpdateModel();

            FichaInspecaoItemBLL.Update(model);
        }
    }
}
