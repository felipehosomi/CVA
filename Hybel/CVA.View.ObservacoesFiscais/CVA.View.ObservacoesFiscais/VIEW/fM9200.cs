using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CVA.View.ObservacoesFiscais.VIEW
{
    /// <summary>
    /// Botão direito 
    /// </summary>
    public class fM9200 : BaseFormParent
    {
        public fM9200(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                Form form = SBOApp.Application.Forms.ActiveForm;
                if (form.UniqueID.Contains("2000008200"))
                {
                    Matrix mt_Item = (Matrix)form.Items.Item("mt_Item").Specific;
                    mt_Item.DeleteRow(mt_Item.GetNextSelectedRow());
                    if (form.Mode != BoFormMode.fm_UPDATE_MODE && form.Mode != BoFormMode.fm_ADD_MODE)
                    {
                        form.Mode = BoFormMode.fm_UPDATE_MODE;
                    }
                    mt_Item.FlushToDataSource();
                }
            }

            return true;
        }
    }
}
