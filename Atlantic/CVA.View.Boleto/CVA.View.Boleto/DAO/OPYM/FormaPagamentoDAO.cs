using CVA.AddOn.Common.Controllers;
using CVA.View.Boleto.Resources;

namespace CVA.View.Boleto.DAO.OPYM
{
    public class FormaPagamentoDAO
    {
        public static MODEL.FormaPagamentoModel Get(string where)
        {
            CrudController crudController = new CrudController();
            return crudController.FillModelAccordingToSql<MODEL.FormaPagamentoModel>(Query.FormaPagamento_Get + where);
        }
    }
}
