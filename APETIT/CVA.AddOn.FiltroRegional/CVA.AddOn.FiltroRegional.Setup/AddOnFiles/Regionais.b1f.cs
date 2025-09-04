using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using SAPbobsCOM;
using System.Windows.Forms;

namespace CVA.AddOn.FiltroRegional
{
    [FormAttribute("CVA.AddOn.FiltroRegional.Regionais", "Regionais.b1f")]
    class Regionais : UserFormBase
    {
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.Matrix Matrix1;
        private SAPbouiCOM.Matrix Matrix2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button0;
        private string sCardCode;
        public SAPbouiCOM.Matrix matrixPN;

        public Regionais(bool filtro, string cardCode)
        {
            carregaInf(filtro, cardCode);
        }

        public Regionais(bool filtro, string cardCode, ref SAPbouiCOM.Matrix originMatrix)
        {
            matrixPN = originMatrix;
            carregaInf(filtro, cardCode);
        }

        private void carregaInf(bool filtro, string cardCode)
        {
            var oCompany = Database.Company;
            var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            string sQuery = "SELECT \"Code\", \"Name\" FROM \"@CVA_CAD_REGIONAIS\" WHERE \"U_Regional\" = 1";
            recordSet.DoQuery(sQuery);

            if (recordSet.RecordCount > 0)
            {
                for (int i = 0; i < recordSet.RecordCount; i++)
                {
                    Matrix0.AddRow();
                    ((SAPbouiCOM.EditText)Matrix0.Columns.Item("Code").Cells.Item(i + 1).Specific).Value = recordSet.Fields.Item("Code").Value.ToString();
                    ((SAPbouiCOM.EditText)Matrix0.Columns.Item("Name").Cells.Item(i + 1).Specific).Value = recordSet.Fields.Item("Name").Value.ToString();
                    recordSet.MoveNext();
                }
            }

            sQuery = "SELECT \"Code\", \"Name\" FROM \"@CVA_CAD_REGIONAIS\" WHERE \"U_Estado\" = 1";
            recordSet.DoQuery(sQuery);

            if (recordSet.RecordCount > 0)
            {
                for (int i = 0; i < recordSet.RecordCount; i++)
                {
                    Matrix1.AddRow();
                    ((SAPbouiCOM.EditText)Matrix1.Columns.Item("Code1").Cells.Item(i + 1).Specific).Value = recordSet.Fields.Item("Code").Value.ToString();
                    ((SAPbouiCOM.EditText)Matrix1.Columns.Item("Name").Cells.Item(i + 1).Specific).Value = recordSet.Fields.Item("Name").Value.ToString();
                    recordSet.MoveNext();
                }
            }

            sQuery = "SELECT \"Code\", \"Name\" FROM \"@CVA_CAD_REGIONAIS\" WHERE \"U_Regiao\" = 1";
            recordSet.DoQuery(sQuery);

            if (recordSet.RecordCount > 0)
            {
                for (int i = 0; i < recordSet.RecordCount; i++)
                {
                    Matrix2.AddRow();
                    ((SAPbouiCOM.EditText)Matrix2.Columns.Item("Code2").Cells.Item(i + 1).Specific).Value = recordSet.Fields.Item("Code").Value.ToString();
                    ((SAPbouiCOM.EditText)Matrix2.Columns.Item("Name").Cells.Item(i + 1).Specific).Value = recordSet.Fields.Item("Name").Value.ToString();
                    recordSet.MoveNext();
                }
            }

            if (filtro)
            {
                Button1.Item.Visible = false;
            }
            else
            {
                sCardCode = cardCode;
                CarregaInf();
                Button0.Item.Visible = false;
            }
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_6").Specific));
            this.Matrix1 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_7").Specific));
            this.Matrix2 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_8").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.Button0.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button0_ClickBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_1").Specific));
            this.Button1.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.Button1_ClickBefore);
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {
        }

        private void Button0_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var oCompany = Database.Company;
            var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sFiltro = string.Empty;
            bool selecRegional = false, selecEstados = false, selecRegiao = false;

            for (int i = 1; i <= Matrix0.VisualRowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)Matrix0.Columns.Item("check").Cells.Item(i).Specific).Checked)
                {
                    sFiltro += "," + ((SAPbouiCOM.EditText)Matrix0.Columns.Item("Code").Cells.Item(i).Specific).Value.ToString().PadLeft(2, '0');
                    selecRegional = true;
                }
            }

            for (int i = 1; i <= Matrix1.VisualRowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)Matrix1.Columns.Item("check1").Cells.Item(i).Specific).Checked)
                {
                    sFiltro += "," + ((SAPbouiCOM.EditText)Matrix1.Columns.Item("Code1").Cells.Item(i).Specific).Value.ToString().PadLeft(2, '0');
                    selecEstados = true;
                }
            }

            for (int i = 1; i <= Matrix2.VisualRowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)Matrix2.Columns.Item("check2").Cells.Item(i).Specific).Checked)
                {
                    sFiltro += "," + ((SAPbouiCOM.EditText)Matrix2.Columns.Item("Code2").Cells.Item(i).Specific).Value.ToString().PadLeft(2, '0');
                    selecRegiao = true;
                }
            }

            if (!string.IsNullOrEmpty(sFiltro) && selecRegional && selecEstados && selecRegiao)
            {

                for (int j = 0; j < SAPbouiCOM.Framework.Application.SBO_Application.Forms.Count; j++)
                {
                    if (SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item(j).Type == 540000900)
                    {

                        SAPbouiCOM.Form oForm = SAPbouiCOM.Framework.Application.SBO_Application.Forms.Item(j);
                        oForm.Freeze(true);

                        for (int i = 1; i <= matrixPN.VisualRowCount; i++)
                        {
                            var CardCode = ((SAPbouiCOM.EditText)matrixPN.Columns.Item("540000001").Cells.Item(i).Specific).Value;

                            string sQuery = $"SELECT STRING_AGG(\"U_Regional\", ',') \"regionaisPN\" FROM \"@CVA_REGIONAIS_PN\" WHERE \"U_CardCode\" = '{CardCode}'";
                            recordSet.DoQuery(sQuery);

                            string regionaisPN = recordSet.Fields.Item("regionaisPN").Value.ToString();
                            List<string> listFiltro = sFiltro.Split(',').ToList();
                            listFiltro.RemoveAt(0);

                            foreach (string s in regionaisPN.Split(','))
                            {
                                string value = s.PadLeft(2, '0');
                                if (listFiltro.Contains(value))
                                {
                                    listFiltro.RemoveAt(listFiltro.IndexOf(value));
                                }
                            }

                            if (listFiltro.Any())
                            {
                                matrixPN.DeleteRow(i);
                                i--;
                            }

                            if (i % 20 == 0)
                            {
                                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage("Filtrando registros. Aguarde!", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                            }
                        }

                        oForm.Freeze(false);

                        break;
                    }
                }

                this.UIAPIRawForm.Close();
            } else
            {
                SAPbouiCOM.Framework.Application.SBO_Application.SetStatusBarMessage("Selecione pelo menos um item de cada tabela.", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        private class ListRegional
        {
            public string code { get; set; }
        }

        private void CarregaInf()
        {
            var oCompany = Database.Company;
            var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            string sQuery = $"SELECT \"U_Regional\" FROM \"@CVA_REGIONAIS_PN\" WHERE \"U_CardCode\" = '{sCardCode}';";

            recordSet.DoQuery(sQuery);

            List<ListRegional> selection = new List<ListRegional>();

            while (!recordSet.EoF)
            {
                selection.Add(new ListRegional() { code = recordSet.Fields.Item("U_Regional").Value.ToString() });

                recordSet.MoveNext();
            }

            for (int i = 1; i <= Matrix0.VisualRowCount; i++)
            {
                string str = ((SAPbouiCOM.EditText)Matrix0.Columns.Item("Code").Cells.Item(i).Specific).Value.ToString();
                var strResult = selection.Where(x => x.code == str);
                if (strResult.Any())
                {
                    ((SAPbouiCOM.CheckBox)Matrix0.Columns.Item("check").Cells.Item(i).Specific).Checked = true;
                }
            }

            for (int i = 1; i <= Matrix1.VisualRowCount; i++)
            {
                string str = ((SAPbouiCOM.EditText)Matrix1.Columns.Item("Code1").Cells.Item(i).Specific).Value.ToString();
                var strResult = selection.Where(x => x.code == str);
                if (strResult.Any())
                {
                    ((SAPbouiCOM.CheckBox)Matrix1.Columns.Item("check1").Cells.Item(i).Specific).Checked = true;
                }
            }

            for (int i = 1; i <= Matrix2.VisualRowCount; i++)
            {
                string str = ((SAPbouiCOM.EditText)Matrix2.Columns.Item("Code2").Cells.Item(i).Specific).Value.ToString();
                var strResult = selection.Where(x => x.code == str);
                if (strResult.Any())
                {
                    ((SAPbouiCOM.CheckBox)Matrix2.Columns.Item("check2").Cells.Item(i).Specific).Checked = true;
                }
            }
        }

        private void Button1_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var oCompany = Database.Company;
            var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

            string sQuery = $"DELETE FROM \"@CVA_REGIONAIS_PN\" WHERE \"U_CardCode\" = '{sCardCode}';";
            recordSet.DoQuery(sQuery);

            //Salva registros de Regionais
            for (int i = 1; i <= Matrix0.VisualRowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)Matrix0.Columns.Item("check").Cells.Item(i).Specific).Checked)
                {
                    var regional = ((SAPbouiCOM.EditText)Matrix0.Columns.Item("Code").Cells.Item(i).Specific).Value;
                    sQuery = "INSERT INTO \"@CVA_REGIONAIS_PN\" (\"Code\", \"Name\", \"U_Regional\", \"U_CardCode\") " +
                       $"VALUES ((SELECT COALESCE(MAX(TO_INT(\"Code\")) + 1, 1) \"Code\" FROM \"@CVA_REGIONAIS_PN\"), (SELECT COALESCE(MAX(TO_INT(\"Code\")) + 1, 1) \"Code\" FROM \"@CVA_REGIONAIS_PN\"), '{regional}', '{sCardCode}');";
                    recordSet.DoQuery(sQuery);
                }
            }

            //Salva registros de Estados
            for (int i = 1; i <= Matrix1.VisualRowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)Matrix1.Columns.Item("check1").Cells.Item(i).Specific).Checked)
                {
                    var regional = ((SAPbouiCOM.EditText)Matrix1.Columns.Item("Code1").Cells.Item(i).Specific).Value;
                    sQuery = "INSERT INTO \"@CVA_REGIONAIS_PN\" (\"Code\", \"Name\", \"U_Regional\", \"U_CardCode\") " +
                       $"VALUES ((SELECT COALESCE(MAX(TO_INT(\"Code\")) + 1, 1) \"Code\" FROM \"@CVA_REGIONAIS_PN\"), (SELECT COALESCE(MAX(TO_INT(\"Code\")) + 1, 1) \"Code\" FROM \"@CVA_REGIONAIS_PN\"), '{regional}', '{sCardCode}');";
                    recordSet.DoQuery(sQuery);
                }
            }

            //Salva registros de Regiões
            for (int i = 1; i <= Matrix2.VisualRowCount; i++)
            {
                if (((SAPbouiCOM.CheckBox)Matrix2.Columns.Item("check2").Cells.Item(i).Specific).Checked)
                {
                    var regional = ((SAPbouiCOM.EditText)Matrix2.Columns.Item("Code2").Cells.Item(i).Specific).Value.ToString();
                    sQuery = "INSERT INTO \"@CVA_REGIONAIS_PN\" (\"Code\", \"Name\", \"U_Regional\", \"U_CardCode\") " +
                       $"VALUES ((SELECT COALESCE(MAX(TO_INT(\"Code\")) + 1, 1) \"Code\" FROM \"@CVA_REGIONAIS_PN\"), (SELECT COALESCE(MAX(TO_INT(\"Code\")) + 1, 1) \"Code\" FROM \"@CVA_REGIONAIS_PN\"), '{regional}', '{sCardCode}');";

                    recordSet.DoQuery(sQuery);
                }
            }

            this.UIAPIRawForm.Close();
        }
    }
}
