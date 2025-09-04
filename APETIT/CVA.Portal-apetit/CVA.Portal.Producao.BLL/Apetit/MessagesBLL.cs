using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Apetit
{
    public class MessagesBLL : BaseBLL
    {
        public async Task<string> SendMessage(MessagesModel model)
        {
            ServiceLayerUtil sl = new ServiceLayerUtil();
            await sl.Login();
            string retorno = String.Empty;

            if (model != null)
            {
                model.Priority = "pr_Normal";
                retorno = await sl.PostAsync<MessagesModel>("Messages", model);
            }
            else
                retorno = "Mensagem Nula";

            if (!string.IsNullOrEmpty(retorno))
                retorno = "Erro ao enviar mensagem: " + retorno;

            await sl.Logout();

            return retorno;
        }

    }
}
