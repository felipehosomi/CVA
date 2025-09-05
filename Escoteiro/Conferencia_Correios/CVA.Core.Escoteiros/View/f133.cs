


using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Core.Escoteiros.DAO;
using CVA.Core.Escoteiros.Model;
using Newtonsoft.Json;
using RestSharp;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CVA.Core.Escoteiros.View
{
    public class f133 : BaseForm
    {
        private Form Form;
        public static Button _Button;
        #region Constructor

        public f133()
        {
            FormCount++;
        }

        public f133(ItemEvent itemEvent)
        {
            this.ItemEventInfo = itemEvent;
        }

        public f133(BusinessObjectInfo businessObjectInfo)
        {
            this.BusinessObjectInfo = businessObjectInfo;
        }

        public f133(ContextMenuInfo contextMenuInfo)
        {
            this.ContextMenuInfo = contextMenuInfo;
        }

        #endregion

        public override bool ItemEvent()
        {
            if (!ItemEventInfo.BeforeAction)
            {
                if (ItemEventInfo.EventType == BoEventTypes.et_FORM_LOAD)
                {
                    try
                    {
                        Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);
                        var item = Form.Items.Add("btn_Inte", BoFormItemTypes.it_BUTTON);
                        item.Left = 146;
                        item.Top = 592;
                        item.Width = 100;
                        item.Height = 19;

                        _Button = (Button)item.Specific;
                        _Button.Caption = "Integrar Intelipost";

                        return true;
                    }
                    catch (Exception)
                    {
                    }

                }

                if (ItemEventInfo.EventType == BoEventTypes.et_CLICK && ItemEventInfo.ItemUID == "btn_Inte")
                {
                    #region Carrega Json
                    try
                    {
                        Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);

                        SBOApp.Application.SetStatusBarMessage("Inciando Integração com o Intelipost. Aguarde...", BoMessageTime.bmt_Short, false);
                        Form.Freeze(true);

                        Form = SBOApp.Application.Forms.GetFormByTypeAndCount(ItemEventInfo.FormType, ItemEventInfo.FormTypeCount);

                        DBDataSource ds_OINV = Form.DataSources.DBDataSources.Item("OINV");
                        string docentry = ds_OINV.GetValue("DocEntry", ds_OINV.Offset);

                        var ListaIntelipost = _IntelipostModel(docentry);

                        Shipment_OrderModel _Shipment_OrderModel = new Shipment_OrderModel();

                        foreach (var item in ListaIntelipost)
                        {
                            // Parte  I


                            _Shipment_OrderModel.order_number = item.order_number;
                            _Shipment_OrderModel.customer_shipping_costs = item.custumer_shipping_costs;
                            _Shipment_OrderModel.sales_channel = item.sales_channel;
                            _Shipment_OrderModel.created = item.created;
                            _Shipment_OrderModel.shipped_date = item.shipped_date;
                            _Shipment_OrderModel.shipment_order_type = item.shipped_order_type;
                            _Shipment_OrderModel.delivery_method_id = item.delivery_method_id;
                            _Shipment_OrderModel.delivery_method_external_id = item.delivery_method_External_id;

                            // Parte II
                            _Shipment_OrderModel.end_customer = new End_Customer();
                            _Shipment_OrderModel.end_customer.first_name =  item.first_name.Length > 60 ? item.first_name.Substring(0,60) : item.first_name;
                            _Shipment_OrderModel.end_customer.last_name = "";
                            _Shipment_OrderModel.end_customer.email = item.email;

                            if (item.phone.Length > 2 && item.phone.Substring(2, 1) != "9")
                            {
                                _Shipment_OrderModel.end_customer.phone = item.phone;
                            }
                            else
                            {
                                _Shipment_OrderModel.end_customer.phone = "";
                            }
                           

                            if (item.cellphone.Length > 2 && item.cellphone.Substring(2, 1) == "9")
                            {
                                _Shipment_OrderModel.end_customer.cellphone = item.cellphone.ToString();
                            }
                            else if (item.phone.Length > 2 && item.phone.Substring(2, 1) == "9")
                            {
                                _Shipment_OrderModel.end_customer.cellphone = item.phone.ToString();
                            }
                            else
                            {
                                _Shipment_OrderModel.end_customer.cellphone = "";
                            }


                            if (item.is_company == "True")
                            {
                                _Shipment_OrderModel.end_customer.is_company = true;
                            }
                            else
                            {
                                if (item.is_company == "False")
                                {
                                    _Shipment_OrderModel.end_customer.is_company = false;
                                }
                            }

                            _Shipment_OrderModel.end_customer.federal_tax_payer_id = item.federal_tax_payer_id;
                            _Shipment_OrderModel.end_customer.shipping_country = item.shipping_country;
                            _Shipment_OrderModel.end_customer.shipping_city = item.shipping_city;
                            _Shipment_OrderModel.end_customer.shipping_address = item.shipping_address;
                            _Shipment_OrderModel.end_customer.shipping_number = item.shipping_number;
                            _Shipment_OrderModel.end_customer.shipping_quarter = item.shipping_quarter;
                            _Shipment_OrderModel.end_customer.shipping_reference = item.shipping_reference;
                            _Shipment_OrderModel.end_customer.shipping_additional = item.shipping_additional;
                            _Shipment_OrderModel.end_customer.shipping_zip_code = item.shipping_zip_code;
                            _Shipment_OrderModel.origin_zip_code = item.origin_zip_code;
                            _Shipment_OrderModel.origin_warehouse_code = item.origin_warehouse_code;


                            // Parte III
                            var listaIntelipostVolume = _IntelipostVolume(item.erp.ToString());

                            int count = listaIntelipostVolume.Count;
                            int i = 0;

                            _Shipment_OrderModel.shipment_order_volume_array = new Shipment_Order_Volume_Array[count];

                            foreach (var model in listaIntelipostVolume)
                            {
                                var obj = new Shipment_Order_Volume_Array();

                                obj.name = model.name;
                                obj.shipment_order_volume_number = model.shipment_order_volume_number;
                                obj.volume_type_code = model.volume_type_code;
                                obj.weight = model.weight;
                                obj.width = model.widht;
                                obj.height = model.height;
                                obj.products_quantity = model.prdoducts_quantity;
                                obj.products_nature = model.products_name;


                                // Parte IV

                                obj.shipment_order_volume_invoice = new Shipment_Order_Volume_Invoice();

                                obj.shipment_order_volume_invoice.invoice_series = item.invoice_serie;
                                obj.shipment_order_volume_invoice.invoice_number = item.invoice_number.ToString();
                                obj.shipment_order_volume_invoice.invoice_key = item.invoice_key;
                                obj.shipment_order_volume_invoice.invoice_date = item.invoice_date;
                                obj.shipment_order_volume_invoice.invoice_total_value = item.invoice_total_value.ToString().Replace(",", ".");
                                obj.shipment_order_volume_invoice.invoice_products_value = item.invoice_products_value.ToString().Replace(",", ".");
                                obj.shipment_order_volume_invoice.invoice_cfop = item.invoice_cfop;

                                obj.tracking_code = item.tracking_code;

                                _Shipment_OrderModel.shipment_order_volume_array[i] = obj;
                                i++;

                            }
                            _Shipment_OrderModel.estimated_delivery_date = item.estimate_delivery_date;

                            _Shipment_OrderModel.external_order_numbers = new External_Order_Numbers();
                            _Shipment_OrderModel.external_order_numbers.erp = item.erp.ToString();

                        }

                        string json = JsonConvert.SerializeObject(_Shipment_OrderModel);

                        string local = @"C:\CVA Consultoria\Files Json Intelipost\";
                        if (local.Substring(local.Length - 1) != "\\")
                        {
                            local = local.Trim() + "\\";
                        }

                        System.IO.File.WriteAllText(local + DateTime.Now.ToString("yyyyMMdd_HHmm_") + _Shipment_OrderModel.order_number + "_NF_Saída_Enviado.json", json);

                        #endregion

                        RestClient client = new RestClient("https://api.intelipost.com.br/api/v1/shipment_order");
                        RestRequest request = new RestRequest(Method.POST);
                        request.AddHeader("content-type", "application/json");
                        request.AddHeader("plugin-version", "v2.0.0");
                        request.AddHeader("plugin", "intelipost-plugin");
                        request.AddHeader("platform-version", "v1.0.0");
                        request.AddHeader("platform", "intelipost-docs");
                        request.AddHeader("api-key", "f9c4674123bffbec4a1dc1db791b2e61922d6ce5ede8e5214f2233312e641073");
                        request.AddParameter("application/json", json, ParameterType.RequestBody);

                        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                        IRestResponse response = client.Execute(request);

                        System.IO.File.WriteAllText(local + DateTime.Now.ToString("yyyyMMdd_HHmm_") + _Shipment_OrderModel.order_number + "_NF_Saída_Retorno.json", response.Content);

                        #region Atualiza OINV

                        SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        oRecordSet.DoQuery(String.Format(Query.UpadateStatusIntegradoOINV, docentry));

                        #endregion
                        Form.Freeze(false);
                        if (response.StatusCode.ToString() == "OK")
                        {
                            SBOApp.Application.SetStatusBarMessage("Integração Concluida com Sucesso !!! ", BoMessageTime.bmt_Short, false);
                        }
                        else
                        {
                            SBOApp.Application.SetStatusBarMessage("Integração Não realizada, Verifique o arquivo de retorno !!! ", BoMessageTime.bmt_Short, false);
                        }


                    }
                    catch (Exception ex)
                    {
                        Form.Freeze(false);
                        SBOApp.Application.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
                        return false;
                    }


                }
            }
            return true;
        }

        public override bool FormDataEvent()
        {
            if (!BusinessObjectInfo.BeforeAction)
            {
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD)
                {
                    try
                    {
                        Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);
                        CancelOrder _CancelOrder = new CancelOrder();

                        DBDataSource ds_GC = Form.DataSources.DBDataSources.Item("OINV");


                        _CancelOrder.order_number = ds_GC.GetValue("U_CVA_OrderNum", ds_GC.Offset);
                        string Integrado = ds_GC.GetValue("U_CVA_Intelipost", ds_GC.Offset);
                        string Status = ds_GC.GetValue(4, ds_GC.Offset);
                        string header = Form.Title;

                        if (Integrado == "S" &&( header == "Nota fiscal de saída - Cancelamento"|| header == "A/R Invoice - Cancellation"))
                        {
                            string json = Serializar(_CancelOrder);
                            string local = @"C:\CVA Consultoria\Files Json Intelipost\";
                            if (local.Substring(local.Length - 1) != "\\")
                            {
                                local = local.Trim() + "\\";
                            }

                            System.IO.File.WriteAllText(local + DateTime.Now.ToString("yyyyMMdd_HHmm_") + _CancelOrder.order_number + "_NF_Saída_Cancelamento_Enviado.json", json);


                            RestClient client = new RestClient("https://api.intelipost.com.br/api/v1/shipment_order/cancel_shipment_order/");
                            RestRequest request = new RestRequest(Method.POST);
                            request.AddHeader("content-type", "application/json");
                            request.AddHeader("plugin-version", "v2.0.0");
                            request.AddHeader("plugin", "intelipost-plugin");
                            request.AddHeader("platform-version", "v1.0.0");
                            request.AddHeader("platform", "intelipost-docs");
                            request.AddHeader("api-key", "f9c4674123bffbec4a1dc1db791b2e61922d6ce5ede8e5214f2233312e641073");
                            request.AddParameter("application/json", json, ParameterType.RequestBody);

                            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                            IRestResponse response = client.Execute(request);

                            System.IO.File.WriteAllText(local + DateTime.Now.ToString("yyyyMMdd_HHmm_") + _CancelOrder.order_number + "_NF_Saída_Cancelamento_Retorno.json", response.Content);

                            if (response.StatusCode.ToString() == "OK")
                            {
                                SBOApp.Application.SetStatusBarMessage("Canecalmento Intelipost realizado com Sucesso !!! ", BoMessageTime.bmt_Short, false);
                            }
                            else
                            {
                                SBOApp.Application.SetStatusBarMessage("Cancelamento no Intelipost não realizado, Verifique o arquivo de retorno !!! ", BoMessageTime.bmt_Short, false);
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                        SBOApp.Application.SetStatusBarMessage(ex.Message, BoMessageTime.bmt_Short, true);
                    }


                    return true;
                }
                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_LOAD)
                {
                    try
                    {
                        Form = SBOApp.Application.Forms.Item(BusinessObjectInfo.FormUID);

                        var item = Form.Items.Add("btn_Inte", BoFormItemTypes.it_BUTTON);

                        item.Left = 146;
                        item.Top = 607;
                        item.Width = 100;
                        item.Height = 19;

                        _Button = (Button)item.Specific;
                        _Button.Caption = "Integrar Intelipost";
                    }
                    catch (Exception)
                    {

                    }


                    return true;
                }

            }
            return true;
        }

        private List<IntelipostModel> _IntelipostModel(string DocEntry)
        {
            var ListaIntelipostModel = new List<IntelipostModel>();

            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.CarregaObjIntelipost, DocEntry));

            if (!oRecordSet.EoF)
            {
                var model = new IntelipostModel();

                model.order_number = oRecordSet.Fields.Item(0).Value.ToString();
                model.custumer_shipping_costs = float.Parse(oRecordSet.Fields.Item(1).Value.ToString(), System.Globalization.NumberStyles.AllowDecimalPoint);
                model.sales_channel = oRecordSet.Fields.Item(2).Value;
                model.scheduled = oRecordSet.Fields.Item(3).Value;
                model.created = Convert.ToDateTime(oRecordSet.Fields.Item(4).Value);
                model.shipped_date = Convert.ToDateTime(oRecordSet.Fields.Item(5).Value);
                model.shipped_order_type = oRecordSet.Fields.Item(6).Value;
                model.delivery_method_id = Convert.ToInt32(oRecordSet.Fields.Item(7).Value);
                model.delivery_method_External_id = Convert.ToInt32(oRecordSet.Fields.Item(8).Value);
                model.first_name = oRecordSet.Fields.Item(9).Value;
                model.last_name = oRecordSet.Fields.Item(10).Value;
                model.email = oRecordSet.Fields.Item(11).Value;
                model.phone = oRecordSet.Fields.Item(12).Value;
                model.cellphone = oRecordSet.Fields.Item(13).Value;
                model.is_company = oRecordSet.Fields.Item(14).Value;
                model.federal_tax_payer_id = oRecordSet.Fields.Item(15).Value;
                model.shipping_country = oRecordSet.Fields.Item(16).Value;
                model.shipping_state = oRecordSet.Fields.Item(17).Value;
                model.shipping_city = oRecordSet.Fields.Item(18).Value;
                model.shipping_address = oRecordSet.Fields.Item(19).Value;
                model.shipping_number = oRecordSet.Fields.Item(20).Value;
                model.shipping_quarter = oRecordSet.Fields.Item(21).Value;
                model.shipping_reference = oRecordSet.Fields.Item(22).Value;
                model.shipping_additional = oRecordSet.Fields.Item(23).Value;
                model.shipping_zip_code = oRecordSet.Fields.Item(24).Value;
                model.origin_zip_code = oRecordSet.Fields.Item(25).Value;
                model.origin_warehouse_code = oRecordSet.Fields.Item(26).Value;
                model.invoice_serie = oRecordSet.Fields.Item(27).Value;
                model.invoice_number = Convert.ToInt32(oRecordSet.Fields.Item(28).Value);
                model.invoice_key = oRecordSet.Fields.Item(29).Value;
                model.invoice_date = Convert.ToDateTime(oRecordSet.Fields.Item(30).Value);
                model.invoice_total_value = Convert.ToDouble(oRecordSet.Fields.Item(31).Value);
                model.invoice_products_value = Convert.ToDouble(oRecordSet.Fields.Item(32).Value);
                model.invoice_cfop = oRecordSet.Fields.Item(33).Value;
                model.tracking_code = oRecordSet.Fields.Item(34).Value;
                model.estimate_delivery_date = Convert.ToDateTime(oRecordSet.Fields.Item(35).Value);
                model.erp = Convert.ToInt32(oRecordSet.Fields.Item(36).Value);

                ListaIntelipostModel.Add(model);
            }
            return ListaIntelipostModel;

        }

        private List<IntelipostVolume> _IntelipostVolume(string DocEntry)
        {
            var ListaIntelipostVolume = new List<IntelipostVolume>();

            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)SBOApp.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecordSet.DoQuery(String.Format(Query.CarregaObjIntelipostVolume, DocEntry));

            if (!oRecordSet.EoF)
            {
                for (int i = 0; i < oRecordSet.RecordCount; i++)
                {


                    var model = new IntelipostVolume();

                    model.name = oRecordSet.Fields.Item(0).Value;
                    model.shipment_order_volume_number = Convert.ToInt32(oRecordSet.Fields.Item(1).Value);
                    model.volume_type_code = oRecordSet.Fields.Item(2).Value;
                    model.weight = Convert.ToSingle(oRecordSet.Fields.Item(3).Value);
                    model.widht = Convert.ToInt32(oRecordSet.Fields.Item(4).Value);
                    model.height = Convert.ToInt32(oRecordSet.Fields.Item(5).Value);
                    model.lenght = Convert.ToInt32(oRecordSet.Fields.Item(6).Value);
                    model.prdoducts_quantity = Convert.ToInt32(oRecordSet.Fields.Item(7).Value);
                    model.products_name = oRecordSet.Fields.Item(8).Value;

                    ListaIntelipostVolume.Add(model);
                    oRecordSet.MoveNext();
                }
            }
            return ListaIntelipostVolume;

        }

        public static string Serializar<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

    }
}
