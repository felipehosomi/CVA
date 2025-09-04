using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Qualidade
{
    public class FichaProdutoModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }

        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }

        [ModelController(ColumnName = "U_Tipo")]
        public string Tipo { get; set; }

        [ModelController(ColumnName = "U_CodEtapa")]
        public string CodEtapa { get; set; }

        [ModelController(ColumnName = "U_CodGrupo")]
        public int CodGrupo { get; set; }

        [ModelController(ColumnName = "U_CodItem")]
        public string CodItem { get; set; }

        [ModelController(ColumnName = "U_CodModelo")]
        public string CodModelo { get; set; }

        [ModelController(ColumnName = "U_Ativo")]
        public int AtivoInt { get; set; }

        [ModelController(ColumnName = "U_Obrigatorio")]
        public int ObrigatorioInt { get; set; }

        public bool Ativo { get; set; }
        public bool Obrigatorio { get; set; }

        public string TipoDesc
        {
            get
            {
                return Tipo == "G" ? "Grupo" : "Item";
            }
        }

        public string EtapaDesc { get; set; }

        public string FichaDesc { get; set; }

        public string ItemDesc { get; set; }

        public string GrupoDesc { get; set; }

        [ModelController(FillOnSelect = true, DataBaseFieldYN = false)]
        public int? StageId { get; set; }

        public string GrupoOuItemDesc
        {
            get
            {
                return Tipo == "G" ? GrupoDesc : ItemDesc;
            }
        }

        public string Item
        {
            get
            {
                return CodItem + " - " + ItemDesc;
            }
        }
    }
}
