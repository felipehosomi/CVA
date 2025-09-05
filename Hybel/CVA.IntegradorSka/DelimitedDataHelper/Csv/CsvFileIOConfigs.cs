namespace DelimitedDataHelper.Csv
{
    public class CsvReaderConfig : DelimitedFileReaderConfig
    {
        public bool IsQuoted { get; set; }

        public CsvReaderConfig(bool isQuoted = true, bool needTrimStartEndWhiteSpaces = true) : base(needTrimStartEndWhiteSpaces)
        {
            IsQuoted = isQuoted;
        }

        public CsvReaderConfig()
        {
            IsQuoted = true;
            NeedTrimStartEndWhiteSpaces = true;
        }
    }

    public class CsvWriterConfig : DelimitedFileWriterConfig
    {
        public bool IsQuoted { get; set; }

        public CsvWriterConfig(bool isQuoted = true, bool writeHeader = true) : base(writeHeader)
        {
            IsQuoted = isQuoted;
        }
        
        public CsvWriterConfig()
        {
            IsQuoted = true;
            WriteHeader = true;
        }
    }
}
