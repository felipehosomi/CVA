using MODEL.Classes;
using System;
using System.Data;
using DAO.Resources;
using AUXILIAR;

namespace DAO.Classes
{
    public class MemberDAO 
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    //conn.CreateDataAdapter(StoredProcedure.Member_Get);
                    //conn.InsertParameter("ID", id);

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
                    //conn.CreateDataAdapter(StoredProcedure.Member_Get_All);
                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(MemberModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    //conn.CreateDataAdapter(StoredProcedure.Member_Insert);
                    //conn.InsertParameter("USR", model.User.Id);
                    //conn.InsertParameter("STU", model.Status);
                    //conn.InsertParameter("NOM", model.Nome);
                    //conn.InsertParameter("TEL", model.Telefone);
                    //conn.InsertParameter("EML", model.Email);
                    //conn.InsertParameter("DPT", model.Departamento);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(MemberModel model)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    //conn.CreateDataAdapter(StoredProcedure.Member_Update);
                    //conn.InsertParameter("ID", model.Id);
                    //conn.InsertParameter("USR", model.User.Id);
                    //conn.InsertParameter("STU", model.Status);
                    //conn.InsertParameter("NOM", model.Nome);
                    //conn.InsertParameter("TEL", model.Telefone);
                    //conn.InsertParameter("EML", model.Email);
                    //conn.InsertParameter("DPT", model.Departamento);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Remove(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    //conn.CreateDataAdapter(StoredProcedure.Member_Remove);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
