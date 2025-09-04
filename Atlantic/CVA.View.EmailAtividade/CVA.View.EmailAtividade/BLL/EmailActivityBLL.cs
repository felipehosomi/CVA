using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.EmailAtividade.DAO;
using CVA.View.EmailAtividade.MODEL;
using CVA.View.EmailAtividade.Resources;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAtividade.BLL
{
    public class EmailActivityBLL
    {
        EmailServerDAO EmailServerDAO { get; }

        public EmailActivityBLL()
        {
            EmailServerDAO = new EmailServerDAO();
        }

        public static string GetNextCode()
        {
            return CrudController.GetNextCode("@CVA_EMAIL_ACTIVITY").PadLeft(8, '0');
        }

        public string SendEmail(int clgCode)
        {
            string msg = String.Empty;
            CrudController crudController = new CrudController();
            EmailConfigModel emailConfigModel = EmailServerDAO.GetEmail();
            if (emailConfigModel == null)
            {
                return "Configuração de envio de e-mail não encontrada, favor verifique o cadastro";
            }

            EmailTagsModel tagsModel = crudController.FillModelAccordingToSql<EmailTagsModel>(String.Format(Query.EmailTags_Get, clgCode));
            tagsModel.CompanyDB = SBOApp.Company.CompanyDB;
            tagsModel.CompanyName = SBOApp.Company.CompanyName;

            if (String.IsNullOrEmpty(tagsModel.Email))
            {
                return "E-mail não encontrado para o usuário";
            }

            MailMessage mail = new MailMessage();

            mail.To.Add(tagsModel.Email);

            mail.Subject = emailConfigModel.Subject;
            mail.Body = this.EmailMessage(emailConfigModel.Message, tagsModel);
            //mail.IsBodyHtml = true;

            msg = EmailServerBLL.SendEmail(mail, emailConfigModel);
            return msg;
        }

        public string EmailMessage(string message, EmailTagsModel tags)
        {
            int docType = Convert.ToInt32(tags.DocType);

            switch ((BoObjectTypes)docType)
            {
                case BoObjectTypes.oQuotations:
                    tags.DocType = "Cotação de Venda";
                    break;
                case BoObjectTypes.oOrders:
                    tags.DocType = "Pedido de Venda";
                    break;
                case BoObjectTypes.oDeliveryNotes:
                    tags.DocType = "Entrega de Mercadoria";
                    break;
                case BoObjectTypes.oReturns:
                    tags.DocType = "Devolução";
                    break;
                case BoObjectTypes.oInvoices:
                    tags.DocType = "Nota Fiscal de Saída";
                    break;
                case BoObjectTypes.oCreditNotes:
                    tags.DocType = "Dev. Nota Fiscal de Saída";
                    break;
                case BoObjectTypes.oDownPayments:
                    tags.DocType = "Adiantamento de Cliente";
                    break;

                case BoObjectTypes.oPurchaseRequest:
                    tags.DocType = "Solicitação de Compra";
                    break;
                case BoObjectTypes.oPurchaseQuotations:
                    tags.DocType = "Oferta de Compra";
                    break;
                case BoObjectTypes.oPurchaseOrders:
                    tags.DocType = "Pedido de Compra";
                    break;
                case BoObjectTypes.oPurchaseDeliveryNotes:
                    tags.DocType = "Recebimento de Mercadorias";
                    break;
                case BoObjectTypes.oPurchaseReturns:
                    tags.DocType = "Devolução de Mercadorias";
                    break;
                case BoObjectTypes.oPurchaseInvoices:
                    tags.DocType = "Nota Fiscal de Entrada";
                    break;
                //case (BoObjectTypes)69:
                //    tags.DocType = "Despesas de Importação";
                //    break;
                case BoObjectTypes.oPurchaseDownPayments:
                    tags.DocType = "Adiantamento para Fornecedor";
                    break;

                case BoObjectTypes.oIncomingPayments:
                    tags.DocType = "Contas a Receber";
                    break;
                case BoObjectTypes.oVendorPayments:
                    tags.DocType = "Contas a Pagar";
                    break;
                case BoObjectTypes.oWarehouses:
                    tags.DocType = "Depósito";
                    break;
                case BoObjectTypes.oChecksforPayment:
                    tags.DocType = "Cheques para Pagamento";
                    break;
                case BoObjectTypes.oJournalEntries:
                    tags.DocType = "Lançamento Contábil Manual";
                    break;

                case BoObjectTypes.oInventoryTransferRequest:
                    tags.DocType = "Solicitação de Transferência do Estoque";
                    break;
                case BoObjectTypes.oStockTransfer:
                    tags.DocType = "Transferência do Estoque";
                    break;
                case BoObjectTypes.oInventoryGenExit:
                    tags.DocType = "Saída de Mercadorias";
                    break;
                case BoObjectTypes.oInventoryGenEntry:
                    tags.DocType = "Entrada de Mercadorias";
                    break;
                case BoObjectTypes.oMaterialRevaluation:
                    tags.DocType = "Reavaliação do Estoque";
                    break;
                case BoObjectTypes.oProductionOrders:
                    tags.DocType = "Ordem de Produção";
                    break;
                case BoObjectTypes.oItems:
                    tags.DocType = "Item";
                    break;
                //case (BoObjectTypes)1320000012:
                //    tags.DocType = "Gerenciamento de Campanha";
                //    break;

                //case (BoObjectTypes)1250000025:
                //    tags.DocType = "Contratos Básicos";
                //    break;
                default:
                    tags.DocType = "";
                    break;
            }

            foreach (PropertyInfo property in tags.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    message = message.Replace($"{{{property.Name}}}", ((DateTime)property.GetValue(tags)).ToString("dd/MM/yyyy"));
                }
                else
                {
                    message = message.Replace($"{{{property.Name}}}", property.GetValue(tags).ToString());
                }
            }
            return message;
        }
    }
}
