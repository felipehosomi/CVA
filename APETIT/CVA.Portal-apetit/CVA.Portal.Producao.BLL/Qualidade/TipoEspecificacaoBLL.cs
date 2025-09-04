using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class TipoEspecificacaoBLL : BaseBLL
    {
        public TipoEspecificacaoBLL()
        {
            DAO.TableName = "@CVA_TIPO_ESPEC";
        }

        public List<TipoEspecificacaoModel> Get()
        {
            return DAO.RetrieveModelList<TipoEspecificacaoModel>("", "\"Code\"");
        }

        public TipoEspecificacaoModel Get(string codigo)
        {
            TipoEspecificacaoModel model = DAO.RetrieveModel<TipoEspecificacaoModel>($"\"Code\" = '{codigo}'");
            return model;
        }

        public void Create(TipoEspecificacaoModel model)
        {
            DAO.Model = model;
            DAO.CreateModel();
        }

        public void Update(TipoEspecificacaoModel model)
        {
            DAO.Model = model;
            DAO.UpdateModel();
        }

        public void Delete(string codigo)
        {
            object codFicha = DAO.ExecuteScalar(String.Format(Commands.Resource.GetString("ModeloFicha_GetByEspecificacao"), BaseBLL.Database, codigo));
            if (codFicha == null)
            {
                TipoEspecificacaoModel model = new TipoEspecificacaoModel();
                model.Code = codigo;
                DAO.Model = model;
                DAO.DeleteModel();
            }
            else
            {
                throw new Exception("Especificação está vinculada a ficha " + codFicha);
            }
        }
    }
}
