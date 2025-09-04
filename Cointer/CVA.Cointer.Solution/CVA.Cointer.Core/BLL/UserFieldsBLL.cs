using SAPbobsCOM;
using SBO.Hub;
using SBO.Hub.Helpers;
using System;

namespace CVA.Cointer.Core.BLL
{
    class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            try
            {
                UserObject userObject = new UserObject();
                userObject.CreateUserTable("CVA_CONSIGNMENT", "[CVA] Consignação", BoUTBTableType.bott_MasterData);
                userObject.InsertUserField("@CVA_CONSIGNMENT", "DocEntry", "DocEntry NF", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObject.InsertUserField("@CVA_CONSIGNMENT", "LineNum", "LineNum", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObject.InsertUserField("@CVA_CONSIGNMENT", "BatchNum", "Lote", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObject.InsertUserField("@CVA_CONSIGNMENT", "Quantity", "Quantidade", BoFieldTypes.db_Float, BoFldSubTypes.st_Quantity, 10);
                userObject.InsertUserField("@CVA_CONSIGNMENT", "DraftEntry", "DocEntry Esboço", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

                userObject.CreateUserObject("CVA_CONSIGNMENT", "[CVA] Consignação", "CVA_CONSIGNMENT", BoUDOObjType.boud_MasterData);

                userObject.InsertUserField("OINV", "CVA_DocEntryDev", "DocEntry Devolução", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObject.InsertUserField("OINV", "CVA_DocEntryDraft", "DocEntry Esboço", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage("Erro ao criar campos de usuário: " + ex.Message);
            }
        }
    }
}
