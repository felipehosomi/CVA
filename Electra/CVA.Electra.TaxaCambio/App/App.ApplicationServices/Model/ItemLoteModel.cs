using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.Model
{
    public class ItemLoteModel : INotifyPropertyChanged
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double? Quantity { get; set; }
        public string Lote { get; set; }
        public DateTime? LoteValidade { get; set; }
        public int Etiquetas { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
