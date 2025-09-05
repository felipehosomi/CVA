namespace TaxDeterminationLoader
{
    using SAPbouiCOM;
    using System;

    internal class frmOKSFTCD
    {
        private EditText oEditSepChar;
        private Form oForm;
        private static readonly Support Sup = new Support();

        public void adjustForm(Form oForm)
        {
            try
            {
                this.oForm = oForm;
                oForm.DataSources.UserDataSources.Add("OKSFTCDECS", BoDataType.dt_SHORT_TEXT, 1);
                Item item = oForm.Items.Item("OKSFTCDECS");
                this.oEditSepChar = (EditText) item.Specific;
                this.oEditSepChar.DataBind.SetBound(true, "", "OKSFTCDECS");
                Sup.setUDFEditValue("OKSFTCD", "OKSFTCDECS", ";");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}

