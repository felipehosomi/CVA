
using SAPbouiCOM;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Repository.Services;

namespace App.ApplicationServices
{
    public static class UserDataTablesExtension
    {
        public static void AddUserTable(this UserTablesMD vTable, string Nome, string Descricao, BoUTBTableType TableType)
        {
            try
            {
                if (vTable == null)
                {
                    vTable = (SAPbobsCOM.UserTablesMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
                }

                var vNome = Nome.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
                var vDesc = Descricao;

                if (vDesc.Length > 30)
                {
                    vDesc = vDesc.Substring(0, 30);
                }


                if (!vTable.GetByKey(vNome))
                {
                    vTable.TableName = vNome;
                    vTable.TableDescription = vDesc;
                    vTable.TableType = TableType;

                    if (vTable.Add() != 0)
                    {
                        throw new Exception(AddonService.diCompany.GetLastErrorDescription());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        //public static void Add

        //public void AddUserTable(string NomeTB, string Desc, SAPbobsCOM.BoUTBTableType oTableType)
        //{
        //    int lErrCode;
        //    string sErrMsg = "";


        //    SAPbobsCOM.UserTablesMD oUserTable = (SAPbobsCOM.UserTablesMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTable);
        //    oUserTable = null;

        //    oUserTable = (SAPbobsCOM.UserTablesMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

        //    try
        //    {
        //        if (!oUserTable.GetByKey(NomeTB))
        //        {
        //            oUserTable.TableName = NomeTB.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
        //            oUserTable.TableDescription = Desc;
        //            oUserTable.TableType = oTableType;
        //            try
        //            {
        //                if (oUserTable.Add() != 0)
        //                {
        //                    AddonService.diCompany.GetLastError(out lErrCode, out sErrMsg);

        //                    throw new Exception(sErrMsg);
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTable);
        //        oUserTable = null;

        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //    }
        //}

        //public void AddUserField(string NomeTabela, string NomeCampo, string DescCampo, SAPbobsCOM.BoFieldTypes Tipo, SAPbobsCOM.BoFldSubTypes SubTipo, Int16 Tamanho, string[,] valoresValidos, string valorDefault, string linkedTable)
        //{
        //    int lErrCode;
        //    string sErrMsg = "";

        //    try
        //    {
        //        string sSquery = string.Format("SELECT [name] FROM syscolumns WHERE [name] = 'U_{0}' AND id = (SELECT id FROM sysobjects WHERE type = 'U' AND ([NAME] = '{1}' OR [NAME] = '@{1}'))", NomeCampo, NomeTabela.Replace("[", "").Replace("]", ""));
        //        object oResult = AddonService.ExecuteSqlScalar(sSquery);
        //        if (oResult != null) return;

        //        SAPbobsCOM.UserFieldsMD oUserField;
        //        oUserField = (SAPbobsCOM.UserFieldsMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
        //        oUserField.TableName = NomeTabela.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
        //        oUserField.Name = NomeCampo;
        //        oUserField.Description = DescCampo;
        //        oUserField.Type = Tipo;
        //        oUserField.SubType = SubTipo;
        //        oUserField.DefaultValue = valorDefault;
        //        if (!string.IsNullOrEmpty(linkedTable)) oUserField.LinkedTable = linkedTable;

        //        //adicionar valores válidos
        //        if (valoresValidos != null)
        //        {
        //            Int32 qtd = valoresValidos.GetLength(0);
        //            if (qtd > 0)
        //            {
        //                for (int i = 0; i < qtd; i++)
        //                {
        //                    oUserField.ValidValues.Value = valoresValidos[i, 0];
        //                    oUserField.ValidValues.Description = valoresValidos[i, 1];
        //                    oUserField.ValidValues.Add();
        //                }
        //            }
        //        }

        //        if (Tamanho != 0)
        //            oUserField.EditSize = Tamanho;

        //        try
        //        {
        //            oUserField.Add();
        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();
        //            GC.Collect();
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserField);
        //            oUserField = null;
        //            AddonService.diCompany.GetLastError(out lErrCode, out sErrMsg);
        //            if (lErrCode != 0)
        //            {
        //                throw new Exception(sErrMsg);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        oUserField = null;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
