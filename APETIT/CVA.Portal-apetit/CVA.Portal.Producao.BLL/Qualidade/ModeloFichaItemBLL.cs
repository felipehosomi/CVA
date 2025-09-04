using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class ModeloFichaItemBLL : BaseBLL
    {
        public ModeloFichaItemBLL()
        {
            DAO.TableName = "@CVA_MODELO_FICHA1";
        }

        public List<ModeloFichaItemModel> Get()
        {
            return DAO.RetrieveModelList<ModeloFichaItemModel>();
        }

        public ModeloFichaItemModel Get(string code)
        {
            return DAO.RetrieveModel<ModeloFichaItemModel>($"\"Code\" = '{code}'");
        }

        public List<ModeloFichaItemModel> GetByCodModelo(string codModelo)
        {
            List<ModeloFichaItemModel> list = DAO.RetrieveModelList<ModeloFichaItemModel>($"\"U_CodModelo\" = '{codModelo}'");
            TipoEspecificacaoBLL tipoEspecificacaoBLL = new TipoEspecificacaoBLL();
            foreach (var item in list)
            {
                item.TipoCampo = tipoEspecificacaoBLL.Get(item.CodEspec).Tipo;
            }
            return list;
        }

        public void Create(ModeloFichaModel model)
        {
            foreach (var itemModel in model.ItemList)
            {
                if (itemModel.Deleted == 0)
                {
                    itemModel.CodModelo = model.Code;
                    DAO.Model = itemModel;
                    DAO.CreateModel();
                }
            }
        }

        public void Update(ModeloFichaModel model)
        {
            foreach (var itemModel in model.ItemList)
            {
                itemModel.CodModelo = model.Code;
                DAO.Model = itemModel;
                if (itemModel.Deleted == 0)
                {
                    if (String.IsNullOrEmpty(itemModel.Code))
                    {
                        DAO.CreateModel();
                    }
                    else
                    {
                        DAO.UpdateModel();
                    }
                }
                else
                {
                    DAO.DeleteModel();
                }
            }
        }
    }
}
