namespace TaxDeterminationLoader.Resources
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static string BusinessOneNotFound
        {
            get
            {
                return ResourceManager.GetString("BusinessOneNotFound", resourceCulture);
            }
        }

        internal static string ComboBoxDataSourceName
        {
            get
            {
                return ResourceManager.GetString("ComboBoxDataSourceName", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string DetermCodImposto_formImport
        {
            get
            {
                return ResourceManager.GetString("DetermCodImposto_formImport", resourceCulture);
            }
        }

        internal static string ErrorFoundWhileSavingXMLFile
        {
            get
            {
                return ResourceManager.GetString("ErrorFoundWhileSavingXMLFile", resourceCulture);
            }
        }

        internal static string QuerySboForDecimalAndThousandSeparator
        {
            get
            {
                return ResourceManager.GetString("QuerySboForDecimalAndThousandSeparator", resourceCulture);
            }
        }

        internal static string QuerySboForDecimalSeparator_FieldName
        {
            get
            {
                return ResourceManager.GetString("QuerySboForDecimalSeparator_FieldName", resourceCulture);
            }
        }

        internal static string QuerySboForThousandSeparator_FieldName
        {
            get
            {
                return ResourceManager.GetString("QuerySboForThousandSeparator_FieldName", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("TaxDeterminationLoader.Resources.Resources", typeof(TaxDeterminationLoader.Resources.Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static string SelectVerifDetType
        {
            get
            {
                return ResourceManager.GetString("SelectVerifDetType", resourceCulture);
            }
        }

        internal static string SelectVerifState
        {
            get
            {
                return ResourceManager.GetString("SelectVerifState", resourceCulture);
            }
        }

        internal static string SelectVerifyKeyFields
        {
            get
            {
                return ResourceManager.GetString("SelectVerifyKeyFields", resourceCulture);
            }
        }

        internal static string SelectVerifyTaxCode
        {
            get
            {
                return ResourceManager.GetString("SelectVerifyTaxCode", resourceCulture);
            }
        }

        internal static string XMLExtension
        {
            get
            {
                return ResourceManager.GetString("XMLExtension", resourceCulture);
            }
        }

        internal static string XMLFilesSubDir
        {
            get
            {
                return ResourceManager.GetString("XMLFilesSubDir", resourceCulture);
            }
        }
    }
}

