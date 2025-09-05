using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{
    public class CodigoOperacaoView : INotifyPropertyChanged
    {

        private int? _bELNR_ID;
        public int? BELNR_ID
        {
            get { return _bELNR_ID; }
            set
            {
                if (_bELNR_ID == value) return;
                _bELNR_ID = value;
                OnPropertyChanged();
            }
        }

        private int? _bELPOS_ID;
        public int? BELPOS_ID
        {
            get { return _bELPOS_ID; }
            set
            {
                if (_bELPOS_ID == value) return;
                _bELPOS_ID = value;
                OnPropertyChanged();
            }
        }
        
        private string _aG_ID;
        public string AG_ID
        {
            get { return _aG_ID; }
            set
            {
                if (_aG_ID == value) return;
                _aG_ID = value;
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

        private string _bEZ;
        public string BEZ
        {
            get { return _bEZ; }
            set
            {
                if (_bEZ == value) return;
                _bEZ = value;
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
