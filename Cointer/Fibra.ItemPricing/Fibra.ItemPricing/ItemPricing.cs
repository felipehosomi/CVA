using B1SLayer;
using Fibra.ItemPricing.Controllers;
using Fibra.ItemPricing.Infrastructure;
using Fibra.ItemPricing.Models;
using Flurl.Http;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Fibra.ItemPricing
{
    class ItemPricing
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

        internal static Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task SetItemPrice()
        {
            try
            {
                SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);

                await UserStructures.VerifyAsync();

                var hana = new Hana();
                // Obtenção de novos custos de itens a serem atualizados nas listas de preços
                await hana.ExecuteNonQueryAsync(String.Format(HanaCommands.GetNewItemCost, Database));

                // Obtenção de custos que ainda não foram atualizados nas listas de preços
                var itemCosts = await SLConnection.Request("U_CVA_OICJ").Filter("U_Status eq 'O'").OrderBy("Code").GetAsync<List<ItemCostJournal>>();
                if (itemCosts.Count == 0) return;

                foreach (var itemCost in itemCosts)
                {
                    try
                    {
                        var itemsController = new ItemsController();
                        // Atualiza a lista de preço com o novo custo do item
                        itemsController.SetPrice(itemCost.ItemCode, itemCost.PriceList, itemCost.Cost).Wait();
                        // Altera o status para fechado
                        SetPricingStatus(itemCost, PricingStatus.Close).Wait();
                        // Armazena no log a mensagem de sucesso
                        Logger.Info($"Atualização do preço da lista de preço ({itemCost.PriceList}) do item {itemCost.ItemCode} para o valor {itemCost.Cost} realizada com sucesso.");
                    }
                    catch (FlurlHttpException ex)
                    {
                        var responseString = await ex.GetResponseStringAsync();
                        // Altera o status para erro
                        SetPricingStatus(itemCost, PricingStatus.Error, responseString).Wait();
                        // Armazena no log a mensagem de erro
                        Logger.Error(ex, responseString);
                    }
                    catch (Exception ex)
                    {
                        // Altera o status para erro
                        SetPricingStatus(itemCost, PricingStatus.Error, ex.Message).Wait();
                        // Armazena no log a mensagem de erro
                        Logger.Error(ex, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Armazena no log a mensagem de erro
                Logger.Error(ex, ex.Message);
            }
            finally
            {
                await SLConnection.LogoutAsync();
            }
        }

        public static async Task SetPricingStatus(ItemCostJournal itemCostJournal, PricingStatus status, string errorMessage = "")
        {
            itemCostJournal.Status = ((char)status).ToString();
            itemCostJournal.UpdateDate = DateTime.Today.ToString("yyyy-MM-dd");
            itemCostJournal.ErrorMessage = errorMessage;

            await SLConnection.Request($"U_CVA_OICJ({itemCostJournal.Code})").PutAsync(itemCostJournal);
        }
    }
}
