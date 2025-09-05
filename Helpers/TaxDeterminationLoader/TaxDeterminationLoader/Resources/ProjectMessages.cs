namespace TaxDeterminationLoader.Resources
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [DebuggerNonUserCode, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"), CompilerGenerated]
    internal class ProjectMessages
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal ProjectMessages()
        {
        }

        internal static string BatchStockInsufficient
        {
            get
            {
                return ResourceManager.GetString("BatchStockInsufficient", resourceCulture);
            }
        }

        internal static string BlankFilePath
        {
            get
            {
                return ResourceManager.GetString("BlankFilePath", resourceCulture);
            }
        }

        internal static string BlankSeparatorChar
        {
            get
            {
                return ResourceManager.GetString("BlankSeparatorChar", resourceCulture);
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

        internal static string DetTypeWrong
        {
            get
            {
                return ResourceManager.GetString("DetTypeWrong", resourceCulture);
            }
        }

        internal static string EfectFromMandatory
        {
            get
            {
                return ResourceManager.GetString("EfectFromMandatory", resourceCulture);
            }
        }

        internal static string Error1
        {
            get
            {
                return ResourceManager.GetString("Error1", resourceCulture);
            }
        }

        internal static string FailedImport
        {
            get
            {
                return ResourceManager.GetString("FailedImport", resourceCulture);
            }
        }

        internal static string FileNotExist
        {
            get
            {
                return ResourceManager.GetString("FileNotExist", resourceCulture);
            }
        }

        internal static string FileProblems1
        {
            get
            {
                return ResourceManager.GetString("FileProblems1", resourceCulture);
            }
        }

        internal static string FileProblems2
        {
            get
            {
                return ResourceManager.GetString("FileProblems2", resourceCulture);
            }
        }

        internal static string FileProblems3
        {
            get
            {
                return ResourceManager.GetString("FileProblems3", resourceCulture);
            }
        }

        internal static string InvalidDate
        {
            get
            {
                return ResourceManager.GetString("InvalidDate", resourceCulture);
            }
        }

        internal static string InvalidState
        {
            get
            {
                return ResourceManager.GetString("InvalidState", resourceCulture);
            }
        }

        internal static string InvalidTaxCode
        {
            get
            {
                return ResourceManager.GetString("InvalidTaxCode", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("TaxDeterminationLoader.Resources.ProjectMessages", typeof(ProjectMessages).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static string SQLError
        {
            get
            {
                return ResourceManager.GetString("SQLError", resourceCulture);
            }
        }

        internal static string SucessImport
        {
            get
            {
                return ResourceManager.GetString("SucessImport", resourceCulture);
            }
        }
    }
}

