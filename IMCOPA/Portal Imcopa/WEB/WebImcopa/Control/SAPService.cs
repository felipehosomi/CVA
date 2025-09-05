using System;
using System.Data;
using System.Configuration;
using ImcopaWEB;
using SAP.Connector;
using System.ComponentModel;
using System.Text;
using System.Net.Mail;
using WebImcopa.MODEL;
using System.Collections.Generic;

namespace WebImcopa.control
{
    public class SAPService
    {
        public string[] Quotations_Create(ZSDE007 i_Header, ZSDE008Table t_Item, ZSDE009Table t_Obs)
        {
            string[] retorno = new string[2];

            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            try
            {
                ImcopaWEB.BAPIRET2Table t_Return = new ImcopaWEB.BAPIRET2Table();
                BAPISDITMTable t_Return2 = new BAPISDITMTable();
                string iDoc = string.Empty;

                retorno[0] = "1";
                retorno[1] = string.Empty;

                sapProxy.Timeout = 120000;
                //ZSDE027 teste = new ZSDE027();
                sapProxy.Connection.Open();
                //teste.
                sapProxy.Zsd_Quotations_Create(i_Header, out iDoc, ref t_Item, ref t_Obs, ref t_Return, ref t_Return2);
                //sapProxy.Zpwi_Customer_Create();
                retorno[1] = iDoc;
                for (int i = 0; i < t_Return.Count; i++)
                {
                    if (t_Return[i].Type.ToString().Equals("E"))
                    {
                        retorno[0] = "0";
                        retorno[1] = t_Return[i].Message.ToString();
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                sapProxy.Connection.Close();
            }
            return retorno;
        }

        public string[] Quotations_Delete(string i_Cotacao)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            DataTable dt_Erros = new DataTable();
            string[] retorno = new string[2];
            try
            {
                BDCMSGCOLLTable t_erros = new BDCMSGCOLLTable();

                retorno[0] = "1";
                retorno[1] = string.Empty;

                sapProxy.Timeout = 120000;
                sapProxy.Connection.Open();

                sapProxy.Zsd_Quotations_Delete(i_Cotacao, ref t_erros);

                for (int i = 0; i < t_erros.Count; i++)
                {
                    if (t_erros[i].Msgtyp.ToString().Equals("E"))
                    {
                        retorno[0] = "0";
                        retorno[1] = t_erros[i].Msgv1.ToString() + " - " + i_Cotacao;
                        break;
                    }
                }
                return retorno;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
        }

        public DataTable Quotations_Get_Customer(string cliente, string codrep, string cnpj, string cpf)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            ZSDE011Table lt_Customer = new ZSDE011Table();
            DataTable dt_Customer = new DataTable();
            try
            {
                sapProxy.Timeout = 120000;
                sapProxy.Connection.Open();

                sapProxy.Zsd_Quotations_Get_Customer(cnpj, cpf, cliente, codrep, ref lt_Customer);
                dt_Customer = lt_Customer.ToADODataTable();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
            return dt_Customer;
        }

        public DataTable Quotations_Get_Pay_Cond()
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            ZSDE010Table lt_Condicoes = new ZSDE010Table();
            DataTable dt_CondicoesPag = new DataTable();
            try
            {
                sapProxy.Timeout = 120000;
                sapProxy.Connection.Open();
                sapProxy.Zsd_Quotations_Get_Pay_Cond(ref lt_Condicoes);
                dt_CondicoesPag = lt_Condicoes.ToADODataTable();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
            return dt_CondicoesPag;
        }

        public object[] Quotations_Tracking(string corretor, string cotacao, string dtfim, string dtini, string cliente, string grupo)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            string cnpj = string.Empty;
            object[] objRetorno = new object[2];
            DataTable dt_Cotacoes = new DataTable();

            ZSDE012Table lt_cotacoes = new ZSDE012Table();
            //VBFATable lt_faturas = new VBFATable();
            //DataTable dt_Tracking = new DataTable();
            try
            {
                sapProxy.Connection.Open();
                sapProxy.Timeout = 120000;
                sapProxy.Zsd_Quotations_Tracking(cnpj, corretor, cotacao, dtfim, dtini, cliente, grupo, ref lt_cotacoes);
                dt_Cotacoes = lt_cotacoes.ToADODataTable();

                dt_Cotacoes.Columns.Add(new DataColumn("FAT", typeof(bool)));
                dt_Cotacoes.Columns.Add(new DataColumn("CRD", typeof(bool)));
                foreach (DataRow row in dt_Cotacoes.Rows)
                {
                    row["KUNNR"] = Convert.ToInt32(row["KUNNR"].ToString());
                    row["NAME1"] = row["NAME1"].ToString().ToUpper();
                    row["ORT01"] = row["ORT01"].ToString().ToUpper();
                    row["DZTERM"] = row["DZTERM"].ToString().ToUpper();
                    row["DTFAT"] = formataData(row["DTFAT"].ToString());
                    row["ZFBDT"] = formataData(row["ZFBDT"].ToString());
                    row["ZFBDT2"] = formataData(row["ZFBDT2"].ToString());
                    row["ZFBDT3"] = formataData(row["ZFBDT3"].ToString());
                    row["DCVEN"] = formataValor(row["DCVEN"].ToString());
                    row["MATNR"] = formataValor(row["MATNR"].ToString());
                    row["DCCOT"] = formataValor(row["DCCOT"].ToString());
                    row["DCNF"] = formataValor(row["DCNF"].ToString());
                    row["FAT"] = Convert.ToInt32(row["BLFAT"]);
                    row["CRD"] = Convert.ToInt32(row["BLCRED"]);
                }
                //dt_Tracking = lt_cotacoes.ToADODataTable();
                //objRetorno[0] = lt_faturas.ToADODataTable();
                objRetorno[0] = dt_Cotacoes;

            }
            catch
            {
                throw;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
            return objRetorno;
        }

        public string[] Pesquisar_Cliente(ZSDE027 model)
        {
            var result = new string[2];
            var sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);

            try
            {
                var t_Return = new BAPIRET1Table();


                sapProxy.Timeout = 240000;
                sapProxy.Connection.Open();

                sapProxy.Zpwi_Customer_Check(model, ref t_Return);
                sapProxy.Connection.Close();

                if (t_Return.Count > 0)
                {
                    result[0] = "0";
                    result[1] = t_Return[0].Message.ToString();
                    return result;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                result[0] = "0";
                result[1] = ex.Message;
                sapProxy.Connection.Close();
                return result;
            }
        }

        public object[] Quotations_Tracking_Detail(string corretor, string cotacao)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            string pcond = string.Empty;
            ZSDE011Table lt_cabecalho = new ZSDE011Table();
            ZSDE014Table lt_itens = new ZSDE014Table();

            DataTable dt_Cabecalho = new DataTable();
            DataTable dt_Itens = new DataTable();
            object[] objRetorno = new object[3];
            try
            {
                sapProxy.Connection.Open();
                sapProxy.Timeout = 120000;
                sapProxy.Zsd_Quotations_Tracking_Detail(corretor, cotacao, out pcond, ref lt_cabecalho, ref lt_itens);

                dt_Cabecalho = lt_cabecalho.ToADODataTable();
                dt_Itens = lt_itens.ToADODataTable();
                objRetorno[0] = dt_Cabecalho;
                objRetorno[1] = dt_Itens;
                objRetorno[2] = pcond;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
            return objRetorno;
        }

        public object[] Quotations_Get_Materials(string cliente)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            ZSDE005Table lt_GrMercadorias = new ZSDE005Table();
            ZSDE006Table lt_Materiais = new ZSDE006Table();
            ImcopaWEB.BAPIRET2Table lt_return2 = new ImcopaWEB.BAPIRET2Table();
            DataTable dt_GrMercadorias = new DataTable();
            DataTable dt_Materiais = new DataTable();
            DataTable dt_Retorno = new DataTable();
            object[] objRetorno = new object[3];
            try
            {
                sapProxy.Connection.Open();
                sapProxy.Timeout = 120000;
                sapProxy.Zsd_Quotations_Get_Materials(cliente, ref lt_GrMercadorias, ref lt_Materiais, ref lt_return2);

                dt_GrMercadorias = lt_GrMercadorias.ToADODataTable();
                dt_Materiais = lt_Materiais.ToADODataTable();
                dt_Retorno = lt_return2.ToADODataTable();
                objRetorno[0] = dt_GrMercadorias;
                foreach (DataRow row in dt_Materiais.Rows)
                {
                    row["MATNR"] = formataValor(row["MATNR"].ToString());
                }
                objRetorno[1] = dt_Materiais;
                objRetorno[2] = dt_Retorno;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
            return objRetorno;
        }

        public List<CondicaoPagamentoModel> Buscar_CondicoesPgto(ZSDE027 I_Zsde027)
        {
            var sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            var I_Zsde046 = new ZSDE046();
            var t_Return = new ZSDE045Table();
            var modelList = new List<CondicaoPagamentoModel>();

            try
            {
                sapProxy.Connection.Open();
                sapProxy.Timeout = 120000;

                sapProxy.Zpwi_Customer_Condpag_Ret(I_Zsde027, out I_Zsde046, ref t_Return);

                for (int i = 0; i < t_Return.Count; i++)
                {
                    var model = new CondicaoPagamentoModel
                    {
                        Code = t_Return[i].Zterm,
                        Desc = t_Return[i].Text1,
                        CodeFormaPgto = I_Zsde046.T_Cod_Cli
                    };

                    modelList.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
            return modelList;
        }

        public object[] Quotations_Fornecedor(string codrep, int infad)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            string nome_repr = string.Empty;
            string email_repr = string.Empty;
            string bloqueado = string.Empty;
            object[] objRetorno = new object[7];
            ZSDE030Table t_Zsde030 = new ZSDE030Table();
            ZSDE031Table t_Zsde031 = new ZSDE031Table();
            ZSDE032Table t_Zsde032 = new ZSDE032Table();
            ZSDE033Table t_Zsde033 = new ZSDE033Table();
            ZSDE034Table t_Zsde034 = new ZSDE034Table();
            try
            {
                sapProxy.Connection.Open();
                sapProxy.Timeout = 120000;
                sapProxy.Zsd_Quotations_Get_Fornecedor(infad, codrep, out bloqueado, out email_repr, out nome_repr, ref t_Zsde030, ref t_Zsde031, ref t_Zsde032, ref t_Zsde033, ref t_Zsde034);
                objRetorno[0] = codrep;
                objRetorno[1] = nome_repr;
                if (infad == 1)
                {
                    objRetorno[2] = t_Zsde030.ToADODataTable();
                    objRetorno[3] = t_Zsde031.ToADODataTable();
                    objRetorno[4] = t_Zsde032.ToADODataTable();
                    objRetorno[5] = t_Zsde033.ToADODataTable();
                }
                else if (infad == 2)
                {
                    objRetorno[6] = t_Zsde034.ToADODataTable();
                }
                else if (infad == 0)
                {
                    objRetorno[2] = email_repr;
                    objRetorno[3] = bloqueado;
                }
                //else if (infad == 1000 || infad == 2000)
                //{
                //    objRetorno[6] = t_Zsde034.ToADODataTable();
                //}


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
            return objRetorno;
        }

        public string[] Salvar_Cliente(ZSDE027 model)
        {
            string[] retorno = new string[2];
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            try
            {
                BAPIRET1Table t_Return = new ImcopaWEB.BAPIRET1Table();
                string e_Kunnr = string.Empty;

                sapProxy.Timeout = 240000;
                sapProxy.Connection.Open();

                sapProxy.Zpwi_Customer_Create(model, out e_Kunnr, ref t_Return);

                retorno[0] = "1";
                retorno[1] = "Cliente " + e_Kunnr + " cadastrado com sucesso";

                for (int i = 0; i < t_Return.Count; i++)
                {
                    if (t_Return[i].Type.ToString().Equals("E"))
                    {
                        retorno[0] = "0";
                        retorno[1] = t_Return[i].Message.ToString() + "Cliente: " + e_Kunnr;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno[0] = "0";
                retorno[1] = ex.Message;
            }
            finally
            {
                sapProxy.Connection.Close();
            }
            return retorno;
        }

        public DataTable List_Customer(ZSDE028 i_Zsde028, int Open_Doc)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            BAPIRET1Table t_Return = new ImcopaWEB.BAPIRET1Table();
            ZSDE029Table t_Zsde029 = new ZSDE029Table();
            ZSDE036Table t_Zsde036 = new ZSDE036Table();
            DataTable dt_Retorno = new DataTable();
            try
            {
                //sapProxy.Timeout = 240000;
                sapProxy.Connection.Open();

                sapProxy.Zpwi_Customer_List(Open_Doc, i_Zsde028, ref t_Return, ref t_Zsde029, ref t_Zsde036);

                if (Open_Doc == 1)
                {
                    dt_Retorno = t_Zsde036.ToADODataTable();
                    dt_Retorno.Columns.Add(new DataColumn("ZFBDT2", typeof(DateTime)));
                    foreach (DataRow row in dt_Retorno.Rows)
                    {
                        row["STCD1"] = mascaraCNPJ(row["STCD1"].ToString());
                        row["NAME1"] = row["NAME1"].ToString().ToUpper();
                        //row["XBLNR"] = formataValor(row["XBLNR"].ToString());
                        row["ZFBDT2"] = formataData(row["ZFBDT"].ToString());
                        //row["DMBTR"] = formataValor(row["DMBTR"].ToString());
                        row["SGTXT"] = row["SGTXT"].ToString().ToUpper();
                    }
                }
                else if (Open_Doc == 0)
                {
                    dt_Retorno = t_Zsde029.ToADODataTable();
                    foreach (DataRow row in dt_Retorno.Rows)
                    {
                        row["BEZEI"] = row["BEZEI"].ToString().ToUpper();
                        row["NAME1"] = row["NAME1"].ToString().ToUpper();
                        row["ERDAT"] = formataData(row["ERDAT"].ToString());
                        row["KLIMK"] = formataValor(row["KLIMK"].ToString());
                        row["SKFOR"] = formataValor(row["SKFOR"].ToString());
                        row["SAUFT"] = formataValor(row["SAUFT"].ToString());
                        row["SALDO"] = formataValor(row["SALDO"].ToString());
                        row["STCD1"] = mascaraCNPJ(row["STCD1"].ToString());
                        row["VTEXT"] = cut(row["VTEXT"].ToString(), 15);
                        row["ORT02"] = cut(row["ORT02"].ToString(), 20);
                    }
                }
                return dt_Retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
        }

        /// <summary>
        /// Retorna as metas de venda de produdos do representante.
        /// </summary>
        /// <param name="iKunnr">Código do representante.</param>
        /// <returns></returns>
        public DataTable Get_Products_Goal_Realized(string iKunnr)
        {
            var sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            var tDados = new ZSDTT_0002();

            try
            {
                sapProxy.Connection.Open();
                sapProxy.Zsd_Get_Products_Goal_Realized("9000", iKunnr, ref tDados);
                var dtRetorno = tDados.ToADODataTable();

                if (dtRetorno != null && dtRetorno.Rows != null && dtRetorno.Rows.Count > 0)
                {
                    var ltGrMercadorias = new ZSDE005Table();
                    var ltMateriais = new ZSDE006Table();
                    var ltReturn2 = new ImcopaWEB.BAPIRET2Table();
                    sapProxy.Zsd_Quotations_Get_Materials(iKunnr, ref ltGrMercadorias, ref ltMateriais, ref ltReturn2);
                    var dtMateriais = ltMateriais.ToADODataTable();

                    foreach (DataRow row in dtRetorno.Rows)
                    {
                        var descMaterial = this.formataValor(row["MATNR"].ToString()).ToString();

                        foreach (DataRow row1 in dtMateriais.Rows)
                        {
                            if (row1["MATNR"].ToString() == row["MATNR"].ToString())
                            {
                                descMaterial = string.Format("{0} - {1}", descMaterial, row1["MAKTX"]);
                            }
                        }

                        row["LIFNR"] = row["LIFNR"].ToString();
                        row["MES"] = row["MES"].ToString();
                        row["MATNR"] = descMaterial;
                        row["META"] = Convert.ToDecimal(row["META"].ToString());//.Replace(" ", string.Empty).Replace(".", ",");                        
                        row["REALIZADO"] = Convert.ToDecimal(row["REALIZADO"].ToString().Trim());
                        row["PORCENT"] = row["PORCENT"].ToString().Replace("*", "");
                    }
                }

                return dtRetorno;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                {
                    sapProxy.Connection.Close();
                }
            }
        }

        public DataTable List_Billing(ZSDE028 i_Zsde028)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            BAPIRET1Table t_Return = new ImcopaWEB.BAPIRET1Table();
            ZSDE037Table t_Zsde037 = new ZSDE037Table();
            DataTable dt_Retorno = new DataTable();
            try
            {
                //sapProxy.Timeout = 240000;
                sapProxy.Connection.Open();

                sapProxy.Zpwi_Billing_List(i_Zsde028, ref t_Return, ref t_Zsde037);

                dt_Retorno = t_Zsde037.ToADODataTable();
                //dt_Retorno.Columns.Add(new DataColumn("ZFBDT2", typeof(DateTime)));
                foreach (DataRow row in dt_Retorno.Rows)
                {
                    row["NAME1"] = row["NAME1"].ToString().ToUpper();
                    row["INCO2"] = row["INCO2"].ToString().ToUpper();
                    row["FKDAT"] = formataData(row["FKDAT"].ToString());
                    row["MATNR"] = formataValor(row["MATNR"].ToString());
                    row["KUNNR"] = formataValor(row["KUNNR"].ToString());
                    row["FKIMG"] = formataValor(row["FKIMG"].ToString());
                    //row["KZWI1"] = formataValor(row["KZWI1"].ToString());
                }
                return dt_Retorno;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }
        }

        public DataTable Sales_Get_History(String iAubel, Decimal iFkimg, String iKunag, String name, Decimal iNetwr, String iOrt01, String iVebeln, String dtIni, String dtFim, String iCorretor)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            ZSDTT_0001 t_Return = new ZSDTT_0001();

            DataTable dt_Retorno = new DataTable();

            try
            {
                sapProxy.Connection.Open();
                sapProxy.Zsd_Sales_Get_History(iAubel, iCorretor, dtFim, dtIni, iFkimg, iKunag, name, iNetwr, iOrt01, iVebeln, ref t_Return); //, ref t_Return                

                dt_Retorno = t_Return.ToADODataTable();
            }
            catch (RfcException ex)
            {
                throw ex;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }

            return dt_Retorno;
        }

        public DataTable Get_Clients_Linked(string iKunner)
        {
            bapi sapProxy = new bapi(ConfigurationManager.AppSettings["R3"]);
            //ZSDTT_0002 t_Return = new ZSDTT_0002();
            ZSDTT_0005 t_Return = new ZSDTT_0005();
            DataTable dt_Retorno = new DataTable();

            try
            {
                sapProxy.Connection.Open();
                sapProxy.Zsd_Get_Clients_Linked("9000", iKunner, ref t_Return);

                dt_Retorno = t_Return.ToADODataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sapProxy.Connection.IsOpen)
                    sapProxy.Connection.Close();
            }

            return dt_Retorno;
        }

        private string mascaraCNPJ(string strCnpj)
        {
            try
            {
                MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
                mtpCnpj.Set(ZerosEsquerda(strCnpj, 11));
                return mtpCnpj.ToString();
            }
            catch
            {
                return strCnpj;
            }
        }

        public decimal formataValor(string val)
        {
            int iValor_Aux = 0;
            if (val != String.Empty)
            {
                decimal valor = Convert.ToDecimal(val);
                iValor_Aux = Convert.ToInt32(valor);
            }
            return Convert.ToDecimal(iValor_Aux);
        }

        private string formataData(string dt)
        {
            string data = null;
            if (dt == "00000000")
                dt = String.Empty;

            if (dt != String.Empty)
                data = dt.Substring(6, 2) + "/" + dt.Substring(4, 2) + "/" + dt.Substring(0, 4);
            else
                data = String.Empty;
            return data;
        }

        public static string ZerosEsquerda(string strString, int intTamanho)
        {
            string strResult = "";
            for (int intCont = 1; intCont <= (intTamanho - strString.Length); intCont++)
            {
                strResult += "0";
            }
            return strResult + strString;
        }

        public string cut(object descricao, int def)
        {
            int tamanho = descricao.ToString().Length;
            int cutSize = def;
            string retorno;
            if (tamanho < cutSize)
                cutSize = tamanho;
            retorno = descricao.ToString().Substring(0, cutSize).ToString();
            if (cutSize == def)
                retorno += "..";
            return retorno;
        }

        /// <summary>
        /// Rotina para validação de CNPJ
        /// </summary>
        /// <param name="cnpj">string</param>
        /// <returns>bool</returns>
        public bool ValidaCnpj(string cnpj)
        {
            try
            {
                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int soma;
                int resto;
                string digito;
                string tempCnpj;

                cnpj = cnpj.Trim();
                cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

                if (cnpj.Length != 14)
                    return false;

                tempCnpj = cnpj.Substring(0, 12);

                soma = 0;
                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCnpj = tempCnpj + digito;
                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cnpj.EndsWith(digito);
            }
            catch
            {
                return false;
                //throw;
            }
        }

        public bool EnviarEmail(string email, string assunto, string mensagem)
        {
            bool EmailEnviado = false;
            string _from = ConfigurationSettings.AppSettings["emailRemetente"];

            SmtpClient client = new SmtpClient();

            MailAddress from = new MailAddress(_from);
            MailAddress to = new MailAddress(email);
            MailMessage Mensagem = new MailMessage(from, to);

            Mensagem.IsBodyHtml = true;
            Mensagem.Subject = assunto;
            Mensagem.Body = mensagem;
            Mensagem.IsBodyHtml = true;
            Mensagem.BodyEncoding = ASCIIEncoding.UTF8;

            try
            {
                //Envia o email
                client.Send(Mensagem);
                EmailEnviado = true;
            }
            catch
            {
                EmailEnviado = false;
            }

            return EmailEnviado;
        }
    }
}