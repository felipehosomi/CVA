using BillingAssistant.Controllers;
using SAPbobsCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BillingAssistant.Infrastructure
{
    public class QueryManager : IDisposable, IEnumerable, IEnumerator
    {
        private int index = -1;
        private bool _moveNext = false;
        private Recordset _recordset;
        public ExtendedInformation Advanced { get; private set; }

        private QueryManager(Recordset recordset)
        {
            _recordset = recordset;
            Advanced = new ExtendedInformation(_recordset);
        }

        private QueryManager(string query)
        {
            var connection = CommonController.Company;

            _recordset = (Recordset)connection.GetBusinessObject(BoObjectTypes.BoRecordset);
            _recordset.DoQuery(query);
            Advanced = new ExtendedInformation(_recordset);

        }

        public Recordset GetRecordSet()
        {
            return _recordset;
        }

        ~QueryManager()
        {
            Dispose();
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public class ExtendedInformation : IDisposable
        {
            private Recordset _recordset;

            public ExtendedInformation(Recordset recordset)
            {
                _recordset = recordset;
            }

            public int ColumnCount
            {
                get { return _recordset.Fields.Count; }
            }

            public string ColumnName(int index)
            {
                return _recordset.Fields.Item(index).Name;
            }

            public void Dispose()
            {
                if (_recordset != null)
                    Marshal.ReleaseComObject(_recordset);

                _recordset = null;

                GC.Collect();

                GC.WaitForPendingFinalizers();
            }

            ~ExtendedInformation()
            {
                Dispose();
            }
        }

        public static QueryManager DoQuery(string query)
        {
            return new QueryManager(query);
        }

        public T Get<T>(int index)
        {
            return (T)_recordset.Fields.Item(index).Value;
        }

        public T Get<T>(string fieldName)
        {
            return (T)_recordset.Fields.Item(fieldName).Value;
        }

        public bool HasRow
        {
            get { return _recordset.RecordCount > 0; }
        }
        public int RecordCount
        {
            get { return _recordset.RecordCount; }
        }


        public void Dispose()
        {
            Advanced = null;
            if (_recordset != null)
                Marshal.ReleaseComObject(_recordset);

            _recordset = null;

            GC.Collect();

            GC.WaitForPendingFinalizers();
        }

        public bool MoveNext()
        {
            index++;
            if (_recordset.RecordCount <= index)
                return false;

            if (!_moveNext)
            {
                _moveNext = true;
                return true;
            }

            _moveNext = true;

            _recordset.MoveNext();
            return true;
        }

        public void Reset()
        {
            _recordset.MoveFirst();
        }

        public object Current { get { return this; } }
    }
}
