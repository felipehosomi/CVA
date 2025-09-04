using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ImportadorFolha.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();
            //userObjectController.RemoveUserTable("@CVA_FOLHA_ITEM");
            //userObjectController.RemoveUserObject("CVA_FOLHA");
            //userObjectController.RemoveUserTable("@CVA_FOLHA");

            userObjectController.CreateUserTable("CVA_FOLHA", "CVA - Importação Folha", BoUTBTableType.bott_MasterData);
            userObjectController.CreateUserTable("CVA_FOLHA_ITEM", "CVA - Importação Folha", BoUTBTableType.bott_MasterDataLines);

            userObjectController.InsertUserField("@CVA_FOLHA_ITEM", "Posicao", "Posição", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_FOLHA_ITEM", "Tipo_Campo", "Tipo Campo LCM", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Tipo_Campo", "1", "Alfanumérico");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Tipo_Campo", "2", "Inteiro");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Tipo_Campo", "3", "Decimal");

            userObjectController.InsertUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "Campo LCM", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "AccountCode", "Conta Contábil");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "ShortName", "Código PN");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "Credit", "Crédito");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "Debit", "Débito");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "ProjectCode", "Projeto");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "LineMemo", "Observação");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "CostingCode", "Centro de Custo");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "CostingCode2", "Centro de Custo 2");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "CostingCode3", "Centro de Custo 3");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "CostingCode4", "Centro de Custo 4");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "CostingCode5", "Centro de Custo 5");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "Reference1", "Referência");
            userObjectController.AddValidValueToUserField("@CVA_FOLHA_ITEM", "Campo_LCM", "Reference2", "Referência 2");

            userObjectController.InsertUserField("@CVA_FOLHA_ITEM", "Consulta", "Consulta Formatada", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);

            //userObjectController.InsertUserField("@CVA_FOLHA_ITEM", "Tabela", "Tabela", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("@CVA_FOLHA_ITEM", "Campo_De", "Campo Origem", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            //userObjectController.InsertUserField("@CVA_FOLHA_ITEM", "Campo_Para", "Campo Destino", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserObject("CVA_FOLHA", "CVA - Importação Folha", "@CVA_FOLHA", BoUDOObjType.boud_MasterData);
            userObjectController.AddChildTableToUserObject("CVA_FOLHA", "@CVA_FOLHA_ITEM");
        }
    }
}
