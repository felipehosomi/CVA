using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.CriadorDeCampos
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                var uri = $@"{AppDomain.CurrentDomain.BaseDirectory}\Connections.xml";
                Console.WriteLine("********** Iniciando processo **********");
                Console.WriteLine("*** Arquivo de conexão: " + uri);

                foreach (var item in XmlHelper.StreamConnections(uri))
                {
                    Console.WriteLine("**** Tentando se conectar em " + item.CompanyDb);
                    var Company = Connect.ConnectToCompany(item);
                    Console.WriteLine("**** Conectado em: " + Company.CompanyName);
                    UserFieldsData.Create(Company);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("**** Erro ****");
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("********** Processo finalizado **********");
            Console.ReadKey();
        }
    }
}
