using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelimitedDataHelper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileConfigAttribute : Attribute
    {
        /// <summary>
        /// Ignorar propriedade
        /// </summary>
        public bool Ignore { get; set; } = false;
    }
}
