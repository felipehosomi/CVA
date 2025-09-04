using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

using DAO.Resources;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class CollaboratorDAO 
    {
        public CollaboratorModel Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<CollaboratorModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(CollaboratorModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Insert);

                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("NOM", model.Nome);
                    conn.InsertParameter("EMA", model.Email);
                    conn.InsertParameter("TEL", model.Telefone);
                    conn.InsertParameter("CEL", model.Celular);
                    conn.InsertParameter("DT_NASC", model.DataNascimento);
                    conn.InsertParameter("GEN", model.Genero);
                    conn.InsertParameter("EST_CIV", model.EstadoCivil);
                    conn.InsertParameter("RG", model.RG);
                    conn.InsertParameter("CPF", model.CPF);
                    conn.InsertParameter("CNPJ", model.CNPJ);
                    conn.InsertParameter("PASS", model.Passaporte);
                    conn.InsertParameter("EMI_RG", model.EmissaoRG);
                    conn.InsertParameter("ORG_EXP", model.OrgaoEmissor);
                    conn.InsertParameter("VAL_PAS", model.ValidadePassaporte);
                    conn.InsertParameter("NAT", model.Naturalidade);
                    conn.InsertParameter("NAC", model.Nacionalidade);                  
                    conn.InsertParameter("PRJ_MAN", model.GerenciaProjetos);
                    conn.InsertParameter("TYP", model.Tipo.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoadCombo_Collaborator()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.LoadCombo_Collaborator);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollaboratorTypeModel Get_Type(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Get_Type);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<CollaboratorTypeModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SpecialtyModel> Get_Specialties(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Get_Specialties);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<SpecialtyModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update_Specialties(CollaboratorModel model, SpecialtyModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator1_Update);

                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("COL", model.Id);
                    conn.InsertParameter("FUNC", item.Id);
                    conn.InsertParameter("VAL_PAY", item.Value);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public AddressModel Get_Address(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Get_Address);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<AddressModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Remove_Specialty(SpecialtyModel model, int idCol)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Remove_Specialty);
                    conn.InsertParameter("ID_COL", idCol);
                    conn.InsertParameter("ID_SPC", model.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert_Specialties(CollaboratorModel model, SpecialtyModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Insert_Specialty);

                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("COL", model.Id);
                    conn.InsertParameter("FUNC", item.Id);
                    conn.InsertParameter("VAL_PAY", item.Value);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public DataTable Get_SpecificSpecialty(CollaboratorModel model, SpecialtyModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Get_SpecificSpecialty);
                    conn.InsertParameter("ID_COL", model.Id);
                    conn.InsertParameter("ID_FUNC", item.Id);

                    return conn.GetResult();
                }
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }

        public int Insert_Address(CollaboratorModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Insert_Address);

                    conn.InsertParameter("USR", model.Endereco.User.Id);
                    conn.InsertParameter("STU", model.Endereco.Status.Id);
                    conn.InsertParameter("STT", model.Endereco.Street);
                    conn.InsertParameter("STTNO", model.Endereco.StreetNo);
                    conn.InsertParameter("BLC", model.Endereco.Block);
                    conn.InsertParameter("CIT", model.Endereco.City);
                    conn.InsertParameter("UF", model.Endereco.Uf_Id);
                    conn.InsertParameter("ZIP", model.Endereco.ZipCode);
                    conn.InsertParameter("COL_ID", model.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Update_Address(CollaboratorModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Update_Address);

                    conn.InsertParameter("USR", model.Endereco.User.Id);
                    conn.InsertParameter("STU", model.Endereco.Status.Id);
                    conn.InsertParameter("STT", model.Endereco.Street);
                    conn.InsertParameter("STTNO", model.Endereco.StreetNo);
                    conn.InsertParameter("BLC", model.Endereco.Block);
                    conn.InsertParameter("CIT", model.Endereco.City);
                    conn.InsertParameter("UF", model.Endereco.Uf_Id);
                    conn.InsertParameter("ZIP", model.Endereco.ZipCode);
                    conn.InsertParameter("ADR_ID", model.Endereco.Id);
                    

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
         
        public int VerifyExistence(CollaboratorModel collaborator)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_VerifyDocs);
                    conn.InsertParameter("CNPJ", collaborator.CNPJ);
                    conn.InsertParameter("CPF", collaborator.CPF);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
         
        public List<CollaboratorModel> GetProjectManagerPermission()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_ProjectManager);
                    return conn.GetResult().ToListData<CollaboratorModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public List<CollaboratorModel> Get_Active()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Get_Active);
                    return conn.GetResult().ToListData<CollaboratorModel>();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public DataTable GetCollaboratorByFilters(string name, string cpf, string cnpj, int sector, int specialty, int status)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Search);
                    conn.InsertParameter("NAME", name);
                    conn.InsertParameter("CPF", cpf);
                    conn.InsertParameter("CNPJ", cnpj);
                    conn.InsertParameter("SECTOR", sector);
                    conn.InsertParameter("SPECIALTY", specialty);
                    conn.InsertParameter("STATUS", status);


                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> Get_CollaboratorBySpecialty(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_GetSpecialty);
                    conn.InsertParameter("FUNC", id);
                    return conn.GetResult().ToListData<CollaboratorModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> Get()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Get_All);
                    return conn.GetResult().ToListData<CollaboratorModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollaboratorModel GetForIdUser(int userId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.User_GetCollaboratorId);
                    conn.InsertParameter("ID", userId);

                    return conn.GetResult().ToListData<CollaboratorModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorTypeModel> GetActiveTypes()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.GetActiveTypes);
                   

                    return conn.GetResult().ToListData<CollaboratorTypeModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollaboratorModel GetById(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<CollaboratorModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddressModel GetAddressById(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Address_GetId);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().ToListData<AddressModel>().FirstOrDefault();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(CollaboratorModel collaborator)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_Update);

                    conn.InsertParameter("NOM", collaborator.Nome);
                    conn.InsertParameter("STU", collaborator.Status.Id);
                    conn.InsertParameter("USR", collaborator.User.Id);
                    conn.InsertParameter("EMA", collaborator.Email);
                    conn.InsertParameter("TEL", collaborator.Telefone);
                    conn.InsertParameter("CEL", collaborator.Celular);
                    conn.InsertParameter("DT_NASC", collaborator.DataNascimento);
                    conn.InsertParameter("GEN", collaborator.Genero);
                    conn.InsertParameter("EST_CIV", collaborator.EstadoCivil);
                    conn.InsertParameter("RG", collaborator.RG);
                    conn.InsertParameter("CPF", collaborator.CPF);
                    conn.InsertParameter("CNPJ", collaborator.CNPJ);
                    conn.InsertParameter("PASS", collaborator.Passaporte);
                    conn.InsertParameter("EMI_RG", collaborator.EmissaoRG);
                    conn.InsertParameter("ORG_EXP", collaborator.OrgaoEmissor);
                    conn.InsertParameter("VAL_PAS", collaborator.ValidadePassaporte);
                    conn.InsertParameter("NAT", collaborator.Naturalidade);
                    conn.InsertParameter("NAC", collaborator.Nacionalidade);
                    conn.InsertParameter("PRJ_MAN", collaborator.GerenciaProjetos);
                    conn.InsertParameter("TYP", collaborator.Tipo.Id);                    
                    conn.InsertParameter("ID_ADR", collaborator.Endereco.Id);
                    conn.InsertParameter("ID", collaborator.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CollaboratorModel> Get_NotUser()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Collaborator_NotUser);
                    return conn.GetResult().ToListData<CollaboratorModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void AddParametersToProc(CollaboratorModel collaborator, Connection conn)
        {
            try
            {
                if (collaborator.Id != 0)
                    conn.InsertParameter("ID", collaborator.Id);
                
                conn.InsertParameter("USR", collaborator.User.Id);
                conn.InsertParameter("STU", collaborator.Status.Id);
                conn.InsertParameter("NOM", collaborator.Nome);
                conn.InsertParameter("EMA", collaborator.Email);
                conn.InsertParameter("CEL", collaborator.Celular);
                conn.InsertParameter("CPF", collaborator.CPF);
                conn.InsertParameter("CNPJ", collaborator.CNPJ);
                conn.InsertParameter("DT_NASC", collaborator.DataNascimento);
                conn.InsertParameter("EST_CIV", collaborator.EstadoCivil);
                conn.InsertParameter("GEN", collaborator.Genero);
                conn.InsertParameter("RG", collaborator.RG);
                conn.InsertParameter("EMI_RG", collaborator.EmissaoRG);
                conn.InsertParameter("ORG_EXP", collaborator.OrgaoEmissor);
                conn.InsertParameter("NAT", collaborator.Naturalidade);
                conn.InsertParameter("NAC", collaborator.Nacionalidade);
                conn.InsertParameter("TEL", collaborator.Telefone);
                conn.InsertParameter("PASS", collaborator.Passaporte);
                conn.InsertParameter("VAL_PAS", collaborator.ValidadePassaporte);
                conn.InsertParameter("PRJ_MAN", collaborator.GerenciaProjetos);
                conn.InsertParameter("TYP", collaborator.Tipo.Id);
                conn.InsertParameter("ID_ADR", collaborator.Endereco.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
