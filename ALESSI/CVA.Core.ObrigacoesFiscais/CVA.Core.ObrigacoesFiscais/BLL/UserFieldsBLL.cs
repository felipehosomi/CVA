using CVA.AddOn.Common;
using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System;
using System.IO;

namespace CVA.Core.ObrigacoesFiscais.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            try
            {
                UserObjectController userObjectController = new UserObjectController();
                userObjectController.InsertUserField("OUSG", "ApropCred", "Aprop. Crédito", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("OUSG", "ApropCred", "Y", "Sim");
                userObjectController.AddValidValueToUserField("OUSG", "ApropCred", "N", "Não", true);

                userObjectController.InsertUserField("OINV", "CVA_Obrig_Fiscal", "Compõe obrigações fiscais", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("OINV", "CVA_Obrig_Fiscal", "Y", "Sim", true);
                userObjectController.AddValidValueToUserField("OINV", "CVA_Obrig_Fiscal", "N", "Não");

                userObjectController.InsertUserField("OCRD", "CVA_SimplesNacional", "Simples Nacional", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("OCRD", "CVA_SimplesNacional", "Y", "Sim");
                userObjectController.AddValidValueToUserField("OCRD ", "CVA_SimplesNacional", "N", "Não", true);

                #region CVA_MODEL_EX
                userObjectController.CreateUserTable("CVA_MODEL_EX", "CVA - Exclusão de modelo", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_MODEL_EX", "Nfm", "Modelo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
                userObjectController.InsertUserField("@CVA_MODEL_EX", "Excluir", "Excluir Modelo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, true);
                userObjectController.AddValidValueToUserField("@CVA_MODEL_EX", "Excluir", "Y", "Sim", true);
                userObjectController.AddValidValueToUserField("@CVA_MODEL_EX", "Excluir", "N", "Não");

                userObjectController.CreateUserObject("CVA_MODEL_EX", "CVA - Exclusão de modelo", "@CVA_MODEL_EX", BoUDOObjType.boud_MasterData);
                #endregion

                #region CVA_LAYOUT
                userObjectController.CreateUserTable("CVA_LAYOUT", "CVA - Layout", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Name", "Nome Layout", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, true);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Separator", "Separador", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Start_Sep", "Inicia com Separador", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Start_Sep", "Y", "Sim");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Start_Sep", "N", "Não", true);

                userObjectController.InsertUserField("@CVA_LAYOUT", "Char_String", "Caracter String", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Pad_String", "Preenc. String", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Pad_String", "1", "Direita");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Pad_String", "2", "Esquerda");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Pad_String", "3", "Nenhum");

                userObjectController.InsertUserField("@CVA_LAYOUT", "Decimal_Sep", "Separador Decimal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Decimal_Qty", "Qtde. Casas Decimais", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Date_Format", "Formato Data", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Char_Num", "Caracter Numérico", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
                userObjectController.InsertUserField("@CVA_LAYOUT", "Pad_Num", "Preenchimento Numérico", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Pad_Num", "1", "Direita");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Pad_Num", "2", "Esquerda");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_Pad_Num", "3", "Nenhum");

                userObjectController.InsertUserField("@CVA_LAYOUT", "File_Format", "Formato Data", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_File_Format", "1", "Padrão", true);
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_File_Format", "2", "ASCII");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_File_Format", "3", "UTF-7");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_File_Format", "4", "UTF-8");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_File_Format", "5", "UTF-32");
                userObjectController.AddValidValueToUserField("@CVA_LAYOUT", "U_File_Format", "6", "Unicode");

                userObjectController.InsertUserField("@CVA_LAYOUT", "Dir", "Dir. Geração Arquivo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);

                userObjectController.CreateUserObject("CVA_LAYOUT", "CVA - Layout", "@CVA_LAYOUT", BoUDOObjType.boud_MasterData);
                userObjectController.MakeFieldsSearchable("@CVA_LAYOUT");
                #endregion

                #region CVA_FILE_MAP / CVA_FILE_MAP_ITEM
                userObjectController.CreateUserTable("CVA_FILE_MAP", "CVA - Geração TXT", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_FILE_MAP", "Layout", "Nome Layout", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254, true);
                userObjectController.InsertUserField("@CVA_FILE_MAP", "Description", "Descrição deo Registro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_FILE_MAP", "Position", "Posição Registro", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
                userObjectController.InsertUserField("@CVA_FILE_MAP", "SqlType", "Tipo Objeto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "SqlType", "SP", "Stored Procedure");
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "SqlType", "FN", "Function");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "ObjType", "VI", "View");
                userObjectController.InsertUserField("@CVA_FILE_MAP", "ObjName", "Nome Objeto", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_FILE_MAP", "Identifier", "Identificador", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
                userObjectController.InsertUserField("@CVA_FILE_MAP", "Child", "Registro Filho", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 100);

                //userObjectController.InsertUserField("@CVA_FILE_MAP", "ObjType", "Tipo Objeto", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "ObjType", "20", "Recebimento de Mercadoria");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "ObjType", "21", "Devolução de Mercadoria");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "ObjType", "18", "Nota Fiscal de Entrada");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "ObjType", "19", "Dev. Nota Fiscal de Entrada");

                //userObjectController.InsertUserField("@CVA_FILE_MAP", "RegType", "Tipo Registro", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "RegType", "1", "Cabeçalho");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "RegType", "2", "Linha");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "DI_Object", "3", "Lote");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "DI_Object", "4", "Série");
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP", "DI_Object", "5", "Parcela");

                userObjectController.CreateUserObject("CVA_FILE_MAP", "Geração TXT", "@CVA_FILE_MAP", BoUDOObjType.boud_MasterData);
                userObjectController.MakeFieldsSearchable("@CVA_FILE_MAP");

                userObjectController.CreateUserTable("CVA_FILE_MAP_ITEM", "CVA - Geração TXT - Itens", BoUTBTableType.bott_MasterDataLines);
                userObjectController.AddChildTableToUserObject("CVA_FILE_MAP", "CVA_FILE_MAP_ITEM");

                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "DI_Field", "Nome Campo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200, true);
                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "DI_Type", "Tipo Campo", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "DI_Type", "1", "Alfanumérico");
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "DI_Type", "2", "Data");
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "DI_Type", "3", "Inteiro");
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "DI_Type", "4", "Decimal");

                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "From", "Posição De", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "To", "Posição Até", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "Size", "Tamanho", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "Decimal", "Casas decimais", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "Format", "Formato", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
                userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "OnlyNumbers", "Somento números", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "OnlyNumbers", "N", "Não", true);
                userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "OnlyNumbers", "Y", "Sim");
                //userObjectController.InsertUserField("@CVA_FILE_MAP_ITEM", "Ignore", "Ignorar", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "Ignore", "N", "Não", true);
                //userObjectController.AddValidValueToUserField("@CVA_FILE_MAP_ITEM", "Ignore", "Y", "Sim");

                userObjectController.CreateUserTable("CVA_FILE_MAP_LINK", "CVA - Geração TXT - Link", BoUTBTableType.bott_MasterDataLines);
                userObjectController.AddChildTableToUserObject("CVA_FILE_MAP", "CVA_FILE_MAP_LINK");

                userObjectController.InsertUserField("@CVA_FILE_MAP_LINK", "Parent", "Campo Pai", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
                userObjectController.InsertUserField("@CVA_FILE_MAP_LINK", "Child", "Campo Filho", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 200);
                #endregion

                #region CVA_GIA_ST
                userObjectController.CreateUserTable("CVA_GIA_ST", "CVA - GIA-ST", BoUTBTableType.bott_MasterData);
                userObjectController.InsertUserField("@CVA_GIA_ST", "Filial", "Filial", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, true);
                userObjectController.InsertUserField("@CVA_GIA_ST", "Periodo", "Periodo Apuração", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 10);
                userObjectController.InsertUserField("@CVA_GIA_ST", "DtDe", "Data de", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, true);
                userObjectController.InsertUserField("@CVA_GIA_ST", "DtAte", "Data até", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, true);

                userObjectController.InsertUserField("@CVA_GIA_ST", "Declarante", "Declarante", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, true);

                userObjectController.InsertUserField("@CVA_GIA_ST", "Sem_Mov", "Sem Movimento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Sem_Mov", "N", "Não", true);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Sem_Mov", "Y", "Sim");

                userObjectController.InsertUserField("@CVA_GIA_ST", "Retificacao", "Retificação", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Retificacao", "N", "Não", true);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Retificacao", "Y", "Sim");

                userObjectController.InsertUserField("@CVA_GIA_ST", "DtVenc", "Data Vencimento", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, true);
                userObjectController.InsertUserField("@CVA_GIA_ST", "Ressarcimento", "Ressarcimento", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
                userObjectController.InsertUserField("@CVA_GIA_ST", "Credito", "Crédito Periodo Anterior", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
                userObjectController.InsertUserField("@CVA_GIA_ST", "Antecipado", "Pagamentos Antecipados", BoFieldTypes.db_Float, BoFldSubTypes.st_Price, 10);
                userObjectController.InsertUserField("@CVA_GIA_ST", "UF", "UF", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "UF", "PR", "Paraná");
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "UF", "RS", "Rio Grande do Sul");
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "UF", "SC", "Santa Catarina");
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "UF", "SP", "São Paulo");

                userObjectController.InsertUserField("@CVA_GIA_ST", "Obs", "Obs", BoFieldTypes.db_Memo, BoFldSubTypes.st_None, 2000);

                userObjectController.InsertUserField("@CVA_GIA_ST", "Petroleo", "Petroleo", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Petroleo", "N", "Não", true);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Petroleo", "Y", "Sim");

                userObjectController.InsertUserField("@CVA_GIA_ST", "Efet_Transf", "Efetuou Transferência", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Efet_Transf", "N", "Não", true);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "Efet_Transf", "Y", "Sim");

                userObjectController.InsertUserField("@CVA_GIA_ST", "EC_8715", "EC Nº 87/15", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "EC_8715", "N", "Não", true);
                userObjectController.AddValidValueToUserField("@CVA_GIA_ST", "EC_8715", "Y", "Sim");

                userObjectController.CreateUserObject("CVA_GIA_ST", "CVA - GIA-ST", "@CVA_GIA_ST", BoUDOObjType.boud_MasterData);
                #endregion
            }
            catch (Exception ex)
            {
                SBOApp.Application.SetStatusBarMessage(ex.Message);
            }
            //StreamWriter sw = new StreamWriter("c:\\CVA Consultoria\\log.txt");
            //sw.WriteLine(userObjectController.Log);
            //sw.Close();
        }
    }
}
