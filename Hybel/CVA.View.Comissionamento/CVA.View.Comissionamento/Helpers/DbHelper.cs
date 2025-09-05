using System;
using System.Collections.Generic;
using System.Linq;
using SAPbobsCOM;
using System.Xml.Linq;

namespace CVA.View.Comissionamento.Helpers
{
    public class DbHelper
    {
        private readonly SapFactory _factory;

        public DbHelper(SapFactory factory)
        {
            _factory = factory;
        }

        public void VerifyData()
        {
            try
            {
                CreateTables();
                CreateFields();
                CreateUdos();
                CreateQueries();
                CreateReport();
                //CreateProcs();
            }
            catch
            {

            }
        }

        private void CreateTables()
        {
            var oProgress = _factory.Application.StatusBar.CreateProgressBar("Registrando tabelas definidas pelo usuário", 4, true);

            try
            {
                if (!UserTableExists("CVA_EQUIPE"))
                    CreateUserTable("CVA_EQUIPE", "[CVA] Comissão: Equipe", BoUTBTableType.bott_MasterData);
                if (!UserTableExists("CVA_GERENTE"))
                    CreateUserTable("CVA_GERENTE", "[CVA] Comissão: Gerente", BoUTBTableType.bott_MasterData);
                if (!UserTableExists("CVA_GERENTE1"))
                    CreateUserTable("CVA_GERENTE1", "[CVA] Vendedores X Gerente", BoUTBTableType.bott_MasterDataLines);
                if (!UserTableExists("CVA_META"))
                    CreateUserTable("CVA_META", "[CVA] Metas de comissão", BoUTBTableType.bott_MasterData);
                if (!UserTableExists("CVA_META1"))
                    CreateUserTable("CVA_META1", "[CVA] Valores Metas", BoUTBTableType.bott_MasterDataLines);
                if (!UserTableExists("CVA_TIPO_COMISSAO"))
                    CreateUserTable("CVA_TIPO_COMISSAO", "[CVA] Tipos de comissão", BoUTBTableType.bott_MasterData);
                oProgress.Value++;
                if (!UserTableExists("CVA_CRIT_COMISSAO"))
                    CreateUserTable("CVA_CRIT_COMISSAO", "[CVA] Critérios de comissão", BoUTBTableType.bott_MasterData);
                oProgress.Value++;
                if (!UserTableExists("CVA_REGR_COMISSAO"))
                    CreateUserTable("CVA_REGR_COMISSAO", "[CVA] Regras de comissão", BoUTBTableType.bott_MasterData);
                oProgress.Value++;
                if (!UserTableExists("CVA_CALC_COMISSAO"))
                    CreateUserTable("CVA_CALC_COMISSAO", "[CVA] Cálculo de comissão", BoUTBTableType.bott_MasterData);
                oProgress.Value++;
                try
                {
                    oProgress.Stop();
                    oProgress = null;
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                try
                {
                    oProgress.Stop();
                    oProgress = null;
                }
                catch
                {
                }
                _factory.Application.SetStatusBarMessage(ex.Message);
                throw;
            }

        }

        private void CreateFields()
        {
            var oProgress = _factory.Application.StatusBar.CreateProgressBar("Processando campos definidos pelo usuário", 63, true);

            try
            {
                if (!UserFieldExists("@CVA_EQUIPE", "VALOR"))
                    CreateUserField("CVA_EQUIPE", "VALOR", "Valor Comissão", 10, BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, BoYesNoEnum.tYES);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_GERENTE1", "IDVENDEDOR"))
                    CreateUserField("CVA_GERENTE1", "IDVENDEDOR", "ID Vendedor", 10, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_GERENTE1", "NOME"))
                    CreateUserField("CVA_GERENTE1", "NOME", "Nome Vendedor", 254, BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_META", "FILIAL"))
                    CreateUserField("CVA_META", "FILIAL", "Filial", 3, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, BoYesNoEnum.tNO);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_META", "TIPO"))
                    CreateUserField("CVA_META", "TIPO", "Tipo de comissão", 50, BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "CVA_TIPO_COMISSAO");
                oProgress.Value++;
                if (!UserFieldExists("@CVA_META1", "VALIDODE"))
                    CreateUserField("CVA_META1", "VALIDODE", "Competência de", 50, BoFieldTypes.db_Date, BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_META1", "VALIDOATE"))
                    CreateUserField("CVA_META1", "VALIDOATE", "Competência até", 50, BoFieldTypes.db_Date, BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_META1", "VALOR"))
                    CreateUserField("CVA_META1", "VALOR", "Valor Meta", 10, BoFieldTypes.db_Float, BoFldSubTypes.st_Sum, BoYesNoEnum.tNO);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_TIPO_COMISSAO", "TIPO"))
                    CreateUserField("CVA_TIPO_COMISSAO", "TIPO", "É tipo vendedor?", 1, BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, BoYesNoEnum.tYES, "Y", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CRIT_COMISSAO", "POS"))
                    CreateUserField("CVA_CRIT_COMISSAO", "POS", "Posição", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CRIT_COMISSAO", "ATIVO"))
                    CreateUserField("CVA_CRIT_COMISSAO", "ATIVO", "Ativo?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "Y", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "TIPO"))
                    CreateUserField("CVA_REGR_COMISSAO", "TIPO", "Tipo de comissão", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "CVA_TIPO_COMISSAO");
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "FILIAL"))
                    CreateUserField("CVA_REGR_COMISSAO", "FILIAL", "Filial", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "COMISSIONADO"))
                    CreateUserField("CVA_REGR_COMISSAO", "COMISSIONADO", "Comissionado", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "COMNAME"))
                    CreateUserField("CVA_REGR_COMISSAO", "COMNAME", "Nome do comissionado", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "MOMENTO"))
                    CreateUserField("CVA_REGR_COMISSAO", "MOMENTO", "Momento da comissão", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, new Dictionary<object, object> { { "F", "Faturamento" }, { "R", "Recebimento" }, { "P", "Pedido" } });
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "VENDEDOR"))
                    CreateUserField("CVA_REGR_COMISSAO", "VENDEDOR", "Vendedor", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "EQUIPE"))
                    CreateUserField("CVA_REGR_COMISSAO", "EQUIPE", "Equipe", 50);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "VNDNAME"))
                    CreateUserField("CVA_REGR_COMISSAO", "VNDNAME", "Nome do vendedor", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "ITEM"))
                    CreateUserField("CVA_REGR_COMISSAO", "ITEM", "Código do item", 20);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "ITEMNAME"))
                    CreateUserField("CVA_REGR_COMISSAO", "ITEMNAME", "Nome do item", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "GRUPO"))
                    CreateUserField("CVA_REGR_COMISSAO", "GRUPO", "Grupo de item", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "ITMSGRPNAM"))
                    CreateUserField("CVA_REGR_COMISSAO", "ITMSGRPNAM", "Nome do grupo do item", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "CENTROCUSTO"))
                    CreateUserField("CVA_REGR_COMISSAO", "CENTROCUSTO", "Centro de custo", 8);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "PRCNAME"))
                    CreateUserField("CVA_REGR_COMISSAO", "PRCNAME", "Nome do centro de custo", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "FABRICANTE"))
                    CreateUserField("CVA_REGR_COMISSAO", "FABRICANTE", "% de Comissão", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "CLIENTE"))
                    CreateUserField("CVA_REGR_COMISSAO", "CLIENTE", "Cliente", 15);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "CARDNAME"))
                    CreateUserField("CVA_REGR_COMISSAO", "CARDNAME", "Nome do cliente", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "CIDADE"))
                    CreateUserField("CVA_REGR_COMISSAO", "CIDADE", "Cidade", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "COUNTY"))
                    CreateUserField("CVA_REGR_COMISSAO", "COUNTY", "Nome da cidade", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "ESTADO"))
                    CreateUserField("CVA_REGR_COMISSAO", "ESTADO", "Estado", 2);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "SETOR"))
                    CreateUserField("CVA_REGR_COMISSAO", "SETOR", "Setor", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "COMISSAO"))
                    CreateUserField("CVA_REGR_COMISSAO", "COMISSAO", "% de comissão", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "COMISSAO_REAL"))
                    CreateUserField("CVA_REGR_COMISSAO", "COMISSAO_REAL", "Comissão real", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "PRIORIDADE"))
                    CreateUserField("CVA_REGR_COMISSAO", "PRIORIDADE", "Prioridade", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "META"))
                    CreateUserField("CVA_REGR_COMISSAO", "META", "Meta", 60);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "PORCMETA"))
                    CreateUserField("CVA_REGR_COMISSAO", "PORCMETA", "Porcentagem Meta", 10, SAPbobsCOM.BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "GROUP"))
                    CreateUserField("CVA_REGR_COMISSAO", "GROUP", "Grupo de cliente", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "GROUPNAME"))
                    CreateUserField("CVA_REGR_COMISSAO", "GROUPNAME", "Nome do grupo de cliente", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_REGR_COMISSAO", "ATIVO"))
                    CreateUserField("CVA_REGR_COMISSAO", "ATIVO", "Ativo", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tYES, "Y", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "BPLID"))
                    CreateUserField("CVA_CALC_COMISSAO", "BPLID", "Filial", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "COMISSIONADO"))
                    CreateUserField("CVA_CALC_COMISSAO", "COMISSIONADO", "Comissionado", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "COMNAME"))
                    CreateUserField("CVA_CALC_COMISSAO", "COMNAME", "Nome do comissionado", 254);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "META"))
                    CreateUserField("CVA_CALC_COMISSAO", "META", "Meta", 10, SAPbobsCOM.BoFieldTypes.db_Float, BoFldSubTypes.st_Sum);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "PORCMETA"))
                    CreateUserField("CVA_CALC_COMISSAO", "PORCMETA", "Porcentagem Meta", 10, SAPbobsCOM.BoFieldTypes.db_Float, BoFldSubTypes.st_Percentage);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "CARDCODE"))
                    CreateUserField("CVA_CALC_COMISSAO", "CARDCODE", "Cód. cliente", 15);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "CARDNAME"))
                    CreateUserField("CVA_CALC_COMISSAO", "CARDNAME", "Razão social", 100);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "REGRA"))
                    CreateUserField("CVA_CALC_COMISSAO", "REGRA", "Regra de comissão", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "CVA_REGR_COMISSAO");
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "DOCDATE"))
                    CreateUserField("CVA_CALC_COMISSAO", "DOCDATE", "Data do documento", 12, SAPbobsCOM.BoFieldTypes.db_Date);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "DUEDATE"))
                    CreateUserField("CVA_CALC_COMISSAO", "DUEDATE", "Data do vencimento", 12, SAPbobsCOM.BoFieldTypes.db_Date);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "DOCENTRY"))
                    CreateUserField("CVA_CALC_COMISSAO", "DOCENTRY", "Chave do documento", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "OBJTYPE"))
                    CreateUserField("CVA_CALC_COMISSAO", "OBJTYPE", "Tipo do documento", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "ITEMCODE"))
                    CreateUserField("CVA_CALC_COMISSAO", "ITEMCODE", "Cód. item", 20);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "ITEMNAME"))
                    CreateUserField("CVA_CALC_COMISSAO", "ITEMNAME", "Descrição do item", 100);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "LINENUM"))
                    CreateUserField("CVA_CALC_COMISSAO", "LINENUM", "Linha", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "VALOR"))
                    CreateUserField("CVA_CALC_COMISSAO", "VALOR", "Valor", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "PARCELA"))
                    CreateUserField("CVA_CALC_COMISSAO", "PARCELA", "Parcela", 10, SAPbobsCOM.BoFieldTypes.db_Numeric);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "IMPOSTOS"))
                    CreateUserField("CVA_CALC_COMISSAO", "IMPOSTOS", "Impostos", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "TOTALVENDAS"))
                    CreateUserField("CVA_CALC_COMISSAO", "TOTALVENDAS", "Valor Total Vendas", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "COMISSAO"))
                    CreateUserField("CVA_CALC_COMISSAO", "COMISSAO", "% de comissão", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "COMISSAOEQUIP"))
                    CreateUserField("CVA_CALC_COMISSAO", "COMISSAOEQUIP", "% comissão equipe", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "VALORCOMISSAO"))
                    CreateUserField("CVA_CALC_COMISSAO", "VALORCOMISSAO", "Valor Vendedor", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "VALORCOMEQUIP"))
                    CreateUserField("CVA_CALC_COMISSAO", "VALORCOMEQUIP", "Comissão Equipe", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "DSR"))
                    CreateUserField("CVA_CALC_COMISSAO", "DSR", "Valor DSR", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "VALORCOMTOTAL"))
                    CreateUserField("CVA_CALC_COMISSAO", "VALORCOMTOTAL", "Comissão Total", 10, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Sum);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "CENTROCUSTO"))
                    CreateUserField("CVA_CALC_COMISSAO", "CENTROCUSTO", "Centro de custo", 8);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "PAGO"))
                    CreateUserField("CVA_CALC_COMISSAO", "PAGO", "Comissão paga?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "DATAPAGAMENTO"))
                    CreateUserField("CVA_CALC_COMISSAO", "DATAPAGAMENTO", "Data de pagamento", 12, SAPbobsCOM.BoFieldTypes.db_Date);
                oProgress.Value++;
                if (!UserFieldExists("@CVA_CALC_COMISSAO", "TAXDATE"))
                    CreateUserField("CVA_CALC_COMISSAO", "TAXDATE", "Data do recebimento", 12, SAPbobsCOM.BoFieldTypes.db_Date);
                oProgress.Value++;
                if (!UserFieldExists("OSLP", "CVA_IMPINCL"))
                    CreateUserField("OSLP", "CVA_IMPINCL", "Comissões: Impostos inclusos no preço?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                oProgress.Value++;
                if (!UserFieldExists("OSLP", "CVA_IMPADC"))
                    CreateUserField("OSLP", "CVA_IMPADC", "Comissões: Impostos adicionais?", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", new Dictionary<object, object> { { "Y", "Sim" }, { "N", "Não" } });
                oProgress.Value++;

                try
                {
                    oProgress.Stop();
                    oProgress = null;
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                try
                {
                    oProgress.Stop();
                    oProgress = null;
                }
                catch
                {
                }
                _factory.Application.SetStatusBarMessage(ex.Message);
                throw;
            }
        }

        private void CreateUdos()
        {
            var oProgress = _factory.Application.StatusBar.CreateProgressBar("Registrando objetos definidos pelo usuário", 7, true);

            try
            {
                if (!UserObjectExists("UDOTIPO"))
                    CreateUserObject($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOTIPO.xml");
                else
                {
                    if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\_UDOTIPO.txt"))
                        UpdateUserObject("UDOTIPO", $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOTIPO.xml");
                }
                oProgress.Value++;
                if (!UserObjectExists("UDOCRIT"))
                    CreateUserObject($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOCRIT.xml");
                else
                {
                    if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\_UDOCRIT.txt"))
                        UpdateUserObject("UDOCRIT", $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOCRIT.xml");
                }
                oProgress.Value++;
                if (!UserObjectExists("UDOREGR"))
                    CreateUserObject($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOREGR.xml");
                else
                {
                    if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\_UDOREGR.txt"))
                        UpdateUserObject("UDOREGR", $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOREGR.xml");
                }
                oProgress.Value++;
                if (!UserObjectExists("UDOCALC"))
                    CreateUserObject($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOCALC.xml");
                else
                {
                    if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\_UDOCALC.txt"))
                        UpdateUserObject("UDOCALC", $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOCALC.xml");
                }
                oProgress.Value++;
                if (!UserObjectExists("UDOMETA"))
                    CreateUserObject($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOMETA.xml");
                else
                {
                    if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\_UDOMETA.txt"))
                        UpdateUserObject("UDOMETA", $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOMETA.xml");
                }
                oProgress.Value++;
                if (!UserObjectExists("UDOEQPE"))
                    CreateUserObject($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOEQPE.xml");
                else
                {
                    if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\_UDOEQPE.txt"))
                        UpdateUserObject("UDOEQPE", $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOEQPE.xml");
                }
                oProgress.Value++;
                if (!UserObjectExists("UDOGRNT"))
                    CreateUserObject($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOGRNT.xml");
                else
                {
                    if (System.IO.File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\_UDOGRNT.txt"))
                        UpdateUserObject("UDOGRNT", $"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udos\\UDOGRNT.xml");
                }
                oProgress.Value++;
                try
                {
                    oProgress.Stop();
                    oProgress = null;
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                try
                {
                    oProgress.Stop();
                    oProgress = null;
                }
                catch
                {
                }
                _factory.Application.SetStatusBarMessage(ex.Message);
                throw;
            }
        }

        private void CreateQueries()
        {
            var xml = XElement.Load($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Configs\\pesquisas_formatadas.xml");
            var consultas = xml.Elements();

            foreach (var item in consultas)
            {
                AssignFormattedSearch(item.Element("nome").Value.ToString(), item.Element("consulta").Value.ToString(), item.Element("form").Value.ToString(), item.Element("item").Value.ToString(), item.Element("coluna").Value.ToString());
            }
        }

        private void CreateReport()
        {
            var oCompanyService = _factory.Company.GetCompanyService();
            var oLayoutService = (ReportLayoutsService)oCompanyService.GetBusinessService(ServiceTypes.ReportLayoutsService);

            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            oRecordset.DoQuery("select DocCode from rdoc where docname = '[CVA] Monitoramento de Comissões'");

            if (oRecordset.RecordCount > 0)
            {
                var docCode = oRecordset.Fields.Item(0).Value.ToString();

                var rptFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\Reports\Comissoes.rpt";
                var oBlobParams = (BlobParams)oCompanyService.GetDataInterface(CompanyServiceDataInterfaces.csdiBlobParams);
                oBlobParams.Table = "RDOC";
                oBlobParams.Field = "Template";

                var oKeySegment = oBlobParams.BlobTableKeySegments.Add();
                oKeySegment.Name = "DocCode";
                oKeySegment.Value = docCode;

                var oBlob = (Blob)oCompanyService.GetDataInterface(CompanyServiceDataInterfaces.csdiBlob);
                var oFile = new System.IO.FileStream(rptFilePath, System.IO.FileMode.Open);
                var fileSize = (int)oFile.Length;
                var buf = new byte[fileSize];
                oFile.Read(buf, 0, fileSize);
                oFile.Close();
                oFile.Dispose();

                oBlob.Content = Convert.ToBase64String(buf, 0, fileSize);
                oCompanyService.SetBlob(oBlobParams, oBlob);
            }
            else
            {
                var oReport = (ReportLayout)oLayoutService.GetDataInterface(ReportLayoutsServiceDataInterfaces.rlsdiReportLayout);

                oReport.Name = "[CVA] Monitoramento de Comissões";
                oReport.TypeCode = "RCRI";
                oReport.Author = _factory.Company.UserName;
                oReport.Category = ReportLayoutCategoryEnum.rlcCrystal;

                var oNewReportParams = oLayoutService.AddReportLayoutToMenu(oReport, "43531");
                var newReportCode = oNewReportParams.LayoutCode;

                var rptFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\Files\Reports\Comissoes.rpt";
                var oBlobParams = (BlobParams)oCompanyService.GetDataInterface(CompanyServiceDataInterfaces.csdiBlobParams);
                oBlobParams.Table = "RDOC";
                oBlobParams.Field = "Template";

                var oKeySegment = oBlobParams.BlobTableKeySegments.Add();
                oKeySegment.Name = "DocCode";
                oKeySegment.Value = newReportCode;

                var oBlob = (Blob)oCompanyService.GetDataInterface(CompanyServiceDataInterfaces.csdiBlob);
                var oFile = new System.IO.FileStream(rptFilePath, System.IO.FileMode.Open);
                var fileSize = (int)oFile.Length;
                var buf = new byte[fileSize];
                oFile.Read(buf, 0, fileSize);
                oFile.Close();
                oFile.Dispose();

                oBlob.Content = Convert.ToBase64String(buf, 0, fileSize);

                oCompanyService.SetBlob(oBlobParams, oBlob);
            }
        }

        private void CreateProcs()
        {
            var script = System.IO.File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Queries\\query.sql");
            var commandStrings = System.Text.RegularExpressions.Regex.Split(script, @"^\s*GO\s*$", System.Text.RegularExpressions.RegexOptions.Multiline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            foreach (var commandString in commandStrings)
            {
                if(commandString.Trim() != "")
                {
                    var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    oRecordset.DoQuery(commandString);
                }
            }
        }

        private bool UserTableExists(string name)
        {
            UserTablesMD table = (UserTablesMD)_factory.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            var ret = table.GetByKey(name);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(table);
            return ret;
        }

        private void CreateUserTable(string name, string description, BoUTBTableType type = BoUTBTableType.bott_NoObject)
        {
            UserTablesMD table = (UserTablesMD)_factory.Company.GetBusinessObject(BoObjectTypes.oUserTables);
            table.TableName = name;
            table.TableDescription = description;
            table.TableType = type;

            if (table.Add() != 0)
            {
                int errCode;
                string errMsg;
                _factory.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(table);

                throw new Exception($"Erro ao criar tabela de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(table);
        }

        private bool UserFieldExists(string tableName, string name)
        {
            Recordset oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = "SELECT 1 [result] FROM SYS.OBJECTS so " +
                        "INNER JOIN SYS.COLUMNS sc ON sc.object_id = so.object_id " +
                        "WHERE so.schema_id = 1 " +
                        $"AND so.name = '{tableName}' AND sc.name = 'U_{name}'";
            oRecordset.DoQuery(query);
            var lRet = (int)oRecordset.Fields.Item("result").Value;
            var ret = lRet == 1;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
            return ret;
        }

        private void CreateUserField(string tableName, string fieldName, string description, int size,
            BoFieldTypes type = BoFieldTypes.db_Alpha, BoFldSubTypes subType = BoFldSubTypes.st_None,
            BoYesNoEnum mandatory = BoYesNoEnum.tNO, string defaultValue = null,
            Dictionary<object, object> validValues = null, string linkedTable = null)
        {
            UserFieldsMD field = (UserFieldsMD)_factory.Company.GetBusinessObject(BoObjectTypes.oUserFields);

            field.TableName = tableName;
            field.Name = fieldName;
            field.Type = type;
            field.Size = size;
            field.EditSize = size;
            field.Description = description;
            field.SubType = subType;
            field.Mandatory = mandatory;
            field.LinkedTable = linkedTable;

            if (validValues != null)
            {
                foreach (var item in validValues)
                {
                    field.ValidValues.Add();
                    field.ValidValues.Description = item.Value.ToString();
                    field.ValidValues.Value = item.Key.ToString();
                }

                field.DefaultValue = defaultValue;
            }

            if (field.Add() != 0)
            {
                int errCode;
                string errMsg;
                _factory.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(field);

                throw new Exception($"Erro ao criar campo de usuário: ({errCode}) {errMsg}.");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(field);
        }

        private bool UserObjectExists(string udoCode)
        {
            UserObjectsMD udo = (UserObjectsMD)_factory.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            var ret = udo.GetByKey(udoCode);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            return ret;
        }

        private void CreateUserObject(string xmlFilePath)
        {
            _factory.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            UserObjectsMD udo = (UserObjectsMD)_factory.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

            udo.Browser.ReadXml(xmlFilePath, 0);
            var udoCode = udo.Code;

            if (udo.Add() != 0)
            {
                int errCode;
                string errMsg;

                _factory.Company.GetLastError(out errCode, out errMsg);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

                throw new Exception($"Erro ao criar objeto definido pelo usuário: ({errCode}) {errMsg}");
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

            AddUdoData(udoCode);
        }

        private void UpdateUserObject(string code, string xmlFilePath)
        {
            _factory.Company.XmlExportType = BoXmlExportTypes.xet_ExportImportMode;
            UserObjectsMD udo = (UserObjectsMD)_factory.Company.GetBusinessObject(BoObjectTypes.oUserObjectsMD);
            udo.GetByKey(code);

            var prevXml = udo.GetAsXML();

            udo.Browser.ReadXml(xmlFilePath, 0);

            var nextXml = udo.GetAsXML();

            if (prevXml != nextXml)
            {
                var udoCode = udo.Code;

                if (udo.Update() != 0)
                {
                    int errCode;
                    string errMsg;

                    _factory.Company.GetLastError(out errCode, out errMsg);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);

                    throw new Exception($"Erro ao criar objeto definido pelo usuário: ({errCode}) {errMsg}");
                }

                try
                {
                    System.IO.File.Delete($"{AppDomain.CurrentDomain.BaseDirectory}\\Files\\Udo\\_{code}.txt");
                }
                catch { }         
            }    

            System.Runtime.InteropServices.Marshal.ReleaseComObject(udo);
        }

        private void AddUdoData(string udoCode)
        {
            var oCompanyService = _factory.Company.GetCompanyService();

            if (udoCode == "UDOTIPO")
            {
                var oGeneralService = oCompanyService.GetGeneralService("UDOTIPO");
                var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "1");
                oGeneralData.SetProperty("Name", "REPRESENTANTE");
                oGeneralData.SetProperty("U_TIPO", "N");
                var oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOTIPO");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "2");
                oGeneralData.SetProperty("Name", "VENDEDOR");
                oGeneralData.SetProperty("U_TIPO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);
            }

            if (udoCode == "UDOCRIT")
            {
                var oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                var oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "1");
                oGeneralData.SetProperty("Name", "Vendedor (obrigatório)");
                oGeneralData.SetProperty("U_POS", "1");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                var oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "2");
                oGeneralData.SetProperty("Name", "Item");
                oGeneralData.SetProperty("U_POS", "2");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "3");
                oGeneralData.SetProperty("Name", "Grupo de itens");
                oGeneralData.SetProperty("U_POS", "3");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "4");
                oGeneralData.SetProperty("Name", "Centro de custo");
                oGeneralData.SetProperty("U_POS", "4");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "5");
                oGeneralData.SetProperty("Name", "Fabricante");
                oGeneralData.SetProperty("U_POS", "5");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "6");
                oGeneralData.SetProperty("Name", "Cliente");
                oGeneralData.SetProperty("U_POS", "6");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "7");
                oGeneralData.SetProperty("Name", "Cidade");
                oGeneralData.SetProperty("U_POS", "7");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "8");
                oGeneralData.SetProperty("Name", "Estado");
                oGeneralData.SetProperty("U_POS", "8");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "9");
                oGeneralData.SetProperty("Name", "Setor");
                oGeneralData.SetProperty("U_POS", "9");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);

                oGeneralService = oCompanyService.GetGeneralService("UDOCRIT");
                oGeneralData = (GeneralData)oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);
                oGeneralData.SetProperty("Code", "10");
                oGeneralData.SetProperty("Name", "Grupo de cliente");
                oGeneralData.SetProperty("U_POS", "10");
                oGeneralData.SetProperty("U_ATIVO", "Y");
                oGeneralParams = oGeneralService.Add(oGeneralData);
            }
        }

        public void AssignFormattedSearch(string queryName, string theQuery, string formId, string itemId, string colId = "-1")
        {
            var oFms = (FormattedSearches)_factory.Company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
            var oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            var query = $"SELECT * FROM CSHS T0 INNER JOIN OUQR T1 ON T0.QueryId = T1.IntrnalKey WHERE T0.FormID = '{formId}' AND T0.ItemId = '{itemId}' AND T0.ColID = '{colId}'";

            oRecordset.DoQuery(query);

            if (oRecordset.RecordCount == 0)
            {
                int queryId = CreateQuery(queryName, theQuery);
                oFms.Action = BoFormattedSearchActionEnum.bofsaQuery;
                oFms.FormID = formId;
                oFms.ItemID = itemId;
                oFms.ColumnID = colId;
                oFms.QueryID = queryId;
                oFms.FieldID = itemId;

                if (colId.Equals("-1"))
                    oFms.ByField = BoYesNoEnum.tYES;
                else
                    oFms.ByField = BoYesNoEnum.tNO;

                long lRetCode = oFms.Add();

                if (lRetCode == -2035)
                {
                    oRecordset.DoQuery($"SELECT TOP 1 T0.IndexID FROM [dbo].[CSHS] T0 WHERE T0.FormID = '{formId}' AND T0.ItemID = '{itemId}' AND T0.ColID = '{colId}'");

                    if (oRecordset.RecordCount > 0)
                    {
                        oFms.GetByKey((int)oRecordset.Fields.Item(0).Value);
                        oFms.Action = BoFormattedSearchActionEnum.bofsaQuery;
                        oFms.FormID = formId;
                        oFms.ItemID = itemId;
                        oFms.ColumnID = colId;
                        oFms.QueryID = queryId;
                        oFms.FieldID = itemId;

                        if (colId.Equals("-1"))
                            oFms.ByField = BoYesNoEnum.tYES;
                        else
                            oFms.ByField = BoYesNoEnum.tNO;

                        lRetCode = oFms.Update();
                    }
                }

                if (lRetCode != 0)
                    throw new Exception(_factory.Company.GetLastErrorDescription());
            }
            else
            {
                CreateQuery(queryName, theQuery);
            }
        }

        private void RemoveFormattedSearch(string queryName, string itemId, string formId)
        {
            Recordset oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            UserQueries oQuery = (UserQueries)_factory.Company.GetBusinessObject(BoObjectTypes.oUserQueries);
            FormattedSearches oFms = (FormattedSearches)_factory.Company.GetBusinessObject(BoObjectTypes.oFormattedSearches);

            oRecordset.DoQuery($"SELECT IndexId FROM CSHS WHERE ItemId = '{itemId}' AND FormId = '{formId}'");

            if (oRecordset.RecordCount > 0)
            {
                oFms.GetByKey(Convert.ToInt32(oRecordset.Fields.Item(0).Value));
                oFms.Remove();
            }

            oRecordset.DoQuery($"SELECT IntrnalKey, QCategory FROM OUQR WHERE QName = '{queryName}' and QCategory = {GetSysCatID()}");

            if (oRecordset.RecordCount > 0)
            {
                oQuery.GetByKey(Convert.ToInt32(oRecordset.Fields.Item(0).Value), Convert.ToInt32(oRecordset.Fields.Item(1).Value));
                oQuery.Remove();
            }
        }

        private bool ExistsQuery(string query)
        {
            query = query.Replace("'", "''");
            bool exists = false;

            Recordset oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordset.DoQuery($"SELECT TOP 1 1 FROM OUQR WHERE CAST(QString AS NVARCHAR(MAX)) = '{query}'");

            if (oRecordset.RecordCount > 0)
                exists = true;

            return exists;
        }

        private int CreateQuery(string QueryName, string TheQuery)
        {
            int ret = 0;
            ret = -1;

            Recordset oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            UserQueries oQuery = (UserQueries)_factory.Company.GetBusinessObject(BoObjectTypes.oUserQueries);

            oRecordset.DoQuery($"SELECT TOP 1 INTRNALKEY FROM OUQR WHERE QCATEGORY={GetSysCatID()} AND QNAME='{QueryName}'");
            if (oRecordset.RecordCount > 0)
            {
                ret = (int)oRecordset.Fields.Item(0).Value;
                oQuery.GetByKey(ret, GetSysCatID());
                oQuery.Query = TheQuery;
                oQuery.Update();
            }
            else
            {
                oQuery.QueryCategory = GetSysCatID();
                oQuery.QueryDescription = QueryName;
                oQuery.Query = TheQuery;

                if (oQuery.Add() != 0)
                    throw new Exception(_factory.Company.GetLastErrorDescription());

                string newKey = _factory.Company.GetNewObjectKey();

                if (newKey.Contains('\t'))
                    newKey = newKey.Split('\t')[0];

                ret = Convert.ToInt32(newKey);
            }

            return ret;
        }

        private int GetSysCatID()
        {
            int ret = -3;
            Recordset oRecordset = (Recordset)_factory.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

            oRecordset.DoQuery("SELECT TOP 1 CATEGORYID FROM OQCN WHERE CATNAME = 'Geral'");
            if (oRecordset.RecordCount > 0)
                ret = Convert.ToInt32(oRecordset.Fields.Item(0).Value);

            return ret;
        }
    }
}
