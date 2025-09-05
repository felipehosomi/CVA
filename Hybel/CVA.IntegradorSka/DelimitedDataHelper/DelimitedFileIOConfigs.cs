namespace DelimitedDataHelper
{
    public class DelimitedFileReaderConfig
    {
        public bool NeedTrimStartEndWhiteSpaces { get; set; }

        public DelimitedFileReaderConfig()
        {
            NeedTrimStartEndWhiteSpaces = true;
        }

        public DelimitedFileReaderConfig(bool needTrimStartEndWhiteSpaces)
        {
            NeedTrimStartEndWhiteSpaces = needTrimStartEndWhiteSpaces;
        }
    }

    public class DelimitedFileWriterConfig
    {
        public bool WriteHeader { get; set; }

        public DelimitedFileWriterConfig()
        {
            WriteHeader = true;
        }

        public DelimitedFileWriterConfig(bool writeHeader)
        {
            WriteHeader = writeHeader;
        }
    }
}
