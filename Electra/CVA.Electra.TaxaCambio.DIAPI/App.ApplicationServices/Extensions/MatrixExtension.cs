
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.ApplicationServices
{
    public static class MatrixExtension
    {
        public static void SetColumns_Editable_False(this Columns vItem, params string[] Columns)
        {
            try
            {
                string vColumnName = "";

                for (int i = 0; i < Columns.Length; i++)
                {
                    vColumnName = Columns[i];

                    vItem.Item(vColumnName).Editable = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetColumns_Editable_True(this Columns vItem, params string[] Columns)
        {
            try
            {
                string vColumnName = "";

                for (int i = 0; i < Columns.Length; i++)
                {
                    vColumnName = Columns[i];

                    vItem.Item(vColumnName).Editable = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetColumns_Visible(this Columns vItem, bool Visible, params string[] Columns)
        {
            try
            {
                string vColumnName = "";

                for (int i = 0; i < Columns.Length; i++)
                {
                    vColumnName = Columns[i];

                    vItem.Item(vColumnName).Visible = Visible;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
