using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace CVA.Apetit.Addon.Class
{
    class FilialService
    {
        internal static void PreencherCombo(string comboUID, SAPbouiCOM.IForm form, bool opcaoTodas = false)
        {
            string sql = "";

            try
            {
                var combo = (ComboBox)form.Items.Item(comboUID).Specific;

                if (comboUID == "cbFilial")
                {
                    sql = @"SELECT ""BPLId"" AS ""Chave"", ""BPLName"" AS ""Descricao"" FROM OBPL ORDER BY ""BPLId"" ";
                }
                else if (comboUID == "cbReco")
                {
                    sql = @"
SELECT 
    T0.""MsnCode"" AS ""Chave""
    ,T0.""Descr"" AS ""Descricao""
FROM OMSN T0
    INNER JOIN OFCT T1 ON T1.""AbsID"" = T0.""FCTAbs""
WHERE T0.""PurchReq"" = 'Y' 
";
                }

                if (!string.IsNullOrEmpty(sql))
                {
                    //for (int i = combo.ValidValues.Count; i >= 1; i--)
                    //{
                    //    combo.ValidValues.Remove(0);
                    //}

            //        If aCombo.ValidValues.Count > 0 Then
            //            For i As Integer = 1 To aCombo.ValidValues.Count
            //        aCombo.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index)
            //    Next
            //End If


                    var dt = Conexao.ExecuteSqlDataTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        combo.ValidValues.Add(dt.Rows[i]["Chave"].ToString(), dt.Rows[i]["Descricao"].ToString());
                    }

                    if (opcaoTodas)
                    {
                        combo.ValidValues.Add("0", "Todas");
                        combo.Select("0");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void PreencherComboMatriz(string comboUID, SAPbouiCOM.IForm form, string MatrizID, bool opcaoTodas = false)
        {
            string sql = "";

            try
            {
                if (comboUID == "cItemGroup")
                {
                    sql = string.Format(@"
SELECT ""ItmsGrpCod"" AS ""Chave"", ""ItmsGrpNam"" AS ""Descricao"" 
FROM OITB 
ORDER BY ""ItmsGrpCod"" 
");
                }

                if (comboUID == "cFamilia")
                {
                    sql = string.Format(@"
SELECT ""Code"" AS ""Chave"", ""Name"" AS ""Descricao"" 
FROM ""@CVA_FAMILIA"" 
ORDER BY ""Code"" 
");
                }

                if (comboUID == "cSFamilia")
                {
                    sql = string.Format(@"
SELECT ""Code"" AS ""Chave"", ""Name"" AS ""Descricao"" 
FROM ""@CVA_SUBFAMILA"" 
ORDER BY ""Code"" 
");
                }

                if (comboUID == "cDepPad")
                {
                    sql = string.Format(@"SELECT ""WhsCode"" AS ""Chave"", ""WhsName"" AS ""Descricao"" FROM OWHS ORDER BY ""WhsCode"" ");
                }
                if (comboUID == "cCD")
                {
                    sql = string.Format(@"SELECT ""BPLId"" AS ""Chave"", ""BPLName"" AS ""Descricao"" FROM OBPL ORDER BY ""BPLId""  ");
                }
                if ((comboUID == "uUsgTran") || (comboUID == "uUsgRem"))
                {
                    sql = string.Format(@"SELECT ""ID"" AS ""Chave"", ""Usage"" AS ""Descricao"" FROM OUSG WHERE UPPER(""Usage"") LIKE 'S%' ORDER BY ""ID""  ");
                }
                if (comboUID == "uUsgRet")
                {
                    sql = string.Format(@"SELECT ""ID"" AS ""Chave"", ""Usage"" AS ""Descricao"" FROM OUSG WHERE UPPER(""Usage"") LIKE 'E%' ORDER BY ""ID""  ");
                }
                if (comboUID == "cPrice")
                {
                    sql = string.Format(@"
SELECT '-2' AS ""Chave"", 'Última Entrada' AS ""Descricao"" FROM DUMMY
UNION ALL
SELECT '-1' AS ""Chave"", 'Preço Médio' AS ""Descricao"" FROM DUMMY
UNION ALL
SELECT ""ListNum"" AS ""Chave"", ""ListName"" AS ""Descricao"" FROM OPLN ORDER BY 1 
");
                }

                if (!string.IsNullOrEmpty(sql))
                {
                    var dt = Conexao.ExecuteSqlDataTable(sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string whsCode = dt.Rows[i]["Chave"].ToString();
                        string whsName = dt.Rows[i]["Descricao"].ToString();
                        ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Add(whsCode, whsName);
                    }

                    if (opcaoTodas)
                    {
                        ((SAPbouiCOM.Matrix)(form.Items.Item(MatrizID).Specific)).Columns.Item(comboUID).ValidValues.Add("0", "Todas");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
