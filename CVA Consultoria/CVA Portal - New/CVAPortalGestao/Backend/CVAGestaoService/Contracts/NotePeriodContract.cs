using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class NotePeriodContract
    {
        #region Atributos
        private NotePeriodBLL _notePeriodBLL { get; set; }
        #endregion

        #region Construtor
        public NotePeriodContract()
        {
            this._notePeriodBLL = new NotePeriodBLL();
        }
        #endregion

        public List<NotePeriod> GetAllPeriods()
        {
            return _notePeriodBLL.GetAllPeriods();
        }

        public NotePeriod GetPeriod(int year, int month)
        {
            return _notePeriodBLL.GetPeriod(year, month);
        }

        public void AddPeriod(int year, int month)
        {
            _notePeriodBLL.AddPeriod(year, month);
        }

        public void DeletePeriod(int year, int month)
        {
            _notePeriodBLL.DeletePeriod(year, month);
        }
    }
}