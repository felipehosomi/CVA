using CVA.View.ControleQualidade.DAO;
using CVA.View.ControleQualidade.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.BLL
{
    public class AlertaBLL
    {
        private SAPbobsCOM.Company _company { get; set; }
        private UsuarioDAO _usuarioDAO { get; set; }

        public AlertaBLL(SAPbobsCOM.Company company, UsuarioDAO usuarioDAO)
        {
            _company = company;
            _usuarioDAO = usuarioDAO;
        }

        public string EnviaAlerta(string code, string desc, string obs)
        {
            CompanyService companyService = null;
            MessagesService messageService = null;
            Message message = null;
            MessageDataColumn clmCodigo = null;
            MessageDataColumn clmDesc = null;
            MessageDataColumn clmObs = null;
            
            MessageDataLine line = null;

            try
            {

                companyService = _company.GetCompanyService();
                messageService = (MessagesService)companyService.GetBusinessService(ServiceTypes.MessagesService);

                message = (Message)messageService.GetDataInterface(MessagesServiceDataInterfaces.msdiMessage);

                message.Subject = "Plano de Inspeção CQ";
                message.Text = "Plano de inspeção de Controle de Qualidade foi adicionado. Favor verificar aprovação.";

                int index = 0;

                List<Usuario> usuarioList = _usuarioDAO.GetUsuariosAprovadores();

                foreach (var item in usuarioList)
                {
                    message.RecipientCollection.Add();

                    message.RecipientCollection.Item(index).SendFax = BoYesNoEnum.tNO;
                    message.RecipientCollection.Item(index).SendSMS = BoYesNoEnum.tNO;

                    message.RecipientCollection.Item(index).SendInternal = BoYesNoEnum.tYES;
                    message.RecipientCollection.Item(index).UserCode = item.UserCode;
                    message.RecipientCollection.Item(index).NameTo = item.Nome;
                    message.RecipientCollection.Item(index).SendEmail = BoYesNoEnum.tNO;
                    index++;
                }

                clmCodigo = message.MessageDataColumns.Add();
                clmCodigo.ColumnName = "Código";
                //clmCodigo.Link = BoYesNoEnum.tYES;

                clmDesc = message.MessageDataColumns.Add();
                clmDesc.ColumnName = "Descrição";

                clmObs = message.MessageDataColumns.Add();
                clmObs.ColumnName = "Observações";

                line = clmCodigo.MessageDataLines.Add();
                //line.Object = "CVAQualityInspection";
                //line.ObjectKey = code;
                line.Value = code;

                line = clmDesc.MessageDataLines.Add();
                line.Value = desc;

                line = clmObs.MessageDataLines.Add();
                line.Value = obs;

                messageService.SendMessage(message);
                return String.Empty;
            }
            catch (Exception e)
            {
                return "Erro ao enviar alerta ao aprovador: " + e.Message;
            }
            finally
            {
                if (companyService != null)
                {
                    Marshal.ReleaseComObject(companyService);
                    Marshal.ReleaseComObject(messageService);
                    Marshal.ReleaseComObject(message);
                    Marshal.ReleaseComObject(clmCodigo);
                    Marshal.ReleaseComObject(line);

                    companyService = null;
                    messageService = null;
                    message = null;
                    clmCodigo = null;
                    line = null;
                }
            }
        }
    }
}
