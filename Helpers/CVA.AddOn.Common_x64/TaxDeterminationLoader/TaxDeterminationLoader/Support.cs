namespace TaxDeterminationLoader
{
    using SAPbobsCOM;
    using SAPbouiCOM;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;
    using Resources;

    internal sealed class Support
    {
        public static SAPbobsCOM.Company _oCompany;
        private static Form _oForm;
        private static Item _oItem;
        private static Matrix _oMatrix;
        public static Application _sboApp;
        private string _sboCnx = Environment.GetCommandLineArgs().GetValue(1).ToString();
        private static string _statusbarMessagesPrefix;
        private const int cellHeight = 15;
        private const int titleHeight = 0x11;

        private string CorrectCellAndTitleHightsInFormXMLString(string xmlString)
        {
            xmlString = ReplaceItemValueInXMLString(xmlString, itemToChangeType.TitleHeight, 0x11);
            return ReplaceItemValueInXMLString(xmlString, itemToChangeType.CellHeight, 15);
        }

        private static string CorrectNumberDecimalSeparator(string inNumber, DecimalSeparatorType separatorType)
        {
            string str;
            string str2;
            double num = 0.2;
            string newValue = num.ToString().Substring(1, 1);
            try
            {
                Recordset businessObject = (Recordset)_oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                businessObject.DoQuery(Resources.Resources.QuerySboForDecimalAndThousandSeparator);
                str = businessObject.Fields.Item(Resources.Resources.QuerySboForDecimalSeparator_FieldName).Value.ToString();
                str2 = businessObject.Fields.Item(Resources.Resources.QuerySboForThousandSeparator_FieldName).Value.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            StringBuilder builder = new StringBuilder();
            foreach (char ch in inNumber)
            {
                switch (ch)
                {
                    case ',':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        builder.Append(ch);
                        break;
                }
            }
            inNumber = builder.ToString();
            if (separatorType == DecimalSeparatorType.Windows)
            {
                inNumber = inNumber.Replace(str2, string.Empty);
            }
            string str4 = inNumber;
            if (str != newValue)
            {
                switch (separatorType)
                {
                    case DecimalSeparatorType.Windows:
                        return inNumber.Replace(str, newValue);

                    case DecimalSeparatorType.Sbo:
                        return inNumber.Replace(newValue, str);
                }
            }
            return str4;
        }

        private static string CorrectNumberDecimalSeparatorFromGridToWindowsFormat(string inNumber)
        {
            string str2;
            string str3;
            double num = 0.2;
            string newValue = num.ToString().Substring(1, 1);
            try
            {
                Recordset businessObject = (Recordset)_oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                businessObject.DoQuery(Resources.Resources.QuerySboForDecimalAndThousandSeparator);
                str2 = businessObject.Fields.Item(Resources.Resources.QuerySboForDecimalSeparator_FieldName).Value.ToString();
                str3 = businessObject.Fields.Item(Resources.Resources.QuerySboForThousandSeparator_FieldName).Value.ToString();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            StringBuilder builder = new StringBuilder();
            foreach (char ch in inNumber)
            {
                switch (ch)
                {
                    case ',':
                    case '-':
                    case '.':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        builder.Append(ch);
                        break;
                }
            }
            inNumber = builder.ToString();
            inNumber = inNumber.Replace(str3, string.Empty);
            return inNumber.Replace(str2, newValue);
        }

        public string[] fileReader(string filePath, string separator)
        {
            string[] strArray = null;
            using (StreamReader reader = new StreamReader(filePath))
            {
                long num = 0L;
                string str = "";
                while ((str = reader.ReadLine()) != null)
                {
                    num += 1L;
                    if (str != "")
                    {
                        strArray = str.Split(new char[] { separator[0] });
                    }
                }
            }
            return strArray;
        }

        internal static int GetEditBoxInt32(string editBoxName)
        {
            EditText specific = (EditText)_oForm.Items.Item(editBoxName).Specific;
            string str = specific.String;
            return (string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str));
        }

        internal static decimal GetMatrixItemFromCurrentRowAsDecimal(string columnName, int rowIndex)
        {
            decimal num;
            try
            {
                Column o = _oMatrix.Columns.Item(columnName);
                EditText specific = (EditText)o.Cells.Item(rowIndex).Specific;
                string inNumber = specific.String;
                Marshal.ReleaseComObject(specific);
                Marshal.ReleaseComObject(o);
                num = Convert.ToDecimal(CorrectNumberDecimalSeparator(CorrectNumberDecimalSeparatorFromGridToWindowsFormat(inNumber), DecimalSeparatorType.Windows));
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return num;
        }

        internal static string GetMatrixItemFromCurrentRowAsString(string columnName, int rowIndex)
        {
            string str2;
            int iRowIndex = rowIndex == -1 ? 1 : rowIndex;
            try
            {
                Column o = _oMatrix.Columns.Item(columnName);
                EditText specific = (EditText)o.Cells.Item(iRowIndex).Specific;
                string str = specific.String;
                Marshal.ReleaseComObject(specific);
                Marshal.ReleaseComObject(o);
                str2 = str;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str2;
        }

        public Recordset getRecordSet(string query, string conditions)
        {
            Recordset businessObject = (Recordset)_oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                businessObject.DoQuery(query + conditions);
            }
            catch (Exception exception)
            {
                Log.writeLog(ProjectMessages.SQLError + ";" + exception.Message);
                Console.WriteLine(exception.Message);
            }
            return businessObject;
        }

        public string getRecordSetResult(Recordset RS, int pos)
        {
            return RS.Fields.Item(pos).Value.ToString();
        }

        public string getSAPFComboValue(string form, string item, string type)
        {
            _oItem = _sboApp.Forms.GetForm(form, 1).Items.Item(item);
            ComboBox specific = (ComboBox)_oItem.Specific;
            if (specific.Selected != null)
            {
                if (type == "value")
                {
                    return specific.Selected.Value;
                }
                if (type == "description")
                {
                    return specific.Selected.Description;
                }
                return "";
            }
            return "";
        }

        public string getUDFComboValue(string formUID, string comboUID, string type)
        {
            _oItem = _sboApp.Forms.Item(formUID).Items.Item(comboUID);
            ComboBox specific = (ComboBox)_oItem.Specific;
            if (specific.Selected != null)
            {
                if (type == "value")
                {
                    return specific.Selected.Value;
                }
                if (type == "description")
                {
                    return specific.Selected.Description;
                }
                return "";
            }
            return "";
        }

        public string getUDFEditValue(string formUID, string editUID)
        {
            _oItem = _sboApp.Forms.Item(formUID).Items.Item(editUID);
            EditText specific = (EditText)_oItem.Specific;
            return specific.Value;
        }

        internal static bool IsFormOpen(string formType)
        {
            bool flag2;
            bool flag = false;
            try
            {
                for (int i = 0; i < _sboApp.Forms.Count; i++)
                {
                    if (_sboApp.Forms.Item(i).TypeEx == formType)
                    {
                        flag = true;
                        break;
                    }
                }
                flag2 = flag;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return flag2;
        }

        private void LoadAFormFromFile(string xmlFileInnerString, bool makeVisible, string uniqueID, string formType, BoFormBorderStyle formBorderStyle)
        {
            try
            {
                FormCreationParams creationPackage = (FormCreationParams)pSbo.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.XmlData = xmlFileInnerString;
                if (!string.IsNullOrEmpty(uniqueID))
                {
                    creationPackage.UniqueID = uniqueID;
                }
                if (!string.IsNullOrEmpty(formType))
                {
                    creationPackage.FormType = formType;
                }
                creationPackage.BorderStyle = formBorderStyle;
                _oForm = pSbo.Forms.AddEx(creationPackage);
                _oForm.Visible = makeVisible;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void LoadAFormFromSRF(string formType, string xmlFile, bool fileContents, bool makeVisible)
        {
            LoadAFormFromSRF(formType, xmlFile, fileContents, makeVisible, false);
        }

        public void LoadAFormFromSRF(string formType, string xmlFile, bool fileContents, bool makeVisible, bool freeze)
        {
            try
            {
                string innerXml;
                if (fileContents)
                {
                    innerXml = xmlFile;
                }
                else
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlFile);
                    innerXml = document.InnerXml;
                }
                FormCreationParams creationPackage = (FormCreationParams)pSbo.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
                creationPackage.FormType = formType;
                creationPackage.UniqueID = formType;
                creationPackage.XmlData = innerXml;
                _oForm = pSbo.Forms.AddEx(creationPackage);
                _oForm.Freeze(freeze);
            }
            catch (Exception exception)
            {
                ShowMessageInStatusBar("Houve um erro na carga de um formul\x00e1rio: " + exception.Message, BoStatusBarMessageType.smt_Error);
            }
        }

        public void LoadAFormFromSRF(string xmlFile, bool fileContents, bool makeVisible, string uniqueID, string formType, BoFormBorderStyle formBorderStyle)
        {
            try
            {
                string innerXml;
                if (fileContents)
                {
                    innerXml = xmlFile;
                }
                else
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlFile);
                    innerXml = document.InnerXml;
                }
                LoadAFormFromFile(innerXml, makeVisible, uniqueID, formType, formBorderStyle);
            }
            catch (Exception exception)
            {
                ShowMessageInStatusBar("Houve um erro na carga de um formul\x00e1rio: " + exception.Message, BoStatusBarMessageType.smt_Error);
            }
        }

        public void LoadForm(string xmlFile, string type)
        {
            try
            {
                if (type == "path")
                {
                    LoadSRFfromPath(ref xmlFile);
                }
                else if (type == "xmlstring")
                {
                    LoadSRFfromXML(ref xmlFile);
                }
            }
            catch (Exception exception)
            {
                Log.writeLog("Erro ao abrir form;" + exception.Message);
                Console.WriteLine(exception.Message);
            }
        }

        public bool LoadFormSafely(string formUniqueID, string formXMLDefinition)
        {
            return this.LoadFormSafely(formUniqueID, formXMLDefinition, false);
        }

        public bool LoadFormSafely(string formUniqueID, string formXMLDefinition, bool freezeForm)
        {
            return this.LoadFormSafely(formUniqueID, formXMLDefinition, string.Empty, freezeForm);
        }

        public bool LoadFormSafely(string formUniqueID, string formXMLDefinition, bool freezeForm, bool makeVisible)
        {
            return this.LoadFormSafely(formUniqueID, formXMLDefinition, string.Empty, freezeForm, makeVisible, true);
        }

        public bool LoadFormSafely(string formUniqueID, string formXMLDefinition, string formTitle, bool freezeForm)
        {
            return this.LoadFormSafely(formUniqueID, formXMLDefinition, string.Empty, freezeForm, true, true);
        }

        public bool LoadFormSafely(string formUniqueID, string formXMLDefinition, bool freezeForm, bool makeVisible, bool correctMatricesHeights)
        {
            return this.LoadFormSafely(formUniqueID, formXMLDefinition, string.Empty, freezeForm, makeVisible, correctMatricesHeights);
        }

        public bool LoadFormSafely(string formUniqueID, string formXMLDefinition, string formTitle, bool freezeForm, bool makeVisible)
        {
            bool flag = false;
            foreach (SAPbouiCOM.Form form in pSbo.Forms)
            {
                if (form.UniqueID == formUniqueID)
                {
                    form.State = BoFormStateEnum.fs_Restore;
                    _oForm = form;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                this.LoadAFormFromSRF(formUniqueID, formXMLDefinition, true, makeVisible);
                if (!string.IsNullOrEmpty(formTitle))
                {
                    _oForm.Title = formTitle;
                }
                _oForm.Freeze(false);
            }
            else
            {
                _oForm.Visible = makeVisible;
            }
            if (freezeForm)
            {
                _oForm.Freeze(true);
            }
            return flag;
        }

        public bool LoadFormSafely(string formUniqueID, string formXMLDefinition, string formTitle, bool freezeForm, bool makeVisible, bool correctMatricesHeights)
        {
            bool flag = false;
            foreach (SAPbouiCOM.Form form in pSbo.Forms)
            {
                if (form.UniqueID == formUniqueID)
                {
                    form.State = BoFormStateEnum.fs_Restore;
                    _oForm = form;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                if (correctMatricesHeights)
                {
                    formXMLDefinition = this.CorrectCellAndTitleHightsInFormXMLString(formXMLDefinition);
                }
                this.LoadAFormFromSRF(formUniqueID, formXMLDefinition, true, makeVisible);
                if (!string.IsNullOrEmpty(formTitle))
                {
                    _oForm.Title = formTitle;
                }
                _oForm.Freeze(false);
            }
            else
            {
                _oForm.Visible = makeVisible;
            }
            if (freezeForm)
            {
                _oForm.Freeze(true);
            }
            return flag;
        }

        private void LoadSRFfromPath(ref string FileName)
        {
            XmlDocument document = new XmlDocument();
            document.Load((System.Windows.Forms.Application.StartupPath + @"\Forms") + @"\" + FileName);
            string xmlStr = document.InnerXml.ToString();
            _sboApp.LoadBatchActions(ref xmlStr);
        }

        private void LoadSRFfromXML(ref string xmlFile)
        {
            XmlDocument document = new XmlDocument();
            _sboApp.LoadBatchActions(ref xmlFile);
        }

        private string ReplaceItemValueInXMLString(string xmlString, itemToChangeType itemToChange, int newValue)
        {
            string str = string.Empty;
            switch (itemToChange)
            {
                case itemToChangeType.TitleHeight:
                    str = "titleHeight=";
                    break;

                case itemToChangeType.CellHeight:
                    str = "cellHeight=";
                    break;
            }
            string[] strArray = xmlString.Split(new string[] { str }, StringSplitOptions.RemoveEmptyEntries);
            int length = strArray.Length;
            if (length == 1)
            {
                return xmlString;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(strArray[0]);
            for (int i = 1; i < length; i++)
            {
                builder.Append(str);
                builder.Append('"');
                builder.Append(newValue.ToString());
                builder.Append('"');
                int index = strArray[i].Substring(1).IndexOf('"');
                builder.Append(strArray[i].Substring(index + 2));
            }
            return builder.ToString();
        }

        internal void SetApplication()
        {
            SboGuiApi api = new SboGuiApi();
            try
            {
                api.Connect(this._sboCnx);
                _sboApp = api.GetApplication();
                _oCompany = new SAPbobsCOM.Company();
                _oCompany = (SAPbobsCOM.Company)_sboApp.Company.GetDICompany();
                if (_oCompany.Connected)
                {
                    ShowMessageInStatusBar("Conectado!", BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    ShowMessageInStatusBar("N\x00e3o Conectado!", BoStatusBarMessageType.smt_Error);
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        internal static void SetEventHandlingFilters(List<EventFilterType> eventFilters)
        {
            EventFilters filters = new EventFilters();
            foreach (EventFilterType type in eventFilters)
            {
                EventFilter filter = filters.Add(type._eventType);
                foreach (string str in type._formUniqueId)
                {
                    filter.AddEx(str);
                }
            }
            _sboApp.SetFilter(filters);
        }

        internal static void SetMatrixContents(string columnName, int columnPos, int rowIndex, string textToSet)
        {
            Column column = null;
            if (columnName == "")
            {
                column = _oMatrix.Columns.Item(columnPos);
            }
            else if (columnName != "")
            {
                column = _oMatrix.Columns.Item(columnName);
            }
            EditText editText = column.Cells.Item(rowIndex).Specific as EditText;
            if (editText != null)
            {
                editText.String = textToSet;
                Marshal.ReleaseComObject(editText);
            }
            ComboBox comboBox = column.Cells.Item(rowIndex).Specific as ComboBox;
            if (comboBox != null)
            {
                try
                {
                    for (int i = 0; i < comboBox.ValidValues.Count; i++)
                    {
                        string value = comboBox.ValidValues.Item(i).Value;
                        if (value.Contains("_"))
                        {
                            value = value.Split('_')[value.Split('_').Length - 1];
                        }
                        if (value.ToLower() == textToSet.ToLower() || comboBox.ValidValues.Item(i).Description.ToLower() == textToSet.ToLower())
                        {
                            comboBox.Select(i, BoSearchKey.psk_Index);
                            break;
                        }
                    }
                }
                catch
                {
                    try
                    {
                        comboBox.Select(textToSet, BoSearchKey.psk_ByDescription);
                    }
                    catch { }
                }
                Marshal.ReleaseComObject(comboBox);
            }
            CheckBox checkBox = column.Cells.Item(rowIndex).Specific as CheckBox;
            if (checkBox != null)
            {
                checkBox.Checked = checkBox.ValOn == textToSet || textToSet.ToLower() == "true";
                Marshal.ReleaseComObject(checkBox);
            }
            
            Marshal.ReleaseComObject(column);
        }

        internal static void SetoFormByFormTypeEx(string formTypeEx)
        {
            try
            {
                for (int i = 0; i < _sboApp.Forms.Count; i++)
                {
                    SAPbouiCOM.Form form = _sboApp.Forms.Item(i);
                    if (form.TypeEx == formTypeEx)
                    {
                        _oForm = form;
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        internal static void SetoMatrix(string matrixName)
        {
            SetoMatrix(matrixName, true);
        }

        private static void SetoMatrix(string matrixName, bool specific)
        {
            if (specific)
            {
                try
                {
                    _oMatrix = (Matrix)_oForm.Items.Item(matrixName).Specific;
                }
                catch (Exception exception)
                {
                    ShowMessageInStatusBar(exception.Message, BoStatusBarMessageType.smt_Error);
                }
            }
            else
            {
                _oMatrix = (Matrix)_oForm.Items.Item(matrixName);
            }
        }

        public static void FreezeForm(bool freeze)
        {
            _oForm.Freeze(freeze);
        }

        public void setUDFEditValue(string formUID, string editUID, string val)
        {
            _oItem = _sboApp.Forms.Item(formUID).Items.Item(editUID);
            EditText specific = (EditText)_oItem.Specific;
            specific.String = val;
        }

        internal static int showMessageBox(string message, int type, string btn1, string btn2, string btn3)
        {
            return _sboApp.MessageBox(message, type, btn1, btn2, btn3);
        }

        internal static void ShowMessageInStatusBar(string message, BoStatusBarMessageType obj)
        {
            _sboApp.StatusBar.SetText(_statusbarMessagesPrefix + ": " + message, BoMessageTime.bmt_Short, obj);
        }

        internal static Matrix oMatrix
        {
            get
            {
                return _oMatrix;
            }
        }

        internal static SAPbouiCOM.Application pSbo
        {
            get
            {
                return _sboApp;
            }
        }

        internal static string StatusbarMessagesPrefix
        {
            set
            {
                _statusbarMessagesPrefix = value;
            }
        }

        private enum DecimalSeparatorType
        {
            Windows,
            Sbo
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct EventFilterType
        {
            internal BoEventTypes _eventType;
            internal string[] _formUniqueId;
        }

        public enum itemToChangeType
        {
            TitleHeight,
            CellHeight
        }
    }
}

