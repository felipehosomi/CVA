using Sap.Data.Hana;
using System.Collections.Generic;
using System.Data;

namespace CVA.Escoteiro.Magento.DAO
{
    public interface IDAO
    {
        object Model { get; set; }
        string TableName { get; set; }
        //string DataBase { get; set; }

        void BeginTransaction();
        void Close();
        void CommitTransaction();
        void Connect();
        void CreateModel(string database);
        void DeleteModel(string database);
        HanaDataReader ExecuteReader(string Hana);
        void ExecuteNonQuery(string command);
        object ExecuteScalar(string command);
        bool Exists(string where, string database);
        DataTable FillDataTable(string command);
        List<T> FillListFromCommand<T>(string command);
        T FillModel<T>(string command);
        T FillModelFromCommand<T>(string command);
        List<T> FillModelList<T>(string command);
        List<string> FillStringList(string command);
        string GetConnectedServer();
        string GetNextCode(string tableName, string database);
        string GetNextCode(string tableName, string fieldName, string database);
        string GetNextCode(string tableName, string fieldName, string where, string database);
        int GetRowCount(string command);
        bool HasRows(string command);
        T RetrieveModel<T>(string where = "", string orderBy = "", string database = "");
        List<T> RetrieveModelList<T>(string where = "", string orderBy = "", string database="");
        void RollbackTransaction();
        void UpdateModel(string database);
    }
}