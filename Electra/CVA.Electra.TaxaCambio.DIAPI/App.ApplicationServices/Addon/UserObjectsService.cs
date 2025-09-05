using App.Repository.Interfaces;
using App.Repository.Repositories;
using App.Repository.Services;
using System;
using System.Collections.ObjectModel;
using System.Net;

namespace App.ApplicationServices.Services
{
    public class UserObjectsService
    {
        public enum BoUDOObjType
        {
            boud_MasterData,
            boud_Document,
        }

        public enum TableType
        {
            bott_MasterData,
            bott_MasterDataLines,
            bott_NoObject,
            bott_Document,
            bott_DocumentLines,
            bott_NoObjectAutoIncrement
        }
        public enum FieldType
        {
            db_Alpha,
            db_Date,
            db_Float,
            db_Memo,
            db_Numeric
        }

        public enum FieldSubType
        {
            st_Address,
            st_Image,
            st_Link,
            st_Measurement,
            st_None,
            st_Percentage,
            st_Phone,
            st_Price,
            st_Quantity,
            st_Rate,
            st_Sum,
            st_Time
        }

        private readonly IServiceLayerRepository<SAPB1.UserTablesMD> _repositoryTable;
        private readonly IServiceLayerRepository<SAPB1.UserFieldMD> _repositoryField;
        private readonly IServiceLayerRepository<SAPB1.UserObjectsMD> _repositoryUDO;

        public UserObjectsService()
        {
            _repositoryTable = new ServiceLayerRepositories<SAPB1.UserTablesMD>("UserTablesMD");
            _repositoryField = new ServiceLayerRepositories<SAPB1.UserFieldMD>("UserFieldsMD");
            _repositoryUDO = new ServiceLayerRepositories<SAPB1.UserObjectsMD>("UserObjectsMD");
        }

        public void TableAdd_DI(string NomeTB, string Desc, SAPbobsCOM.BoUTBTableType oTableType)
        {
            int lErrCode;
            string sErrMsg = "";


            SAPbobsCOM.UserTablesMD oUserTable;
            oUserTable = (SAPbobsCOM.UserTablesMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
            try
            {
                if (oUserTable.GetByKey(NomeTB)) return;

                oUserTable.TableName = NomeTB.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
                oUserTable.TableDescription = Desc;
                oUserTable.TableType = oTableType;
                try
                {
                    oUserTable.Add();
                    AddonService.diCompany.GetLastError(out lErrCode, out sErrMsg);
                    if (lErrCode != 0)
                    {
                        throw new Exception(sErrMsg);
                    }


                }
                catch (Exception)
                {
                    throw;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTable);
                oUserTable = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public void TableAdd(string tableName, string tableDescription, TableType oTableType)
        {
            try
            {
                /*
                dynamic TableExite = DataService.Get($@"UserTablesMD('{tableName}')");

                string Erro = TableExite["ErroMessage"];

                if (Erro != null)
                {
                    var jsonAdd = JsonConvert.SerializeObject(new
                    {
                        TableDescription = tableDescription,
                        TableName = tableName,
                        TableType = oTableType.ToString()
                    });
                    DataService.post(jsonAdd, "UserTablesMD");
                }
                */
                var filter = $@"('{tableName}')";
                try
                {
                    var ret = _repositoryTable.Get(filter);
                    return;
                }
                catch (WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        throw ex;
                    }
                }


                SAPB1.UserTablesMD userTables = new SAPB1.UserTablesMD
                {
                    TableDescription = tableDescription,
                    TableName = tableName,
                    TableType = oTableType.ToString()
                };
                _repositoryTable.Add(userTables, false);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void FieldAdd_DI(string NomeTabela, string NomeCampo, string DescCampo, SAPbobsCOM.BoFieldTypes Tipo, SAPbobsCOM.BoFldSubTypes SubTipo, Int16 Tamanho, string[,] valoresValidos, string valorDefault)
        {
            int lErrCode;
            string sErrMsg = "";

            try
            {
                string sQuery = $"SELECT 1 from CUFD t0 where t0.\"TableID\" = '{NomeTabela}' and t0.\"AliasID\"='{NomeCampo.Trim()}'";
                bool existe = Convert.ToString(ExecuteSqlScalar(sQuery)) == "1";
                if (existe) return;

                var TableName = NomeTabela.Replace("@", "").Replace("[", "").Replace("]", "").Trim();
                SAPbobsCOM.UserFieldsMD oUserField;
                oUserField = (SAPbobsCOM.UserFieldsMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                try
                {
                    oUserField.TableName = TableName;
                    oUserField.Name = NomeCampo;
                    oUserField.Description = DescCampo;
                    oUserField.Type = Tipo;
                    oUserField.SubType = SubTipo;
                    oUserField.DefaultValue = valorDefault;

                    //adicionar valores válidos
                    if (valoresValidos != null)
                    {
                        Int32 qtd = valoresValidos.GetLength(0);
                        if (qtd > 0)
                        {
                            for (int i = 0; i < qtd; i++)
                            {
                                oUserField.ValidValues.Value = valoresValidos[i, 0];
                                oUserField.ValidValues.Description = valoresValidos[i, 1];
                                oUserField.ValidValues.Add();
                            }
                        }
                    }

                    if (Tamanho != 0)
                        oUserField.EditSize = Tamanho;


                    oUserField.Add();
                    AddonService.diCompany.GetLastError(out lErrCode, out sErrMsg);
                    if (lErrCode != 0)
                    {
                        throw new Exception(sErrMsg);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserField);
                }
                oUserField = null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void FieldAdd(string TableName, string FieldName, string Description, FieldType oFieldType, FieldSubType oFieldSubType, Int32 Size, string[,] validValues, string valorDefault)
        {
            try
            {//
                //dynamic TableExite = DataService.Get($@"UserFieldsMD?$filter =TableName eq '{TableName}' and Name eq '{FieldName}'");

                //var validaCampo = "";
                //foreach (var item in TableExite.value)
                //{
                //    validaCampo = item["Name"];
                //}

                //if (validaCampo == "")
                //{

                //UserFields oUserField = new UserFields();
                //oUserField.Name = FieldName;
                //oUserField.Type = oFieldType.ToString();
                //oUserField.Size = Size;
                //oUserField.Description = Description;
                //oUserField.SubType = oFieldSubType.ToString();
                //oUserField.DefaultValue = valorDefault;
                //oUserField.TableName = TableName;
                //oUserField.EditSize = Size;


                //List<ValidValue> oValoresValidos = new List<ValidValue>();
                ////adicionar valores válidos
                //if (validValues != null)
                //{
                //    Int32 qtd = validValues.GetLength(0);
                //    if (qtd > 0)
                //    {
                //        for (int i = 0; i < qtd; i++)
                //        {
                //            ValidValue valores = new ValidValue();
                //            valores.Value = validValues[i, 0];
                //            valores.Description = validValues[i, 1];
                //            oValoresValidos.Add(valores);
                //        }

                //        oUserField.ValidValuesMD = oValoresValidos;
                //    }
                //}
                //}
                //var javaScriptSerializer = new JavaScriptSerializer();
                //var jsonSerializado = javaScriptSerializer.Serialize(oUserField);
                //DataService.post(jsonSerializado, "UserFieldsMD");

                var filter = $@"?$filter =TableName eq '{TableName}' and Name eq '{FieldName}'";
                var field = _repositoryField.Get(filter);
                if (field != null)
                {
                    return;
                }

                ObservableCollection<SAPB1.ValidValueMD> oValidValues = null;
                if (validValues != null)
                {
                    oValidValues = new ObservableCollection<SAPB1.ValidValueMD>();
                    Int32 qtd = validValues.GetLength(0);
                    if (qtd > 0)
                    {
                        for (int i = 0; i < qtd; i++)
                        {
                            SAPB1.ValidValueMD valores = new SAPB1.ValidValueMD
                            {
                                Value = validValues[i, 0],
                                Description = validValues[i, 1]
                            };
                            oValidValues.Add(valores);
                        }
                    }
                }
                SAPB1.UserFieldMD userFieldMD = new SAPB1.UserFieldMD
                {
                    Name = FieldName,
                    Type = oFieldType.ToString(),
                    Size = Size,
                    Description = Description,
                    SubType = oFieldSubType.ToString(),
                    DefaultValue = valorDefault,
                    TableName = TableName,
                    EditSize = Size,
                    ValidValuesMD = oValidValues

                };

                _repositoryField.Add(userFieldMD, false);



            }
            catch (Exception)
            {
                throw;
            }


        }

        public void UDOAdd_DI(string sUDO, string sTable, string sDescricaoUDO, SAPbobsCOM.BoUDOObjType oBoUDOObjType, string[] childTableName, string[] childObjectName, bool CanDelete = true)
        {
            int lRetCode = 0;
            int iTabelasFilhas = 0;
            string sErrMsg = "";
            bool bUpdate = false;
            bool bExisteTabelaFilha = false;

            SAPbobsCOM.UserObjectsMD oUserObjectMD = null;
            oUserObjectMD = (SAPbobsCOM.UserObjectsMD)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
            System.Data.DataTable tb = new System.Data.DataTable();

            try
            {
                if (oUserObjectMD.GetByKey(sUDO))
                {
                    return;
                }

                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanDelete = CanDelete?SAPbobsCOM.BoYesNoEnum.tYES: SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.Code = sUDO;
                oUserObjectMD.Name = sDescricaoUDO;
                oUserObjectMD.ObjectType = oBoUDOObjType;
                oUserObjectMD.TableName = sTable;
                /*
                sQuery = String.Format("SELECT  COLUNAS.NAME AS COLUNA " + Environment.NewLine +
                                       " FROM SYSOBJECTS AS TABELAS," + Environment.NewLine +
                                       "      SYSCOLUMNS AS COLUNAS" + Environment.NewLine +
                                       " WHERE " + Environment.NewLine +
                                       "     TABELAS.ID = COLUNAS.ID" + Environment.NewLine +
                                       "     AND TABELAS.NAME = '@{0}' and (left(COLUNAS.NAME,2)='U_' or COLUNAS.NAME IN ('DocEntry'))", sTable);

                tb = AddonService.ExecuteSqlDataTable(sQuery);

                int count = 0;
                foreach (System.Data.DataRow oRow in tb.Rows)
                {
                    bExisteColuna = false;
                    //verificar se existe coluna
                    for (int g = 0; g < oUserObjectMD.FindColumns.Count; g++)
                    {
                        oUserObjectMD.FindColumns.SetCurrentLine(g);
                        if (oUserObjectMD.FindColumns.ColumnAlias == oRow["COLUNA"].ToString())
                        {
                            bExisteColuna = true;
                            break;
                        }
                    }

                    if (bExisteColuna == true)
                    {
                        oUserObjectMD.FindColumns.ColumnDescription = oRow["COLUNA"].ToString();
                    }
                    else
                    {
                        if (count > 0) oUserObjectMD.FindColumns.Add();
                        oUserObjectMD.FindColumns.ColumnAlias = oRow["COLUNA"].ToString();
                        oUserObjectMD.FindColumns.ColumnDescription = oRow["COLUNA"].ToString();
                    }

                    count++;
                }
                */

                //Adicionar tabelas filhas
                if (childObjectName != null)
                {
                    for (int x = 0; x < childObjectName.Length; x++)
                    {

                        iTabelasFilhas = oUserObjectMD.ChildTables.Count;
                        bExisteTabelaFilha = false;
                        for (int y = 0; y < iTabelasFilhas; y++)
                        {
                            oUserObjectMD.ChildTables.SetCurrentLine(y);
                            if (oUserObjectMD.ChildTables.TableName == childTableName[x])
                            {
                                bExisteTabelaFilha = true;
                                break;
                            }
                        }

                        if (bExisteTabelaFilha == false)
                        {
                            if (x > 0) oUserObjectMD.ChildTables.Add();
                            if (childObjectName[x] != "" && childTableName[x] != "")
                            {
                                oUserObjectMD.ChildTables.TableName = childTableName[x];
                                oUserObjectMD.ChildTables.ObjectName = childObjectName[x];
                            }
                        }

                    }
                }

                if (bUpdate)
                    lRetCode = oUserObjectMD.Update();
                else
                    lRetCode = oUserObjectMD.Add();

                // check for errors in the process
                if (lRetCode != 0)
                {
                    AddonService.diCompany.GetLastError(out lRetCode, out sErrMsg);
                }

            }
            catch (Exception e)
            { System.Windows.Forms.MessageBox.Show(e.ToString()); }
            finally
            {
                tb.Dispose();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        public void UDOAdd(string sCode, string sTable, string sNameUDO, UserObjectsService.BoUDOObjType oBoUDOObjType, string[] childTableName, string[] childObjectName)
        {
            try
            {
                /*
                int x = 0;
                int totalArray = childTableName.GetLength(0) - 1;
                List<ChildTables> oChildList = new List<ChildTables>();

                dynamic TableExite = DataService.Get($@"UserObjectsMD('{sCode}')");

                string Erro = TableExite["ErroMessage"];

                if (Erro != null)
                {
                    foreach (var item in childTableName)
                    {
                        string nomeTabela = childTableName[x];
                        string nomeObjeto = childObjectName[x];

                        x++;

                        oChildList.Add(new ChildTables
                        {
                            ObjectName = nomeObjeto,
                            SonNumber = x.ToString(),
                            TableName = nomeTabela,
                        });
                    }

                    UserObjectsMD oUserObjectsMD = new UserObjectsMD
                    {
                        Code = sCode,
                        Name = sNameUDO,
                        TableName = sTable,
                        ObjectType = oBoUDOObjType.ToString(),
                        CanCreateDefaultForm = "tNO",
                        ExtensionName = "",
                        CanCancel = "tNO",
                        CanDelete = "tYES",
                        CanLog = "tYES",
                        ManageSeries = "tYES",
                        CanFind = "tYES",
                        CanYearTransfer = "tNO",
                        CanClose = "tNO",
                        OverwriteDllfile = "tYES",
                        UseUniqueFormType = "tYES",
                        CanArchive = "tNO",
                        MenuItem = "tNO",
                        MenuCaption = "",
                        FatherMenuID = "",
                        Position = "",
                        UserObjectMD_ChildTables = oChildList
                    };

                    var javaScriptSerializer = new JavaScriptSerializer();
                    var jsonSerializado = javaScriptSerializer.Serialize(oUserObjectsMD);
                    DataService.post(jsonSerializado, "UserObjectsMD");
                }
                */

                var filter = $@"('{sCode}')";
                try
                {
                    _repositoryUDO.Get(filter);
                    return;
                }
                catch (WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response.StatusCode != HttpStatusCode.NotFound)
                    {
                        throw ex;
                    }
                }

                ObservableCollection<SAPB1.UserObjectMD_ChildTable> ChildTables = null;
                if (childTableName != null) //if (childTableName.Length > 0)                
                {
                    ChildTables = new ObservableCollection<SAPB1.UserObjectMD_ChildTable>();
                    int x = 0;
                    foreach (var item in childTableName)
                    {
                        string nomeTabela = childTableName[x];
                        string nomeObjeto = childObjectName[x];

                        x++;

                        ChildTables.Add(new SAPB1.UserObjectMD_ChildTable
                        {
                            ObjectName = nomeObjeto,
                            SonNumber = x,
                            TableName = nomeTabela,
                        });
                    }
                }
                SAPB1.UserObjectsMD objectsMD = new SAPB1.UserObjectsMD
                {
                    Code = sCode,
                    Name = sNameUDO,
                    TableName = sTable,
                    ObjectType = oBoUDOObjType.ToString(),
                    CanCreateDefaultForm = "tNO",
                    ExtensionName = "",
                    CanCancel = "tNO",
                    CanDelete = "tYES",
                    CanLog = "tYES",
                    ManageSeries = "tNO",
                    CanFind = "tYES",
                    CanYearTransfer = "tNO",
                    CanClose = "tNO",
                    OverwriteDllfile = "tYES",
                    UseUniqueFormType = "tYES",
                    CanArchive = "tNO",
                    MenuItem = "tNO",
                    MenuCaption = "",
                    FatherMenuID = null,
                    Position = null,
                    UserObjectMD_ChildTables = ChildTables
                };

                _repositoryUDO.Add(objectsMD, false);

            }
            catch (Exception)
            {
                throw;
            }
        }


        public dynamic ExecuteSqlScalar(string query)
        {
            try
            {
                dynamic obj = null;
                var oRs = (SAPbobsCOM.Recordset)AddonService.diCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRs.DoQuery(query);
                if (!oRs.EoF)
                {
                    obj = oRs.Fields.Item(0).Value;
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRs);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
