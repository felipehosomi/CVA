using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class MaritalStatusContract
    {
        private MaritalStatusBLL MaritalStatusBLL { get; set; }

        public MaritalStatusContract()
        {
            this.MaritalStatusBLL = new MaritalStatusBLL();
        }
        public List<MaritalStatusModel> Get()
        {
            return MaritalStatusBLL.Get();
        }
    }
}