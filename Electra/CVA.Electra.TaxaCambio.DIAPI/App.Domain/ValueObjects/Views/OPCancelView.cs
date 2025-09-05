using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{
    public class OPCancelView : INotifyPropertyChanged
    {

        private int? _u_OPCODE;
        public int? U_OPCODE
        {
            get { return _u_OPCODE; }
            set
            {
                if (_u_OPCODE == value) return;
                _u_OPCODE = value;
                OnPropertyChanged();
            }
        }

        private int? _u_POSICAO;
        public int? U_POSICAO
        {
            get { return _u_POSICAO; }
            set
            {
                if (_u_POSICAO == value) return;
                _u_POSICAO = value;
                OnPropertyChanged();
            }
        }
        

        private string _u_STATUSOP;
        public string U_STATUSOP
        {
            get { return _u_STATUSOP; }
            set
            {
                if (_u_STATUSOP == value) return;
                _u_STATUSOP = value;
                OnPropertyChanged();
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
