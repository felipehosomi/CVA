using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{

    public class SearchSalesOrderItensView : INotifyPropertyChanged
    {
        private bool _isSelected;
        private int _docEntryOrder;
        private int _docNumOrder;
        private string _cardCode;
        private string _cardName;
        private DateTime _docDateOrder;
        private int _docEntryStockTransfers;
        private DateTime _docDateStockTransfers;
        private int _lineNum;
        private string _itemCode;
        private string _itemName;
        private double _openQty;
        private double _packQty;
        private double _SelectedQty;
        private double _quantity;

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

        public int DocNumOrder
        {
            get { return _docNumOrder; }
            set
            {
                if (_docNumOrder == value) return;
                _docNumOrder = value;
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

        public int DocEntryStockTransfers
        {
            get { return _docEntryStockTransfers; }
            set
            {
                if (_docEntryStockTransfers == value) return;
                _docEntryStockTransfers = value;
                OnPropertyChanged();
            }
        }

        public DateTime DocDateStockTransfers
        {
            get { return _docDateStockTransfers; }
            set
            {
                if (_docDateStockTransfers == value) return;
                _docDateStockTransfers = value;
                OnPropertyChanged();
            }
        }


        public int LineNum
        {
            get { return _lineNum; }
            set
            {
                if (_lineNum == value) return;
                _lineNum = value;
                OnPropertyChanged();
            }
        }

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

        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (_itemName == value) return;
                _itemName = value;
                OnPropertyChanged();
            }
        }

        public double Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged();
            }
        }

        public double OpenQty
        {
            get { return _openQty; }
            set
            {
                if (_openQty == value) return;
                _openQty = value;
                OnPropertyChanged();
            }
        }

        public double PackQty
        {
            get { return _packQty; }
            set
            {
                if (_packQty == value) return;
                _packQty = value;
                OnPropertyChanged();
            }
        }

        public double SelectedQty
        {
            get { return _SelectedQty; }
            set
            {
                if (_SelectedQty == value) return;
                _SelectedQty = value;
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
