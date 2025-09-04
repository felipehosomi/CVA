using SAPbouiCOM;

namespace MenuConsolidator.Extensions
{
    public static class DataSourceExtensions
    {
        public static DataTable AddRow(this DataTable source, int count, bool firstRow = false)
        {
            if (firstRow)
            {
                source.Rows.Add(count);
                source.Rows.Remove(source.Rows.Count - 1);
            }
            else
            {
                source.Rows.Add(count);
            }

            return source;
        }

        public static DBDataSource InsertRow(this DBDataSource source, bool firstRow = false)
        {
            if (firstRow)
            {
                if (source.Size > 0)
                {
                    source.InsertRecord(0);
                    source.RemoveRecord(1);
                }
                else
                {
                    source.InsertRecord(0);
                    source.InsertRecord(0);
                    source.RemoveRecord(1);
                }
            }
            else
            {
                source.InsertRecord(source.Size);
            }

            return source;
        }
    }
}
