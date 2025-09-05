using CVA.Escoteiro.Magento.DAO.Resources;
using CVA.Escoteiro.Magento.DAO.Util;
using Escoteiro.Magento.Models;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.BLL.BLL
{
    public class CVA_MAGENTO_STOCK_BLL : BaseBLL
    {
        public string DataBase;

        public CVA_MAGENTO_STOCK_BLL()
        {
            DataBase = ConfigurationManager.AppSettings["Database"];
        }

        public List<CVA_MAGENTO_STOCK_Model> GetOutdatedStockQuantity()
        {
            var stockList = new List<CVA_MAGENTO_STOCK_Model>();
            var serviceLayer = new ServiceLayerUtil();
            var sql = String.Format(HanaCommands.ItemStockQuantity_Get, DataBase);

            using (HanaDataReader dr = DAO.ExecuteReader(sql))
            {
                if (dr.HasRows)
                {
                    var dt = new DataTable();
                    dt.Load(dr);

                    foreach (DataRow row in dt.Rows)
                    {
                        stockList.Add(new CVA_MAGENTO_STOCK_Model
                        {
                            U_BarCode = row["BcdCode"].ToString(),
                            U_ItemCode = row["ItemCode"].ToString(),
                            U_ItemName = row["ItemName"].ToString(),
                            U_WhsQty = double.Parse(row["WhsQty"].ToString()),
                            U_LastSyncDt = DateTime.Now.ToString("yyyyMMdd"),
                            U_LastSyncHr = DateTime.Now.ToString("HHmm")
                        });
                    }
                }
            }

            return stockList;
        }

        public async Task<string> SaveUpdatedStockQuantity(CVA_MAGENTO_STOCK_Model stockData)
        {
            var t = Newtonsoft.Json.JsonConvert.SerializeObject(stockData);


            var returnList = new List<string>();
            var serviceLayer = new ServiceLayerUtil();
            var retorno = string.Empty;
            var sql = String.Format(HanaCommands.ItemStockQuantity_GetCode, DataBase, stockData.U_BarCode, stockData.U_ItemCode);
            var code = DAO.ExecuteScalar(sql);

            await serviceLayer.Login(DataBase);

            if (code == null)
            {
                stockData.Code = DAO.ExecuteScalar(String.Format(HanaCommands.ItemStockQuantity_GetNextCode, DataBase)).ToString();
                stockData.Name = stockData.Code;
                returnList = await serviceLayer.PostAsyncReturnList("U_CVA_STOCK_MAGENTO", stockData);

                while (returnList[1].Contains("-2035"))
                {
                    DAO.ExecuteScalar(String.Format(@"select ""{0}"".""@U_CVA_STOCK_MAGENTO"".NEXTVAL from dummy", DataBase));
                    returnList = await serviceLayer.PostAsyncReturnList("U_CVA_STOCK_MAGENTO", stockData);
                }
            }
            else
            {
                returnList = await serviceLayer.PatchAsyncReturnList("U_CVA_STOCK_MAGENTO", code, stockData);
            }

            if (returnList[0] == "NOK")
            {
                throw new Exception("Erro ao gravar dados na tabela CVA_STOCK_MAGENTO: " + returnList[1]);
                //retorno = "Erro ao gravar dados na tabela CVA_STOCK_MAGENTO: " + returnList[1];
            }

            return retorno;
        }
    }
}
