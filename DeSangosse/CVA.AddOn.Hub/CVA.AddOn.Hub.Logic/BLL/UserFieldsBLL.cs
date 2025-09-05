using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            userObjectController.InsertUserField("OSTC", "CVA_ObsNF", "Observação NF", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);

            //userObjectController.InsertUserField("OPDN", "CVA_Permite_PN", "Permite Lançamento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            //userObjectController.AddValidValueToUserField("OPDN", "CVA_Permite_PN", "N", "Não", true);
            //userObjectController.AddValidValueToUserField("OPDN", "CVA_Permite_PN", "Y", "Sim");

            userObjectController.InsertUserField("OITB", "CVA_Validacao", "Desativa Validação", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OITB", "CVA_Validacao", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OITB", "CVA_Validacao", "S", "Sim");
            
            userObjectController.InsertUserField("OUSG", "BloqQtde", "Bloqueia Qtde", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OUSG", "BloqQtde", "S", "Sim", true);
            userObjectController.AddValidValueToUserField("OUSG", "BloqQtde", "N", "Não");

            userObjectController.InsertUserField("OPOR", "CVA_Tipo_Frete", "Tipo Frete", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Tipo_Frete", "1", "Frete Fracionado");
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Tipo_Frete", "2", "Frete Complemento");
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Tipo_Frete", "3", "Frete Dedicado");
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Tipo_Frete", "4", "Frete Emergencial");

            userObjectController.InsertUserField("OPOR", "CVA_Frete_Marca", "Marca - Frete Dedicado", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Frete_Marca", "1", "Carreta");
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Frete_Marca", "2", "Toco");
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Frete_Marca", "3", "Truck");
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Frete_Marca", "4", "3/4");
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Frete_Marca", "5", "Utilitários");

            userObjectController.InsertUserField("OPOR", "CVA_Rateio_Frete", "Rateio Frete Efetuado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Rateio_Frete", "N", "Não", true);
            userObjectController.AddValidValueToUserField("OPOR", "CVA_Rateio_Frete", "Y", "Sim");

            userObjectController.InsertUserField("OPOR", "CVA_MotivoCancel", "Motivo Cancelamento", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 500);

            userObjectController.CreateUserTable("CVA_DOC_FRETE", "Docs. Relacionados Frete", BoUTBTableType.bott_NoObject);

            userObjectController.InsertUserField("@CVA_DOC_FRETE", "PO_DocEntry", "DocEntry pedido compra", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DOC_FRETE", "DocType", "Tipo Documento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "17", "Pedido de Venda");
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "15", "Entrega");
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "13", "Nota Fiscal de Saída");
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "14", "Dev. Nota Fiscal de Saída");
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "22", "Pedido de Compra");
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "20", "Recebimento de Mercadoria");
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "18", "Nota Fiscal de Entrada");
            userObjectController.AddValidValueToUserField("@CVA_DOC_FRETE", "DocType", "19", "Dev. Nota Fiscal de Entrada");

            userObjectController.InsertUserField("@CVA_DOC_FRETE", "DocEntry", "DocEntry do Documento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DOC_FRETE", "DocNum", "DocNum do Documento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DOC_FRETE", "Serial", "Serial do Documento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);

            userObjectController.CreateUserTable("LOTES_PV", "[CVA] Lotes Pedido de Venda", BoUTBTableType.bott_NoObject);

            userObjectController.InsertUserField("@LOTES_PV", "Itemcode", "Itemcode", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
            userObjectController.InsertUserField("@LOTES_PV", "Lote"    , "Lote", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 30);
        }
    }
}
