using CVA.AddOn.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileWriterAttribute : Attribute
    {
        private int position;
        public int Position
        {
            get
            {
                return position - 1;
            }
            set
            {
                position = value;
            }
        }

        public int Size { get; set; }

        public int DecimalPlaces { get; set; } = 2;

        public string DecimalSeparator { get; set; }

        public string Format { get; set; }

        public PaddingTypeEnum PaddingType { get; set; } = PaddingTypeEnum.NotSet;

        public string PaddingChar { get; set; }

        public bool OnylNumeric { get; set; }
    }
}
