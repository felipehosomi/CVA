using CVA.AddOn.Common.Forms;
using Picking.Producao.Addon.View;
using System;

namespace CVA.Escoteiro.Magento.AddOn.View
{
    public class fMPesquisasSatisfacao : BaseFormParent
    {
        public fMPesquisasSatisfacao(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                FrmPesquisasSatisfacao form = new FrmPesquisasSatisfacao();
                form.FormID = 1101;
                form.Show("srfFiles\\fPesquisasSatisfacao.srf");
            }

            return true;
        }
    }
}