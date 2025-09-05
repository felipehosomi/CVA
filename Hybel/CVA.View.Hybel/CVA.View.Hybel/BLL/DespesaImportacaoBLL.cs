using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using SAPbobsCOM;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CVA.View.Hybel.BLL
{
    public class DespesaImportacaoBLL
    {
        public static ValoresValidos Adicionar(int docNumNFproduto, int docNumNFCTeFrete, int docNumNFArmazenagem, int DocNumNFServicoImportacao, DateTime DataDespesas)
        {

            #region variáveis e objetos
            ValoresValidos mensagemRetorno = new ValoresValidos();
            LandedCostsService landedCostsService = (LandedCostsService)SBOApp.Company.GetCompanyService().GetBusinessService(ServiceTypes.LandedCostsService);
            LandedCost docDespesaImportacao = (LandedCost)landedCostsService.GetDataInterface(SAPbobsCOM.LandedCostsServiceDataInterfaces.lcsLandedCost);
            LandedCost_ItemLine linhaDespesaImportacao = null;
            double valorFrete = 0;
            double valorArmazenagem = 0;
            double valorServicoImportacao = 0;

            double valorImpostoFretePIS = 0;
            double valorImpostoFreteICMS = 0;
            double valorImpostoFreteCOFINS = 0;

            double valorImpostoServicoAssImportacaoPIS = 0;
            double valorImpostoServicoAssImportacaoCOFINS = 0;

            int docEntryNFArmazenagem = 0;
            int docEntryNFServicoImportacao = 0;
            int docEntryNFFrete = 0;
            string SerialNFFrete = string.Empty;
            string SerialNFArmazenagem = string.Empty;
            string SerialServicoImportacao = string.Empty;
            bool existeCusto = false;

            int docEntryNFProdutos = Convert.ToInt32(CrudController.ExecuteScalar(String.Format(SQL.NotaEntrada_GetByDocNum, docNumNFproduto)));
            Documents notaFiscalFrete = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
            Documents notaFiscalArmazenagem = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
            Documents notaFiscalServicoImportacao = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);

            Documents notaFiscalProduto = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
            notaFiscalProduto.GetByKey(docEntryNFProdutos);

            Recordset recordSetDespesasImportacao = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset recordSetNFCteFrete = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset recordSetNFArmazenagem = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            Recordset recordSetNFServicoImportacao = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            DespesasImportacao despesaFreteEncontrada = new DespesasImportacao();
            DespesasImportacao despesaArmazenagemEncontrada = new DespesasImportacao();
            DespesasImportacao despesaServicoImportacaoEncontrada = new DespesasImportacao();

            DespesasImportacao despesaFretePisEncontrada = new DespesasImportacao();
            DespesasImportacao despesaFreteCofinsEncontrada = new DespesasImportacao();
            DespesasImportacao despesaFreteIcmsEncontrada = new DespesasImportacao();

            DespesasImportacao despesaServicoAImportacaoPisEncontrada = new DespesasImportacao();
            DespesasImportacao despesaServicoAImportacaoCofinsEncontrada = new DespesasImportacao();
            #endregion

            List<DespesasImportacao> listaDespesas = CarregarDespesasImportacaoSap();

            if (docNumNFCTeFrete > 0)
            {
                docEntryNFFrete = Convert.ToInt32(CrudController.ExecuteScalar(String.Format(SQL.NotaEntrada_GetByDocNum, docNumNFCTeFrete)));
                notaFiscalFrete.GetByKey(docEntryNFFrete);
                recordSetNFCteFrete.DoQuery(String.Format(SQL.NotaFiscaEntradalBuscarImpostos, docNumNFCTeFrete));

                Recordset recordSetFrete = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSetFrete.DoQuery(String.Format(SQL.NotaEntrada_DespesasImportacao, docNumNFCTeFrete));
                valorFrete = (double)recordSetFrete.Fields.Item("Total").Value;

                List<ValoresImpostos> valores = new List<ValoresImpostos>();

                for (int i = 0; i < recordSetNFCteFrete.RecordCount; i++)
                {
                    ValoresImpostos valor = new ValoresImpostos();
                    valor.Nome = recordSetNFCteFrete.Fields.Item("Name").Value.ToString();
                    valor.ValorImposto = (double)recordSetNFCteFrete.Fields.Item("TaxSum").Value;
                    valores.Add(valor);
                    recordSetNFCteFrete.MoveNext();
                }

                if (valores.Count > 0)
                {
                    valorImpostoFretePIS = valores.Where(d => d.Nome == "PIS").FirstOrDefault().ValorImposto;
                    valorImpostoFreteCOFINS = valores.Where(d => d.Nome == "COFINS").FirstOrDefault().ValorImposto;
                    valorImpostoFreteICMS = valores.Where(d => d.Nome == "ICMS").FirstOrDefault().ValorImposto;
                }

                notaFiscalFrete.Lines.SetCurrentLine(0);
                despesaFreteEncontrada = listaDespesas.Where(d => d.CodigoItem == notaFiscalFrete.Lines.ItemCode).FirstOrDefault();

                despesaFretePisEncontrada = listaDespesas.Where(d => d.Tipo == TipoDespesaImportacao.FretePIS).FirstOrDefault();
                despesaFreteCofinsEncontrada = listaDespesas.Where(d => d.Tipo == TipoDespesaImportacao.FreteCOFINS).FirstOrDefault();
                despesaFreteIcmsEncontrada = listaDespesas.Where(d => d.Tipo == TipoDespesaImportacao.FreteICMS).FirstOrDefault();

            }

            if (docNumNFArmazenagem > 0)
            {
                docEntryNFArmazenagem = Convert.ToInt32(CrudController.ExecuteScalar(String.Format(SQL.NotaEntrada_GetByDocNum, docNumNFArmazenagem)));
                notaFiscalArmazenagem.GetByKey(docEntryNFArmazenagem);
                recordSetNFArmazenagem.DoQuery(String.Format(SQL.NotaEntrada_DespesasImportacao, docNumNFArmazenagem));
                valorArmazenagem = (double)recordSetNFArmazenagem.Fields.Item("DocTotal").Value;
                notaFiscalArmazenagem.Lines.SetCurrentLine(0);
                despesaArmazenagemEncontrada = listaDespesas.Where(d => d.CodigoItem == notaFiscalArmazenagem.Lines.ItemCode).FirstOrDefault();
                SerialNFArmazenagem = recordSetNFArmazenagem.Fields.Item("Serial").Value.ToString();
            }

            if (DocNumNFServicoImportacao > 0)
            {
                docEntryNFServicoImportacao = Convert.ToInt32(CrudController.ExecuteScalar(String.Format(SQL.NotaEntrada_GetByDocNum, DocNumNFServicoImportacao)));
                notaFiscalServicoImportacao.GetByKey(docEntryNFServicoImportacao);
                recordSetNFServicoImportacao.DoQuery(String.Format(SQL.NotaFiscaEntradalBuscarImpostos, DocNumNFServicoImportacao));

                notaFiscalServicoImportacao.Lines.SetCurrentLine(0);
                despesaServicoImportacaoEncontrada = listaDespesas.Where(d => d.CodigoItem == notaFiscalServicoImportacao.Lines.ItemCode).FirstOrDefault();

                despesaServicoAImportacaoPisEncontrada = listaDespesas.Where(d => d.Tipo == TipoDespesaImportacao.ServicoAssImportacaoPIS).FirstOrDefault();
                despesaServicoAImportacaoCofinsEncontrada = listaDespesas.Where(d => d.Tipo == TipoDespesaImportacao.ServicoAssImportacaoCOFINS).FirstOrDefault();

                SerialServicoImportacao = recordSetNFServicoImportacao.Fields.Item("Serial").Value.ToString();

                Recordset recordSetServico = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSetServico.DoQuery(String.Format(SQL.NotaEntrada_DespesasImportacao, DocNumNFServicoImportacao));
                valorServicoImportacao = (double)recordSetServico.Fields.Item("Total").Value;

                List<ValoresImpostos> valores = new List<ValoresImpostos>();

                for (int i = 0; i < recordSetNFServicoImportacao.RecordCount; i++)
                {
                    ValoresImpostos valor = new ValoresImpostos();
                    valor.Nome = recordSetNFServicoImportacao.Fields.Item("Name").Value.ToString();
                    valor.ValorImposto = (double)recordSetNFServicoImportacao.Fields.Item("TaxSum").Value;
                    valores.Add(valor);
                    recordSetNFServicoImportacao.MoveNext();
                }

                if (valores.Count > 0)
                {
                    valorImpostoServicoAssImportacaoPIS = valores.Where(d => d.Nome == "PIS").FirstOrDefault().ValorImposto;
                    valorImpostoServicoAssImportacaoCOFINS = valores.Where(d => d.Nome == "COFINS").FirstOrDefault().ValorImposto;
                }
            }

            //Criando Despesa de Importação, cabeçalho
            docDespesaImportacao.DueDate = DataDespesas;
            docDespesaImportacao.PostingDate = DataDespesas;
            docDespesaImportacao.Series = 23;
            docDespesaImportacao.VendorCode = notaFiscalProduto.CardCode;
            docDespesaImportacao.BillofLadingNumber = SerialNFFrete + SerialNFArmazenagem + SerialServicoImportacao;

            try
            {
                //Adiciona as linhas dos itens da NF
                for (int i = 0; i < notaFiscalProduto.Lines.Count; i++)
                {
                    notaFiscalProduto.Lines.SetCurrentLine(i);
                    linhaDespesaImportacao = docDespesaImportacao.LandedCost_ItemLines.Add();
                    linhaDespesaImportacao.BaseDocumentType = LandedCostBaseDocumentTypeEnum.asPurchaseInvoice;
                    linhaDespesaImportacao.BaseEntry = notaFiscalProduto.DocEntry;
                    linhaDespesaImportacao.BaseLine = notaFiscalProduto.Lines.LineNum;
                    linhaDespesaImportacao.Quantity = notaFiscalProduto.Lines.Quantity;
                    linhaDespesaImportacao.Warehouse = notaFiscalProduto.Lines.WarehouseCode;
                }

                //Adiciona as linhas dos custos das despesas
                foreach (var despesaSap in listaDespesas)
                {
                    switch (despesaSap.Tipo)
                    {
                        case TipoDespesaImportacao.Vazio:
                            break;
                        case TipoDespesaImportacao.Frete:
                            if (despesaFreteEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao0 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao0.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao0.amount = valorFrete;
                                linhaCustoDespesaImportacao0.AmountFC = valorFrete;
                                existeCusto = true;
                            }
                            break;
                        case TipoDespesaImportacao.Armazenagem:
                            if (despesaArmazenagemEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao1 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao1.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao1.amount = valorArmazenagem;
                                linhaCustoDespesaImportacao1.AmountFC = valorArmazenagem;
                                existeCusto = true;
                            }
                            break;
                        case TipoDespesaImportacao.ServicoAssImportacao:
                            if (despesaServicoImportacaoEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao2 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao2.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao2.amount = valorServicoImportacao;
                                linhaCustoDespesaImportacao2.AmountFC = valorServicoImportacao;
                                existeCusto = true;
                            }
                            break;
                        case TipoDespesaImportacao.FretePIS:
                            if (despesaFretePisEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao3 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao3.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao3.amount = valorImpostoFretePIS;
                                linhaCustoDespesaImportacao3.AmountFC = valorImpostoFretePIS;
                                existeCusto = true;
                            }
                            break;
                        case TipoDespesaImportacao.FreteICMS:
                            if (despesaFreteIcmsEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao4 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao4.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao4.amount = valorImpostoFreteICMS;
                                linhaCustoDespesaImportacao4.AmountFC = valorImpostoFreteICMS;
                                existeCusto = true;
                            }
                            break;
                        case TipoDespesaImportacao.FreteCOFINS:
                            if (despesaFreteCofinsEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao5 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao5.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao5.amount = valorImpostoFreteCOFINS;
                                linhaCustoDespesaImportacao5.AmountFC = valorImpostoFreteCOFINS;
                                existeCusto = true;
                            }
                            break;
                        case TipoDespesaImportacao.ServicoAssImportacaoPIS:
                            if (despesaServicoAImportacaoPisEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao6 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao6.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao6.amount = valorImpostoServicoAssImportacaoPIS;
                                linhaCustoDespesaImportacao6.AmountFC = valorImpostoServicoAssImportacaoPIS;
                                existeCusto = true;
                            }
                            break;
                        case TipoDespesaImportacao.ServicoAssImportacaoCOFINS:
                            if (despesaServicoAImportacaoCofinsEncontrada.CodigoDespesa == despesaSap.CodigoDespesa)
                            {
                                LandedCost_CostLine linhaCustoDespesaImportacao7 = (LandedCost_CostLine)docDespesaImportacao.LandedCost_CostLines.Add();
                                linhaCustoDespesaImportacao7.LandedCostCode = despesaSap.CodigoDespesa;
                                linhaCustoDespesaImportacao7.amount = valorImpostoServicoAssImportacaoCOFINS;
                                linhaCustoDespesaImportacao7.AmountFC = valorImpostoServicoAssImportacaoCOFINS;
                                existeCusto = true;
                            }
                            break;
                        default:
                            break;
                    }

                }

                if (existeCusto)
                {
                    SAPbobsCOM.LandedCostParams oLandedCostParams = (SAPbobsCOM.LandedCostParams)landedCostsService.GetDataInterface(SAPbobsCOM.LandedCostsServiceDataInterfaces.lcsLandedCostParams);
                    oLandedCostParams = landedCostsService.AddLandedCost(docDespesaImportacao);
                    mensagemRetorno.DocNum = oLandedCostParams.LandedCostNumber;
                }
                else
                {
                    string frete = string.Empty;
                    string armazenagem = string.Empty;
                    string servico = string.Empty;

                    mensagemRetorno.erro = true;

                    if (docNumNFCTeFrete == 0)
                        frete = "-";
                    else
                        frete = docNumNFCTeFrete.ToString();

                    if (docNumNFArmazenagem == 0)
                        armazenagem = "-";
                    else
                        armazenagem = docNumNFArmazenagem.ToString();

                    if (DocNumNFServicoImportacao == 0)
                        servico = "-";
                    else
                        servico = DocNumNFServicoImportacao.ToString();

                    string notas = $"Frete:{frete} Armazenagem:{armazenagem} Serviço Ass. de Importação:{servico}";

                    mensagemRetorno.mensagemErro = $"Não existe(m) item(ns) de despesas de importação na Nota(s): {notas}";

                    return mensagemRetorno;
                }
            }
            catch (Exception ex)
            {
                mensagemRetorno.erro = true;
                mensagemRetorno.mensagemErro = ex.Message;
            }
            finally
            {
                Marshal.ReleaseComObject(landedCostsService);
                Marshal.ReleaseComObject(docDespesaImportacao);
                Marshal.ReleaseComObject(linhaDespesaImportacao);
                Marshal.ReleaseComObject(notaFiscalProduto);
                Marshal.ReleaseComObject(notaFiscalFrete);
                Marshal.ReleaseComObject(notaFiscalArmazenagem);
                Marshal.ReleaseComObject(notaFiscalServicoImportacao);
                Marshal.ReleaseComObject(recordSetNFCteFrete);
                Marshal.ReleaseComObject(recordSetDespesasImportacao);
                Marshal.ReleaseComObject(recordSetNFArmazenagem);
                Marshal.ReleaseComObject(recordSetNFServicoImportacao);
                landedCostsService = null;
                docDespesaImportacao = null;
                linhaDespesaImportacao = null;
                notaFiscalProduto = null;
                recordSetNFCteFrete = null;
                notaFiscalFrete = null;
                notaFiscalArmazenagem = null;
                notaFiscalServicoImportacao = null;
            }

            return mensagemRetorno;
        }

        public static List<DespesasImportacao> CarregarDespesasImportacaoSap()
        {
            Recordset recordSetDespesasImportacao = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordSetDespesasImportacao.DoQuery(String.Format(SQL.BuscarDespesasImportacao));
            List<DespesasImportacao> listaDespesas = new List<DespesasImportacao>();

            try
            {
                for (int i = 0; i < recordSetDespesasImportacao.RecordCount; i++)
                {
                    DespesasImportacao despesasImportacao = new DespesasImportacao();
                    despesasImportacao.CodigoDespesa = recordSetDespesasImportacao.Fields.Item("AlcCode").Value.ToString();
                    despesasImportacao.Nome = recordSetDespesasImportacao.Fields.Item("AlcName").Value.ToString();
                    despesasImportacao.ContaContabil = recordSetDespesasImportacao.Fields.Item("LaCAllcAcc").Value.ToString();
                    despesasImportacao.CodigoItem = recordSetDespesasImportacao.Fields.Item("U_CVA_CodItem").Value.ToString();
                    int.TryParse(recordSetDespesasImportacao.Fields.Item("U_CVA_TipoDespesa").Value.ToString(), out int tipo);

                    switch (tipo)
                    {
                        case 1:
                            despesasImportacao.Tipo = TipoDespesaImportacao.Frete;
                            break;
                        case 2:
                            despesasImportacao.Tipo = TipoDespesaImportacao.Armazenagem;
                            break;
                        case 3:
                            despesasImportacao.Tipo = TipoDespesaImportacao.ServicoAssImportacao;
                            break;
                        case 4:
                            despesasImportacao.Tipo = TipoDespesaImportacao.FretePIS;
                            break;
                        case 5:
                            despesasImportacao.Tipo = TipoDespesaImportacao.FreteICMS;
                            break;
                        case 6:
                            despesasImportacao.Tipo = TipoDespesaImportacao.FreteCOFINS;
                            break;
                        case 7:
                            despesasImportacao.Tipo = TipoDespesaImportacao.ServicoAssImportacaoPIS;
                            break;
                        case 8:
                            despesasImportacao.Tipo = TipoDespesaImportacao.ServicoAssImportacaoCOFINS;
                            break;
                        case 0:
                            despesasImportacao.Tipo = TipoDespesaImportacao.Vazio;
                            break;
                    }

                    listaDespesas.Add(despesasImportacao);

                    recordSetDespesasImportacao.MoveNext();
                }
            }
            catch (Exception) { }

            return listaDespesas;
        }
    }

    public struct DespesasImportacao
    {
        public string Nome { get; set; }
        public string CodigoDespesa { get; set; }
        public string ContaContabil { get; set; }
        public string CodigoItem { get; set; }
        public TipoDespesaImportacao Tipo { get; set; }
    }

    public struct ValoresImpostos
    {
        public string Nome { get; set; }
        public double ValorImposto { get; set; }
    }

    public enum TipoDespesaImportacao
    {
        Vazio = 0,
        Frete = 1,
        Armazenagem = 2,
        ServicoAssImportacao = 3,
        FretePIS = 4,
        FreteICMS = 5,
        FreteCOFINS = 6,
        ServicoAssImportacaoPIS = 7,
        ServicoAssImportacaoCOFINS = 8
    }
}
