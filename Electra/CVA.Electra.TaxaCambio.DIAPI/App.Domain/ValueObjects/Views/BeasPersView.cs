using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{

    public class BeasPersView : INotifyPropertyChanged
    {
        private string _pERS_ID;
        public string PERS_ID
        {
            get { return _pERS_ID; }
            set
            {
                if (_pERS_ID == value) return;
                _pERS_ID = value;
                OnPropertyChanged();
            }
        }

        private string _nAME1;
        public string NAME1
        {
            get { return _nAME1; }
            set
            {
                if (_nAME1 == value) return;
                _nAME1 = value;
                OnPropertyChanged();
            }
        }

        private string _nAME2;
        public string NAME2
        {
            get { return _nAME2; }
            set
            {
                if (_nAME2 == value) return;
                _nAME2 = value;
                OnPropertyChanged();
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName == value) return;
                _displayName = value;
                OnPropertyChanged();
            }
        }

        private int? _empID;
        public int? EmpID
        {
            get { return _empID; }
            set
            {
                if (_empID == value) return;
                _empID = value;
                OnPropertyChanged();
            }
        }

        private string _u_CodeBar;
        public string U_CodeBar
        {
            get { return _u_CodeBar; }
            set
            {
                if (_u_CodeBar == value) return;
                _u_CodeBar = value;
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
