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
        private UserBLL _userBLL { get; set; }
        private StatusBLL _statusBLL { get; set; }
        private MemberBLL _memberBLL { get; set; }
        private PricingBLL _pricingBLL { get; set; }
        private StepItemBLL _stepItemBLL { get; set; }
        private SpecialtyBLL _specialtyBLL { get; set; }
        private ProjectStepBLL _projectStepBLL { get; set; }
        private CollaboratorBLL _collaboratorBLL { get; set; }
        private StatusReportBLL _statusReportBLL { get; set; }
        #endregion

        #region Construtor
        public ProjectBLL()
        {
            this._projectDAO = new ProjectDAO();
            this._userBLL = new UserBLL();

            this._memberBLL = new MemberBLL();
            this._pricingBLL = new PricingBLL();
            this._stepItemBLL = new StepItemBLL();
            this._specialtyBLL = new SpecialtyBLL();
            this._projectStepBLL = new ProjectStepBLL();
            this._collaboratorBLL = new CollaboratorBLL();
            this._statusReportBLL = new StatusReportBLL();
        }
        #endregion


        #region Tela Projeto
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


        public MessageModel Save(ProjectModel model)
        {
            return Update(model);
        }

        public MessageModel Update(ProjectModel model)
        {
            try
            {
                var isValid = Validate(model);
                if (isValid != null)
                    return isValid;

                if (_projectDAO.Update(model) > 0)
                {
                    foreach (var item in model.Fases)
                    {
                        if (item.Id == 0)
                            _projectDAO.Insert_Steps(model, item);
                        else
                            _projectDAO.Update_Step(model, item);
                    }

                    _projectDAO.Remove_Resources(model.Id);
                    foreach (var item in model.Recursos)
                    {
                        _projectDAO.Insert_Resources(model, item);
                    }

                    if (model.Membros != null)
                    {
                        _projectDAO.Remove_Members(model.Id);
                        foreach (var item in model.Membros)
                            _projectDAO.Insert_Members(model, item);
                    }

                    if (model.Alteracoes != null)
                    {
                        _projectDAO.Remove_Alteracoes(model.Id);
                        foreach (var item in model.Alteracoes)
                            _projectDAO.Insert_Alteracoes(model, item);
                    }

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

        public MessageModel Validate(ProjectModel model)
        {
            if (model.Codigo == null)
                return MessageBLL.Generate("Obrigatório informar o código", 99, true);
            if (model.Nome == null)
                return MessageBLL.Generate("Obrigatório informar o nome", 99, true);
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
            if (model.Recursos == null)
                return MessageBLL.Generate("Obrigatório informar ao menos um colaborador", 99, true);
            else
                return null;
        }

        public List<ProjectModel> LoadModel(DataTable result)
        {
            var list = new List<ProjectModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new ProjectModel();
                model.Status = new StatusModel();
                model.Cliente = new ClientModel();
                model.TipoProjeto = new ProjectTypeModel();
                model.Gerente = new CollaboratorModel();


                model.Id = Convert.ToInt32(result.Rows[i]["Id"].ToString());
                model.Tag = result.Rows[i]["Tag"].ToString();
                model.Codigo = result.Rows[i]["Codigo"].ToString();
                model.Nome = result.Rows[i]["Nome"].ToString();
                model.Descricao = result.Rows[i]["Descricao"].ToString();
                model.DataInicial = Convert.ToDateTime(result.Rows[i]["DataInicial"].ToString());
                model.DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString());
                model.ResponsavelDespesa = result.Rows[i]["ResponsavelDespesa"].ToString();
                model.ControleHoras = result.Rows[i]["ControleHoras"].ToString();


                model.Status.Id = Convert.ToInt32(result.Rows[i]["Status"].ToString());

                model.Cliente.Id = Convert.ToInt32(result.Rows[i]["Cliente.Id"].ToString());
                model.Cliente.Name = result.Rows[i]["Cliente.Nome"].ToString();

                model.TipoProjeto.Id = Convert.ToInt32(result.Rows[i]["TipoProjeto.Id"].ToString());
                model.TipoProjeto.Nome = result.Rows[i]["TipoProjeto.Nome"].ToString();
                model.TipoProjeto.AMS = result.Rows[i]["TipoProjeto.AMS"].ToString();

                model.Gerente.Id = Convert.ToInt32(result.Rows[i]["Gerente.Id"].ToString());
                model.Gerente.Nome = result.Rows[i]["Gerente.Nome"].ToString();


                model.Fases = Load_Fases(_projectDAO.Get_Fases(model.Id));
                model.Membros = Load_Membros(_projectDAO.Get_Membros(model.Id));
                model.Recursos = Load_Recursos(_projectDAO.Get_Resources(model.Id));
                model.HorasVendidas = Load_HorasVendidas(_projectDAO.Get_SoldHours(model.Id));
                model.Alteracoes = Load_Alteracoes(_projectDAO.Get_Alteracoes(model.Id));

                list.Add(model);
            }

            return list;
        }

        public List<StepModel> Load_Fases(DataTable result)
        {
            var list = new List<StepModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new StepModel();

                model.Id = Convert.ToInt32(result.Rows[i]["Id"].ToString());
                model.StepId = Convert.ToInt32(result.Rows[i]["StepId"].ToString());
                model.Nome = result.Rows[i]["Nome"].ToString();
                model.DataInicio = Convert.ToDateTime(result.Rows[i]["DataInicio"].ToString());
                model.DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString());
                model.HorasOrcadas = result.Rows[i]["HorasOrcadas"].ToString();
                model.HorasConsumidas = result.Rows[i]["HorasConsumidas"].ToString();
                model.Liberada = result.Rows[i]["Liberada"].ToString();

                list.Add(model);
            }
            return list;
        }

        public List<ResourceModel> Load_Recursos(DataTable result)
        {
            var list = new List<ResourceModel>();

            for (var i = 0; i < result.Rows.Count; i++)
            {
                var model = new ResourceModel();

                model.Status = new StatusModel();

                model.Status.Id = Convert.ToInt32(result.Rows[i]["Status.Id"]);
                model.Status.Descricao = result.Rows[i]["Status.Descricao"].ToString();
                model.ColaboradorId = Convert.ToInt32(result.Rows[i]["Colaborador.Id"]);
                model.ColaboradorNome = result.Rows[i]["Colaborador.Nome"].ToString();
                model.FaseId = Convert.ToInt32(result.Rows[i]["Fase.Id"]);
                model.FaseNome = result.Rows[i]["Fase.Nome"].ToString();
                model.EspecialidadeId = Convert.ToInt32(result.Rows[i]["Especialidade.Id"]);
                model.EspecialidadeNome = result.Rows[i]["Especialidade.Name"].ToString();
                model.Horas = Convert.ToInt32(result.Rows[i]["Horas"]);
                model.HorasConsumidas = Convert.ToDouble(result.Rows[i]["HorasConsumidas"]);

                list.Add(model);
            }
            return list;
        }

        public List<MemberModel> Load_Membros(DataTable result)
        {
            var list = new List<MemberModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new MemberModel();
                model.Status = new StatusModel();


                model.Id = Convert.ToInt32(result.Rows[i]["Id"].ToString());
                model.Nome = result.Rows[i]["Nome"].ToString();
                model.Telefone = result.Rows[i]["Telefone"].ToString();
                model.Email = result.Rows[i]["Email"].ToString();
                model.Departamento = result.Rows[i]["Departamento"].ToString();

                model.Status.Id = Convert.ToInt32(result.Rows[i]["Status"].ToString());

                list.Add(model);
            };

            return list;
        }

        public List<SoldHoursModel> Load_HorasVendidas(DataTable result)
        {
            var list = new List<SoldHoursModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new SoldHoursModel();

                model.Horas = Convert.ToInt32(result.Rows[i]["Horas"].ToString());
                model.Especialidade = result.Rows[i]["Especialidade"].ToString();
                model.HorasConsumidas = Convert.ToDouble(result.Rows[i]["HorasConsumidas"].ToString());

                list.Add(model);
            };

            return list;
        }

        private List<AlterationModel> Load_Alteracoes(DataTable result)
        {
            var list = new List<AlterationModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new AlterationModel();

                model.Data = Convert.ToDateTime(result.Rows[i]["Data"].ToString());
                model.Descricao = result.Rows[i]["Descricao"].ToString();
                model.FaseNome = result.Rows[i]["FaseNome"].ToString();
                model.EspecialidadeNome = result.Rows[i]["EspecialidadeNome"].ToString();
                model.ColaboradorNome = result.Rows[i]["ColaboradorNome"].ToString();
                model.HorasAdicionadas = Convert.ToInt32(result.Rows[i]["HorasAdicionadas"].ToString());

                list.Add(model);
            };

            return list;
        }
        #endregion











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

        public List<StepModel> GetSteps()
        {
            return _projectDAO.GetSteps();
        }







        public TimeSpan ConvertHours(string horario)
        {
            return TimeSpan.Parse(horario);
        }

        public int ConvertMinutes(string horario)
        {
            return TimeSpan.Parse(horario).Minutes;
        }






        #region Excluir



        public MessageModel Project_Add_ColToProject(int projectId, int colaboradorId)
        {
            if (_projectDAO.Project_Add_ColToProject(projectId, colaboradorId) > 0)
                return MessageBLL.Generate("Colaborador alocado no projeto.", 0);
            else
                return MessageBLL.Generate("Ocorreu um erro ao alocar recurso no projeto", 99, true);
        }


        public MessageModel Project_Remove_ColToProject(int projectId, int colaboradorId)
        {
            if (_projectDAO.Project_Remove_ColToProject(projectId, colaboradorId) > 0)
                return MessageBLL.Generate("Colaborador desalocado do projeto.", 0);
            else
                return MessageBLL.Generate("Ocorreu um erro ao desaalocar recurso do projeto", 99, true);
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
                //StatusReport = new List<StatusReportModel>()
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
                    HorasConsumidas = result.Rows[i]["Fase.HorasConsumidas"].ToString()
                };
                statusReport.Fases.Add(fase);
            }
            //model.StatusReport.Add(statusReport);

            return model;
        }
        #endregion





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



        public List<ProjectModel> Get_ByUser(int id)
        {
            try
            {
                var list = new List<ProjectModel>();

                var result = _projectDAO.Get_ByUser(id);

                for (int i = 0; i < result.Rows.Count; i++)
                {
                    var model = new ProjectModel();
                    model.User = new UserModel();
                    model.Status = new StatusModel();
                    model.Cliente = new ClientModel();
                    model.TipoProjeto = new ProjectTypeModel();


                    model.Id = Convert.ToInt32(result.Rows[i]["Id"].ToString());
                    model.Codigo = result.Rows[i]["Codigo"].ToString();
                    model.Nome = result.Rows[i]["Nome"].ToString();
                    model.Descricao = result.Rows[i]["Descricao"].ToString();
                    model.DataInicial = Convert.ToDateTime(result.Rows[i]["DataInicial"].ToString());
                    model.DataPrevista = Convert.ToDateTime(result.Rows[i]["DataPrevista"].ToString());
                    model.Tag = result.Rows[i]["Tag"].ToString();

                    model.User.Id = Convert.ToInt32(result.Rows[i]["User"].ToString());

                    model.Status.Id = Convert.ToInt32(result.Rows[i]["Status"].ToString());
                    model.Status.Descricao = result.Rows[i]["Descricao"].ToString();

                    model.Cliente.Id = Convert.ToInt32(result.Rows[i]["Cliente.Id"].ToString());
                    model.Cliente.Name = result.Rows[i]["Cliente.Nome"].ToString();

                    model.TipoProjeto.Id = Convert.ToInt32(result.Rows[i]["TipoProjeto.Id"].ToString());
                    model.TipoProjeto.Nome = result.Rows[i]["TipoProjeto.Nome"].ToString();
                    model.TipoProjeto.AMS = result.Rows[i]["TipoProjeto.AMS"].ToString();

                    list.Add(model);
                }

                return list;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<ProjectModel> Search(ProjectFilterModel model)
        {
            var result = _projectDAO.Search(model);

            return Load_SimplifiedModel(result);
        }

        private List<ProjectModel> Load_SimplifiedModel(DataTable result)
        {
            var list = new List<ProjectModel>();

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
                    }
                };
                list.Add(model);
            }
            return list;
        }


        public Double Get_LimiteHorasFase(NoteModel model)
        {
            var result = _projectDAO.Get_LimiteHorasFase(model);
            return Convert.ToDouble(result.Rows[0]["Limite"].ToString());
        }
    }
}
