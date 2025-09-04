using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class OportunittyObservationModel : IModel
    {
        public DateTime Date { get; set; }
        public string Observation { get; set; }
        public string Colaborador { get; set; }
        public DateTime Data { get; set; }
    }
}
