using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using System;

namespace CVA.View.Hybel.BLL
{
    public class RoteiroBLL
    {
        public static void SetRoteiroLayout(ItemModel itemModel, int op)
        {
            int userId = UserBLL.GetUserId();
            string sql = String.Format(SQL.SimuladorVenda_SetLayout, userId, itemModel.ItemCode, op,
                                            itemModel.EstoqueFisico.ToString().Replace(",", "."),
                                            itemModel.EstoqueReservado.ToString().Replace(",", "."),
                                            itemModel.EstoqueEncomendado.ToString().Replace(",", "."),
                                            itemModel.EstoqueDisponivel.ToString().Replace(",", "."));
            CrudController.ExecuteNonQuery(sql);
        }
    }
}
