using DAL.Connection;
using DAL.UserInterface;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTROLLER
{
    public class MenuEventController
    {
        public static void MenuEvents(ref MenuEvent pVal, out bool BubbleEvent)
        {
            var ret = true;

            try
            {
                if (!pVal.BeforeAction)
                {
                    if (pVal.MenuUID.Equals("IMPPED"))
                    {
                        var oForm = FormsDao.LoadForm($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_importador_pedido.srf", "10001");
                        oForm.Visible = true;
                    }
                    if (pVal.MenuUID.Equals("IMPNF"))
                    {
                        var oForm = FormsDao.LoadForm($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_gerador_nf.srf", "10002");
                        oForm.Visible = true;
                    }
                    if (pVal.MenuUID.Equals("CANCELDOC"))
                    {
                        var oForm = FormsDao.LoadForm($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\cva_cancela_doc.srf", "10003");
                        oForm.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                ConnectionDao.Instance.Application.SetStatusBarMessage(ex.Message);
            }

            BubbleEvent = ret;
        }
    }
}
