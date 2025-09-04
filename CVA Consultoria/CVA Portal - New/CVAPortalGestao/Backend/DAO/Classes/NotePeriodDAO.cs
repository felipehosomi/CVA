using AUXILIAR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Classes
{
    public class NotePeriodDAO
    {
        public DataTable GetAllPeriods()
        {
            try
            {
                using (var conn = new Connection())
                {
                    return conn.ExecuteSqlCommand("SELECT * FROM CVA_PERIODOS ORDER BY YEAR, MONTH");
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetPeriod(int year, int month)
        {
            try
            {
                using (var conn = new Connection())
                {
                    return conn.ExecuteSqlCommand($"SELECT TOP 1 * FROM CVA_PERIODOS WHERE YEAR = {year} AND MONTH = {month}");
                }
            }
            catch
            {
                throw;
            }
        }

        public void AddPeriod(int year, int month)
        {
            try
            {
                using (var conn = new Connection())
                {
                    conn.ExecuteSqlCommand($"INSERT INTO CVA_PERIODOS VALUES ({year}, {month})");
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePeriod(int year, int month)
        {
            try
            {
                using (var conn = new Connection())
                {
                    conn.ExecuteSqlCommand($"DELETE FROM CVA_PERIODOS WHERE [YEAR] = {year} AND [MONTH] = {month}");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
