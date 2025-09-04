using CVA.Apetit.Servico.Consolidacao.DAO;
using log4net;
using SAPbobsCOM;
using SBO.Hub.DAO;
using SBO.Hub.SBOHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Apetit.Servico.Consolidacao
{
    public class Processo
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Recordset RstUpdate;

        public Processo()
        {
        }

        public void Executar()
        {
            log4net.Config.XmlConfigurator.Configure();

            string msg = "";
            DateTime startDate = DateTime.Now;

            try
            {
                Logger.Info("Processo iniciado");

                msg = Class.Acesso.LerConfiguracao();

                if (!string.IsNullOrEmpty(msg))
                    throw new Exception(msg);

                if (Class.Conexao.ConectarB1())
                {
                    Logger.Info("Conectado no SAP, iniciando integração!");
                    //GerarDocumentosSAP();
                    SetNewProductionOrder();
                }
                else
                    throw new Exception("Erro ao conectar ao B1");
                //this.SetNewProductionServiceLayer();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                Logger.Info($"Processo concluído - Tempo de processamento: {Math.Truncate((DateTime.Now - startDate).TotalMinutes)} minutos e {(DateTime.Now - startDate).Seconds} segundos");
            }

        }

        private void SetNewProductionOrder()
        {
            try
            {
                string sql = @"SELECT OWOR.""DocEntry"" 
                        FROM OWOR
                            INNER JOIN ""@CVA_PLANEJAMENTO"" OMNP
                            INNER JOIN ""@CVA_LN_PLANEJAMENTO"" MNP1 ON MNP1.""DocEntry"" = OMNP.""DocEntry""
                            INNER JOIN ""@CVA_MNP2"" MNP2 on MNP2.""DocEntry"" = MNP1.""DocEntry"" AND MNP2.""U_Day"" = MNP1.""U_Day""
                            INNER JOIN ""@CVA_TURNO"" ""TURNO"" on TURNO.""Name"" = MNP2.""U_Shift""
                            ON OMNP.""DocEntry"" = OWOR.""U_CVA_PlanCode"" AND TURNO.""Code"" = OWOR.""U_CVA_Turno"" AND OWOR.""ItemCode"" = MNP1.""U_CVA_INSUMO""
                            AND OMNP.""U_CVA_ID_SERVICO"" = OWOR.""U_CVA_SERVICO"" AND DAYOFMONTH (OWOR.""StartDate"") = MNP2.""U_Day""
                        WHERE OWOR.""Status"" = 'P'";


                var dataTable = Class.Conexao.ExecuteSqlDataTable(sql);
                Logger.Info("OPs pendentes de liberação: " + dataTable.Rows.Count);
               
                foreach (System.Data.DataRow row in dataTable.Rows)
                {
                    var productionOrders = (SAPbobsCOM.ProductionOrders)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                    productionOrders.GetByKey(Convert.ToInt32(row["DocEntry"].ToString()));

                    productionOrders.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposReleased;

                    if (productionOrders.Update() != 0)
                    {
                        Logger.Error(string.Concat($"ERRO AO LIBERAR OP - {row["DocEntry"].ToString()}/{row["LineId"].ToString()} - DIA {row["U_Day"].ToString()} - TURNO {row["U_Shift"].ToString()} - PRATO {row["U_CVA_TIPO_PRATO_DES"].ToString()} - {Class.Conexao.oCompany.GetLastErrorDescription()}"));
                    }
                    else
                    {
                        Logger.Info(string.Concat($"OP liberada com sucesso - {row["DocEntry"].ToString()}/{row["LineId"].ToString()} - DIA {row["U_Day"].ToString()} - TURNO {row["U_Shift"].ToString()} - PRATO {row["U_CVA_TIPO_PRATO_DES"].ToString()}"));
                    }

                    Marshal.ReleaseComObject(productionOrders);
                    productionOrders = null;
                }

                Logger.Info("Buscando dados...");

                sql = Hana.Menu_GetReleased;
                dataTable = Class.Conexao.ExecuteSqlDataTable(sql);
                Logger.Info("Registros encontrados: " + dataTable.Rows.Count);
                if (dataTable.Rows.Count > 0)
                {
                    RstUpdate = (Recordset)Class.Conexao.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                }

                while (dataTable.Rows.Count > 0)
                {
                    this.GeneratePOs(dataTable);

                    dataTable = Class.Conexao.ExecuteSqlDataTable(sql);
                    Logger.Info("Registros encontrados: " + dataTable.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ERRO GERAL: " + ex.Message);
            }

            //    foreach (var planner in plannerStatus)
            //    {
            //        if (planner.Value == "NOK") continue;

            //        ClosePlanner(planner.Key);
            //    }
        }

        private void GeneratePOs(System.Data.DataTable dataTable)
        {
            string previousDocEntry = "";
            string previousLineId = "";
            if (dataTable.Rows.Count > 0)
            {
                previousDocEntry = dataTable.Rows[0]["DocEntry"].ToString();
                previousLineId = dataTable.Rows[0]["LineId"].ToString();
            }

            int groupCount = 0;
            string error = "";

            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                DateTime startDate = DateTime.Now;
                if (previousDocEntry != row["DocEntry"].ToString())
                {
                    UpdatePlanning(previousDocEntry);
                    previousDocEntry = row["DocEntry"].ToString();
                }
                if (previousDocEntry != row["DocEntry"].ToString() || previousLineId != row["LineId"].ToString())
                {
                    previousLineId = row["LineId"].ToString();
                    groupCount = 0;
                    error = "";
                }
                groupCount++;

                var productionOrders = (SAPbobsCOM.ProductionOrders)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                var productTrees = (SAPbobsCOM.ProductTrees)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees);

                productTrees.GetByKey(row["ItemCode"].ToString());
                productionOrders.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotStandard;
                productionOrders.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposPlanned;
                productionOrders.ItemNo = row["ItemCode"].ToString();
                productionOrders.PlannedQuantity = Convert.ToDouble(row["Quantity"].ToString());
                productionOrders.Warehouse = row["DflWhs"].ToString();
                productionOrders.PostingDate = DateTime.Today;
                productionOrders.StartDate = Convert.ToDateTime(row["Date"].ToString());
                productionOrders.DueDate = Convert.ToDateTime(row["Date"].ToString());
                productionOrders.UserFields.Fields.Item("U_CVA_Turno").Value = row["U_Shift"].ToString();
                productionOrders.UserFields.Fields.Item("U_CVA_SERVICO").Value = row["IdServico"].ToString();
                productionOrders.UserFields.Fields.Item("U_CVA_CONTRATO").Value = row["U_AbsID"].ToString();
                productionOrders.UserFields.Fields.Item("U_CVA_PlanCode").Value = row["DocNum"].ToString();
                productionOrders.UserFields.Fields.Item("U_CVA_LineId").Value = row["LineId"].ToString();

                for (int i = 0; i < productTrees.Items.Count; i++)
                {
                    productTrees.Items.SetCurrentLine(i);

                    if (i > 0)
                    {
                        productionOrders.Lines.Add();
                    }

                    productionOrders.Lines.ItemNo = productTrees.Items.ItemCode;
                    productionOrders.Lines.ItemType = SAPbobsCOM.ProductionItemType.pit_Item;
                    productionOrders.Lines.ProductionOrderIssueType = SAPbobsCOM.BoIssueMethod.im_Manual;
                    productionOrders.Lines.BaseQuantity = productTrees.Items.Quantity / productTrees.PlanAvgProdSize;
                    productionOrders.Lines.PlannedQuantity = productionOrders.PlannedQuantity * productionOrders.Lines.BaseQuantity;
                    productionOrders.Lines.Warehouse = row["DflWhs"].ToString();
                }

                string newObjectKey = "0";
                if (productionOrders.Add() == 0)
                {
                    newObjectKey = Class.Conexao.oCompany.GetNewObjectKey();
                    Logger.Info(string.Concat($"SUCESSO - {row["DocEntry"].ToString()}/{row["LineId"].ToString()} - DIA {row["U_Day"].ToString()} - TURNO {row["U_Shift"].ToString()} - PRATO {row["U_CVA_TIPO_PRATO_DES"].ToString()} - ITEM {row["ItemCode"].ToString()} - {newObjectKey}"));

                    productionOrders.GetByKey(Convert.ToInt32(newObjectKey));
                    productionOrders.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposReleased;

                    if (productionOrders.Update() != 0)
                    {
                        Marshal.ReleaseComObject(productionOrders);
                        productionOrders = null;

                        productionOrders = (SAPbobsCOM.ProductionOrders)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                        productionOrders.GetByKey(Convert.ToInt32(newObjectKey));
                        productionOrders.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposReleased;

                        if (productionOrders.Update() != 0)
                        {
                            //error = Class.Conexao.oCompany.GetLastErrorDescription();
                            Logger.Error(string.Concat($"ERRO AO LIBERAR OP - {row["DocEntry"].ToString()}/{row["LineId"].ToString()} - DIA {row["U_Day"].ToString()} - TURNO {row["U_Shift"].ToString()} - PRATO {row["U_CVA_TIPO_PRATO_DES"].ToString()} - ITEM {row["ItemCode"].ToString()} - {Class.Conexao.oCompany.GetLastErrorDescription()}"));
                        }
                    }
                }
                else
                {
                    error += $"Turno {row["U_Shift"].ToString()} - {Class.Conexao.oCompany.GetLastErrorDescription()}";
                    Logger.Error(string.Concat($"ERRO - {row["DocEntry"].ToString()}/{row["LineId"].ToString()} - DIA {row["U_Day"].ToString()} - TURNO {row["U_Shift"].ToString()} - PRATO {row["U_CVA_TIPO_PRATO_DES"].ToString()} - ITEM {row["ItemCode"].ToString()} - {error}"));
                }

                if (groupCount == Convert.ToInt32(row["GroupCount"]))
                {
                    this.UpdateLine(error, newObjectKey, row["DocEntry"].ToString(), row["LineId"].ToString());
                }

                Logger.Info($"Tempo de processamento: {Math.Truncate((DateTime.Now - startDate).TotalMilliseconds)} milisegundos");

                Marshal.ReleaseComObject(productionOrders);
                productionOrders = null;

                Marshal.ReleaseComObject(productTrees);
                productTrees = null;
            }

            if (previousDocEntry != "")
            {
                UpdatePlanning(previousDocEntry);
            }
        }

        private void UpdatePlanning(string docEntry)
        {
            RstUpdate.DoQuery($@"update ""@CVA_PLANEJAMENTO""
                                set ""U_Status"" = case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {docEntry}) > 0 then 'R' 
   					                            else case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {docEntry}) = 0 then 'L' end end
                                where ""DocEntry"" = {docEntry};");
        }

        private void UpdateLine(string error, string newObjectKey, string docEntry, string LineId)
        {
            string update;

            if (!String.IsNullOrEmpty(error))
            {
                Logger.Error($"ERRO: {error}");
                if (error.Length > 254)
                {
                    error = error.Substring(0, 254);
                }

                update = $@"update ""@CVA_LN_PLANEJAMENTO""
                                    set ""U_LineStatus"" = CASE WHEN '{error}' = '' THEN 'L' ELSE 'R' END, ""U_CVA_ERROR"" = '{error}'
                                    where ""DocEntry"" = {docEntry}
                                    and ""LineId"" = {LineId};";
            }
            else
            {
                update = $@"update ""@CVA_LN_PLANEJAMENTO""
                                    set ""U_LineStatus"" = CASE WHEN '{error}' = '' THEN 'L' ELSE 'R' END, ""U_CVA_ERROR"" = '{error}', ""U_CVA_OWOR"" = {newObjectKey},
                                    ""U_CVA_OWOR_ALL"" = CASE WHEN IFNULL(""U_CVA_OWOR_ALL"", '') = '' THEN '{newObjectKey}' ELSE ""U_CVA_OWOR_ALL"" || ', ' || '{newObjectKey}' END
                                    where ""DocEntry"" = {docEntry}
                                    and ""LineId"" = {LineId};";
            }

            RstUpdate.DoQuery(update);
        }


        private void SetNewProductionServiceLayer()
        {
            try
            {
                HanaDAO dao = new HanaDAO(Class.Acesso.CompanyDB, Class.Acesso.Server, Class.Acesso.DbUserName, Class.Acesso.DbPassword);
                Logger.Info("Buscando dados...");
                string sql = @"select OMNP.""DocEntry"" ""PlanEntry"", OMNP.""DocNum"" ""U_CVA_PlanCode"", OMNP.""U_AbsID"" ""U_CVA_CONTRATO"", OBPL.""DflWhs"" ""Warehouse"", 
                                                                       add_days(OMNP.""U_CVA_DATA_REF"", MNP1.""U_Day"" - 1) as ""StartDate"",
                                                                       OPLN.""Code"" as ""U_CVA_SERVICO"", TURNO.""Code"" as ""U_CVA_Turno"",
                                                                       OITM.""ItemCode"" ""ItemNo"",
                                                                       case OITM.""InvntryUom"" when 'UN' then 
                                                                            round((MNP2.""U_Quantity"" * (MNP1.""U_CVA_PERCENT"" / 100)) , 0, round_half_up)
                                                                       else 
                                                                            ((MNP2.""U_Quantity"" * (MNP1.""U_CVA_PERCENT"" / 100)))
                                                                       end as ""PlannedQuantity"", MNP1.""U_Day"" ""Day"", MNP1.U_CVA_TIPO_PRATO_DES ""TipoPrato"", MNP1.""LineId"" ""U_CVA_LineId""
                                                                  from ""@CVA_PLANEJAMENTO"" as OMNP
                                                                 inner join ""@CVA_LN_PLANEJAMENTO"" as MNP1 on MNP1.""DocEntry"" = OMNP.""DocEntry""
                                                                 inner join ""@CVA_MNP2"" as MNP2 on MNP2.""DocEntry"" = MNP1.""DocEntry""
                                                                   and MNP2.""U_Day"" = MNP1.""U_Day""
                                                                 inner join ""@CVA_TURNO"" as ""TURNO"" on TURNO.""Name"" = MNP2.""U_Shift""
                                                                 inner join ""OITM"" OITM on OITM.""ItemCode"" = MNP1.""U_CVA_INSUMO""
                                                                  left join ""OCRD"" OCRD on OCRD.""CardCode"" = OMNP.""U_CVA_ID_CLIENTE""
                                                                  left join ""OOAT"" OOAT on OOAT.""Number"" = OMNP.""U_CVA_ID_CONTRATO""
                                                                  left join ""OBPL"" OBPL on OBPL.""BPLId"" = OOAT.""U_CVA_FILIAL""  
                                                                  left join ""@CVA_SERVICO_PLAN"" as OPLN on OPLN.""Code"" = OMNP.""U_CVA_ID_SERVICO""
                                                                 where MNP1.""U_LineStatus"" = 'R'
                                                                   and MNP2.""U_Quantity"" > 0
                                                                   and MNP1.""U_CVA_PERCENT"" > 0
                                                                    AND NOT EXISTS 
																    (
																	    SELECT T4.""ItemCode"" FROM OWOR T4 WHERE OMNP.""DocEntry"" = T4.""U_CVA_PlanCode"" AND TURNO.""Code"" = T4.""U_CVA_Turno"" AND T4.""ItemCode"" = MNP1.""U_CVA_INSUMO"" AND OMNP.""U_CVA_ID_SERVICO"" = T4.""U_CVA_SERVICO"" AND DAYOFMONTH ( T4.""StartDate"") = MNP2.""U_Day""
                                                                    )
                                                                 order by OMNP.""DocEntry""";

                List<ProductionOrderModel> list = dao.FillListFromSql<ProductionOrderModel>(sql);

                string sqlItemTree = @"SELECT
                                    ITT1.""Code"" ""ItemNo"",
	                                ITT1.""Quantity"" / OITT.""PlAvgSize"" ""BaseQuantity""
                                FROM OITT

                                    INNER JOIN ITT1 ON ITT1.""Father"" = OITT.""Code""

                                    WHERE OITT.""Code"" = '{0}'
                                ORDER BY ITT1.""VisOrder""";

                var plannerStatus = new Dictionary<int, string>();

                ServiceLayer serviceLayer = new ServiceLayer();

                Logger.Info("Registros encontrados: " + list.Count);

                string previousDocEntry = "";
                if (list.Count > 0)
                {
                    previousDocEntry = list[0].PlanEntry.ToString();
                }

                foreach (var op in list)
                {
                    DateTime startDate = DateTime.Now;

                    if (previousDocEntry != op.PlanEntry.ToString())
                    {
                        dao.ExecuteNonQuery($@"update ""@CVA_PLANEJAMENTO""
                                                   set ""U_Status"" = case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {previousDocEntry}) > 0 then 'R' 
   					                                                else case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {previousDocEntry}) = 0 then 'L' end end
                                                 where ""DocEntry"" = {previousDocEntry};");

                        previousDocEntry = op.PlanEntry.ToString();
                    }
                    Logger.Info($"Iniciando criação OP - {op.U_CVA_PlanCode}/{op.U_CVA_LineId} - DIA {op.Day} - TURNO {op.U_CVA_Turno} - PRATO {op.TipoPrato} - ITEM {op.ItemNo}");

                    op.ProductionOrderType = "bopotStandard";
                    op.ProductionOrderStatus = "boposPlanned";
                    op.DueDate = op.StartDate;
                    op.ProductionOrderLines = dao.FillListFromSql<Productionorderline>(String.Format(sqlItemTree, op.ItemNo));

                    foreach (var item in op.ProductionOrderLines)
                    {
                        item.ItemType = "pit_Item";
                        item.ProductionOrderIssueType = "im_Manual";
                        item.PlannedQuantity = Math.Round(op.PlannedQuantity * item.BaseQuantity, 5);
                        item.Warehouse = op.Warehouse;
                    }

                    string error = "";
                    ProductionOrderModel newOp = op;
                    try
                    {
                        newOp = serviceLayer.PostAndGetAdded<ProductionOrderModel>("ProductionOrders", "AbsoluteEntry", op, true);
                        Logger.Info($"OP criada - DocEntry: {newOp.AbsoluteEntry}");
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            error = "Erro ao CRIAR OP: " + ex.InnerException.Message;
                        }
                        else
                        {
                            error = "Erro ao CRIAR OP: " + ex.Message;
                        }
                    }

                    if (String.IsNullOrEmpty(error))
                    {
                        try
                        {
                            newOp.ProductionOrderStatus = "boposReleased";
                            newOp.ProductionOrderLines = null;
                            serviceLayer.Patch<ProductionOrderModel>("ProductionOrders", newOp.AbsoluteEntry, newOp);
                        }
                        catch
                        {
                            try
                            {
                                newOp.ProductionOrderStatus = "boposReleased";
                                newOp.ProductionOrderLines = null;
                                serviceLayer.Patch<ProductionOrderModel>("ProductionOrders", newOp.AbsoluteEntry, newOp);
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    error = "Erro ao LIBERAR OP: " + ex.InnerException.Message;
                                }
                                else
                                {
                                    error = "Erro ao LIBERAR OP: " + ex.Message;
                                }
                            }
                        }
                    }

                    string update;

                    if (!String.IsNullOrEmpty(error))
                    {
                        if (error.Length > 254)
                        {
                            error = error.Substring(0, 254);
                        }

                        Logger.Error($"ERRO: {error}");

                        update = $@"update ""@CVA_LN_PLANEJAMENTO""
                                    set ""U_LineStatus"" = CASE WHEN '{error}' = '' THEN 'L' ELSE 'R' END, ""U_CVA_ERROR"" = '{error}'
                                    where ""DocEntry"" = {op.PlanEntry}
                                    and ""LineId"" = {op.U_CVA_LineId};";
                    }
                    else
                    {
                        update = $@"update ""@CVA_LN_PLANEJAMENTO""
                                    set ""U_LineStatus"" = CASE WHEN '{error}' = '' THEN 'L' ELSE 'R' END, ""U_CVA_ERROR"" = '{error}', ""U_CVA_OWOR"" = {newOp.AbsoluteEntry},
                                    ""U_CVA_OWOR_ALL"" = CASE WHEN IFNULL(""U_CVA_OWOR_ALL"", '') = '' THEN '{newOp.AbsoluteEntry}' ELSE ""U_CVA_OWOR_ALL"" || ', ' || '{newOp.AbsoluteEntry}' END
                                    where ""DocEntry"" = {op.PlanEntry}
                                    and ""LineId"" = {op.U_CVA_LineId};";
                    }

                    dao.ExecuteNonQuery(update);
                    Logger.Info($"Tempo de processamento: {Math.Truncate((DateTime.Now - startDate).TotalMilliseconds)} milisegundos");

                }

                if (previousDocEntry != "")
                {
                    dao.ExecuteNonQuery($@"update ""@CVA_PLANEJAMENTO""
                                                   set ""U_Status"" = case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {previousDocEntry}) > 0 then 'R' 
   					                                                else case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {previousDocEntry}) = 0 then 'L' end end
                                                 where ""DocEntry"" = {previousDocEntry};");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ERRO GERAL: " + ex.Message);
            }

            //    foreach (var planner in plannerStatus)
            //    {
            //        if (planner.Value == "NOK") continue;

            //        ClosePlanner(planner.Key);
            //    }
        }

        private void ClosePlanner(int docEntry)
        {
            Class.Conexao.ExecuteSqlNonQuery($@"update ""@CVA_LN_PLANEJAMENTO""
                                                   set ""U_LineStatus"" = 'L'  
                                                 where ""DocEntry"" = {docEntry}
                                                   and ""U_LineStatus"" = 'R';");

            Class.Conexao.ExecuteSqlNonQuery($@"update ""@CVA_PLANEJAMENTO""
                                                   set ""U_Status"" = case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {docEntry}) > 0 then 'R' 
   					                                                else case when (select count(*) from ""@CVA_LN_PLANEJAMENTO"" where ""U_LineStatus"" = 'P' and ""DocEntry"" = {docEntry}) = 0 then 'L' end end
                                                 where ""DocEntry"" = {docEntry};");
        }

        private void GerarDocumentosSAP()
        {
            string sql;
            int filial, lote;

            sql = string.Format(@"
SELECT T0.""U_Lote"", T0.""U_ID_Filial""
FROM ""@CVA_CAR_LOTE"" T0
    INNER JOIN ""@CVA_CAR_CONSOL"" T1 ON T1.""U_Lote"" = T0.""U_Lote""
WHERE
    IFNULL(T0.""U_Status"", 0) IN (0, 2)
    AND IFNULL(T1.""U_Status"", 0) IN (0, 2)
    AND IFNULL(T0.""U_Cancelado"", 'N') <> 'Y'
GROUP BY T0.""U_Lote"", T0.""U_ID_Filial""
");

            System.Data.DataTable oDT = Class.Conexao.ExecuteSqlDataTable(sql);
            foreach (System.Data.DataRow linha in oDT.Rows)
            {
                //oCon.CondVal = linha["ItmsGrpCod"].ToString();
                Int32.TryParse(linha["U_ID_Filial"].ToString(), out filial);
                Int32.TryParse(linha["U_Lote"].ToString(), out lote);

                if ((filial > 0) && (lote > 0))
                {
                    InserePrevisao(filial, lote);
                    InsereOP(filial, lote);
                    InserePedVenda(filial, lote);
                }
            }
        }


        //==================================================================================================================================//
        private void InserePrevisao(int filial, int lote)
        //==================================================================================================================================//
        {
            string sql, sFilial, code, name, depositoPadrao = "", sErrMsg, newDocEntry, origem, bplName, bplIdCD;
            int iErro = 0, iErrCode, j;
            DateTime dataPrevIni, dataPrevFim, dataMin, dataMax;
            System.Data.DataTable oDT;

            try
            {
                SAPbobsCOM.Recordset oRec2 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                bplIdCD = Class.Geral.RetornaCodFilialCD(filial.ToString());
                sFilial = filial.ToString().PadLeft(4, '0');
                sql = string.Format(@"SELECT ""U_DataDe"" FROM ""@CVA_CAR_LOTE"" WHERE ""U_Lote"" = {0} ", lote);
                dataPrevIni = Convert.ToDateTime(Class.Conexao.ExecuteSqlScalar(sql).ToString());
                sql = string.Format(@"SELECT ""U_DataAte"" FROM ""@CVA_CAR_LOTE"" WHERE ""U_Lote"" = {0} ", lote);
                dataPrevFim = Convert.ToDateTime(Class.Conexao.ExecuteSqlScalar(sql).ToString());

                sql = string.Format(@"
SELECT T0.* 
FROM ""@CVA_CAR_CONSOL"" T0
WHERE T0.""U_Lote"" = {0}
    AND T0.""U_Tipo"" = '{1}'
    AND IFNULL(T0.""U_Status"", 0) IN (0, 2)
", lote, "PC");
                oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                if (oDT.Rows.Count > 0)
                {
                    SAPbobsCOM.SalesForecast oSalesForecast = (SAPbobsCOM.SalesForecast)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSalesForecast);

                    bplName = RetornaNomeFilial(filial);
                    dataMin = RetornaData("Min", lote, "PC");
                    dataMax = RetornaData("Max", lote, "PC");
                    code = lote.ToString().PadLeft(6, '0') + "_PC";
                    name = "PC - FILIAL " + bplName + " " + dataPrevIni.ToString("dd/MM/yyyy") + " até " + dataPrevFim.ToString("dd/MM/yyyy");

                    oSalesForecast.ForecastCode = code;
                    oSalesForecast.ForecastName = name;
                    oSalesForecast.View = SAPbobsCOM.ForecastViewTypeEnum.fvtDaily;
                    oSalesForecast.ForecastStartDate = dataMin;
                    oSalesForecast.ForecastEndDate = dataMax;
                    oSalesForecast.UserFields.Fields.Item("U_BPLId").Value = filial.ToString();
                    oSalesForecast.UserFields.Fields.Item("U_Lote").Value = lote;

                    SAPbobsCOM.SalesForecast_Lines oSalesForecastLine = oSalesForecast.Lines;

                    j = 0;
                    foreach (System.Data.DataRow linha in oDT.Rows)
                    {
                        origem = linha["U_Origem"].ToString();
                        if (origem == "FI")
                            depositoPadrao = Class.Geral.RetornaCodDepPadrao(filial.ToString());
                        else
                            depositoPadrao = Class.Geral.RetornaCodDepPadrao(bplIdCD);

                        if (j > 0)
                            oSalesForecastLine.Add();
                        oSalesForecastLine.ForecastedDay = Convert.ToDateTime(linha["U_Data"].ToString());
                        oSalesForecastLine.ItemNo = linha["U_ItemCode"].ToString();
                        oSalesForecastLine.Quantity = Convert.ToDouble(linha["U_Quant"].ToString());
                        oSalesForecastLine.Warehouse = depositoPadrao;
                        j++;
                    }

                    Class.Conexao.gravaLog("Previsão: " + oSalesForecast.ForecastCode);

                    iErro = oSalesForecast.Add();
                    if (iErro != 0)
                    {
                        Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                        AtualizaTabela2("0", lote, "PC", 2, sErrMsg);
                    }
                    else
                    {
                        newDocEntry = Class.Conexao.oCompany.GetNewObjectKey();
                        AtualizaTabela2(newDocEntry, lote, "PC", 1, "");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void InsereOP(int filial, int lote)
        //==================================================================================================================================//
        {
            string sql, depositoPadrao = "", newDocEntry, sErrMsg;
            int erro, iErrCode, j;
            System.Data.DataTable oDT, dtLinhas;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"
SELECT * 
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}
    AND ""U_Tipo"" = '{1}'
    AND IFNULL(""U_Status"", 0) IN (0, 2)
", lote, "OP");

                oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                sql = string.Format(@"SELECT ""DflWhs"" FROM ""OBPL"" WHERE ""BPLId"" = {0} ", filial);
                oRec.DoQuery(sql);
                if (oRec.RecordCount > 0)
                    depositoPadrao = oRec.Fields.Item("DflWhs").Value.ToString();

                foreach (System.Data.DataRow linha in oDT.Rows)
                {
                    SAPbobsCOM.ProductionOrders oOP = (SAPbobsCOM.ProductionOrders)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);

                    oOP.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotStandard;
                    oOP.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposPlanned;
                    oOP.ItemNo = linha["U_ItemCode"].ToString();
                    oOP.PlannedQuantity = Convert.ToDouble(linha["U_Quant"].ToString());
                    oOP.Warehouse = depositoPadrao;
                    oOP.PostingDate = DateTime.Today;
                    oOP.StartDate = Convert.ToDateTime(linha["U_DataOrig"].ToString());
                    oOP.DueDate = Convert.ToDateTime(linha["U_DataOrig"].ToString());
                    oOP.UserFields.Fields.Item("U_CVA_Turno").Value = linha["U_IdTurno"].ToString();
                    oOP.UserFields.Fields.Item("U_CVA_SERVICO").Value = linha["U_IdServico"].ToString();
                    oOP.UserFields.Fields.Item("U_Lote").Value = lote;

                    sql = string.Format(@"
SELECT
    T4.""Code"" AS ""Prato""
	,T5.""Code"" AS ""ItemCode""
	,T6.""ItemName""
	,(T5.""Quantity"" / T4.""PlAvgSize"") AS ""Unit""
	,(T5.""Quantity"" / T4.""PlAvgSize"") * {1} AS ""Quant""
FROM ""OITT"" T4
    INNER JOIN ""ITT1"" T5 ON T5.""Father"" = T4.""Code""
    INNER JOIN ""OITM"" T6 ON T6.""ItemCode"" = T5.""Code""
WHERE T4.""Code"" = '{0}'
", oOP.ItemNo, oOP.PlannedQuantity);

                    dtLinhas = Class.Conexao.ExecuteSqlDataTable(sql);
                    j = 0;
                    foreach (System.Data.DataRow linha1 in dtLinhas.Rows)
                    {
                        if (j > 0)
                            oOP.Lines.Add();

                        oOP.Lines.ItemNo = linha1["ItemCode"].ToString();
                        oOP.Lines.BaseQuantity = Convert.ToDouble(linha1["Unit"].ToString());
                        //oOP.Lines.DistributionRule = "";
                        //oOP.Lines.EndDate = oOP.DueDate;
                        oOP.Lines.ItemType = SAPbobsCOM.ProductionItemType.pit_Item;
                        oOP.Lines.PlannedQuantity = oOP.PlannedQuantity * oOP.Lines.BaseQuantity;
                        oOP.Lines.ProductionOrderIssueType = SAPbobsCOM.BoIssueMethod.im_Manual;
                        //oOP.Lines.StartDate = oOP.StartDate;
                        oOP.Lines.Warehouse = depositoPadrao;

                        j++;
                    }

                    Class.Conexao.gravaLog("OP: " + oOP.ItemNo);

                    erro = oOP.Add();
                    if (erro != 0)
                    {
                        Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                        AtualizaTabelaOP("0", linha["Code"].ToString(), 2, sErrMsg);
                    }
                    else
                    {
                        newDocEntry = Class.Conexao.oCompany.GetNewObjectKey();
                        if (oOP.GetByKey(Convert.ToInt32(newDocEntry)))
                        {
                            oOP.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposReleased;
                            erro = oOP.Update();
                            if (erro != 0)
                            {
                                Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                                AtualizaTabelaOP("0", linha["Code"].ToString(), 2, sErrMsg);
                            }
                            else
                            {
                                AtualizaTabelaOP(newDocEntry, linha["Code"].ToString(), 1, "");
                            }
                        }
                        else
                        {
                            AtualizaTabelaOP("0", linha["Code"].ToString(), 2, "OP " + newDocEntry + " não localizada");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void InserePedVenda(int filial, int lote)
        //==================================================================================================================================//
        {
            string sql, depositoPadrao = "", sErrMsg, newDocEntry, aux, sData, cardCode = "", usage = "", itemCode, bplName = "", filialCD = "", dia, mes, ano;
            int iErro = 0, iErrCode, j, tipoPreco = 0, iLinha;
            double quant;
            DateTime data;
            System.Data.DataTable oDT;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                SAPbobsCOM.Recordset oRec1 = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                // Buscar as configurações de PN, usage, etc para esta filial
                sql = string.Format(@"
SELECT T0.*, T1.""DflWhs"" AS ""DepPadrao"", T1.""BPLName""
FROM ""@CVA_CAR_CONFIG"" T0
    LEFT JOIN ""OBPL"" T1 ON T1.""BPLId"" = T0.""U_BPLId""
WHERE T0.""U_BPLId"" = {0}
", filial);
                oRec1.DoQuery(sql);

                if (oRec1.RecordCount > 0)
                {
                    //cardCode = oRec1.Fields.Item("U_CardCodePN").Value.ToString(); -> esta informação vem direto da tabela de filial
                    tipoPreco = Convert.ToInt32(oRec1.Fields.Item("U_PrecoUnit").Value.ToString());
                    //sFilialOrigem = oRec1.Fields.Item("U_BPLId").Value.ToString(); 
                    //depositoPadrao = oRec1.Fields.Item("DepPadrao").Value.ToString();
                    //depositoPadrao = oRec1.Fields.Item("U_WhsCode").Value.ToString();
                    filialCD = oRec1.Fields.Item("U_BPLId_CD").Value.ToString();   // Código da filial CD p/ buscar o depósito padrão da mesma
                    usage = oRec1.Fields.Item("U_UsgTransf").Value.ToString();
                    bplName = oRec1.Fields.Item("BPLName").Value.ToString();
                    cardCode = oRec1.Fields.Item("U_CardCodePN").Value.ToString();
                }

                sql = string.Format(@"SELECT ""DflWhs"" FROM OBPL WHERE ""BPLId"" = {0} ", filialCD);
                oRec1.DoQuery(sql);
                if (oRec1.RecordCount > 0)
                    depositoPadrao = oRec1.Fields.Item("DflWhs").Value.ToString();

                // Determinar quantos Pedidos de Venda agrupando por data de entrega
                sql = string.Format(@"
SELECT ""U_Lote"", ""U_Data"", LPAD(DAYOFMONTH(""U_Data""), 2, '0') AS ""Dia"", LPAD(MONTH(""U_Data""), 2, '0') AS ""Mes"", YEAR(""U_Data"") AS ""Ano""
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}
    AND ""U_Tipo"" = 'PV' 
    AND IFNULL(""U_Status"", 0) IN (0, 2)
GROUP BY ""U_Lote"", ""U_Data""
", lote);
                oRec.DoQuery(sql);

                if (oRec.RecordCount > 0)
                {
                    oRec.MoveFirst();
                    // Para cada data de entrega, criar um Pedido de Venda com todos os itens
                    for (int i = 0; i < oRec.RecordCount; i++)
                    {
                        SAPbobsCOM.Documents oOrders = (SAPbobsCOM.Documents)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                        //aux = oRec.Fields.Item("U_Data").Value.ToString();
                        //data = Convert.ToDateTime(aux.Substring(6, 4) + "-" + aux.Substring(3, 2) + "-" + aux.Substring(0, 2));
                        //sData = data.ToString("yyyy-MM-dd");

                        dia = oRec.Fields.Item("Dia").Value.ToString();
                        mes = oRec.Fields.Item("Mes").Value.ToString();
                        ano = oRec.Fields.Item("Ano").Value.ToString();
                        aux = dia + "/" + mes + "/" + ano;
                        DateTime.TryParseExact(aux, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data);
                        sData = data.ToString("yyyy-MM-dd");

                        oOrders.CardCode = cardCode;
                        oOrders.DocDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        oOrders.DocDueDate = data;
                        oOrders.TaxDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        oOrders.BPL_IDAssignedToInvoice = 2;   // Convert.ToInt32(filial);
                        //oOrders.BPL_IDAssignedToInvoice = Convert.ToInt32(filial);
                        //oOrders.BPL_IDAssignedToInvoice = Convert.ToInt32(sFilialOrigem);
                        oOrders.Comments = "Consolidação de Planejamento - Transferência - CD para FILIAL " + bplName;
                        oOrders.UserFields.Fields.Item("U_Lote").Value = lote;
                        iLinha = 0;

                        sql = string.Format(@"
SELECT *
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = 'PV'  
    AND ""U_Data"" = '{1}'
    AND IFNULL(""U_Status"", 0) IN (0, 2)
", lote, sData);
                        oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                        foreach (System.Data.DataRow linha in oDT.Rows)
                        {
                            itemCode = linha["U_ItemCode"].ToString();
                            quant = Convert.ToDouble(linha["U_Quant"].ToString());

                            if (iLinha > 0)
                                oOrders.Lines.Add();
                            oOrders.Lines.ItemCode = itemCode;
                            oOrders.Lines.ShipDate = data;
                            //oOrders.Lines.UoMCode = "";     -> read only
                            oOrders.Lines.Quantity = quant;
                            //oOrders.Lines.WarehouseCode = "";
                            oOrders.Lines.Price = RetornaPreco(tipoPreco, itemCode);
                            //oOrders.Lines.Price = 10.5;
                            oOrders.Lines.Usage = usage;
                            oOrders.Lines.WarehouseCode = depositoPadrao;
                            //oOrders.Lines.
                            iLinha++;
                        }


                        Class.Conexao.gravaLog("PV: " + sData);
                        iErro = oOrders.Add();
                        if (iErro != 0)
                        {
                            Class.Conexao.oCompany.GetLastError(out iErrCode, out sErrMsg);
                            //AtualizaTabelaPV(string newDocEntry, int lote, DateTime data, int erro, string msgErro)
                            AtualizaTabelaPV("0", lote, data, 2, sErrMsg);
                        }
                        else
                        {
                            newDocEntry = Class.Conexao.oCompany.GetNewObjectKey();
                            AtualizaTabelaPV(newDocEntry, lote, data, 1, "");
                        }

                        oRec.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private string RetornaNomeFilial(int filial)
        //==================================================================================================================================//
        {
            string s = "", sql;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                sql = string.Format(@"SELECT ""BPLName"" FROM ""OBPL"" WHERE ""BPLId"" = {0} ", filial);
                oRec.DoQuery(sql);
                if (oRec.RecordCount > 0)
                    s = oRec.Fields.Item("BPLName").Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return s;
        }

        //==================================================================================================================================//
        private DateTime RetornaData(string opcao, int lote, string tipo)
        //==================================================================================================================================//
        {
            DateTime dtAux = DateTime.MinValue;
            string sql, aux;

            if (opcao == "Min")
                sql = string.Format(@"
SELECT MIN(""U_Data"") 
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = '{1}'  
", lote, tipo);
            else
                sql = string.Format(@"
SELECT MAX(""U_Data"") 
FROM ""@CVA_CAR_CONSOL"" 
WHERE ""U_Lote"" = {0}  
    AND ""U_Tipo"" = '{1}'  
", lote, tipo);

            aux = Class.Conexao.ExecuteSqlScalar(sql).ToString();
            DateTime.TryParse(aux, out dtAux);

            return dtAux;
        }

        //==================================================================================================================================//
        private double RetornaPreco(int tipoPreco, string itemCode)
        //==================================================================================================================================//
        {
            string sql;
            double preco = 0;

            SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            if (tipoPreco < 0)
            {

                sql = string.Format(@"SELECT IFNULL(""LastPurPrc"", 0) AS ""LastPurPrc"", IFNULL(""AvgPrice"", 0) AS ""AvgPrice"" FROM OITM WHERE ""ItemCode"" = '{0}'", itemCode);
                oRec.DoQuery(sql);

                if (oRec.RecordCount > 0)
                {
                    if (tipoPreco == -2)
                        preco = Convert.ToDouble(oRec.Fields.Item("LastPurPrc").Value.ToString(), CultureInfo.InvariantCulture);
                    else
                        preco = Convert.ToDouble(oRec.Fields.Item("AvgPrice").Value.ToString(), CultureInfo.InvariantCulture);
                }
            }
            if (tipoPreco > 0)
            {
                sql = string.Format(@"SELECT IFNULL(""Price"", 0) AS ""Price"" FROM ITM1 WHERE ""ItemCode"" = '{0}' AND ""PriceList"" = {1}", itemCode, tipoPreco);
                oRec.DoQuery(sql);

                if (oRec.RecordCount > 0)
                {
                    preco = Convert.ToDouble(oRec.Fields.Item("Price").Value.ToString(), CultureInfo.InvariantCulture);
                }
            }

            return preco;
        }

        //==================================================================================================================================//
        private void AtualizaTabela2(string newDocEntry, int lote, string tipo, int erro, string msgErro)
        //==================================================================================================================================//
        {
            string sql = "", msg;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                //sData = data.ToString("yyyy-MM-dd");
                msg = msgErro.Replace("'", "");

                //if (tipo == "CD")
                //{
                sql = string.Format(@"
UPDATE 
    ""@CVA_CAR_CONSOL"" 
SET 
    ""U_DocEntryPC"" = {0}
    ,""U_Status"" = {1}
    ,""U_Msg"" = '{2}' 
WHERE 
    ""U_Lote"" = {3} 
    AND ""U_Tipo"" = '{4}'  
", newDocEntry, erro, msg, lote, tipo);
                //}

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void AtualizaTabelaOP(string newDocEntry, string code, int erro, string msgErro)
        //==================================================================================================================================//
        {
            string sql = "", sData, msg;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                msg = msgErro.Replace("'", "");

                sql = string.Format(@"
UPDATE 
    ""@CVA_CAR_CONSOL"" 
SET 
    ""U_DocEntryOP"" = {0}
    ,""U_Status"" = {1}
    ,""U_Msg"" = '{2}' 
WHERE 
    ""Code"" = '{3}' 
", newDocEntry, erro, msg, code);

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void AtualizaTabelaPV(string newDocEntry, int lote, DateTime data, int erro, string msgErro)
        //==================================================================================================================================//
        {
            string sql = "", sData, msg;

            try
            {
                SAPbobsCOM.Recordset oRec = (SAPbobsCOM.Recordset)Class.Conexao.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                msg = msgErro.Replace("'", "");
                sData = data.ToString("yyyy-MM-dd");

                sql = string.Format(@"
UPDATE 
    ""@CVA_CAR_CONSOL"" 
SET 
    ""U_DocEntryPV"" = {0}
    ,""U_Status"" = {1}
    ,""U_Msg"" = '{2}' 
WHERE 
    ""U_Lote"" = {3} 
    AND ""U_Tipo"" = 'PV'
    AND ""U_Data"" = '{4}'  
", newDocEntry, erro, msg, lote, sData);

                oRec.DoQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
