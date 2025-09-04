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
    public class FilialBLL : BaseBLL
    {
        public List<ComboBoxModel> Get()
        {
            try
            {
                return DAO.FillListFromCommand<ComboBoxModel>(string.Format(Commands.Resource.GetString("OfertaCompra_RetornaFiliais"), Database));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ComboBoxModelHANA> GetFilial(string userId)
        {
            try
            {
                //CvaSession.Usuario.Usuario
                return DAO.FillListFromCommand<ComboBoxModelHANA>(string.Format(Commands.Resource.GetString("SaidaMateriais_GetFilialPadrao"), Database, userId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SaidaInsumoModelGetOBPL GetSaidaInsumoOBPL(string BPLId)
        {
            try
            {
                return DAO.FillModelFromCommand<SaidaInsumoModelGetOBPL>(string.Format(Commands.Resource.GetString("SaidaMateriais_OBPL"), Database, BPLId));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
