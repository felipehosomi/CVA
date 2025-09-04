using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackIndicator.Models
{
    class OrdersLines
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class DataTable
        {

            private DataTableRow[] rowsField;

            private string uidField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Row", IsNullable = false)]
            public DataTableRow[] Rows
            {
                get
                {
                    return this.rowsField;
                }
                set
                {
                    this.rowsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Uid
            {
                get
                {
                    return this.uidField;
                }
                set
                {
                    this.uidField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class DataTableRow
        {

            private DataTableRowCell[] cellsField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Cell", IsNullable = false)]
            public DataTableRowCell[] Cells
            {
                get
                {
                    return this.cellsField;
                }
                set
                {
                    this.cellsField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class DataTableRowCell
        {

            private string columnUidField;

            private string valueField;

            /// <remarks/>
            public string ColumnUid
            {
                get
                {
                    return this.columnUidField;
                }
                set
                {
                    this.columnUidField = value;
                }
            }

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
        }


    }
}
