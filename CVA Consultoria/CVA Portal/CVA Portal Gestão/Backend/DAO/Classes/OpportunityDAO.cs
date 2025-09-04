using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class OpportunityDAO
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunity_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_All()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunity_Get_All);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public DataTable Search(string code, int clientId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunity_Search);
                    conn.InsertParameter("COD", code);
                    conn.InsertParameter("ID_CLI", clientId);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Insert(OpportunityModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunity_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("COD", model.Codigo);
                    conn.InsertParameter("CLI_ID", model.Cliente.Id);
                    conn.InsertParameter("EXP_DAT", model.DataPrevista);
                    conn.InsertParameter("PERC_ID", model.Temperatura.Id);
                    conn.InsertParameter("IDEN", model.Nome);
                    conn.InsertParameter("FIL_ID", "3");
                    conn.InsertParameter("AMS", 0);
                    conn.InsertParameter("TOT_TEMP", model.ValorOportunidade);
                    conn.InsertParameter("TOT_OPRT", model.ValorOportunidade);
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("ID_TIP_PRJ", model.TipoProjeto.Id);
                    conn.InsertParameter("VAL_PRJ", model.ValorOportunidade);
                    conn.InsertParameter("CST_ORC", model.CustoOrcado);
                    conn.InsertParameter("HRS_ORC", model.HorasOrcadas);
                    conn.InsertParameter("ING_LIQ", model.IngressoLiquido);
                    conn.InsertParameter("RSC_GER", model.RiscoGerenciavel);
                    conn.InsertParameter("ING_TOT", model.IngressoTotal);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Update(OpportunityModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunity_Update);
                    conn.InsertParameter("ID", model.Id);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("EXP_DAT", model.DataPrevista);
                    conn.InsertParameter("PERC", model.Temperatura.Id);
                    conn.InsertParameter("PRJ_MAN", model.Responsavel.Id);
                    conn.InsertParameter("TECH", model.Tecnico.Id);
                    conn.InsertParameter("CON_COM", model.ContatoComercial.Id);
                    conn.InsertParameter("VAL_NEG", Convert.ToDecimal(model.ValorOportunidade));
                    conn.InsertParameter("TEMP", model.Temperatura.Id);
                    conn.InsertParameter("DSCR", model.Descricao);
                    conn.InsertParameter("IDEN", model.Nome);
                    conn.InsertParameter("CLI_ID", model.Cliente.Id);
                    conn.InsertParameter("AMS", 0);
                    conn.InsertParameter("TOT_TEMP", model.ValorOportunidade);
                    conn.InsertParameter("TOT_OPRT", model.ValorOportunidade);
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("ID_TIP_PRJ", model.TipoProjeto.Id);
                    conn.InsertParameter("VAL_PRJ", model.ValorOportunidade);
                    conn.InsertParameter("CST_ORC", model.CustoOrcado);
                    conn.InsertParameter("HRS_ORC", model.HorasOrcadas);
                    conn.InsertParameter("ING_LIQ", model.IngressoLiquido);
                    conn.InsertParameter("RSC_GER", model.RiscoGerenciavel);
                    conn.InsertParameter("ING_TOT", model.IngressoTotal);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int ConvertToProject(OpportunityModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunity_ConvertForProject);
                    conn.InsertParameter("ID", model.Id);
                    conn.InsertParameter("USR", model.User.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public OpportunityModel NewCodeGenerator()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty_NewCode);
                    return conn.GetResult().ToListData<OpportunityModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientModel GetClient_BYOportunittyId(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty_Client_GetID);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<ClientModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PercentProjectModel GetPercent_BYOportunittyId(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty_GetPercent_ID);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<PercentProjectModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetContacts_BYOportunittyId(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty_Contact_GETID);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public OportunittyFinancialModel GetFinancial_BYOportunittyID(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.GetOportunitty_FinancialID);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<OportunittyFinancialModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

   

        public DataTable GetOportunitty4_ById(int oportunittyId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty_GetStepsID);
                    conn.InsertParameter("ID", oportunittyId);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<OportunittyObservationModel> GetOportunitty5_ByID(int oportunittyId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty5_GetID);
                    conn.InsertParameter("ID", oportunittyId);

                    return conn.GetResult().ToListData<OportunittyObservationModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

       

        public int ValidateCode(string code)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunity_ValidateCode);
                    conn.InsertParameter("COD", code);

                    return conn.GetResult().Rows.Count;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StepModel> GetSteps()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty_GetSteps);

                    return conn.GetResult().ToListData<StepModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert_Description(OpportunityModel oportunitty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty1_Insert);
                    conn.InsertParameter("USR", oportunitty.User.Id);
                    conn.InsertParameter("STU", oportunitty.Status.Id);
                    conn.InsertParameter("OPRT_ID", oportunitty.Id);
                    conn.InsertParameter("DSCR", oportunitty.Descricao);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert_Contact(OpportunityModel oportunitty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty2_Insert);
                    conn.InsertParameter("USR", oportunitty.User.Id);
                    conn.InsertParameter("STU", oportunitty.Status.Id);
                    conn.InsertParameter("PRJ_MAN", 10);
                    conn.InsertParameter("VEND", 23);
                    conn.InsertParameter("TECH", 86);
                    conn.InsertParameter("CON_COM", 13);
                    conn.InsertParameter("OPRT_ID", oportunitty.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Insert_Financial(OpportunityModel oportunitty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty3_Insert);
                    conn.InsertParameter("USR", oportunitty.User.Id);
                    conn.InsertParameter("STU", oportunitty.Status.Id);
                    conn.InsertParameter("VAL_NEG", Convert.ToDouble(oportunitty.ValorOportunidade));
                    conn.InsertParameter("TEMP", oportunitty.Temperatura.Id);
                    conn.InsertParameter("ID_OPRT", oportunitty.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert_Steps(OpportunityModel oportunitty, OportunittyStepsModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty4_Insert);
                    conn.InsertParameter("USR", oportunitty.User.Id);
                    conn.InsertParameter("STU", oportunitty.Status.Id);
                    conn.InsertParameter("OPRT_ID", oportunitty.Id);
                    conn.InsertParameter("FAS_PRJ", item.ProjectStep.Id);
                    conn.InsertParameter("DAT", item.DateConclusion);
                    conn.InsertParameter("DAT_INI", item.DateInit);
                    conn.InsertParameter("DSCR", item.Description);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert_Observations(OpportunityModel oportunitty, string description, string colaborador, DateTime data)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty5_Insert);
                    conn.InsertParameter("USR", oportunitty.User.Id);
                    conn.InsertParameter("STU", oportunitty.Status.Id);
                    conn.InsertParameter("OPRT_ID", oportunitty.Id);
                    conn.InsertParameter("DSCR", description);
                    conn.InsertParameter("COL", colaborador);
                    conn.InsertParameter("DAT", data);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert_ExpenseManager(OpportunityModel oportunitty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty6_Insert);
                    conn.InsertParameter("USR", oportunitty.User.Id);
                    conn.InsertParameter("STU", oportunitty.Status.Id);
                    conn.InsertParameter("OPRT_ID", oportunitty.Id);
                    conn.InsertParameter("DESP", oportunitty.ResponsavelDespesa);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

       

        public int Update_Steps(OportunittyStepsModel projectStepModel, int oportunittyId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty4_Update);

                    conn.InsertParameter("USR", projectStepModel.User.Id);
                    conn.InsertParameter("STU", projectStepModel.Status.Id);
                    conn.InsertParameter("DSCR", projectStepModel.Description);
                    conn.InsertParameter("DAT", projectStepModel.DateInit);
                    conn.InsertParameter("DAT_FIN", projectStepModel.DateConclusion);
                    conn.InsertParameter("ID", oportunittyId);
                    conn.InsertParameter("FAS_PRJ", projectStepModel.ProjectStep.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateObservations(OportunittyObservationModel observations, int oportunittyId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Oportunitty5_Update);
                    conn.InsertParameter("USR", 1);// observations.User.Id);
                    conn.InsertParameter("STU", 1);// observations.Status.Id);
                    conn.InsertParameter("DSCR", "");// observations.Observation);
                    conn.InsertParameter("ID", 1);// observations.Id);
                    conn.InsertParameter("OPRT_ID", oportunittyId);
                    conn.InsertParameter("COL", "");// observations.Colaborador);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateExpenseManager(OpportunityModel oportunitty)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.UpdateExpenseManager);
                    conn.InsertParameter("ID", oportunitty.Id);
                    conn.InsertParameter("USR", oportunitty.User.Id);
                    conn.InsertParameter("STU", oportunitty.Status.Id);
                    conn.InsertParameter("OPRT_ID", oportunitty.Id);
                    conn.InsertParameter("DESP", oportunitty.ResponsavelDespesa);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       
        
    }
}
