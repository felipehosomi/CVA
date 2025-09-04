using CVA.Portal.Producao.BLL.Configuracoes;
using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Configuracoes;
using CVA.Portal.Producao.Model.Qualidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Qualidade
{
    public class FichaProdutoBLL : BaseBLL
    {
        public FichaProdutoBLL()
        {
            DAO.TableName = "@CVA_FICHA_PRODUTO";
        }

        public List<FichaProdutoModel> Get()
        {
            return DAO.FillListFromCommand<FichaProdutoModel>(String.Format(Commands.Resource.GetString("FichaProduto_Get"), BaseBLL.Database));
        }

        public FichaProdutoModel Get(string codigo)
        {
            FichaProdutoModel model = DAO.RetrieveModel<FichaProdutoModel>($"\"Code\" = '{codigo}'");
            model.Ativo = model.AtivoInt == 1;
            model.Obrigatorio = model.ObrigatorioInt == 1;
            return model;
        }

        public FichaProdutoModel GetObrigatorio(int docEntryOP, string codItem, string codEtapa, double quantidade)
        {
            FichaProdutoModel fichaProdutoModel = this.GetByItemEtapa(codItem, codEtapa);
            if (fichaProdutoModel.ObrigatorioInt == 1)
            {
                FichaInspecaoBLL fichaInspecaoBLL = new FichaInspecaoBLL();
                if (fichaInspecaoBLL.GetQtdeApontada(docEntryOP, codEtapa, 1) >= quantidade)
                {
                    fichaProdutoModel.ObrigatorioInt = 0;
                }
            }
            return fichaProdutoModel;
        }

        public List<FichaProdutoModel> GetByOP(int nrOP)
        {
            var model = DAO.FillModelList<FichaProdutoModel>(String.Format(Commands.Resource.GetString("FichaProduto_GetByOP"), BaseBLL.Database, nrOP));
            model = model.Where(x => x.StageId != null).ToList();
            return model;
        }

        public FichaProdutoModel GetByItemEtapa(string codItem, string codEtapa)
        {
            FichaProdutoModel model = DAO.FillModel<FichaProdutoModel>(String.Format(Commands.Resource.GetString("FichaProduto_GetByItemEtapa"), BaseBLL.Database, codItem, codEtapa.ToLower()));
            model.Ativo = model.AtivoInt == 1;
            model.Obrigatorio = model.ObrigatorioInt == 1;
            return model;
        }

        public void Create(FichaProdutoModel model)
        {
            string whereDuplicate = $"\"U_CodEtapa\" = '{model.CodEtapa}' AND ((\"U_CodItem\" = '{model.CodItem}' AND \"U_CodGrupo\" IS NULL) OR (\"U_CodGrupo\" = '{model.CodGrupo}' AND \"U_CodItem\" IS NULL))";

            if (DAO.Exists(whereDuplicate))
            {
                throw new Exception("Etapa e item/grupo já cadastrados");
            }

            model.AtivoInt = Convert.ToInt32(model.Ativo);
            model.ObrigatorioInt = Convert.ToInt32(model.Obrigatorio);
            DAO.Model = model;
            DAO.CreateModel();
        }

        public void Update(FichaProdutoModel model)
        {
            if (DAO.Exists($"\"Code\" <> '{model.Code}' AND \"U_CodEtapa\" = '{model.CodEtapa}' AND (\"U_CodItem\" = '{model.CodItem}' OR \"U_CodGrupo\" = {model.CodGrupo})"))
            {
                throw new Exception("Etapa e item/grupo já cadastrados");
            }

            model.AtivoInt = Convert.ToInt32(model.Ativo);
            model.ObrigatorioInt = Convert.ToInt32(model.Obrigatorio);
            DAO.Model = model;
            DAO.UpdateModel();
        }
    }
}
