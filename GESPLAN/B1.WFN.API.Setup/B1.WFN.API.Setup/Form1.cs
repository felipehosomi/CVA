using Sap.Data.Hana;
using SAPbobsCOM;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace B1.WFN.API.Setup
{
    public partial class Form1 : Form
    {
        public Company Company = new Company();
        public string MsgErro = string.Empty;
        public int CodErro;
        private const string CONN_STRING = "0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056";
        private static SAPbouiCOM.Application SBO_Application = null;

        private static HanaConnection HanaConnection;
        public static SqlConnection SqlConnection;

        public Form1()
        {
            InitializeComponent();

            txtUserDB.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtUserDB.Text))
            {
                MessageBox.Show("Informe o usuário do banco de dados.");
                return;
            }

            if (String.IsNullOrEmpty(txtPwdDB.Text))
            {
                MessageBox.Show("Informe a senha do usuário do banco de dados.");
                return;
            }

            try
            {
                #region [ Conexões ]
                txtLog.AppendText("Iniciando conexão à instância aberta do SAP Business One.");

                // Nova instância do objeto Company
                SAPbouiCOM.SboGuiApi oSBOGuiApi = null;
                oSBOGuiApi = new SAPbouiCOM.SboGuiApi();
                oSBOGuiApi.Connect(CONN_STRING);
                SBO_Application = oSBOGuiApi.GetApplication(-1);

                if (SBO_Application == null)
                {
                    txtLog.AppendText(String.Format("\r\n" + "Erro de conexão: Não possível se conectar à instância aberta do SAP Business One. Verifique se há alguma instância aberta do ERP."));
                    return;
                }

                Company = UserObjectsService.oCompany = (SAPbobsCOM.Company)SBO_Application.Company.GetDICompany();

                txtLog.AppendText("\r\n" + "Conexão realizada com sucesso.");

                txtLog.AppendText("\r\n" + "Iniciando conexão ao banco de dados do SAP Business One.");

                if (Company.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    // Definição da conexão no Hana
                    HanaConnection = new HanaConnection($"Server={Company.Server};UserID={txtUserDB.Text};Password={txtPwdDB.Text};Current Schema={Company.CompanyDB}");

                    try
                    {
                        HanaConnection.Open();
                        HanaConnection.Close();
                        txtLog.AppendText("\r\n" + "Conexão realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        txtLog.AppendText("\r\n" + $"Erro de conexão: {ex.Message}");
                        HanaConnection.Close();
                        return;
                    }
                }
                else
                {
                    SqlConnection = new SqlConnection($"Data Source={Company.Server};User ID={txtUserDB.Text};Password={txtPwdDB.Text};Initial Catalog={Company.CompanyDB}");

                    try
                    {
                        SqlConnection.Open();
                        SqlConnection.Close();
                        txtLog.AppendText("\r\n" + "Conexão realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        txtLog.AppendText("\r\n" + $"Erro de conexão: {ex.Message}");
                        SqlConnection.Close();
                        return;
                    }
                }

                #endregion

                #region [ Inserção de UDF ]
                var tableName = String.Empty;
                var tableDesc = String.Empty;

                tableName = "OJDT";
                tableDesc = "Lançamento Contábil Manual";

                txtLog.AppendText("\r\n" + String.Format("Criando campos na tabela {0} ({1})", tableDesc, tableName));

                // UDF
                UserObjectsService.CreateUserField(tableName, "UpdateTS", "Update Full Time", BoFieldTypes.db_Numeric, BoFldSubTypes.st_None, 10, ref txtLog);




                #endregion

                #region [ Inserção de scripts ]
                var resourceNames = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Assembly.GetManifestResourceNames();

                if (Company.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    foreach (var resourceName in Array.FindAll(resourceNames, x => x.EndsWith(".sql") && x.Contains("Hana_Scripts.View")))
                    {
                        try
                        {
                            var resourceParts = resourceName.Split('.');
                            var alterView = CheckView(Company.CompanyDB, resourceParts[resourceParts.Length - 2]);
                            txtLog.AppendText("\r\n" + (alterView ? String.Format("Alterando {0}", resourceName) : String.Format("Criando {0}", resourceName)));

                            HanaConnection.Open();

                            var command = new HanaCommand(alterView ? GetEmbeddedTextContent(resourceName).Replace("create", "alter") : GetEmbeddedTextContent(resourceName), HanaConnection).ExecuteNonQuery();

                            HanaConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            txtLog.AppendText("\r\n" + String.Format(ex.Message));
                            HanaConnection.Close();
                        }
                    }

                    foreach (var resourceName in Array.FindAll(resourceNames, x => x.EndsWith(".sql") && x.Contains("Hana_Scripts.Stored_Procedure")))
                    {
                        try
                        {
                            var resourceParts = resourceName.Split('.');
                            var alterProcedure = CheckProcedure(Company.CompanyDB, resourceParts[resourceParts.Length - 2]);
                            txtLog.AppendText("\r\n" + (alterProcedure ? String.Format("Alterando {0}", resourceName) : String.Format("Criando {0}", resourceName)));

                            HanaConnection.Open();

                            var command = new HanaCommand(alterProcedure ? GetEmbeddedTextContent(resourceName).Replace("create", "alter") : GetEmbeddedTextContent(resourceName), HanaConnection).ExecuteNonQuery();

                            HanaConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            txtLog.AppendText("\r\n" + String.Format(ex.Message));
                            HanaConnection.Close();
                        }
                    }
                }
                else
                {
                    foreach (var resourceName in Array.FindAll(resourceNames, x => x.EndsWith(".sql") && x.Contains("SQL_Server_Scripts.View")))
                    {
                        try
                        {
                            var resourceParts = resourceName.Split('.');
                            var alterView = CheckView(Company.CompanyDB, resourceParts[resourceParts.Length - 2]);
                            txtLog.AppendText("\r\n" + (alterView ? String.Format("Alterando {0}", resourceName) : String.Format("Criando {0}", resourceName)));

                            SqlConnection.Open();

                            var command = new SqlCommand(alterView ? GetEmbeddedTextContent(resourceName).Replace("CREATE", "ALTER") : GetEmbeddedTextContent(resourceName), SqlConnection).ExecuteNonQuery();

                            SqlConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            txtLog.AppendText("\r\n" + String.Format(ex.Message));
                            SqlConnection.Close();
                        }
                    }

                    foreach (var resourceName in Array.FindAll(resourceNames, x => x.EndsWith(".sql") && x.Contains("SQL_Server_Scripts.Stored_Procedure")))
                    {
                        try
                        {
                            var resourceParts = resourceName.Split('.');
                            var alterProcedure = CheckProcedure(Company.CompanyDB, resourceParts[resourceParts.Length - 2]);
                            txtLog.AppendText("\r\n" + (alterProcedure ? String.Format("Alterando {0}", resourceName) : String.Format("Criando {0}", resourceName)));

                            SqlConnection.Open();

                            var command = new SqlCommand(alterProcedure ? GetEmbeddedTextContent(resourceName).Replace("create", "alter") : GetEmbeddedTextContent(resourceName), SqlConnection).ExecuteNonQuery();

                            SqlConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            txtLog.AppendText("\r\n" + String.Format(ex.Message));
                            SqlConnection.Close();
                        }
                    }
                }

                try
                {
                    txtLog.AppendText("\r\n" + "Alterando a PostTransactionNotice");

                    if (Company.DbServerType == BoDataServerTypes.dst_HANADB)
                    {
                        HanaConnection.Open();

                        var command = new HanaCommand($@"select ""DEFINITION"" from ""PUBLIC"".""PROCEDURES"" where ""PROCEDURE_NAME"" like 'SBO_SP_POSTTRANSACTIONNOTICE' and ""SCHEMA_NAME"" like '{Company.CompanyDB}'", HanaConnection);
                        var reader = command.ExecuteReader();
                        reader.Read();
                        var postTransactionScript = reader.GetString(0);
                        postTransactionScript = postTransactionScript.Replace(@"DEFINITION-- B1 DEPENDS: BEFORE:PT:PROCESS_START", "");
                        postTransactionScript = postTransactionScript.Replace(@"CREATE", "ALTER");
                        var newScriptpPosition = postTransactionScript.IndexOf("-- Select the return values", 0);
                        var newScript = @"if (:object_type = '30' and (:transaction_type = 'A' or :transaction_type = 'U')) then
	                                      update OJDT
	                                         set OJDT.""U_UpdateTS"" = cast(replace(cast(current_time as varchar), ':', '') as int)
	                                       where OJDT.""TransId"" = list_of_cols_val_tab_del;
                                      end if;

                                      ";

                        if (!postTransactionScript.Contains(newScript))
                        {
                            postTransactionScript = postTransactionScript.Insert(postTransactionScript.IndexOf("-- Select the return values", 0), newScript);
                            var alterCommand = new HanaCommand(postTransactionScript, HanaConnection).ExecuteNonQuery();
                        }

                        HanaConnection.Close();
                    }
                    else
                    {
                        SqlConnection.Open();

                        var command = new SqlCommand(@"SELECT OBJECT_DEFINITION (OBJECT_ID(N'SBO_SP_POSTTRANSACTIONNOTICE'))", SqlConnection);
                        var reader = command.ExecuteReader();
                        reader.Read();
                        var postTransactionScript = reader.GetString(0);
                        postTransactionScript = postTransactionScript.Replace(@"CREATE", "ALTER");
                        var newScriptpPosition = postTransactionScript.IndexOf("-- Select the return values", 0);
                        var newScript = @"if (@object_type = '30' and (@transaction_type = 'A' or @transaction_type = 'U')) 
                                          begin
	                                          update OJDT
	                                             set OJDT.U_UpdateTS = cast(replace(substring(cast(cast(getdate() as time) as varchar), 0, 9), ':', '') as int)
	                                           where OJDT.TransId = @list_of_cols_val_tab_del;
                                          end

                                          ";

                        if (!postTransactionScript.Contains(newScript))
                        {
                            postTransactionScript = postTransactionScript.Insert(postTransactionScript.IndexOf("-- Select the return values", 0), newScript);
                            var alterCommand = new SqlCommand(postTransactionScript, SqlConnection).ExecuteNonQuery();
                        }

                        SqlConnection.Close();
                    }
                }
                catch (Exception ex)
                {
                    txtLog.AppendText("\r\n" + String.Format(ex.Message));

                    if (Company.DbServerType == BoDataServerTypes.dst_HANADB)
                    {
                        HanaConnection.Close();
                    }
                    else
                    {
                        SqlConnection.Close();
                    }
                }
                #endregion

                txtLog.AppendText("\r\n" + "Setup finalizado." + "\r\n");
            }
            catch (Exception ex)
            {
                txtLog.AppendText("\r\n" + String.Format(ex.Message) + "\r\n");
                return;
            }
        }

        public static string GetEmbeddedTextContent(string arquivo)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(arquivo);
            var xml = "";
            if (stream != null)
            {
                var reader = new StreamReader(stream);
                xml = reader.ReadToEnd();
            }

            return xml;
        }

        private bool CheckView(string companyDB, string resourceName)
        {
            bool exists;

            if (Company.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                HanaConnection.Open();
                var command = new HanaCommand($@"select count(*) as ""count"" from SYS.VIEWS where SCHEMA_NAME = '{companyDB}' and VIEW_NAME like '{resourceName}'", HanaConnection);
                var reader = command.ExecuteReader();
                reader.Read();
                exists = reader.GetString(0) != "0";
                HanaConnection.Close();
            }
            else
            {
                SqlConnection.Open();
                var command = new SqlCommand($@"select count(*) from sys.views where name = '{resourceName}'", SqlConnection);
                var reader = command.ExecuteReader();
                reader.Read();
                exists = reader.GetInt32(0) != 0;
                SqlConnection.Close();
            }

            return exists;
        }

        private bool CheckProcedure(string companyDB, string resourceName)
        {
            bool exists;

            if (Company.DbServerType == BoDataServerTypes.dst_HANADB)
            {
                HanaConnection.Open();
                var command = new HanaCommand($@"select count(*) as ""count"" from SYS.PROCEDURES where SCHEMA_NAME = '{companyDB}' and PROCEDURE_NAME like '{resourceName.ToUpper()}'", HanaConnection);
                var reader = command.ExecuteReader();
                reader.Read();
                exists = reader.GetString(0) != "0";
                HanaConnection.Close();
            }
            else
            {
                SqlConnection.Open();
                var command = new SqlCommand($@"select count(*) from sys.procedures where name = '{resourceName}'", SqlConnection);
                var reader = command.ExecuteReader();
                reader.Read();
                exists = reader.GetInt32(0) != 0;
                SqlConnection.Close();
            }

            return exists;
        }
    }
}
