using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{
    public class OPLoteView : INotifyPropertyChanged
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

        private string _whsCode;
        public string WhsCode
        {
            get { return _whsCode; }
            set
            {
                if (_whsCode == value) return;
                _whsCode = value;
                OnPropertyChanged();
            }
        }

        private string _loteId;
        public string LoteId
        {
            get { return _loteId; }
            set
            {
                if (_loteId == value) return;
                _loteId = value;
                OnPropertyChanged();
            }
        }
        
        private int? _sysNumber;
        public int? SysNumber
        {
            get { return _sysNumber; }
            set
            {
                if (_sysNumber == value) return;
                _sysNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _inDate;
        public DateTime? InDate
        {
            get { return _inDate; }
            set
            {
                if (_inDate == value) return;
                _inDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _expDate;
        public DateTime? ExpDate
        {
            get { return _expDate; }
            set
            {
                if (_expDate == value) return;
                _expDate = value;
                OnPropertyChanged();
            }
        }

        private int? _key;
        public int? Key
        {
            get { return _key; }
            set
            {
                if (_key == value) return;
                _key = value;
                OnPropertyChanged();
            }
        }
        
        private double? _quantity;
        public double? Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity == value) return;
                _quantity = value;
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
