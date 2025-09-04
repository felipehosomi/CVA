using CVA.AddOn.Common.Controllers;
using System;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class TipoEspecificacaoModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Descricao")]
        public string Descricao { get; set; }

        [ModelController(ColumnName = "U_Tipo")]
        public string Tipo { get; set; }

        public string TipoDesc
        {
            get
            {
                switch (Tipo)
                {
                    case "T":
                        return "Texto";
                    case "N":
                        return "Numérico";
                    case "D":
                        return "Data";
                }
                return String.Empty;
            }
        }
    }
}
