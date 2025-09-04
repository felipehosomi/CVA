using ClosedXML.Excel;
using CVA.TomTicketDownload.BLL;
using CVA.TomTicketDownload.Model;
using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CVA.TomTicketDownload
{
    class Program
    {
        private static readonly string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        private static readonly string token = ConfigurationManager.AppSettings["Token"];
        private static readonly string caminhoDestino = ConfigurationManager.AppSettings["CaminhoDestino"];
        private static readonly string chamadoUnitario = ConfigurationManager.AppSettings["ChamadoUnitario"];
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [STAThread]
        static void Main(string[] args)
        {
            UserFieldsBLL _TomTicketDAO = new UserFieldsBLL();
            ServiceBLL bll = new ServiceBLL();
            List<DateTime> lst = (List<DateTime>)holidayListBrazil.GetHolidaysByCurrentYear();

            Logger.Info("Inicio da Execução");
            if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && !lst.Exists(x => x.Date == DateTime.Now.Date))
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["createTableFields"]))
                {
                    Console.WriteLine("Criando tabela");
                    _TomTicketDAO.CreateTable();
                    Console.WriteLine("Tabela criada. " + DateTime.Now);
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["sincronizarTodos"]))
                {
                    Console.WriteLine("Sincronizando todos chamados do TomTicket");
                    bll.SincronizaChamadosAsync().Wait();
                    Console.WriteLine("Sincronização finalizada às " + DateTime.Now);
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["sincronizarUltimos"]))
                {
                    Console.WriteLine("Sincronizando ultimos chamados do TomTicket");
                    bll.SincronizaUltimosChamados();
                    Console.WriteLine("Sincronização finalizada às " + DateTime.Now);
                }

                if (!String.IsNullOrEmpty(chamadoUnitario))
                {
                    Console.WriteLine("Sincronizando chamados unitários do TomTicket");
                    bll.SincronizaChamadoUnitario(chamadoUnitario);
                    Console.WriteLine("Sincronização finalizada às " + DateTime.Now);
                }

                if(Convert.ToBoolean(ConfigurationManager.AppSettings["recalcularHoras"]))
                {
                    Console.WriteLine("Realizando calculo de horas em todos chamados");
                    bll.recalcularHoras();
                    Console.WriteLine("Calculo finalizado às " + DateTime.Now);
                }
            }
            Logger.Info("Fim da Execução");

        }

        private static string SalvarExcel(List<Chamado> chamados)
        {
            Directory.CreateDirectory(caminhoDestino);
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Chamados");

            // Cabeçalho
            var headerNames = typeof(Chamado).GetProperties().Select(prop => prop.Name).ToList();

            for (int i = 0; i < headerNames.Count; i++)
            {
                ws.Cell(1, i + 1).Value = headerNames[i];
            }

            // Conteúdo
            ws.Cell(2, 1).InsertData(chamados);

            string file = Path.Combine(caminhoDestino, "chamados_tomticket.xlsx");
            wb.SaveAs(file);
            return file;
        }
    }
}