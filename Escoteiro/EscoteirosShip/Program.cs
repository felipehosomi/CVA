using EscoteirosShip.Models.Magento;
using Sap.Data.Hana;
using System;
using System.Threading.Tasks;
using System.IO;
using EscoteirosShip.Models.Log;

namespace EscoteirosShip
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await createShipmentAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                Console.Read();
                throw;
            }

            try
            {
                //await updateStock();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                Console.Read();
                throw;
            }
        }

        static private async Task createShipmentAsync()
        {
            //HanaConnection conn = new HanaConnection("Server=10.100.45.35:30015;UserID=SYSTEM;Password=Acesso@2018");
            HanaConnection conn = new HanaConnection("Server=10.120.6.235:30015;UserID=SYSTEM;Password=Cva@2690");

            Orders orders = new Orders();
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

                string sql = string.Format("select    ord.\"U_CVA_EntityId\" ") +
                             string.Format("from      SBO_ESCOTEIROS_PRO.ORDR ord ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.RDR1 ordite on (ord.\"DocEntry\" = ordite.\"DocEntry\") ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.INV1 invite on (invite.\"BaseEntry\" = ordite.\"DocEntry\" and invite.\"BaseLine\" = ordite.\"LineNum\") ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.OINV inv on (inv.\"DocEntry\" = invite.\"DocEntry\") ") +
                             string.Format("where     ord.\"CANCELED\" = 'N' ") +
                             string.Format("and       ord.\"DocStatus\" = 'C' ") +
                             string.Format("and       ord.\"U_CVA_EntityId\" is not null ") +
                             string.Format("group by  ord.\"U_CVA_EntityId\" ") +
                             string.Format("order by  ord.\"U_CVA_EntityId\" desc ");

                HanaCommand cmd = new HanaCommand(sql, conn);
                HanaDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("HanaOrder:" + reader.GetString(0));
                    string shipment = OrderShip.create_SHIPMENT(token, reader.GetString(0));
                    registraLog.grava(reader.GetString(0) + "|" + shipment + "\r\n");
                    Console.WriteLine("OrderId:" + reader.GetString(0) + "|" + shipment);
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

        static private async Task updateStock()
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

                string sql = string.Format("select    pro.\"CodeBars\" ") +
                             string.Format("from      SBO_ESCOTEIROS_PRO.ORDR ord ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.RDR1 ordite on (ord.\"DocEntry\" = ordite.\"DocEntry\") ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.OITM pro on (pro.\"ItemCode\" = ordite.\"ItemCode\") ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.INV1 invite on (invite.\"BaseEntry\" = ordite.\"DocEntry\" and invite.\"BaseLine\" = ordite.\"LineNum\") ") +
                             string.Format("          inner join SBO_ESCOTEIROS_PRO.OINV inv on (inv.\"DocEntry\" = invite.\"DocEntry\") ") +
                             string.Format("where     ord.\"CANCELED\" = 'N' ") +
                             string.Format("and       ord.\"DocStatus\" = 'C' ") +
                             string.Format("and       ord.\"U_CVA_EntityId\" is not null ") +
                             string.Format("and       ord.\"DocEntry\" in (select    auxord.\"DocEntry\" ") +
                             string.Format("                             from      SBO_ESCOTEIROS_PRO.ORDR auxord ") +
                             string.Format("                                       inner join SBO_ESCOTEIROS_PRO.RDR1 auxordite on (auxord.\"DocEntry\" = auxordite.\"DocEntry\") ") +
                             string.Format("                                       inner join SBO_ESCOTEIROS_PRO.OITM auxpro on (auxpro.\"ItemCode\" = auxordite.\"ItemCode\") ") +
                             string.Format("                                       inner join SBO_ESCOTEIROS_PRO.INV1 auxinvite on (auxinvite.\"BaseEntry\" = auxordite.\"DocEntry\" and auxinvite.\"BaseLine\" = auxordite.\"LineNum\") ") +
                             string.Format("                                       inner join SBO_ESCOTEIROS_PRO.OINV auxinv on (auxinv.\"DocEntry\" = auxinvite.\"DocEntry\") ") +
                             string.Format("                             where     auxord.\"CANCELED\" = 'N' ") +
                             string.Format("                             and       auxpro.\"CodeBars\" = '7908231106955' ") +
                             string.Format("                             and       auxord.\"DocStatus\" = 'C' ") +
                             string.Format("                             and       auxord.\"U_CVA_EntityId\" is not null ") +
                             string.Format("                             group by  auxord.\"DocEntry\") ") +
                             string.Format("group by  pro.\"CodeBars\" ") +
                             string.Format("order by  pro.\"CodeBars\" ");

                HanaCommand cmd = new HanaCommand(sql, conn);
                HanaDataReader reader = cmd.ExecuteReader();
                string retorno = "";
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                    retorno = StockItems.update_STOCK(token, reader.GetString(0));
                    registraLog.grava(retorno + "\r\n");
                    Console.WriteLine(retorno + "\r\n");
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
