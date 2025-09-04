using DAO.Resources;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class ClientDAO
    {
        public DataTable LoadCombo()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.LoadCombo_Client);
                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(ClientModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("RAZ", model.Name);
                    conn.InsertParameter("FAN", model.FantasyName);
                    conn.InsertParameter("CNPJ", model.CNPJ);
                    conn.InsertParameter("DSCR", model.Description);
                    if (model.DescriptionAMS == null)
                    {
                        model.DescriptionAMS = String.Empty;
                    }

                    conn.InsertParameter("DSCR_AMS", model.DescriptionAMS);
                    conn.InsertParameter("POL_DESP_LOC", model.LocalPoliticExpense);
                    conn.InsertParameter("TAG", model.Tag);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(ClientModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client_Update);

                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("RAZ", model.Name);
                    conn.InsertParameter("FAN", model.FantasyName);
                    conn.InsertParameter("CNPJ", model.CNPJ);
                    conn.InsertParameter("DSCR", (model.Description == null) ? String.Empty : model.Description);
                    conn.InsertParameter("DSCR_AMS", (model.DescriptionAMS == null) ? String.Empty : model.DescriptionAMS);
                    conn.InsertParameter("POL_DESP_LOC", model.LocalPoliticExpense);
                    conn.InsertParameter("TAG", model.Tag);
                    conn.InsertParameter("ID", model.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public int Add_Contact(ClientModel client, int contactId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Add_Contact);
                    conn.InsertParameter("USR", client.User.Id);
                    conn.InsertParameter("STU", client.Status.Id);
                    conn.InsertParameter("CLI_ID", client.Id);
                    conn.InsertParameter("CON_ID", contactId);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Add_Address(ClientModel client, int addressId)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Add_Address);
                    conn.InsertParameter("USR", client.User.Id);
                    conn.InsertParameter("STU", client.Status.Id);
                    conn.InsertParameter("CLI_ID", client.Id);
                    conn.InsertParameter("END_ID", addressId);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Delete_Contact(ClientModel client)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Delete_Contact);
                    conn.InsertParameter("ID", client.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Add_PoliticExpense(ClientModel model, PoliticExpenseModel item)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Add_PoliticExpense);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", model.Status.Id);
                    conn.InsertParameter("ID_CLI", model.Id);
                    conn.InsertParameter("ID_TIP_DESP", item.Expense.Id);
                    conn.InsertParameter("VLR", item.Value);

                    return conn.GetResult().SuccessId();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Delete_Address(ClientModel client)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Delete_Address);
                    conn.InsertParameter("ID", client.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        

        public int Delete_PoliticExpense(ClientModel client)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client5_Delete);
                    conn.InsertParameter("ID", client.Id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ContactModel> Get_Contacts(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client_GetContacts);
                    conn.InsertParameter("ID_CLI", id);

                    return conn.GetResult().ToListData<ContactModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AddressModel> Get_Address(ClientModel client)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client2_Get);
                    conn.InsertParameter("ID", client.Id);

                    return conn.GetResult().ToListData<AddressModel>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_PoliticExpenses(ClientModel client)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client5_Get);
                    conn.InsertParameter("ID", client.Id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable SearchByCNPJ(string CNPJ)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client_Search_ByCNPJ);
                    conn.InsertParameter("CNPJ", CNPJ);

                    DataTable dt = conn.GetResult();
                    return dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Search(string name)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client_Search);
                    conn.InsertParameter("NAME", name);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Client_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}