using CVA.IntegracaoMagento.SalesOrder.Controller;
using CVA.IntegracaoMagento.SalesOrder.Infrastructure;
using CVA.IntegracaoMagento.SalesOrder.Models.Magento;
using CVA.IntegracaoMagento.SalesOrder.Models.SAP;
using Flurl.Http;
using Newtonsoft.Json;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CVA.IntegracaoMagento.SalesOrder
{
    public class Integration
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];
        private static readonly string OrderSpecific = ConfigurationManager.AppSettings["Order"];

        public Integration()
        {
        }

        public async Task SetIntegration()
        {
            DateTime dataAtual = DateTime.Now.AddSeconds(1);
            string sCaminho = String.Format(@"{0}Log", System.AppDomain.CurrentDomain.BaseDirectory.ToString());

            if (!(System.IO.Directory.Exists(sCaminho)))
                System.IO.Directory.CreateDirectory(sCaminho);

            sCaminho = String.Format(@"{0}\\Log_{1}.txt", sCaminho, String.Format(@"{0}{1}{2}", dataAtual.Year.ToString(), dataAtual.Month.ToString().PadLeft(2, '0'), dataAtual.Day.ToString().PadLeft(2, '0')));

            if (!(System.IO.File.Exists(sCaminho)))
                System.IO.File.Create(sCaminho).Close();

            Util.GravarLog(sCaminho, "[PROCESSO] - Iniciando o processo.");

            try
            {
                SLConnection = new SLConnection(ServiceLayerURL, Database, B1User, B1Password, 29);
                var objConfig = await SLConnection.GetAsync<List<Metadata_Config.CVA_CONFIG_MAG>>("CVA_CONFIG_MAG");
                var token = new Token();

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão HANA (OK)");


                //token.apiAddressUri = @"https://api.grupoboticario.com.br";
                //token.username = "cva";
                //token.password = "6STKym@pu*DR";
                //token.MagentoClientId = "b04f39cf-12bc-411c-99ff-2d00c64744d3";
                //token.MagentoClientSecret = "C0xN8lB2uY3mP2mH8xJ7mL5kT8yY3uQ4vH6tP4oA8tT4pR2tL6";


                token.apiAddressUri = objConfig[0].U_ApiUrl; //MagentoURL
                token.username = objConfig[0].U_ApiUsuario; //MagentoUser
                token.password = objConfig[0].U_ApiSenha; //MagentoPassword
                token.MagentoClientId = objConfig[0].U_ApiClientId; //MagentoClientId
                token.MagentoClientSecret = objConfig[0].U_ApiClientSecret; //MagentoClientSecret


                string sMensagemErro = String.Empty;
                Token.create_CN(token, ref sMensagemErro);

                if (String.IsNullOrEmpty(token.bearerToken))
                    throw new Exception(String.Format(@"Bearer Token não gerado. Detalhes: {0}", sMensagemErro));

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão Magento (OK)");
                token.bearerToken = token.bearerToken.Replace('"', ' ').Trim();

                /*
                var oPaymentTest = await SLConnection.GetAsync<Metadata_BusinessPartner>($"BusinessPartners('C00000002')?$select=PeymentMethodCode");
                string sTeste = "Retirar na loja - Avenida Rui Barbosa, 4110 | 83050-010";
                string sTeste1 = "Retirar na loja";
                string sTeste2 = "";
                if (sTeste.Contains(sTeste1))
                    sTeste2 = "1";
                */

                Sales.Main objSales = new Sales.Main();
                objSales = SalesOrderController.SalesToSap(token, OrderSpecific);
                string sCodigoPedidoMagento = String.Empty;
                string sQuery = String.Empty;
                int iTotalPedidos = 0;

                //json para teste
                //var item = JsonConvert.DeserializeObject<Sales.Item>(@"{""base_currency_code"":""BRL"",""base_discount_amount"":0.0,""base_grand_total"":0.1,""base_discount_tax_compensation_amount"":0.0,""base_shipping_amount"":0.0,""base_shipping_discount_amount"":0.0,""base_shipping_discount_tax_compensation_amnt"":0.0,""base_shipping_incl_tax"":0.0,""base_shipping_tax_amount"":0.0,""base_subtotal"":1.5,""base_subtotal_incl_tax"":1.5,""base_tax_amount"":0.0,""base_total_due"":0.1,""base_to_global_rate"":1.0,""base_to_order_rate"":1.0,""billing_address_id"":11456,""created_at"":""2021 - 08 - 30 14:12:45"",""customer_email"":""renanp @grupoboticario.com.br"",""customer_firstname"":""Renan"",""customer_group_id"":1,""customer_is_guest"":0,""customer_lastname"":""Lass Pompei"",""customer_taxvat"":""09146260943"",""customer_note_notify"":1,""discount_amount"":0.0,""email_sent"":0,""entity_id"":5728,""global_currency_code"":""BRL"",""grand_total"":0.1,""discount_tax_compensation_amount"":0.0,""increment_id"":""8000003998"",""is_virtual"":0,""order_currency_code"":""BRL"",""protect_code"":""e0b884378fa7e3e57e7a28c167658a0a"",""quote_id"":13839,""remote_ip"":""191.5.234.66"",""shipping_amount"":0.0,""shipping_description"":""Retirada na loja - Av.Rui Barbosa, nº 4.110, Bairro Parque da Fonte"",""shipping_discount_amount"":0.0,""shipping_discount_tax_compensation_amount"":0.0,""shipping_incl_tax"":0.0,""shipping_tax_amount"":0.0,""state"":""new"",""status"":""pending"",""store_currency_code"":""BRL"",""store_id"":8,""store_name"":""AGB - SJP\nSão José dos Pinhais\nSJP Stock"",""store_to_base_rate"":0,""store_to_order_rate"":0,""subtotal"":1.5,""subtotal_incl_tax"":1.5,""tax_amount"":0.0,""total_due"":0.1,""total_item_count"":1.0,""total_qty_ordered"":1.0,""updated_at"":""2021 - 08 - 30 14:12:46"",""weight"":0.0,""codigo_cliente"":null,""is_pos_order"":null,""items"":[{""amount_refunded"":0.0,""base_amount_refunded"":0.0,""base_discount_amount"":0.0,""base_discount_invoiced"":0.0,""base_discount_tax_compensation_amount"":0.0,""base_original_price"":1.5,""base_price"":1.5,""base_price_incl_tax"":1.5,""base_row_invoiced"":0.0,""base_row_total"":1.5,""base_row_total_incl_tax"":1.5,""base_tax_amount"":0.0,""base_tax_invoiced"":0.0,""created_at"":""2021 - 08 - 30 14:12:45"",""discount_amount"":0.0,""discount_invoiced"":0.0,""discount_percent"":0.0,""free_shipping"":0.0,""discount_tax_compensation_amount"":0.0,""is_qty_decimal"":0,""is_virtual"":0,""item_id"":21291,""name"":""SACOLA P MENINO 2019"",""no_discount"":0,""order_id"":5728,""original_price"":1.5,""price"":1.5,""price_incl_tax"":1.5,""product_id"":19464,""product_type"":""simple"",""qty_canceled"":0.0,""qty_invoiced"":0.0,""qty_ordered"":1.0,""qty_refunded"":0.0,""qty_shipped"":0.0,""quote_item_id"":45027,""row_invoiced"":0.0,""row_total"":1.5,""row_total_incl_tax"":1.5,""row_weight"":0.0,""sku"":""00732"",""store_id"":8,""tax_amount"":0.0,""tax_invoiced"":0.0,""tax_percent"":0.0,""updated_at"":""2021 - 08 - 30 14:12:45""}],""billing_address"":{""address_type"":""billing"",""city"":""São José dos Pinhais"",""country_id"":""BR"",""email"":""renanp @grupoboticario.com.br"",""entity_id"":11456,""firstname"":""Renan"",""lastname"":""Lass Pompei"",""parent_id"":5728,""postcode"":""83045200"",""region"":""Paraná"",""region_code"":""PR"",""region_id"":499,""street"":[""Rua Arcy Possebon"",""29"",""Afonso Pena""],""telephone"":""4130812472""},""payment"":{""account_status"":null,""additional_information"":["""",""mastercard"",""0207"",""token_0vp8KljukuR47R3r"",""0"","""",""1"",""MundiPagg - Cartão de Crédito"",""34212519187173"",""34212519187173"",null],""amount_ordered"":0.1,""base_amount_ordered"":0.1,""base_shipping_amount"":0.0,""cc_type"":""mastercard"",""cc_exp_month"":""5"",""cc_exp_year"":""2028"",""cc_last4"":""0207"",""cc_ss_start_month"":""0"",""cc_ss_start_year"":""0"",""entity_id"":5728,""method"":""mundipagg_creditcard"",""parent_id"":5728,""shipping_amount"":0.0},""status_histories"":[{""comment"":""MP - Pedido pendente na Mundipagg.Id or_8NbLVNWUlUnv6Y7k"",""created_at"":""2021 - 08 - 30 14:12:46"",""entity_id"":6346,""entity_name"":""order"",""is_customer_notified"":2,""is_visible_on_front"":0,""parent_id"":5728,""status"":""pending""}],""extension_attributes"":{""shipping_assignments"":[{""shipping"":{""address"":{""address_type"":""shipping"",""city"":""São José dos Pinhais"",""country_id"":""BR"",""customer_address_id"":44230,""email"":""renanp @grupoboticario.com.br"",""entity_id"":11455,""firstname"":""Renan"",""lastname"":""Lass Pompei"",""parent_id"":5728,""postcode"":""83045200"",""region"":""Paraná"",""region_code"":""PR"",""region_id"":499,""street"":[""Rua Arcy Possebon"",""29"",""Afonso Pena""],""telephone"":""4130812472""},""method"":""flatrate_flatrate"",""total"":{""base_shipping_amount"":0.0,""base_shipping_discount_amount"":0.0,""base_shipping_discount_tax_compensation_amnt"":0.0,""base_shipping_incl_tax"":0.0,""base_shipping_tax_amount"":0.0,""shipping_amount"":0.0,""shipping_discount_amount"":0.0,""shipping_discount_tax_compensation_amount"":0.0,""shipping_incl_tax"":0.0,""shipping_tax_amount"":0.0}},""items"":[{""base_currency_code"":null,""base_discount_amount"":0.0,""base_grand_total"":0.0,""base_discount_tax_compensation_amount"":0.0,""base_shipping_amount"":0.0,""base_shipping_discount_amount"":0.0,""base_shipping_discount_tax_compensation_amnt"":0.0,""base_shipping_incl_tax"":0.0,""base_shipping_tax_amount"":0.0,""base_subtotal"":0.0,""base_subtotal_incl_tax"":0.0,""base_tax_amount"":0.0,""base_total_due"":0.0,""base_to_global_rate"":0.0,""base_to_order_rate"":0.0,""billing_address_id"":0,""created_at"":""2021 - 08 - 30 14:12:45"",""customer_email"":null,""customer_firstname"":null,""customer_group_id"":0,""customer_is_guest"":0,""customer_lastname"":null,""customer_taxvat"":null,""customer_note_notify"":0,""discount_amount"":0.0,""email_sent"":0,""entity_id"":0,""global_currency_code"":null,""grand_total"":0.0,""discount_tax_compensation_amount"":0.0,""increment_id"":null,""is_virtual"":0,""order_currency_code"":null,""protect_code"":null,""quote_id"":0,""remote_ip"":null,""shipping_amount"":0.0,""shipping_description"":null,""shipping_discount_amount"":0.0,""shipping_discount_tax_compensation_amount"":0.0,""shipping_incl_tax"":0.0,""shipping_tax_amount"":0.0,""state"":null,""status"":null,""store_currency_code"":null,""store_id"":8,""store_name"":null,""store_to_base_rate"":0,""store_to_order_rate"":0,""subtotal"":0.0,""subtotal_incl_tax"":0.0,""tax_amount"":0.0,""total_due"":0.0,""total_item_count"":0.0,""total_qty_ordered"":0.0,""updated_at"":""2021 - 08 - 30 14:12:45"",""weight"":0.0,""codigo_cliente"":null,""is_pos_order"":null,""items"":null,""billing_address"":null,""payment"":null,""status_histories"":null,""extension_attributes"":null}]}],""payment_additional_info"":[{""key"":""cc_saved_card"",""value"":""""},{""key"":""cc_type"",""value"":""mastercard""},{""key"":""cc_last_4"",""value"":""0207""},{""key"":""cc_token_credit_card"",""value"":""token_0vp8KljukuR47R3r""},{""key"":""cc_savecard"",""value"":""0""},{""key"":""cc_buyer_checkbox"",""value"":""""},{""key"":""cc_installments"",""value"":""1""},{""key"":""method_title"",""value"":""MundiPagg - Cartão de Crédito""},{""key"":""cc_tid"",""value"":""34212519187173""},{""key"":""cc_nsu_authorization"",""value"":""34212519187173""},{""key"":""cc_nsu_capture"",""value"":""null""}],""applied_taxes"":[],""item_applied_taxes"":[]}}");

                foreach (var item in objSales.items)
                {
                    Util.GravarLog(sCaminho, @"[PROCESSO] - Iniciando Integração");

                    string sNumeroSAP = String.Empty;
                    string sMensagemSAP = String.Empty;
                    string sProcesso = String.Empty;
                    string sCardCode = String.Empty;
                    try
                    {
                        bool bPedidoPDV = (item.is_pos_order == "1" ? true : false);
                        var hana = new Hana();

                        sCodigoPedidoMagento = item.entity_id.ToString();
                        var oPaymentMethod = item.payment;

                        #region [ Verificar se o Pedido Magento já foi integrado no SAP ]

                        sQuery = String.Format(@"SELECT ""DocNum"" FROM ""{0}"".""ORDR"" WHERE ""U_CVA_Magento_Entity"" = {1}", Database, sCodigoPedidoMagento);
                        string sPedidosExistentes = String.Empty;

                        using (var dr = hana.ExecuteReaderAsync(sQuery).Result)
                        {
                            if (dr.HasRows)
                            {
                                var dt = new DataTable();
                                dt.Load(dr);

                                foreach (DataRow row in dt.Rows)
                                {
                                    sPedidosExistentes += (!String.IsNullOrEmpty(sPedidosExistentes) ? String.Format(@", {0}", row["DocNum"].ToString()) : row["DocNum"].ToString());
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(sPedidosExistentes))
                            throw new Exception(String.Format(@"Pedido Magento nº {0} já integrado (Pedido SAP nº {1})", sCodigoPedidoMagento, sPedidosExistentes));

                        #endregion

                        #region [ Criando o Pedido de Venda no SAP ]

                        Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - CRIANDO PEDIDO NO SAP:" + sCodigoPedidoMagento));

                        sProcesso = "Pedido de Venda";

                        if (item.subtotal_incl_tax <= 0)
                            throw new Exception(String.Format(@"O valor do pedido é menor ou igual a R$ 0,00."));

                        //-- Filial
                        var filial = objConfig[0].CVA_CONFIG_MAG1Collection.Where(s => s.U_FilialMagento == item.store_id.ToString()).FirstOrDefault();
                        if (filial == null || String.IsNullOrEmpty(filial.U_FilialSap))
                            throw new Exception(String.Format(@"Filial Magento (store_id: {0}) não configurado no SAP.", item.store_id.ToString()));

                        //-- Código do Cliente
                        if (String.IsNullOrEmpty(item.codigo_cliente) && String.IsNullOrEmpty(item.customer_taxvat))
                            throw new Exception(String.Format(@"O Parceiro não foi identificado no SAP. Os campos [codigo_cliente] e/ou [customer_taxvat] são obrigatórios no pedido Magento."));

                        if (!String.IsNullOrEmpty(item.codigo_cliente))
                        {
                            try
                            {
                                var oBP = await SLConnection.GetAsync<Metadata_BusinessPartner>($"BusinessPartners('{item.codigo_cliente}')");
                                sCardCode = item.codigo_cliente;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("Nenhum registro concordante"))
                                    throw new Exception(String.Format(@"Parceiro {0} não cadastrado no SAP.", item.codigo_cliente));
                                else
                                    throw new Exception(String.Format(@"Erro ao consultar o Parceiro {0} no SAP. Detalhes: {1}", item.codigo_cliente, ex.Message));
                            }
                        }
                        else
                        {
                            string sCPFMagento = item.customer_taxvat.Replace("-", String.Empty).Replace(".", String.Empty).Replace("/", String.Empty).PadLeft(11, '0');                            
                            sQuery = String.Format(@"SELECT TOP 1 ""CardCode"" FROM ""{0}"".""CRD7""
                                                     WHERE REPLACE(REPLACE(REPLACE(""TaxId4"", '-', ''), '.', ''), '/', '') = '{1}'", Database, sCPFMagento);

                            using (var dr = hana.ExecuteReaderAsync(sQuery).Result)
                            {
                                if (dr.HasRows)
                                {
                                    var dt = new DataTable();
                                    dt.Load(dr);

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        sCardCode = row["CardCode"].ToString();
                                        break;
                                    }
                                }
                            }

                            if (String.IsNullOrEmpty(sCardCode))
                                throw new Exception(String.Format(@"Parceiro não identificado no SAP. TaxVat: {0}", sCPFMagento));
                        }

                        #region [ Validações ]

                        Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - VALIDAÇÕES:" + sCodigoPedidoMagento));

                        var CC = objConfig[0].CVA_CONFIG_MAG3Collection.Where(s => s.U_FormaMagento == oPaymentMethod.method.ToString()).FirstOrDefault();
                        if (CC == null || String.IsNullOrEmpty(CC.U_Conta))
                            throw new Exception(String.Format(@"Forma de Pagamento (Payment Method: {0}) não configurado no SAP.", oPaymentMethod.method.ToString()));

                        if (oPaymentMethod.method.Equals("mundipagg_creditcard"))
                        {
                            var cartao = await SLConnection.GetAsync<List<Metadata_CreditCard.CreditCard>>("CreditCards");
                            if (cartao == null || cartao.Count == 0)
                                throw new Exception($"Nenhum cartão de crédito encontrado no SAP.");

                            var nsu = item.extension_attributes.payment_additional_info.Where(s => s.key.Contains("cc_nsu_authorization")).FirstOrDefault();
                            if (nsu == null || String.IsNullOrEmpty(nsu.value))
                                throw new Exception(String.Format(@"Campo NSU inválido."));

                            if (String.IsNullOrEmpty(oPaymentMethod.cc_exp_year) || String.IsNullOrEmpty(oPaymentMethod.cc_exp_month))
                                throw new Exception(String.Format(@"Informações de validade do cartão inválidas. Detalhes: Ano ({0}) - Mês ({1})", oPaymentMethod.cc_exp_year.ToString(), oPaymentMethod.cc_exp_month.ToString()));

                            if (String.IsNullOrEmpty(oPaymentMethod.additional_information[2].ToString()))
                                throw new Exception(String.Format(@"Número do cartão inválido. Detalhes: {0}", oPaymentMethod.additional_information[2].ToString()));

                            if (oPaymentMethod.amount_ordered <= 0.00)
                                throw new Exception(String.Format(@"Valor atribuído ao cartão inválido. Detalhes: {0}", oPaymentMethod.amount_ordered.ToString()));
                        }

                        #endregion

                        var oBusinessPartner = await SLConnection.GetAsync<Metadata_BusinessPartner>($"BusinessPartners('{sCardCode}')");

                        Metadata_SalesOrder.Orders oOrders = new Metadata_SalesOrder.Orders();
                        oOrders.BPL_IDAssignedToInvoice = Convert.ToInt32(filial.U_FilialSap);
                        oOrders.DocDate = DateTime.Now;
                        oOrders.DocDueDate = DateTime.Now;
                        oOrders.TaxDate = Convert.ToDateTime(item.created_at.Substring(0, 10));
                        oOrders.CardCode = sCardCode;
                        oOrders.CardName = String.Format(@"{0} {1}", item.customer_firstname, item.customer_lastname);
                        if (String.IsNullOrEmpty(oBusinessPartner.ShipToDefault)) oOrders.ShipToCode = oBusinessPartner.ShipToDefault;
                        if (String.IsNullOrEmpty(oBusinessPartner.BilltoDefault)) oOrders.PayToCode = oBusinessPartner.BilltoDefault;

                        //-- Tipo de Envio
                        var oConfigFrete = objConfig[0].CVA_CONFIG_MAG5Collection.Where(s => item.shipping_description.Contains(s.U_FreteMagento) &&
                                                                                        s.U_TipoPV == (bPedidoPDV ? "PD" : "EC")).FirstOrDefault();
                        if (oConfigFrete == null || String.IsNullOrEmpty(oConfigFrete.U_FreteSap))
                            throw new Exception(String.Format(@"Frete '{0}' - Origem '{1}' não configurado no SAP.", item.shipping_description, (bPedidoPDV ? "PDV" : "E-Commerce")));
                        oOrders.TransportationCode = Convert.ToInt32(oConfigFrete.U_FreteSap);                        

                        //-- Condição de Pagamento
                        if (String.IsNullOrEmpty(objConfig[0].CVA_CONFIG_MAG2Collection[0].U_CondSap))
                            throw new Exception(String.Format(@"Condição de Pagamento não configurado no SAP."));
                        oOrders.GroupNumber = Convert.ToInt32(objConfig[0].CVA_CONFIG_MAG2Collection[0].U_CondSap);

                        //-- Forma de Pagamento
                        var oConfigForma = objConfig[0].CVA_CONFIG_MAG3Collection.Where(s => s.U_FormaMagento == oPaymentMethod.method).FirstOrDefault();

                        if (oPaymentMethod.method.Equals("free"))
                        {
                            if (oBusinessPartner != null)
                                oOrders.PaymentMethod = oBusinessPartner.PeymentMethodCode;
                        }
                        else
                        {
                            if (oConfigForma == null || String.IsNullOrEmpty(oConfigForma.U_FormaMagento))
                                throw new Exception(String.Format(@"Forma de Pagamento '{0}' não configurado no SAP.", oPaymentMethod.method));
                            oOrders.PaymentMethod = oConfigForma.U_FormaSap;
                        }

                        oOrders.U_CVA_Magento_Entity = item.entity_id;
                        oOrders.U_CVA_Magento_Id = item.increment_id;                        

                        DateTime horaAtual = DateTime.Now.AddSeconds(30);
                        oOrders.U_CVA_Magento_Data = horaAtual;
                        oOrders.U_CVA_Magento_Hora = Convert.ToInt32(String.Format(@"{0}{1}{2}", horaAtual.Hour.ToString(), horaAtual.Minute.ToString().PadLeft(2, '0'), horaAtual.Second.ToString().PadLeft(2, '0')));
                        oOrders.U_CVA_Magento_Status = "1";
                        oOrders.U_CVA_Magento_Msg = "";
                        oOrders.U_CVA_SourceChannel = (bPedidoPDV ? "B" : "C");
                        oOrders.DocumentLines = new List<Metadata_SalesOrder.DocumentLine>();
                        oOrders.DocumentAdditionalExpenses = new List<Metadata_SalesOrder.DocumentAdditionalExpens>();
                        oOrders.TaxExtension = new Metadata_SalesOrder.TaxExtension();

                        //-- Depósito
                        var deposito = objConfig[0].CVA_CONFIG_MAG1Collection.Where(s => s.U_FilialMagento == item.store_id.ToString()).FirstOrDefault();
                        if (deposito == null || String.IsNullOrEmpty(deposito.U_Deposito))
                            throw new Exception(String.Format(@"Depósito SAP (store_id: {0}) não configurado no SAP.", item.store_id.ToString()));

                        //-- Utilização
                        var utilizacao = objConfig[0].U_Utilizacao;
                        if (String.IsNullOrEmpty(utilizacao))
                            throw new Exception(String.Format(@"Utilização não configurada no SAP."));

                        //-- Data de Vencimento
                        try
                        {
                            DateTime dtVencimento = Convert.ToDateTime(item.created_at.Substring(0, 10));
                            var dataVencimento = objConfig[0].CVA_CONFIG_MAG4Collection.Where(s => dtVencimento >= s.U_DataDe &&
                                                                                              s.U_DataAte >= dtVencimento).FirstOrDefault();
                            if (dataVencimento == null || String.IsNullOrEmpty(dataVencimento.U_DataVenc.ToShortDateString()))
                                throw new Exception(String.Format(@"Data de Vencimento não configurado no SAP."));

                            oOrders.U_CVA_Vcto = dataVencimento.U_DataVenc;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(String.Format(@"Data de Vencimento não configurado no SAP."));
                        }
                        
                        Metadata_SalesOrder.UpdateOrder oUpdateOrder = new Metadata_SalesOrder.UpdateOrder();
                        oUpdateOrder.DocumentLines = new List<Metadata_SalesOrder.UpdateOrderLines>();
                        int iIndex = 0;

                        string sItensNaoEncontrados = String.Empty;

                        foreach (var itemLine in objSales.items[iTotalPedidos].items)
                        {
                            try
                            {
                                var oItems = await SLConnection.GetAsync<Metadata_Items>($"Items('{itemLine.sku}')?$select=ItemCode");
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("Nenhum registro concordante"))
                                    sItensNaoEncontrados += (String.IsNullOrEmpty(sItensNaoEncontrados) ? itemLine.sku : String.Format(@", {0}", itemLine.sku));
                                else
                                    throw new Exception(String.Format(@"Erro ao consultar o item {0} no SAP. Detalhes: {1}", itemLine.sku, ex.Message));
                            }

                            var oDocumentLine = new Metadata_SalesOrder.DocumentLine
                            {
                                LineNum = iIndex,
                                ItemCode = itemLine.sku,
                                ItemDescription = itemLine.name,
                                BarCode = "",
                                Quantity = itemLine.qty_ordered,
                                Price = itemLine.base_price,
                                DiscountPercent = itemLine.discount_percent,
                                Usage = Convert.ToInt32(utilizacao),
                                WarehouseCode = deposito.U_Deposito,
                                UnitPrice = itemLine.base_price,
                                U_CVA_Magento_ItemId = itemLine.item_id.ToString()
                            };
                            oOrders.DocumentLines.Add(oDocumentLine);

                            var oUpdateOrderLine = new Metadata_SalesOrder.UpdateOrderLines
                            {
                                LineNum = iIndex,
                                UnitPrice = itemLine.base_price
                            };
                            oUpdateOrder.DocumentLines.Add(oUpdateOrderLine);

                            iIndex++;
                        }

                        if (!String.IsNullOrEmpty(sItensNaoEncontrados))
                            throw new Exception(String.Format(@"Item(s) SKU {0} não cadastrado(s) no SAP.", sItensNaoEncontrados));

                        if (item.shipping_amount > 0)
                        {
                            var oAdditionalExpens = new Metadata_SalesOrder.DocumentAdditionalExpens
                            {
                                ExpenseCode = Convert.ToInt32(objConfig[0].U_Despesa),
                                LineTotal = item.shipping_amount
                            };
                            oOrders.DocumentAdditionalExpenses.Add(oAdditionalExpens);
                        }

                        oOrders.TaxExtension.TaxId4 = item.customer_taxvat;

                        //-- Incoterms
                        string sIncoterms = String.Empty;
                        sQuery = String.Format(@"SELECT TOP 1 LEFT(""TrnspName"", 1) AS ""TipoFrete"" FROM ""{0}"".""OSHP""
                                                 WHERE ""TrnspCode"" = '{1}'", Database, oConfigFrete.U_FreteSap);

                        using (var dr = hana.ExecuteReaderAsync(sQuery).Result)
                        {
                            if (dr.HasRows)
                            {
                                var dt = new DataTable();
                                dt.Load(dr);

                                foreach (DataRow row in dt.Rows)
                                {
                                    sIncoterms = row["TipoFrete"].ToString();
                                    break;
                                }
                            }
                        }
                        oOrders.TaxExtension.Incoterms = sIncoterms;

                        #endregion

                        try
                        {
                            var jsonPV = JsonConvert.SerializeObject(oOrders);
                            var jsonMagento = JsonConvert.SerializeObject(item);
                            Util.GravarLog(sCaminho, String.Format(@"[JSON] - Magento: {0}", jsonMagento));
                            Util.GravarLog(sCaminho, String.Format(@"[JSON] - SAP: {0}", jsonPV));

                            oOrders = await SLConnection.PostAsync("Orders", oOrders);
                            sNumeroSAP = oOrders.DocNum.ToString();

                            #region [ Atualizar o Pedido com o cálculo reverso do imposto ]

                            /*
                            sProcesso = "Pedido de Venda (Atualização)";                            

                            foreach (var itemLines in oUpdateOrder.DocumentLines)
                            {
                                sQuery = String.Format(@"SELECT ""{0}"".""CVA_CALCULAPRECOLIQUIDO""('{1}','{2}') AS ""Retorno"" FROM DUMMY;", Database, oOrders.DocEntry.ToString(), itemLines.LineNum.ToString());
                                Util.GravarLog(sCaminho, String.Format(@"[QUERY] - Update Sales: {0}", sQuery));
                                using (var dr = hana.ExecuteReaderAsync(sQuery).Result)
                                {
                                    if (dr.HasRows)
                                    {
                                        var dt = new DataTable();
                                        dt.Load(dr);

                                        foreach (DataRow row in dt.Rows)
                                        {
                                            itemLines.UnitPrice = double.Parse(row["Retorno"].ToString());
                                        }
                                    }
                                }
                            }

                            var json = JsonConvert.SerializeObject(oUpdateOrder);
                            Util.GravarLog(sCaminho, String.Format(@"[JSON] - Update Sales: {0}", json));

                            await SLConnection.PatchAsync($"Orders({oOrders.DocEntry})", oUpdateOrder);

                            */

                            #endregion

                            if (oConfigForma.U_Adiant.Equals("S"))
                            {
                                #region [ Criando o Adiantamento de Clientes no SAP ]

                                sProcesso = "Adiantamento";

                                var oDownPayments = new Metadata_DownPayments.DownPayments()
                                {
                                    DownPaymentType = "dptInvoice",
                                    CardCode = oOrders.CardCode,
                                    DocDate = oOrders.DocDate,
                                    TaxDate = oOrders.DocDate,
                                    DocDueDate = oOrders.DocDueDate,
                                    BPL_IDAssignedToInvoice = oOrders.BPL_IDAssignedToInvoice,
                                    DocumentLines = new List<Metadata_DownPayments.DocumentLine>()
                                };

                                foreach (var oDownPaymentsLines in oOrders.DocumentLines)
                                {
                                    var docLine = new Metadata_DownPayments.DocumentLine
                                    {
                                        ItemCode = oDownPaymentsLines.ItemCode,
                                        UnitPrice = oDownPaymentsLines.UnitPrice,
                                        DiscountPercent = oDownPaymentsLines.DiscountPercent,
                                        Quantity = oDownPaymentsLines.Quantity,
                                        Usage = oDownPaymentsLines.Usage,
                                        WarehouseCode = oDownPaymentsLines.WarehouseCode,
                                        BaseEntry = oOrders.DocEntry,
                                        BaseType = 17,
                                        BaseLine = oDownPaymentsLines.LineNum
                                    };

                                    oDownPayments.DocumentLines.Add(docLine);
                                }

                                //Ajuste realizado para gerar corretamente o adiantamento quando o pagamento vem com Cartão Crédito e Débito em conta.
                                //Duas formas de pagto permitida no Magento. 23/06/2021 - Merge em 15/07/2021

                                if (item.base_grand_total == item.subtotal_incl_tax)
                                    //Cliente usou apenas uma forma de pgto
                                    oDownPayments.DocTotal = item.subtotal_incl_tax; 
                                else
                                    //Cliente usou duas formas de pgto.
                                    oDownPayments.DocTotal = item.base_grand_total;

                               oDownPayments = await SLConnection.PostAsync("DownPayments", oDownPayments);

                                #endregion

                                #region [ Criando o Pagamento do Adiantamento no SAP ]

                                sProcesso = "Pagamento do Adiantamento";

                                var duedate = Convert.ToDateTime(item.updated_at.Replace(" ", "").Substring(0,10)).Date;

                                var oIncomingPayment = new Metadata_IncomingPayment.IncomingPayment
                                {
                                    DocType = "rCustomer",
                                    DocDate = DateTime.Today.ToString("yyyy-MM-dd"),
                                    DueDate = duedate.ToString("yyyy-MM-dd"),
                                    TaxDate = DateTime.Today.ToString("yyyy-MM-dd"),
                                    CardCode = oDownPayments.CardCode,
                                    BPLID = oDownPayments.BPL_IDAssignedToInvoice,
                                    PaymentInvoices = new List<Metadata_IncomingPayment.Paymentinvoice>()
                                };

                                var oPaymentinvoice = new Metadata_IncomingPayment.Paymentinvoice
                                {
                                    DocEntry = (int)oDownPayments.DocEntry,
                                    InstallmentId = 1,
                                    InvoiceType = "it_DownPayment",
                                    SumApplied = (float)oDownPayments.DocTotal
                                };

                                oIncomingPayment.PaymentInvoices.Add(oPaymentinvoice);

                                switch (oPaymentMethod.method)
                                {
                                    case "banktransfer": //-- Transferência (Usada somente para testes)
                                        {
                                            oIncomingPayment.TransferAccount = CC.U_Conta;
                                            oIncomingPayment.TransferSum = oPaymentMethod.amount_ordered;
                                            break;
                                        }
                                    case "mundipagg_creditcard": //-- Cartão de Crédito
                                        {
                                            oIncomingPayment.PaymentCreditCards = new List<Metadata_IncomingPayment.Paymentcreditcard>();
                                            var objCreditCard = await SLConnection.GetAsync<List<Metadata_CreditCard.CreditCard>>("CreditCards");
                                            //if (objCreditCard == null || objCreditCard.Count == 0)
                                            //    throw new Exception($"Nenhum cartão de crédito encontrado no SAP.");

                                            var objNSU = item.extension_attributes.payment_additional_info.Where(s => s.key.Contains("cc_nsu_authorization")).FirstOrDefault();
                                            //if (objNSU == null || String.IsNullOrEmpty(objNSU.value))
                                            //    throw new Exception(String.Format(@"Campo NSU inválido."));

                                            var paymentCreditCard = new Metadata_IncomingPayment.Paymentcreditcard
                                            {
                                                CreditCard = objCreditCard.FirstOrDefault().CreditCardCode,
                                                CardValidUntil = new DateTime(Convert.ToInt32(oPaymentMethod.cc_exp_year), Convert.ToInt32(oPaymentMethod.cc_exp_month), 1).ToString("yyyy-MM-dd"),                                                
                                                CreditCardNumber = oPaymentMethod.additional_information[2].ToString(),
                                                PaymentMethodCode = 2, // Crédito,
                                                NumOfPayments = 1,
                                                VoucherNum = (objNSU.value.Length > 20 ? objNSU.value.Substring(0, 20) : objNSU.value),
                                                CreditSum = oPaymentMethod.amount_ordered
                                            };

                                            oIncomingPayment.PaymentCreditCards.Add(paymentCreditCard);
                                            break;
                                        }
                                    case "mundipagg_billet": //-- Boleto
                                        {
                                            oIncomingPayment.TransferAccount = CC.U_Conta;
                                            oIncomingPayment.TransferSum = oPaymentMethod.amount_ordered;
                                            break;
                                        }
                                    case "free": //-- Débito em Conta (pago com 100% do saldo)
                                        {
                                            oIncomingPayment.CashAccount = CC.U_Conta; //Integration.Config.CashAccount;
                                            oIncomingPayment.CashSum = oDownPayments.DocTotal;
                                            break;
                                        }
                                    case "poscash": //-- PDV
                                        {
                                            oIncomingPayment.CashAccount = CC.U_Conta; //Integration.Config.CashAccount;
                                            oIncomingPayment.CashSum = oDownPayments.DocTotal;
                                            break;
                                        }
                                }

                                oIncomingPayment = await SLConnection.PostAsync("IncomingPayments", oIncomingPayment);

                                #endregion
                            }

                            #region [ Atualizando o Magento com as informações do Pedido SAP ]

                            Util.GravarLog(sCaminho, "Atualizando o Magento com as informações do Pedido SAP ");

                            //if (!String.IsNullOrWhiteSpace(sMensagemSAP))
                            //{

                            //    //var jsonErro = JsonConvert.SerializeObject(oOrders);
                            //    //Util.GravarLog(sCaminho, String.Format(@"[ERRO] - JSON: {0}", jsonErro));
                            //    Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Pedido: {0} - Detalhes: {1}", sCodigoPedidoMagento, sMensagemSAP));
                            //}

                            string sRetorno = await SalesOrderController.UpdateSalesMagento(token, sCodigoPedidoMagento, sNumeroSAP, sMensagemSAP);

                            if (!String.IsNullOrWhiteSpace(sRetorno))
                                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Pedido: {0} - Detalhes: {1} Retorno- "+sRetorno, sCodigoPedidoMagento, sMensagemSAP));

                            #endregion

                        }
                        catch (Exception ex)
                        {
                            sMensagemSAP = String.Format(@"{0} - {1}", sProcesso, ex.Message);
                            Util.GravarLog(sCaminho, sMensagemSAP);
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.GravarLog(sCaminho, String.Format(@"[ERRO] Já integrado- Pedido: {0} - Detalhes: {1}", sCodigoPedidoMagento, ex.Message));

                        if (!ex.Message.Contains("já integrado"))
                        {
                            string sRetorno = await SalesOrderController.UpdateSalesMagento(token, sCodigoPedidoMagento, sNumeroSAP, ex.Message);
                            if (!String.IsNullOrEmpty(sRetorno))
                                Util.GravarLog(sCaminho, String.Format(@"[ERRO] Ultimo Catch - Pedido: {0} - Detalhes: {1} -"+ sRetorno, sCodigoPedidoMagento, ex.Message));
                        }
                    }

                    iTotalPedidos++;
                }
            }
            catch (FlurlHttpException ex)
            {
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex.Message));
                var responseString = await ex.Call.Response.GetStringAsync();
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - {0}", ex.Message));
            }

            Util.GravarLog(sCaminho, "[PROCESSO] - Processo finalizado.");
        }
    }
}
