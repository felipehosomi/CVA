using System;
using SAPbouiCOM;
using MenuConsolidator.Controllers;

namespace MenuConsolidator.Extensions
{
    public static class ComboBoxExtensions
    {
        public static void AddValuesFromQuery(this ComboBox comboBox, string query, string fieldValue = null, string fieldDescription = null, bool noLock = true)
        {
            var recordset = (SAPbobsCOM.Recordset)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            comboBox.AddValuesFromQuery(recordset, query, fieldValue, fieldDescription, noLock);
        }

        


        public static SAPbobsCOM.Recordset AddValuesFromQuery(this ComboBox comboBox, SAPbobsCOM.Recordset recordset, string query, string fieldValue = null, string fieldDescription = null, bool noLock = true)
        {
            if (recordset == null)
            {
                throw new ArgumentNullException("recordset");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            recordset.DoQuery(query);

            return comboBox.AddValuesFromRecordset(recordset, fieldValue, fieldDescription);
        }

        public static SAPbobsCOM.Recordset AddValuesFromRecordset(this ComboBox comboBox, SAPbobsCOM.Recordset recordset, string fieldValue = null, string fieldDescription = null)
        {
            if (comboBox == null)
            {
                throw new ArgumentNullException("comboBox");
            }
            if (recordset == null)
            {
                throw new ArgumentNullException("recordset");
            }

            recordset.MoveFirst();

            if (fieldDescription == null)
            {
                SAPbobsCOM.Fields fields;

                if (fieldValue == null)
                {
                    fields = recordset.Fields;
                    if (fields.Count > 1)
                    {
                        fieldValue = fields.Item(0).Name;
                        fieldDescription = fields.Item(1).Name;
                    }
                    else
                    {
                        fieldValue = fieldDescription = fields.Item(0).Name;
                    }
                }
                else
                {
                    fields = recordset.Fields;
                    if (fields.Count > 1)
                    {
                        fieldDescription = (fields.Item(0).Name == fieldValue) ? fields.Item(1).Name : fields.Item(0).Name;
                    }
                    else
                    {
                        fieldDescription = fieldValue;
                    }
                }
            }

            var validValues = comboBox.ValidValues;

            while (!recordset.EoF)
            {
                var value = recordset.Fields.Item(fieldValue);
                var description = recordset.Fields.Item(fieldDescription);
                validValues.Add(((dynamic)value.Value).ToString(), ((dynamic)description.Value).ToString());
                recordset.MoveNext();
            }

            return recordset;
        }

        public static void AddValuesFromQuery(this Column colum, string query, string fieldValue = null, string fieldDescription = null, bool noLock = true)
        {
            var recordset = (SAPbobsCOM.Recordset)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            colum.AddValuesFromQuery(recordset, query, fieldValue, fieldDescription, noLock);
        }

        public static SAPbobsCOM.Recordset AddValuesFromQuery(this Column colum, SAPbobsCOM.Recordset recordset, string query, string fieldValue = null, string fieldDescription = null, bool noLock = true)
        {
            if (recordset == null)
            {
                throw new ArgumentNullException("recordset");
            }

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            recordset.DoQuery(query);

            return colum.AddValuesFromRecordset(recordset, fieldValue, fieldDescription);
        }

        public static SAPbobsCOM.Recordset AddValuesFromRecordset(this Column colum, SAPbobsCOM.Recordset recordset, string fieldValue = null, string fieldDescription = null)
        {
            if (colum == null)
            {
                throw new ArgumentNullException("colum");
            }
            if (recordset == null)
            {
                throw new ArgumentNullException("recordset");
            }

            recordset.MoveFirst();

            if (fieldDescription == null)
            {
                SAPbobsCOM.Fields fields;

                if (fieldValue == null)
                {
                    fields = recordset.Fields;
                    if (fields.Count > 1)
                    {
                        fieldValue = fields.Item(0).Name;
                        fieldDescription = fields.Item(1).Name;
                    }
                    else
                    {
                        fieldValue = fieldDescription = fields.Item(0).Name;
                    }
                }
                else
                {
                    fields = recordset.Fields;
                    if (fields.Count > 1)
                    {
                        fieldDescription = (fields.Item(0).Name == fieldValue) ? fields.Item(1).Name : fields.Item(0).Name;
                    }
                    else
                    {
                        fieldDescription = fieldValue;
                    }
                }
            }

            var validValues = colum.ValidValues;

            while (!recordset.EoF)
            {
                var value = recordset.Fields.Item(fieldValue);
                var description = recordset.Fields.Item(fieldDescription);
                validValues.Add(((dynamic)value.Value).ToString(), ((dynamic)description.Value).ToString());
                recordset.MoveNext();
            }

            return recordset;
        }
    }
}
