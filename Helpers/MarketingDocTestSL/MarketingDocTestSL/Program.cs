using B1SLayer;
using Newtonsoft.Json;
using NLog;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace MarketingDocTestSL
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly SLConnection connection = new SLConnection(
            ConfigurationManager.AppSettings["ServiceLayerURL"],
            ConfigurationManager.AppSettings["Database"],
            ConfigurationManager.AppSettings["B1User"],
            ConfigurationManager.AppSettings["B1Password"], 29);

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            try
            {
                var stopWatch = new Stopwatch();
                string orderJson = File.ReadAllText("sample_order.txt");
                var orderObject = JsonConvert.DeserializeObject<OrderModel>(orderJson);
                logger.Info($"Order with {orderObject.DocumentLines.Length} lines loaded from text file.");
                logger.Info("Continue with test? Press any key to continue...");
                Console.ReadLine();

                logger.Info("Logging in Service Layer...");
                await connection.LoginAsync();

                logger.Info("Starting timer. Sending new Order (POST)...");

                stopWatch.Start();
                var createdOrder = await connection.Request("Orders").PostAsync<OrderModel>(orderObject);
                stopWatch.Stop();

                logger.Info($"Order with DocNum {createdOrder.DocNum} and DocEntry {createdOrder.DocEntry} created. Elapsed time: {stopWatch.Elapsed}");
                stopWatch.Reset();
                logger.Info("Restarting timer. Updating the Order (PATCH)...");

                stopWatch.Start();
                await connection.Request("Orders", createdOrder.DocEntry).PatchAsync(new { });
                stopWatch.Stop();

                logger.Info($"Order updated. Elapsed time: {stopWatch.Elapsed}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error ocurred.");
            }
            finally
            {
                logger.Info("Logging out from Service Layer...");
                await connection.LogoutAsync();
                logger.Info("Test finished. Press any key to close.");
                Console.ReadKey();
            }
        }
    }
}