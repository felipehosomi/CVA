using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Comissionamento.Data
{
    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    [System.Xml.Serialization.XmlRoot(Namespace = "", IsNullable = false)]
    public partial class UserTableBOM
    {

        private UserTableBOMBO[] boField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("BO")]
        public UserTableBOMBO[] BO
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
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class UserTableBOMBO
    {

        private UserTableBOMBOAdmInfo admInfoField;

        private UserTableBOMBOUserTablesMD userTablesMDField;

        /// <remarks/>
        public UserTableBOMBOAdmInfo AdmInfo
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
        public UserTableBOMBOUserTablesMD UserTablesMD
        {
            get
            {
                return this.userTablesMDField;
            }
            set
            {
                this.userTablesMDField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class UserTableBOMBOAdmInfo
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
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class UserTableBOMBOUserTablesMD
    {

        private UserTableBOMBOUserTablesMDRow rowField;

        /// <remarks/>
        public UserTableBOMBOUserTablesMDRow row
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
    [System.Xml.Serialization.XmlType(AnonymousType = true)]
    public partial class UserTableBOMBOUserTablesMDRow
    {

        private string tableNameField;

        private string tableDescriptionField;

        private string tableTypeField;

        private string archivableField;

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
        public string TableDescription
        {
            get
            {
                return this.tableDescriptionField;
            }
            set
            {
                this.tableDescriptionField = value;
            }
        }

        /// <remarks/>
        public string TableType
        {
            get
            {
                return this.tableTypeField;
            }
            set
            {
                this.tableTypeField = value;
            }
        }

        /// <remarks/>
        public string Archivable
        {
            get
            {
                return this.archivableField;
            }
            set
            {
                this.archivableField = value;
            }
        }
    }


}
