using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DelimitedDataHelper
{
    public class DelimitedDataFile
    {
        protected readonly string Delimiter;
        public string[] Rows { get; protected set; }

        public DelimitedDataFile(string fileName, string delimiter = "\t")
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException("File Not Found", fileName);
            Rows = File.ReadAllLines(fileName);
            Delimiter = delimiter;
        }

        public virtual DelimitedDataFile SkipNRows(int n)
        {
            Rows = Rows.Skip(n).ToArray();
            return this;
        }

        public virtual IEnumerable<T> GetData<T>(DelimitedFileReaderConfig config = null) where T : new()
        {
            if (config == null) config = new DelimitedFileReaderConfig();
            var objProperties = typeof(T).GetProperties();

            foreach (var x in Rows)
            {
                var items = x.Split(new[] { Delimiter }, StringSplitOptions.None);
                var result = new T();
                var i = 0;
                foreach (var propertyInfo in objProperties)
                {
                    var value = config.NeedTrimStartEndWhiteSpaces ? Sanitize(items[i]) : items[i];
                    propertyInfo.SetValue(result, Convert.ChangeType(value, propertyInfo.PropertyType));
                    i++;
                }
                yield return result;
            }
        }

        protected virtual string Sanitize(string s)
        {
            return s.Trim();
        }
    }
}
