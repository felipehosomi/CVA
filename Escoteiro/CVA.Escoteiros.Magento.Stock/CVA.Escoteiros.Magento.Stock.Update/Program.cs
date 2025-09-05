using CVA.Escoteiros.Magento.Stock.Update.Models.Magento;
using Sap.Data.Hana;
using System;
using System.Configuration;
using System.IO;

namespace CVA.Escoteiros.Magento.Stock.Update
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

        static void Main(string[] args)
        {
            HanaConnection conn = new HanaConnection("Server=" + Server + ";UserID=" + DBUser + ";Password=" + DBPassword);
            Token token = new Token();
            StreamWriter outputFile = new StreamWriter(@"C:\SUPORTE\CVA Escoteiro Magento StockManager Service New\Log\" + DateTime.Now.ToString("yyyyMMddHHmm") + "log.txt", false);
            try
            {
                token.apiAddressUri = Api;
                token.username = User;
                token.password = Password;
                Token.create_CN(token);
                outputFile.WriteLine("Token: " + token.bearerTolken);
                conn.Open();
                int qtd = 0;
                do
                {
                    outputFile.WriteLine("Hana: " + conn.State.ToString());
                    string SCHEMA = ConfigurationManager.AppSettings["Database"];
                    string sql = string.Format("SELECT oit.\"OnHand\",oit.\"ItemCode\", ") +
                                 string.Format("        coalesce((select sum(t1.\"Quantity\") ") +
                                 string.Format("            from " + SCHEMA + ".rdr1 t1") +
                                 string.Format("            join " + SCHEMA + ".ordr t2 on t2.\"DocEntry\" = t1.\"DocEntry\" ") +
                                 string.Format("            where  t1.\"ItemCode\" = oit.\"ItemCode\" and ") +
                                 string.Format("            t1.\"LineStatus\" = 'O' and t2.\"U_OrigemPedido\" in ('2', '3') and ") +
                                 string.Format("            t1.\"WhsCode\" = oit.\"WhsCode\" and ") +
                                 string.Format("            t2.\"CANCELED\" = 'N'),0) as \"QuantityBlock\", ") +
                                 string.Format("            oim.\"CodeBars\" as \"Sku\" ") +
                                 string.Format("FROM " + SCHEMA + ".OITW oit, " + SCHEMA + ".OITM oim ") +
                                 //string.Format("WHERE oit.\"ItemCode\" = 'EACBO002U-59' and oit.\"WhsCode\" = (select  \"U_Deposito\" from " + SCHEMA + ".\"@CVA_MAGENTO_PARAM\" limit 1)");
                                 string.Format("WHERE oim.\"CodeBars\" is not null and oit.\"ItemCode\" = oim.\"ItemCode\" and  oim.\"U_CVA_Active\" = 'Y' and oit.\"WhsCode\" = (select  \"U_Deposito\" from " + SCHEMA + ".\"@CVA_MAGENTO_PARAM\" limit 1) limit 200 offset "+qtd+"");

                    HanaCommand cmd = new HanaCommand(sql, conn);

                    HanaDataReader reader = cmd.ExecuteReader();
                    if (!reader.Read())
                    {
                    
                        break;
                    }
                    while (reader.Read())
                    {   if( reader.GetString(3) == "?" || String.IsNullOrEmpty( reader.GetString(3)))
                        {
                            Console.WriteLine("ItemCode:" + reader.GetString(1) + "|Sku:" + reader.GetString(3) + "|MagentoStock:" + reader.GetString(2) + "|Retorno:" + "erro sku");
                            outputFile.WriteLine("ItemCode:" + reader.GetString(1) + "|Sku:" + reader.GetString(3) + "|MagentoStock:" + reader.GetString(2) + "|Retorno:" + "erro sku");
                            outputFile.Flush();
                            continue;
                        }
                        var qtdStock = Convert.ToDecimal(reader.GetHanaDecimal(0));
                        var qtdBlock = Convert.ToDecimal(reader.GetHanaDecimal(2));
                        var qtdMagentoStock = qtdStock - qtdBlock;
                        var is_in_stock = true;
                        if (qtdMagentoStock <= 0 || qtdStock <= 0)
                        {
                            is_in_stock = false;
                            qtdMagentoStock = 0;
                        }
                        string stock = StockItems.update_STOCK(token, reader.GetString(3), qtdMagentoStock.ToString().Replace(',', '.'), is_in_stock);
                        Console.WriteLine("ItemCode:" + reader.GetString(1) + "|Sku:" + reader.GetString(3) + "|MagentoStock:" + reader.GetString(2) + "|Retorno:" + stock);
                        outputFile.WriteLine("ItemCode:" + reader.GetString(1) + "|Sku:" + reader.GetString(3) + "|MagentoStock:" + reader.GetString(2) + "|Retorno:" + stock);
                        outputFile.Flush();
                    }
                    qtd += 200;
                    reader.Close();
                } while (true);
            }
            catch (Exception ex)
            {
                outputFile.Close();
                conn.Close();
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
