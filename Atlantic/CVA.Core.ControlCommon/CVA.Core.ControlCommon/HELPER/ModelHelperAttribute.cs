using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.HELPER
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ModelHelperAttribute : Attribute
    {
        /// <summary>
        /// Nome da Coluna na tabela
        /// </summary>
        public string ColumnName { get; set; }
    }
}
