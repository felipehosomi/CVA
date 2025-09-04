using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class FichaInspecaoItemBLL : BaseBLL
    {
        public FichaInspecaoItemBLL()
        {
            DAO.TableName = "@CVA_FICHA_INSPECAO1";
        }

        public List<FichaInspecaoItemModel> Get()
        {
            return DAO.RetrieveModelList<FichaInspecaoItemModel>();
        }

        public FichaInspecaoItemModel Get(string code)
        {
            return DAO.RetrieveModel<FichaInspecaoItemModel>($"\"Code\" = '{code}'");
        }

        public List<FichaInspecaoItemModel> GetByFicha(string codFicha)
        {
            List<FichaInspecaoItemModel> list = DAO.RetrieveModelList<FichaInspecaoItemModel>($"\"U_CodFicha\" = '{codFicha}'");
            TipoEspecificacaoBLL tipoEspecificacaoBLL = new TipoEspecificacaoBLL();
            foreach (var item in list)
            {
                item.TipoCampo = tipoEspecificacaoBLL.Get(item.CodEspec).Tipo;
            }
            return list;
        }

        public void Create(FichaInspecaoModel model)
        {
            if (model.ItemList != null)
            {
                foreach (var itemModel in model.ItemList)
                {
                    itemModel.CodFicha = model.Code;
                    DAO.Model = itemModel;
                    DAO.CreateModel();
                }
            }
            if (model.AmostraList != null)
            {
                foreach (var amostraModel in model.AmostraList)
                {
                    foreach (var itemModel in amostraModel.ItemList)
                    {
                        itemModel.CodFicha = model.Code;
                        DAO.Model = itemModel;
                        DAO.CreateModel();
                    }
                }
            }
        }

        public void Update(FichaInspecaoModel model)
        {
            foreach (var itemModel in model.ItemList)
            {
                itemModel.CodFicha = model.Code;
                DAO.Model = itemModel;
                if (String.IsNullOrEmpty(itemModel.Code))
                {
                    DAO.CreateModel();
                }
                else
                {
                    DAO.UpdateModel();
                }
            }
        }
    }
}
