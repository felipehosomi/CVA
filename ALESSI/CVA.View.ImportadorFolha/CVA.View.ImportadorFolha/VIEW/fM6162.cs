using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.VIEW
{
    /// <summary>
    /// Menu - Form f2000006062 - Tabela NF - Remover Linha
    /// </summary>
    public class fM6162 : BaseFormParent
    {
        public fM6162(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                Form form = SBOApp.Application.Forms.ActiveForm;
                if (form.UniqueID.Contains("2000006062"))
                {
                    Matrix mt_Fields = (Matrix)form.Items.Item("mt_Fields").Specific;
                    mt_Fields.DeleteRow(mt_Fields.GetNextSelectedRow());
                    if (form.Mode != BoFormMode.fm_UPDATE_MODE && form.Mode != BoFormMode.fm_ADD_MODE)
                    {
                        form.Mode = BoFormMode.fm_UPDATE_MODE;
                    }
                    mt_Fields.FlushToDataSource();
                }
            }

            return true;
        }
    }
}
