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
    public partial class UserFieldBOM
    {

        private UserFieldBOMBO[] boField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BO")]
        public UserFieldBOMBO[] BO
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
    public partial class UserFieldBOMBO
    {

        private UserFieldBOMBOAdmInfo admInfoField;

        private UserFieldBOMBOUserFieldsMD userFieldsMDField;

        private UserFieldBOMBORow[] validValuesMDField;

        /// <remarks/>
        public UserFieldBOMBOAdmInfo AdmInfo
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
        public UserFieldBOMBOUserFieldsMD UserFieldsMD
        {
            get
            {
                return this.userFieldsMDField;
            }
            set
            {
                this.userFieldsMDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("row", IsNullable = false)]
        public UserFieldBOMBORow[] ValidValuesMD
        {
            get
            {
                return this.validValuesMDField;
            }
            set
            {
                this.validValuesMDField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserFieldBOMBOAdmInfo
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
    public partial class UserFieldBOMBOUserFieldsMD
    {

        private UserFieldBOMBOUserFieldsMDRow rowField;

        /// <remarks/>
        public UserFieldBOMBOUserFieldsMDRow row
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
    public partial class UserFieldBOMBOUserFieldsMDRow
    {

        private string nameField;

        private string typeField;

        private byte sizeField;

        private string descriptionField;

        private string subTypeField;

        private string linkedTableField;

        private string defaultValueField;

        private string tableNameField;

        private byte fieldIDField;

        private byte editSizeField;

        private string mandatoryField;

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
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public byte Size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string SubType
        {
            get
            {
                return this.subTypeField;
            }
            set
            {
                this.subTypeField = value;
            }
        }

        /// <remarks/>
        public string LinkedTable
        {
            get
            {
                return this.linkedTableField;
            }
            set
            {
                this.linkedTableField = value;
            }
        }

        /// <remarks/>
        public string DefaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
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
        public byte FieldID
        {
            get
            {
                return this.fieldIDField;
            }
            set
            {
                this.fieldIDField = value;
            }
        }

        /// <remarks/>
        public byte EditSize
        {
            get
            {
                return this.editSizeField;
            }
            set
            {
                this.editSizeField = value;
            }
        }

        /// <remarks/>
        public string Mandatory
        {
            get
            {
                return this.mandatoryField;
            }
            set
            {
                this.mandatoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class UserFieldBOMBORow
    {

        private string valueField;

        private string descriptionField;

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }


}
