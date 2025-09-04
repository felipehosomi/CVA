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
        private StatusBLL _statusBLL { get; set; }
        private PricingBLL _pricingBLL { get; set; }
        private OpportunityDAO _opportunityDAO { get; set; }
        #endregion

        #region Construtor
        public OpportunityBLL()
        {
            this._statusBLL = new StatusBLL();
            this._pricingBLL = new PricingBLL();
            this._opportunityDAO = new OpportunityDAO();
        }
        #endregion


        public OpportunityModel Get(int id)
        {
            var result = _opportunityDAO.Get(id);
            return LoadModel(result);
        }

        public List<OpportunityModel> Get_All()
        {
            var result = _opportunityDAO.Get_All();
            return LoadSimplifiedModel(result);
        }

        public List<OpportunityModel> Search(string code, int clientId)
        {
            var result = _opportunityDAO.Search(code, clientId);
            return LoadSearchResult(result);
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
            var isValid = ValidateModel(model);
            if (isValid.Error != null)
                return isValid;

            model.Id = _opportunityDAO.Insert(model);
            if (model.Id == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao inserir oportunidade.", 88, true);

            if (_opportunityDAO.Insert_Description(model) == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao inserir descrição.", 99, true);

            if (_opportunityDAO.Insert_Contact(model) == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao inserir contatos.", 99, true);

            

            if (_opportunityDAO.Insert_Financial(model) == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao inserir dados financeiros.", 99, true);

            if (_opportunityDAO.Insert_ExpenseManager(model) == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao inserir responsável pela política de despesa.", 99, true);

            if (model.Fases != null)
            {
                foreach (var item in model.Fases)
                {
                    item.User = model.User;
                    item.Status = new StatusModel()
                    {
                        Id = model.Status.Id
                    };

                    if (_opportunityDAO.Insert_Steps(model, item) == 0)
                        return MessageBLL.Generate("ATENÇÃO: Falha ao inserir fases.", 99, true);
                }
            }

            if (model.Detalhes != null)
            {
                foreach (var item in model.Detalhes)
                {
                    item.User = model.User;
                    item.Status = new StatusModel()
                    {
                        Id = model.Status.Id
                    };

                    if (_opportunityDAO.Insert_Observations(model, item.Observation, item.Colaborador, item.Data) == 0)
                        return MessageBLL.Generate("ATENÇÃO: Falha ao inserir observações.", 99, true);
                }
            }

            if (model.Pricing != null)
            {
                model.Pricing.User = new UserModel()
                {
                    Id = model.User.Id
                };
                _pricingBLL.InsertOprt(model.Pricing, model.Id);
            }

            return MessageBLL.Generate("SUCESSO: Oportunidade inserida.", model.Id);
        }

        public MessageModel Update(OpportunityModel model)
        {
            var isValid = ValidateModel(model);
            if (isValid.Error != null)
                return isValid;

            if (_opportunityDAO.Update(model) == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao atualizar oportunidade.", 88, true);

            if (_opportunityDAO.UpdateExpenseManager(model) == 0)
                return MessageBLL.Generate("ATENÇÃO: Falha ao atualizar responsável pela despesa.", 99, true);

         
            //Alteração

            if (model.Fases != null)
            {
                var steps = GetOportunitty4_ById(model.Id);
                foreach (var step in steps)
                {
                    step.Status = new StatusModel()
                    {
                        Id = 2
                    };
                    step.User = model.User;
                    if (_opportunityDAO.Update_Steps(step, model.Id) == 0)
                        return MessageBLL.Generate("Erro ao atualizar fases da oportunidade!", 99, true);
                }

                foreach (var step in model.Fases)
                {

                    step.Status = new StatusModel()
                    {
                        Id = 1
                    };
                    step.User = model.User;
                    if (_opportunityDAO.Update_Steps(step, model.Id) == 0)
                        return MessageBLL.Generate("Erro ao atualizar fases da oportunidade!", 99, true);
                }
            }
            //_opportunityDAO.Remove_Steps(model.Id);
            //foreach (var step in model.Steps)
            //{
            //    if (_opportunityDAO.Insert_Steps(model.Id, step) == 0)
            //        return MessageBLL.Generate("ATENÇÃO: Falha ao atualizar fases.", 99, true);
            //}
            //-Alteração


            //Alteração

            if (model.Detalhes != null)
            {
                _opportunityDAO.UpdateObservations(model.Detalhes[0], model.Id);
                foreach (var item in model.Detalhes)
                {
                    _opportunityDAO.Insert_Observations(model, item.Observation, item.Colaborador, item.Data);
                }
            }

            //_opportunityDAO.Remove_Observations(model.Id);
            //foreach (var observation in model.Observations)
            //{
            //    if (_opportunityDAO.Insert_Observations(model.Id, observation) == 0)
            //        return MessageBLL.Generate("ATENÇÃO: Falha ao atualizar observações.", 99, true);
            //}
            //-Alteração


            //Alteração
            
      

            if (model.Pricing != null)
            {
                model.Pricing.User = new UserModel()
                {
                    Id = model.User.Id
                };
                if (model.Pricing.Id > 0)
                    _pricingBLL.Update_OpportunityPricing(model.Pricing, model.Id);
                else
                    _pricingBLL.InsertOprt(model.Pricing, model.Id);
            };

            return MessageBLL.Generate("SUCESSO: Oportunidade atualizada.", 0);
        }

        public MessageModel ValidateModel(OpportunityModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Codigo))
                    return MessageBLL.Generate("ATENÇÃO: Informe o código.", 99, true);
                if (model.Id == 0 && _opportunityDAO.ValidateCode(model.Codigo) > 0)
                    return MessageBLL.Generate("ATENÇÃO: Já existe uma oportunidade cadastrada com este código.", 99, true);
                if (model.Cliente == null || model.Cliente.Id == 0)
                    return MessageBLL.Generate("ATENÇÃO: Informe o cliente.", 99, true);
                if (model.DataPrevista == DateTime.MinValue)
                    return MessageBLL.Generate("ATENÇÃO: Informe a data prevista de início.", 99, true);
                if (String.IsNullOrEmpty(model.Nome))
                    return MessageBLL.Generate("ATENÇÃO: Informe a identificação da oportunidade", 99, true);
                if (String.IsNullOrEmpty(model.Descricao))
                    model.Descricao = "";
                //if (model.ContatoComercial == null)
                //    return MessageBLL.Generate("ATENÇÃO: Informe o contato comercial.", 99, true);
                //if (model.Responsavel == null)
                //    return MessageBLL.Generate("ATENÇÃO: Informe o responsável.", 99, true);
                //if (model.Tecnico == null)
                //    return MessageBLL.Generate("ATENÇÃO: Informe o responsável técnico.", 99, true);
                //if (model.Vendedor == null)
                //    return MessageBLL.Generate("ATENÇÃO: Informe o vendedor.", 99, true);
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
                var model = new OpportunityModel()
                {
                    Id = Convert.ToInt32(result.Rows[0]["Id"]),
                    Status = new StatusModel
                    {
                        Id = Convert.ToInt32(result.Rows[0]["Status.Id"])
                    },
                    Cliente = new ClientModel()
                    {
                        Id = Convert.ToInt32(result.Rows[0]["Cliente.Id"])
                    },

                    Codigo = result.Rows[0]["Codigo"].ToString(),

                    Temperatura = new PercentProjectModel()
                    {
                        Id = Convert.ToInt32(result.Rows[0]["Temperatura.Id"]),
                    },
                    Nome = result.Rows[0]["Nome"].ToString(),
                    ResponsavelDespesa = result.Rows[0]["ResponsavelDespesa"].ToString(),
                    Tag = result.Rows[0]["Tag"].ToString(),
                    TipoProjeto = new ProjectTypeModel
                    {
                        Id = Convert.ToInt32(result.Rows[0]["TipoProjeto.Id"].ToString()),
                    },

                    Responsavel = new CollaboratorModel()
                    {
                        Id = Convert.ToInt32(result.Rows[0]["Responsavel.Id"]),
                    },
                    Vendedor = new CollaboratorModel()
                    {
                        Id = Convert.ToInt32(result.Rows[0]["Vendedor.Id"]),
                    },
                    Tecnico = new CollaboratorModel()
                    {
                        Id = Convert.ToInt32(result.Rows[0]["Tecnico.Id"]),
                    },
                    ContatoComercial = new ContactModel()
                    {
                        Id = Convert.ToInt32(result.Rows[0]["ContatoComercial.Id"].ToString()),
                    },


                    Descricao = result.Rows[0]["Descricao"].ToString(),
                    DataPrevista = Convert.ToDateTime(result.Rows[0]["DataPrevista"].ToString()),


                    Pricing = new PricingModel(),
                    ValorOportunidade = result.Rows[0]["ValorOportunidade"].ToString(),
                    CustoOrcado = result.Rows[0]["CustoOrcado"].ToString(),
                    HorasOrcadas = result.Rows[0]["HorasOrcadas"].ToString(),
                    IngressoLiquido = result.Rows[0]["IngressoLiquido"].ToString(),
                    RiscoGerenciavel = result.Rows[0]["RiscoGerenciavel"].ToString(),
                    IngressoTotal = result.Rows[0]["IngressoTotal"].ToString(),
                    Detalhes = new List<OportunittyObservationModel>(),
                 //   Fases = new List<OportunittyStepsModel>(),
                  
                    Convertida = Convert.ToInt32(result.Rows[0]["Convertida"].ToString())
                };

                model.Pricing = _pricingBLL.Get_By_Opportunitty(model.Id);


               // model.Fases = GetOportunitty4_ById(model.Id);


              
               // model.Detalhes = _opportunityDAO.GetOportunitty5_ByID(model.Id);



                return model;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }




        public List<OpportunityModel> LoadSimplifiedModel(DataTable result)
        {
            var modelList = new List<OpportunityModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new OpportunityModel()
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"]),
                    Tag = result.Rows[i]["Cliente.Tag"].ToString(),
                    Codigo = result.Rows[i]["Codigo"].ToString(),
                    Nome = result.Rows[i]["Nome"].ToString(),
                };

                modelList.Add(model);
            }
            return modelList;
        }

        public List<OpportunityModel> LoadSearchResult(DataTable result)
        {
            var modelList = new List<OpportunityModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new OpportunityModel()
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"]),
                    Codigo = result.Rows[i]["Codigo"].ToString(),
                    Nome = result.Rows[i]["Nome"].ToString(),
                    Cliente = new ClientModel()
                    {
                        Name = result.Rows[i]["Cliente.Nome"].ToString()
                    },

                    ValorOportunidade = result.Rows[i]["ValorOportunidade"].ToString(),

                    Status = new StatusModel
                    {
                        Descricao = result.Rows[i]["Status.Descricao"].ToString()
                    },
                    Temperatura = new PercentProjectModel()
                    {
                        Percent = Convert.ToInt32(result.Rows[i]["Temperatura.Valor"])
                    }
                };

                modelList.Add(model);
            }
            return modelList;
        }


        //---- Revisar métodos abaixo -------------------------------------------------------------------------




        public string NewCodeGenerator()
        {
            return _opportunityDAO.NewCodeGenerator().Codigo;
        }






        public List<StepModel> GetSteps()
        {
            try
            {
                return _opportunityDAO.GetSteps();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }



        public List<OportunittyStepsModel> GetOportunitty4_ById(int oportunittyId)
        {
            try
            {
                var data = _opportunityDAO.GetOportunitty4_ById(oportunittyId);
                var list = new List<OportunittyStepsModel>();
                if (data.Rows.Count > 0)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        var model = new OportunittyStepsModel();
                        model.Id = Convert.ToInt32(data.Rows[i]["Id"]);
                        model.Description = data.Rows[i]["Description"].ToString();
                        model.ProjectStep = new ProjectStepModel()
                        {
                            Id = Convert.ToInt32(data.Rows[i]["ProjectStep"]),
                            Name = data.Rows[i]["Name"].ToString()
                        };
                        model.DateConclusion = Convert.ToDateTime(data.Rows[i]["DateConclusion"]);
                        model.DateInit = Convert.ToDateTime(data.Rows[i]["DateInit"]);

                        list.Add(model);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<OportunittyObservationModel> GetOportunitty5_ByID(int oportunittyId)
        {
            try
            {
                return _opportunityDAO.GetOportunitty5_ByID(oportunittyId);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

       

        public MessageModel CopyToProject(OpportunityModel oportunitty)
        {
            try
            {
                if (oportunitty.Convertida == 0)
                {
                    var copy = _opportunityDAO.ConvertToProject(oportunitty);
                    if (copy != 0)
                        return MessageBLL.Generate("Projeto gerado com sucesso", 1);
                    else
                        return MessageBLL.Generate("Erro ao converter oportunidade para projeto", 99, true);
                }
                else
                    return MessageBLL.Generate("Esta oportunidade já foi convertida em projeto", 99, true);

            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }
    }
}