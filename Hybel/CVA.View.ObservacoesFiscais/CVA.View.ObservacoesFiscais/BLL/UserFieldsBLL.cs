using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ObservacoesFiscais.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            userObjectController.CreateUserTable("CVA_OBSAUTO", "CVA - Observação", BoUTBTableType.bott_MasterData);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "Status", "Status", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "Status", "Y", "Sim", true);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "Status", "N", "Não");

            userObjectController.InsertUserField("@CVA_OBSAUTO", "Texto", "Texto", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);
            userObjectController.InsertUserField("@CVA_OBSAUTO", "OINV", "Nota Fiscal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OINV", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OINV", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "OQUT", "Cotação", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OQUT", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OQUT", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "ORDR", "Pedido Venda", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORDR", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORDR", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "ODLN", "Entrega", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ODLN", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ODLN", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "ORDN", "Devolução", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORDN", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORDN", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "ORIN", "Dev. Nota Saída", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORIN", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORIN", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "OINV_F", "NF Receb. Futuro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OINV_F", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OINV_F", "N", "Não", true);

            //userObjectController.InsertUserField("@CVA_OBSAUTO", "ODPI", "Adiantamento Cliente", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ODPI", "Y", "Sim");
            //userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ODPI", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "OPRQ", "Solicitação Compra", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPRQ", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPRQ", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "OPCH", "Nota Fiscal Entrada", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPCH", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPCH", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "ORPC", "Dev. NF Entrada", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORPC", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORPC", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "ORPD", "Dev. Mercadorias", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORPD", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ORPD", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "OPOR", "Pedido Compra", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPOR", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPOR", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "OPDN", "Recebimento Mercadoria", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPDN", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPDN", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "OPCH_F", "NF Entrega Futura", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPCH_F", "Y", "Sim");
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "OPCH_F", "N", "Não", true);

            //userObjectController.InsertUserField("@CVA_OBSAUTO", "ODPO", "Adiantamento Fornecedor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ODPO", "Y", "Sim");
            //userObjectController.AddValidValueToUserField("@CVA_OBSAUTO", "ODPO", "N", "Não", true);

            userObjectController.InsertUserField("@CVA_OBSAUTO", "Parent", "Predecessor", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);

            userObjectController.CreateUserObject("CVA_OBSAUTO", "CVA - Observação", "@CVA_OBSAUTO", BoUDOObjType.boud_MasterData);

            userObjectController.CreateUserTable("CVA_OBSAUTO1", "CVA - Obs. Linhas", BoUTBTableType.bott_MasterDataLines);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "Status", "Status", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO1", "Status", "Y", "Sim", true);
            userObjectController.AddValidValueToUserField("@CVA_OBSAUTO1", "Status", "N", "Não");

            userObjectController.InsertUserField("@CVA_OBSAUTO1", "ItemCode", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "Usage", "Utilização", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "TaxCode", "Cód. Imposto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "CFOP", "CFOP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "NCM", "NCM", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "CardCode", "Cód. PN", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "GroupNum", "Grupo Cliente", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "State", "UF", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "City", "Cidade", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "BPLId", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "SQL", "SQL", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);
            userObjectController.InsertUserField("@CVA_OBSAUTO1", "Itmsgrpnam", "Grupo Itens", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.AddChildTableToUserObject("CVA_OBSAUTO", "@CVA_OBSAUTO1");
        }
    }
}
