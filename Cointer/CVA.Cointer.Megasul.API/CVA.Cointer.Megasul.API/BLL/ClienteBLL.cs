using CVA.Cointer.Megasul.API.DAO;
using CVA.Cointer.Megasul.API.DAO.Resources;
using CVA.Cointer.Megasul.API.Models;
using CVA.Cointer.Megasul.API.Models.ServiceLayer;
using SBO.Hub.SBOHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVA.Cointer.Megasul.API.BLL
{
    public class ClienteBLL
    {
        public ClienteResponseModel Insert(ClienteModel clienteModel)
        {
            HanaDAO hanaDAO = new HanaDAO();
            ClienteResponseModel responseModel = new ClienteResponseModel();
            ServiceLayer serviceLayer = new ServiceLayer();

            responseModel.clientes = new List<ClienteResponse>();

            foreach (var item in clienteModel.clientes)
            {
                string error = String.Empty;
                string cardCode = hanaDAO.ExecuteScalar(Hana.Cliente_GetMaxCode).ToString();
                cardCode = "C" + (Convert.ToInt32(cardCode.Substring(1)) + 1).ToString().PadLeft(cardCode.Length - 1, '0');
                try
                {
                    item.cnpj_cpf = FormatCpfCnpj(item.cnpj_cpf);

                    ParceiroNegocioModel parceiroNegocioModel = new ParceiroNegocioModel();
                    parceiroNegocioModel.Series = 1;
                    parceiroNegocioModel.CardCode = cardCode;
                    parceiroNegocioModel.CardType = "cCustomer";
                    parceiroNegocioModel.CardName = item.razao_social;
                    parceiroNegocioModel.CardForeignName = item.nome;
                    parceiroNegocioModel.EmailAddress = item.email;
                    parceiroNegocioModel.Cellular = item.celular;
                    parceiroNegocioModel.Phone1 = item.telefone;
                    parceiroNegocioModel.GroupCode = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["GrupoCliente"]);

                    //if (item.telefone != null)
                    //{
                    //    if (item.telefone.Contains("("))
                    //    {
                    //        parceiroNegocioModel.Phone1 = item.telefone.Substring(0, 4);
                    //        parceiroNegocioModel.Phone2 = item.telefone.Substring(4);
                    //    }
                    //    else
                    //    {
                    //        parceiroNegocioModel.Phone2 = item.telefone;
                    //    }
                    //}

                    parceiroNegocioModel.BPAddresses = new List<Bpaddress>();
                    Bpaddress enderecoCobranca = new Bpaddress();
                    enderecoCobranca.AddressName = "Cobrança";
                    enderecoCobranca.AddressType = "bo_BillTo";

                    object cidadeId = hanaDAO.ExecuteScalar(String.Format(Hana.Cidade_Get, item.codigo_municipio));
                    if (cidadeId == null)
                    {
                        throw new Exception($"Cód. IBGE {item.codigo_municipio} não encontrado");
                    }
                    enderecoCobranca.City = item.cidade;
                    enderecoCobranca.County = cidadeId.ToString();
                    enderecoCobranca.State = item.uf;

                    object paisId = hanaDAO.ExecuteScalar(String.Format(Hana.Pais_Get, item.codigo_pais));
                    if (paisId == null)
                    {
                        throw new Exception($"Cód. país {item.codigo_pais} não encontrado");
                    }
                    enderecoCobranca.Country = paisId.ToString();

                    enderecoCobranca.Street = item.endereco;
                    enderecoCobranca.StreetNo = item.numero;
                    enderecoCobranca.Block = item.bairro;
                    enderecoCobranca.BuildingFloorRoom = item.complemento;
                    enderecoCobranca.ZipCode = item.cep;

                    parceiroNegocioModel.BPAddresses.Add(enderecoCobranca);

                    Bpaddress enderecoEntrega = new Bpaddress();
                    enderecoEntrega.AddressName = "Destinatário";
                    enderecoEntrega.AddressType = "bo_ShipTo";
                    enderecoEntrega.City = item.cidade;
                    enderecoEntrega.County = cidadeId.ToString();
                    enderecoEntrega.State = item.uf;
                    enderecoEntrega.Country = paisId.ToString();
                    enderecoEntrega.Street = item.endereco;
                    enderecoEntrega.StreetNo = item.numero;
                    enderecoEntrega.Block = item.bairro;
                    enderecoEntrega.BuildingFloorRoom = item.complemento;
                    enderecoEntrega.ZipCode = item.cep;

                    parceiroNegocioModel.BPAddresses.Add(enderecoEntrega);

                    parceiroNegocioModel.BPFiscalTaxIDCollection = new List<Bpfiscaltaxidcollection>();
                    Bpfiscaltaxidcollection taxEmpty = new Bpfiscaltaxidcollection();
                    taxEmpty.AddrType = "bo_ShipTo";
                    taxEmpty.Address = "";
                    if (item.tipo_pessoa == "F")
                    {
                        taxEmpty.TaxId4 = item.cnpj_cpf;
                        taxEmpty.TaxId1 = "";
                    }
                    else
                    {
                        taxEmpty.TaxId0 = item.cnpj_cpf;
                        taxEmpty.TaxId1 = item.ie_rg;
                        taxEmpty.TaxId3 = item.im;
                    }
                    parceiroNegocioModel.BPFiscalTaxIDCollection.Add(taxEmpty);

                    Bpfiscaltaxidcollection taxCobranca = new Bpfiscaltaxidcollection();
                    taxCobranca.AddrType = "bo_BillTo";
                    taxCobranca.Address = "Cobrança";
                    if (item.tipo_pessoa == "F")
                    {
                        taxCobranca.TaxId4 = item.cnpj_cpf;
                        taxCobranca.TaxId1 = "Isento";
                    }
                    else
                    {
                        taxCobranca.TaxId0 = item.cnpj_cpf;
                        taxCobranca.TaxId1 = item.ie_rg;
                        taxCobranca.TaxId3 = item.im;
                    }
                    parceiroNegocioModel.BPFiscalTaxIDCollection.Add(taxCobranca);

                    Bpfiscaltaxidcollection taxEntrega = new Bpfiscaltaxidcollection();
                    taxEntrega.AddrType = "bo_ShipTo";
                    taxEntrega.Address = "Destinatário";
                    if (item.tipo_pessoa == "F")
                    {
                        taxEntrega.TaxId4 = item.cnpj_cpf;
                        taxEntrega.TaxId1 = "Isento";
                    }
                    else
                    {
                        taxEntrega.TaxId0 = item.cnpj_cpf;
                        taxEntrega.TaxId1 = item.ie_rg;
                        taxEntrega.TaxId3 = item.im;
                    }
                    parceiroNegocioModel.BPFiscalTaxIDCollection.Add(taxEntrega);

                    error = serviceLayer.Post<ParceiroNegocioModel>("BusinessPartners", parceiroNegocioModel);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    ClienteResponse clienteResponse = new ClienteResponse();
                    clienteResponse.codigo_megasul = item.codigo_megasul;
                    clienteResponse.codigo_sap = !String.IsNullOrEmpty(error) ? "" : cardCode;
                    clienteResponse.erro = !String.IsNullOrEmpty(error);
                    clienteResponse.msgDeRetorno = error;
                    responseModel.clientes.Add(clienteResponse);
                }
            }

            return responseModel;
        }

        public ClienteResponseModel Update(ClienteModel clienteModel)
        {
            HanaDAO hanaDAO = new HanaDAO();
            ClienteResponseModel responseModel = new ClienteResponseModel();
            ServiceLayer serviceLayer = new ServiceLayer();

            responseModel.clientes = new List<ClienteResponse>();

            foreach (var item in clienteModel.clientes)
            {
                string error = String.Empty;
                try
                {
                    item.cnpj_cpf = FormatCpfCnpj(item.cnpj_cpf);

                    ParceiroNegocioModel parceiroNegocioModel = serviceLayer.GetByID<ParceiroNegocioModel>("BusinessPartners", item.codigo_sap);
                    parceiroNegocioModel.CardName = item.razao_social;
                    parceiroNegocioModel.CardForeignName = item.nome;
                    parceiroNegocioModel.EmailAddress = item.email;
                    parceiroNegocioModel.Cellular = item.celular;
                    parceiroNegocioModel.Phone1 = item.telefone;
                    parceiroNegocioModel.GroupCode = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["GrupoCliente"]);

                    object cidadeId = hanaDAO.ExecuteScalar(String.Format(Hana.Cidade_Get, item.codigo_municipio));
                    if (cidadeId == null)
                    {
                        throw new Exception($"Cód. IBGE {item.codigo_municipio} não encontrado");
                    }
                    object paisId = hanaDAO.ExecuteScalar(String.Format(Hana.Pais_Get, item.codigo_pais));
                    if (paisId == null)
                    {
                        throw new Exception($"Cód. país {item.codigo_pais} não encontrado");
                    }

                    foreach (var endereco in parceiroNegocioModel.BPAddresses)
                    {
                        endereco.City = item.cidade;
                        endereco.County = cidadeId.ToString();
                        endereco.State = item.uf;
                        endereco.Country = paisId.ToString();
                        endereco.Street = item.endereco;
                        endereco.StreetNo = item.numero;
                        endereco.Block = item.bairro;
                        endereco.ZipCode = item.cep;
                        endereco.BuildingFloorRoom = item.complemento;
                    }

                    foreach (var cpfCnpj in parceiroNegocioModel.BPFiscalTaxIDCollection)
                    {
                        if (item.tipo_pessoa == "F")
                        {
                            cpfCnpj.TaxId4 = item.cnpj_cpf;
                            cpfCnpj.TaxId0 = "";
                            cpfCnpj.TaxId1 = "";
                            cpfCnpj.TaxId3 = "";
                        }
                        else
                        {
                            cpfCnpj.TaxId0 = item.cnpj_cpf;
                            cpfCnpj.TaxId1 = item.ie_rg;
                            cpfCnpj.TaxId3 = item.im;
                            cpfCnpj.TaxId4 = "";
                        }
                    }

                    error = serviceLayer.Patch<ParceiroNegocioModel>("BusinessPartners", item.codigo_sap, parceiroNegocioModel);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                finally
                {
                    ClienteResponse clienteResponse = new ClienteResponse();
                    clienteResponse.codigo_megasul = item.codigo_megasul;
                    clienteResponse.codigo_sap = !String.IsNullOrEmpty(error) ? "" : item.codigo_sap;
                    clienteResponse.erro = !String.IsNullOrEmpty(error);
                    clienteResponse.msgDeRetorno = error;
                    responseModel.clientes.Add(clienteResponse);
                }
            }

            return responseModel;
        }

        private string FormatCpfCnpj(string cpfCnpj)
        {
            cpfCnpj = cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cpfCnpj.Length == 11)
            {
                return Convert.ToUInt64(cpfCnpj).ToString(@"000\.000\.000\-00");
            }
            else
            {
                return Convert.ToUInt64(cpfCnpj).ToString(@"00\.000\.000\/0000\-00");
            }
        }
    }
}