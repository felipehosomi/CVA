using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class SubPeriodModel : IModel
    {
        public int? CollaboratorId { get; set; }
        public int? ClientId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public string Collaborator { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
    }
}
