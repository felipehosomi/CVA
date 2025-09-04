using CVA.View.EmailAutorizacao.HELPER;
using CVA.View.EmailAutorizacao.MODEL;
using CVA.View.EmailAutorizacao.SERVICE;
using CVA.View.EmailAutorizacao.SERVICE.Resource;
using Dover.Framework.DAO;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAutorizacao.BLL
{
    public class PedidoCompraBLL
    {
        Application Application { get; set; }
        PedidoCompraDAO PedidoCompraDAO { get; set; }
        EmailServerBLL EmailServerBLL { get; set; }

        public PedidoCompraBLL(Application application, PedidoCompraDAO pedidoCompraDAO, EmailServerBLL emailServerBLL)
        {
            Application = application;
            PedidoCompraDAO = pedidoCompraDAO;
            EmailServerBLL = emailServerBLL;
        }

        public string SendAuthorizationEmail(int docNum, double docTotal)
        {
            string msg = String.Empty;
            try
            {
                List<EmailMessageModel> emailList = PedidoCompraDAO.RetrieveEmailList(docNum, docTotal);
                // Se encontrar um esboço pendente de aprovação, envia o e-mail
                if (emailList.Count > 0)
                {
                    if (StaticKeys.ConfigModel == null || String.IsNullOrEmpty(StaticKeys.ConfigModel.Banco))
                    {
                        return "CVA - Configurações não encontradas para base do Portal. Verifique a tabela [@CVA_CONFIG_DB]";
                    }

                    MailMessage mail = new MailMessage();

                    foreach (var item in emailList)
                    {
                        if (!String.IsNullOrEmpty(item.Email))
                        {
                            mail.To.Add(item.Email);

                            mail.Subject = "Autorização de Pedido de Compra";
                            mail.Body = this.EmailBody(item);
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

        public string EmailBody(EmailMessageModel emailMessageModel)
        {
            return $@"Sr(a). {emailMessageModel.UserName},

Segue dados do pedido para aprovação:

Pedido Nº : {emailMessageModel.DocNum}
Data do Pedido: {emailMessageModel.DocDate.ToString("dd/MM/yyyy")}
Fornecedor: {emailMessageModel.CardCode} - {emailMessageModel.CardName}
Valor: {emailMessageModel.DocTotal.ToString("R$ #,##0.00")}
Empresa: {emailMessageModel.BPLName}
Base de dados: {Application.Company.DatabaseName}";
        }
    }
}
