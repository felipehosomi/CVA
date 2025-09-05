using System.Collections.Generic;

namespace DelimitedDataHelper.Pipe
{
    public class PipeDelimitedFile : DelimitedDataFile
    {
        public PipeDelimitedFile(string fileName) : base(fileName, "|")
        {

        }
    }

    public static class PipeDelimitedDataWriter
    {
        public static void WriteToPipeDelimitedFile<T>(this IEnumerable<T> data, string fileName, DelimitedFileWriterConfig config = null)
        {
            new DelimitedFileWriter("|").CreateFileWithData(data, fileName, config);
        }
    }
}
