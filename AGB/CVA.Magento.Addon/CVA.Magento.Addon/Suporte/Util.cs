using SAPbobsCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Magento.Addon
{
    public class Util
    {
        private static SAPbouiCOM.Application sboApp = Application.SBO_Application;
        private static Company oCompany = (Company)sboApp.Company.GetDICompany();

        #region [ Get Next Code Hana ]

        public static string GetNextCodeHana(string sBase, string sTabela)
        {
            Recordset oRS = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sCode = String.Empty;

            try
            {
                string sQuery = String.Format(@"SELECT IFNULL(MAX(CAST(T0.""Code"" AS int))+1,1) AS ""Code""
                                                FROM ""{0}"".""@{1}"" T0", sBase, sTabela);
                oRS.DoQuery(sQuery);

                if (oRS.RecordCount > 0)
                {
                    sCode = oRS.Fields.Item(0).Value.ToString();
                    sCode = sCode.PadLeft(10, '0');
                }

                return sCode;
            }
            catch
            {
                return sCode;
            }
        }

        #endregion

        #region [ PreencherCombo ]

        public static void PreencherCombo(SAPbouiCOM.ComboBox oCombo, string sBase, string sTabela, string sFilial)
        {
            try
            {
                Recordset oRS = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sQuery = String.Empty;

                switch (sTabela)
                {
                    case "OUSG": { sQuery = String.Format(@"SELECT ""ID"", ""Usage""
                                                            FROM ""{0}"".""OUSG""
                                                            WHERE ""Locked"" = 'N'", sBase); break; }
                    case "OEXD": { sQuery = String.Format(@"SELECT ""ExpnsCode"", ""ExpnsName""
                                                            FROM ""{0}"".""OEXD""
                                                            WHERE ""ExpnsType"" = 1", sBase); break; }                    
                    case "OBPL": { sQuery = String.Format(@"SELECT ""BPLId"", ""BPLName""
                                                            FROM ""{0}"".""OBPL""
                                                            WHERE ""Disabled"" = 'N'", sBase); break; }
                    case "OWHS": { sQuery = String.Format(@"SELECT ""WhsCode"", ""WhsName""
                                                            FROM ""{0}"".""OWHS""
                                                            WHERE ""BPLid"" = '{1}'", sBase, sFilial); break; }
                    case "OACT": { sQuery = String.Format(@"SELECT ""AcctCode"", ""AcctName""
                                                            FROM ""{0}"".""OACT""
                                                            WHERE ""Levels"" = '5'
                                                              AND ""GroupMask"" = '1'
                                                              AND ""Finanse"" = 'Y'", sBase); break; }
                    case "OCTG": { sQuery = String.Format(@"SELECT ""GroupNum"", ""PymntGroup""
                                                            FROM ""{0}"".""OCTG""
                                                            ORDER BY ""GroupNum""", sBase); break; }
                    case "OPYM": { sQuery = String.Format(@"SELECT ""PayMethCod"", ""Descript""
                                                            FROM ""{0}"".""OPYM""", sBase); break; }
                    case "OSHP": { sQuery = String.Format(@"SELECT ""TrnspCode"", ""TrnspName""
                                                            FROM ""{0}"".""OSHP""
                                                            ORDER BY ""TrnspCode""", sBase); break; }
                }

                oRS.DoQuery(sQuery);
                if (oRS.RecordCount > 0)
                {
                    while (oCombo.ValidValues.Count > 0)
                        oCombo.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);

                    while (!oRS.EoF)
                    {
                        oCombo.ValidValues.Add(oRS.Fields.Item(0).Value.ToString(), oRS.Fields.Item(1).Value.ToString());
                        oRS.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ PreencherFiliais ]

        public static void PreencherFiliais(string sBase, ref List<Filial> listFiliais)
        {
            try
            {
                int iIndex = 0;
                Recordset oRS = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sQuery = sQuery = String.Format(@"SELECT ""BPLId""
                                                         FROM ""{0}"".""OBPL""
                                                         WHERE ""Disabled"" = 'N';", sBase);
                oRS.DoQuery(sQuery);
                if (oRS.RecordCount > 0)
                {
                    listFiliais = new List<Filial>();
                    
                    while (!oRS.EoF)
                    {
                        Filial objFilial = new Filial();
                        objFilial.Index = iIndex;
                        objFilial.BPLId = Convert.ToInt32(oRS.Fields.Item(0).Value);
                        listFiliais.Add(objFilial);
                        oRS.MoveNext();
                        iIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region [ PreencherMatrix ]

        public static void PreencherMatrix(SAPbouiCOM.Form oForm, string sBase)
        {
            oForm.Freeze(true);

            try
            {

                #region [ Depósitos ]

                List<Filial> listFiliais = new List<Filial>();
                Util.PreencherFiliais(oCompany.CompanyDB, ref listFiliais);

                SAPbouiCOM.DBDataSource oDB = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG1");
                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxDep").Specific;
                
                if (oMatrix.RowCount == 0)
                    oDB.SetValue("U_FilialMagento", 0, " ");
                
                oMatrix.LoadFromDataSource();
                oMatrix.FlushToDataSource();

                for (int i = 0; i < oDB.Size; i++)
                {
                    SAPbouiCOM.ComboBox objComboFilial = ((SAPbouiCOM.ComboBox)oMatrix.Columns.Item("Col_0").Cells.Item(i + 1).Specific);
                    SAPbouiCOM.ComboBox objComboDeposito = ((SAPbouiCOM.ComboBox)oMatrix.Columns.Item("Col_2").Cells.Item(i + 1).Specific);

                    PreencherCombo(objComboFilial, sBase, "OBPL", String.Empty);

                    if (!String.IsNullOrEmpty(objComboFilial.Value))
                    {
                        PreencherCombo(objComboDeposito, sBase, "OWHS", objComboFilial.Value); //listFiliais[i].BPLId.ToString()
                        oDB.SetValue("U_FilialSap", i, objComboFilial.Value); //listFiliais[i].BPLId.ToString()
                    }
                }

                oMatrix.LoadFromDataSource();

                #endregion

                #region [ Condições ]

                SAPbouiCOM.DBDataSource oDBCond = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG2");
                SAPbouiCOM.Matrix oMatrix1 = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxCond").Specific;

                if (oMatrix1.RowCount == 0)
                    oDBCond.SetValue("U_CondMagento", 0, " ");

                oMatrix1.LoadFromDataSource();
                oMatrix1.FlushToDataSource();

                for (int i = 0; i < oDBCond.Size; i++)
                {
                    SAPbouiCOM.ComboBox objComboCondicao = ((SAPbouiCOM.ComboBox)oMatrix1.Columns.Item("Col_0").Cells.Item(i + 1).Specific);
                    PreencherCombo(objComboCondicao, sBase, "OCTG", String.Empty);
                }

                oMatrix1.LoadFromDataSource();

                #endregion

                #region [ Formas ]

                SAPbouiCOM.DBDataSource oDBFormas = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG3");
                SAPbouiCOM.Matrix oMatrix2 = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxFormas").Specific;

                if (oMatrix2.RowCount == 0)
                    oDBFormas.SetValue("U_FormaMagento", 0, " ");

                oMatrix2.LoadFromDataSource();
                oMatrix2.FlushToDataSource();

                for (int i = 0; i < oDBFormas.Size; i++)
                {
                    SAPbouiCOM.ComboBox objComboFormas = ((SAPbouiCOM.ComboBox)oMatrix2.Columns.Item("Col_0").Cells.Item(i + 1).Specific);
                    SAPbouiCOM.ComboBox objComboConta = ((SAPbouiCOM.ComboBox)oMatrix2.Columns.Item("Col_1").Cells.Item(i + 1).Specific);

                    PreencherCombo(objComboFormas, sBase, "OPYM", String.Empty);
                    PreencherCombo(objComboConta, sBase, "OACT", String.Empty);
                }

                oMatrix2.LoadFromDataSource();

                #endregion

                #region [ Datas ]

                SAPbouiCOM.DBDataSource oDBDatas = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG4");
                SAPbouiCOM.Matrix oMatrix3 = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxDatas").Specific;

                if (oMatrix3.RowCount == 0)
                    oDBDatas.SetValue("U_DataDe", 0, " ");

                oMatrix3.LoadFromDataSource();

                #endregion

                #region [ Frete ]

                SAPbouiCOM.DBDataSource oDBFrete = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG5");
                SAPbouiCOM.Matrix mtxFrete = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxFrete").Specific;

                if (mtxFrete.RowCount == 0)
                    oDBFrete.SetValue("U_FreteMagento", 0, " ");

                mtxFrete.LoadFromDataSource();
                mtxFrete.FlushToDataSource();

                for (int i = 0; i < oDBFrete.Size; i++)
                {
                    SAPbouiCOM.ComboBox objComboFrete = ((SAPbouiCOM.ComboBox)mtxFrete.Columns.Item("Col_0").Cells.Item(i + 1).Specific);
                    PreencherCombo(objComboFrete, sBase, "OSHP", String.Empty);
                }

                mtxFrete.LoadFromDataSource();

                #endregion

                oForm.Freeze(false);
            }
            catch (Exception ex)
            {
                oForm.Freeze(false);
                throw ex;
            }
        }

        #endregion

        #region [ Validar informações da Matrix ]

        public static void ValidarMatrix(SAPbouiCOM.Form oForm)
        {
            try
            {

                #region [ Depósitos ]

                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxDep").Specific;
                SAPbouiCOM.DBDataSource oDB = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG1");

                if (oMatrix.RowCount >= 1)
                {
                    oMatrix.FlushToDataSource();

                    for (int i = oDB.Size - 1; i >= 0; i--)
                    {
                        if (oDB.Size >= 1)
                        {
                            if (string.IsNullOrEmpty(oDB.GetValue("U_FilialMagento", i).ToString()))
                                oDB.RemoveRecord(i);
                        }
                    }

                    oMatrix.LoadFromDataSource();
                }

                #endregion

                #region [ Condições ]

                SAPbouiCOM.Matrix oMatrix2 = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxCond").Specific;
                SAPbouiCOM.DBDataSource oDBCond = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG2");

                if (oMatrix2.RowCount >= 1)
                {
                    oMatrix2.FlushToDataSource();

                    for (int i = oDBCond.Size - 1; i >= 0; i--)
                    {
                        if (oDBCond.Size >= 1)
                        {
                            if (string.IsNullOrEmpty(oDBCond.GetValue("U_CondMagento", i).ToString()))
                                oDBCond.RemoveRecord(i);
                        }
                    }

                    oMatrix2.LoadFromDataSource();
                }

                #endregion

                #region [ Formas ]

                SAPbouiCOM.Matrix oMatrix3 = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxFormas").Specific;
                SAPbouiCOM.DBDataSource oDBFormas = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG3");

                if (oMatrix3.RowCount >= 1)
                {
                    oMatrix3.FlushToDataSource();

                    for (int i = oDBFormas.Size - 1; i >= 0; i--)
                    {
                        if (oDBFormas.Size >= 1)
                        {
                            if (string.IsNullOrEmpty(oDBFormas.GetValue("U_FormaMagento", i).ToString()))
                                oDBFormas.RemoveRecord(i);
                        }
                    }

                    oMatrix3.LoadFromDataSource();
                }

                #endregion

                #region [ Datas ]

                SAPbouiCOM.Matrix oMatrix4 = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxDatas").Specific;
                SAPbouiCOM.DBDataSource oDBDatas = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG4");

                if (oMatrix4.RowCount >= 1)
                {
                    oMatrix4.FlushToDataSource();

                    for (int i = oDBDatas.Size - 1; i >= 0; i--)
                    {
                        if (oDBDatas.Size >= 1)
                        {
                            if (string.IsNullOrEmpty(oDBDatas.GetValue("U_DataDe", i).ToString()))
                                oDBDatas.RemoveRecord(i);
                        }
                    }

                    oMatrix4.LoadFromDataSource();
                }

                #endregion

                #region [ Frete ]

                SAPbouiCOM.Matrix oMatrix5 = (SAPbouiCOM.Matrix)oForm.Items.Item("mtxFrete").Specific;
                SAPbouiCOM.DBDataSource oDBFrete = oForm.DataSources.DBDataSources.Item("@CVA_CONFIG_MAG5");

                if (oMatrix5.RowCount >= 1)
                {
                    oMatrix5.FlushToDataSource();

                    for (int i = oDBFrete.Size - 1; i >= 0; i--)
                    {
                        if (oDBFrete.Size >= 1)
                        {
                            if (string.IsNullOrEmpty(oDBFrete.GetValue("U_FreteSap", i).ToString()))
                                oDBFrete.RemoveRecord(i);
                        }
                    }

                    oMatrix5.LoadFromDataSource();
                }

                #endregion

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

    }
}
