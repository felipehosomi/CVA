using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.View.DIME.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
            //select U_ExcAmtS, U_OthAmtS,*from INV4
            userObjectController.InsertUserField("INV4", "ExcAmtS", "ExcAmtS", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("INV4", "OthAmtS", "ExcAmtS", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);

            #region CVA_DIME
            userObjectController.CreateUserTable("CVA_DIME", "CVA - DIME", BoUTBTableType.bott_MasterData);
            userObjectController.InsertUserField("@CVA_DIME", "Filial", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
            userObjectController.InsertUserField("@CVA_DIME", "Periodo", "Periodo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10, true);
            userObjectController.InsertUserField("@CVA_DIME", "DtDe", "Data de", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, true);
            userObjectController.InsertUserField("@CVA_DIME", "DtAte", "Data até", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, true);
            userObjectController.InsertUserField("@CVA_DIME", "Dir", "Diretório", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, true);

            userObjectController.InsertUserField("@CVA_DIME", "Declaracao", "Tipo Declaração", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Declaracao", "1", "Normal", true);

            userObjectController.InsertUserField("@CVA_DIME", "Apuracao", "Tipo Apuração", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Apuracao", "1", "Não é apuração consolidada");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Apuracao", "2", "É estabelecimento consolidador");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Apuracao", "3", "É estabelecimento consolidado");

            userObjectController.InsertUserField("@CVA_DIME", "Trans_Cred", "Trans. Créd. Período", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Trans_Cred", "1", "Não apurou ou reservou nem recebeu créditos");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Trans_Cred", "2", "Apurou ou reservou créditos");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Trans_Cred", "3", "Recebeu créditos");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Trans_Cred", "4", "Apurou ou reservou e recebeu créditos");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Trans_Cred", "5", "Apuração e reserva crédito sistema cooperativo agropecuário");

            userObjectController.InsertUserField("@CVA_DIME", "Movimento", "Tipo Movimento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Movimento", "1", "Sem movimento e sem saldos");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Movimento", "2", "Sem movimento e com saldos");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Movimento", "3", "Com movimento");

            userObjectController.InsertUserField("@CVA_DIME", "ST", "Substituto Tributário", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "ST", "1", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "ST", "2", "Não");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "ST", "3", "Substituído solidário");

            userObjectController.InsertUserField("@CVA_DIME", "Escrita", "Tem Escrita Contábil", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2, true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Escrita", "1", "Sim é o estabelecimento principal");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Escrita", "2", "Não");
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Escrita", "3", "Sim, dados informados no estabelecimento principal");

            userObjectController.InsertUserField("@CVA_DIME", "Empregados", "Qtde. Trab. Atividade", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);

            userObjectController.InsertUserField("@CVA_DIME", "Saldo_Credor", "Saldo Credor", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            // Quadro 04
            userObjectController.InsertUserField("@CVA_DIME", "Deb_Ativo", "Débito Ativo Perm.", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("@CVA_DIME", "Dif_Uso_Cons", "Diferencial Uso/Consumo", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("@CVA_DIME", "CIAP", "ICMS CIAP", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);

            // Quadro 05
            userObjectController.InsertUserField("@CVA_DIME", "Cred_Ativo", "Créd. Ativo Perm.", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("@CVA_DIME", "Cred_Outros", "Outros Créditos", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("@CVA_DIME", "Cred_ST", "Créd. ICMS-ST", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);

            // Gera quadros 80 e 84
            userObjectController.InsertUserField("@CVA_DIME", "Info_ED", "Info. Estoque/Despesa", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Info_ED", "1", "Sim", true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Info_ED", "2", "Não");

            // Gera quadros 81, 82 e 83
            userObjectController.InsertUserField("@CVA_DIME", "Demo_Cont", "Demonstrativos Contábeis", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Demo_Cont", "1", "Sim", true);
            userObjectController.AddValidValueToUserField("@CVA_DIME", "Demo_Cont", "2", "Não");

            userObjectController.CreateUserObject("CVA_DIME", "CVA - DIME", "@CVA_DIME", BoUDOObjType.boud_MasterData);
            #endregion

            #region CVA_DIME_09
            userObjectController.CreateUserTable("CVA_DIME_09", "CVA - DIME - Quadro 09", BoUTBTableType.bott_MasterData);

            userObjectController.InsertUserField("@CVA_DIME_09", "Periodo", "Periodo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DIME_09", "Filial", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserObject("CVA_DIME_09", "CVA - DIME - Quadro 09", "@CVA_DIME_09", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_DIME_09_ITEM", "CVA - DIME - Quadro 09 - Item", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_DIME_09_ITEM", "Item", "Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
            userObjectController.InsertUserField("@CVA_DIME_09_ITEM", "Valor", "Valor", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);

            userObjectController.AddChildTableToUserObject("CVA_DIME_09", "@CVA_DIME_09_ITEM");
            #endregion

            #region CVA_DIME_12
            userObjectController.CreateUserTable("CVA_DIME_12", "CVA - DIME - Quadro 12", BoUTBTableType.bott_MasterData);

            userObjectController.InsertUserField("@CVA_DIME_12", "Periodo", "Periodo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DIME_12", "Filial", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserObject("CVA_DIME_12", "CVA - DIME - Quadro 12 - Item", "@CVA_DIME_12", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_DIME_12_ITEM", "CVA - DIME - Quadro 12", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_DIME_12_ITEM", "Origem", "Origem", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.AddValidValueToUserField("@CVA_DIME_12_ITEM", "Origem", "1", "Imposto");
            userObjectController.AddValidValueToUserField("@CVA_DIME_12_ITEM", "Origem", "2", "Substituição Tributária");
            userObjectController.AddValidValueToUserField("@CVA_DIME_12_ITEM", "Origem", "3", "Débitos Específicos");
            userObjectController.InsertUserField("@CVA_DIME_12_ITEM", "Receita", "Cód. Receita", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DIME_12_ITEM", "Vencimento", "Vencimento", BoFieldTypes.db_Date, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("@CVA_DIME_12_ITEM", "Valor", "Valor", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("@CVA_DIME_12_ITEM", "Classe", "Cód. Classe", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DIME_12_ITEM", "NrAcordo", "Número do Acordo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);

            userObjectController.AddChildTableToUserObject("CVA_DIME_12", "@CVA_DIME_12_ITEM");
            #endregion

            //#region CVA_DIME_25
            //userObjectController.CreateUserTable("CVA_DIME_25", "CVA - DIME - Quadro 25", BoUTBTableType.bott_MasterData);

            //userObjectController.InsertUserField("@CVA_DIME_25", "Periodo", "Periodo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            //userObjectController.InsertUserField("@CVA_DIME_25", "Filial", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            //userObjectController.CreateUserObject("CVA_DIME_25", "CVA - DIME - Quadro 25 - Item", "@CVA_DIME_25", BoUDOObjType.boud_MasterData);

            //userObjectController.AddChildTableToUserObject("CVA_DIME_12", "@CVA_DIME_12_ITEM");
            //#endregion

            #region CVA_DIME_46
            userObjectController.CreateUserTable("CVA_DIME_46", "CVA - DIME - Quadro 46", BoUTBTableType.bott_MasterData);

            userObjectController.InsertUserField("@CVA_DIME_46", "Periodo", "Periodo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DIME_46", "Filial", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserObject("CVA_DIME_46", "CVA - DIME - 46", "@CVA_DIME_46", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_DIME_46_ITEM", "CVA - DIME - Quadro 46 - Item", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_DIME_46_ITEM", "Identificacao", "Identificação", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
            userObjectController.InsertUserField("@CVA_DIME_46_ITEM", "Valor", "Valor", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
            userObjectController.InsertUserField("@CVA_DIME_46_ITEM", "Origem", "Origem", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.AddValidValueToUserField("@CVA_DIME_46_ITEM", "Origem", "1", "Crédito por transferência de créditos");
            userObjectController.AddValidValueToUserField("@CVA_DIME_46_ITEM", "Origem", "14", "Créditos por DCIP");
            userObjectController.AddValidValueToUserField("@CVA_DIME_46_ITEM", "Origem", "16", "ICMS-ST");

            userObjectController.AddChildTableToUserObject("CVA_DIME_46", "@CVA_DIME_46_ITEM");
            #endregion
        }
    }
}
