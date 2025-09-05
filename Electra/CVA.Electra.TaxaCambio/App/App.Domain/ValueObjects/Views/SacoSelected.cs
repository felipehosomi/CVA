using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{

    public class SacoSelected 
    {
        private string _U_ITEMCODE;
        public string U_ITEMCODE
        {
            get { return _U_ITEMCODE; }
            set
            {
                if (_U_ITEMCODE == value) return;
                _U_ITEMCODE = value;
            }
        }

        private int _U_POSICAO;
        public int U_POSICAO
        {
            get { return _U_POSICAO; }
            set
            {
                if (_U_POSICAO == value) return;
                _U_POSICAO = value;
    
            }
        }
        

     
    }
}
