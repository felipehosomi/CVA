using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.BLL
{
    public class InitializeBLL
    {
        public static void Initialize()
        {
            try
            {
                UserFieldsBLL.CreateUserFields();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage("Erro ao criar campos de usuário: " + ex.Message);
            }



        }
    }
}
