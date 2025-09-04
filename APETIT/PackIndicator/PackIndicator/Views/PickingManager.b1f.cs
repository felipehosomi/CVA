using CsvHelper;
using PackIndicator.Controllers;
using PackIndicator.Models;
using SAPbobsCOM;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace PackIndicator.Views
{
    [FormAttribute("81", "Views/PickingManager.b1f")]
    class PickingManager : SystemFormBase
    {
        public PickingManager()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.btSuggest = ((SAPbouiCOM.Button)(this.GetItem("btSuggest").Specific));
            this.btSuggest.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btSuggest_PressedAfter);
            this.btGenWMS = ((SAPbouiCOM.Button)(this.GetItem("btGenWMS").Specific));
            this.btGenWMS.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btGenWMS_PressedAfter);
            this.btImport = ((SAPbouiCOM.Button)(this.GetItem("btImport").Specific));
            this.btImport.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btImport_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ActivateAfter += new ActivateAfterHandler(this.Form_ActivateAfter);

        }

        private void OnCustomInitialize()
        {
        }

        #region [ Item Events ]
        private void btSuggest_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("A análise de sugestão de picking pode ser um processo demorado, deseja continuar?", 1, "Sim", "Nâo") != 1) return;

            try
            {
                CommonController.MotherForm = UIAPIRawForm;

                var pickingIndicator = new PickingIndicator();
                pickingIndicator.Show();
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
        }

        private void btGenWMS_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var dialog = new SelectFileDialog("", "", "", DialogType.FOLDER);
                dialog.Open();

                if (string.IsNullOrEmpty(dialog.SelectedFolder)) return;

                var mtPickingData = (Matrix)UIAPIRawForm.Items.Item("17").Specific;
                var fileData = new List<WMSIntegrationFile.Header>();
                var dir = dialog.SelectedFolder;

                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Obtendo informações das linhas selecionadas...", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                for (var i = 1; i <= mtPickingData.RowCount; i++)
                {
                    // Caso o checkbox da linha não esteja selecionado, ignora
                    if (!((CheckBox)mtPickingData.Columns.Item("1").Cells.Item(i).Specific).Checked) continue;

                    #region  [ Obtenção das informações que comporão o arquivo de integração ]
                    var businessPartner = (SAPbobsCOM.BusinessPartners)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                    businessPartner.GetByKey(((EditText)mtPickingData.Columns.Item("12").Cells.Item(i).Specific).Value);

                    var order = (SAPbobsCOM.Documents)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                    order.GetByKey(int.Parse(((EditText)mtPickingData.Columns.Item("24").Cells.Item(i).Specific).Value));
                    order.Lines.SetCurrentLine(int.Parse(((EditText)mtPickingData.Columns.Item("14").Cells.Item(i).Specific).Value) - 1);

                    var item = (SAPbobsCOM.Items)CommonController.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                    item.GetByKey(((EditText)mtPickingData.Columns.Item("10").Cells.Item(i).Specific).Value);

                    var shipDate = DateTime.ParseExact(((EditText)mtPickingData.Columns.Item("7").Cells.Item(i).Specific).Value, "yyyyMMdd", null);
                    var routeCode = ((EditText)mtPickingData.Columns.Item("U_CVA_RotaEntrega").Cells.Item(i).Specific).Value;
                    var storageCategory = ((EditText)mtPickingData.Columns.Item("U_CVA_CatEstoque").Cells.Item(i).Specific).Value;

                    var wmsheader = new WMSIntegrationFile.Header();

                    if (fileData.Exists(x => x.DocEntry == order.DocEntry && x.ShipDate == shipDate && x.RouteCode == routeCode && x.StorageCategory == storageCategory))
                    {
                        wmsheader = fileData.Where(x => x.DocEntry == order.DocEntry && x.ShipDate == shipDate && x.RouteCode == routeCode && x.StorageCategory == storageCategory).FirstOrDefault();
                    }
                    else
                    {
                        wmsheader.DocEntry = order.DocEntry;
                        wmsheader.CardCode = businessPartner.CardCode;
                        wmsheader.CardFName = businessPartner.CardForeignName;
                        wmsheader.County = RemoveAccents(CommonController.GetCountyByAbsId(int.Parse(String.IsNullOrEmpty(order.AddressExtension.ShipToCounty) ? "0" : order.AddressExtension.ShipToCounty)));
                        wmsheader.State = order.AddressExtension.ShipToState;
                        wmsheader.Street = RemoveAccents($"{order.AddressExtension.ShipToAddressType} {order.AddressExtension.ShipToStreet}, {order.AddressExtension.ShipToStreetNo}");
                        wmsheader.Block = RemoveAccents(order.AddressExtension.ShipToBlock);
                        wmsheader.ZipCode = order.AddressExtension.ShipToZipCode;
                        wmsheader.DocNum = order.DocNum;
                        wmsheader.ShipDate = shipDate;
                        wmsheader.RouteCode = routeCode;
                        wmsheader.StorageCategory = storageCategory;
                        wmsheader.FileType = "E";
                    }

                    var wmsItem = new WMSIntegrationFile.Items();
                    wmsItem.ItemCode = item.ItemCode;
                    wmsItem.ItemName = item.ItemName;
                    wmsItem.Price = order.Lines.Price;
                    wmsItem.Quantity = double.Parse(((EditText)mtPickingData.Columns.Item("6").Cells.Item(i).Specific).Value);
                    wmsItem.Weight = String.IsNullOrEmpty(((EditText)mtPickingData.Columns.Item("1320000108").Cells.Item(i).Specific).Value.ToString()) ? 0.0 : double.Parse(((EditText)mtPickingData.Columns.Item("1320000108").Cells.Item(i).Specific).Value.Replace(item.SalesUnit.ToLower(), ""));
                    wmsItem.AbsEntry = int.Parse(((EditText)mtPickingData.Columns.Item("25").Cells.Item(i).Specific).Value);
                    wmsItem.PickEntry = int.Parse(((EditText)mtPickingData.Columns.Item("31").Cells.Item(i).Specific).Value);
                    wmsItem.UomEntry = order.Lines.UoMEntry;

                    wmsheader.Items.Add(wmsItem);

                    if (fileData.Exists(x => x.DocEntry == order.DocEntry && x.ShipDate == shipDate && x.RouteCode == routeCode && x.StorageCategory == storageCategory)) continue;

                    fileData.Add(wmsheader);
                    #endregion
                }

                if (fileData.Count == 0)
                {
                    SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Nenhuma linha foi selecionada para a geração do arquivo de integração.", SAPbouiCOM.BoMessageTime.bmt_Short);
                    return;
                }

                #region [ Geração do arquivo de integração ]

                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Gerando arquivo de integração...", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                foreach (var route in fileData.GroupBy(x => new { x.RouteCode, x.StorageCategory }).ToList())
                {
                    using (var mem = new MemoryStream())
                    using (var writer = new StreamWriter(mem))
                    using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csvWriter.Configuration.Delimiter = ";";

                        var routeCode = route.FirstOrDefault().RouteCode;
                        var storageCategory = route.FirstOrDefault().StorageCategory;
                        var minShipDate = fileData.Where(x => x.RouteCode == routeCode && x.StorageCategory == storageCategory).Min(x => x.ShipDate);
                        var maxShipDate = fileData.Where(x => x.RouteCode == routeCode && x.StorageCategory == storageCategory).Max(x => x.ShipDate);

                        foreach (var data in fileData.Where(x => x.RouteCode == routeCode && x.StorageCategory == storageCategory))
                        {
                            csvWriter.WriteField("1");
                            csvWriter.WriteField(data.CardCode.ToUpper());
                            csvWriter.WriteField(data.CardFName.ToUpper());
                            csvWriter.WriteField(data.County.ToUpper());
                            csvWriter.WriteField(data.State.ToUpper());
                            csvWriter.WriteField(data.Street.ToUpper());
                            csvWriter.WriteField(data.Block.ToUpper());
                            csvWriter.WriteField(data.ZipCode);
                            csvWriter.WriteField(data.DocNum);
                            csvWriter.WriteField(data.ShipDate.ToString("ddMMyyyy"));
                            csvWriter.WriteField(""); // Quantidade
                            csvWriter.WriteField(data.RouteCode.ToUpper());
                            csvWriter.WriteField(data.FileType);
                            csvWriter.NextRecord();

                            foreach (var item in data.Items)
                            {
                                csvWriter.WriteField("2");
                                csvWriter.WriteField((int)item.Quantity * 100000);
                                csvWriter.WriteField($"{item.ItemCode}.{item.UomEntry}");
                                csvWriter.WriteField(item.Weight);
                                csvWriter.WriteField(""); // Volume
                                csvWriter.WriteField(item.ItemName.ToUpper());
                                csvWriter.WriteField(item.Price);
                                csvWriter.WriteField(item.PickEntry);
                                csvWriter.WriteField(item.AbsEntry);
                                csvWriter.WriteField(data.DocNum);
                                csvWriter.NextRecord();
                            }
                        }

                        writer.Flush();

                        var fileName = $"E{routeCode}{storageCategory}{minShipDate.ToString("ddMMyyyy")}{maxShipDate.ToString("ddMMyyyy")}.txt";
                        File.WriteAllText(Path.Combine(dir, fileName), Encoding.UTF8.GetString(mem.ToArray()));

                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText($"Gerado arquivo de integração: {fileName}", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                    }
                }

                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Todos os arquivos de integração foram gerados com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

                #endregion
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
        }

        private void btImport_PressedAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                var dialog = new SelectFileDialog("", "", "(*.txt)|*.txt", DialogType.OPEN);
                dialog.Open();

                if (String.IsNullOrEmpty(dialog.SelectedFile)) return;

                UIAPIRawForm.Freeze(true);

                var absEntries = new List<int>();
                var wmsItems = new List<WMSIntegrationFile.Items>();

                #region [ Obtenção das informações contidas no arquivo ]
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Realizando a leitura nos arquivos...", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                foreach (var file in dialog.SelectedFiles)
                {
                    using (var reader = new StreamReader(file))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.HasHeaderRecord = false;
                        csv.Configuration.Delimiter = ";";

                        while (csv.Read())
                        {
                            if (csv.GetField(0) == "1" && csv.GetField(12) != "R")
                            {
                                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText($"O arquivo {file} não se trata de um arquivo de retorno, pois o sentido indicado nele é diferente de R.", SAPbouiCOM.BoMessageTime.bmt_Short);
                                break;
                            }
                            else if (csv.GetField(0) == "2")
                            {
                                var item = csv.GetRecord<WMSIntegrationFile.Items>();
                                item.Quantity = item.Quantity / 100000;
                                wmsItems.Add(item);
                                if (!absEntries.Contains(item.AbsEntry)) absEntries.Add(item.AbsEntry);
                            }
                        }
                    }
                }
                #endregion

                if (absEntries.Count == 0) return;

                #region [ Efetuação do picking ]
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Efetuando picking...", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                foreach (var absEntry in absEntries)
                {
                    PickingController.SetPickList(absEntry, wmsItems.Where(x => x.AbsEntry == absEntry).ToList());

                    //PickingController.SetZeroPickList(absEntry, wmsItems.Where(x => x.AbsEntry == absEntry && x.Quantity == 0).ToList());
                }

                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Picking efetuado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);

                UIAPIRawForm.Items.Item("8").Click(); // Picking efetuado 

                //// Verifica no arquivo se existem pickings não realizados 
                //if (wmsItems.Exists(x => x.Quantity == 0))
                //{
                //    UIAPIRawForm.Items.Item("6").Click(); // Liberado

                //    var mtPickingData = (Matrix)UIAPIRawForm.Items.Item("17").Specific;

                //    foreach (var item in wmsItems.Where(x => x.Quantity == 0))
                //    {
                //        for (var i = 1; i <= mtPickingData.RowCount; i++)
                //        {
                //            var docNum = int.Parse(((EditText)mtPickingData.Columns.Item("2").Cells.Item(i).Specific).Value);
                //            var docLineNum = int.Parse(((EditText)mtPickingData.Columns.Item("14").Cells.Item(i).Specific).Value);
                //            var absEntry = int.Parse(((EditText)mtPickingData.Columns.Item("25").Cells.Item(i).Specific).Value);

                //            if (item.DocNum != docNum && PickingController.GetDocLineNum(item.AbsEntry, item.PickEntry) != docLineNum && absEntry != item.AbsEntry) continue;

                //            mtPickingData.Columns.Item("0").Cells.Item(i).Click();
                //            SAPbouiCOM.Framework.Application.SBO_Application.SendKeys("^(K)"); // Ctrl + K para eliminar a linha
                //            break;
                //        }

                // TODO: Alterar a linha do pedido de venda para a unidade de medidad base
                //    }

                //    UIAPIRawForm.Items.Item("1").Click(); // Atualização do gerente de picking
                //    UIAPIRawForm.Items.Item("8").Click(); // Picking efetuado 
                //}
                #endregion
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);

                // Se está em uma trasação
                if (CommonController.Company.InTransaction)
                {
                    // Então
                    // Finaliza a transação realizando um rollback para uma nova ser aberta
                    CommonController.Company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        private void Form_ActivateAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (!CommonController.InProcess) return;

            try
            {
                UIAPIRawForm.Freeze(true);
                UIAPIRawForm.Items.Item("6").Click(); // Liberado
                UIAPIRawForm.Items.Item("7").Click(); // Aberto

                if (CommonController.TransitionObject == null) return;

                #region [ Atualização das quantidades a liberar ]
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Atualizando as quantidades para liberar...", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                var pickingData = (List<PickingData>)CommonController.TransitionObject;
                var mtPickingData = (Matrix)UIAPIRawForm.Items.Item("10").Specific;

                for (var i = 1; i <= mtPickingData.RowCount; i++)
                {
                    // Caso a linha já tenha uma UM original, então já ocorreu a sugestão na linha, porém algum erro fez com que ela não tenha sido
                    // liberada para picking. Sendo assim, seleciona a linha, mas não realiza nova sugestão
                    if (!String.IsNullOrEmpty(((EditText)mtPickingData.Columns.Item("U_CVA_OriginalUom").Cells.Item(i).Specific).Value))
                    {
                        // Caso não exista quantidade disponível, ignorar
                        if (String.IsNullOrEmpty(((EditText)mtPickingData.Columns.Item("2").Cells.Item(i).Specific).Value) ||
                            double.Parse(((EditText)mtPickingData.Columns.Item("2").Cells.Item(i).Specific).Value) <= 0.0) continue;

                        ((CheckBox)mtPickingData.Columns.Item("1").Cells.Item(i).Specific).Checked = true;
                        continue;
                    }

                    var docEntry = int.Parse(((EditText)mtPickingData.Columns.Item("23").Cells.Item(i).Specific).Value);
                    var docLineNum = int.Parse(((EditText)mtPickingData.Columns.Item("31").Cells.Item(i).Specific).Value);

                    if (!pickingData.Exists(x => x.DocEntry == docEntry && x.DocLineNum == docLineNum)) continue;

                    var availableQty = double.Parse(((EditText)mtPickingData.Columns.Item("2").Cells.Item(i).Specific).Value);

                    // Caso não haja quantidade disponível, ignorar
                    if (availableQty <= 0.0) continue;

                    var pickingLine = pickingData.FirstOrDefault(x => x.DocEntry == docEntry && x.DocLineNum == docLineNum);

                    if (pickingLine.OriginalUom == ((EditText)mtPickingData.Columns.Item("10000062").Cells.Item(i).Specific).Value) continue;

                    if (pickingLine.DocLineType == LineType.Balance) continue;

                    var qty = pickingLine.Packages.Count == 0 ? 0 : pickingLine.Packages.FirstOrDefault().Volume;

                    // Caso não tenha sido utilizada nenhuma embalagem na linha do pedido, ignorar
                    if (qty == 0.0) continue;

                    ((EditText)mtPickingData.Columns.Item("3").Cells.Item(i).Specific).Value = availableQty < qty ? availableQty.ToString() : qty.ToString();
                    ((CheckBox)mtPickingData.Columns.Item("1").Cells.Item(i).Specific).Checked = pickingLine.Packages.Count != 0;
                }

                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Quantidades atualizadas com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                #endregion

                #region [ Liberação do picking ]
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Realizando a liberação do picking...", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);

                UIAPIRawForm.Items.Item("11").Click(); // Liberar para lista de picking
                UIAPIRawForm.Items.Item("6").Click(); // Liberado

                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("Picking liberado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
                #endregion
            }
            catch (Exception ex)
            {
                SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
                CommonController.InProcess = false;
                CommonController.TransitionObject = null;
            }
        }

        private static string RemoveAccents(string text)
        {
            var sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) == UnicodeCategory.NonSpacingMark) continue;
                sbReturn.Append(letter);
            }

            return sbReturn.ToString();
        }

        private Button btSuggest;
        private Button btGenWMS;
        private Button btImport;
    }
}
