using BLL;
using DAL.Connection;
using MODEL;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONTROLLER
{
    public class ImportadorPedidoController
    {
        public static void ItemEvents(string FormUID, ref ItemEvent pVal, out bool BubbleEvent, Form form)
        {
            if (!pVal.BeforeAction)
            {
                if (pVal.EventType == BoEventTypes.et_CLICK)
                {
                    if (pVal.ItemUID == "bt_File")
                    {
                        EditText et_File = form.Items.Item("et_File").Specific as EditText;
                        et_File.Value = DialogBlo.OpenFileDialog("Excel|*.xlsx");
                    }
                    if (pVal.ItemUID == "bt_Imp")
                    {
                        try
                        {
                            EditText et_File = form.Items.Item("et_File").Specific as EditText;

                            PedidoCompraBlo importBlo = new PedidoCompraBlo();
                            DataTable dt_Imp = form.DataSources.DataTables.Item("dt_Imp");
                            dt_Imp.Rows.Clear();

                            Arquivo arquivo = importBlo.Import(et_File.Value, dt_Imp, form);
                            if (!String.IsNullOrEmpty(arquivo.MENSAGEMSTATUS))
                            {
                                ConnectionDao.Instance.Application.SetStatusBarMessage(arquivo.MENSAGEMSTATUS);
                            }
                            else
                            {
                                ConnectionDao.Instance.Application.MessageBox("Importação finalizada!");
                            }
                        }
                        catch (Exception ex)
                        {
                            ConnectionDao.Instance.Application.SetStatusBarMessage(ex.Message);
                        }
                        finally
                        {
                            form.Freeze(false);
                        }
                    }
                }
            }
            BubbleEvent = true;
        }
    }
}
