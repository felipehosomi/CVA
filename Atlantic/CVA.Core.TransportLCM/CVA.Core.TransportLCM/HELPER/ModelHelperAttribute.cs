using System;

namespace CVA.Core.TransportLCM.HELPER
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
