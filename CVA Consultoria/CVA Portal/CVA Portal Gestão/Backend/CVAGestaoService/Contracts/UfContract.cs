using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class UfContract
    {
        private UfBLL UfBLL { get; set; }

        public UfContract()
        {
            this.UfBLL = new UfBLL();
        }

        public List<UfModel> Get()
        {
            return this.UfBLL.Get();
        }
    }
}