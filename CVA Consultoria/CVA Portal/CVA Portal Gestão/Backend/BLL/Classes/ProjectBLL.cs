using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class ProjectBLL
    {
        #region Atributos
        private ProjectDAO _projectDAO { get; set; }
        private StatusBLL _statusBLL { get; set; }
        private SpecialtyBLL _specialtyBLL { get; set; }
        private CollaboratorBLL _collaboratorBLL { get; set; }
        private PricingBLL _pricingBLL { get; set; }
        private ProjectStepBLL _projectStepBLL { get; set; }
        private MemberBLL _memberBLL { get; set; }
        private StepItemBLL _stepItemBLL { get; set; }
        private NoteBLL _noteBLL { get; set; }
        private UserBLL _userBLL { get; set; }
        private StatusReportBLL _statusReportBLL { get; set; }
        #endregion

        #region Construtor
        public ProjectBLL()
        {
            this._projectDAO = new ProjectDAO();
            this._specialtyBLL = new SpecialtyBLL();
            this._collaboratorBLL = new CollaboratorBLL();
            this._pricingBLL = new PricingBLL();
            this._projectStepBLL = new ProjectStepBLL();
            this._memberBLL = new MemberBLL();
            this._userBLL = new UserBLL();
            this._noteBLL = new NoteBLL();
            this._stepItemBLL = new StepItemBLL();
            this._statusReportBLL = new StatusReportBLL();
        }
        #endregion

        public ProjectModel Get(int id)
        {
            try
            {
                var result = _projectDAO.Get(id);
                return LoadModel(result)[0];
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ProjectModel> Get_All()
        {
            try
            {
                var result = _projectDAO.Get_All();
                return LoadSimplifiedModel(result);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ProjectModel> LoadCombo_Project()
        {
            
            var list = new List<ProjectModel>();
            var result = _projectDAO.LoadCombo_Project();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ProjectModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"]),
                    Codigo = result.Rows[i]["Codigo"].ToString(),
                    Tag = result.Rows[i]["Tag"].ToString(),
                    Nome = result.Rows[i]["Nome"].ToString()
                };
                list.Add(model);
            }
            return list;
        }

        public ProjectModel Get_StatusReportParcial(int id)
        {
            var result = _projectDAO.Get_StatusReportParcial(id);

            var model = new ProjectModel
            {
                Id = Convert.ToInt32(result.Rows[0]["Projeto.Id"].ToString()),
                Codigo = result.Rows[0]["Projeto.Codigo"].ToString(),
                Tag = result.Rows[0]["Projeto.Tag"].ToString(),
                Nome = result.Rows[0]["Projeto.Nome"].ToString(),
                StatusReport = new List<StatusReportModel>()
            };

            var statusReport = new StatusReportModel
            {
                Data = Convert.ToDateTime(result.Rows[0]["Data"].ToString()),
                Descricao = result.Rows[0]["Descricao"].ToString(),
                PontosAtencao = result.Rows[0]["PontosAtencao"].ToString(),
                PlanoDeAcao = result.Rows[0]["PlanoDeAcao"].ToString(),
                Conquistas = result.Rows[0]["Conquistas"].ToString(),
                ProximosPassos = result.Rows[0]["ProximosPassos"].ToString(),
                GerenteProjeto = new CollaboratorModel
                {
                    Nome = result.Rows[0]["GerenteProjeto.Nome"].ToString()
                },
                HorasOrcadas = result.Rows[0]["HorasOrcadas"].ToString(),
                HorasConsumidas = result.Rows[0]["HorasConsumidas"].ToString(),
                Concluido = result.Rows[0]["Concluido"].ToString(),
                Fases = new List<StepModel>()
            };

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var fase = new StepModel
                {
                    Nome = result.Rows[i]["Fase.Nome"].ToString(),
                    DataInicio = Convert.ToDateTime(result.Rows[i]["Fase.DataInicio"].ToString()),
                    DataPrevista = Convert.ToDateTime(result.Rows[i]["Fase.DataPrevista"].ToString()),
                    HorasOrcadas = result.Rows[i]["Fase.HorasOrcadas"].ToString(),
                    HorasConsumidas = result.Rows[i]["Fase.HorasConsumidas"].ToString(),
                    Concluido = result.Rows[i]["Fase.Concluido"].ToString()
                };
                statusReport.Fases.Add(fase);
            }
            model.StatusReport.Add(statusReport);

            return model;
        }

        public List<ProjectModel> Get_ByClient(int id)
        {
            try
            {
                return _projectDAO.Get_ByClient(id);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ProjectModel> Get_ByCollaborator(int id)
        {
            try
            {
                return _projectDAO.Get_ByCollaborator(id);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ProjectModel> Get_ByClientAndCollaborator(int idClient, int idCollaborator)
        {
            try
            {
                return _projectDAO.Get_ByClientAndCollaborator(idClient, idCollaborator);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<StepModel> GetSteps()
        {
            return _projectDAO.GetSteps();
        }

        public MessageModel Save(ProjectModel model){
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(ProjectModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                model.Id = _projectDAO.Insert(model);
                if (model.Id > 0)
                {
                    if (model.Membros != null)
                        foreach (var item in model.Membros)
                            _projectDAO.Insert_Members(model, item);

                    foreach (var item in model.Recursos)
                        _projectDAO.Insert_Resources(model, item);

                    if (model.SpecialtyRules != null)
                        foreach (var item in model.SpecialtyRules)
                            _projectDAO.Insert_ResourceRules(model, item);

                   

                    foreach (var item in model.Fases)
                    {
                        var id = _projectDAO.Insert_Steps(model, item);
                        foreach (var item2 in model.Itens)
                        {
                            if (item.StepId == item2.Fase.StepId)
                            {
                                item2.Fase.Id = id;
                                item2.User = new UserModel()
                                {
                                    Id = model.User.Id
                                };
                                _stepItemBLL.Insert(item2);
                            }
                        }
                    }

                    model.Pricing.User = new UserModel()
                    {
                        Id = model.User.Id
                    };
                    _pricingBLL.Insert_ProjectPricing(model.Pricing, model.Id);

                    //foreach (var item in model.StatusReport)
                    //{
                    //    item.Fases = model.Fases;
                    //    _projectDAO.Insert_StatusReport(model, item);
                    //}

                    return MessageBLL.Generate("Projeto inserido.", 0);
                }

                else
                    return MessageBLL.Generate("Falha ao inserir projeto.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        

        public TimeSpan ConvertHours(string horario)
        {
            return TimeSpan.Parse(horario);
        }

        public int ConvertMinutes(string horario)
        {
            return TimeSpan.Parse(horario).Minutes;
        }

        public MessageModel Update(ProjectModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid != null)
                    return isValid;

                if (_projectDAO.Update(model) > 0)
                {
                    if (model.Membros != null)
                    {
                        _projectDAO.Remove_Members(model.Id);
                        foreach (var item in model.Membros)
                            _projectDAO.Insert_Members(model, item);
                    }

                    _projectDAO.Remove_Resources(model.Id);
                    foreach (var item in model.Recursos)
                        _projectDAO.Insert_Resources(model, item);

                    _projectDAO.Remove_ResourceRules(model.Id);
                    if (model.SpecialtyRules != null)
                        foreach (var item in model.SpecialtyRules)
                            _projectDAO.Insert_ResourceRules(model, item);
                 

                    foreach (var item in model.Fases)
                    {
                        _stepItemBLL.Remove_ForProject(item.Id);
                        var result = _projectDAO.Get_SpecificStep(model, item);
                        if (result.Rows.Count <= 0)
                        {
                            var id = _projectDAO.Insert_Steps(model, item);
                            if (model.Itens != null && model.Itens.Count > 0)
                            {
                                foreach (var item2 in model.Itens)
                                {
                                    if (item.StepId == item2.Fase.StepId)
                                    {
                                        item2.Fase.Id = id;
                                        item2.User = new UserModel()
                                        {
                                            Id = model.User.Id
                                        };
                                        var x = _stepItemBLL.Insert(item2);
                                    }
                                }
                            }
                        }
                        else
                        {
                            _projectDAO.Update_Step(model, item);
                            if (model.Itens != null && model.Itens.Count > 0)
                            {
                                foreach (var item2 in model.Itens)
                                {
                                    if (item.StepId == item2.Fase.StepId)
                                    {
                                        item2.Fase.Id = item.Id;
                                        item2.User = new UserModel()
                                        {
                                            Id = model.User.Id
                                        };
                                        _stepItemBLL.Insert(item2);
                                    }
                                }
                            }
                        }
                    }

                    //if (model.StatusReport != null)
                    //    foreach (var item in model.StatusReport)
                    //    {
                    //        if (item.Id == 0)
                    //        {
                    //            item.Fases = model.Fases;
                    //            item.Id = _projectDAO.Insert_StatusReport(model, item);
                    //            item.User = model.User;
                    //            foreach (var item2 in item.Fases)
                    //                _projectDAO.Insert_StatusReport_Steps(item, item2);

                    //        }
                    //    }

                    model.Pricing.User = new UserModel()
                    {
                        Id = model.User.Id
                    };


                    return MessageBLL.Generate("Projeto atualizado.", 0);
                }
                else
                    return MessageBLL.Generate("Falha ao atualizar projeto.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }

        public string Generate_Number(int id)
        {
            try
            {
                return _projectDAO.Generate_Number(id).Rows[0]["Codigo"].ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public MessageModel Remove_Step(StepModel model)
        {
            try
            {
                if (_projectDAO.Remove_Steps(model) == 1)
                    return MessageBLL.Generate("Fase removida.", 0);
                else
                    return MessageBLL.Generate("Ímpossível remover uma fase que possua apontamentos vínculados.", 99, true);
            }
            catch (Exception exception)
            {
                return MessageBLL.Generate(exception.Message, 99, true);
            }
        }


        public MessageModel ValidateModel(ProjectModel model)
        {
            if (model.Codigo == null)
                return MessageBLL.Generate("Obrigatório informar o código", 99, true);
            if (model.Nome == null)
                return MessageBLL.Generate("Obrigatório informar o nome", 99, true);
            if (model.Id == 0 && _projectDAO.ValidateName(model) != 0)
                return MessageBLL.Generate("Já existe um projeto com este nome.", 99, true);
            if (model.Gerente == null)
                return MessageBLL.Generate("Obrigatório informar o gerente", 99, true);
            if (model.Descricao == null)
                return MessageBLL.Generate("Obrigatório informar a descrição", 99, true);
            if (model.DataInicial == null)
                return MessageBLL.Generate("Obrigatório informar a data inicial", 99, true);
            if (model.DataPrevista == null)
                return MessageBLL.Generate("Obrigatório informar a data prevista", 99, true);
            if (model.Cliente == null || model.Cliente.Id <= 0)
                return MessageBLL.Generate("Obrigatório informar o cliente", 99, true);
            if (model.TipoProjeto.Id <= 0)
                return MessageBLL.Generate("Obrigatório informar o tipo de projeto", 99, true);
            if (model.Status == null)
                return MessageBLL.Generate("Obrigatório informar o status", 99, true);
            if (model.ResponsavelDespesa == null)
                return MessageBLL.Generate("Obrigatório informar o responsável pelas despesas", 99, true);
            if (model.Fases == null)
                return MessageBLL.Generate("Obrigatório informar ao menos uma fase", 99, true);
            //if (model.Membros == null)
            //    return MessageBLL.Generate("Obrigatório informar ao menos um membro", 99, true);
            if (model.Recursos == null)
                return MessageBLL.Generate("Obrigatório informar ao menos um colaborador", 99, true);
            if (model.Pricing == null)
                return MessageBLL.Generate("Obrigatório informar o Pricing", 99, true);
            //if (model.Itens == null)
            //    return MessageBLL.Generate("Obrigatório informar as especialidades utilizadas nas fases", 99, true);
            else
                return null;
        }

        public List<ProjectModel> LoadModel(DataTable result)
        {
            var modelList = new List<ProjectModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ProjectModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Tag = result.Rows[i]["Tag"].ToString(),
                    Codigo = result.Rows[i]["Codigo"].ToString(),
                    Nome = result.Rows[i]["Nome"].ToString(),
                    Descricao = result.Rows[i]["Descricao"].ToString(),
                    DataInicial = Convert.ToDateTime(result.Rows[i]["DataInicial"].ToString()),
                    DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString()),
                    ValorProjeto = result.Rows[i]["ValorProjeto"].ToString(),
                    CustoOrcado = result.Rows[i]["CustoOrcado"].ToString(),
                    CustoReal = result.Rows[i]["CustoReal"].ToString(),
                    HorasOrcadas = result.Rows[i]["HorasOrcadas"].ToString(),
                    HorasConsumidas = result.Rows[i]["HorasConsumidas"].ToString(),
                    ResponsavelDespesa = result.Rows[i]["ResponsavelDespesa"].ToString(),
                    IngressoLiquido = result.Rows[i]["IngressoLiquido"].ToString(),
                    RiscoGerenciavel = result.Rows[i]["RiscoGerenciavel"].ToString(),
                    IngressoTotal = result.Rows[i]["IngressoTotal"].ToString()
                };
                model.Status = new StatusModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Status"].ToString())
                };

                model.Cliente = new ClientModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Cliente.Id"].ToString()),
                    Name = result.Rows[i]["Cliente.Nome"].ToString()
                };

                model.TipoProjeto = new ProjectTypeModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["TipoProjeto.Id"].ToString()),
                    Nome = result.Rows[i]["TipoProjeto.Nome"].ToString(),
                    AMS = result.Rows[i]["TipoProjeto.AMS"].ToString(),
                };

                if (!String.IsNullOrEmpty(result.Rows[i]["Gerente.Id"].ToString()))
                    model.Gerente = new CollaboratorModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Gerente.Id"].ToString()),
                        Nome = result.Rows[i]["Gerente.Nome"].ToString()
                    };

                model.Membros = _memberBLL.Get_ProjectMembers(model.Id);
                model.Recursos = _collaboratorBLL.Get_ProjectResources(model.Id);
                model.Fases = _projectStepBLL.Get_ProjectSteps(model.Id);
                model.Pricing = _pricingBLL.Get_By_Project(model.Id);
                model.SpecialtyRules = _projectDAO.Get_SpecialtyRules(model.Id);
                model.Itens = _stepItemBLL.Get_ForProject(model.Id);
                model.StatusReport = _statusReportBLL.Get_All(model.Id);
             
                modelList.Add(model);
            }
            return modelList;
        }

        public List<ProjectModel> LoadSimplifiedModel(DataTable result)
        {
            var modelList = new List<ProjectModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ProjectModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    Tag = result.Rows[i]["Tag"].ToString(),
                    Codigo = result.Rows[i]["Codigo"].ToString(),
                    Nome = result.Rows[i]["Nome"].ToString()
                };

                modelList.Add(model);
            }
            return modelList;
        }

        public string IsAMS(int id)
        {
            return _projectDAO.IsAMS(id).Rows[0]["AMS"].ToString();
        }

        public List<ProjectModel> GetActiveProjects()
        {
            try
            {
                var result = _projectDAO.GetActiveProjects();
                return LoadSimplifiedModel(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ProjectModel> Get_ByUser(int id)
        {
            try
            {
                var result = _projectDAO.Get_ByUser(id);

                var modelList = new List<ProjectModel>();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    var model = new ProjectModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                        User = new UserModel
                        {
                            Id = Convert.ToInt32(result.Rows[i]["User"].ToString())
                        },

                        Status = new StatusModel
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Status"].ToString()),
                            Descricao = result.Rows[i]["Descricao"].ToString()
                        },
                        Codigo = result.Rows[i]["Codigo"].ToString(),
                        Nome = result.Rows[i]["Nome"].ToString(),
                        Descricao = result.Rows[i]["Descricao"].ToString(),
                        DataInicial = Convert.ToDateTime(result.Rows[i]["DataInicial"].ToString()),
                        DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString()),
                        Tag = result.Rows[i]["Tag"].ToString(),

                        Cliente = new ClientModel
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Cliente.Id"].ToString()),
                            Name = result.Rows[i]["Cliente.Nome"].ToString()
                        },

                        TipoProjeto = new ProjectTypeModel
                        {
                            Id = Convert.ToInt32(result.Rows[i]["TipoProjeto.Id"].ToString()),
                            Nome = result.Rows[i]["TipoProjeto.Nome"].ToString(),
                            AMS = result.Rows[i]["TipoProjeto.AMS"].ToString(),
                        }
                    };

                    modelList.Add(model);
                }

                return modelList;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ProjectModel> Filter_Projects(int clientId, string code)
        {
            var modelList = new List<ProjectModel>();

            var result = _projectDAO.Filter_Projects(clientId, code);
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ProjectModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"]),
                    Status = new StatusModel
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Status.Id"].ToString()),
                        Descricao = result.Rows[i]["Status.Descricao"].ToString()
                    },
                    Codigo = result.Rows[i]["Codigo"].ToString(),
                    Nome = result.Rows[i]["Nome"].ToString(),
                    DataInicial = Convert.ToDateTime(result.Rows[i]["DataInicial"]),
                    Tag = result.Rows[i]["Tag"].ToString(),
                    Cliente = new ClientModel
                    {
                        Name = result.Rows[i]["Cliente.Nome"].ToString()
                    },
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public void Update_ProjectCost(int id)
        {
            var custo = 0.0;
            var horas = new TimeSpan();
            var result = _noteBLL.Get_ProjectNotes(id);
            foreach (var note in result)
            {
                custo += Convert.ToDouble(note.Value.ToString());

                var horaFinal = TimeSpan.Parse(note.FinishHour.Value.ToLongTimeString()); // Ajusta a hora final para fazer cálculos
                var horaInicial = TimeSpan.Parse(note.InitHour.Value.ToLongTimeString()); // Ajusta a hora inicial para fazer cálculos

                var intervalo = new TimeSpan();
                if (note.IntervalHour != null) // Caso o intervalo seja diferente de 0
                    intervalo = TimeSpan.Parse(note.IntervalHour.Value.ToLongTimeString()); // Ajusta o intervalo para fazer cálculos
                else // Senão
                    intervalo = new TimeSpan(); // Seta o intervalo como 0

                //Calcula a duração do apontamento sendo salvo
                TimeSpan horasNesteApontamento = (horaFinal - horaInicial);
                if (intervalo != null)
                    horasNesteApontamento = horasNesteApontamento - intervalo;
                horas += horasNesteApontamento;
            }


            _projectDAO.Update_ProjectCost(id, Math.Round(custo, 2), Math.Round(horas.TotalHours, 2));
        }

        public void Update_StepCost(int id1, int id2)
        {
            var custo = 0.0;
            var horas = new TimeSpan();
            var result = _noteBLL.Get_StepNotes(id1, id2);
            foreach (var note in result)
            {
                custo += Convert.ToDouble(note.Value.ToString());

                var horaFinal = TimeSpan.Parse(note.FinishHour.Value.ToLongTimeString()); // Ajusta a hora final para fazer cálculos
                var horaInicial = TimeSpan.Parse(note.InitHour.Value.ToLongTimeString()); // Ajusta a hora inicial para fazer cálculos

                var intervalo = new TimeSpan();
                if (note.IntervalHour != null) // Caso o intervalo seja diferente de 0
                    intervalo = TimeSpan.Parse(note.IntervalHour.Value.ToLongTimeString()); // Ajusta o intervalo para fazer cálculos
                else // Senão
                    intervalo = new TimeSpan(); // Seta o intervalo como 0

                //Calcula a duração do apontamento sendo salvo
                TimeSpan horasNesteApontamento = (horaFinal - horaInicial);
                if (intervalo != null)
                    horasNesteApontamento = horasNesteApontamento - intervalo;
                horas += horasNesteApontamento;
            }


            _projectDAO.Update_StepCost(id1, id2, Math.Round(custo, 2), Math.Round(horas.TotalHours, 2));
        }

        public bool Check_ProjectType(int id)
        {
            var result = _projectDAO.Check_ProjectType(id);

            if (result.Rows.Count == 0)
                return false;
            else
                return true;
        }




        public MessageModel Upd_ProjectResourcesSpecialties(int idProject, int idSpecialty, int idCollaborator)
        {
            _projectDAO.Upd_ProjectResourcesSpecialties(idProject, idSpecialty, idCollaborator);

            return MessageBLL.Generate("Remoção com sucesso", 0);

        }

        public List<SpecialtyRuleModel> Get_SpecialtyRules(int id)
        {
            return _projectDAO.Get_SpecialtyRules_by_PRJ(id);
        }
    }
}
