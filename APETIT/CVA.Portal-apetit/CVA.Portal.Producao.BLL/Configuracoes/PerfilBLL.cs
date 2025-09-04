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
    public class PerfilBLL : BaseBLL
    {
        PerfilViewBLL PerfilViewBLL = new PerfilViewBLL();

        public PerfilBLL()
        {
            DAO.TableName = "@CVA_PERFIL";
        }

        public List<PerfilModel> Get()
        {
            return DAO.RetrieveModelList<PerfilModel>();
        }

        public PerfilModel Get(string codigo)
        {
            PerfilModel model = DAO.RetrieveModel<PerfilModel>($"\"Code\" = '{codigo}'");
            if (model != null)
            {
                model.Ativo = model.AtivoInt == 1;
            }
            else
            {
                model = new PerfilModel();
            }

            List<PerfilViewModel> viewList = PerfilViewBLL.GetByPerfil(codigo);
            model.ViewList = new List<PerfilViewModel>();
            foreach (var pai in viewList.Where(v => String.IsNullOrEmpty(v.CodPai)).OrderBy(v => v.Posicao))
            {
                pai.Selected = pai.SelectedInt == 1;
                model.ViewList.Add(pai);
                foreach (var item in viewList.Where(v => v.CodPai == pai.CodView).OrderBy(v => v.Posicao))
                {
                    item.Selected = item.SelectedInt == 1;
                    item.DescView = "\t\t" + item.DescView;
                    model.ViewList.Add(item);
                }
            }
            return model;
        }

        public void Create(PerfilModel model)
        {
            model.AtivoInt = Convert.ToInt32(model.Ativo);
            DAO.Model = model;
            DAO.CreateModel();
            PerfilViewBLL.Create(model);
        }

        public void Update(PerfilModel model)
        {
            model.AtivoInt = Convert.ToInt32(model.Ativo);
            DAO.Model = model;
            DAO.UpdateModel();

            PerfilViewBLL.Update(model);
        }
    }
}
