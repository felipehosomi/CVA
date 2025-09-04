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

namespace CVA.Portal.Producao.BLL.Apetit
{
    public class ContratosBLL : BaseBLL
    {
        public List<ComboBoxModelHANA> GetContrato(string BPLId)
        {
            try
            {
                if (!string.IsNullOrEmpty(BPLId))
                    return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("SaidaMateriais_ContratosAtivos"), Database, BPLId));
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ComboBoxModelHANA> GetContratoNumber(string BPLId)
        {
            try
            {
                if (!string.IsNullOrEmpty(BPLId))
                    return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("SaidaMateriais_ContratosAtivosNumber"), Database, BPLId));
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ComboBoxModelHANA> GetContratoApontamento(string BPLId)
        {
            try
            {
                if (!string.IsNullOrEmpty(BPLId))
                    return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("Apontamento_Planejamento_Contrato"), Database, BPLId));
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
