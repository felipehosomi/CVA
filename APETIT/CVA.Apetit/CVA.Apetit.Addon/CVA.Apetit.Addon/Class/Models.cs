using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Apetit.Addon.Class
{
    /// <summary>
    /// Model genérico com os campos criados a partir de um SELECT
    /// </summary>
    public class GenericModel
    {
        /// <summary>
        /// Campos e valores
        /// </summary>
        public Dictionary<String, object> Fields { get; set; }

        /// <summary>
        /// Retorna valor do campo
        /// </summary>
        /// <param name="fieldName">Nome do campo</param>
        /// <returns></returns>
        public object GetFieldValue(string fieldName)
        {
            return this.Fields[fieldName];
        }

        /// <summary>
        /// Retorna tipo do campo
        /// </summary>
        /// <param name="fieldName">Nome do campo</param>
        /// <returns></returns>
        public Type GetFieldType(string fieldName)
        {
            return this.Fields[fieldName].GetType();
        }
    }
}
