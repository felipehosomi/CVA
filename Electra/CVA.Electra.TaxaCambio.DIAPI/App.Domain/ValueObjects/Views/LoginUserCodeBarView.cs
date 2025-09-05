using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{
    public class LoginUserCodeBarView
    {
        public int? empID { get; set; }       
        public string U_SD_Password { get; set; }
        public string U_CodeBar { get; set; }
    }
}
