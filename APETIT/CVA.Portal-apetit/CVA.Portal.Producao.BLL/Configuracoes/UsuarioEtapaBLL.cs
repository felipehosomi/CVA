using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Configuracoes;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Configuracoes
{
    public class UsuarioEtapaBLL : BaseBLL
    {
        public UsuarioEtapaBLL()
        {
            DAO.TableName = "@CVA_USUARIO_ETAPA";
        }

        public List<UsuarioEtapaModel> Get()
        {
            return DAO.RetrieveModelList<UsuarioEtapaModel>();
        }

        public UsuarioEtapaModel Get(string code)
        {
            return DAO.RetrieveModel<UsuarioEtapaModel>($"\"Code\" = '{code}'");
        }

        public List<UsuarioEtapaModel> GetByUsuario(string codUsuario)
        {
            List<UsuarioEtapaModel> list = DAO.FillListFromCommand<UsuarioEtapaModel>(String.Format(Commands.Resource.GetString("UsuarioEtapa_GetByUsuario"), BaseBLL.Database, codUsuario));
            return list;
        }

        public void Create(UsuarioModel model)
        {
            if (model.EtapaList != null)
            {
                foreach (var item in model.EtapaList)
                {
                    if (item.Selected)
                    {
                        UsuarioEtapaModel viewModel = new UsuarioEtapaModel();
                        viewModel.CodUsuario = model.Code;
                        viewModel.CodEtapa = item.CodEtapa;
                        DAO.Model = viewModel;
                        DAO.CreateModel();
                    }
                }
            }
        }

        public void Update(UsuarioModel model)
        {
            DAO.ExecuteNonQuery(String.Format(Commands.Resource.GetString("UsuarioEtapa_Delete"), BaseBLL.Database, model.Code));
            this.Create(model);
        }
    }
}
