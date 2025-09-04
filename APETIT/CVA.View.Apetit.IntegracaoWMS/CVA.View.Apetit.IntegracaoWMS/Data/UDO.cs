using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.IntegracaoWMS.Data
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class UserObjectBOM
    {

        private UserObjectBOMBO[] boField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BO")]
        public UserObjectBOMBO[] BO
        {
            get
            {
                return this.boField;
            }
            set
            {
                this.boField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserObjectBOMBO
    {

        private UserObjectBOMBOAdmInfo admInfoField;

        private UserObjectBOMBOUserObjectsMD userObjectsMDField;

        private object userObjectMD_ChildTablesField;

        private UserObjectBOMBORow[] userObjectMD_FindColumnsField;

        private UserObjectBOMBORow1[] userObjectMD_FormColumnsField;

        private object userObjectMD_EnhancedFormColumnsField;

        /// <remarks/>
        public UserObjectBOMBOAdmInfo AdmInfo
        {
            get
            {
                return this.admInfoField;
            }
            set
            {
                this.admInfoField = value;
            }
        }

        /// <remarks/>
        public UserObjectBOMBOUserObjectsMD UserObjectsMD
        {
            get
            {
                return this.userObjectsMDField;
            }
            set
            {
                this.userObjectsMDField = value;
            }
        }

        /// <remarks/>
        public object UserObjectMD_ChildTables
        {
            get
            {
                return this.userObjectMD_ChildTablesField;
            }
            set
            {
                this.userObjectMD_ChildTablesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("row", IsNullable = false)]
        public UserObjectBOMBORow[] UserObjectMD_FindColumns
        {
            get
            {
                return this.userObjectMD_FindColumnsField;
            }
            set
            {
                this.userObjectMD_FindColumnsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("row", IsNullable = false)]
        public UserObjectBOMBORow1[] UserObjectMD_FormColumns
        {
            get
            {
                return this.userObjectMD_FormColumnsField;
            }
            set
            {
                this.userObjectMD_FormColumnsField = value;
            }
        }

        /// <remarks/>
        public object UserObjectMD_EnhancedFormColumns
        {
            get
            {
                return this.userObjectMD_EnhancedFormColumnsField;
            }
            set
            {
                this.userObjectMD_EnhancedFormColumnsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserObjectBOMBOAdmInfo
    {

        private byte objectField;

        private byte versionField;

        /// <remarks/>
        public byte Object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
            }
        }

        /// <remarks/>
        public byte Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserObjectBOMBOUserObjectsMD
    {

        private UserObjectBOMBOUserObjectsMDRow rowField;

        /// <remarks/>
        public UserObjectBOMBOUserObjectsMDRow row
        {
            get
            {
                return this.rowField;
            }
            set
            {
                this.rowField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserObjectBOMBOUserObjectsMDRow
    {

        private string codeField;

        private string nameField;

        private string tableNameField;

        private string logTableNameField;

        private string objectTypeField;

        private string manageSeriesField;

        private string canDeleteField;

        private string canCloseField;

        private string canCancelField;

        private object extensionNameField;

        private string canFindField;

        private string canYearTransferField;

        private string canCreateDefaultFormField;

        private string canLogField;

        private string overwriteDllfileField;

        private string useUniqueFormTypeField;

        private string canArchiveField;

        private string menuItemField;

        private object menuCaptionField;

        private string enableEnhancedFormField;

        private string rebuildEnhancedFormField;

        private string menuUIDField;

        private string canApproveField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string TableName
        {
            get
            {
                return this.tableNameField;
            }
            set
            {
                this.tableNameField = value;
            }
        }

        /// <remarks/>
        public string LogTableName
        {
            get
            {
                return this.logTableNameField;
            }
            set
            {
                this.logTableNameField = value;
            }
        }

        /// <remarks/>
        public string ObjectType
        {
            get
            {
                return this.objectTypeField;
            }
            set
            {
                this.objectTypeField = value;
            }
        }

        /// <remarks/>
        public string ManageSeries
        {
            get
            {
                return this.manageSeriesField;
            }
            set
            {
                this.manageSeriesField = value;
            }
        }

        /// <remarks/>
        public string CanDelete
        {
            get
            {
                return this.canDeleteField;
            }
            set
            {
                this.canDeleteField = value;
            }
        }

        /// <remarks/>
        public string CanClose
        {
            get
            {
                return this.canCloseField;
            }
            set
            {
                this.canCloseField = value;
            }
        }

        /// <remarks/>
        public string CanCancel
        {
            get
            {
                return this.canCancelField;
            }
            set
            {
                this.canCancelField = value;
            }
        }

        /// <remarks/>
        public object ExtensionName
        {
            get
            {
                return this.extensionNameField;
            }
            set
            {
                this.extensionNameField = value;
            }
        }

        /// <remarks/>
        public string CanFind
        {
            get
            {
                return this.canFindField;
            }
            set
            {
                this.canFindField = value;
            }
        }

        /// <remarks/>
        public string CanYearTransfer
        {
            get
            {
                return this.canYearTransferField;
            }
            set
            {
                this.canYearTransferField = value;
            }
        }

        /// <remarks/>
        public string CanCreateDefaultForm
        {
            get
            {
                return this.canCreateDefaultFormField;
            }
            set
            {
                this.canCreateDefaultFormField = value;
            }
        }

        /// <remarks/>
        public string CanLog
        {
            get
            {
                return this.canLogField;
            }
            set
            {
                this.canLogField = value;
            }
        }

        /// <remarks/>
        public string OverwriteDllfile
        {
            get
            {
                return this.overwriteDllfileField;
            }
            set
            {
                this.overwriteDllfileField = value;
            }
        }

        /// <remarks/>
        public string UseUniqueFormType
        {
            get
            {
                return this.useUniqueFormTypeField;
            }
            set
            {
                this.useUniqueFormTypeField = value;
            }
        }

        /// <remarks/>
        public string CanArchive
        {
            get
            {
                return this.canArchiveField;
            }
            set
            {
                this.canArchiveField = value;
            }
        }

        /// <remarks/>
        public string MenuItem
        {
            get
            {
                return this.menuItemField;
            }
            set
            {
                this.menuItemField = value;
            }
        }

        /// <remarks/>
        public object MenuCaption
        {
            get
            {
                return this.menuCaptionField;
            }
            set
            {
                this.menuCaptionField = value;
            }
        }

        /// <remarks/>
        public string EnableEnhancedForm
        {
            get
            {
                return this.enableEnhancedFormField;
            }
            set
            {
                this.enableEnhancedFormField = value;
            }
        }

        /// <remarks/>
        public string RebuildEnhancedForm
        {
            get
            {
                return this.rebuildEnhancedFormField;
            }
            set
            {
                this.rebuildEnhancedFormField = value;
            }
        }

        /// <remarks/>
        public string MenuUID
        {
            get
            {
                return this.menuUIDField;
            }
            set
            {
                this.menuUIDField = value;
            }
        }

        /// <remarks/>
        public string CanApprove
        {
            get
            {
                return this.canApproveField;
            }
            set
            {
                this.canApproveField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserObjectBOMBORow
    {

        private string columnAliasField;

        private string columnDescriptionField;

        /// <remarks/>
        public string ColumnAlias
        {
            get
            {
                return this.columnAliasField;
            }
            set
            {
                this.columnAliasField = value;
            }
        }

        /// <remarks/>
        public string ColumnDescription
        {
            get
            {
                return this.columnDescriptionField;
            }
            set
            {
                this.columnDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserObjectBOMBORow1
    {

        private byte sonNumberField;

        private string formColumnAliasField;

        private string formColumnDescriptionField;

        private string editableField;

        /// <remarks/>
        public byte SonNumber
        {
            get
            {
                return this.sonNumberField;
            }
            set
            {
                this.sonNumberField = value;
            }
        }

        /// <remarks/>
        public string FormColumnAlias
        {
            get
            {
                return this.formColumnAliasField;
            }
            set
            {
                this.formColumnAliasField = value;
            }
        }

        /// <remarks/>
        public string FormColumnDescription
        {
            get
            {
                return this.formColumnDescriptionField;
            }
            set
            {
                this.formColumnDescriptionField = value;
            }
        }

        /// <remarks/>
        public string Editable
        {
            get
            {
                return this.editableField;
            }
            set
            {
                this.editableField = value;
            }
        }
    }


}
