using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.BancadaTeste.BLL
{
    public class POFileBLL
    {
        public static string GetOP(string path)
        {
            StreamReader sr = new StreamReader(path);
            string op = sr.ReadLine();
            sr.Close();
            return op;
        }
    }
}
