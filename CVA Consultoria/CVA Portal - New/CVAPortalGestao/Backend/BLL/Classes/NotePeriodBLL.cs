using DAO.Classes;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Classes
{
    public class NotePeriodBLL
    {
        #region Atributos
        private NotePeriodDAO _notePeriodDAO { get; set; }
        #endregion

        public NotePeriodBLL()
        {
            _notePeriodDAO = new NotePeriodDAO();
        }

        public List<NotePeriod> GetAllPeriods()
        {
            var periodList = new List<NotePeriod>();

            try
            {
                var dtResult = _notePeriodDAO.GetAllPeriods();

                if (dtResult != null)
                {
                    for (int i = 0; i < dtResult.Rows.Count; i++)
                    {
                        periodList.Add(
                            new NotePeriod
                            {
                                Year = Convert.ToInt32(dtResult.Rows[i]["YEAR"]),
                                Month = Convert.ToInt32(dtResult.Rows[i]["MONTH"])
                            }
                        );
                    }
                }

                return periodList;
            }
            catch
            {
                throw;
            }
        }

        public NotePeriod GetPeriod(int year, int month)
        {
            NotePeriod notePeriod = null;

            try
            {
                var dtResult = _notePeriodDAO.GetPeriod(year, month);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    notePeriod = new NotePeriod
                    {
                        Year = Convert.ToInt32(dtResult.Rows[0]["YEAR"]),
                        Month = Convert.ToInt32(dtResult.Rows[0]["MONTH"])
                    };
                }

                return notePeriod;
            }
            catch
            {
                throw;
            }
        }

        public void AddPeriod (int year, int month)
        {
            try
            {
                _notePeriodDAO.AddPeriod(year, month);
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
                _notePeriodDAO.DeletePeriod(year, month);
            }
            catch
            {
                throw;
            }
        }
    }
}
