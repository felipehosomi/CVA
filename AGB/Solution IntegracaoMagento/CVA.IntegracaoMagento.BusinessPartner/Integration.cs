using CVA.IntegracaoMagento.BusinessPartner.Models.Magento;
using CVA.IntegracaoMagento.BusinessPartner.Models.SAP;
using CVA.IntegracaoMagento.BusinessPartner.Controller;
using Flurl.Http;
using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CVA.IntegracaoMagento.BusinessPartner
{
    public class Integration
    {
        internal static SLConnection SLConnection;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

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
                var customer = new Customers();

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão HANA (OK)");

                token.apiAddressUri = objConfig[0].U_ApiUrl; //MagentoURL
                token.username = objConfig[0].U_ApiUsuario; //MagentoUser
                token.password = objConfig[0].U_ApiSenha; //MagentoPassword
                token.MagentoClientId = objConfig[0].U_ApiClientId; //MagentoClientId
                token.MagentoSecretId = objConfig[0].U_ApiClientSecret; //MagentoClientSecret
              
                Token.create_CN(token);

                if (String.IsNullOrEmpty(token.bearerTolken))
                    throw new Exception("Bearer Token não gerado.");

                Util.GravarLog(sCaminho, "[PROCESSO] - Conexão Magento (OK)");

                var businessPartner = new BusinessPartners();
                var businessPartnerMD = await SLConnection.GetAsync<List<Metadata_BusinessPartners.BusinessPartner>>("sml.svc/CVAMAGENTOBP");
                if (businessPartnerMD.Count > 0)
                {
                    //Token.create_CN(token);
                    foreach (var item in businessPartnerMD)
                    {
                        customer.customer = new Customers.Customer();
                        customer.customer.dob = item.U_datadenascimento;
                        customer.customer.email = item.E_Mail;
                        string cardNameFirst = "";
                        string cardNameLast = "";

                        if (item.CardName.Length > item.CardName.Replace(" ", "").Length)
                        {
                            cardNameFirst = item.CardName.Substring(0, item.CardName.IndexOf(" "));
                            cardNameLast = item.CardName.Substring(item.CardName.IndexOf(" ") + 1);
                            customer.customer.firstname = cardNameFirst;
                            customer.customer.lastname = cardNameLast;
                        }
                        else
                            customer.customer.firstname = item.CardName;

                        if (item.Gender == "F")
                            customer.customer.gender = 2;
                        else
                            customer.customer.gender = 1;

                        customer.customer.taxvat = item.TaxId4.Replace(".", String.Empty).Replace("/", String.Empty).Replace("-", String.Empty);
                        customer.customer.group_id = 1;

                        //Endereço de entrega
                        IList<string> logradouro = new List<string>();
                        logradouro.Add(item.AddrType.ToString() + " " + item.Street.ToString());
                        logradouro.Add(item.StreetNo.ToString());
                        logradouro.Add(item.Block.ToString());
                        var regionAddress = new Customers.Customer.Region();
                        regionAddress.regionCode = item.State;
                        switch (item.State)
                        {
                            case "AC":
                                regionAddress.region = "Acre";
                                regionAddress.regionId = 485;
                                break;
                            case "AL":
                                regionAddress.region = "Alagoas";
                                regionAddress.regionId = 486;
                                break;
                            case "AP":
                                regionAddress.region = "Amapá";
                                regionAddress.regionId = 487;
                                break;
                            case "AM":
                                regionAddress.region = "Amazonas";
                                regionAddress.regionId = 488;
                                break;
                            case "BA":
                                regionAddress.region = "Bahia";
                                regionAddress.regionId = 489;
                                break;
                            case "CE":
                                regionAddress.region = "Ceará";
                                regionAddress.regionId = 490;
                                break;
                            case "ES":
                                regionAddress.region = "Espírito Santo";
                                regionAddress.regionId = 491;
                                break;
                            case "GO":
                                regionAddress.region = "Goiás";
                                regionAddress.regionId = 492;
                                break;
                            case "MA":
                                regionAddress.region = "Maranhão";
                                regionAddress.regionId = 493;
                                break;
                            case "MT":
                                regionAddress.region = "Mato Grosoo";
                                regionAddress.regionId = 494;
                                break;
                            case "MS":
                                regionAddress.region = "Mato Grosso do Sul";
                                regionAddress.regionId = 495;
                                break;
                            case "MG":
                                regionAddress.region = "Minas Gerais";
                                regionAddress.regionId = 496;
                                break;
                            case "PA":
                                regionAddress.region = "Pará";
                                regionAddress.regionId = 497;
                                break;
                            case "PB":
                                regionAddress.region = "Paraíba";
                                regionAddress.regionId = 498;
                                break;
                            case "PR":
                                regionAddress.region = "Paraná";
                                regionAddress.regionId = 499;
                                break;
                            case "PE":
                                regionAddress.region = "Pernambuco";
                                regionAddress.regionId = 500;
                                break;
                            case "PI":
                                regionAddress.region = "Piauí";
                                regionAddress.regionId = 501;
                                break;
                            case "RJ":
                                regionAddress.region = "Rio de Janeiro";
                                regionAddress.regionId = 502;
                                break;
                            case "RN":
                                regionAddress.region = "Rio Grande do Norte";
                                regionAddress.regionId = 503;
                                break;
                            case "RS":
                                regionAddress.region = "Rio Grande do Sul";
                                regionAddress.regionId = 504;
                                break;
                            case "RO":
                                regionAddress.region = "Rondônia";
                                regionAddress.regionId = 505;
                                break;
                            case "RR":
                                regionAddress.region = "Roraima";
                                regionAddress.regionId = 506;
                                break;
                            case "SC":
                                regionAddress.region = "Santa Catarina";
                                regionAddress.regionId = 507;
                                break;
                            case "SP":
                                regionAddress.region = "São Paulo";
                                regionAddress.regionId = 508;
                                break;
                            case "SE":
                                regionAddress.region = "Sergipe";
                                regionAddress.regionId = 509;
                                break;
                            case "TO":
                                regionAddress.region = "Tocantins";
                                regionAddress.regionId = 510;
                                break;
                            case "DF":
                                regionAddress.region = "Distrito Federal";
                                regionAddress.regionId = 511;
                                break;
                        }
                        customer.customer.addresses = new List<Customers.Customer.Address>();
                        customer.customer.addresses.Add(new Customers.Customer.Address
                        {
                            defaultShipping = true,
                            defaultBilling = false,
                            firstname = cardNameFirst,
                            lastname = cardNameLast,
                            countryId = item.Country,
                            region = regionAddress,
                            postcode = item.ZipCode,
                            street = logradouro,
                            city = item.City,
                            telephone = item.Phone2 + item.Phone1,
                            fax = item.Cellular,
                            country_id = item.Country,
                            vat_id = item.TaxId4.Replace(".", String.Empty).Replace("/", String.Empty).Replace("-", String.Empty)
                        });

                        //Endereço de cobrança
                        IList<string> coblogradouro = new List<string>();
                        coblogradouro.Add(item.cobAddrType.ToString() + " " + item.cobStreet.ToString());
                        coblogradouro.Add(item.cobStreetNo.ToString());
                        coblogradouro.Add(item.cobBlock.ToString());
                        var cobregionAddress = new Customers.Customer.Region();
                        cobregionAddress.regionCode = item.cobState;
                        switch (item.cobState)
                        {
                            case "AC":
                                cobregionAddress.region = "Acre";
                                cobregionAddress.regionId = 485;
                                break;
                            case "AL":
                                cobregionAddress.region = "Alagoas";
                                cobregionAddress.regionId = 486;
                                break;
                            case "AP":
                                cobregionAddress.region = "Amapá";
                                cobregionAddress.regionId = 487;
                                break;
                            case "AM":
                                cobregionAddress.region = "Amazonas";
                                cobregionAddress.regionId = 488;
                                break;
                            case "BA":
                                cobregionAddress.region = "Bahia";
                                cobregionAddress.regionId = 489;
                                break;
                            case "CE":
                                cobregionAddress.region = "Ceará";
                                cobregionAddress.regionId = 490;
                                break;
                            case "ES":
                                cobregionAddress.region = "Espírito Santo";
                                cobregionAddress.regionId = 491;
                                break;
                            case "GO":
                                cobregionAddress.region = "Goiás";
                                cobregionAddress.regionId = 492;
                                break;
                            case "MA":
                                cobregionAddress.region = "Maranhão";
                                cobregionAddress.regionId = 493;
                                break;
                            case "MT":
                                cobregionAddress.region = "Mato Grosoo";
                                cobregionAddress.regionId = 494;
                                break;
                            case "MS":
                                cobregionAddress.region = "Mato Grosso do Sul";
                                cobregionAddress.regionId = 495;
                                break;
                            case "MG":
                                cobregionAddress.region = "Minas Gerais";
                                cobregionAddress.regionId = 496;
                                break;
                            case "PA":
                                cobregionAddress.region = "Pará";
                                cobregionAddress.regionId = 497;
                                break;
                            case "PB":
                                cobregionAddress.region = "Paraíba";
                                cobregionAddress.regionId = 498;
                                break;
                            case "PR":
                                cobregionAddress.region = "Paraná";
                                cobregionAddress.regionId = 499;
                                break;
                            case "PE":
                                cobregionAddress.region = "Pernambuco";
                                cobregionAddress.regionId = 500;
                                break;
                            case "PI":
                                cobregionAddress.region = "Piauí";
                                cobregionAddress.regionId = 501;
                                break;
                            case "RJ":
                                cobregionAddress.region = "Rio de Janeiro";
                                cobregionAddress.regionId = 502;
                                break;
                            case "RN":
                                cobregionAddress.region = "Rio Grande do Norte";
                                cobregionAddress.regionId = 503;
                                break;
                            case "RS":
                                cobregionAddress.region = "Rio Grande do Sul";
                                cobregionAddress.regionId = 504;
                                break;
                            case "RO":
                                cobregionAddress.region = "Rondônia";
                                cobregionAddress.regionId = 505;
                                break;
                            case "RR":
                                cobregionAddress.region = "Roraima";
                                cobregionAddress.regionId = 506;
                                break;
                            case "SC":
                                cobregionAddress.region = "Santa Catarina";
                                cobregionAddress.regionId = 507;
                                break;
                            case "SP":
                                cobregionAddress.region = "São Paulo";
                                cobregionAddress.regionId = 508;
                                break;
                            case "SE":
                                cobregionAddress.region = "Sergipe";
                                cobregionAddress.regionId = 509;
                                break;
                            case "TO":
                                cobregionAddress.region = "Tocantins";
                                cobregionAddress.regionId = 510;
                                break;
                            case "DF":
                                cobregionAddress.region = "Distrito Federal";
                                cobregionAddress.regionId = 511;
                                break;
                        }
                        customer.customer.addresses.Add(new Customers.Customer.Address
                        {
                            defaultShipping = false,
                            defaultBilling = true,
                            firstname = cardNameFirst,
                            lastname = cardNameLast,
                            countryId = item.cobCountry,
                            region = cobregionAddress,
                            postcode = item.cobZipCode,
                            street = coblogradouro,
                            city = item.cobCity,
                            telephone = item.Phone2 + item.Phone1,
                            fax = item.Cellular,
                            country_id = item.cobCountry,
                            vat_id = item.TaxId4.Replace(".", String.Empty).Replace("/", String.Empty).Replace("-", String.Empty)
                        });

                        customer.customer.custom_attributes = new List<Customers.Customer.Custom_Attribute>();

                        string sSaldo = item.Balance.ToString();
                        sSaldo = sSaldo.Replace(',', '.');
                        customer.customer.custom_attributes.Add(new Customers.Customer.Custom_Attribute
                        {
                            attribute_code = "saldo",
                            value = sSaldo
                        });
                        if (item.Active == "Y")
                        {
                            customer.customer.custom_attributes.Add(new Customers.Customer.Custom_Attribute
                            {
                                attribute_code = "is_disabled",
                                value = "0"
                            });
                        }
                        else
                        {
                            customer.customer.custom_attributes.Add(new Customers.Customer.Custom_Attribute
                            {
                                attribute_code = "is_disabled",
                                value = "1"
                            });
                        }
                        customer.customer.codigo_cliente = item.CardCode;
                        customer.customer.website_id = 1;
                        businessPartner.CardCode = item.CardCode;
                        string sMensagemErro = String.Empty;

                        //Util.GravarLog(sCaminho, String.Format(@"[PN] - CardCode: {0}", item.CardCode));

                        if (string.IsNullOrEmpty(item.MagentoId))
                        {
                            businessPartner.U_Magento_Id = BusinessPartnersController.create_CUSTOMER(token, customer, ref sMensagemErro);

                            //var jsonBP = JsonConvert.SerializeObject(businessPartner);
                            //Util.GravarLog(sCaminho, String.Format(@"[JSON] - Business Partner: {0}", jsonBP));

                            if (!String.IsNullOrEmpty(sMensagemErro))
                            {
                                var jsonCustomer = JsonConvert.SerializeObject(customer);
                                Util.GravarLog(sCaminho, String.Format(@"[JSON] - Customer: {0}", jsonCustomer));
                                Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Cadastro: {0} - {1}", item.CardCode, sMensagemErro));
                            }
                            else
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(businessPartner.U_Magento_Id))
                                    {
                                        Util.GravarLog(sCaminho, "[PROCESSO] - Cadastro criado: " + item.CardCode + " - MagentoId: " + businessPartner.U_Magento_Id);
                                        await SLConnection.PatchAsync($"BusinessPartners('{businessPartner.CardCode}')", businessPartner);
                                    } 
                                }
                                catch (Exception ex)
                                {
                                    Util.GravarLog(sCaminho, String.Format(@"[ERRO] - Erro ao atualizar o Parceiro no SAP. Detalhes: '{0}'", ex.Message));
                                }   
                            }
                        }
                        else
                        {
                            customer.customer.id = int.Parse(item.MagentoId);
                            string sRetorno = BusinessPartnersController.update_CUSTOMER(token, customer);
                            Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - Cadastro: {0} - MagentoID: {1} {2}", item.CardCode, item.MagentoId, (sRetorno.Contains("erro") ? String.Format(@"- Erro: {0}", sRetorno) : String.Empty)));
                        }
                    }
                }
            }
            catch (FlurlHttpException ex)
            {
                var responseString = await ex.Call.Response.GetStringAsync();
                Util.GravarLog(sCaminho, "[ERRO] - " + responseString);
            }
            catch (Exception ex)
            {
                Util.GravarLog(sCaminho, "[ERRO] - " + ex.Message);
            }

            Util.GravarLog(sCaminho, "[PROCESSO] - Processo finalizado.");
        }
    }
}
