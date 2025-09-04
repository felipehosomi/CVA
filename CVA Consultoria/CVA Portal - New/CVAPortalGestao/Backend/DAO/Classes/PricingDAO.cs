using DAO.Resources;
using MODEL.Classes;
using System;
using System.Data;
using AUXILIAR;

namespace DAO.Classes
{
    public class PricingDAO
    {
        public DataTable Get(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Pricing_Get);
                    conn.InsertParameter("ID", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_info(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Get_Info);
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
            throw new NotImplementedException();
        }

        public DataTable Get_By_Project(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Pricing_Get_By_Project);
                    conn.InsertParameter("ID_PRJ", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable Get_By_Opportunitty(int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Pricing_Get_By_Opportunitty);
                    conn.InsertParameter("ID_OPR", id);

                    return conn.GetResult();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Insert(PricingModel model, int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Pricing_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", 1);
                    conn.InsertParameter("ID_PRJ", id);
                    conn.InsertParameter("PCT_BKO", model.PorcentagemBackoffice);
                    conn.InsertParameter("PCT_RSC", model.PorcentagemRisco);
                    conn.InsertParameter("PCT_MRG", model.PorcentagemMargem);
                    conn.InsertParameter("PCT_COM", model.PorcentagemComissao);
                    conn.InsertParameter("PCT_IMP", model.PorcentagemImposto);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int Opportunitty_Insert(PricingModel model, int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Opportunitty_Pricing_Insert);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("ID_OPR", id);
                    conn.InsertParameter("PCT_BKO", model.PorcentagemBackoffice);
                    conn.InsertParameter("PCT_RSC", model.PorcentagemRisco);
                    conn.InsertParameter("PCT_MRG", model.PorcentagemMargem);
                    conn.InsertParameter("PCT_COM", model.PorcentagemComissao);
                    conn.InsertParameter("PCT_IMP", model.PorcentagemImposto);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Remove(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(PricingModel model, int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Pricing_Update);
                    conn.InsertParameter("ID", model.Id);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", 1);
                    conn.InsertParameter("ID_PRJ", id);
                    conn.InsertParameter("PCT_BKO", model.PorcentagemBackoffice);
                    conn.InsertParameter("PCT_RSC", model.PorcentagemRisco);
                    conn.InsertParameter("PCT_MRG", model.PorcentagemMargem);
                    conn.InsertParameter("PCT_COM", model.PorcentagemComissao);
                    conn.InsertParameter("PCT_IMP", model.PorcentagemImposto);

                    return conn.GetResult().Success();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Opportunitty_Update(PricingModel model, int id)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.CreateDataAdapter(StoredProcedure.Pricing_UpdateOPRT);
                    conn.InsertParameter("ID", model.Id);
                    conn.InsertParameter("USR", model.User.Id);
                    conn.InsertParameter("STU", 1);
                    conn.InsertParameter("ID_OPR", id);
                    conn.InsertParameter("PCT_BKO", model.PorcentagemBackoffice);
                    conn.InsertParameter("PCT_RSC", model.PorcentagemRisco);
                    conn.InsertParameter("PCT_MRG", model.PorcentagemMargem);
                    conn.InsertParameter("PCT_COM", model.PorcentagemComissao);
                    conn.InsertParameter("PCT_IMP", model.PorcentagemImposto);

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
