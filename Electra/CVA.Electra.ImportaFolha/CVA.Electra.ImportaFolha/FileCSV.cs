using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CVA.Electra.ImportaFolha
{
    public class FileCSV
    {
        public IEnumerable<LinhaCSV> query;

        public Boolean lerArquivo(string file)
        {

            try
            {
                string[] allLines = File.ReadAllLines(file);

                if (allLines.Length > 1)
                {
                    query = from line in allLines
                            let data = line.Split(';')
                            select new LinhaCSV(data);

                    return true;
                }
                else
                {
                    query = null;
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
