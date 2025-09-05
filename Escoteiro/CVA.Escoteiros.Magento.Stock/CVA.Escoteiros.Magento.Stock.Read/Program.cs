using CVA.Escoteiros.Magento.Stock.Read.Models.Log;
using CVA.Escoteiros.Magento.Stock.Read.Models.Magento;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Escoteiros.Magento.Stock.Read
{
    class Program
    {
        public static string Api = ConfigurationManager.AppSettings["ApiMagento"];
        public static string User = ConfigurationManager.AppSettings["ApiMagentoUser"];
        public static string Password = ConfigurationManager.AppSettings["ApiMagentoPassWord"];
        public static string Server = ConfigurationManager.AppSettings["Server"];
        public static string Database = ConfigurationManager.AppSettings["Database"];
        public static string B1User = ConfigurationManager.AppSettings["B1User"];
        public static string B1Password = ConfigurationManager.AppSettings["B1Password"];
        public static string DBUser = ConfigurationManager.AppSettings["DBUser"];
        public static string DBPassword = ConfigurationManager.AppSettings["DBPassword"];

        static async Task Main(string[] args)
        {
            HanaConnection conn = new HanaConnection("Server=" + Server + ";UserID=" + DBUser + ";Password=" + DBPassword);
            Token token = new Token();
            StreamWriter outputFile = new StreamWriter(@"C:\SUPORTE\CVA Escoteiro Magento StockManager Service New\Log\" + DateTime.Now.ToString("yyyyMMddHHmm") + "statuslog.txt", false);
            try
            {
                token.apiAddressUri = Api;
                token.username = User;
                token.password = Password;
                Token.create_CN(token);
                outputFile.WriteLine("Token: " + token.bearerTolken);
                conn.Open();
                outputFile.WriteLine("Hana: " + conn.State.ToString());

                string sql = string.Format("select    stock.\"ItemCode\" as \"stockItemCode\", ") +
                             string.Format("          coalesce(stock.\"Sku\",'0') as \"stockSku\", ") +
                             string.Format("          stock.\"WareHouse\" as \"stockWareHouse\", ") +
                             string.Format("          stock.\"OnHand\" as \"stockOnHand\", ") +
                             string.Format("          stock.\"MovOnHand\" as \"stockMovOnHand\", ") +
                             string.Format("          stock.\"MovOpenQty\" as \"stockMovOpenQty\", ") +
                             string.Format("          stock.\"OpenQtyManual\" as \"stockOpenQtyManual\", ") +
                             string.Format("          stock.\"OpenQtyMagento\" as \"stockOpenQtyMagento\", ") +
                             string.Format("          (case when (stock.\"OnHand\" - stock.\"OpenQtyManual\") > 0 then (stock.\"OnHand\" - stock.\"OpenQtyManual\") ") +
                             string.Format("                else 0 ") +
                             string.Format("           end) as \"stockMagentoStock\" ") +
                             string.Format("from      (select    whs.\"ItemCode\" as \"ItemCode\", ") +
                             string.Format("                     pro.\"CodeBars\" as \"Sku\", ") +
                             string.Format("                     par.\"U_Deposito\" as \"WareHouse\", ") +
                             string.Format("                     coalesce(whs.\"OnHand\",0) as \"OnHand\", ") +
                             string.Format("                     coalesce((select    sum(mov.\"InQty\" - mov.\"OutQty\") ") +
                             string.Format("                               from      " + Database + ".\"B1_OinmWithBinTransfer\" mov ") +
                             string.Format("                               where     mov.\"ItemCode\" = whs.\"ItemCode\" ") +
                             string.Format("                               and       mov.\"Warehouse\" = par.\"U_Deposito\" ") +
                             string.Format("                               and       mov.\"CreateDate\" <= (SELECT add_days(current_date,-1) from dummy)),0) as \"MovOnHand\", ") +
                             string.Format("                     coalesce((select    sum(ordite.\"Quantity\") ") +
                             string.Format("                               from      " + Database + ".\"ORDR\" ord ") +
                             string.Format("                                         inner join " + Database + ".\"RDR1\" ordite on (ord.\"DocEntry\" = ordite.\"DocEntry\" and ordite.\"ItemCode\" = whs.\"ItemCode\" and ordite.\"WhsCode\" = par.\"U_Deposito\") ") +
                             string.Format("                               where     (ord.\"CreateDate\" >= (SELECT add_days(current_date,0) from dummy) or ord.\"UpdateDate\" >= (SELECT add_days(current_date,0) from dummy))),0) as \"MovOpenQty\", ") +
                             string.Format("                     coalesce((select    sum(ordite.\"Quantity\") ") +
                             string.Format("                               from      " + Database + ".\"ORDR\" ord ") +
                             string.Format("                                         inner join " + Database + ".\"RDR1\" ordite on (ord.\"DocEntry\" = ordite.\"DocEntry\" and ordite.\"ItemCode\" = whs.\"ItemCode\" and ordite.\"WhsCode\" = par.\"U_Deposito\") ") +
                             string.Format("                               where     ord.\"DocStatus\" = 'O' ") +
                             string.Format("                               and       ord.\"CANCELED\" = 'N' ") +
                             string.Format("                               and       ordite.\"LineStatus\" = 'O' ") +
                             string.Format("                               and       ord.\"U_OrigemPedido\" in ('2', '3')),0) as \"OpenQtyManual\", ") +
                             string.Format("                     coalesce((select    sum(ordite.\"Quantity\") ") +
                             string.Format("                               from      " + Database + ".\"ORDR\" ord ") +
                             string.Format("                                         inner join " + Database + ".\"RDR1\" ordite on (ord.\"DocEntry\" = ordite.\"DocEntry\" and ordite.\"ItemCode\" = whs.\"ItemCode\" and ordite.\"WhsCode\" = par.\"U_Deposito\") ") +
                             string.Format("                               where     ord.\"DocStatus\" = 'O' ") +
                             string.Format("                               and       ord.\"CANCELED\" = 'N' ") +
                             string.Format("                               and       ordite.\"LineStatus\" = 'O' ") +
                             string.Format("                               and       ord.\"U_OrigemPedido\" in ('1')),0) as \"OpenQtyMagento\" ") +
                             string.Format("           from      " + Database + ".\"OITW\" whs ") +
                             string.Format("                     inner join " + Database + ".\"OITM\" pro on (whs.\"ItemCode\" = pro.\"ItemCode\") ") +
                             string.Format("                     inner join " + Database + ".\"@CVA_MAGENTO_PARAM\" par on (par.\"U_Deposito\" = whs.\"WhsCode\")) stock ") +
                             string.Format("where     stock.\"Sku\" is not null ") +
                             string.Format("order by  stock.\"ItemCode\" ");

                HanaCommand cmd = new HanaCommand(sql, conn);
                HanaDataReader reader = cmd.ExecuteReader();
                SendMail sendMail = new SendMail();
                sendMail.fieldReport = new List<SendMail.FieldsReport>();
                outputFile.WriteLine("ItemCode|Sku|WareHouse|OnHand|MovOnHand|MovOpenQty|OpenQtyManual|OpenQtyMagento|SAPMagentoStock|MagentoStock|MagentoSalableQuantity|MagentoMinQuantity");
                while (reader.Read())
                {
                    var stockStatuses = await StockStatuses.read_STOCK(token, reader.GetString(1));
                    var stockItems = await StockItems.read_STOCK(token, reader.GetString(1));
                    string log = string.Format(reader.GetString(0) + "|" + reader.GetString(1) + "|" + reader.GetString(2) + "|" + (int?)reader.GetDecimal(3) + "|" + (int?)reader.GetDecimal(4) + "|" + (int?)reader.GetDecimal(5) + "|" + (int?)reader.GetDecimal(6) + "|" + (int?)reader.GetDecimal(7) + "|" + (int?)reader.GetDecimal(8) + "|" + stockStatuses.stock_item.qty + "|" + stockStatuses.qty + "|" + stockStatuses.stock_item.min_qty);
                    stockStatuses.stock_item.qty = stockItems.qty;                    
                    bool quantity = false;
                    bool salablequantity = false;
                    if ((int?)reader.GetDecimal(8) != stockStatuses.stock_item.qty)
                    {
                        log += string.Format("|Quantidade divergente");
                        quantity = true;
                    }
                    if (((int?)reader.GetDecimal(8) - (int?)reader.GetDecimal(7) - stockStatuses.stock_item.min_qty) != stockStatuses.qty)
                    {
                        log += string.Format("|Saldo vendável divergente");
                        salablequantity = true;
                    }
                    if (quantity || salablequantity)
                    {
                        sendMail.fieldReport.Add(new SendMail.FieldsReport
                        {
                            ItemCode = reader.GetString(0),
                            Sku = reader.GetString(1),
                            Saldo = (int?)reader.GetDecimal(8),
                            SaldoDisponivel = ((int?)reader.GetDecimal(8) - (int?)reader.GetDecimal(7) - stockStatuses.stock_item.min_qty),
                            MagentoQuantidade = stockStatuses.stock_item.qty,
                            MagentoVendavel = stockStatuses.qty,
                            Descricao = "Quantidade ou saldo vendável divergente"
                        });
                        outputFile.WriteLine(log);
                        outputFile.Flush();
                    }
                }
                outputFile.WriteLine("Envio email:" + SendMail.Mail_SEND(sendMail));
                reader.Close();
            }
            catch (Exception ex)
            {
                outputFile.WriteLine("Erro:" + ex.Message.ToString());
                throw;
            }
            finally
            {
                outputFile.WriteLine("Finish");
                outputFile.Close();
                conn.Close();
            }
        }
    }
}
