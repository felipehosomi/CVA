using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class StatusContract
    {
        private StatusBLL StatusBLL { get; set; }

        public StatusContract()
        {
            
        }

        public List<StatusModel> GetStatus(int objectId)
        {
            this.StatusBLL = new StatusBLL();
            return StatusBLL.Get(objectId);
        }
    }
}