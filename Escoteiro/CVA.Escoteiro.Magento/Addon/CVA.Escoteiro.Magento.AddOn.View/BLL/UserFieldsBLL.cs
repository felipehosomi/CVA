using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;
using System.Collections.Generic;

namespace CVA.Escoteiro.Magento.AddOn.View
{
    class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            userObjectController.InsertUserField("OCRD", "CVA_EntityId", "ID Cliente Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.InsertUserField("ORDR", "CVA_EntityId", "ID Pedido Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("ORDR", "CVA_Increment_id", "Nº Pedido Site Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("ORDR", "CVA_IntegratedCancellation", "Magento|Cancelamento integrado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.AddValidValueToUserField("ORDR", "CVA_IntegratedCancellation", "N", "Não", true);
            userObjectController.AddValidValueToUserField("ORDR", "CVA_IntegratedCancellation", "Y", "Sim");

            userObjectController.InsertUserField("OITM", "CVA_ShortDescription", "Magento|Descrição curta", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 0);
            userObjectController.InsertUserField("OITM", "CVA_Url", "Magento|URL", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 0);
            userObjectController.InsertUserField("OITM", "CVA_Integrated", "Magento|Integrado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.AddValidValueToUserField("OITM", "CVA_Integrated", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OITM", "CVA_Integrated", "Y", "Sim");

            userObjectController.InsertUserField("ORCT", "CVA_EntityId", "ID Pedido Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("ORCT", "CVA_Increment_id", "Nº Pedido Site Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserTable("CVA_ORDERS_MAGENTO", "Pedido de Venda Magento", BoUTBTableType.bott_NoObjectAutoIncrement);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "EntityId", "ID Pedido Magento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "State", "Situação do Pedido", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "Data", "Data Criação", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "Hora", "Hora Criação", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "JSON", "JSON", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "StatusProc", "Status Processamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 3);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "Mensagem", "Mensagem", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "DataProc", "Data Processado", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "HoraProc", "Hora Processao", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "DocEntry", "Nº Pedido SAP", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_ORDERS_MAGENTO", "Integrar", "Integrar Pedido Magento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);

            userObjectController.CreateUserTable("CVA_MAGENTO_PARAM", "Magento Parâmetrizações", BoUTBTableType.bott_NoObjectAutoIncrement);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "BplID", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "Deposito", "Depósito", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "Sequencia", "Sequência", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "Utilizacao", "Utilização", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "Series", "Série Cliente", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "TaxExpsCode", "Imposto do Frete", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None,10);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "Metodo", "Método Crédito Magento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "State", "State Pagamento Pedido", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "Status", "Status Pagamento Pedido", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "CrTypeName", "Método Crédito SAP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "CashAccount", "Conta contábil de pagamento com dinheiro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 15);
            userObjectController.InsertUserField("@CVA_MAGENTO_PARAM", "CreditCardOp", "Operadora do cartão de crédito", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

            userObjectController.CreateUserTable("CVA_MAGENTO_DT", "Magento Registro de Datas", BoUTBTableType.bott_NoObjectAutoIncrement);
            userObjectController.InsertUserField("@CVA_MAGENTO_DT", "EndPoint", "EndPoint", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_MAGENTO_DT", "DataCreate", "Data Criação", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_MAGENTO_DT", "HoraCreate", "Hora Criação", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10);
            userObjectController.InsertUserField("@CVA_MAGENTO_DT", "SegundoCreate", "Segundo Criação", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Time, 10);
            userObjectController.InsertUserField("@CVA_MAGENTO_DT", "DataUpdate", "Data Update", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_MAGENTO_DT", "HoraUpdate", "Hora Update", BoFieldTypes.db_Date, BoFldSubTypes.st_Time, 10);
            userObjectController.InsertUserField("@CVA_MAGENTO_DT", "SegundoUpdate", "Segundo Update", BoFieldTypes.db_Numeric, BoFldSubTypes.st_Time, 10);

            userObjectController.CreateUserTable("CVA_OICT", "[CVA] Categorias do produto", BoUTBTableType.bott_NoObject);
            userObjectController.InsertUserField("@CVA_OICT", "ID", "ID da categoria", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_OICT", "Name", "Nome da categoria", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_OICT", "ItemCode", "Cód. Item", BoFieldTypes.db_Alpha, BoFldSubTypes.st_Time, 50);
        }

        public static void InsertDefaultData()
        {
        }
    }
}
