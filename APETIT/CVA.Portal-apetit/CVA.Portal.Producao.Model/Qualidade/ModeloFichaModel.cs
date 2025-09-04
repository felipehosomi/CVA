using CVA.AddOn.Common.Controllers;
using System.Collections.Generic;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class ModeloFichaModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Descricao")]
        public string Descricao { get; set; }

        [ModelController(ColumnName = "U_QtdeAmostra")]
        public int QtdeAmostra { get; set; }

        [ModelController(ColumnName = "U_Observacao")]
        public string Observacao { get; set; }

        [ModelController(ColumnName = "U_ObsPlano")]
        public string ObsPlano { get; set; }

        [ModelController(ColumnName = "U_NrRevisao")]
        public int NrRevisao { get; set; }

        [ModelController(ColumnName = "U_Status")]
        public int Status { get; set; }

        public string StatusDesc
        {
            get
            {
                if (Status == 0)
                {
                    return "Não aprovado";
                }
                else
                {
                    return "Aprovado";
                }
            }
        }

        public List<ModeloFichaItemModel> ItemList { get; set; }
    }
}
