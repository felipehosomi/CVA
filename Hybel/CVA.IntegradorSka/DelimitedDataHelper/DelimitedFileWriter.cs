using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DelimitedDataHelper
{
    internal class DelimitedFileWriter
    {
        private readonly string _delimiter;

        public DelimitedFileWriter(string delimiter = "\t")
        {
            _delimiter = delimiter;
        }

        public void CreateFileWithData<T>(IEnumerable<T> data, string fileName, DelimitedFileWriterConfig config = null)
        {
            if (config == null) config = new DelimitedFileWriterConfig();
            if (File.Exists(fileName)) File.Delete(fileName);
            var objProperties = typeof(T).GetProperties();

            if (config.WriteHeader) WriteHeader(fileName, objProperties);
            File.AppendAllText(fileName, WriteRows(data, objProperties));
        }

        protected virtual void WriteHeader(string output, PropertyInfo[] objProperties)
        {
            var result = new StringBuilder();
            foreach (var objProperty in objProperties)
            {
                bool ignore = false;
                // Busca os Custom Attributes
                foreach (Attribute attribute in objProperty.GetCustomAttributes(true))
                {
                    FileConfigAttribute fileConfigAttribute = attribute as FileConfigAttribute;
                    if (fileConfigAttribute != null)
                    {
                        if (fileConfigAttribute.Ignore)
                        {
                            ignore = true;
                            break;
                        }
                    }
                }
                if (!ignore)
                {
                    result.Append(Escape(objProperty.Name)).Append(_delimiter);
                }
            }

            result.Length -= _delimiter.Length;
            result.AppendLine();
            File.AppendAllText(output, result.ToString());
        }

        protected virtual string WriteRows<T>(IEnumerable<T> objs, PropertyInfo[] objProperties)
        {
            if (objs == null) return null;

            var result = new StringBuilder();

            foreach (var obj in objs)
            {
                foreach (var objProperty in objProperties)
                {
                    bool ignore = false;
                    // Busca os Custom Attributes
                    foreach (Attribute attribute in objProperty.GetCustomAttributes(true))
                    {
                        FileConfigAttribute fileConfigAttribute = attribute as FileConfigAttribute;
                        if (fileConfigAttribute != null)
                        {
                            if (fileConfigAttribute.Ignore)
                            {
                                ignore = true;
                                break;
                            }
                        }

                    }
                    if (!ignore)
                    {
                        string value = Escape(objProperty.GetValue(obj));
                        result.Append(value).Append(_delimiter);
                    }
                }
                result.Length -= _delimiter.Length;
                result.AppendLine();
            }

            return result.ToString();
        }

        protected virtual string Escape(object o)
        {
            return o?.ToString() ?? string.Empty;
        }
    }
}
