using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B1SLayer;

namespace CVA.Portal.Producao.BLL.Apetit
{
    public class SaidaMateriaisBLL : BaseBLL
    {

        public List<ComboBoxModelHANA> GetInsumos(string BPLId)
        {
            try
            {
                return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("SaidaMateriais_ItensInsumo"), Database, BPLId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ComboBoxModelHANA> GetInsumosInvSell(string BPLId)
        {
            try
            {
                return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("SaidaMateriais_ItensInsumoSellInv"), Database, BPLId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ComboBoxModelHANA> GetInsumosReposicao(string BPLId)
        {
            try
            {
                return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("Reposicao_ItensInsumo"), Database, BPLId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ComboBoxModelHANA> GetMotivo(string BPLId)
        {
            try
            {
                return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("ReposicaoInsumo_Motivo"), Database));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public SaidaInsumoModelGetItemOnHand GetItemOnHand(string itemCode, string whs)
        {
            try
            {
                return DAO.FillModelFromCommand<SaidaInsumoModelGetItemOnHand>(string.Format(Commands.Resource.GetString("SaidaMateriais_ItemOnHand"), Database, itemCode, whs));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<SaidaInsumoItensModel02> ReportPosicaoEstoque(string rptBPLId)
        {
            try
            {
                return DAO.FillListFromCommand<SaidaInsumoItensModel02>(string.Format(Commands.Resource.GetString("SaidaMateriais_ReportPosicaoEstoque"), Database, rptBPLId));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public SaidaInsumoModelGetItemOnHand GetItemOnHandBPLId(string itemCode, string bplId)
        {
            try
            {
                return DAO.FillModelFromCommand<SaidaInsumoModelGetItemOnHand>(string.Format(Commands.Resource.GetString("SaidaMateriais_ItemOnHandBPLId"), Database, itemCode, bplId));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> SaveSL(SaidaInsumoModel model)
        {
            model.Itens = model.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();

            string retorno = String.Empty;

            if (model.Itens.Count > 0)
            {
                var bllFilial = new FilialBLL();
                var bllLotes = new BatchesBLL();

                SaidaInsumoModelGetOBPL objFilial = null;
                var saidaInsumosModel = new DocumentoMarketingInsumoModel();
                saidaInsumosModel.DocDate = DateTime.Today.ToString("yyyy-MM-dd");
                saidaInsumosModel.TaxDate = DateTime.Today.ToString("yyyy-MM-dd");
                saidaInsumosModel.Comments = model.Motivo;
                saidaInsumosModel.JournalMemo = "Saída de Materiais – Cliente: " + model.ClienteCode;

                objFilial = bllFilial.GetSaidaInsumoOBPL(model.BPLIdCode);

                saidaInsumosModel.BPL_IDAssignedToInvoice = Convert.ToInt32(model.BPLIdCode);
                
                saidaInsumosModel.DocumentLines = new List<DocumentInsumoline>();

                foreach (var item in model.Itens)
                {
                    var lineModel = new DocumentInsumoline();
                    lineModel.ItemCode = item.InsumoCode;
                    lineModel.Quantity = item.Qty;
                    
                    lineModel.WhsCode = objFilial.DflWhs;
                    //lineModel.U_CVA_TpAjuste = model.TipoSaidaCode;
                    lineModel.U_CVA_TipoSaida = model.TipoSaidaCode;
                    lineModel.CostingCode = objFilial.U_CVA_Dim1Custo;
                    
                    if (!string.IsNullOrEmpty(objFilial.U_CVA_Dim2Custo))
                        lineModel.CostingCode2 = objFilial.U_CVA_Dim2Custo;

                    if (bllLotes.BatchControl(item.InsumoCode).ManBtchNum == "Y")
                    {
                        lineModel.BatchNumbers = new List<Batchnumber>();
                        var listOfBatches = bllLotes.GetItemBatches(item.InsumoCode, lineModel.WhsCode);

                        double TotalItensJaRegistrados = 0;
                        foreach (var batch in listOfBatches)
                        {
                            double QuantidadeCalculadaLoteLinha = 0;

                            if (TotalItensJaRegistrados == item.Qty) break;

                            var qtyFaltante = item.Qty - TotalItensJaRegistrados;

                            if (qtyFaltante > 0)
                            {
                                if (batch.Quantity <= qtyFaltante)
                                    QuantidadeCalculadaLoteLinha = batch.Quantity;
                                else if (batch.Quantity > qtyFaltante)
                                    QuantidadeCalculadaLoteLinha = qtyFaltante;
                            }

                            if (QuantidadeCalculadaLoteLinha <= 0) break;

                            TotalItensJaRegistrados = TotalItensJaRegistrados + QuantidadeCalculadaLoteLinha;

                            Batchnumber batchModel = new Batchnumber();
                            batchModel.BatchNumber = batch.DistNumber;
                            batchModel.Quantity = QuantidadeCalculadaLoteLinha;
                            lineModel.BatchNumbers.Add(batchModel);
                        }
                    }

                    saidaInsumosModel.DocumentLines.Add(lineModel);
                }

                try
                {
                    await SLUtil.Connection.Request("InventoryGenExits").WithReturnNoContent().PostAsync(saidaInsumosModel);
                } catch (Exception ex)
                {
                    return "Erro ao gerar saída de insumos: " + ex.Message;
                }
            }

            return "";
        }
    }
}
