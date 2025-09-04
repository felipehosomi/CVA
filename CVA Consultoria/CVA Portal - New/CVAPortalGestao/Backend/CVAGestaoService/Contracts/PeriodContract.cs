using BLL.Classes;
using MODEL.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class PeriodContract
    {
        private SubPeriodBLL _subPeriodBLL { get; set; }

        public PeriodContract()
        {
            this._subPeriodBLL = new SubPeriodBLL();
        }

     
        public MessageModel Open()
        {
            return null;// return _periodBLL.Open();
        }

        public MessageModel SetStatusSubPeriod(int periodId, int statusId)
        {
            return _subPeriodBLL.SetStatus(periodId, statusId);
        }

        public MessageModel SetStatusSubPeriodList(string periodIdList, int statusId)
        {
            return _subPeriodBLL.SetStatusOnList(periodIdList, statusId);
        }
    }
}