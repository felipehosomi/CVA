using System;
using SAPbobsCOM;
using SBO.Hub;
using SBO.Hub.Helpers;


namespace CVA.Fibra.ConciliacaoCartaCredito.Core.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            try
            {
                UserObject userObject = new UserObject();

                userObject.CreateUserTable("@CVA_CONFIG_DEPOSIT", "CVA - Depósito Cartão Crédito", BoUTBTableType.bott_NoObject);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "AccountBoe", "Conta Boleto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "AccountPix", "Conta PIX", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "BPLIdBoe", "Filial Boleto", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "AccountComission", "Conta Taxa Operação", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "AccountAdvance", "Conta Taxa Antecipação", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "AccountCancel", "Conta Cancelamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "AccountFees", "Conta Outras Taxas", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObject.InsertUserField("@CVA_CONFIG_DEPOSIT", "AccountcreditISS", "Conta Recebimento Créditos de ISS", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);

                userObject.CreateUserTable("@CVA_DEPOSIT_LOG", "CVA - Log Depósito", BoUTBTableType.bott_NoObject);
                userObject.InsertUserField("@CVA_DEPOSIT_LOG", "NSU", "NSU", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
                userObject.InsertUserField("@CVA_DEPOSIT_LOG", "TransId", "ID LCM", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObject.InsertUserField("@CVA_DEPOSIT_LOG", "Type", "Tipo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
                userObject.AddValidValueToUserField("@CVA_DEPOSIT_LOG", "U_Type", "1", "Boleto");
                userObject.AddValidValueToUserField("@CVA_DEPOSIT_LOG", "U_Type", "2", "Cancelamento");
                userObject.AddValidValueToUserField("@CVA_DEPOSIT_LOG", "U_Type", "3", "Taxa Transferência");
                userObject.AddValidValueToUserField("@CVA_DEPOSIT_LOG", "U_Type", "4", "Demais Taxas");
                userObject.AddValidValueToUserField("@CVA_DEPOSIT_LOG", "U_Type", "5", "Estorno");
                userObject.AddValidValueToUserField("@CVA_DEPOSIT_LOG", "U_Type", "6", "PIX");
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage("Erro ao criar campos de usuário: " + ex.Message);
            }
        }
    }
}
