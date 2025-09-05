using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{
    public class OPView : INotifyPropertyChanged
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

        private int? _sortId;
        public int? SortId
        {
            get { return _sortId; }
            set
            {
                if (_sortId == value) return;
                _sortId = value;
                OnPropertyChanged();
            }
        }

        private string _aRT1_ID;
        public string ART1_ID
        {
            get { return _aRT1_ID; }
            set
            {
                if (_aRT1_ID == value) return;
                _aRT1_ID = value;
                OnPropertyChanged();
            }
        }

        
        private string _itemCodeFather;
        public string ItemCodeFather
        {
            get { return _itemCodeFather; }
            set
            {
                if (_itemCodeFather == value) return;
                _itemCodeFather = value;
                OnPropertyChanged();
            }
        }

        private int? _itemFatherAbsEntry;
        public int? ItemFatherAbsEntry
        {
            get { return _itemFatherAbsEntry; }
            set
            {
                if (_itemFatherAbsEntry == value) return;
                _itemFatherAbsEntry = value;
                OnPropertyChanged();
            }
        }

        private string _itemFatherMatType;
        public string ItemFatherMatType
        {
            get { return _itemFatherMatType; }
            set
            {
                if (_itemFatherMatType == value) return;
                _itemFatherMatType = value;
                OnPropertyChanged();
            }
        }

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

        private string _itemName;
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

        private int? _itemAbsEntry;
        public int? ItemAbsEntry
        {
            get { return _itemAbsEntry; }
            set
            {
                if (_itemAbsEntry == value) return;
                _itemAbsEntry = value;
                OnPropertyChanged();
            }
        }

        private string _itemMatType;
        public string ItemMatType
        {
            get { return _itemMatType; }
            set
            {
                if (_itemMatType == value) return;
                _itemMatType = value;
                OnPropertyChanged();
            }
        }

        private double? _iNPUT_QTY;
        public double? INPUT_QTY
        {
            get { return _iNPUT_QTY; }
            set
            {
                if (_iNPUT_QTY == value) return;
                _iNPUT_QTY = value;
                OnPropertyChanged();
            }
        }

        private string _iNPUT_UNIT;
        public string INPUT_UNIT
        {
            get { return _iNPUT_UNIT; }
            set
            {
                if (_iNPUT_UNIT == value) return;
                _iNPUT_UNIT = value;
                OnPropertyChanged();
            }
        }

        private double? _percDif;
        public double? PercDif
        {
            get { return _percDif; }
            set
            {
                if (_percDif == value) return;
                _percDif = value;
                OnPropertyChanged();
            }
        }
        

        private string _barCode;
        public string BarCode
        {
            get { return _barCode; }
            set
            {
                if (_barCode == value) return;
                _barCode = value;
                OnPropertyChanged();
            }
        }

        private string _U_SD_PESAGEM;
        public string U_SD_PESAGEM
        {
            get { return _U_SD_PESAGEM; }
            set
            {
                if (_U_SD_PESAGEM == value) return;
                _U_SD_PESAGEM = value;
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

    public class ListPesagemView : INotifyPropertyChanged
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

        private double? _peso;
        public double? Peso
        {
            get { return _peso; }
            set
            {
                if (_peso == value) return;
                _peso = value;
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
