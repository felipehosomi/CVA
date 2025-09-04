using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Configuracoes
{
    public class ColaboradorBLL : BaseBLL
    {
        public List<ColaboradorModel> Get()
        {
            try
            {
                return DAO.FillListFromCommand<ColaboradorModel>(String.Format(Commands.Resource.GetString("Colaborador_Get"), BaseBLL.Database));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Colaborador_PCP_GERENTE_Model> GetPCPGerente(string userCode)
        {
            try
            {
                return DAO.FillListFromCommand<Colaborador_PCP_GERENTE_Model>(String.Format(Commands.Resource.GetString("Colaborador_PCP_GERENTE"), BaseBLL.Database, userCode));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
