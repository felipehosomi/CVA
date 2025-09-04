using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class UnitMeterContract
    {
        private UnitMeterBLL _unitMeterBLL { get; set; }
        public UnitMeterContract()
        {
            this._unitMeterBLL = new UnitMeterBLL();
        }
        public List<UnitMeterModel> Get()
        {
            return _unitMeterBLL.Get();
        }
    }
}