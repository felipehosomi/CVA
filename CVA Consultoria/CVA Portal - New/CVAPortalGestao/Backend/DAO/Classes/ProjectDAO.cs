using System;
using System.Data;
using System.Collections.Generic;
using DAO.Resources;
using MODEL.Classes;
using AUXILIAR;

namespace DAO.Classes
{
    public class ProjectDAO
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_GetById);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable LoadCombo_Project()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.LoadCombo_Project);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert(ProjectModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("COD", model.Codigo);
                    conn.InsertParameter("NOM", model.Nome);
                    conn.InsertParameter("DSC", model.Descricao);
                    conn.InsertParameter("DAT_INI", model.DataInicial);
                    conn.InsertParameter("DAT_PRV", model.DataPrevista);
                    conn.InsertParameter("ID_CLI", model.Cliente.Id);
                    conn.InsertParameter("ID_TIP_PRJ", model.TipoProjeto.Id);
                    conn.InsertParameter("VAL_PRJ", 0);
                    conn.InsertParameter("CST_ORC", 0);
                    conn.InsertParameter("RSP_DSP", model.ResponsavelDespesa);
                    conn.InsertParameter("GER", model.Gerente.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_Alteracoes(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_Alteracoes);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Remove_Alteracoes(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Remove_Alteracoes);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert_Alteracoes(ProjectModel model, AlterationModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Insert_Alteracoes);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("DAT", item.Data);
                    conn.InsertParameter("DSC", item.Descricao);
                    conn.InsertParameter("FAS", item.FaseNome);
                    conn.InsertParameter("ESP", item.EspecialidadeNome);
                    conn.InsertParameter("COL", item.ColaboradorNome);
                    conn.InsertParameter("HRS", item.HorasAdicionadas);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }



        public int Project_Remove_ColToProject(int projectId, int colaboradorId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Remove_Recurso);
                    conn.InsertParameter("ID_PRJ", projectId);
                    conn.InsertParameter("ID_COL", colaboradorId);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Project_Add_ColToProject(int projectId, int colaboradorId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Add_Recurso);
                    conn.InsertParameter("ID_PRJ", projectId);
                    conn.InsertParameter("ID_COL", colaboradorId);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Update(ProjectModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Update);
                    conn.InsertParameter("ID", model.Id);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("COD", model.Codigo);
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("NOM", model.Nome);
                    conn.InsertParameter("DSC", model.Descricao);
                    conn.InsertParameter("DAT_INI", model.DataInicial);
                    conn.InsertParameter("DAT_PRV", model.DataPrevista);
                    conn.InsertParameter("ID_CLI", model.Cliente.Id);
                    conn.InsertParameter("RSP_DSP", model.ResponsavelDespesa);
                    conn.InsertParameter("ID_TIP_PRJ", model.TipoProjeto.Id);
                    conn.InsertParameter("GER", model.Gerente.Id);
                    conn.InsertParameter("CTRL_HRS", model.ControleHoras);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_StatusReportParcial(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StatusReport_Get_Parcial);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }



        public List<StepModel> GetSteps()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_GetSteps);

                    return conn.GetResult().ToListData<StepModel>();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }



        public int Insert_Members(ProjectModel model, MemberModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Insert_Members);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("NOM", item.Nome);
                    conn.InsertParameter("TEL", item.Telefone);
                    conn.InsertParameter("EML", item.Email);
                    conn.InsertParameter("DPT", item.Departamento);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert_Resources(ProjectModel model, ResourceModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Insert_Resources);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", item.Status.Id);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("ID_COL", item.ColaboradorId);
                    conn.InsertParameter("ID_FAS", item.FaseId);
                    conn.InsertParameter("ID_ESP", item.EspecialidadeId);
                    conn.InsertParameter("HRS", item.Horas);
                    conn.InsertParameter("HRS_CON", item.HorasConsumidas);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_Membros(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Member_Get_ProjectMembers);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_Fases(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectStep_Get_ProjectSteps);
                    conn.InsertParameter("ID_PRJ", id);
                    conn.InsertParameter("ID_COL", 0);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Insert_ResourceRules(ProjectModel model, SpecialtyRuleModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Insert_SpecialtyRules);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("ID_COL", item.CollaboratorId);
                    conn.InsertParameter("ID_SPC", item.SpecialtyId);
                    conn.InsertParameter("VAL", item.Value);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert_Steps(ProjectModel model, StepModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Insert_Steps);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("ID_FAS", item.StepId);
                    conn.InsertParameter("DAT_INI", item.DataInicio);
                    conn.InsertParameter("DAT_PRV", item.DataPrevista);
                    conn.InsertParameter("HOR_ORC", item.HorasOrcadas);
                    conn.InsertParameter("HOR_USD", item.HorasConsumidas);
                    conn.InsertParameter("LBR", item.Liberada);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Update_Step(ProjectModel model, StepModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Update_Step);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("ID_FAS", item.StepId);
                    conn.InsertParameter("DAT_INI", item.DataInicio);
                    conn.InsertParameter("DAT_PRV", item.DataPrevista);
                    conn.InsertParameter("HOR_ORC", item.HorasOrcadas);
                    conn.InsertParameter("LBR", item.Liberada);


                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Remove_Members(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Remove_Members);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Remove_Resources(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Remove_Resources);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Remove_ResourceRules(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Remove_SpecialtyRules);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Remove_Steps(StepModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Remove_Steps);
                    conn.InsertParameter("ID_PRJ", model.ProjectId);
                    conn.InsertParameter("ID_FAS", model.StepId);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Remove_StatusReport(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_StatusReport_Remove);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Search(ProjectFilterModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Projet_GetByFilter);
                    conn.InsertParameter("CLI_ID", model.ClientId);
                    conn.InsertParameter("CODE", model.Code);
                    conn.InsertParameter("STATUS", model.Status);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_ByUser(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_ByUser);
                    conn.InsertParameter("ID_COL", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int ValidateName(ProjectModel project)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_ValidateName);
                    conn.InsertParameter("NOM", project.Nome);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable IsAMS(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_IsAMS);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_SoldHours(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_SoldHours);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public List<SpecialtyRuleModel> Get_SpecialtyRules(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_SpecialtyRules);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult().ToListData<SpecialtyRuleModel>();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_Resources(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {//apagarumdesse
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_Resources);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_SpecificStep(ProjectModel model, StepModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Get_SpecificStep);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("ID_FAS", item.StepId);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_All_StatusReport(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.StatusReport_Get_All);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public DataTable Get_LimiteHorasFase(NoteModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Get_LimiteHorasFase);
                    conn.InsertParameter("ID_PRJ", model.Project.Id);
                    conn.InsertParameter("ID_FAS", model.Step.Id);
                    conn.InsertParameter("ID_ESP", model.Specialty.Id);
                    conn.InsertParameter("USR", model.USR);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public List<AMSTicketModel> GetTicketsByProject(int projectId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.AMSTicket_GetByProject);
                    conn.InsertParameter("PRJ_ID", projectId);

                    return conn.GetResult().ToListData<AMSTicketModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AMSTicketModel> GetTicketsByProject(int projectId, DateTime date)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.AMSTicket_GetByProject);
                    conn.InsertParameter("PRJ_ID", projectId);
                    conn.InsertParameter("DATE", date);

                    return conn.GetResult().ToListData<AMSTicketModel>();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public List<AMSTicketModel> GetTicketsByProject(int projectId, DateTime date, int numTicket)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.AMSTicket_GetByProject);
                    conn.InsertParameter("PRJ_ID", projectId);
                    conn.InsertParameter("DATE", date);
                    conn.InsertParameter("NUM_TICK", numTicket);

                    return conn.GetResult().ToListData<AMSTicketModel>();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}