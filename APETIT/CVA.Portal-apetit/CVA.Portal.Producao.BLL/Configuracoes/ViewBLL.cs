using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Configuracoes
{
    public class ViewBLL : BaseBLL
    {
        public ViewBLL()
        {
            DAO.TableName = "@CVA_VIEW";
        }

        public List<ViewModel> Get()
        {
            return DAO.RetrieveModelList<ViewModel>();
            //return DAO.RetrieveModelList<ViewModel>("\"U_CodPai\" IS NOT NULL");
        }

        public List<ViewModel> GetPorUsuario(string codUsuario)
        {
            List<ViewModel> model = DAO.FillModelList<ViewModel>(String.Format(Commands.Resource.GetString("View_GetPorUsuario"), BaseBLL.Database, codUsuario));
            return model;
        }

        public List<ViewModel> GetPorFornecedor()
        {
            List<ViewModel> model = DAO.FillModelList<ViewModel>(String.Format(Commands.Resource.GetString("View_Fornecedor"), BaseBLL.Database));
            return model;
        }
    }
}
