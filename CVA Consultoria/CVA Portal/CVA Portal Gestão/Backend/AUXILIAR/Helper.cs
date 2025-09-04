using MODEL.Classes;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AUXILIAR
{
    public class Helper
    {
        #region Atributos
 
        XDocument doc = XDocument.Load(@"C:\CVA Consultoria\CVA Portal de Gestão\Resource.xml");
        public SqlConnection conn = new SqlConnection();

        #endregion

        public string ReadPassword(string emailAddress)
        {
            string sqlSelect = $"SELECT PAS FROM CVA_USR WHERE EML LIKE '%{emailAddress}%'";
            string password = "";

            conn.ConnectionString = readConnectionString();

            conn.Open();

            SqlCommand commandSelect = new SqlCommand(sqlSelect, conn);
            SqlDataReader result = commandSelect.ExecuteReader();

            while (result.Read())
            {
                password = result["PAS"].ToString();
            }
            conn.Close();

            return password;
        }


        public CollaboratorModel ImportarDadosColaborador()
        {
            var model = new CollaboratorModel();

            byte[] file = File.ReadAllBytes(@"C:\CVA Consultoria\CVA Portal de Gestão - Teste\Colaboradores\Dados_Colaborador.xlsx");
            MemoryStream ms = new MemoryStream(file);
            using (var package = new ExcelPackage(ms))
            {
                //Obtendo a Planilha
                var arquivo = package.Workbook;
                if (arquivo != null)
                {
                    var planilha = arquivo.Worksheets.First();

                    //Informações Gerais
                    model.Nome = planilha.Cells[6, 7].Value.ToString();
                    model.DataNascimento = Convert.ToDateTime(planilha.Cells[7, 7].Value);

                    if (planilha.Cells[8, 7].Value.ToString() == "Feminino")
                        model.Genero = 1;
                    if (planilha.Cells[8, 7].Value.ToString() == "Masculino")
                        model.Genero = 2;

                    if (planilha.Cells[9, 7].Value.ToString() == "Solteiro")
                        model.EstadoCivil = 1;
                    if (planilha.Cells[9, 7].Value.ToString() == "Casado")
                        model.EstadoCivil = 2;
                    if (planilha.Cells[9, 7].Value.ToString() == "Divorciado")
                        model.EstadoCivil = 3;
                    if (planilha.Cells[9, 7].Value.ToString() == "Viúvo")
                        model.EstadoCivil = 4;
                    if (planilha.Cells[9, 7].Value.ToString() == "União Estável")
                        model.EstadoCivil = 5;



                    model.Telefone = planilha.Cells[10, 7].Value.ToString();
                    model.Celular = planilha.Cells[11, 7].Value.ToString();
                    model.Email = planilha.Cells[12, 7].Value.ToString();

                    //Endereço
                    model.Endereco = new AddressModel
                    {
                        ZipCode = planilha.Cells[14, 7].Value.ToString()
                    };

                    //Documentos
                    model.CPF = planilha.Cells[21, 7].Value.ToString();
                    model.RG = planilha.Cells[22, 7].Value.ToString();
                    model.OrgaoEmissor = planilha.Cells[23, 7].Value.ToString();
                    model.ValidadePassaporte = Convert.ToDateTime(planilha.Cells[24, 7].Value);


                    if (planilha.Cells[25, 7].Value.ToString() == "CLT")
                        model.Tipo = new CollaboratorTypeModel { Id = 1 };

                    if (planilha.Cells[25, 7].Value.ToString() == "PJ")
                        model.Tipo = new CollaboratorTypeModel { Id = 2 };

                    if (planilha.Cells[25, 7].Value.ToString() == "Freelancer")
                        model.Tipo = new CollaboratorTypeModel { Id = 3 };

                    if (planilha.Cells[25, 7].Value.ToString() == "Estagiário")
                        model.Tipo = new CollaboratorTypeModel { Id = 4 };


                    model.CNPJ = planilha.Cells[21, 20].Value.ToString();
                    model.EmissaoRG = Convert.ToDateTime(planilha.Cells[22, 20].Value);
                    model.Passaporte = planilha.Cells[23, 20].Value.ToString();
                    model.Naturalidade = planilha.Cells[24, 20].Value.ToString();
                    model.Nacionalidade = planilha.Cells[25, 20].Value.ToString();

                    var especialidadeList = new List<SpecialtyModel>();

                    for (int j = 36; j <= 45; j = j + 9)
                    {
                        for (int i = 6; i < 26; i++)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(planilha.Cells[i, j].Value.ToString()))
                                {
                                    var especialidade = new SpecialtyModel
                                    {
                                        Name = planilha.Cells[i, j - 1].Value.ToString()
                                    };
                                    especialidadeList.Add(especialidade);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    model.Especialidades = especialidadeList;
                }
                return model;
            }
        }




        public string readConnectionString()
        {
            return doc.Element("portal").Element("database").Value;
        }

        public string readSenderEmail()
        {
            return doc.Element("portal").Element("EmailCVA").Element("email").Value;
        }

        public string readSenderPassword()
        {
            return doc.Element("portal").Element("EmailCVA").Element("password").Value;
        }

        public string readHost()
        {
            return doc.Element("portal").Element("conexao").Element("host").Value;
        }

        public string readPort()
        {
            return doc.Element("portal").Element("conexao").Element("port").Value;
        }

        public string readSubject()
        {
            return doc.Element("portal").Element("modeloEmail").Element("subject").Value;
        }

        public string readBody()
        {
            return doc.Element("portal").Element("modeloEmail").Element("body").Value;
        }

        public string GetPDFPath()
        {
            return doc.Element("portal").Element("politicExpense").Element("filePath").Value;

        }

        public string readNotificationBody()
        {
            return doc.Element("portal").Element("modeloEmail").Element("notificationBody").Value;
        }

    }
}
