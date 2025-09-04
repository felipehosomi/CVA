using DAL.UserInterface;
using SAPbouiCOM;
using System;

namespace BLL
{
    public class MenuBlo
    {
        public static void SetMenus()
        {
            MenusDao.Add("43520", "CVA", "CVA - Importação de pedidos", -1, BoMenuType.mt_POPUP, $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\if_order-history_49596.wmf");
            MenusDao.Add("CVA", "IMPPED", "Importar pedido", 0, BoMenuType.mt_STRING);
            MenusDao.Add("CVA", "IMPNF", "Transformar pedidos em nota", 1, BoMenuType.mt_STRING);
            //MenusDao.Add("CVA", "CANCELDOC", "Cancelar documentos", 2, BoMenuType.mt_STRING);
        }
    }
}
