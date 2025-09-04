using DAL.Connection;
using DAL.UserData;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserFieldsBlo
    {
        public static string VerifyData()
        {
            var oProgressBar = ConnectionDao.Instance.Application.StatusBar.CreateProgressBar("Verificando dados mestre...", 35, true);

            try
            {
                if (!UserTablesDao.Exists("CVA_IMP_PED"))
                    UserTablesDao.Create("CVA_IMP_PED", "[CVA] Importação de pedidos", BoUTBTableType.bott_MasterData);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED", "ARQUIVO"))
                    UserFieldsDao.Create("CVA_IMP_PED", "ARQUIVO", "Nome do arquivo", 254, BoFieldTypes.db_Memo);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED", "DATA"))
                    UserFieldsDao.Create("CVA_IMP_PED", "DATA", "Data da importação", 10, BoFieldTypes.db_Date);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED", "LINHAS"))
                    UserFieldsDao.Create("CVA_IMP_PED", "LINHAS", "Quantidade de linhas do arquivo", 11, BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED", "STATUS"))
                    UserFieldsDao.Create("CVA_IMP_PED", "STATUS", "Status da importação", 1, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, BoYesNoEnum.tNO, "0", 
                        new Dictionary<object, object> {
                            { 0, "Importando" },
                            { 1, "Pedido gerado" },
                            { 2, "Erro ao gerar pedido" },
                            { 3, "NF gerada" },
                            { 4, "Erro ao gerar NF" },
                            { 5, "Pedido cancelado" },
                            { 6, "Erro ao cancelar pedido" },
                            { 7, "NF cancelada" },
                            { 8, "Erro ao cancelar N" },
                        });

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED", "LOG"))
                    UserFieldsDao.Create("CVA_IMP_PED", "LOG", "Log da importação", 254, BoFieldTypes.db_Memo);

                oProgressBar.Value++;

                if (!UserTablesDao.Exists("CVA_IMP_PED1"))
                    UserTablesDao.Create("CVA_IMP_PED1", "[CVA] Linhas do arquivo", BoUTBTableType.bott_MasterDataLines);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "CNPJ"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "CNPJ", "CNPJ Atlantic", 20);

                oProgressBar.Value++;


                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "BASE"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "BASE", "Nome da Base", 200);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "EMPRESA"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "EMPRESA", "Nome da Empresa", 200);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "COND"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "COND", "Condição de pagamento", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "LCTO"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "LCTO", "Data de lançamento", 11, BoFieldTypes.db_Date);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "DTST"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "DTST", "Data do status", 11, BoFieldTypes.db_Date);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "VCTO"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "VCTO", "Data de vencimento", 11, BoFieldTypes.db_Date);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "DIM1"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "DIM1", "Dimensão 01", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "DIM2"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "DIM2", "Dimensão 02", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "DIM3"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "DIM3", "Dimensão 03", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "DIM4"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "DIM4", "Dimensão 04", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "DIM5"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "DIM5", "Dimensão 05", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "FORMA"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "FORMA", "Forma de pagamento", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "ITEM"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "ITEM", "Item", 50);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "LINHA"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "LINHA", "Linha", 11, BoFieldTypes.db_Numeric);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "LOG"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "LOG", "Log da importação", 254, BoFieldTypes.db_Memo);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "MODEL"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "MODEL", "Modelo da nota", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "NUM"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "NUM", "Numero da nota", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "NUMSAP"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "NUMSAP", "Número do pedido SAP", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "OBS"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "OBS", "Observações", 254, BoFieldTypes.db_Memo);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "PN"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "PN", "PN", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "PRJ"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "PRJ", "Projeto", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "QTD"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "QTD", "Quantidade", 11, BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "SERIE"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "SERIE", "Número de série da nota", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "STATUS"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "STATUS", "Status da importação", 1, BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, BoYesNoEnum.tNO, "0", new Dictionary<object, object> { { 0, "Importando linha" }, { 1, "Linha sem erro" }, { 2, "Linha com erro" } });

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "UTIL"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "UTIL", "Utilização", 100);

                oProgressBar.Value++;

                if (!UserFieldsDao.Exists("@CVA_IMP_PED1", "VALOR"))
                    UserFieldsDao.Create("CVA_IMP_PED1", "VALOR", "Valor", 11, BoFieldTypes.db_Float, BoFldSubTypes.st_Price);

                oProgressBar.Value++;

                if (!UserObjectsDao.Exists("CVA_IMP_PED"))
                    UserObjectsDao.CreateUserObject("CVA_IMP_PED", "[CVA] Importação de pedidos", "@CVA_IMP_PED", BoUDOObjType.boud_MasterData);

                UserObjectsDao.AddChildTableToUserObject("CVA_IMP_PED", "@CVA_IMP_PED1");

                oProgressBar.Value++;

                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                try
                {
                    oProgressBar.Stop();
                }
                catch { }
            }
        }
    }
}
