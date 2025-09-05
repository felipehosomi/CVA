using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.ImportacaoLCM.Controller
{
    public class ValidationFields
    {
        public static bool ValidateComboBox(SAPbouiCOM.ComboBox oCombo)
        {
            if (oCombo == null)
                return false;
            if (oCombo.Selected == null)
                return false;
            if (string.IsNullOrEmpty(oCombo.Selected.Value))
                return false;
            return true;
        }

        public static bool ValidateCheckbox(SAPbouiCOM.CheckBox oCheck)
        {
            if (oCheck == null)
                return false;
            return true;
        }

        public static bool ValidateEditText(SAPbouiCOM.EditText oEdit)
        {
            if (oEdit == null)
                return false;
            if (string.IsNullOrEmpty(oEdit.Value))
                return false;
            return true;
        }
    }
}
