using System.Collections.Generic;

namespace DelimitedDataHelper.Csv
{
    internal class CsvFileWriter : DelimitedFileWriter
    {
        public CsvFileWriter() : base(",")
        {

        }

        protected override string Escape(object o)
        {
            return o == null ? "" : $"\"{o.ToString().Replace("\"", "\"\"")}\"";
        }
    }

    public static class CsvFileExtensions
    {
        public static void WriteToCsvFile<T>(this IEnumerable<T> data, string fileName, CsvWriterConfig config = null)
        {
            if (config == null) config = new CsvWriterConfig();
            if (config.IsQuoted)
            {
                new CsvFileWriter().CreateFileWithData(data, fileName, config);
                return;
            }
            new DelimitedFileWriter().CreateFileWithData(data, fileName, config);
        }
    }
}
