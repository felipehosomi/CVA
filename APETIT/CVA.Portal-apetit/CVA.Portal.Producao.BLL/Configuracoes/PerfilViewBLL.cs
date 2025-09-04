using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Configuracoes
{
    public class PerfilViewBLL : BaseBLL
    {
        public PerfilViewBLL()
        {
            DAO.TableName = "@CVA_PERFIL_VIEW";
        }

        public List<PerfilViewModel> Get()
        {
            return DAO.RetrieveModelList<PerfilViewModel>();
        }

        public PerfilViewModel Get(string code)
        {
            return DAO.RetrieveModel<PerfilViewModel>($"\"Code\" = '{code}'");
        }

        public List<PerfilViewModel> GetByPerfil(string codPerfil)
        {
            List<PerfilViewModel> list = DAO.FillListFromCommand<PerfilViewModel>(String.Format(Commands.Resource.GetString("PerfilView_GetByPerfil"), BaseBLL.Database, codPerfil));
            return list;
        }

        public void Create(PerfilModel model)
        {
            foreach (var item in model.ViewList)
            {
                if (item.Selected)
                {
                    PerfilViewModel viewModel = new PerfilViewModel();
                    viewModel.CodPerfil = model.Code;
                    viewModel.CodView = item.CodView;
                    DAO.Model = viewModel;
                    DAO.CreateModel();
                }    
            }
        }

        public void Update(PerfilModel model)
        {
            DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("PerfilView_Delete"), BaseBLL.Database, model.Code));
            this.Create(model);
        }
    }
}
