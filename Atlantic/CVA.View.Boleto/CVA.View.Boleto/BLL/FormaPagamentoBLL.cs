using CVA.View.Boleto.DAO.OPYM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Boleto.BLL
{
    public class FormaPagamentoBLL
    {
        public static MODEL.FormaPagamentoModel GetCode(string linhaDigitavel, string banco)
        {
            string where = String.Empty;
            if (linhaDigitavel.Length == 44)
            {
                where = " WHERE U_CVA_Bol_Conces = 'Y'";
            }
            else if (linhaDigitavel.Length == 47)
            {
                if (linhaDigitavel.Substring(0, 3) == banco.Trim())
                {
                    where = " WHERE U_CVA_Bol_Mesmo_Banco = 'Y'";
                }
                else
                {
                    where = " WHERE U_CVA_Bol_Outro_Banco = 'Y'";
                }
            }
            if (!String.IsNullOrEmpty(where))
            {
                return FormaPagamentoDAO.Get(where);
            }
            return null;
        }
    }
}
