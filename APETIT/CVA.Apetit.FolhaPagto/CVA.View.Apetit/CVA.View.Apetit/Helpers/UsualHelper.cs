using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.Helpers
{
    public static class UsualHelper
    {        
        public static string SubStrPositioned(this string txt, ref int ini, int length)
        {
            var str = txt.Substring(ini, length);
            ini = length + ini;
            return str;
        }
    }
}
