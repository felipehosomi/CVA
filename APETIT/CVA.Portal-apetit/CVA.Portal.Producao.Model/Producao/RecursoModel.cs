using CVA.AddOn.Common.Controllers;

namespace CVA.Portal.Producao.Model.Producao
{
    public class RecursoModel
    {
        [ModelController]
        public string ResCode { get; set; }

        [ModelController]
        public string ResName { get; set; }

        [ModelController]
        public string ResType { get; set; }
    }
}
