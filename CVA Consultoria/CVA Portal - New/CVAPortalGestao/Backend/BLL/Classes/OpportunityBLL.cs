using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class OpportunityBLL
    {
        #region Atributos
        private PricingBLL _pricingBLL { get; set; }
        private OpportunityDAO _opportunityDAO { get; set; }
        #endregion

        #region Construtor
        public OpportunityBLL()
        {
            this._pricingBLL = new PricingBLL();
            this._opportunityDAO = new OpportunityDAO();
        }
        #endregion


        public List<OpportunityModel> Search(string code, int clientId)
        {
            var result = _opportunityDAO.Search(code, clientId);
            return LoadSimplifiedModel(result);
        }

        public OpportunityModel Get(int id)
        {
            var result = _opportunityDAO.Get(id);
            return LoadModel(result);
        }

        public MessageModel Save(OpportunityModel model)
        {
            if (model.Id != 0)
                return Update(model);
            else
                return Insert(model);
        }

        public MessageModel Insert(OpportunityModel model)
        {
            var isValid = Validate(model);
            if (isValid.Error != null)
                return isValid;

            model.Id = _opportunityDAO.Insert(model);

            if (model.Id == 0)
                return MessageBLL.Generate("ATENÇÃO: Ocorreu um erro ao gravar a oportunidade.", 99, true);
            if (model.Id == -1)
                return MessageBLL.Generate("ATENÇÃO: Já existe uma oportunidade cadastrada com este código.", 99, true);

            if (model.Pricing != null)
            {
                model.Pricing.User = new UserModel() { Id = model.User.Id };
                _pricingBLL.InsertOprt(model.Pricing, model.Id);
            }

            return MessageBLL.Generate("SUCESSO: Oportunidade inserida.", model.Id);
        }

        public MessageModel Update(OpportunityModel model)
        {
            var isValid = Validate(model);
            if (isValid.Error != null)
                return isValid;

            var Success = _opportunityDAO.Update(model);

            if (Success == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao atualizar oportunidade.", 88, true);
            if (Success == -1)
                return MessageBLL.Generate("ATENÇÃO: Já existe uma oportunidade cadastrada com este código.", 99, true);


            if (model.Pricing != null)
            {
                model.Pricing.User = new UserModel() { Id = model.User.Id };

                if (model.Pricing.Id > 0)
                    _pricingBLL.Update_OpportunityPricing(model.Pricing, model.Id);
                else
                    _pricingBLL.InsertOprt(model.Pricing, model.Id);
            };

            return MessageBLL.Generate("SUCESSO: Oportunidade atualizada.", 0);
        }

        public MessageModel Validate(OpportunityModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Codigo))
                    return MessageBLL.Generate("ATENÇÃO: Informe o código.", 99, true);
                if (model.Cliente == null || model.Cliente.Id == 0)
                    return MessageBLL.Generate("ATENÇÃO: Informe o cliente.", 99, true);
                if (model.DataPrevista == DateTime.MinValue)
                    return MessageBLL.Generate("ATENÇÃO: Informe a data prevista de início.", 99, true);
                if (String.IsNullOrEmpty(model.Nome))
                    return MessageBLL.Generate("ATENÇÃO: Informe a identificação da oportunidade", 99, true);
                if (String.IsNullOrEmpty(model.ResponsavelDespesa))
                    return MessageBLL.Generate("ATENÇÃO: Informe o responsável pela política de despesa.", 99, true);

                return MessageBLL.Generate("Formulário validado com sucesso!", 0);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public OpportunityModel LoadModel(DataTable result)
        {
            try
            {
                var model = new OpportunityModel();

                model.Status = new StatusModel();
                model.Cliente = new ClientModel();
                model.Temperatura = new PercentProjectModel();
                model.TipoProjeto = new ProjectTypeModel();
                model.Vendedor = new CollaboratorModel();
                model.Pricing = new PricingModel();

                model.Id = Convert.ToInt32(result.Rows[0]["Id"]);
                model.Status.Id = Convert.ToInt32(result.Rows[0]["Status.Id"]);
                model.Cliente.Id = Convert.ToInt32(result.Rows[0]["Cliente.Id"]);
                model.Codigo = result.Rows[0]["Codigo"].ToString();
                model.Temperatura.Id = Convert.ToInt32(result.Rows[0]["Temperatura.Id"]);
                model.Nome = result.Rows[0]["Nome"].ToString();
                model.ResponsavelDespesa = result.Rows[0]["ResponsavelDespesa"].ToString();
                model.Tag = result.Rows[0]["Tag"].ToString();
                model.TipoProjeto.Id = Convert.ToInt32(result.Rows[0]["TipoProjeto.Id"].ToString());
                model.Vendedor.Id = Convert.ToInt32(result.Rows[0]["Vendedor.Id"]);
                model.DataPrevista = Convert.ToDateTime(result.Rows[0]["DataPrevista"].ToString());
                model.ValorOportunidade = result.Rows[0]["ValorOportunidade"].ToString();
                model.CustoOrcado = result.Rows[0]["CustoOrcado"].ToString();
                model.HorasOrcadas = result.Rows[0]["HorasOrcadas"].ToString();
                model.IngressoLiquido = result.Rows[0]["IngressoLiquido"].ToString();
                model.RiscoGerenciavel = result.Rows[0]["RiscoGerenciavel"].ToString();
                model.IngressoTotal = result.Rows[0]["IngressoTotal"].ToString();
                model.Convertida = Convert.ToInt32(result.Rows[0]["Convertida"].ToString());


                model.Pricing = _pricingBLL.Get_By_Opportunitty(model.Id);

                return model;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public MessageModel CopyToProject(OpportunityModel model)
        {
            try
            {
                if (model.Convertida == 0)
                {
                    if (_opportunityDAO.ConvertToProject(model) != 0)
                        return MessageBLL.Generate("Projeto gerado com sucesso", 1);
                    else
                        return MessageBLL.Generate("Erro ao converter oportunidade para projeto", 99, true);
                }
                else
                    return MessageBLL.Generate("Esta oportunidade já foi convertida para projeto", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public List<OpportunityModel> LoadSimplifiedModel(DataTable result)
        {
            try
            {
                var list = new List<OpportunityModel>();

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    var model = new OpportunityModel();

                    model.Cliente = new ClientModel();
                    model.Temperatura = new PercentProjectModel();
                    model.Status = new StatusModel();

                    model.Id = Convert.ToInt32(result.Rows[i]["Id"]);
                    model.Codigo = result.Rows[i]["Codigo"].ToString();
                    model.Tag = result.Rows[i]["Tag"].ToString();
                    model.Nome = result.Rows[i]["Nome"].ToString();
                    model.Cliente.Name = result.Rows[i]["Cliente.Nome"].ToString();
                    model.Status.Descricao = result.Rows[i]["Status.Descricao"].ToString();
                    model.Temperatura.Percent = Convert.ToInt32(result.Rows[i]["Temperatura.Valor"]);

                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Generate_NewCode(int id)
        {
            var result = _opportunityDAO.Generate_NewCode(id);
            return result.Rows[0]["Codigo"].ToString();
        }
    }
}