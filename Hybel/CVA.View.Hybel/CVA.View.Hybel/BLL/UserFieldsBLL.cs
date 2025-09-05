using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System.Collections.Generic;

namespace CVA.View.Hybel.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
            #region Despesas Importação
            Dictionary<string, string> validValues = new Dictionary<string, string>();
            validValues.Add("Frete", "1");
            validValues.Add("Armazenagem", "2");
            validValues.Add("Serv. A. Importação", "3");
            validValues.Add("Frete PIS", "4");
            validValues.Add("Frete ICMS", "5");
            validValues.Add("Frete COFINS", "6");
            validValues.Add("Serv. A. Import. PIS", "7");
            validValues.Add("Serv. A. Import. COFINS", "8");

            userObjectController.InsertUserField("OALC", "CVA_CodItem", "Código Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
            userObjectController.InsertUserField("OALC", "CVA_TipoDespesa", "Tipo Despesa", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, validValues);

            #endregion

            #region SystemTables
            userObjectController.InsertUserField("OBOE", "CVA_Email", "Status envio e-mail", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OBOE", "CVA_Email", "N", "Não enviado", true);
            userObjectController.AddValidValueToUserField("OBOE", "CVA_Email", "P", "Pendente de Envio");
            userObjectController.AddValidValueToUserField("OBOE", "CVA_Email", "E", "Enviado");
            userObjectController.AddValidValueToUserField("OBOE", "CVA_Email", "R", "Erro no envio");

            userObjectController.InsertUserField("OBOE", "CVA_Data_Email", "Data último envio e-mail", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("OBOE", "CVA_Erro_Email", "Erro envio e-mail", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);

            userObjectController.InsertUserField("RDR1", "CVA_Concorrente", "Cód. Concorrente", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
            userObjectController.InsertUserField("RDR1", "CVA_Engenharia", "Cód. Engenharia", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
            userObjectController.InsertUserField("RDR1", "CVA_Hybel", "Cód. Hybel", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);

            #endregion

            #region DadosItem
            userObjectController.CreateUserTable("CVA_MONTADORA", "CVA - Montadora", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserObject("CVA_MONTADORA", "CVA - Montadora", "@CVA_MONTADORA", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_MONT_PROD", "CVA - Prod. Montadora", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_MONT_PROD", "Produto", "Código Produto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.AddChildTableToUserObject("CVA_MONTADORA", "@CVA_MONT_PROD");

            userObjectController.CreateUserTable("CVA_APLICACAO", "CVA - Aplicação", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserObject("CVA_APLICACAO", "CVA - Aplicação", "@CVA_APLICACAO", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_MAQUINA", "CVA - Tipo Máquina", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_MAQUINA", "Tipo", "Tipo Máquina", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.AddChildTableToUserObject("CVA_APLICACAO", "@CVA_MAQUINA");

            userObjectController.CreateUserTable("CVA_FUNCAO", "CVA - Função", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserObject("CVA_FUNCAO", "CVA - Função", "@CVA_FUNCAO", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_MONT_ITEM", "CVA - Montadora X Itens", BoUTBTableType.bott_MasterData);

            userObjectController.InsertUserField("@CVA_MONT_ITEM", "ItemCode", "Código Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            //userObjectController.InsertUserField("@CVA_MONT_ITEM", "ItemName", "Descrição Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.InsertUserField("@CVA_MONT_ITEM", "Montadora", "Montadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MONT_ITEM", "Mont_Prod", "Produto Montadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.InsertUserField("@CVA_MONT_ITEM", "Aplicacao", "Aplicação", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MONT_ITEM", "Tipo_Maquina", "Tipo Máquina", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MONT_ITEM", "Funcao", "Função", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_MONT_ITEM", "Obs", "Observação", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);

            userObjectController.CreateUserObject("CVA_MONT_ITEM", "CVA - Montadora X Itens", "@CVA_MONT_ITEM", BoUDOObjType.boud_MasterData);
            #endregion

            #region CVA_CONFIG_EMAIL
            userObjectController.CreateUserTable("CVA_CONFIG_EMAIL", "CVA - Config. E-mail", BoUTBTableType.bott_MasterData);
            userObjectController.InsertUserField("@CVA_CONFIG_EMAIL", "Servidor", "Servidor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_CONFIG_EMAIL", "Porta", "Porta", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_CONFIG_EMAIL", "SSL", "SSL", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.AddValidValueToUserField("@CVA_CONFIG_EMAIL", "SSL", "N", "Não", true);
            userObjectController.AddValidValueToUserField("@CVA_CONFIG_EMAIL", "SSL", "Y", "Sim");
            userObjectController.InsertUserField("@CVA_CONFIG_EMAIL", "Email", "E-mail", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_CONFIG_EMAIL", "Senha", "Senha", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_CONFIG_EMAIL", "Assunto", "Assunto do E-mail", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserObject("CVA_CONFIG_EMAIL", "CVA - Config. E-mail", "@CVA_CONFIG_EMAIL", BoUDOObjType.boud_MasterData);
            #endregion

            #region CVA_CONCORRENTE
            userObjectController.CreateUserTable("CVA_CONCORRENTE", "CVA - Concorrente X Itens", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserObject("CVA_CONCORRENTE", "CVA - Concorrente X Itens", "@CVA_CONCORRENTE", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_CONC_ITEM", "CVA - Concorrente X Itens", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_CONC_ITEM", "ItemCode", "Código Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_CONC_ITEM", "ItemName", "Descrição Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_CONC_ITEM", "Concorrente", "Concorrente", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_CONC_ITEM", "Obs", "Observação", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);

            userObjectController.AddChildTableToUserObject("CVA_CONCORRENTE", "@CVA_CONC_ITEM");
            #endregion

            #region CVA_MOTIVO_CANC
            userObjectController.CreateUserTable("CVA_MOTIVO_CANC", "CVA - Motivo Cancelamento", BoUTBTableType.bott_NoObject);
            #endregion

            #region CVA_SIM_VENDA
            userObjectController.CreateUserTable("CVA_SIM_VENDA", "CVA - Simulador Vendas", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserTable("CVA_SIM_VENDA_ITEM", "CVA - Simulador Vendas", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_ITEM", "ItemCode", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_ITEM", "ItemName", "Descrição", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_ITEM", "Quantidade", "Quantidade", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 254);
            userObjectController.AddChildTableToUserObject("CVA_SIM_VENDA", "@CVA_SIM_VENDA_ITEM");

            userObjectController.CreateUserObject("CVA_SIM_VENDA", "CVA - Simulador Vendas", "@CVA_SIM_VENDA", BoUDOObjType.boud_MasterData, false, false, false, false, false, false, true, 0, 0, "", null);

            userObjectController.CreateUserTable("CVA_SIM_VENDA_OP", "CVA - Simul. Vendas OP", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_OP", "ItemCode", "Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 60);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_OP", "OP", "OP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_OP", "Fis", "Fis", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_OP", "Res", "Res", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_OP", "Enc", "Enc", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
            userObjectController.InsertUserField("@CVA_SIM_VENDA_OP", "Dis", "Dis", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);



            #endregion
        }
    }
}
