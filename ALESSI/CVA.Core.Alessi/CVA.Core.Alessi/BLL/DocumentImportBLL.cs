using CVA.AddOn.Common;
using CVA.Core.Alessi.DAO.UserTables;
using CVA.Core.Alessi.MODEL;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.Alessi.BLL
{
    public class DocumentImportBLL
    {
        private List<ImportLogModel> LogList;
        private bool HasError;

        public DocumentImportBLL()
        {
            LogList = new List<ImportLogModel>();
            HasError = false;
        }

        public List<ImportLogModel> DoImport(ImportParametersModel parametersModel)
        {
            if (!System.IO.File.Exists(parametersModel.FilePath))
            {
                LogList.Add(new ImportLogModel() { Description = "Arquivo não encontrado!" });
                return LogList;
            }

            LogBLL.Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            ImportLogModel logModel = new ImportLogModel();
            logModel.Description = "Iniciando importação";
            ImportLogBLL.AddLog(logModel);

            DocumentMappingDAO documentMappingDAO = new DocumentMappingDAO();
            List<DocumentMappingModel> documentMappingList = documentMappingDAO.GetMapping(parametersModel.ObjType, parametersModel.Layout);
            List<DocumentMappingItemModel> parametersList = new List<DocumentMappingItemModel>();
            foreach (var docMap in documentMappingList)
            {
                parametersList.AddRange(docMap.FieldsList.Where(d => !String.IsNullOrEmpty(d.Parameter)));
            }

            DefaultValuesDAO defaultValuesDAO = new DefaultValuesDAO();
            List<DefaultValuesModel> defaultValuesList = defaultValuesDAO.GetDefaultValues(parametersModel.ObjType, parametersModel.Layout);

            StreamReader sr = new StreamReader(parametersModel.FilePath);
            Documents doc = this.GetDocumentWithDefaultValues(parametersModel, defaultValuesList);

            if (HasError)
            {
                return LogList;
            }

            try
            {
                string line;
                doc.BPL_IDAssignedToInvoice = parametersModel.BPlId;

                int lineCount = 0;

                if ((ErrorHandlerEnum)parametersModel.ErrorHandler == ErrorHandlerEnum.WithTransaction)
                {
                    SBOApp.Company.StartTransaction();
                }

                while ((line = sr.ReadLine()) != null)
                {
                    lineCount++;
                    DocumentMappingModel mappingModel = documentMappingList.FirstOrDefault(d => line.StartsWith(d.LineIdentifier));
                    if (mappingModel == null) // Se não encontrou o identificador, vai para próxima linha
                    {
                        continue;
                    }

                    switch ((DocumentObjectEnum)mappingModel.LineType)
                    {
                        case DocumentObjectEnum.Header:
                            // Se já possui item e é um novo header, adiciona o atual
                            if (!String.IsNullOrEmpty(doc.Lines.ItemCode))
                            {
                                if (!HasError)
                                {
                                    this.SetLinesDefaultValues(ref doc, defaultValuesList);
                                    if (doc.Add() != 0)
                                    {
                                        logModel = new ImportLogModel();
                                        logModel.Description = $"Erro ao adicionar documento: " + SBOApp.Company.GetLastErrorDescription();

                                        ImportLogBLL.AddLog(logModel);
                                        LogList.Add(logModel);

                                        // Quando ocorre erro ao adicionar a transaction é parada
                                        if ((ErrorHandlerEnum)parametersModel.ErrorHandler == ErrorHandlerEnum.WithTransaction && !SBOApp.Company.InTransaction)
                                        {
                                            SBOApp.Company.StartTransaction();
                                        }
                                    }
                                    else
                                    {
                                        logModel = new ImportLogModel();
                                        logModel.Description = $"Documento adicionado: " + SBOApp.Company.GetNewObjectKey();

                                        ImportLogBLL.AddLog(logModel);
                                        LogList.Add(logModel);
                                    }
                                }

                                Marshal.ReleaseComObject(doc);
                                doc = null;

                                doc = this.GetDocumentWithDefaultValues(parametersModel, defaultValuesList);
                                doc.BPL_IDAssignedToInvoice = parametersModel.BPlId;
                                HasError = false;
                            }
                            break;
                        case DocumentObjectEnum.Line:
                            if (!String.IsNullOrEmpty(doc.Lines.ItemCode))
                            {
                                doc.Lines.Add();
                            }
                            break;
                    }

                    foreach (var fieldsModel in mappingModel.FieldsList)
                    {
                        object[] param = new object[1];
                        try
                        {
                            if (fieldsModel.PositionFrom > 0)
                            {
                                // Busca valor no arquivo texto
                                string strValue = line.Substring(fieldsModel.PositionFrom - 1, fieldsModel.Size);

                                switch ((FieldTypeEnum)fieldsModel.DiType)
                                {
                                    case FieldTypeEnum.AlphaNumeric:
                                        param[0] = strValue;
                                        break;
                                    case FieldTypeEnum.Date:
                                        fieldsModel.Format = fieldsModel.Format.Replace("D", "d").Replace("m", "M").Replace("Y", "y").Replace("A", "y").Replace("a", "y");
                                        param[0] = DateTime.ParseExact(strValue, fieldsModel.Format, CultureInfo.CurrentCulture);
                                        break;
                                    case FieldTypeEnum.Integer:
                                        param[0] = Convert.ToInt32(strValue);
                                        break;
                                    case FieldTypeEnum.Decimal:
                                        string decimalValue = strValue.Substring(0, fieldsModel.Size - fieldsModel.DecimalPlaces);
                                        decimalValue += "," + strValue.Substring(fieldsModel.Size - fieldsModel.DecimalPlaces, fieldsModel.DecimalPlaces);
                                        param[0] = Convert.ToDouble(decimalValue);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            HasError = true;
                            logModel = new ImportLogModel();
                            logModel.Line = lineCount;
                            logModel.Description = $"Erro ao buscar campo {fieldsModel.DiField} no arquivo: {ex.Message}";

                            ImportLogBLL.AddLog(logModel);
                            LogList.Add(logModel);
                            continue;
                        }

                        // Executa consulta caso exista
                        if (!String.IsNullOrEmpty(fieldsModel.Query.Trim()))
                        {
                            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                            string sql = fieldsModel.Query;
                            try
                            {
                                if (sql.Contains("{0}"))
                                {
                                    switch ((FieldTypeEnum)fieldsModel.DiType)
                                    {
                                        case FieldTypeEnum.Integer:
                                        case FieldTypeEnum.AlphaNumeric:
                                            sql = String.Format(sql, param[0].ToString());
                                            break;
                                        case FieldTypeEnum.Date:
                                            sql = String.Format(sql, ((DateTime)param[0]).ToString(fieldsModel.Format));
                                            break;
                                        case FieldTypeEnum.Decimal:
                                            sql = String.Format(sql, param[0].ToString().Replace(",", "."));
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                foreach (var parameterModel in parametersList)
                                {
                                    if (sql.Contains($"{{{parameterModel.Parameter}}}"))
                                    {
                                        switch ((FieldTypeEnum)fieldsModel.DiType)
                                        {
                                            case FieldTypeEnum.Integer:
                                                if (parameterModel.ParameterValue == null)
                                                {
                                                    parameterModel.ParameterValue = 0;
                                                }

                                                sql = sql.Replace($"{{{parameterModel.Parameter}}}", parameterModel.ParameterValue.ToString());
                                                break;
                                            case FieldTypeEnum.AlphaNumeric:
                                                if (parameterModel.ParameterValue == null)
                                                {
                                                    parameterModel.ParameterValue = "";
                                                }
                                                sql = sql.Replace($"{{{parameterModel.Parameter}}}", parameterModel.ParameterValue.ToString());
                                                break;
                                            case FieldTypeEnum.Date:
                                                if (parameterModel.ParameterValue == null)
                                                {
                                                    parameterModel.ParameterValue = new DateTime(1900, 01, 01);
                                                }
                                                sql = sql.Replace($"{{{parameterModel.Parameter}}}", $"{Convert.ToDateTime(parameterModel.ParameterValue).ToString("yyyy-MM-dd")}");
                                                break;
                                            case FieldTypeEnum.Decimal:
                                                if (parameterModel.ParameterValue == null)
                                                {
                                                    parameterModel.ParameterValue = 0;
                                                }
                                                sql = sql.Replace($"{{{parameterModel.Parameter}}}", parameterModel.ParameterValue.ToString().Replace(",", "."));
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                                rst.DoQuery(sql);
                                if (rst.Fields.Item(0).Value.ToString() == "")
                                {
                                    logModel = new ImportLogModel();
                                    logModel.Line = lineCount;
                                    logModel.Description = $"Consulta não retornou registros - {sql}";

                                    ImportLogBLL.AddLog(logModel);
                                    LogList.Add(logModel);
                                    continue;
                                }
                                else
                                {
                                    param[0] = rst.Fields.Item(0).Value;
                                }
                            }
                            catch (Exception ex)
                            {
                                HasError = true;
                                logModel = new ImportLogModel();
                                logModel.Line = lineCount;
                                logModel.Description = $"Erro ao executar consulta: {ex.Message} - {sql}";

                                ImportLogBLL.AddLog(logModel);
                                LogList.Add(logModel);

                                continue;
                            }
                            finally
                            {
                                Marshal.ReleaseComObject(rst);
                                rst = null;
                            }
                        }

                        try
                        {
                            if (!String.IsNullOrEmpty(fieldsModel.Parameter.Trim()))
                            {
                                fieldsModel.ParameterValue = param[0];
                            }

                            // Seta valor na propriedade
                            if (fieldsModel.DiField.StartsWith("U_"))
                            {
                                switch ((DocumentObjectEnum)mappingModel.LineType)
                                {
                                    case DocumentObjectEnum.Header:
                                        doc.UserFields.Fields.Item(fieldsModel.DiField).Value = param[0];
                                        break;
                                    case DocumentObjectEnum.Line:
                                        doc.Lines.UserFields.Fields.Item(fieldsModel.DiField).Value = param[0];
                                        break;
                                }
                            }
                            else
                            {
                                switch ((DocumentObjectEnum)mappingModel.LineType)
                                {
                                    case DocumentObjectEnum.Header:
                                        typeof(Documents).InvokeMember(fieldsModel.DiField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, doc, param);
                                        break;
                                    case DocumentObjectEnum.Line:
                                        typeof(Document_Lines).InvokeMember(fieldsModel.DiField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty, null, doc.Lines, param);
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            HasError = true;
                            logModel = new ImportLogModel();
                            logModel.Line = lineCount;
                            logModel.Description = $"Erro ao setar campo {fieldsModel.DiField}: {ex.Message}";

                            ImportLogBLL.AddLog(logModel);
                            LogList.Add(logModel);
                        }
                    }
                }

                if (!HasError)
                {
                    this.SetLinesDefaultValues(ref doc, defaultValuesList);
                    if (doc.Add() != 0)
                    {
                        logModel = new ImportLogModel();
                        logModel.Description = $"Erro ao adicionar documento: " + SBOApp.Company.GetLastErrorDescription();

                        ImportLogBLL.AddLog(logModel);
                        LogList.Add(logModel);
                    }
                    else
                    {
                        logModel = new ImportLogModel();
                        logModel.Description = $"Documento adicionado: " + SBOApp.Company.GetNewObjectKey();

                        ImportLogBLL.AddLog(logModel);
                        LogList.Add(logModel);
                    }

                    if (SBOApp.Company.InTransaction)
                    {
                        SBOApp.Company.EndTransaction(BoWfTransOpt.wf_Commit);
                    }
                }
                else
                {
                    if (SBOApp.Company.InTransaction)
                    {
                        SBOApp.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                    }
                }
            }
            catch (Exception ex)
            {
                logModel = new ImportLogModel();
                logModel.Description = $"Erro geral: " + ex.Message;

                ImportLogBLL.AddLog(logModel);
                LogList.Add(logModel);
            }
            finally
            {
                sr.Close();

                Marshal.ReleaseComObject(doc);
                doc = null;
            }

            return LogList;
        }

        private Documents GetDocumentWithDefaultValues(ImportParametersModel paremetersModel, List<DefaultValuesModel> defaultValuesList)
        {
            Documents doc = (Documents)SBOApp.Company.GetBusinessObject((BoObjectTypes)paremetersModel.ObjType);
            doc.BPL_IDAssignedToInvoice = paremetersModel.BPlId;

            int header = (int)DocumentObjectEnum.Header;
            defaultValuesList = defaultValuesList.Where(d => d.DiObj == header).ToList();

            foreach (var defaultValuesModel in defaultValuesList)
            {
                object[] param = new object[1];
                try
                {
                    switch ((FieldTypeEnum)defaultValuesModel.DiType)
                    {
                        case FieldTypeEnum.AlphaNumeric:
                            param[0] = defaultValuesModel.Value;
                            break;
                        case FieldTypeEnum.Date:
                            param[0] = DateTime.ParseExact(defaultValuesModel.Value, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                            break;
                        case FieldTypeEnum.Integer:
                            param[0] = Convert.ToInt32(defaultValuesModel.Value);
                            break;
                        case FieldTypeEnum.Decimal:
                            param[0] = Convert.ToDouble(defaultValuesModel.Value);
                            break;
                        default:
                            break;
                    }

                    if (defaultValuesModel.DiField.StartsWith("U_"))
                    {
                        doc.UserFields.Fields.Item(defaultValuesModel.DiField).Value = param[0];
                    }
                    else
                    {
                        typeof(Documents).InvokeMember(defaultValuesModel.DiField, System.Reflection.BindingFlags.SetProperty, null, doc, param);
                    }
                }
                catch (Exception ex)
                {
                    ImportLogModel logModel = new ImportLogModel();
                    logModel.Description = $"Erro ao setar valor default do campo {defaultValuesModel.DiField}: {ex.Message}";

                    ImportLogBLL.AddLog(logModel);
                    LogList.Add(logModel);
                    HasError = true;
                    continue;
                }
            }
            return doc;
        }

        private void SetLinesDefaultValues(ref Documents doc, List<DefaultValuesModel> defaultValuesList)
        {
            int line = (int)DocumentObjectEnum.Line;
            defaultValuesList = defaultValuesList.Where(d => d.DiObj == line).ToList();

            foreach (var defaultValuesModel in defaultValuesList)
            {
                object[] param = new object[1];
                try
                {
                    switch ((FieldTypeEnum)defaultValuesModel.DiType)
                    {
                        case FieldTypeEnum.AlphaNumeric:
                            param[0] = defaultValuesModel.Value;
                            break;
                        case FieldTypeEnum.Date:
                            param[0] = DateTime.ParseExact(defaultValuesModel.Value, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                            break;
                        case FieldTypeEnum.Integer:
                            param[0] = Convert.ToInt32(defaultValuesModel.Value);
                            break;
                        case FieldTypeEnum.Decimal:
                            param[0] = Convert.ToDouble(defaultValuesModel.Value);
                            break;
                        default:
                            break;
                    }

                    for (int i = 0; i < doc.Lines.Count; i++)
                    {
                        doc.Lines.SetCurrentLine(i);
                        if (defaultValuesModel.DiField.StartsWith("U_"))
                        {
                            doc.Lines.UserFields.Fields.Item(defaultValuesModel.DiField).Value = param[0];
                        }
                        else
                        {
                            typeof(Document_Lines).InvokeMember(defaultValuesModel.DiField, System.Reflection.BindingFlags.SetProperty, null, doc.Lines, param);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ImportLogModel logModel = new ImportLogModel();
                    logModel.Description = $"Erro ao setar valor default do campo {defaultValuesModel.DiField}: {ex.Message}";

                    ImportLogBLL.AddLog(logModel);
                    LogList.Add(logModel);
                    HasError = true;
                    continue;
                }
            }
        }
    }
}
