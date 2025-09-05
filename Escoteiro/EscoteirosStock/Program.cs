using EscoteirosShip.Models.Log;
using EscoteirosShip.Models.Magento;
using EscoteirosStock.models.Magento;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscoteirosStock
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //HanaConnection conn = new HanaConnection("Server=10.100.45.35:30015;UserID=SYSTEM;Password=Acesso@2018");
            HanaConnection conn = new HanaConnection("Server=10.120.6.235:30015;UserID=SYSTEM;Password=Cva@2690");

            RegistraLog registraLog = new RegistraLog();
            try
            {
                Token token = new Token();
                token.username = "sap";
                token.password = "3itkBEwFbPyM1";
                Token.create_CN(token);
                Console.WriteLine("Token: " + token.bearerTolken);
                conn.Open();
                Console.WriteLine("Hana: " + conn.State.ToString());

                string sql = string.Format("select    pro.\"ItemCode\", ") +
                             string.Format("          pro.\"ItemName\", ") +
                             string.Format("          pro.\"CodeBars\", ") +
                             string.Format("          est.\"OnHand\", ") +
                             string.Format("          est.\"IsCommited\", ") +
                             string.Format("          est.\"OnOrder\", ") +
                             string.Format("          est.\"Consig\", ") +
                             string.Format("          est.\"Counted\", ") +
                             string.Format("          est.\"MinStock\", ") +
                             string.Format("          est.\"MaxStock\", ") +
                             string.Format("          est.\"AvgPrice\" ") +
                             string.Format("from      SBO_ESCOTEIROS_PRO.OITM pro ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.OITW est on (pro.\"ItemCode\" = est.\"ItemCode\") ") +
                             string.Format("where     est.\"WhsCode\" = '02' ") +
                             string.Format("and       pro.\"CodeBars\" is not null ") +
                             string.Format("order by  pro.\"CodeBars\" ");

                HanaCommand cmd = new HanaCommand(sql, conn);
                HanaDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var resourceCode = await stockStatuses.read_STOCK(token, reader.GetString(2));
                    Console.WriteLine("|ItemCode:" + reader.GetString(0) +
                                      "|ItemName:" + reader.GetString(1) +
                                      "|BarCode:" + reader.GetString(2) +
                                      "|OnHand:" + reader.GetString(3) +
                                      "|ProductId:" + resourceCode.product_id +
                                      "|SalableQuantity:" + resourceCode.qty +
                                      "|StockStatus:" + resourceCode.stock_status +
                                      "|Item_id:" + resourceCode.stock_item.item_id +
                                      "|Product_id:" + resourceCode.stock_item.product_id +
                                      "|stock_id:" + resourceCode.stock_item.stock_id +
                                      "|Quantity:" + resourceCode.stock_item.qty +
                                      "|Min Quantity:" + resourceCode.stock_item.min_qty);
                    registraLog.grava(reader.GetString(0) + "|" +
                                      reader.GetString(1) + "|" +
                                      reader.GetString(2) + "|" +
                                      reader.GetString(3) + "|" +
                                      resourceCode.product_id + "|" +
                                      resourceCode.qty + "|" +
                                      resourceCode.stock_status + "|" +
                                      resourceCode.stock_item.item_id + "|" +
                                      resourceCode.stock_item.product_id + "|" +
                                      resourceCode.stock_item.stock_id + "|" +
                                      resourceCode.stock_item.qty + "|" +
                                      resourceCode.stock_item.min_qty + "\n");
                }
                Console.WriteLine("FIM");
                Console.Read();
                reader.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
