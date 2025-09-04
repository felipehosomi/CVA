using CVA.AddOn.Common.DataBase;
using SAPbobsCOM;
using System.IO;

namespace CVA.View.Dctf.BLL
{
    public class UserFieldsBLL
    {
        public static void CreateUserFields()
        {
            UserObjectController userObjectController = new UserObjectController();

            #region CVA_DCTF
            userObjectController.CreateUserTable("CVA_DCTF", "CVA - DCTF", BoUTBTableType.bott_MasterData);
            userObjectController.InsertUserField("@CVA_DCTF", "Dir", "Diretório", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 254);
            userObjectController.InsertUserField("@CVA_DCTF", "CNPJ", "CNPJ", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50, true);
            userObjectController.InsertUserField("@CVA_DCTF", "Mes", "Mês Apuração", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 2);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "1", "Janeiro");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "2", "Fevereiro");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "3", "Março");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "4", "Abril");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "5", "Maio");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "6", "Junho");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "7", "Julho");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "8", "Agosto");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "9", "Setembro");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "10", "Outubro");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "11", "Novembro");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Mes", "12", "Dezembro");

            userObjectController.InsertUserField("@CVA_DCTF", "Ano", "Ano Apuração", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 4);
            userObjectController.InsertUserField("@CVA_DCTF", "SitEspecial", "Situação Especial", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");
            userObjectController.InsertUserField("@CVA_DCTF", "DtEvento", "Data Evento", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10);
            userObjectController.InsertUserField("@CVA_DCTF", "Evento", "Evento", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Evento", "0", "Normal", true);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Evento", "1", "Extinção");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Evento", "2", "Fusão");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Evento", "3", "Incorporação/Incorporada");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Evento", "4", "Incorporação/Incorporadora");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Evento", "5", "Cisão Total");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Evento", "6", "Cisão Parcial");

            userObjectController.InsertUserField("@CVA_DCTF", "DtDe", "Data De", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, true);
            userObjectController.InsertUserField("@CVA_DCTF", "DtAte", "Data Até", BoFieldTypes.db_Date, BoFldSubTypes.st_None, 10, true);

            userObjectController.InsertUserField("@CVA_DCTF", "Retific", "Declaração Retificadora", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");
            userObjectController.InsertUserField("@CVA_DCTF", "NrRetific", "Nº Recibo Retific.", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 12);
            userObjectController.InsertUserField("@CVA_DCTF", "SimplesNacional", "Simples Nacional", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");
            userObjectController.InsertUserField("@CVA_DCTF", "QualPJ", "Qualificação PJ", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2, true);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "01", "Financeira");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "02", "– Sociedade Seguradora, de Capitalização ou Entidade Aberta de Previdência Complementar (com fins lucrativos)");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "03", "Corretora Autônoma de Seguro");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "04", "Cooperativa de Crédito ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "05", "Entidade Fechada de Previdência Complementar ou Entidade Aberta de Previdência Complementar (sem fins lucrativos)");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "06", "Sociedade Cooperativa ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "07", "PJ em Geral");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "08", "Sociedade Cooperativa de Produção Agropecuária ou de Consumo");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "09", "Autarquia/Fundação Pública");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "10", "Órgão Público da Administração Direta");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "11", "Empresa Pública/Sociedade de Economia Mista/Demais PJ de que trata o inc. III, art. 34, Lei 10.833/2005 ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "QualPJ", "12", "Mais de uma qualificação durante o mês");

            userObjectController.InsertUserField("@CVA_DCTF", "Tributacao", "Forma tributação", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1, true);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Tributacao", "0", "Real/Trimestral ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Tributacao", "1", "Real/Estimativa ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Tributacao", "2", "Presumido");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Tributacao", "3", "Arbitrado");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Tributacao", "4", "Imune do IRPJ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Tributacao", "5", "Isenta do IRPJ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Tributacao", "8", "Declarante não é Contribuinte do IRPJ");

            userObjectController.InsertUserField("@CVA_DCTF", "Balanco", "Levantou balanço mês", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");
            userObjectController.InsertUserField("@CVA_DCTF", "SCP", "Débitos SCP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");
            userObjectController.InsertUserField("@CVA_DCTF", "CPRB", "Optante CPRB", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 1, false, "N");

            userObjectController.InsertUserField("@CVA_DCTF", "SitPJ", "Situação PJ mês", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "SitPJ", "0", "Não preenchido");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "SitPJ", "1", "PJ iniciou atividades no mês da declaração");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "SitPJ", "2", "PJ excluída do Simples no mês da declaração");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "SitPJ", "3", "Surgimento de nova PJ em razão de fusão ou cisão no mês da declaração");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "SitPJ", "4", "PJ não se enquadra em nenhuma das situações anteriores no mês da declaração");

            userObjectController.InsertUserField("@CVA_DCTF", "Lei", "Opções Lei 12.973/2014", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1, true);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Lei", "0", "Não preenchido");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Lei", "1", "Aplicação das disposições contidas nos arts. 1º e 2º e 4º a 70");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Lei", "2", "Aplicação das disposições contidas nos arts. 76 a 92");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Lei", "3", "Aplicação das disposições contidas nos arts. 1º e 2º e 4º a 70 e 76 a 92 ");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "Lei", "4", "Não optante");

            userObjectController.InsertUserField("@CVA_DCTF", "VarMon", "Variações Monetárias", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1, true);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "VarMon", "0", "Não preenchido");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "VarMon", "1", "Regime de Caixa");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "VarMon", "2", "Regime de Competência");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "VarMon", "3", "Regime de Caixa – Portaria Ministerial");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "VarMon", "4", "Não se aplica");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "VarMon", "5", "Sem alteração do regime");

            userObjectController.InsertUserField("@CVA_DCTF", "PIS", "Regime PIS/COFINS", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 1, true);
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "PIS", "0", "Não preenchido");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "PIS", "1", "Não-cumulativo");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "PIS", "2", "Cumulativo");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "PIS", "3", "Não-cumulativo e Cumulativo");
            userObjectController.AddValidValueToUserField("@CVA_DCTF", "PIS", "4", "Não se aplica");

            userObjectController.InsertUserField("@CVA_DCTF", "NomeEmp", "Nome Empresarial", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 115, true);
            userObjectController.InsertUserField("@CVA_DCTF", "Rua", "Rua", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 40, true);
            userObjectController.InsertUserField("@CVA_DCTF", "RuaNr", "Número", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 6);
            userObjectController.InsertUserField("@CVA_DCTF", "Complemento", "Complemento", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 21);
            userObjectController.InsertUserField("@CVA_DCTF", "Bairro", "Bairro", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 20);
            userObjectController.InsertUserField("@CVA_DCTF", "Municipio", "Municipio", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 50);
            userObjectController.InsertUserField("@CVA_DCTF", "UF", "Estado", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2, true);
            userObjectController.InsertUserField("@CVA_DCTF", "CEP", "CEP", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8, true);
            userObjectController.InsertUserField("@CVA_DCTF", "DDDTel", "DDD Telefone", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 4);
            userObjectController.InsertUserField("@CVA_DCTF", "Telefone", "Telefone", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 9);
            userObjectController.InsertUserField("@CVA_DCTF", "DDDFax", "DDD Fax", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 4);
            userObjectController.InsertUserField("@CVA_DCTF", "Fax", "Fax", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 9);
            userObjectController.InsertUserField("@CVA_DCTF", "CaixaPostal", "Caixa Postal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 6);
            userObjectController.InsertUserField("@CVA_DCTF", "UF_CP", "UF Caixa Postal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 2);
            userObjectController.InsertUserField("@CVA_DCTF", "CEP_CP", "CEP Caixa Postal", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 8);
            userObjectController.InsertUserField("@CVA_DCTF", "Email", "E-mail", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 40);

            userObjectController.CreateUserObject("CVA_DCTF", "CVA - DCTF", "@CVA_DCTF", BoUDOObjType.boud_MasterData);
            #endregion

            //StreamWriter sw = new StreamWriter("c:\\CVA Consultoria\\log.txt");
            //sw.WriteLine(userObjectController.Log);
            //sw.Close();
        }
    }
}
