using CVA.Portal.Producao.Model.Qualidade;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class ModeloFichaBLL : BaseBLL
    {
        ModeloFichaItemBLL ModeloFichaItemBLL = new ModeloFichaItemBLL();

        public ModeloFichaBLL()
        {
            DAO.TableName = "@CVA_MODELO_FICHA";
        }

        public List<ModeloFichaModel> Get()
        {
            return DAO.RetrieveModelList<ModeloFichaModel>("", "\"Code\"");
        }

        public ModeloFichaModel Get(string codigo)
        {
            ModeloFichaModel model = DAO.RetrieveModel<ModeloFichaModel>($"\"Code\" = '{codigo}'");
            model.ItemList = ModeloFichaItemBLL.GetByCodModelo(codigo);
            return model;
        }

        public ModeloFichaModel GetByItemEtapa(string codItem, string codEtapa)
        {
            FichaProdutoBLL FichaProdutoBLL = new FichaProdutoBLL();
            FichaProdutoModel fichaProdutoModel = FichaProdutoBLL.GetByItemEtapa(codItem, codEtapa);
            ModeloFichaModel modeloFichaModel = this.Get(fichaProdutoModel.CodModelo);
            return modeloFichaModel;
        }

        public void Create(ModeloFichaModel model)
        {
            DAO.Model = model;
            DAO.CreateModel();
            ModeloFichaItemBLL.Create(model);
        }

        public void Update(ModeloFichaModel model)
        {
            DAO.Model = model;
            DAO.UpdateModel();

            ModeloFichaItemBLL.Update(model);
        }
    }
}
