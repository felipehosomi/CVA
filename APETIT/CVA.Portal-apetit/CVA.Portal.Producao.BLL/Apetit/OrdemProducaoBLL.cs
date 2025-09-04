using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CVA.Portal.Producao.Model.MessagesModel;

namespace CVA.Portal.Producao.BLL
{
    public class OrdemProducaoBLL : BaseBLL
    {
        public List<ComboBoxModelHANA> GetOpenOrders(string ordersBPLId, DateTime? ordersDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(ordersBPLId))
                    return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("OrdemProducao_OpenOrders"), Database, ordersBPLId, 
                        (ordersDate.HasValue ? $"TO_DATE('{ordersDate.Value.ToString("yyyy-MM-dd")}')" : "CURRENT_DATE")
                        ));
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ComboBoxModelHANA> GetAllOrders(string ordersBPLId)
        {
            try
            {
                if (!string.IsNullOrEmpty(ordersBPLId))
                    return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("OrdemProducao_AllOrdersTipo"), Database, ordersBPLId));
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetClienteBPLID(string clienteBPLID)
        {
            try
            {
                if (!string.IsNullOrEmpty(clienteBPLID))
                    return DAO.FillModelFromCommand<string>(string.Format(Commands.Resource.GetString("OrdemProducao_ClienteBPLID"), Database, clienteBPLID));
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
