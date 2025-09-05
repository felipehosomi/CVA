using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{

    public class BINView : INotifyPropertyChanged
    {
        private int? _cODE;
        public int? CODE
        {
            get { return _cODE; }
            set
            {
                if (_cODE == value) return;
                _cODE = value;
                OnPropertyChanged();
            }
        }

        private string _aPLATZ_ID;
        public string APLATZ_ID
        {
            get { return _aPLATZ_ID; }
            set
            {
                if (_aPLATZ_ID == value) return;
                _aPLATZ_ID = value;
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
