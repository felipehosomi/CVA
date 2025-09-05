using CVA.View.EDoc.Model;

namespace CVA.View.EDoc.BLL
{
    public class EDocSqlBLL
    {
        private EDocFilterModel FilterModel;

        public EDocSqlBLL(EDocFilterModel filterModel)
        {
            this.FilterModel = filterModel;
        }

        public string GetDefaultSql(string proc)
        {
            string sql = $@"DECLARE @DataDe DATETIME
                            DECLARE @DataAte DATETIME

                            SET @DataDe = CAST('{FilterModel.DataDe}' AS DATETIME)
                            SET @DataAte = CAST('{FilterModel.DataAte}' AS DATETIME)

                            EXEC {proc} {FilterModel.Filial}, @DataDe, @DataAte";
            return sql;
        }

        public string Get0000Sql()
        {
            return this.GetDefaultSql("SP_CVA_EDOC_0000");
        }

        public string Get0005Sql()
        {
            string sql = $@"EXEC SP_CVA_EDOC_0005 {FilterModel.Filial}";
            return sql;
        }

        public string Get0100Sql()
        {
            string sql = $@"EXEC SP_CVA_EDOC_0100 {FilterModel.Filial}";
            return sql;
        }

        public string Get0150Sql()
        {
            return this.GetDefaultSql("SP_CVA_EDOC_0150");
        }

        public string Get0200Sql()
        {
            return this.GetDefaultSql("SP_CVA_EDOC_0200");
        }

        public string Get0400Sql()
        {
            return this.GetDefaultSql("SP_CVA_EDOC_0400");
        }

        public string GetC020Sql()
        {
            return this.GetDefaultSql("SP_CVA_EDOC_C020");
        }

        public string GetC300Sql()
        {
            return this.GetDefaultSql("SP_CVA_EDOC_C300");
        }
    }
}
