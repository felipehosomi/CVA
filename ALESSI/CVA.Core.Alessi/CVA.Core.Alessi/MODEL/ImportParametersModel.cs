using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.MODEL
{
    public class ImportParametersModel
    {
        public int ObjType { get; set; }
        public string Layout { get; set; }
        public int ErrorHandler { get; set; }
        public int BPlId { get; set; }
        public string FilePath { get; set; }
    }
}
