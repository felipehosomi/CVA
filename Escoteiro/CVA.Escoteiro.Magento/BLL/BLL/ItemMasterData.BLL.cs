using CVA.Escoteiro.Magento.Client;
using CVA.Escoteiro.Magento.DAO.Resources;
using CVA.Escoteiro.Magento.DAO.Util;
using CVA.Escoteiro.Magento.Models.Magento;
using CVA.Escoteiro.Magento.Models.SAP;
using Escoteiro.Magento.Models.SAP;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.BLL
{
    public class ItemMasterDataBLL : BaseBLL
    {
        public string DataBase;

        public ItemMasterDataBLL()
        {
            DataBase = ConfigurationManager.AppSettings["Database"];
        }

        public List<string> GetItemsToIntegrate()
        {
            var items = new List<string>();
            var serviceLayer = new ServiceLayerUtil();
            var sql = String.Format(HanaCommands.ItemsToIntegrate_Get, DataBase);

            using (HanaDataReader dr = DAO.ExecuteReader(sql))
            {
                if (dr.HasRows)
                {
                    var dt = new DataTable();
                    dt.Load(dr);

                    foreach (DataRow row in dt.Rows)
                    {
                        items.Add(row["ItemCode"].ToString());
                    }
                }
            }

            return items;
        }

        public async Task<ProductModel> GetItemData(string itemCode)
        {
            var serviceLayer = new ServiceLayerUtil();

            await serviceLayer.Login(DataBase);

            var item = await serviceLayer.GetByIDAsync<ItemsMasterDataModel>("Items", itemCode);
            var categories = await serviceLayer.GetAsync<CVA_OICT_Model>("U_CVA_OICT", $"filter=U_ItemCode eq '{itemCode}'", "?$");

            var product = new ProductModel()
            {
                product = new Product()
                {
                    sku = item.BarCode,
                    name = item.ItemName,
                    visibility = item.U_CVA_Visibility,
                    status = item.U_CVA_Active == "Y" ? 1 : 2,
                    weight = item.SalesUnitWeight,
                    price = item.ItemPrices.Where(x => x.PriceList == 1).First().Price,
                    custom_attributes = new List<CustomAttribute>()
                            {
                                new CustomAttribute() { attribute_code = "is_recurring", value = "0" },
                                new CustomAttribute() { attribute_code = "is_blackfriday", value = 0 },
                                new CustomAttribute() { attribute_code = "venda_restrita", value = 0 },
                                new CustomAttribute() { attribute_code = "options_container", value = "container1" },
                                new CustomAttribute() { attribute_code = "tax_class_id", value = 0 },
                                new CustomAttribute() { attribute_code = "required_options", value = 0 },
                                new CustomAttribute() { attribute_code = "has_options", value = 0 },
                                new CustomAttribute() { attribute_code = "vestuario_escoteiro", value = 0 },
                                new CustomAttribute() { attribute_code = "produto_de_programa", value = 0 }
                            }
                }
            };

            if (item.SalesUnitLength != null) product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "volume_comprimento", value = Convert.ToInt32(item.SalesUnitLength) });
            if (item.SalesUnitHeight != null) product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "volume_altura", value = Convert.ToInt32(item.SalesUnitHeight) });
            if (item.SalesUnitWidth != null) product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "volume_largura", value = Convert.ToInt32(item.SalesUnitWidth) });
            if (item.User_Text != null) product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "description", value = item.User_Text });
            if (item.U_CVA_IdCor != null && !String.IsNullOrEmpty(item.U_CVA_IdCor.ToString())) product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "color", value = item.U_CVA_IdCor });
            if (item.U_CVA_IdTamanho != null && !String.IsNullOrEmpty(item.U_CVA_IdTamanho.ToString())) product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "tamanho", value = item.U_CVA_IdTamanho });
            if (item.U_CVA_ShortDescription != null) product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "short_description", value = item.U_CVA_ShortDescription });
            if (item.U_CVA_Url != null)
            {
                product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "url_path", value = item.U_CVA_Url });
                product.product.custom_attributes.Add(new CustomAttribute() { attribute_code = "url_key", value = item.U_CVA_Url });
            }

            if (categories.value.Count > 0)
            {
                product.product.extension_attributes = new ExtensionAttributes();
                product.product.extension_attributes.category_links = new List<CategoryLink>();
            }

            foreach (var category in categories.value)
            {
                product.product.extension_attributes.category_links.Add(new CategoryLink() { category_id = category.U_ID.ToString(), position = 0 });
            }

            return product;
        }

        public async Task<string> SetItemIntegratedStatus(string itemCode, string productId)
        {
            var returnList = new List<string>();
            var serviceLayer = new ServiceLayerUtil();
            var retorno = string.Empty;

            await serviceLayer.Login(DataBase);

            var item = await serviceLayer.GetByIDAsync<ItemsMasterDataModel>("Items", itemCode);
            item.U_CVA_Product_id = productId;
            item.U_CVA_Integrated = "Y";

            returnList = await serviceLayer.PatchAsyncReturnList("Items", itemCode, item);

            if (returnList[0] == "NOK")
            {
                retorno = "Erro ao alterar o campo OITM.U_CVA_Integrated para 'Y': " + returnList[1];
            }

            return retorno;
        }

        public async Task SetItemActive()
        {
            var returnList = new List<string>();
            var serviceLayer = new ServiceLayerUtil();
            var retorno = string.Empty;
            var sql = String.Format(@"select OITM.""ItemCode"", OITM.""CodeBars""
                                        from ""{0}"".OITM
                                       where OITM.""InvntItem"" = 'Y'
                                         and OITM.""SellItem"" = 'Y'
                                         and OITM.""CodeBars"" is not null", DataBase);

            await serviceLayer.Login(DataBase);

            using (HanaDataReader dr = DAO.ExecuteReader(sql))
            {
                if (dr.HasRows)
                {
                    var dt = new DataTable();
                    dt.Load(dr);

                    foreach (DataRow row in dt.Rows)
                    {
                        var magento = new ClientMagento();
                        var product = magento.GetProduct(row["CodeBars"].ToString());

                        if (product.id == 0) continue;

                        try
                        {
                            var item = await serviceLayer.GetByIDAsync<ItemsMasterDataModel>("Items", row["ItemCode"].ToString());
                            item.U_CVA_Active = product.status == 1 ? "Y" : "N";


                            returnList = await serviceLayer.PatchAsyncReturnList("Items", row["ItemCode"].ToString(), item);

                            if (returnList[0] == "NOK")
                            {
                                retorno = "Erro ao alterar o campo OITM.U_CVA_Active para 'Y': " + returnList[1];
                            }
                        }
                        catch (Exception ex)
                        {
                            var a = ex.Message;
                        }
                    }
                }
            }
        }
    }
}
