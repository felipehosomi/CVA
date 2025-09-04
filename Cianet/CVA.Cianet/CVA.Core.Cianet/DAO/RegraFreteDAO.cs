using CVA.AddOn.Common;
using CVA.Core.Cianet.VIEW;
using SAPbobsCOM;
using System;

namespace CVA.Core.Cianet.DAO
{
    public class RegraFreteDAO
    {
        public double CalculaFrete(string cliente, string produto, string quantidade, out string transpCode)
        {
            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = string.Format(Resources.Query.Get_RegraFrete, cliente, produto, quantidade);
            rst.DoQuery(sql);

            transpCode = "";

            try
            {
                var tsp = rst.Fields.Item(2).Value.ToString();
                if (!string.IsNullOrEmpty(tsp)) transpCode = tsp;
            }
            catch { }

            return Convert.ToDouble(rst.Fields.Item(0).Value);
        }

        public int Get_IdDespesa()
        {
            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = string.Format(Resources.Query.Get_IdDespesa);
            rst.DoQuery(sql);

            return Convert.ToInt32(rst.Fields.Item(0).Value);
        }

        public bool Check_DespesaFrete(string tipo, int idDespesa, int numDoc)
        {
            if (tipo == "P")
            {
                Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sql = string.Format(Resources.Query.Check_DespesaFrete, tipo, idDespesa, numDoc);
                rst.DoQuery(sql);

                return Convert.ToBoolean(rst.Fields.Item(0).Value);
            }
            if (tipo == "C")
            {
                Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sql = string.Format(Resources.Query.Check_DespesaFrete, tipo, idDespesa, numDoc);
                rst.DoQuery(sql);

                return Convert.ToBoolean(rst.Fields.Item(0).Value);
            }
            else
                return false;
        }

        public int Check_UserPermission(string user)
        {
            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = string.Format(Resources.Query.Check_UserPermission, user);
            rst.DoQuery(sql);

            return Convert.ToInt32(rst.Fields.Item(0).Value);
        }

        public string Check_TipoEnvio(string trnspCode)
        {
            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = string.Format(Resources.Query.Check_TipoEnvio, Convert.ToInt32(trnspCode));
            rst.DoQuery(sql);

            return rst.Fields.Item(0).Value.ToString();
        }

        public void Insert_DespesaFrete(string tipo, double totalFrete, int idDespesa, int numDoc)
        {
            var log = new Log();
            if (tipo == "P")
            {
                //log.WriteLog($@"inserindo 'FRETE' no pedido");
                var doc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oOrders);
                //log.WriteLog($@"vai buscar pelo documento = {numDoc}");
                doc.GetByKey(numDoc);
                //log.WriteLog($@"Achou isso > {doc}");
                //log.WriteLog($@"doc.Expeneses.Count = {doc.Expenses.Count}");
                if (doc.Expenses.Count > 1)
                {

                    doc.Expenses.Add();
                }
                doc.Expenses.SetCurrentLine(doc.Expenses.Count - 1);
                doc.Expenses.ExpenseCode = idDespesa;
                doc.Expenses.LineTotal = totalFrete;
                doc.Expenses.DistributionMethod = BoAdEpnsDistribMethods.aedm_RowTotal;

                if (doc.Update() != 0)
                {
                    //log.WriteLog($@"ERRO AO ATUALIZAR O VALOR DO FRETE:{ SBOApp.Company.GetLastErrorDescription()}");
                    throw new Exception(SBOApp.Company.GetLastErrorDescription());
                }
                //log.WriteLog($@"Deu boa");
            }
            if (tipo == "C")
            {
                var doc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oQuotations);
                doc.GetByKey(numDoc);

                if (doc.Expenses.Count > 1)
                    doc.Expenses.Add();
                doc.Expenses.SetCurrentLine(doc.Expenses.Count - 1);
                doc.Expenses.ExpenseCode = idDespesa;
                doc.Expenses.LineTotal = totalFrete;
                doc.Expenses.DistributionMethod = BoAdEpnsDistribMethods.aedm_RowTotal;

                if (doc.Update() != 0)
                    throw new Exception(SBOApp.Company.GetLastErrorDescription());
            }
        }

        public void Update_ValorFrete(string tipo, double totalFrete, int idDespesa, int numDoc)
        {
            if (tipo == "P")
            {
                var doc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oOrders);
                doc.GetByKey(numDoc);

                for (int i = 0; i < doc.Expenses.Count; i++)
                {
                    doc.Expenses.SetCurrentLine(i);

                    if (doc.Expenses.ExpenseCode == idDespesa)
                        doc.Expenses.LineTotal = totalFrete;
                }

                if (doc.Update() != 0)
                    throw new Exception(SBOApp.Company.GetLastErrorDescription());
            }
            if (tipo == "C")
            {
                var doc = (Documents)SBOApp.Company.GetBusinessObject(BoObjectTypes.oQuotations);
                doc.GetByKey(numDoc);

                for (int i = 0; i < doc.Expenses.Count; i++)
                {
                    doc.Expenses.SetCurrentLine(i);

                    if (doc.Expenses.ExpenseCode == idDespesa)
                        doc.Expenses.LineTotal = totalFrete;
                }

                if (doc.Update() != 0)
                    throw new Exception(SBOApp.Company.GetLastErrorDescription());
            }
        }
    }
}
