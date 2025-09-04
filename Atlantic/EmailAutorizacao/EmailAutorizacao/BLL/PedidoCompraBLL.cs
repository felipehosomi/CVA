using System;
using EmailAutorizacao.HELPER;
using EmailAutorizacao.MODEL;
using EmailAutorizacao.SERVICE;
using System.Net.Mail;

namespace EmailAutorizacao.BLL
{
    public class PedidoCompraBLL
    {
        public static string SendAuthorizationEmail(int docNum, double docTotal)
        {
            var msg = string.Empty;
            try
            {
                var emailList = PedidoCompraDAO.RetrieveEmailList(docNum, docTotal);
                // Se encontrar um esboço pendente de aprovação, envia o e-mail
                if (emailList.Count > 0)
                {
                    if (StaticKeys.ConfigModel == null || string.IsNullOrEmpty(StaticKeys.ConfigModel.Banco))
                    {
                        return "CVA - Configurações não encontradas para base do Portal. Verifique a tabela [@CVA_CONFIG_DB]";
                    }

                    var mail = new MailMessage();

                    foreach (var item in emailList)
                    {
                        if (!string.IsNullOrEmpty(item.Email))
                        {
                            mail.To.Add(item.Email);

                            mail.Subject = "Autorização de Pedido de Compra";
                            mail.Body = EmailBody(item);
                            //mail.IsBodyHtml = true;

                            msg = EmailServerBLL.SendEmail(mail);
                            if (!String.IsNullOrEmpty(msg))
                            {
                                break;
                            }
                        }
                    }
                }

                return msg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string EmailBody(EmailMessageModel emailMessageModel)
        {
            return $@"Sr(a). {emailMessageModel.UserName},

Segue dados do pedido para aprovação:

Pedido Nº : {emailMessageModel.DocNum}
Data do Pedido: {emailMessageModel.DocDate.ToString("dd/MM/yyyy")}
Fornecedor: {emailMessageModel.CardCode} - {emailMessageModel.CardName}
Valor: {emailMessageModel.DocTotal.ToString("R$ #,##0.00")}
Empresa: {emailMessageModel.BPLName}
Base de dados: {B1Connection.Instance.Application.Company.DatabaseName}";
        }
    }
}
