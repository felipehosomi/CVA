using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class PercentProjectContract
    {
        private PercentProjectBLL _percentProjectBLL { get; set; }

        public PercentProjectContract()
        {
            this._percentProjectBLL = new PercentProjectBLL();
        }

        public List<PercentProjectModel> Get()
        {
            return _percentProjectBLL.Get();
        }
    }
}