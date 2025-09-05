using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{
    public class LoginUserBranchesListView
    {
        public int? empID { get; set; }
        public int? USERID { get; set; }
        public string UserCode { get; set; }
        public int? BPLId { get; set; }
        public string U_SD_Password { get; set; }
        public string U_CodeBar { get; set; }
    }
}
