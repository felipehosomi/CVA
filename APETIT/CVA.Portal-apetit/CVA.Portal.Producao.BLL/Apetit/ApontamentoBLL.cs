using B1SLayer;
using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.BLL.Util;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Configuracoes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using static CVA.Portal.Producao.Model.MessagesModel;
using static CVA.Portal.Producao.Model.PainelApontamentoModel;

namespace CVA.Portal.Producao.BLL
{
    public class ApontamentoBLL : BaseBLL
    {
        UsuarioBLL BLLUsuario = new UsuarioBLL();
        ColaboradorBLL BLLColaborador = new ColaboradorBLL();
        MessagesBLL BLLMessages = new MessagesBLL();

        private static SemaphoreSlim _semaphoreOrdemProducao = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim _semaphoreSaveTerceiros = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim _semaphoreSaveInventoryGenExits = new SemaphoreSlim(1, 1);
        private static SemaphoreSlim _semaphoreSaveInventoryGenEntries = new SemaphoreSlim(1, 1);


        public ApontamentoGetClienteModel GetCliente(string clienteBPLID)
        {
            try
            {
                return DAO.FillModelFromCommand<ApontamentoGetClienteModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetCliente"), Database, clienteBPLID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApontamentoGetServicoModel> GetGrupoServico(string contratoId)
        {
            try
            {
                return DAO.FillListFromCommand<ApontamentoGetServicoModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetGrupoServico"), Database, contratoId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public ApontamentoGetInfoContratoModel GetInfoContrato(string contratoId)
        {
            try
            {
                var modal = DAO.FillModelFromCommand<ApontamentoGetInfoContratoModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetInfoContrato"), Database, contratoId));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApontamentoItemList> GetItensList(string itemCodeList, string BPLIdList)
        {
            try
            {
                return DAO.FillListFromCommand<ApontamentoItemList>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_ItensList"), Database, BPLIdList));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public CVA_APTO_TERCEIROSModel.APIModel GetTerceiros(string tBPLID, DateTime? tData, string tTurno, string tServico)
        {
            try
            {
                var split = tTurno.Split('|'); 
                var modal = DAO.FillModelFromCommand<CVA_APTO_TERCEIROSModel.CVA_APTO_TerceirosSQL>(string.Format(Commands.Resource.GetString("Terceiros_Get"), Database, tBPLID, tData.Value.ToString("yyyy-MM-dd"), split[1], tServico));
                
                if(modal!=null && !string.IsNullOrEmpty(modal.Code))
                {
                    var obj = new CVA_APTO_TERCEIROSModel.APIModel()
                    {
                        Code = modal.Code,
                        Name = modal.Name,
                        U_DATA = modal.U_DATA,
                        U_FILIAL = modal.U_FILIAL,
                        U_SERVICO = modal.U_SERVICO,
                        U_TURNO = modal.U_TURNO,
                        U_QTYPLAN = modal.U_QTYPLAN,
                        U_QTYREF = modal.U_QTYREF,
                        U_USERPORTAL = modal.U_USERPORTAL
                    };

                    obj.CVA_APTO_TERCEIROS1Collection = DAO.FillListFromCommand<CVA_APTO_TERCEIROSModel.CVA_APTO_Terceiros1collectionSQL>(string.Format(Commands.Resource.GetString("Terceiros_Line_Get"), Database, modal.Code)); ;

                    return obj;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ApontamentoGetItemOnHand GetItemOnHandBPLId(string itemCode, string bplId)
        {
            try
            {
                return DAO.FillModelFromCommand<ApontamentoGetItemOnHand>(string.Format(Commands.Resource.GetString("Apontamento_ItemOnHandBPLId"), Database, itemCode, bplId));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ApontamentoGetClienteModel GetTerceirosBPCode(string bCardCode)
        {
            try
            {
                return DAO.FillModelFromCommand<ApontamentoGetClienteModel>(string.Format(Commands.Resource.GetString("Terceiros_ClienteCode"), Database, bCardCode));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CVA_APTO_TERCEIROSModel.CVA_APTO_TerceirosSAP> GetTerceirosBP(string bBPLID)
        {
            try
            {
                return DAO.FillListFromCommand<CVA_APTO_TERCEIROSModel.CVA_APTO_TerceirosSAP>(string.Format(Commands.Resource.GetString("Terceiros_GetAll_SAP"), Database, bBPLID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ApontamentoItemInfo GetItemInfo(string infoItem, string infoBPLID)
        {
            try
            {
                var listMotivo = GetMotivoApontamento();
               
                var modal = DAO.FillModelFromCommand<ApontamentoItemInfo>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_ItemInfo"), Database, infoItem, infoBPLID));
                
                var stringMotivo = string.Empty;
                foreach (var itemMotivo in listMotivo)
                    stringMotivo += $"<option value=\"{itemMotivo.CODE}\">{itemMotivo.NAME}</option>";

                modal.MotivoList = stringMotivo;

                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApontamentoGetServicoModel> GetServico(string grupoServicoId)
        {
            try
            {
                var split = grupoServicoId.Split('|');

                return DAO.FillListFromCommand<ApontamentoGetServicoModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetServico"), Database, split[1], split[0]));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApontamentoGetInfoItensContratoModel> GetInfoPlanejamentoItens(string contratoId, string servicoId)
        {
            try
            {
                var listMotivo = GetMotivoApontamento();
                
                var split = contratoId.Split('|');
                var model = DAO.FillListFromCommand<ApontamentoGetInfoItensContratoModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetInfoItensContrato"), Database, split[0], servicoId));

                if (listMotivo != null && listMotivo.Count > 0)
                {
                    var stringMotivo = string.Empty;

                    foreach (var itemMotivo in listMotivo)
                        stringMotivo += $"<option value=\"{itemMotivo.CODE}\">{itemMotivo.NAME}</option>";

                    model.ForEach(x => {
                        x.MotivoList = stringMotivo;
                        x.ProdName = x.ProdItem + " - " + x.ProdName;
                        x.QtySaldoHidden = x.QtySaldo;
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApontamentoGetInfoItensContratoModel> GetInfoPlanejamentoItensFechado(string contratoId, string servicoId)
        {
            try
            {
                var listMotivo = GetMotivoApontamento();

                var split = contratoId.Split('|');
                var model = DAO.FillListFromCommand<ApontamentoGetInfoItensContratoModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetInfoItensContratoFechado"), Database, split[0], servicoId));

                if (listMotivo != null && listMotivo.Count > 0)
                {
                    var stringMotivo = string.Empty;

                    foreach (var itemMotivo in listMotivo)
                        stringMotivo += $"<option value=\"{itemMotivo.CODE}\">{itemMotivo.NAME}</option>";

                    model.ForEach(x => {
                        x.MotivoList = stringMotivo;
                        x.ProdName = x.ProdItem + " - " + x.ProdName;
                        x.QtySaldoHidden = x.QtySaldo;
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApontamentoGetQtyModel GetQty(string qtySERVICO, string qtyTURNO, DateTime date, string qtyBPLID)
        {
            try
            {
                var split = qtyTURNO.Split('|');
                var modal = DAO.FillModelFromCommand<ApontamentoGetQtyModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetQty"), Database, qtySERVICO, split[1].ToString(), date.ToString("yyyy-MM-dd"), qtyBPLID, split[0].ToString()));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApontamentoGetNextKeyModel GetNextKeyApontamento()
        {
            try
            {
                var modal = DAO.FillModelFromCommand<ApontamentoGetNextKeyModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_Key"), Database));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApontamentoGetNextKeyModel GetNextKeyApontamentoLinhas()
        {
            try
            {
                var modal = DAO.FillModelFromCommand<ApontamentoGetNextKeyModel>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_Linhas_Key"), Database));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApontamentoStatusUser GetStatusUser(string userCode)
        {
            try
            {
                var modal = DAO.FillModelFromCommand<ApontamentoStatusUser>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_StatusUser"), Database, userCode));
                return modal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<string> SendEmailQtyZeroAsync(ApontamentoModel model)
        {
            var messageModel = new MessagesModel();
            messageModel.Subject = "Dia sem consumo –  " + model.apontamentoData.ToString("dd-MM-yyyy") + " – Nº " + model.ContratoCode;

            messageModel.Text += "Cliente : " + model.ClienteCode + " - " + model.ClienteName + Environment.NewLine;
            messageModel.Text += "Motivo  : Observação do Apontamento" + Environment.NewLine;
            messageModel.Text += "Apontamento pelo usuário : " + model.UserCode + ".";

            var columnContrato = new Messagedatacolumn();
            columnContrato.ColumnName = "Contrato";
            columnContrato.Link = "tYES";

            var columnCliente = new Messagedatacolumn();
            columnCliente.ColumnName = "Cliente";
            columnCliente.Link = "tYES";

            columnContrato.MessageDataLines.Add(new Messagedataline() { Value = model.ContratoCode, Object = "1250000025", ObjectKey = model.ContratoCode });
            columnCliente.MessageDataLines.Add(new Messagedataline() { Value = model.ClienteCode, Object = "2", ObjectKey = model.ClienteCode });

            messageModel.MessageDataColumns.Add(columnContrato);
            messageModel.MessageDataColumns.Add(columnCliente);

            var userID = BLLUsuario.GetUserID(model.UserCode);
            messageModel.User = userID.USERID;

            var listEmployees = BLLColaborador.GetPCPGerente(model.UserCode);
            if (listEmployees != null && listEmployees?.Count > 0)
            {
                foreach (var employees in listEmployees.Where(x => x.DEP == "PCP"))
                {
                    messageModel.RecipientCollection.Add(new Recipientcollection()
                    {
                        UserCode = employees.CODE,
                        UserType = "rt_InternalUser",
                        NameTo = employees.CODE,
                        SendEmail = (!string.IsNullOrEmpty(employees.EMAIL) ? "tYES" : "tNO"),
                        EmailAddress = (!string.IsNullOrEmpty(employees.EMAIL) ? employees.EMAIL : ""),
                        SendSMS = "tNO",
                        CellularNumber = "",
                        SendFax = "tNO",
                        FaxNumber = "",
                        SendInternal = "tYES"
                    });
                }
            }

            return await BLLMessages.SendMessage(messageModel);
        }

        public bool CheckQtyDiff(double qtyRef, double percent, double qtyCns)
        {
            //(qtyRefeicoes * percentItem) / 100
            var calc = (qtyRef * percent) / 100;
            return (calc != qtyCns);
        }
        
        public List<PainelContratoItens> GetPainel(string pnBplId, DateTime dateDe, DateTime dateAte)
        {
            try
            {
                return DAO.FillListFromCommand<PainelContratoItens>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_GetPainel"), Database, pnBplId, dateDe.ToString("yyyy-MM-dd"), dateAte.ToString("yyyy-MM-dd")));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> SendEmailComplementacao(string cpFilial, string cpCONTRATO, string cpCliente, string cpSERVICO, DateTime date, string cpUser)
        {
            var messageModel = new MessagesModel();
            messageModel.Subject = "Complementação de Apontamento –  " + date.ToString("dd-MM-yyyy") + " – Nº " + cpCONTRATO;

            messageModel.Text += "Apontamento pelo usuário : " + cpUser + ".";

            var columnContrato = new Messagedatacolumn();
            columnContrato.ColumnName = "Contrato";
            columnContrato.Link = "tYES";

            var columnCliente = new Messagedatacolumn();
            columnCliente.ColumnName = "Cliente";
            columnCliente.Link = "tYES";

            var columnData = new Messagedatacolumn();
            columnData.ColumnName = "Data";
            columnData.Link = "tNO";

            var columnServico = new Messagedatacolumn();
            columnServico.ColumnName = "Servico";
            columnServico.Link = "tNO";

            columnContrato.MessageDataLines.Add(new Messagedataline() { Value = cpCONTRATO, Object = "1250000025", ObjectKey = cpCONTRATO });
            columnCliente.MessageDataLines.Add(new Messagedataline() { Value = cpCliente, Object = "2", ObjectKey = cpCliente });
            columnData.MessageDataLines.Add(new Messagedataline() { Value = date.ToString("dd-MM-yyyy") });
            columnServico.MessageDataLines.Add(new Messagedataline() { Value = cpSERVICO });

            messageModel.MessageDataColumns.Add(columnContrato);
            messageModel.MessageDataColumns.Add(columnCliente);
            messageModel.MessageDataColumns.Add(columnData);
            messageModel.MessageDataColumns.Add(columnServico);

            var userID = BLLUsuario.GetUserID(cpUser);
            messageModel.User = userID.USERID;

            var listEmployees = BLLColaborador.GetPCPGerente(cpUser);
            if (listEmployees != null && listEmployees?.Count > 0)
            {
                foreach (var employees in listEmployees.Where(x => x.DEP == "PCP"))
                {
                    messageModel.RecipientCollection.Add(new Recipientcollection()
                    {
                        UserCode = employees.CODE,
                        UserType = "rt_InternalUser",
                        NameTo = employees.CODE,
                        //SendEmail = (!string.IsNullOrEmpty(employees.EMAIL) ? "tYES" : "tNO"),
                        //EmailAddress = (!string.IsNullOrEmpty(employees.EMAIL) ? employees.EMAIL : ""),
                        SendSMS = "tNO",
                        CellularNumber = "",
                        SendFax = "tNO",
                        FaxNumber = "",
                        SendInternal = "tYES"
                    });
                }
            }

            return await BLLMessages.SendMessage(messageModel);
        }
        
        public async Task<string> ProductionOrderAsync(ApontamentoEncerramentoModel model)
        {
            var modelNew = new ProductionOrderModel();

            ServiceLayerUtil sl = new ServiceLayerUtil();
            await sl.Login();
            string retorno = string.Empty;

            retorno = await sl.PostAsync("ProductionOrders", modelNew);

            if (!string.IsNullOrEmpty(retorno))
                retorno = "Erro ao gerar ordem de produção: " + retorno;

            return retorno;
        }

        public List<ComboBoxModelHANA> GetMotivoApontamento()
        {
            try
            {
                var modal = DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_Motivo"), Database));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveValidation(ref ApontamentoModel model)
        {
            if (model?.Itens?.Count > 0)
            {
                model.Itens = model.Itens.Where(x => x.Delete == false).ToList();
                model.Itens.RemoveAll(x => x.Delete == true);
            }

            if (model?.BPs?.Count > 0)
            {
                model.BPs = model.BPs.Where(x => x.Remove == false).ToList();
                model.BPs.RemoveAll(x => x.Remove == true);
            }

            //72hs
            var dtNow = DateTime.Now;
            if (model.apontamentoData.Hour == 0)
                model.apontamentoData = model.apontamentoData.AddHours(dtNow.Hour);

            if (model.apontamentoData.Minute == 0)
                model.apontamentoData = model.apontamentoData.AddMinutes(dtNow.Minute);

            if (model.apontamentoData.Second == 0)
                model.apontamentoData = model.apontamentoData.AddSeconds(dtNow.Second);

            var statusUser = GetStatusUser(model.UserCode);
            if (statusUser != null && statusUser.U_CVA_BLOQUEIO_APTO == "Y")
               return "Usuário esta bloqueado para realizar os apontamentos!";

            if (statusUser != null && statusUser.U_CVA_VALDTMP_APTO == "Y")
            {
                TimeSpan diff = dtNow - model.apontamentoData;
                if (diff.TotalHours > 72)
                    return "Bloquear apontamentos com mais de 72 horas!";

                var retDay = CheckDay(model.apontamentoData);
                if (retDay.LASTDAY != null && retDay.YESTERDAY == 0)
                    return $"Favor realizar o apontamento dos dias anteriores, ultimo dia apontado foi {retDay.LASTDAY.Value.ToString("dd/MM/yyyy")}";
            }

            if (CheckDuplicate(model.OrderCode) > 0)
            {
                return "Apontamento já foi realizado.";
            }

            return "";
        }

        public async Task<string> SaveAsync(ApontamentoModel model)
        {
            try
            {
                string validationString = SaveValidation(ref model);
                if (!string.IsNullOrEmpty(validationString))
                {
                    return validationString;
                }

                ProductionOrdersModel retOrderModel = await GetProdutionOrder(model.OrderCode);

                if (model.Itens.Any(x => x.Tipo.Equals("Substituto") || x.Tipo.Equals("[+]"))) //Mudança na ordem de produção
                {
                    if (retOrderModel != null)
                    {
                        string ItensSubs = "";

                        foreach (var itemTo in model.Itens.Where(x => x.Tipo.Equals("Substituto")))
                        {
                            foreach (var itemChange in retOrderModel.ProductionOrderLines.Where(x => x.LineNumber == itemTo.LineNum))
                            {
                                itemChange.ItemNo = itemTo.ItemCode;
                                itemChange.U_CVA_Substituto = itemTo.ItemCodeChange;
                                itemChange.U_CVA_SubMotivo = itemTo.Motivo;
                                itemChange.U_CVA_SubJust = itemTo.Justificativa;

                                ItensSubs += itemTo.ItemCode + ", ";
                            }
                        }

                        string ItensAdd = "";

                        foreach (var itemTo in model.Itens.Where(x => x.Tipo.Equals("[+]")))
                        {
                            if (retOrderModel.ProductionOrderLines.Where(x => x.ItemNo == itemTo.ItemCode && x.U_CVA_SubMotivo == itemTo.Motivo && x.U_CVA_SubJust == itemTo.Justificativa).Count() == 0)
                            {
                                var itemBase = retOrderModel.ProductionOrderLines.First();
                                var itemNew = new ProductionOrderLineModel();
                                itemNew.ItemType = "pit_Item";
                                itemNew.DocumentAbsoluteEntry = Convert.ToInt32(model.OrderCode);
                                itemTo.LineNum = itemNew.LineNumber = itemNew.VisualOrder = (model.Itens.Count() == 0 ? 0 : model.Itens.Max(x => x.LineNum) + 1);

                                itemNew.ProductionOrderIssueType = "im_Manual";
                                itemNew.ItemNo = itemTo.ItemCode;
                                itemNew.PlannedQuantity = Convert.ToSingle(itemTo.QtyPlanejado);

                                itemNew.Warehouse = itemBase.Warehouse;
                                itemNew.StartDate = itemBase.StartDate;
                                itemNew.EndDate = itemBase.EndDate;

                                itemNew.U_CVA_SubMotivo = itemTo.Motivo;
                                itemNew.U_CVA_SubJust = itemTo.Justificativa;

                                retOrderModel.ProductionOrderLines.Add(itemNew);

                                ItensAdd += itemTo.ItemCode + ", ";
                            } else
                            {
                                itemTo.LineNum = (model.Itens.Count() == 0 ? 0 : model.Itens.Max(x => x.LineNum) + 1);
                            }
                        }

                        if (ItensSubs.Length > 0 || ItensAdd.Length > 0)
                        {
                            var retSaveAddOrSub = await SaveProdutionOrder(retOrderModel);

                            if (!string.IsNullOrEmpty(retSaveAddOrSub))
                            {
                                return $"Falha ao adicionar ou substituir itens da OP {retOrderModel.DocumentNumber}: " + retSaveAddOrSub;
                            }
                            else
                            {
                                retOrderModel = await GetProdutionOrder(model.OrderCode);
                                try
                                {
                                    Logger.Log($"Foram adicionados ou substituidos os seguintes itens na OP {retOrderModel.DocumentNumber}" +
                                            $"{(ItensSubs.Length > 0 ? $" - Substituídos: {ItensSubs.Substring(0, ItensSubs.Length - 2)}" : "")}" +
                                            $"{(ItensAdd.Length > 0 ? $" - Adicionados:{ItensAdd.Substring(0, ItensAdd.Length - 2)}" : "")}");
                                }
                                catch { }
                            }
                        }   
                    }
                    else
                    {
                        return $"Não foi encontrado a ordem de produção, DocEntry: {model.OrderCode}.";
                    }
                }

                if (model.Itens.Sum(x => x.QtyUtilizada) != 0)
                {
                    model.startDate = DAO.FillModelFromCommand<ApontamentoDataInicio>($"SELECT TO_VARCHAR(TO_DATE(\"StartDate\"), 'YYYY-MM-DD') \"startDate\" FROM \"{Database}\".\"OWOR\" WHERE \"DocEntry\" = {model.OrderCode}").startDate;

                    var retSaidaInsumoEntradaAcabado = await SaveInventoryGenExits_GenEntries(model, retOrderModel);

                    if (string.IsNullOrEmpty(retSaidaInsumoEntradaAcabado))
                    {
                        retOrderModel.U_CVA_APO = DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("Apontamento_GetIDApontamento"), Database, model.startDate, model.ContratoCode.Split('|')[1], model.ServicoCode, model.BPLIdCode)).ToString();

                        if (model.BPs.Count > 0 && model.BPs.Any(x => x.BPType != "0"))
                        {
                            model.IdApontamento = retOrderModel.U_CVA_APO;

                            Tuple<bool, string> terceirosSL = await SaveTerceiros(model);

                            if (!terceirosSL.Item1)
                            {
                                return "Falha ao salvar o apontamento de terceiros: " + terceirosSL.Item2;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(retOrderModel.U_CVA_APO) || retOrderModel.U_CVA_APO == "0")
                                {
                                    retOrderModel.U_CVA_APO = terceirosSL.Item2;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(retOrderModel.U_CVA_APO) && retOrderModel.U_CVA_APO != "0")
                        {
                            foreach (var itemTo in model.Itens.Where(x => !string.IsNullOrEmpty(x.Justificativa)))
                            {
                                foreach (var itemChange in retOrderModel.ProductionOrderLines.Where(x => x.LineNumber == itemTo.LineNum))
                                {
                                    itemChange.U_CVA_Substituto = itemTo.ItemCodeChange;
                                    itemChange.U_CVA_SubMotivo = itemTo.Motivo;
                                    itemChange.U_CVA_SubJust = itemTo.Justificativa;
                                }
                            }

                            retOrderModel.ProductionOrderStatus = "boposClosed";

                            if (model.QtySobra > 0)
                                retOrderModel.U_CVA_SOBRA = Convert.ToSingle(model.QtySobra);

                            if (model.QtyResto > 0)
                                retOrderModel.U_CVA_RESTO = Convert.ToSingle(model.QtyResto);

                            retOrderModel.Remarks += Environment.NewLine + "Solicitada pelo usuário : " + model.UserCode;
                            retOrderModel.Remarks += Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                            retOrderModel.Remarks = Comments(retOrderModel.Remarks);

                            string retSaveFinal = "";
                            int contRequisicao = 0;

                            for (int numberOfAttempts = 3; numberOfAttempts > 0; numberOfAttempts--)
                            {
                                retSaveFinal = await SaveProdutionOrder(retOrderModel);

                                //Trata bug da SL, retorno 400 para erro do servidor que deveria ser 500.
                                if (retSaveFinal != "Ocorreu erro interno (-2038)")
                                {
                                    break;
                                }
                                else
                                {
                                    contRequisicao++;
                                    Logger.Log($"OP {retOrderModel.DocumentNumber} não foi fechada ({contRequisicao}ª tentativa), vamos tentar novamente! Motivo: {retSaveFinal}");

                                    await Task.Delay(200);
                                }
                            }

                            //Registrar no log OP não fechadas e o motivo.
                            if (!string.IsNullOrEmpty(retSaveFinal))
                            {
                                Logger.Log($"OP {retOrderModel.DocumentNumber} não foi fechada por este motivo(Ultima tentativa): {retSaveFinal}");
                                return $"Falha ao fechar a ordem de produção {retOrderModel.DocumentNumber}, erro: " + retSaveFinal;
                            }
                            else
                            {
                                Logger.Log($"OP {retOrderModel.DocumentNumber} fechada com sucesso.");
                                return "";
                            }
                        }
                    } else
                    {
                        Logger.Log($"Saída de insumo da OP {retOrderModel.DocumentNumber} não foi realizada por este motivo: {retSaidaInsumoEntradaAcabado}");
                        return $"Falha ao realizar o apontamento da OP {retOrderModel.DocumentNumber}.{Environment.NewLine}Detalhes: {retSaidaInsumoEntradaAcabado}";

                        //Logger.Log($"Saída de insumo ou Entrada do acabado da OP {retOrderModel.DocumentNumber} não foi realizada por este motivo: {retSaidaInsumoEntradaAcabado}");
                        //return $"Falha ao realizar o apontamento da OP {retOrderModel.DocumentNumber}.{Environment.NewLine}Detalhes: {retSaidaInsumoEntradaAcabado}";
                    }
                } else if (model.Itens.Sum(x => x.QtyUtilizada) == 0)
                {
                    model.startDate = DAO.FillModelFromCommand<ApontamentoDataInicio>($"SELECT TO_VARCHAR(TO_DATE(\"StartDate\"), 'YYYY-MM-DD') \"startDate\" FROM \"{Database}\".\"OWOR\" WHERE \"DocEntry\" = {model.OrderCode}").startDate;

                    retOrderModel.U_CVA_APO = DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("Apontamento_GetIDApontamento"), Database, model.startDate, model.ContratoCode.Split('|')[1], model.ServicoCode, model.BPLIdCode)).ToString();

                    if (model.BPs.Count > 0 && model.BPs.Any(x => x.BPType != "0"))
                    {
                        model.IdApontamento = retOrderModel.U_CVA_APO;

                        Tuple<bool, string> terceirosSL = await SaveTerceiros(model);

                        if (!terceirosSL.Item1)
                        {
                            return "Falha ao salvar o apontamento de terceiros: " + terceirosSL.Item2;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(retOrderModel.U_CVA_APO) || retOrderModel.U_CVA_APO == "0")
                            {
                                retOrderModel.U_CVA_APO = terceirosSL.Item2;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(retOrderModel.U_CVA_APO) && retOrderModel.U_CVA_APO != "0")
                    {
                        foreach (var itemTo in model.Itens.Where(x => !string.IsNullOrEmpty(x.Justificativa)))
                        {
                            foreach (var itemChange in retOrderModel.ProductionOrderLines.Where(x => x.LineNumber == itemTo.LineNum))
                            {
                                itemChange.U_CVA_Substituto = itemTo.ItemCodeChange;
                                itemChange.U_CVA_SubMotivo = itemTo.Motivo;
                                itemChange.U_CVA_SubJust = itemTo.Justificativa;
                            }
                        }

                        retOrderModel.U_CVA_APO_ZERO = "Y";
                        retOrderModel.ProductionOrderStatus = "boposClosed";

                        if (model.QtySobra > 0)
                            retOrderModel.U_CVA_SOBRA = Convert.ToSingle(model.QtySobra);

                        if (model.QtyResto > 0)
                            retOrderModel.U_CVA_RESTO = Convert.ToSingle(model.QtyResto);

                        retOrderModel.Remarks += Environment.NewLine + "Solicitada pelo usuário : " + model.UserCode;
                        retOrderModel.Remarks += Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        retOrderModel.Remarks = Comments(retOrderModel.Remarks);

                        var retSaveQtdeZero = await SaveProdutionOrder(retOrderModel);

                        if (!string.IsNullOrEmpty(retSaveQtdeZero))
                        {
                            Logger.Log($"OP {retOrderModel.DocumentNumber} com quantidade zerada não foi fechada por este motivo: {retSaveQtdeZero}");
                            return $"Falha ao realizar apontamento zerado da OP {retOrderModel.DocumentNumber}, erro: " + retSaveQtdeZero;
                        }
                        else
                        {
                            string json = JsonConvert.SerializeObject(retOrderModel, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                            return "";
                        }
                    } else
                    {
                        return "Não foi encontrado Nº de apontamento.";
                    }
                }

                return "";
            } catch (Exception ex)
            {
                return $"Falha ao realizar apontamento: {ex.Message}";
            }
        }

        public async Task<ProductionOrdersModel> GetProdutionOrder(string docEntry)
        {
            try
            {
                ProductionOrdersModel ret = await SLUtil.Connection.Request($"ProductionOrders({docEntry})").GetAsync<ProductionOrdersModel>();

                return ret;
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task<string> SaveProdutionOrder(ProductionOrdersModel obj)
        {
            await _semaphoreOrdemProducao.WaitAsync();

            try
            {
                await SLUtil.Connection.Request($"ProductionOrders({obj.AbsoluteEntry})").PatchAsync(obj);

                Logger.Log($"OP {obj.DocumentNumber} salva com sucesso.");

                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Log($"Falha ao salvar a OP {obj.DocumentNumber}. ## Exception Message do erro: {ex.Message} $$ {ex.InnerException.Message}");

                return ex.Message;
            } finally
            {
                _semaphoreOrdemProducao.Release();
            }
        }

        public async Task<string> SaveInventoryGenExits_GenEntries(ApontamentoModel obj, ProductionOrdersModel ordersModel)
        {
            string retorno = "";

            try
            {
                #region "Objeto saida de insumo"
                string comments = ordersModel.Remarks + Environment.NewLine + $"Solicitada pelo usuário : {obj.UserCode}" + Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                InventoryGenExitsModel inventoryGenExitsObject = new InventoryGenExitsModel() { 
                    DocType = "dDocument_Items",
                    DocDueDate = ordersModel.PostingDate,
                    Comments = Comments(comments),
                    JournalMemo = "Portal de Apontamento | Saída de insumos",
                    DocObjectCode = "oInventoryGenExit",
                    BPL_IDAssignedToInvoice = Convert.ToInt32(obj.BPLIdCode),
                    DocDate = obj.startDate
                };

                foreach (var item in obj.Itens.Where(x => x.QtyUtilizada > 0))
                {
                    var itemInventoryGenExitsModel = new InventoryGenExitsModelDocumentline()
                    {
                        BaseEntry = item.DocEntry,
                        BaseLine = item.LineNum,
                        BaseType = 202,
                        Quantity = Convert.ToSingle(item.QtyUtilizada)
                    };

                    inventoryGenExitsObject.DocumentLines.Add(itemInventoryGenExitsModel);
                }
                #endregion

                #region "Objeto entrada de acabado"
                //comments = ordersModel.Remarks + Environment.NewLine + $"Solicitada pelo usuário : {obj.UserCode}" + Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                //InventoryGenExitsModel inventoryGenEntryObject = new InventoryGenExitsModel()
                //{
                //    DocType = "dDocument_Items",
                //    JournalMemo = "Portal de Apontamento | Entrada de produto acabado",
                //    DocObjectCode = "oInventoryGenEntry",
                //    BPL_IDAssignedToInvoice = Convert.ToInt32(obj.BPLIdCode),
                //    DocDate = obj.startDate,
                //    Comments = Comments(comments)
                //};

                //var calcPercent = obj.QtyRef / obj.QtyPlan;
                //var calcQty = calcPercent * ordersModel.PlannedQuantity;

                //InventoryGenExitsModelDocumentline itemInventoryGenEntries = new InventoryGenExitsModelDocumentline()
                //{
                //    BaseEntry = obj.OrderCode,
                //    BaseType = 202,
                //    Quantity = Convert.ToSingle(calcQty)
                //};

                //inventoryGenEntryObject.DocumentLines.Add(itemInventoryGenEntries);
                #endregion

                //List<SLBatchRequest> batchRequest = new List<SLBatchRequest>
                //{
                //    new SLBatchRequest(HttpMethod.Post, "InventoryGenExits", inventoryGenExitsObject),
                //    new SLBatchRequest(HttpMethod.Post, "InventoryGenEntries", inventoryGenEntryObject)
                //};

                List<SLBatchRequest> batchRequest = new List<SLBatchRequest>
                {
                    new SLBatchRequest(HttpMethod.Post, "InventoryGenExits", inventoryGenExitsObject)
                };

                await _semaphoreSaveInventoryGenExits.WaitAsync();

                try
                {
                    retorno = await ExecuteBatchRequest(batchRequest);

                    return retorno;
                } catch(Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    _semaphoreSaveInventoryGenExits.Release();
                }
            } catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> ExecuteBatchRequest(List<SLBatchRequest> batchRequests)
        {
            string retorno = "";

            HttpResponseMessage[] batchResponse = await SLUtil.Connection.PostBatchAsync(batchRequests.ToArray());

            List<HttpResponseMessage> message = batchResponse.Where(x => x.IsSuccessStatusCode == false).ToList();

            if (message.Count() > 0)
            {
                string ret = string.Empty;

                JsonModel jsonModel = new JsonModel();
                SBOErrorModel sboErrorModel = new SBOErrorModel();

                foreach (HttpResponseMessage error in message)
                {
                    jsonModel = JsonConvert.DeserializeObject<JsonModel>(await error.Content.ReadAsStringAsync());
                    sboErrorModel = JsonConvert.DeserializeObject<SBOErrorModel>(await error.Content.ReadAsStringAsync());

                    ret += $"{Environment.NewLine}{(string.IsNullOrEmpty(jsonModel.ExceptionMessage) ? sboErrorModel.error.code + " - " + sboErrorModel.error.message.value : jsonModel.ExceptionMessage)} | ";
                }

                retorno = $"{ret.Substring(0, ret.Length - 2)}";

                return retorno;
            }

            return "";
        }

        public async Task<string> SaveInventoryGenExits(ApontamentoModel obj, ProductionOrdersModel ordersModel)
        {
            try
            {
                var model = new InventoryGenExitsModel();
                model.DocType = "dDocument_Items";
                
                model.DocDate = ordersModel.PostingDate;
                model.DocDueDate = ordersModel.PostingDate;
                

                model.Comments = ordersModel.Remarks;
                model.Comments += Environment.NewLine + "Solicitada pelo usuário : " + obj.UserCode;
                model.Comments += Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                model.Comments = Comments(model.Comments);

                model.JournalMemo = "Portal de Apontamento | Saída de insumos";
                model.DocObjectCode = "oInventoryGenExit";

                model.BPL_IDAssignedToInvoice = Convert.ToInt32(obj.BPLIdCode);

                model.DocDate = obj.startDate;

                foreach (var item in obj.Itens.Where(x=>x.QtyUtilizada > 0))
                {
                    var itemModel = new InventoryGenExitsModelDocumentline();

                    itemModel.BaseEntry = item.DocEntry;
                    itemModel.BaseLine = item.LineNum;
                    itemModel.BaseType = 202;

                    itemModel.Quantity = Convert.ToSingle(item.QtyUtilizada);               

                    model.DocumentLines.Add(itemModel);
                }

                await _semaphoreSaveInventoryGenExits.WaitAsync();

                try
                {
                    await SLUtil.Connection.Request($"InventoryGenExits").WithReturnNoContent().PostAsync(model);

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    _semaphoreSaveInventoryGenExits.Release();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> SaveInventoryGenEntries(ApontamentoModel obj, ProductionOrdersModel ordersModel)
        {
            try
            {
                var model = new InventoryGenExitsModel();
                model.DocType = "dDocument_Items";

                model.DocDate = ordersModel.PostingDate;

                model.Comments = ordersModel.Remarks;
                model.Comments += Environment.NewLine + "Solicitada pelo usuário : " + obj.UserCode;
                model.Comments += Environment.NewLine + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                model.Comments = Comments(model.Comments);

                model.JournalMemo = "Portal de Apontamento | Entrada de produto acabado";

                model.DocObjectCode = "oInventoryGenEntry";

                model.BPL_IDAssignedToInvoice = Convert.ToInt32(obj.BPLIdCode);

                model.DocDate = obj.startDate;

                var itemModel = new InventoryGenExitsModelDocumentline();

                var calcPercent = obj.QtyRef / obj.QtyPlan;
                var calcQty = calcPercent * ordersModel.PlannedQuantity;

                itemModel.BaseEntry = obj.OrderCode;
                itemModel.BaseType = 202;
                itemModel.Quantity = Convert.ToSingle(calcQty);

                model.DocumentLines.Add(itemModel);

                var json = JsonConvert.SerializeObject(model, Formatting.None,
                           new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                await _semaphoreSaveInventoryGenEntries.WaitAsync();

                try
                {
                    await SLUtil.Connection.Request($"InventoryGenEntries").WithReturnNoContent().PostAsync(model);

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    _semaphoreSaveInventoryGenEntries.Release();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public int CheckDuplicate(string docEntry)
        {
            try
            {
                var modal = Convert.ToInt32(DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_CheckDuplicate"), Database, docEntry)));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApontamentoCheckDay CheckDay(DateTime date)
        {
            try
            {
                var modal = DAO.FillModelFromCommand<ApontamentoCheckDay>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_CheckDay"), Database, date.ToString("yyyy-MM-dd")));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Comments(string comment)
        {
            if (comment.Length > 254)
                comment = comment.Substring(0, 253);

            return comment;
        }

        public string GetNextValueTerceiros()
        {
            try
            {
                var modal = DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("Terceiros_GetNextValue"), Database)).ToString();
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ApontamentoGetNextValueTerceiros GetNextValueTerceiros2()
        {
            try
            {
                var modal = DAO.FillModelFromCommand<ApontamentoGetNextValueTerceiros>(string.Format(Commands.Resource.GetString("Terceiros_GetNextValue2"), Database));
                return modal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task<Tuple<bool, string>> SaveTerceiros(ApontamentoModel model)
        {
            if(string.IsNullOrEmpty(model.IdApontamento) || model.IdApontamento == "0")
            {
                var objSL = new CVA_APTO_TERCEIROSModel.SLModel();

                objSL.Code = GetNextValueTerceiros();
                objSL.Name = "Portal de Apontamento";
                objSL.U_DATA = model.apontamentoData.ToString("yyyy-MM-dd");
                objSL.U_FILIAL = model.BPLIdCode;
                objSL.U_SERVICO = model.ServicoCode;
                objSL.U_TURNO = model.ContratoCode.Split('|')[1];
                objSL.U_QTYPLAN = Convert.ToSingle(model.QtyPlan);
                objSL.U_QTYREF = Convert.ToSingle(model.QtyRef);
                objSL.U_USERPORTAL = model.UserCode;

                objSL.CVA_APTO_TERCEIROS1Collection = new List<CVA_APTO_TERCEIROSModel.CVA_APTO_Terceiros1collection>();
                foreach (var itemBP in model.BPs)
                {
                    var objItem = new CVA_APTO_TERCEIROSModel.CVA_APTO_Terceiros1collection();
                    objItem.LineId = (objSL.CVA_APTO_TERCEIROS1Collection.Count() == 0 ?  0 : objSL.CVA_APTO_TERCEIROS1Collection.Max(x => x.LineId) + 1);
                    objItem.U_CARDCODE = itemBP.BPCardCode;
                    objItem.U_CARDNAME = itemBP.BPCardName;
                    objItem.U_QTYAPT = Convert.ToSingle(itemBP.BPQtyRefeicao);
                    objSL.CVA_APTO_TERCEIROS1Collection.Add(objItem);
                }

                await _semaphoreSaveTerceiros.WaitAsync();

                try
                {
                    await SLUtil.Connection.Request("CVA_APTO_TERCEIROS").WithReturnNoContent().PostAsync(objSL);
                    //var retSL = await sl.PostNormalAsync("CVA_APTO_TERCEIROS", objSL);

                    return new Tuple<bool, string>(true, objSL.Code);
                } catch (Exception ex)
                {
                    return new Tuple<bool, string>(false, ex.Message);
                }
                finally
                {
                    _semaphoreSaveTerceiros.Release();
                }
            }
            else
            {
                CVA_APTO_TERCEIROSModel.SLModel objSL = await SLUtil.Connection.Request("CVA_APTO_TERCEIROS", "'" + model.IdApontamento + "'").GetAsync<CVA_APTO_TERCEIROSModel.SLModel>();

                //var objSL = await sl.GetByIDAsync<CVA_APTO_TERCEIROSModel.SLModel>("CVA_APTO_TERCEIROS", "'"+model.IdApontamento+"'");

                objSL.U_QTYREF = Convert.ToSingle(model.QtyRef);

                foreach (var itemBP in model.BPs.Where(x=>x.BPType=="1"))
                {
                    var objItem = new CVA_APTO_TERCEIROSModel.CVA_APTO_Terceiros1collection();
                    objItem.LineId = (objSL.CVA_APTO_TERCEIROS1Collection.Count() == 0 ? 0 : objSL.CVA_APTO_TERCEIROS1Collection.Max(x => x.LineId) + 1);
                    objItem.U_CARDCODE = itemBP.BPCardCode;
                    objItem.U_CARDNAME = itemBP.BPCardName;
                    objItem.U_QTYAPT = Convert.ToSingle(itemBP.BPQtyRefeicao);
                    objSL.CVA_APTO_TERCEIROS1Collection.Add(objItem);
                }

                await _semaphoreSaveTerceiros.WaitAsync();

                try
                {
                    //var retSL = await sl.PatchAsync("CVA_APTO_TERCEIROS", "'" + model.IdApontamento + "'", objSL);

                    await SLUtil.Connection.Request("CVA_APTO_TERCEIROS", "'" + model.IdApontamento + "'").PatchAsync(objSL);
                    
                    return new Tuple<bool, string>(true, objSL.Code);
                }
                catch (Exception ex)
                {
                    return new Tuple<bool, string>(false, ex.Message);
                }
                finally
                {
                    _semaphoreSaveTerceiros.Release();
                }
            }
        }

        public List<ComboBoxModelHANA> GetItens(string BPLId)
        {
            try
            {
                return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("Apontamento_Itens"), Database, BPLId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
