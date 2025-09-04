using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using System.Web.Http;
namespace CVA.Cointer.Megasul.API.Controllers
{
    public class UnidadeMedidaController : ApiController
    {
        HanaDAO dao = new HanaDAO();

        public UnidadeMedidaModel Get(int pagina, int quantidade_registros)
        {
            UnidadeMedidaModel unidadeMedidaModel = new UnidadeMedidaModel();
            string sql = Hana.UnidadeMedida_Get;
            sql += $" limit {quantidade_registros} offset {quantidade_registros * (pagina - 1)} ";
            unidadeMedidaModel.unidade_medida = dao.FillListFromCommand<UnidadeMedida>(sql);
            unidadeMedidaModel.quantidade_registros = unidadeMedidaModel.unidade_medida.Count;
            if (unidadeMedidaModel.unidade_medida.Count > 0)
            {
                unidadeMedidaModel.quantidade_registros_total = unidadeMedidaModel.unidade_medida[0].TotalRecords;
            }

            return unidadeMedidaModel;
        }
    }
}
