using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace CVA.Apetit.Addon.Class
{
    class FormService
    {
        public static void InserirLinhaMtx(SAPbouiCOM.IForm oForm, string Matrix, string sDataSource)
        {
            //SAPbouiCOM.Form oForm = (SAPbouiCOM.Form)B1Connections.theAppl.Forms.ActiveForm;
            DBDataSource oDbDataSource = oForm.DataSources.DBDataSources.Item(sDataSource);
            Matrix oMxFil = (Matrix)oForm.Items.Item(Matrix).Specific;

            //Class.Conexao.sbo_application.SetStatusBarMessage("Processo em andamento. Aguarde..", BoMessageTime.bmt_Long, false);

            try
            {
                oForm.Freeze(true);
                oMxFil.FlushToDataSource();
                oDbDataSource.InsertRecord(oDbDataSource.Size);
                //oForm.DataSources.DBDataSources.Item(sDataSource).SetValue("Code", oDbDataSource.Size - 1, oDbDataSource.GetValue("Code", 0));
                oMxFil.LoadFromDataSource();

                if (oForm.Mode == BoFormMode.fm_OK_MODE)
                    oForm.Mode = BoFormMode.fm_UPDATE_MODE;

                //if (oForm.TypeEx == "frmRegras")
                //{
                    //Util.PreencherComboMatrix(ref oForm, Matrix, 1, oDbDataSource.Size, "OITM", String.Empty);
                    //Util.PreencherComboMatrix(ref oForm, Matrix, 2, oDbDataSource.Size, "OPLN", String.Empty);
                //}
                oForm.Freeze(false);
            }
            catch (Exception ex)
            {
                oForm.Freeze(false);
                Class.Conexao.sbo_application.SetStatusBarMessage(String.Format(@"Ocorreu um erro no processo. Detalhes: {0}", ex.Message), BoMessageTime.bmt_Short, true);
                throw;
            }
        }


        public static void AdicionarLinha(SAPbouiCOM.IForm oForm, string matrixItemUID, string DBDataSources)
        {
            try
            {
                    var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(matrixItemUID).Specific;
                    oMatrix.FlushToDataSource();
                    oForm.DataSources.DBDataSources.Item(DBDataSources).InsertRecord(oForm.DataSources.DBDataSources.Item(DBDataSources).Size);
                    oMatrix.LoadFromDataSource();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void RemoverLinha(SAPbouiCOM.IForm oForm, string matrixItemUID, string DBDataSources)
        {
            try
            {
                var oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(matrixItemUID).Specific;
                oMatrix.FlushToDataSource();
                oForm.DataSources.DBDataSources.Item(DBDataSources).RemoveRecord(1);
                oMatrix.LoadFromDataSource();
            }
            catch (Exception)
            {

                throw;
            }
        }        

        public static bool FormAberto(string FormType)
        {
            try
            {
                SAPbouiCOM.Form oForm = Conexao.sbo_application.Forms.GetForm(FormType, 0);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        internal static void LimparDataSource(SAPbouiCOM.IForm form)
        {
            try
            {
                for (int i = 0; i < form.DataSources.UserDataSources.Count; i++)
                {
                    form.DataSources.UserDataSources.Item(i).Value = null;
                }
                for (int i = 0; i < form.DataSources.DataTables.Count; i++)
                {
                    form.DataSources.DataTables.Item(i).Rows.Clear();
                    form.DataSources.DataTables.Item(i).Rows.Add();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void PreencherComboMatrix(ref SAPbouiCOM.Form oForm, string sMatrix, int iColuna, int iLinha, string sTabela, string sFilial)
        {
            try
            {
                string sQuery = String.Empty;

                switch (sTabela)
                {
                    case "OBPL": { sQuery = "SELECT \"BPLId\", \"BPLName\" FROM \"OBPL\""; break; }
                    case "OITM": { sQuery = String.Format(@"SELECT ""ItemCode"", ""ItemName"" FROM ""OITM"" WHERE ""ItmsGrpCod"" = 104 "); break; }
                    case "OPLN": { sQuery = "SELECT \"ListNum\", \"ListName\" FROM \"OPLN\""; break; }
                    case "OSLP": { sQuery = "SELECT \"SlpCode\", \"SlpName\" FROM \"OSLP\" WHERE \"SlpCode\" > 0"; break; }
                    case "OCTG": { sQuery = "SELECT \"GroupNum\", \"PymntGroup\" FROM \"OCTG\""; break; }
                }

                Matrix oMatrix = (Matrix)oForm.Items.Item(sMatrix).Specific;
                System.Data.DataTable odt = Conexao.ExecuteSqlDataTable(sQuery);
                SAPbouiCOM.ComboBox oCombo = (SAPbouiCOM.ComboBox)oMatrix.Columns.Item(iColuna).Cells.Item(iLinha).Specific;

                while (oCombo.ValidValues.Count > 0)
                    oCombo.ValidValues.Remove(0, BoSearchKey.psk_Index);

                for (int i = 0; i < odt.Rows.Count; i++)
                {
                    oCombo.ValidValues.Add(odt.Rows[i][0].ToString(), odt.Rows[i][1].ToString());
                }

                //if (sTabela.Equals("OBPL") || sTabela.Equals("OITM") || sTabela.Equals("OPLN") || sTabela.Equals("OCTG"))
                //    oCombo.ValidValues.Add("0", "Todas");
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
