using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CVA.Portal.Producao.Model.Producao;
using CVA.Portal.Producao.DAO.Resources;

namespace CVA.Portal.Producao.BLL.Producao
{
    public class ApontamentoHRBLL : BaseBLL
    {
        public ApontamentoHRBLL()
        {
            DAO.TableName = "@CVA_APONTAMENTO_HR";
        }

        public void Create(ApontamentoHRModel model)
        {
            DAO.Model = model;
            DAO.CreateModel();
        }

        public void Update(ApontamentoHRModel model)
        {
            DAO.Model = model;
            DAO.UpdateModel();
        }

        public ApontamentoHRModel GetApontamentoAberto(string usuario, int opDocNum, int codEtapa, int recursoLineNum)
        {
            var apontamento = DAO.RetrieveModel<ApontamentoHRModel>(
                $"\"U_CodUsuario\" = '{usuario}' AND \"U_OPDocNum\" = {opDocNum} AND \"U_CodEtapa\" = {codEtapa} AND \"U_RecursoLineNum\" = {recursoLineNum} AND \"U_Duration\" = 0");
            return apontamento;
        }

        public List<ApontamentoHRModel> GetApontamentosAbertos(string usuario)
        {
            var apontamentos = DAO.RetrieveModelList<ApontamentoHRModel>($"\"U_CodUsuario\" = '{usuario}' AND \"U_Duration\" = 0");
            return apontamentos;
        }
    }
}
