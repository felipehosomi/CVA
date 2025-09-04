using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;
using SAPbobsCOM;

namespace CVA.View.Apetit.Helpers
{
    public class Filters
    {
        public static void Add(string containerUid, BoEventTypes eventType)
        {
            int pos;
            if (!Exists(eventType, out pos))
            {
                EventFilter oFilter = B1Connection.Instance.Filters.Add(eventType);
                oFilter.AddEx(containerUid);
                B1Connection.Instance.Application.SetFilter(B1Connection.Instance.Filters);
            }
            else
            {
                B1Connection.Instance.Filters.Item(pos).AddEx(containerUid);
                B1Connection.Instance.Application.SetFilter(B1Connection.Instance.Filters);
            }
        }

        public static bool Exists(BoEventTypes type, out int position)
        {
            var ret = false;
            position = -1;

            try
            {
                for (var i = 0; i < B1Connection.Instance.Filters.Count; i++)
                {
                    var f = B1Connection.Instance.Filters.Item(i);
                    if (type.Equals(f.EventType))
                    {
                        ret = true;
                        position = i;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                ret = false;
            }

            return ret;
        }
    }

    public static class Extentions
    {
        public static string Aspas(this string value)
        {
            return $"\"{value}\"";
        }

        public static string PreppendSymbol(this string value, string symbol = "@")
        {
            return symbol + value;
        }

        public static string GetNextCode(this Recordset rec, string tableName, string field = "Code")
        {
            rec.DoQuery($"SELECT {field.Aspas()} as {field.Aspas()} From {tableName.PreppendSymbol().Aspas()}");
            if (rec.EoF) rec.DoQuery($"SELECT MAX({field.Aspas()}) as {field.Aspas()} From {tableName.PreppendSymbol().Aspas()}");
            if (rec.EoF) throw new Exception("Erro ao buscar next code");
            rec.MoveLast();

            var ret = rec.Fields.Item(field).Value?.ToString();
            return string.IsNullOrEmpty(ret) ? "1" : (int.Parse(ret) + 1).ToString();
        }

        public static string GetNextLineId(this Recordset rec, string tableName, string code, string codeField = "Code", string fieldLine = "LineId")
        {
            rec.DoQuery($"SELECT MAX({fieldLine.Aspas()}) as {fieldLine.Aspas()} From {tableName.PreppendSymbol().Aspas()} WHERE {codeField.Aspas()} = {code}");
            if (rec.EoF) throw new Exception("Erro ao buscar next line code");
            
            var ret = rec.Fields.Item(fieldLine).Value?.ToString();
            return string.IsNullOrEmpty(ret) ? "1" : (int.Parse(ret) + 1).ToString();
        }
    }
}
