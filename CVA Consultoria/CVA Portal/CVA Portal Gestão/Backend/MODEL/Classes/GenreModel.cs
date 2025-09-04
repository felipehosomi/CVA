using MODEL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.Classes
{
    public class GenreModel : IModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
