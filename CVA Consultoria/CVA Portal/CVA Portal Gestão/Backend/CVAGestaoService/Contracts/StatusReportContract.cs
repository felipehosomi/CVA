using BLL.Classes;
using MODEL.Classes;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class StatusReportContract
    {
        #region Atributos
        private StatusReportBLL _statusReportBLL { get; set; }
        #endregion

        #region Construtor
        public StatusReportContract()
        {
            this._statusReportBLL = new StatusReportBLL();
        }
        #endregion

        public MessageModel Save(StatusReportModel model)
        {
            return _statusReportBLL.Save(model);
        }

        public List<StatusReportModel> Get_All(int id)
        {
            return _statusReportBLL.Get_All(id);
        }

        public string[] Get_ParcialHours(int id, DateTime data)
        {
            return _statusReportBLL.Get_ParcialHours(id, data);
        }
    }
}