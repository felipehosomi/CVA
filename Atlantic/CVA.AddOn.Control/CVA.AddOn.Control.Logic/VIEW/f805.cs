using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.AddOn.Control.Logic.BLL;
using CVA.AddOn.Control.Logic.HELPER;
using CVA.AddOn.Control.Logic.MODEL;
using CVA.AddOn.Control.Logic.MODEL.CVACommon;
using SAPbouiCOM;
using System;
using System.Globalization;
using System.Linq;

namespace CVA.AddOn.Control.Logic.VIEW
{
    public class f805 : BaseForm
    {
        public static string[] EditableFields = new string[] { "1", "2", "20", "26", "28", "31", "33", "34", "10000111", "10000112", "10000114", "10000117", "10000119", "10000120", "10000122", "10000123", "10000124" };
        public static string AcctCode { get; set; } = String.Empty;

        #region Constructor
        public f805()
        {
            FormCount++;
        }

        public f805(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f805(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f805(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }
        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK)
                {
                    if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum)
                    {
                        if (ItemEventInfo.ItemUID == "1")
                        {
                            Form.Freeze(true);

                            try
                            {
                                AccountModel model = new AccountModel();
                                model.AcctCode = AcctCode;

                                model.ValidFor = ((SAPbouiCOM.OptionBtn)Form.Items.Item("10000122").Specific).Selected || ((SAPbouiCOM.OptionBtn)Form.Items.Item("10000124").Specific).Selected;
                                model.FrozenFor = ((SAPbouiCOM.OptionBtn)Form.Items.Item("10000123").Specific).Selected || ((SAPbouiCOM.OptionBtn)Form.Items.Item("10000124").Specific).Selected;

                                DateTime date;
                                if (((SAPbouiCOM.OptionBtn)Form.Items.Item("10000122").Specific).Selected)
                                {
                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("10000111").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.ValidFrom = date;
                                    }
                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("10000112").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.ValidTo = date;
                                    }
                                    model.ValidRemarks = ((EditText)Form.Items.Item("10000114").Specific).Value;
                                }
                                else if (((SAPbouiCOM.OptionBtn)Form.Items.Item("10000123").Specific).Selected)
                                {
                                    
                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("10000120").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.FrozenFrom = date;
                                    }
                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("10000119").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.FrozenTo = date;
                                    }
                                    model.FrozenRemarks = ((EditText)Form.Items.Item("10000117").Specific).Value;
                                }
                                else if (((SAPbouiCOM.OptionBtn)Form.Items.Item("10000124").Specific).Selected)
                                {
                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("20").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.ValidFrom = date;
                                    }
                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("26").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.ValidTo = date;
                                    }
                                    model.ValidRemarks = ((EditText)Form.Items.Item("28").Specific).Value;

                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("34").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.FrozenFrom = date;
                                    }
                                    if (DateTime.TryParseExact(((EditText)Form.Items.Item("33").Specific).Value, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
                                    {
                                        model.FrozenTo = date;
                                    }
                                    model.FrozenRemarks = ((EditText)Form.Items.Item("31").Specific).Value;
                                }

                                AccountBLL bll = new AccountBLL();
                                string msg = bll.UpdateStatus(model);
                                if (!String.IsNullOrEmpty(msg))
                                {
                                    SBOApp.Application.SetStatusBarMessage(msg);
                                }
                                else
                                {
                                    SBOApp.Application.SetStatusBarMessage("Dados atualizados com sucesso!", BoMessageTime.bmt_Medium, false);
                                }
                            }
                            catch (Exception ex)
                            {
                                SBOApp.Application.SetStatusBarMessage(ex.Message);
                            }
                            finally
                            {
                                Form.Freeze(false);
                            }
                        }
                    }
                }

            }
            else
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK || ItemEventInfo.EventType == BoEventTypes.et_KEY_DOWN || ItemEventInfo.EventType == BoEventTypes.et_RIGHT_CLICK)
                {
                    if (StaticKeys.Base?.TipoBase == TipoBaseEnum.Comum)
                    {
                        if (!String.IsNullOrEmpty(ItemEventInfo.ItemUID))
                        {
                            if (!EditableFields.Contains(ItemEventInfo.ItemUID))
                            {
                                SBOApp.Application.SetStatusBarMessage("CVA - Alteração do campo não permitida no banco de dados atual");
                                return false;
                            }
                        }
                    }
                }

            }
            return true;
        }
    }
}
