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

        public DataTable Get_All()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_All);
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
                    conn.InsertParameter("VAL_PRJ", model.ValorProjeto);
                    conn.InsertParameter("CST_ORC", model.CustoOrcado);
                    conn.InsertParameter("HRS_ORC", model.HorasOrcadas);
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

        public List<ProjectModel> Get_ByClient(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_ByClient);
                    conn.InsertParameter("ID_CLI", id);

                    return conn.GetResult().ToListData<ProjectModel>();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public List<ProjectModel> Get_ByCollaborator(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_ByCollaborator);
                    conn.InsertParameter("ID_COL", id);

                    return conn.GetResult().ToListData<ProjectModel>();
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

        public List<ProjectModel> Get_ByClientAndCollaborator(int idClient, int idCollaborator)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Get_ByClientAndCollaborator);
                    conn.InsertParameter("ID_CLI", idClient);
                    conn.InsertParameter("ID_COL", idCollaborator);

                    return conn.GetResult().ToListData<ProjectModel>();
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
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("COD", model.Codigo);
                    conn.InsertParameter("NOM", model.Nome);
                    conn.InsertParameter("DSC", model.Descricao);
                    conn.InsertParameter("DAT_INI", model.DataInicial);
                    conn.InsertParameter("DAT_PRV", model.DataPrevista);
                    conn.InsertParameter("ID_CLI", model.Cliente.Id);
                    conn.InsertParameter("ID_TIP_PRJ", model.TipoProjeto.Id);
                    conn.InsertParameter("VAL_PRJ", model.ValorProjeto);
                    conn.InsertParameter("CST_ORC", model.CustoOrcado);
                    conn.InsertParameter("CST_REA", model.CustoReal);
                    conn.InsertParameter("HRS_ORC", model.HorasOrcadas);
                    conn.InsertParameter("HRS_CON", model.HorasConsumidas);
                    conn.InsertParameter("RSP_DSP", model.ResponsavelDespesa);
                    conn.InsertParameter("ING_LIQ", model.IngressoLiquido);
                    conn.InsertParameter("RSC_GER", model.RiscoGerenciavel);
                    conn.InsertParameter("ING_TOT", model.IngressoTotal);
                    conn.InsertParameter("GER", model.Gerente.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        

        public int Remove(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ProjectType_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
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

        public DataTable Generate_Number(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Generate_Number);
                    conn.InsertParameter("ID_TIP_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert_Resources(ProjectModel model, CollaboratorModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Insert_Resources);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("ID_PRJ", model.Id);
                    conn.InsertParameter("ID_COL", item.Id);

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
                    conn.InsertParameter("CST_ORC", item.CustoOrcado);
                    conn.InsertParameter("HOR_ORC", item.HorasOrcadas);
                    conn.InsertParameter("VAL_USD", item.CustoReal);
                    conn.InsertParameter("HOR_USD", item.HorasConsumidas);
                    conn.InsertParameter("PCT_CON", item.Concluido);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
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
                    conn.InsertParameter("CST_ORC", item.CustoOrcado);
                    conn.InsertParameter("HOR_ORC", item.HorasOrcadas);
                    conn.InsertParameter("VAL_USD", item.CustoReal);
                    conn.InsertParameter("HOR_USD", item.HorasConsumidas);
                    conn.InsertParameter("PCT_CON", item.Concluido);


                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Update_StepCost(int id1, int id2, double custo, double horas)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Update_StepCost);
                    conn.InsertParameter("ID_PRJ", id1);
                    conn.InsertParameter("ID", id2);
                    conn.InsertParameter("CST_REA", custo.ToString());
                    conn.InsertParameter("HRS_CON", horas.ToString());

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Update_ProjectCost(int id, double custo, double horas)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Update_ProjectCost);
                    conn.InsertParameter("ID_PRJ", id);
                    conn.InsertParameter("CST_REA", custo.ToString());
                    conn.InsertParameter("HRS_CON", horas.ToString());

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

        public DataTable Filter_Projects(int clientId, string code)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Projet_GetByFilter);
                    conn.InsertParameter("CLI_ID", clientId);
                    conn.InsertParameter("CODE", code);

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

        public DataTable GetActiveProjects()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_GetActive);
                    return conn.GetResult();
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

        public DataTable Check_ProjectType(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Project_Check_ProjectType);
                    conn.InsertParameter("ID_PRJ", id);
                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Add_ProjectResourcesSpecialties(ProjectModel project, ResourceSpecialtyModel specialty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Add_ProjectResourcesSpecialties);
                    conn.InsertParameter("USR", project.User.Id);
                    conn.InsertParameter("ID_PRJ", project.Id);
                    conn.InsertParameter("ID_COL", specialty.CollaboratorId);
                    conn.InsertParameter("ID_SPC", specialty.SpecialtyId);
                    conn.InsertParameter("VAL", specialty.Value);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Upd_ProjectResourcesSpecialties(int idProject, int idSpecialty, int idCollaborator)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Upd_ProjectResourcesSpecialties);
                    conn.InsertParameter("ID_PRJ", idProject);
                    conn.InsertParameter("ID_COL", idCollaborator);
                    conn.InsertParameter("ID_SPC", idSpecialty);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public List<SpecialtyRuleModel> Get_SpecialtyRules_by_PRJ(int id)
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

    }
}