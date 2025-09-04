using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Apetit
{
    public class ServicoExtraBLL : BaseBLL
    {
        ServiceLayerUtil sl = new ServiceLayerUtil();
        AttachmentBLL _Attachment = new AttachmentBLL();

        public ServicoExtraModelGetItemOnHand GetItemOnHandBPLId(string itemCode, string bplId)
        {
            try
            {
                return DAO.FillModelFromCommand<ServicoExtraModelGetItemOnHand>(string.Format(Commands.Resource.GetString("SaidaMateriais_ItemOnHandBPLId"), Database, itemCode, bplId));
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public ServicoExtraModelGetItemCodeServicoExtra GetItemCodeServicoExtra()
        {
            try
            {
                return DAO.FillModelFromCommand<ServicoExtraModelGetItemCodeServicoExtra>(string.Format(Commands.Resource.GetString("ServicoExtra_GetItemCodeServicoExtra"), Database));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ServicoExtraModelGetBlanketId GetBlanketId(string cardCode)
        {
            try
            {
                return DAO.FillModelFromCommand<ServicoExtraModelGetBlanketId>(string.Format(Commands.Resource.GetString("ServicoExtra_GetBlanketId"), Database, cardCode));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ComboBoxModelHANA> GetInsumos(string BPLId)
        {
            try
            {
                return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("ServicoExtra_ItensInsumo"), Database, BPLId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetByClient(string client)
        {
            try
            {
                return DAO.ExecuteScalar(string.Format(Commands.Resource.GetString("ServicoExtra_GetByClient"), Database, client)).ToString();
            } catch (Exception ex)
            {
                return null;
            }
        }

        public async Task SaveSL(ServicoExtraAPIModel model)
        {
            var login = false;
            try
            {
                if (model == null)
                    throw new Exception("Informações incompativel com o modelo.");
                
                //Check
                model.Itens = model?.Itens?.Where(m => m.Qty > 0 && m.Delete == false).ToList();

                if (model.Itens.Count <= 0)
                    throw new Exception("Nenhum item foi informado.");


                //var itemServicoExtraDefault = GetItemCodeServicoExtra();
                //if (itemServicoExtraDefault == null || string.IsNullOrEmpty(itemServicoExtraDefault.ItemCode))
                //    throw new Exception("Nenhum item padrão 'SERVICO EXTRA' foi informado.");


                //var blanketId = GetBlanketId(model.ClienteCode);
                //if (blanketId == null || blanketId?.AbsID <= 0)
                //   throw new Exception("Nenhum contrato guarda-chuvas foi encontrado..");


                login = string.IsNullOrEmpty(await sl.Login());
                if (!login)
                    throw new Exception("Não foi possivel realizar a conexão de forma correta na service layer. favor comunicar ao administrador.");

                var path = _Attachment.GetAttachmentsPath();
                if (path == null || path?.AttachPath == null)
                    throw new Exception($"Não foi localizado uma pasta padrão para salvar o anexo.");

                if (!path.AttachPath.EndsWith("\\"))
                    path.AttachPath += "\\";

                var count = 0;
                var dtFiles = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var listFiles = new List<AttachmentsAPI>();
                foreach (var itemFiles in model.Anexo)
                {
                    var objAtt = new AttachmentsAPI();
                    objAtt.fileName = "ServicoExtra" + $"_{model.ClienteCode}" + $"_{model.UserCode}" + $"_{count}" + $"_{dtFiles}" + $".{_Attachment.FormatFile(itemFiles.type)}";
                    objAtt.type = itemFiles.type;
                    objAtt.attachmentByte = itemFiles.attachmentByte;
                    objAtt.comments = "Serviço Extra";
                    _Attachment.SaveFile(objAtt.attachmentByte, path.AttachPath, objAtt.fileName);
                    
                    listFiles.Add(objAtt);
                    count++;
                }

                //Attachment Save in SL
                var idAttachement = await _Attachment.AttachmentSL(path.AttachPath, listFiles);

                //Save Order BlancketAgreement
                //var retOrderBlancketAgreement = await SaveSLOrdersBlancketAgreement(model, idAttachement, blanketId.AbsID, itemServicoExtraDefault);

                //Save Order
                var retOrder = await SaveSLOrders(model, idAttachement);

                //Delivery Notes
                //var retDelivery = await SaveSLDeliveryNotes(model, idAttachement, blanketId.AbsID, retOrder);

                //model.Itens = new List<ServicoExtraItensModel>();
                //ServicoExtraItensModel extraServiceItem = new ServicoExtraItensModel()
                //{
                //    InsumoCode = "10.02.25.030.00",
                //    Qty = 1
                //};

                //model.Itens.Add(extraServiceItem);

                //Extra Service Order
                //var retExtraServiceOrder = await SaveSLOrders(model, idAttachement);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (login)
                    await sl.Logout();
            }
        }

        public async Task<int> SaveSLOrdersBlancketAgreement(ServicoExtraAPIModel model, int? attachement, int blanketId, ServicoExtraModelGetItemCodeServicoExtra itemCodeServicoExtra )
        {
            try
            {
                var retorno = string.Empty;

                var objSL = new DocumentoMarketingServicoExtraModel();
                objSL.CardCode = model.ClienteCode;
                objSL.DocDate = model.Dt.ToString("yyyy-MM-dd");
                objSL.TaxDate = model.Dt.ToString("yyyy-MM-dd");
                objSL.DocDueDate = model.Dt.ToString("yyyy-MM-dd");

                objSL.AttachmentEntry = attachement;
                objSL.Comments += "Serviço Extra | " + DateTime.Now.ToString("dd/MM/yyyy");
                objSL.Comments += Environment.NewLine + "Solicitada pelo usuário : " + model.UserCode;
                objSL.Comments = Comments(objSL.Comments);

                objSL.BPL_IDAssignedToInvoice = Convert.ToInt32(model.BPLIdCode);

                objSL.DocumentLines = new List<Documentline>();

                ///////////
                var vUsage = System.Configuration.ConfigurationManager.AppSettings["UsageBlanket"];
                objSL.DocumentLines.Add(new DocumentInsumoline
                {
                    ItemCode = itemCodeServicoExtra.ItemCode,
                    Quantity = 1,
                    AgreementNo = blanketId,
                    Usage = vUsage.ToIntNull()
                });

                var retSL = await sl.PostAndReturnEntryAsync<DocumentoMarketingModel>("Orders", objSL);

                if (retSL.Item1 == 0)
                    throw new Exception("Erro ao gerar pedido de vendas: " + retSL.Item2);

                return retSL.Item1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SaveSLOrders(ServicoExtraAPIModel model, int? attachement)
        {
            try
            {
                var retorno = string.Empty;

                var objSL = new DocumentoMarketingServicoExtraModel();
                objSL.CardCode = model.ClienteCode;
                objSL.DocDate = model.Dt.ToString("yyyy-MM-dd");
                objSL.TaxDate = model.Dt.ToString("yyyy-MM-dd");
                objSL.DocDueDate = model.Dt.ToString("yyyy-MM-dd");

                objSL.AttachmentEntry = attachement;
                objSL.Comments += "Serviço Extra | " + DateTime.Now.ToString("dd/MM/yyyy");
                objSL.Comments += Environment.NewLine + "Solicitada pelo usuário : " + model.UserCode;
                objSL.Comments = Comments(objSL.Comments);

                objSL.BPL_IDAssignedToInvoice = Convert.ToInt32(model.BPLIdCode);

                var vUsage = System.Configuration.ConfigurationManager.AppSettings["UsageOrder"];
                
                objSL.DocumentLines = new List<Documentline>();
                foreach (var item in model.Itens)
                {
                    var lineModel = new Documentline();
                    lineModel.ItemCode = item.InsumoCode;
                    lineModel.Quantity = item.Qty;
                    lineModel.Usage = vUsage.ToIntNull();

                    ///////////

                    objSL.DocumentLines.Add(lineModel);
                }

                var retSL = await sl.PostAndReturnEntryAsync<DocumentoMarketingModel>("Orders", objSL);

                if (retSL.Item1 == 0)
                    throw new Exception("Erro ao gerar pedido de vendas: " + retSL.Item2);

                return retSL.Item1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SaveSLDeliveryNotes(ServicoExtraAPIModel model, int? attachement, int blanketId, int docEntry)
        {
            try
            {
                var retorno = string.Empty;

                var objSL = new DocumentoMarketingServicoExtraModel();
                objSL.CardCode = model.ClienteCode;
                objSL.DocDate = model.Dt.ToString("yyyy-MM-dd");
                objSL.TaxDate = model.Dt.ToString("yyyy-MM-dd");
                objSL.DocDueDate = model.Dt.ToString("yyyy-MM-dd");

                objSL.AttachmentEntry = attachement;
                //objSL.Comments += $"Origem: Contrato guarda-chuva {blanketId}.";
                //objSL.Comments += Environment.NewLine + $"Baseado em Pedidos de venda {docEntry}.";
                objSL.Comments += $"Solicitada pelo usuário : {model.UserCode}." ;
                objSL.Comments += Environment.NewLine + $"Serviço Extra - {DateTime.Now.ToString("dd/MM/yyyy")}";
                objSL.Comments = Comments(objSL.Comments);

                objSL.BPL_IDAssignedToInvoice = Convert.ToInt32(model.BPLIdCode);

                var vUsage = System.Configuration.ConfigurationManager.AppSettings["UsageDelivery"];
                //
                var vSequence = System.Configuration.ConfigurationManager.AppSettings["Sequence"];
                if (!string.IsNullOrEmpty(vSequence))
                    objSL.SequenceCode = vSequence.ToIntNull();

                objSL.DocumentLines = new List<Documentline>();

                var i = 0;
                foreach (var item in model.Itens)
                {
                    objSL.DocumentLines.Add(
                        new Documentline
                        {
                            BaseType = 17,
                            BaseEntry = docEntry,
                            BaseLine = i,
                            Usage = vUsage.ToIntNull()
                        }
                    );
                    i++;
                }

                var retSL = await sl.PostAndReturnEntryAsync<DocumentoMarketingModel>("DeliveryNotes", objSL);

                if (retSL.Item1 == 0)
                    throw new Exception("Erro ao gerar a entraga: " + retSL.Item2);

                return retSL.Item1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task PurchaseRequestSL(ServicoExtraAPIModel model, List<ServicoExtraItensModel> servicoExtraItens, int? attachement, int docEntry)
        {
            try
            {
                var modelNew = new PurchaseRequestModel();

                ServiceLayerUtil sl = new ServiceLayerUtil();

                var login = string.IsNullOrEmpty(await sl.Login());
                if (!login)
                    throw new Exception("Não foi possivel realizar a conexão de forma correta na service layer. favor comunicar ao administrador.");

                modelNew.DocDate = DateTime.Now.ToString("yyyy-MM-dd");
                modelNew.RequriedDate = DateTime.Now.ToString("yyyy-MM-dd");
                modelNew.Requester = model.UserCode;

                modelNew.AttachmentEntry = attachement;
                modelNew.Comments = $">Pedido de vendas ({docEntry}) |";
                modelNew.Comments += Environment.NewLine + "Serviço Extra – " + DateTime.Now.ToString("dd/MM/yyyy");
                modelNew.Comments = Comments(modelNew.Comments);

                modelNew.BPL_IDAssignedToInvoice = Convert.ToInt32(model.BPLIdCode);

                modelNew.DocumentLines = new List<PurchaseRequestLineModel>();
                foreach (var item in servicoExtraItens)
                {
                    var line = new PurchaseRequestLineModel();
                    line.ItemCode = item.InsumoCode;
                    line.RequiredDate = DateTime.Now.ToString("yyyy-MM-dd");
                    line.Quantity = item.Qty;

                    modelNew.DocumentLines.Add(line);
                }

                var retorno = await sl.PostAsync("PurchaseRequests", modelNew);

                if (!string.IsNullOrEmpty(retorno))
                    throw new Exception("Erro ao gerar ordem de produção: " + retorno);
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

        

    }
}
