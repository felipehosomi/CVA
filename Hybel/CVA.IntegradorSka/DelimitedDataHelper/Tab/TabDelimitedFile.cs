using System.Collections.Generic;

namespace DelimitedDataHelper.Tab
{
    public class TabDelimitedFile : DelimitedDataFile
    {
        public TabDelimitedFile(string fileName) : base(fileName)
        {

        }
    }

    public static class TabDelimitedDataWriter
    {
        public static void WriteToTabDelimitedFile<T>(this IEnumerable<T> data, string fileName, DelimitedFileWriterConfig config = null)
        {
            new DelimitedFileWriter().CreateFileWithData(data, fileName, config);
        }
    }
}
