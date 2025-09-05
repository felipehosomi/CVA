using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects
{
    public class UserFields
    {

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Type;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        private int _Size;

        public int Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        

        private string _SubType;

        public string SubType
        {
            get { return _SubType; }
            set { _SubType = value; }
        }

        private string _DefaultValue;

        public string DefaultValue
        {
            get { return _DefaultValue; }
            set { _DefaultValue = value; }
        }

        private string _TableName;

        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }   

        private int _EditSize;

        public int EditSize
        {
            get { return _EditSize; }
            set { _EditSize = value; }
        }        
        

        private List<ValidValue> _ValidValuesMD;

        public List<ValidValue> ValidValuesMD
        {
            get { return _ValidValuesMD; }
            set { _ValidValuesMD = value; }
        }
    }

    public class ValidValue
    {
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
    }
}
