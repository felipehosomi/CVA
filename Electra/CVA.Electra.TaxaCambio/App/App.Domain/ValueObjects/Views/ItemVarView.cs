using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{

    public class ItemVarView : INotifyPropertyChanged
    {
        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                if (_itemCode == value) return;
                _itemCode = value;
                OnPropertyChanged();
            }
        }

        private decimal? _u_SD_PercDif;
        public decimal? U_SD_PercDif
        {
            get { return _u_SD_PercDif; }
            set
            {
                if (_u_SD_PercDif == value) return;
                _u_SD_PercDif = value;
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
