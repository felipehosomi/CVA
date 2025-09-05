using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects.Views
{
    public class MatTypeView : INotifyPropertyChanged
    {
        private int? _absEntry;
        public int? AbsEntry
        {
            get { return _absEntry; }
            set
            {
                if (_absEntry == value) return;
                _absEntry = value;
                OnPropertyChanged();
            }
        }
        
        private string _matType;
        public string MatType
        {
            get { return _matType; }
            set
            {
                if (_matType == value) return;
                _matType = value;
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
