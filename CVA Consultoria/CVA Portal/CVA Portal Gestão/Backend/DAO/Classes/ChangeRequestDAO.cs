using DAO.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using AUXILIAR;
using MODEL.Classes;

namespace DAO.Classes
{
    public class ChangeRequestDAO
    {
        public int Insert(ChangeRequestModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ChangeRequest_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_PRJ", model.Projeto.Id);
                    conn.InsertParameter("COD", model.Codigo);
                    conn.InsertParameter("VER", model.Versao);
                    conn.InsertParameter("AUT", model.Autor);
                    conn.InsertParameter("SIT", model.Situacao);
                    conn.InsertParameter("GPI", model.GPI);
                    conn.InsertParameter("GPE", model.GPE);
                    conn.InsertParameter("DEP", model.Departamento);
                    conn.InsertParameter("PRO", model.Processo);
                    conn.InsertParameter("DSC", model.Descricao);
                    conn.InsertParameter("MOT", model.Motivos);
                    conn.InsertParameter("REC", model.Recomendacoes);
                    conn.InsertParameter("IPO", model.ImpactosPositivos);
                    conn.InsertParameter("INE", model.ImpactosNegativos);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert_Itens(ChangeRequestModel model, ChangeRequestRecursoModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ChangeRequest_Insert_Itens);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_CHR", model.Id);
                    conn.InsertParameter("ID_FAS", item.RecursoFase);
                    conn.InsertParameter("FAS", item.RecursoFaseNome);
                    conn.InsertParameter("ID_SPC", item.RecursoEspecialidade);
                    conn.InsertParameter("SPC", item.RecursoEspecialidadeNome);
                    conn.InsertParameter("HRS", item.RecursoHorasSolicitadas);
                    conn.InsertParameter("SOL", item.RecursoSolicitante);
                    conn.InsertParameter("NSC", item.RecursoNecessidade);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Add_ChangeRequestHours(ChangeRequestModel model, ChangeRequestRecursoModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ChangeRequest_Add_Hours);
                    conn.InsertParameter("ID_FAS", item.RecursoFase);
                    conn.InsertParameter("ID_SPC", item.RecursoEspecialidade);
                    conn.InsertParameter("HRS", item.RecursoHorasSolicitadas);
                    conn.InsertParameter("ID_PRJ", model.Projeto.Id);
                    conn.InsertParameter("USR", model.User.Id);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(ChangeRequestModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ChangeRequest_Update);
                    conn.InsertParameter("ID", model.Id);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("COD", model.Codigo);
                    conn.InsertParameter("VER", model.Versao);
                    conn.InsertParameter("AUT", model.Autor);
                    conn.InsertParameter("SIT", model.Situacao);
                    conn.InsertParameter("GPI", model.GPI);
                    conn.InsertParameter("GPE", model.GPE);
                    conn.InsertParameter("DEP", model.Departamento);
                    conn.InsertParameter("PRO", model.Processo);
                    conn.InsertParameter("DSC", model.Descricao);
                    conn.InsertParameter("MOT", model.Motivos);
                    conn.InsertParameter("REC", model.Recomendacoes);
                    conn.InsertParameter("IPO", model.ImpactosPositivos);
                    conn.InsertParameter("INE", model.ImpactosNegativos);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Remove_Itens(ChangeRequestModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ChangeRequest_Remove_Itens);
                    conn.InsertParameter("ID_CHR", model.Id);   

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ChangeRequest_Get);
                    conn.InsertParameter("ID", id);
                
                    return conn.GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ChangeRequestModel> Get_for_Project(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.ChangeRequest_Get_for_Project);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult().ToListData<ChangeRequestModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}