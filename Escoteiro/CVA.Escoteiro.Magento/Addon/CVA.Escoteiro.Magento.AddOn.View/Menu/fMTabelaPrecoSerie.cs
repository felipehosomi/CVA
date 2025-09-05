using CVA.AddOn.Common.Forms;
using Picking.Producao.Addon.View;
using System;

namespace CVA.Escoteiro.Magento.AddOn.View
{
    public class fMTabelaPrecoSerie : BaseFormParent
    {
        public fMTabelaPrecoSerie(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                FrmTabelaPrecoSerie form = new FrmTabelaPrecoSerie();
                form.FormID = 1100;
                form.Show("srfFiles\\fTabelaPrecoSerie.srf");
            }

            return true;
        }
    }
}