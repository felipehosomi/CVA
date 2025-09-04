using CVA.Portal.Producao.BLL.Apetit;
using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Compras;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using static CVA.Portal.Producao.Model.MessagesModel;

namespace CVA.Portal.Producao.BLL.Compras
{
    public class OfertaCompraBLL : BaseBLL
    {
        MessagesBLL BLLMessages = new MessagesBLL();
        UsuarioBLL BLLUsuario = new UsuarioBLL();

        public List<OfertaCompraListModel> GetList(string sFornecedor)
        {
            string sFiltro = string.Empty;
            if (!string.IsNullOrEmpty(sFornecedor))
            {
                sFiltro = $" AND A.\"CardCode\" = '{sFornecedor}'";
            }

            string command = String.Format(Commands.Resource.GetString("OfertaCompra_Index"), BaseBLL.Database, sFiltro);
            return DAO.FillListFromCommand<OfertaCompraListModel>(command);
        }

        public OfertaCompraModel GetById(int id)
        {
            OfertaCompraModel ofertaCompra = new OfertaCompraModel();

            string command = String.Format(Commands.Resource.GetString("OfertaCompra_Detail"), BaseBLL.Database, id);

            ofertaCompra = DAO.FillModelFromCommand<OfertaCompraModel>(command);

            command = String.Format(Commands.Resource.GetString("OfertaCompra_DetailItens"), BaseBLL.Database, id);

            ofertaCompra.Itens = DAO.FillListFromCommand<OfertaCompraItemModel>(command);

            return ofertaCompra;
        }

        public List<OfertaCompraItemUMModel> GetItensUM(string itemCode)
        {
            string[] sUM = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("OfertaCompra_CodigosUM"), BaseBLL.Database, itemCode)).ToString().Split('|');

            string[] codes = sUM[0].Split(';');
            string[] names = sUM[1].Split(';');

            List<OfertaCompraItemUMModel> list = new List<OfertaCompraItemUMModel>();

            for(int i = 0; i < codes.Length; i++)
            {
                list.Add(new OfertaCompraItemUMModel()
                {
                    Code = codes[i] + "|" + names[i],
                    Desc = names[i]
                });
            }

            return list;
        }

        public async Task<string> Update(int DocEntry, OfertaCompraModel model)
        {
            List<OfertaCompraDocumentLines> list = new List<OfertaCompraDocumentLines>();

            foreach (OfertaCompraItemModel obj in model.Itens)
            {
                OfertaCompraDocumentLines addModelItens;
                if (obj.newRegister)
                {
                    addModelItens = new OfertaCompraDocumentLines()
                    {
                        LineNum = obj.LineNum.ToString(),
                        Quantity = obj.Quantity.ToString().Replace(",", "."),
                        UnitPrice = Convert.ToDecimal(obj.Price.Replace(".", ",").Substring(3, obj.Price.Length - 3)),
                        ShipDate = obj.ShipDate,
                        FreeText = obj.FreeTxt,
                        ItemCode = obj.ItemCode,
                        RequiredQuantity = obj.PQTReqQty.ToString().Replace(",", "."),
                        UoMEntry = obj.UomEntry.Split('|')[0],
                        UoMCode = obj.UomEntry.Split('|')[1]
                    };
                } else
                {
                    addModelItens = new OfertaCompraDocumentLines()
                    {
                        LineNum = obj.LineNum.ToString(),
                        Quantity = obj.Quantity.ToString().Replace(",", "."),
                        UnitPrice = Convert.ToDecimal(obj.Price.Replace(".", ",").Substring(3, obj.Price.Length - 3)),
                        ShipDate = obj.ShipDate,
                        FreeText = obj.FreeTxt,
                        ItemCode = obj.ItemCode,
                        RequiredQuantity = obj.PQTReqQty.ToString().Replace(",", ".")
                    };
                }

                list.Add(addModelItens);
            }

            OfertaCompraPatchModel addModel = new OfertaCompraPatchModel() { DocumentLines = list };

            ServiceLayerUtil sl = new ServiceLayerUtil();
            await sl.Login();
            string retorno = String.Empty;

            retorno = await sl.PatchAsync("PurchaseQuotations", DocEntry.ToString(), addModel);

            if (string.IsNullOrEmpty(retorno))
            {
                foreach (OfertaCompraItemModel obj in model.Itens)
                {
                    var s = obj.UomEntry.Split('|');
                    var teste = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("OfertaCompra_RetornaFornecimento"), BaseBLL.Database, s[0]));
                    var unitMsr = s[1];
                    var NumPerMsr = teste.ToString().Split('|')[1];
                    var UomEntry = s[0];
                    var UomCode = s[1];
                    var InvQty = Convert.ToDouble(obj.Quantity.ToString().Replace('.', ',')) * Convert.ToDouble(NumPerMsr.Replace('.', ','));
                    var OpenInvQty = InvQty;
                    var CodeBars = "";

                    DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("OfertaCompra_AtualizaFornecimento"), BaseBLL.Database, unitMsr, NumPerMsr, UomEntry, UomCode, InvQty.ToString().Replace(",", "."), OpenInvQty.ToString().Replace(",", "."), CodeBars, DocEntry.ToString(), obj.LineNum, obj.LeadTime));
                }
            }

            return retorno;
        }

        public List<ComboBoxModel> GetFornecedores()
        {
            string command = String.Format(Commands.Resource.GetString("OfertaCompra_Fornecedor"), BaseBLL.Database);

            return DAO.FillListFromCommand<ComboBoxModel>(command);
        }

        public List<ComboBoxModel> GetCondicaoPagamento()
        {
            string command = String.Format(Commands.Resource.GetString("OfertaCompra_CondicaoPagamento"), BaseBLL.Database);

            return DAO.FillListFromCommand<ComboBoxModel>(command);
        }

        public void AtualizaStatus(string docNum, int status)
        {
            DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("OfertaCompra_AtualizaStatus"), BaseBLL.Database, status, docNum));
        }

        public async void FechaOfertaCompra(string docNum)
        {
            OfertaCompraModel ofertaCompra = new OfertaCompraModel();

            string command = String.Format(Commands.Resource.GetString("OfertaCompra_Detail"), BaseBLL.Database, docNum);

            ofertaCompra = DAO.FillModelFromCommand<OfertaCompraModel>(command);

            string DocEntry = ofertaCompra.DocEntry.ToString();

            ServiceLayerUtil sl = new ServiceLayerUtil();
            await sl.Login();
            string retorno = String.Empty;

            retorno = await sl.PostAsync($"PurchaseQuotations({DocEntry})/Close", new { code = "" });
        }

        public void AtualizaObsRevisao(string docNum, string obs)
        {
            DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("OfertaCompra_AtualizaObsRevisao"), BaseBLL.Database, obs, docNum));
        }

        public async Task<string> SendEmail(TiposEmail tipoEmail, string sCotacaoCompra, string sNomeContato, string sEmailContato, string sOrdemCompra = "", string sObservacoes = "")
        {
            var messageModel = new MessagesModel();
            string sUserType = "rt_InternalUser";

            switch (tipoEmail)
            {
                case TiposEmail.EnvioCotacao:
                    AtualizaStatus(sCotacaoCompra, 1);
                    messageModel.Subject = $"Atualização da Cotação de Compra Nº {sCotacaoCompra}.";
                    messageModel.Text = $"Prezado Comprador, \r\n\r\nA Oferta de compra Nº {sCotacaoCompra} foi atualizada pelo Fornecedor. \r\n\r\nFavor avaliar.\r\n\r\nAtenciosamente, \r\nDepartamento de Compras Apetit";
                    break;
                case TiposEmail.AprovacaoCotacao:
                    AtualizaStatus(sCotacaoCompra, 4);
                    messageModel.Subject = $"Sua Cotação de Compra Nº {sCotacaoCompra} foi aprovada.";
                    messageModel.Text = $"Prezado Fornecedor, \r\n\r\nSua Cotação de Compra Nº {sCotacaoCompra} foi aprovada com sucesso pela equipe de Compradores da Apetit. \r\n\r\nAtenciosamente, \r\nDepartamento de Compras Apetit";
                    sUserType = "rt_ContactPerson";
                    break;
                case TiposEmail.ReprovacaoCotacao:
                    AtualizaStatus(sCotacaoCompra, 5);
                    FechaOfertaCompra(sCotacaoCompra);
                    messageModel.Subject = $"Sua Cotação de Compra Nº {sCotacaoCompra} foi reprovada.";
                    messageModel.Text = $"Prezado Fornecedor, \r\n\r\nSua Cotação de Compra Nº {sCotacaoCompra} foi reprovada pelo processo de aprovação de compras da Apetit. Agradecemos sua participação. \r\n\r\nAtenciosamente, \r\nDepartamento de Compras Apetit";
                    sUserType = "rt_ContactPerson";
                    break;
                case TiposEmail.RevisaoCotacao:
                    AtualizaStatus(sCotacaoCompra, 2);
                    AtualizaObsRevisao(sCotacaoCompra, sObservacoes);
                    messageModel.Subject = $"Sua Cotação de Compra Nº {sCotacaoCompra} precisa ser revisada.";
                    messageModel.Text = $"Prezado Fornecedor, \r\n\r\nSua Cotação de Compra Nº {sCotacaoCompra} necessita de nova revisão. Favor observar as solicitações do comprador. \r\n\r\n Observações do Comprador: {sObservacoes} \r\n\r\nAtenciosamente, \r\nDepartamento de Comprar Apetit";
                    sUserType = "rt_ContactPerson";
                    break;
                case TiposEmail.RecusaSolicitacao:
                    AtualizaStatus(sCotacaoCompra, 6);
                    FechaOfertaCompra(sCotacaoCompra);
                    messageModel.Subject = $"Cotação de Compra Nº {sCotacaoCompra} foi recusada.";
                    messageModel.Text = $"Prezados Compradores, \r\n\r\nA Cotação de Compra Nº {sCotacaoCompra} foi recusada pelo Fornecedor. \r\n\r\nAtenciosamente, \r\nMensagem automática do Portal";
                    break;
            }

            if(!string.IsNullOrEmpty(sNomeContato) && !string.IsNullOrEmpty(sEmailContato))
            {
                messageModel.RecipientCollection.Add(new Recipientcollection()
                {
                    UserCode = sNomeContato,
                    UserType = sUserType,
                    NameTo = sNomeContato,
                    SendEmail = "tYES",
                    EmailAddress = sEmailContato,
                    SendSMS = "tNO",
                    CellularNumber = "",
                    SendFax = "tNO",
                    FaxNumber = "",
                    SendInternal = "tNO"
                });

                return await BLLMessages.SendMessage(messageModel);
            }

            return "";
        }
    }
}
