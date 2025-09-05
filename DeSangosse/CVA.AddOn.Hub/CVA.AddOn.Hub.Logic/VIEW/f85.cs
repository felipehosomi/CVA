using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Hub.Logic.BLL;
using CVA.AddOn.Hub.Logic.MODEL;
using CVA.Hub.BLL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    public class f85 : BaseForm
    {
        private Form form = null;

        #region Constructor
        public f85()
        {
            FormCount++;
        }

        public f85(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f85(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f85(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        #region Show()

        public object Show()
        {
            return null;
        }

        public object Show(string[] args)
        {
            form = SBOApp.Application.OpenForm(BoFormObjectEnum.fo_PickList, "", args[0]);

            return form;

        }
        #endregion

        public override bool ItemEvent()
        {
            this.IsSystemForm = true;
            base.ItemEvent();
            var bll = new PedidoVendaBLL();
            var lotebll = new LotesPedidoVendaBLL();

            if (ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN || ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
            {
                lotebll.ApagaDadosTabelaLote();
            }
            if (ItemEventInfo.BeforeAction && ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "3" && (ItemEventInfo.FormMode == 2 || ItemEventInfo.FormMode == 1))
            {

                var oMatrix = (Matrix)Form.Items.Item("11").Specific;
                string erro = "";

                string PV = ((EditText)oMatrix.GetCellSpecific("74", 1)).Value;

                if (PV == "PV" || PV == "OR")
                {
                    for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                    {
                        var pedido = ((EditText)oMatrix.GetCellSpecific("9", i)).Value.ToString().Replace(".", ",");
                        var efetuado = ((EditText)oMatrix.GetCellSpecific("19", i)).Value.ToString().Replace(".", ",");

                        var dPedido = double.Parse(pedido);

                        if (String.IsNullOrEmpty(efetuado))
                        {
                            efetuado = "0.0";
                        }

                        var dEfetuado = double.Parse(efetuado);

                        if (dEfetuado != dPedido && (PV == "PV" || PV == "OR"))
                        {
                            erro = erro + $"Linha {i}: Favor liberar a quantidade total do item.\n ";
                        }
                    }

                    if (!string.IsNullOrEmpty(erro))
                    {
                        SBOApp.Application.MessageBox(erro);
                        return false;
                    }

                    var list = new List<LotesModel>();

                    for (var i = 1; i <= oMatrix.VisualRowCount; i++)
                    {
                        var model = new LotesModel()
                        {
                            Item = ((EditText)oMatrix.GetCellSpecific("12", i)).Value.ToString()
                        };
                        list.Add(model);
                    }

                    var lote = lotebll.ValidaLotes(list);

                    var buttonOk = (Button)Form.Items.Item("3").Specific;
                    if (buttonOk.Caption == "OK")
                    {
                        lotebll.ApagaDadosTabelaLote();
                    }
                    return lote;
                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && (ItemEventInfo.ItemUID == "4"))
                {
                    lotebll.ApagaDadosTabelaLote();
                }
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_CLOSE)
                {
                    lotebll.ApagaDadosTabelaLote();
                }
            }

            return true;
        }
    }
}

