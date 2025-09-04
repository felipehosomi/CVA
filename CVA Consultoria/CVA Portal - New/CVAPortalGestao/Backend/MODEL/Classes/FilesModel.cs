using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class FilesModel : IModel
    {
        public string Name { get; set; } 
        public int Size{ get; set; } 
        public string Type{ get; set; } 
        public string Path { get; set; }
    }
}
