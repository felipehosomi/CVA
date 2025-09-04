using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Estoque
{
    public class TransferenciaLogModel
    {
        [ModelController(IsPK = true)]
        public string Code { get; set; }
        [ModelController(FillOnSelect = false)]
        public string Name { get { return Code; } }
        [ModelController(ColumnName = "U_TQDocEntry")]
        public int TQDocEntry { get; set; }
        [ModelController(ColumnName = "U_OPDocNum")]
        public int OPDocNum { get; set; }
        [ModelController(ColumnName = "U_StageId")]
        public int StageId { get; set; }
    }
}
