using CVA.Portal.Producao.Model.Configuracoes;
using System;

namespace CVA.Portal.Producao.BLL.Configuracoes
{
    public class ParametrosBLL : BaseBLL
    {
        public ParametrosBLL()
        {
            DAO.TableName = "@CVA_PORTAL_PARAM";
        }

        public ParametrosModel Get(string code)
        {
            ParametrosModel model = DAO.RetrieveModel<ParametrosModel>($"\"Code\" = '{code}'");
            if (String.IsNullOrEmpty(model.Code))
            {
                model.Code = code;
                model.InspecaoMP = "PDN";
                this.Create(model);
            }

            model.PermiteParcial = model.PermiteParcialInt == 1;
            return model;
        }

        public void Create(ParametrosModel model)
        {
            DAO.Model = model;

            DAO.CreateModel();
        }

        public void Update(ParametrosModel model)
        {
            DAO.Model = model;
            model.PermiteParcialInt = Convert.ToInt32(model.PermiteParcial);
            DAO.UpdateModel();
        }
    }
}
