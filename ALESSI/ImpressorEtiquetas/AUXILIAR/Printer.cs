
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using MODEL;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace AUXILIAR
{
    public class Printer
    {
        #region Atributos
        XMLReader _reader { get; set; }
        #endregion

        #region Construtor
        public Printer()
        {
            this._reader = new XMLReader();
        }
        #endregion

        public void Print(Etiqueta etiqueta, string tipoEtiqueta)
        {
            var server = _reader.ReadServerName();
            var database = _reader.ReadDatabaseName();
            var user = _reader.ReadUserName();
            var password = _reader.ReadPassword();

            var arquivo = new ReportDocument();
            arquivo.Load(tipoEtiqueta);
            foreach (InternalConnectionInfo internalConnectionInfo in arquivo.DataSourceConnections)
                internalConnectionInfo.SetConnection(server, database, user, password);

            arquivo.PrintOptions.PrinterName = etiqueta.Impressora;
            
            arquivo.SetParameterValue("@order", etiqueta.Ordem);
            arquivo.PrintToPrinter(Convert.ToInt32(etiqueta.Quantidade), true, 1, 1);
            arquivo.Close();
            arquivo.Dispose();

            GC.Collect();
        }
    }
}
