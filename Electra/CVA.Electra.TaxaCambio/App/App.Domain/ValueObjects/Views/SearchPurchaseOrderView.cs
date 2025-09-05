using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{

    public class SearchPurchaseOrderView : INotifyPropertyChanged
    {
        private bool _isSelected;
        private int _docEntryOrder;
        private int _docNum;
        private string _cardCode;
        private string _cardName;
        private DateTime _docDateOrder;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public int DocEntryOrder
        {
            get { return _docEntryOrder; }
            set
            {
                if (_docEntryOrder == value) return;
                _docEntryOrder = value;
                OnPropertyChanged();
            }
        }

        public int DocNum
        {
            get { return _docNum; }
            set
            {
                if (_docNum == value) return;
                _docNum = value;
                OnPropertyChanged();
            }
        }

        public string CardCode
        {
            get { return _cardCode; }
            set
            {
                if (_cardCode == value) return;
                _cardCode = value;
                OnPropertyChanged();
            }
        }

        public string CardName
        {
            get { return _cardName; }
            set
            {
                if (_cardName == value) return;
                _cardName = value;
                OnPropertyChanged();
            }
        }

        public DateTime DocDateOrder
        {
            get { return _docDateOrder; }
            set
            {
                if (_docDateOrder == value) return;
                _docDateOrder = value;
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
