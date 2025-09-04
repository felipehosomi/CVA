using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.BLL.Util;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CVA.Portal.Producao.Model.MessagesModel;

namespace CVA.Portal.Producao.BLL.Apetit
{
    public class ReposicaoInsumosBLL : BaseBLL
    {
        MessagesBLL BLLMessages = new MessagesBLL();
        ColaboradorBLL BLLColaborador = new ColaboradorBLL();
        UsuarioBLL BLLUsuario = new UsuarioBLL();
        AttachmentBLL BLLAttachment = new AttachmentBLL();
        ServiceLayerUtil sl = new ServiceLayerUtil();

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

        public ComboBoxModelHANA GetMotivo(string code)
        {
            try
            {
                return DAO.FillModelFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("ReposicaoInsumo_GetMotivo"), Database, code));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> SendMessageSL(ReposicaoInsumoModel model)
        {
            string retorno = string.Empty;
            model.Itens = model.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();

            if(model?.Itens?.Count > 0)
            {
                var messageModel = new MessagesModel();
                messageModel.Subject = "Solicitação de Reposição de Insumos – Cliente " + model.ClienteCode;
                messageModel.Text = "Grade de itens solicitada pelo usuário : " + model.UserCode +".";
                messageModel.Text += Environment.NewLine + $"Motivo: {model.MotivoCode}";
                messageModel.Text += Environment.NewLine + $"Observação: {model.observacao}";

                var columnContrato = new Messagedatacolumn();
                columnContrato.ColumnName = "Cliente";
                columnContrato.Link = "tYES";

                var columnItem = new Messagedatacolumn();
                columnItem.ColumnName = "Nº do Item";
                columnItem.Link = "tYES";

                var columnDescricao = new Messagedatacolumn();
                columnDescricao.ColumnName = "Descrição";
                columnDescricao.Link = "tNO";

                var columnQuantidade = new Messagedatacolumn();
                columnQuantidade.ColumnName = "Quantidade";
                columnQuantidade.Link = "tNO";

                var columnData = new Messagedatacolumn();
                columnData.ColumnName = "Data";
                columnData.Link = "tNO";


                foreach (var item in model.Itens)
                {
                    columnContrato.MessageDataLines.Add(new Messagedataline() { Value = model.ClienteCode, Object = "1250000025", ObjectKey = model.ClienteCode });
                    columnItem.MessageDataLines.Add(new Messagedataline() { Value = item.InsumoCode, Object = "4", ObjectKey = item.InsumoCode });
                    columnDescricao.MessageDataLines.Add(new Messagedataline() { Value = item.InsumoName });
                    columnQuantidade.MessageDataLines.Add(new Messagedataline() { Value = item.Qty.ToString() });
                    columnData.MessageDataLines.Add(new Messagedataline() { Value = item.DtNecessidade.ToString().Replace(" 00:00:00","") });
                }

                messageModel.MessageDataColumns.Add(columnContrato);
                messageModel.MessageDataColumns.Add(columnItem);
                messageModel.MessageDataColumns.Add(columnDescricao);
                messageModel.MessageDataColumns.Add(columnQuantidade);
                messageModel.MessageDataColumns.Add(columnData);


                var userID = BLLUsuario.GetUserID(model.UserCode);
                messageModel.User = userID.USERID;

                var listEmployees = BLLColaborador.GetPCPGerente(model.UserCode);
                if (listEmployees != null && listEmployees?.Count > 0)
                {
                    foreach (var employees in listEmployees)
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

                retorno = await BLLMessages.SendMessage(messageModel);
            }

            if (!string.IsNullOrEmpty(retorno))
                retorno = "Erro ao gerar saída de insumos: " + retorno;

            return retorno;
        }

        public async Task<string> PurchaseRequestSL(ReposicaoInsumoAPIModel model)
        {
            try
            {
                string retorno = string.Empty;

                model.Itens = model.Itens.Where(m => m.Qty > 0 && m.Delete == false).ToList();

                Logger.LogReposicaoInsumos($"Iniciando a criação da solicitação de compra com {model.Itens.Count()} itens. Para o cliente {model.ClienteCode} - {model.ClienteName}");

                if (model?.Itens?.Count > 0)
                {
                    string sLogItens = "---Itens que serão adicionados---";

                    foreach (ReposicaoInsumoItensModel item in model.Itens)
                    {
                        sLogItens += Environment.NewLine + $"{item.InsumoCode} - {item.InsumoName} | Quantidade: {item.Qty}";
                    }

                    var modelNew = new PurchaseRequestModel();
                    
                    var login = string.IsNullOrEmpty(await sl.Login());
                    if (!login)
                    {
                        Logger.LogReposicaoInsumos($"Não foi possivel realizar a conexão de forma correta na service layer");
                        throw new Exception("Não foi possivel realizar a conexão de forma correta na service layer. favor comunicar ao administrador.");
                    }

                    int? idAttachement = null;

                    if (model.Anexo != null && model.Anexo.Count() > 0)
                    {
                        var path = BLLAttachment.GetAttachmentsPath();
                        if (path == null || path?.AttachPath == null)
                            throw new Exception($"Não foi localizado uma pasta padrão para salvar o anexo.");
                        Logger.LogReposicaoInsumos($"Caminho para salvar o anexo: {path}");

                        if (!path.AttachPath.EndsWith("\\"))
                            path.AttachPath += "\\";

                        var count = 0;
                        var dtFiles = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        var listFiles = new List<AttachmentsAPI>();
                        foreach (var itemFiles in model.Anexo)
                        {
                            var objAtt = new AttachmentsAPI();
                            objAtt.fileName = "Reposicao" + $"_{model.ClienteCode}" + $"_{model.UserCode}" + $"_{count}" + $"_{dtFiles}" + $".{BLLAttachment.FormatFile(itemFiles.type)}";
                            objAtt.type = itemFiles.type;
                            objAtt.attachmentByte = itemFiles.attachmentByte;
                            objAtt.comments = "Reposição de Insumo";
                            BLLAttachment.SaveFile(objAtt.attachmentByte, path.AttachPath, objAtt.fileName);

                            listFiles.Add(objAtt);
                            count++;
                        }

                        //Attachment Save in SL
                        idAttachement = await BLLAttachment.AttachmentSL(path.AttachPath, listFiles);
                        Logger.LogReposicaoInsumos($"ID do anexo salvo: {idAttachement}");
                    }

                    modelNew.DocDate = DateTime.Now.ToString("yyyy-MM-dd");
                    modelNew.RequriedDate = model.Itens.OrderByDescending(x => x.DtNecessidade).First().DtNecessidade.ToString("yyyy-MM-dd");
                    modelNew.Requester = model.UserCode;

                    if (idAttachement.HasValue)
                        modelNew.AttachmentEntry = idAttachement;

                    var motivo = GetMotivo(model.MotivoCode);
                    modelNew.U_CVA_ObsPortal = $"Motivo: {motivo.NAME}";
                    //modelNew.Comments += Environment.NewLine + $"Observação: {model.observacao}";
                    modelNew.Comments = model.observacao;
                    modelNew.BPL_IDAssignedToInvoice = Convert.ToInt32(model.BPLIdCode);

                    modelNew.DocumentLines = new List<PurchaseRequestLineModel>();
                    foreach (var item in model.Itens)
                    {
                        var line = new PurchaseRequestLineModel();
                        line.ItemCode = item.InsumoCode;
                        line.RequiredDate = item.DtNecessidade.ToString("yyyy-MM-dd");
                        line.Quantity = item.Qty;

                        modelNew.DocumentLines.Add(line);
                    }

                    var json = JsonConvert.SerializeObject(modelNew, Formatting.None,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });

                    Logger.LogReposicaoInsumos($"Salvando anexo solicitação de compra | JSON: {json}");

                    retorno = await sl.PostAsync("PurchaseRequests", modelNew);

                    var jsonModel = JsonConvert.SerializeObject(model, Formatting.None,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    if (!string.IsNullOrEmpty(retorno))
                    {
                        Logger.LogReposicaoInsumos($"Erro ao salvar solicitação de compra. JSON enviado para PurchaseRequests: {json} | Objeto recebido(ReposicaoInsumoAPIModel): {jsonModel}");
                        throw new Exception("Erro ao salvar solicitação de compra: " + retorno);
                    } else
                    {
                        Logger.LogReposicaoInsumos($"Solicitação de compra salva com sucesso. JSON enviado para PurchaseRequests: {json} | Objeto recebido(ReposicaoInsumoAPIModel): {jsonModel}");
                    }

                    //await sl.Logout();

                    return retorno;
                }

                Logger.LogReposicaoInsumos("Problema no request: Item não capturado.");
                throw new Exception("Problema no request: Item não capturado.");
            }
            catch (Exception ex)
            {
                //Logger.LogReposicaoInsumos($"Erro ao criar solicitação de compra | erro: {ex.Message}");
                return ex.Message;
            }
            
        }

    }
}
