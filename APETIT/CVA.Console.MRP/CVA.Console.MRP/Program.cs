using B1SLayer;
using CVA.Console.MRP.Infra;
using CVA.Console.MRP.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Console.MRP
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Hana hana = new Hana();
        private static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        private static readonly string database = ConfigurationManager.AppSettings["Database"];
        private static readonly SLConnection sl = new SLConnection(
            ConfigurationManager.AppSettings["ServiceLayerURL"],
            database,
            ConfigurationManager.AppSettings["B1User"],
            Encoding.UTF8.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["B1Password"])), 29);

        static async Task Main(string[] args)
        {
            try
            {
                logger.Info("Processo de MRP iniciado!");
                logger.Info($"Database: {database}");
                logger.Info("Informe o código do cenário a ser executado: ");
                string scenario = System.Console.ReadLine().Trim();

                if (!(await hana.ExecuteReaderAsync($@"SELECT 1 FROM ""{database}"".OMSN WHERE ""MsnCode"" = '{scenario}'")).HasRows)
                {
                    logger.Error($"Nenhum cenário encontrado com o código '{scenario}'");
                    return;
                }

                logger.Info($@"Iniciando processo de MRP do cenário ""{scenario}""");

                var reader = await hana.ExecuteReaderAsync($@"CALL ""{database}"".""CVA_RECOMENDACAO_MRP""('{scenario}')");

                if (!reader.HasRows)
                {
                    logger.Error($@"Nenhuma recomendação aberta encontrada para o cenário ""{scenario}""");
                    return;
                }

                var recommendations = new List<Recommendation>();

                while (reader.Read())
                {
                    var recommendation = new Recommendation();
                    recommendation.OrderType = reader.GetString(0);
                    recommendation.BPLid = reader.GetInt32(1);
                    recommendation.Warehouse = reader.GetString(2);
                    recommendation.DueDate = reader.GetDateTime(3);
                    recommendation.CardCode = reader.IsDBNull(4) ? null : reader.GetString(4);
                    recommendation.UomEntry = reader.GetInt32(5);
                    recommendation.UomCode = reader.GetString(6);
                    recommendation.ItemCode = reader.GetString(7);
                    recommendation.Price = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8);
                    recommendation.Quantity = reader.GetDecimal(9);
                    recommendation.ObjAbs = reader.GetInt32(10);
                    recommendations.Add(recommendation);
                }

                var recGroup = recommendations.GroupBy(x => new { x.ObjAbs, x.OrderType, x.BPLid, x.Warehouse, x.CardCode }).OrderBy(x => x.Key.OrderType);
                int docsToCreate = recGroup.Count();
                logger.Info($"Número de documentos a serem criados: {docsToCreate}");
                logger.Info("Pressione Enter para continuar ou qualquer outra tecla para cancelar");

                if (System.Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    logger.Info("Processo cancelado pelo usuário");
                    return;
                }

                logger.Info("Realizando Login na Service Layer...");
                await sl.Login();
                logger.Info("Iniciando criação dos documentos...");
                int currentDoc = 1;

                foreach (var recGroupItem in recGroup)
                {
                    try
                    {
                        string resource = null;

                        if (recGroupItem.Key.OrderType == "P")
                            resource = "PurchaseOrders";
                        else if (recGroupItem.Key.OrderType == "R")
                            resource = "PurchaseRequests";
                        else
                            throw new Exception("O tipo de documento deve ser \"Pedido de compra\" ou \"Solicitação de compra\"");

                        var doc = new MarketingDocument
                        {
                            CardCode = recGroupItem.Key.CardCode,
                            BPL_IDAssignedToInvoice = recGroupItem.Key.BPLid,
                            DocDueDate = recGroupItem.Max(x => x.DueDate),
                            RequriedDate = recGroupItem.Max(x => x.DueDate),
                            Comments = "Origem-MRP"
                        };

                        foreach (var rec in recGroupItem)
                        {
                            var docLine = new DocumentLine
                            {
                                ItemCode = rec.ItemCode,
                                ShipDate = rec.DueDate,
                                RequiredDate = rec.DueDate,
                                WarehouseCode = recGroupItem.Key.Warehouse,
                                UoMEntry = rec.UomEntry,
                                UoMCode = rec.UomCode,
                                Quantity = rec.Quantity,
                                Price = rec.Price
                            };
                            doc.DocumentLines.Add(docLine);
                        }

                        logger.Info($"({currentDoc++}/{docsToCreate}) enviando \"{resource}\": {JsonConvert.SerializeObject(doc, jsonSettings)}");
                        await sl.Request(resource).WithReturnNoContent().PostAsync(doc);
                        logger.Info("OK! Atualizando recomendações para o status de 'Processado'");
                        await SetRecommendationsAsProcessedAsync(recGroupItem.Key.OrderType, recGroupItem.Key.CardCode, recGroupItem.Key.Warehouse, recGroupItem.Key.ObjAbs);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, $"Erro ao processar MRP - documento: {recGroupItem.Key.OrderType}, fornecedor: {recGroupItem.Key.CardCode}, filial: {recGroupItem.Key.BPLid}, depósito: {recGroupItem.Key.Warehouse}{Environment.NewLine}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Erro durante a execução:{Environment.NewLine}");
            }
            finally
            {
                await sl.Logout();
                logger.Info($"Processo finalizado. Fechando em 10 segundos...{Environment.NewLine}");
                System.Threading.Thread.Sleep(10000);
            }
        }

        private static async Task SetRecommendationsAsProcessedAsync(string orderType, string cardCode, string warehouse, int objAbs)
        {
            string updateQuery = $@"UPDATE ""{database}"".ORCM
                                            SET ""Status"" = 'P'
                                     WHERE ORCM.""Status"" = 'O'
                                       AND ORCM.""OrderType"" = '{orderType}'
                                       AND (ORCM.""CardCode"" = '{cardCode}' OR ORCM.""CardCode"" IS NULL)
                                       AND ORCM.""Warehouse"" = '{warehouse}'
                                       AND ORCM.""ObjAbs"" = {objAbs}";

            await hana.ExecuteNonQueryAsync(updateQuery);
        }
    }
}