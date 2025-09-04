using DAO.Resources;
using MODEL.Classes;
using System;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class OpportunityDAO
    {
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
            catch (Exception ex)
            {
                throw ex;
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
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("NOM", model.Nome);
                    conn.InsertParameter("DAT_PRV", model.DataPrevista);
                    conn.InsertParameter("ID_CLI", model.Cliente.Id);
                    conn.InsertParameter("ID_VEN", model.Vendedor.Id);
                    conn.InsertParameter("ID_TIP_PRJ", model.TipoProjeto.Id);
                    conn.InsertParameter("ID_PCT", model.Temperatura.Id);
                    conn.InsertParameter("VAL_PRJ", model.ValorOportunidade.Replace(',','.'));         
                    conn.InsertParameter("CST_ORC", model.CustoOrcado.Replace(',', '.'));
                    conn.InsertParameter("HRS_ORC", model.HorasOrcadas.Replace(',', '.'));
                    conn.InsertParameter("ING_LIQ", model.IngressoLiquido.Replace(',', '.'));
                    conn.InsertParameter("RSC_GER", model.RiscoGerenciavel.Replace(',', '.'));
                    conn.InsertParameter("ING_TOT", model.IngressoTotal.Replace(',', '.'));
                    conn.InsertParameter("RSP_DSP", model.ResponsavelDespesa);

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
                    conn.InsertParameter("COD", model.Codigo);
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("NOM", model.Nome);
                    conn.InsertParameter("DAT_PRV", model.DataPrevista);
                    conn.InsertParameter("ID_CLI", model.Cliente.Id);
                    conn.InsertParameter("ID_VEN", model.Vendedor.Id);
                    conn.InsertParameter("ID_TIP_PRJ", model.TipoProjeto.Id);
                    conn.InsertParameter("ID_PCT", model.Temperatura.Id);
                    conn.InsertParameter("VAL_PRJ", model.ValorOportunidade.Replace(',','.'));
                    conn.InsertParameter("CST_ORC", model.CustoOrcado.Replace(',', '.'));
                    conn.InsertParameter("HRS_ORC", model.HorasOrcadas.Replace(',', '.'));
                    conn.InsertParameter("ING_LIQ", model.IngressoLiquido.Replace(',', '.'));
                    conn.InsertParameter("RSC_GER", model.RiscoGerenciavel.Replace(',', '.'));
                    conn.InsertParameter("ING_TOT", model.IngressoTotal.Replace(',', '.'));
                    conn.InsertParameter("RSP_DSP", model.ResponsavelDespesa);

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

        public DataTable Generate_NewCode(int id)
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
    }
}