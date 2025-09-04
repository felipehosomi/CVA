using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.Enums;
using CVA.AddOn.Common.Util;
using CVA.Core.ObrigacoesFiscais.DAO.Resources;
using CVA.Core.ObrigacoesFiscais.MODEL;
using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CVA.Core.ObrigacoesFiscais.BLL
{
    public class FileBLL
    {
        public static string GetSQL(string objectType, string objectName)
        {
            FileFilterModel filterModel = new FileFilterModel();
            filterModel.DateFrom = DateTime.Today;
            filterModel.DateTo = DateTime.Today;
            filterModel.BranchId = 1;

            return GetSQL(filterModel, objectType, objectName);
        }

        public static string GetSQL(FileFilterModel filterModel, string objectType, string objectName)
        {
            string sql = $@"DECLARE @DateFrom DATETIME = CAST('{filterModel.DateFrom.ToString("yyyyMMdd")}' AS datetime)
                            DECLARE @DateTo DATETIME = CAST('{filterModel.DateTo.ToString("yyyyMMdd")}' AS datetime) ";
            
            sql += Environment.NewLine;
            if (SBOApp.Company.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                switch (objectType.Trim())
                {

                    case "SP":
                        sql = $@" CALL ""{objectName.ToUpper()}"" ({filterModel.BranchId}, '{filterModel.DateFrom.ToString("yyyyMMdd")}', '{filterModel.DateTo.ToString("yyyyMMdd")}')";
                        break;
                    //case "FN":
                    //    sql += $" SELECT * FROM {objectName} ({filterModel.BranchId}, @DateFrom, @DateTo)";
                    //    break;
                        //case "VI":
                        //    sql = $"SELECT * FROM {objectName} WHERE  ({filterModel.BranchId}, @DateFrom, @DateTo)";
                }

            }
            else
            {
                switch (objectType.Trim())
                {

                    case "SP":
                        sql += $" EXEC {objectName} {filterModel.BranchId}, @DateFrom, @DateTo";
                        break;
                    case "FN":
                        sql += $" SELECT * FROM {objectName} ({filterModel.BranchId}, @DateFrom, @DateTo)";
                        break;
                        //case "VI":
                        //    sql = $"SELECT * FROM {objectName} WHERE  ({filterModel.BranchId}, @DateFrom, @DateTo)";
                }

            }
            


            return sql;
        }

        public string GenerateFile(FileFilterModel filterModel)
        {
            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            StreamWriter sw = new StreamWriter(Path.Combine(filterModel.Directory, $"{filterModel.LayoutDesc.Trim()}_{DateTime.Now.ToString("ddMMyyyy_HHmm")}.txt"));
            Dictionary<string, System.Data.DataTable> tablesList = new Dictionary<string, System.Data.DataTable>();

            try
            {
                LayoutModel layoutModel = new CrudController("@CVA_LAYOUT").RetrieveModel<LayoutModel>($"Code = '{filterModel.Layout}'");

                if (String.IsNullOrEmpty(layoutModel.PaddingCharString))
                {
                    layoutModel.PaddingCharString = " ";
                }
                if (String.IsNullOrEmpty(layoutModel.PaddingCharNumeric))
                {
                    layoutModel.PaddingCharNumeric = " ";
                }

                char paddingCharString = Convert.ToChar(layoutModel.PaddingCharString);
                char paddingCharNumeric = Convert.ToChar(layoutModel.PaddingCharNumeric);

                List<FileMappingModel> mappingList = new CrudController("@CVA_FILE_MAP").RetrieveModelList<FileMappingModel>($"U_Layout = '{filterModel.Layout}'", "U_Position");

                foreach (var model in mappingList)
                {
                    model.FieldsList = new CrudController("@CVA_FILE_MAP_ITEM").RetrieveModelList<FileMappingItemModel>($"Code = '{model.Code}'");
                    model.ParentLinkList = new CrudController("@CVA_FILE_MAP_LINK").RetrieveModelList<FileMappingLinkModel>($"Code = '{model.Code}' AND ISNULL(U_Parent, '') <> '' AND ISNULL(U_Child, '') <> '' ");
                    model.ChildLinkList = new CrudController("@").FillModelList<FileMappingLinkModel>(String.Format(Query.FileMapping_GetChildLink, model.Code));

                    System.Data.DataTable dataTable = new System.Data.DataTable(model.Description);

                    foreach (var linkModel in model.ParentLinkList)
                    {
                        dataTable.Columns.Add(linkModel.ParentProperty + "_PK", typeof(string));
                    }
                    foreach (var linkModel in model.ChildLinkList)
                    {
                        dataTable.Columns.Add(linkModel.ChildProperty + "_PK", typeof(string));
                    }

                    foreach (var fieldModel in model.FieldsList)
                    {
                        dataTable.Columns.Add(fieldModel.FieldName, typeof(string));
                    }

                    rst.DoQuery(GetSQL(filterModel, model.ObjectType, model.ObjectName));

                    while (!rst.EoF)
                    {
                        System.Data.DataRow dataRow = dataTable.NewRow();

                        foreach (var linkModel in model.ParentLinkList)
                        {
                            dataRow[linkModel.ParentProperty + "_PK"] = rst.Fields.Item(linkModel.ParentProperty).Value;
                        }
                        foreach (var linkModel in model.ChildLinkList)
                        {
                            dataRow[linkModel.ChildProperty + "_PK"] = rst.Fields.Item(linkModel.ChildProperty).Value;
                        }

                        foreach (var fieldModel in model.FieldsList)
                        {
                            try
                            {
                                object value = rst.Fields.Item(fieldModel.FieldName).Value;
                                switch ((FieldTypeEnum)fieldModel.FieldType)
                                {
                                    case FieldTypeEnum.AlphaNumeric:
                                        if (fieldModel.OnlyNumbers == "Y")
                                        {
                                            Regex digitsOnly = new Regex(@"[^\d]");
                                            value = digitsOnly.Replace(value.ToString(), "");
                                        }
                                        if (layoutModel.PaddingDirectionString == (int)PaddingTypeEnum.Right)
                                        {
                                            value = value.ToString().PadRight(fieldModel.Size, paddingCharString);
                                        }
                                        else if (layoutModel.PaddingDirectionString == (int)PaddingTypeEnum.Left)
                                        {
                                            value = value.ToString().PadLeft(fieldModel.Size, paddingCharString);
                                        }
                                        break;
                                    case FieldTypeEnum.Date:
                                        string format = fieldModel.Format;
                                        format = format.Replace("D", "d").Replace("m", "M").Replace("Y", "y").Replace("A", "y").Replace("a", "y");
                                        value = ((DateTime)value).ToString(format).PadRight(fieldModel.Size, ' ');
                                        break;
                                    case FieldTypeEnum.Integer:
                                        if (layoutModel.PaddingDirectionNumeric == (int)PaddingTypeEnum.Right)
                                        {
                                            value = value.ToString().PadRight(fieldModel.Size, paddingCharNumeric);
                                        }
                                        else if (layoutModel.PaddingDirectionNumeric == (int)PaddingTypeEnum.Left)
                                        {
                                            value = value.ToString().PadLeft(fieldModel.Size, paddingCharNumeric);
                                        }
                                        break;
                                    case FieldTypeEnum.Decimal:
                                        double valueDouble;
                                        if (double.TryParse(value.ToString(), out valueDouble))
                                        {
                                            value = valueDouble.ToString($"f{fieldModel.DecimalPlaces}").Replace(",", layoutModel.DecimalSeparator).Replace(".", layoutModel.DecimalSeparator);
                                        }
                                        else
                                        {
                                            value = value.ToString().Replace(",", layoutModel.DecimalSeparator).Replace(".", layoutModel.DecimalSeparator);
                                        }
                                        if (layoutModel.PaddingDirectionNumeric == (int)PaddingTypeEnum.Right)
                                        {
                                            value = value.ToString().PadRight(fieldModel.Size, paddingCharNumeric);
                                        }
                                        else if (layoutModel.PaddingDirectionNumeric == (int)PaddingTypeEnum.Left)
                                        {
                                            value = value.ToString().PadLeft(fieldModel.Size, paddingCharNumeric);
                                        }
                                        break;
                                    default:
                                        break;
                                }

                                dataRow[fieldModel.FieldName] = value;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Erro ao setar campo {fieldModel.FieldName}: {ex.Message}");
                            }
                        }
                        dataTable.Rows.Add(dataRow);
                        rst.MoveNext();
                    }
                    tablesList.Add(model.Code, dataTable);
                }

                List<FileMappingModel> childList = mappingList.Where(m => m.Code.In(mappingList.Select(l => l.Child).ToArray())).ToList();

                mappingList = mappingList.Where(m => !m.Code.In(mappingList.Select(l => l.Child).ToArray())).ToList();

                foreach (var model in mappingList)
                {
                    System.Data.DataTable table = tablesList[model.Code];
                    foreach (System.Data.DataRow row in table.Rows)
                    {
                        string line = String.Empty;
                        if (layoutModel.StartsWithSeparator == "Y")
                        {
                            line = layoutModel.Separator;
                        }
                        if (!String.IsNullOrEmpty(model.Identifier))
                        {
                            line += model.Identifier + layoutModel.Separator;
                        }
                        
                        for (int i = 0 + model.ChildLinkList.Count + model.ParentLinkList.Count; i < table.Columns.Count; i++)
                        {
                            line += row[i];
                            line += layoutModel.Separator;
                        }
                        sw.WriteLine(line);

                        if (!String.IsNullOrEmpty(model.Child))
                        {
                            System.Data.DataTable childTable = tablesList[model.Child];

                            string select = String.Empty;
                            foreach (var link in model.ParentLinkList)
                            {
                                select += $" AND {link.ChildProperty}_PK = '{row[link.ParentProperty + "_PK"]}'";
                            }
                            select = select.Substring(4);

                            System.Data.DataRow[] foundRows = childTable.Select(select);
                            foreach (var childRow in foundRows)
                            {
                                FileMappingModel childModel = childList.FirstOrDefault(m => m.Code == model.Child);
                                line = String.Empty;
                                if (layoutModel.StartsWithSeparator == "Y")
                                {
                                    line = layoutModel.Separator;
                                }
                                if (!String.IsNullOrEmpty(childModel.Identifier))
                                {
                                    line += childModel.Identifier + layoutModel.Separator;
                                }
                                
                                for (int i = 0; i < childTable.Columns.Count; i++)
                                {
                                    if (!childTable.Columns[i].ColumnName.EndsWith("_PK"))
                                    {
                                        line += childRow[i];
                                        line += layoutModel.Separator;
                                    }
                                }
                                sw.WriteLine(line);
                            }
                        }
                    }
                }

                if (filterModel.ExcelLayout)
                {
                    ExcelBLL.GenerateExcel(filterModel.Directory, layoutModel.Name, tablesList);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                sw.Close();
            }

            return String.Empty;
        }
    }
}
