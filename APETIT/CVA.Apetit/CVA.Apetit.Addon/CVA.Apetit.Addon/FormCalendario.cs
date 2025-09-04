using SAPbobsCOM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Apetit.Addon
{
    public partial class FormCalendario : Form
    {
        Color corDtEntrega = Color.Red;
        Color corFeriado = Color.Green;
        Color corNormal = Color.White;
        Color corAmbos = Color.Orange;
        int fontSize = 6;
        int rowHeight = 15;
        int columnHeadersHeight = 18;
        int larguraCelula = 20;
        int xPos = 40;
        int yPos = 140;
        int espacoHor = 10;
        int espacoVer = 30;
        int[,] feriados;
        bool novo;
        DataGridView grid01 = new DataGridView();
        DataGridView grid02 = new DataGridView();
        DataGridView grid03 = new DataGridView();
        DataGridView grid04 = new DataGridView();
        DataGridView grid05 = new DataGridView();
        DataGridView grid06 = new DataGridView();
        DataGridView grid07 = new DataGridView();
        DataGridView grid08 = new DataGridView();
        DataGridView grid09 = new DataGridView();
        DataGridView grid10 = new DataGridView();
        DataGridView grid11 = new DataGridView();
        DataGridView grid12 = new DataGridView();
        TextBox txt01 = new TextBox();
        TextBox txt02 = new TextBox();
        TextBox txt03 = new TextBox();
        TextBox txt04 = new TextBox();
        TextBox txt05 = new TextBox();
        TextBox txt06 = new TextBox();
        TextBox txt07 = new TextBox();
        TextBox txt08 = new TextBox();
        TextBox txt09 = new TextBox();
        TextBox txt10 = new TextBox();
        TextBox txt11 = new TextBox();
        TextBox txt12 = new TextBox();
        DataGridViewCellStyle styleDtEntrega = new DataGridViewCellStyle();
        DataGridViewCellStyle styleNormal = new DataGridViewCellStyle();
        DataGridViewCellStyle styleFeriado = new DataGridViewCellStyle();
        DataGridViewCellStyle styleAmbos = new DataGridViewCellStyle();
        int ano, periodicidade, bplId, categoria;

        public FormCalendario()
        {
            InitializeComponent();
        }

        //==================================================================================================================================//
        private void Form2_Load(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            List<KeyValuePair<int, string>> comboSource;
            int chave;
            string descricao;

            this.Activate();
            this.Text = "Calendário de Recebimento de Mercadorias por Filial";
            try { this.Icon = new System.Drawing.Icon("logo.ico"); } catch { }

            styleDtEntrega.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);   
            styleDtEntrega.BackColor = corDtEntrega;

            styleNormal.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular, GraphicsUnit.Point);
            styleNormal.BackColor = corNormal;

            styleFeriado.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
            styleFeriado.BackColor = corFeriado;

            styleAmbos.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Bold, GraphicsUnit.Point);
            styleAmbos.BackColor = corAmbos;

            var sql = @"SELECT ""BPLId"", ""BPLName"" FROM OBPL ORDER BY ""BPLId"" ";
            var oDT = Class.Conexao.ExecuteSqlDataTable(sql);
            cbFilial.Items.Clear();
            comboSource = new List<KeyValuePair<int, string>>();
            comboSource.Add(new KeyValuePair<int, string>(0, ""));
            foreach (DataRow linha in oDT.Rows)
            {
                chave = Convert.ToInt32(linha["BPLId"].ToString());
                descricao = linha["BPLId"].ToString() + " - " + linha["BPLName"].ToString();
                comboSource.Add(new KeyValuePair<int, string>(chave, descricao));
            }
            cbFilial.DataSource = new BindingSource(comboSource, null);
            cbFilial.DisplayMember = "Value";
            cbFilial.ValueMember = "Key";

            sql = string.Format(@"
SELECT T1.""FldValue"" AS ""Code"", T1.""Descr"" AS ""Name""
FROM CUFD T0
    INNER JOIN UFD1 T1 ON T1.""TableID"" = T0.""TableID"" AND T1.""FieldID"" = T0.""FieldID""
WHERE T0.""TableID"" = 'OITM'
    AND T0.""AliasID"" = 'CVA_Categoria'
ORDER BY T1.""FldValue""
");
            oDT = Class.Conexao.ExecuteSqlDataTable(sql);
            cbCategoria.Items.Clear();
            comboSource = new List<KeyValuePair<int, string>>();
            comboSource.Add(new KeyValuePair<int, string>(0, ""));
            foreach (DataRow linha in oDT.Rows)
                comboSource.Add(new KeyValuePair<int, string>(Convert.ToInt32(linha["Code"].ToString()), linha["Name"].ToString()));
            cbCategoria.DataSource = new BindingSource(comboSource, null);
            cbCategoria.DisplayMember = "Value";
            cbCategoria.ValueMember = "Key";

            cbPeriodicidade.Items.Clear();
            comboSource = new List<KeyValuePair<int, string>>();
            comboSource.Add(new KeyValuePair<int, string>(0, ""));
            comboSource.Add(new KeyValuePair<int, string>(1, "Manual"));
            comboSource.Add(new KeyValuePair<int, string>(2, "Toda segunda-feira"));
            comboSource.Add(new KeyValuePair<int, string>(3, "Toda terça-feira"));
            comboSource.Add(new KeyValuePair<int, string>(4, "Toda quarta-feira"));
            comboSource.Add(new KeyValuePair<int, string>(5, "Toda quinta-feira"));
            comboSource.Add(new KeyValuePair<int, string>(6, "Toda sexta-feira"));
            comboSource.Add(new KeyValuePair<int, string>(7, "Todo sábado"));
            comboSource.Add(new KeyValuePair<int, string>(8, "Todo domingo"));
            cbPeriodicidade.DataSource = new BindingSource(comboSource, null);
            cbPeriodicidade.DisplayMember = "Value";
            cbPeriodicidade.ValueMember = "Key";

            groupBox1.Location = new Point(xPos, yPos - 30);
            groupBox1.Width = (larguraCelula * 7 + 5) * 4 + espacoHor + 30;
            groupBox1.Height = (rowHeight * 6 + columnHeadersHeight + 5 + espacoVer) * 3 + 10;

            this.Width = groupBox1.Location.X + groupBox1.Width + xPos + 15;
            //this.Height = yPos + groupBox1.Location.Y + groupBox1.Height - 25;
            this.Height = yPos + groupBox1.Location.Y + groupBox1.Height - 50;

            grid01.CellDoubleClick += grid_CellDoubleClick;
            grid02.CellDoubleClick += grid_CellDoubleClick;
            grid03.CellDoubleClick += grid_CellDoubleClick;
            grid04.CellDoubleClick += grid_CellDoubleClick;
            grid05.CellDoubleClick += grid_CellDoubleClick;
            grid06.CellDoubleClick += grid_CellDoubleClick;
            grid07.CellDoubleClick += grid_CellDoubleClick;
            grid08.CellDoubleClick += grid_CellDoubleClick;
            grid09.CellDoubleClick += grid_CellDoubleClick;
            grid10.CellDoubleClick += grid_CellDoubleClick;
            grid11.CellDoubleClick += grid_CellDoubleClick;
            grid12.CellDoubleClick += grid_CellDoubleClick;

            btnOk.Text = "OK";
        }

        //==================================================================================================================================//
        private void MontarTela()
        //==================================================================================================================================//
        {
            try
            {
                ConstruirMatrizFeriados();

                grid01.Name = "01";
                this.Controls.Add(grid01);
                this.Controls.Add(txt01);
                MontarTela2(grid01, txt01);

                grid02.Name = "02";
                this.Controls.Add(grid02);
                this.Controls.Add(txt02);
                MontarTela2(grid02, txt02);

                grid03.Name = "03";
                this.Controls.Add(grid03);
                this.Controls.Add(txt03);
                MontarTela2(grid03, txt03);

                grid04.Name = "04";
                this.Controls.Add(grid04);
                this.Controls.Add(txt04);
                MontarTela2(grid04, txt04);

                grid05.Name = "05";
                this.Controls.Add(grid05);
                this.Controls.Add(txt05);
                MontarTela2(grid05, txt05);

                grid06.Name = "06";
                this.Controls.Add(grid06);
                this.Controls.Add(txt06);
                MontarTela2(grid06, txt06);

                grid07.Name = "07";
                this.Controls.Add(grid07);
                this.Controls.Add(txt07);
                MontarTela2(grid07, txt07);

                grid08.Name = "08";
                this.Controls.Add(grid08);
                this.Controls.Add(txt08);
                MontarTela2(grid08, txt08);

                grid09.Name = "09";
                this.Controls.Add(grid09);
                this.Controls.Add(txt09);
                MontarTela2(grid09, txt09);

                grid10.Name = "10";
                this.Controls.Add(grid10);
                this.Controls.Add(txt10);
                MontarTela2(grid10, txt10);

                grid11.Name = "11";
                this.Controls.Add(grid11);
                this.Controls.Add(txt11);
                MontarTela2(grid11, txt11);

                grid12.Name = "12";
                this.Controls.Add(grid12);
                this.Controls.Add(txt12);
                MontarTela2(grid12, txt12);

                PintarDiasEntrega();
                PintarFeriados();

                if (periodicidade > 0)
                    btnOk.Text = "Adicionar";

                groupBox1.SendToBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        //==================================================================================================================================//
        private void MontarTela2(DataGridView grid, TextBox txt)
        //==================================================================================================================================//
        {
            string s;
            int numGrid, numDia = -99, alturaGrid, larguraGrid;
            DateTime dataAux;

            try
            {
                s = grid.Name;
                Int32.TryParse(s, out numGrid);
                alturaGrid = rowHeight * 6 + columnHeadersHeight + 5;
                larguraGrid = larguraCelula * 7 + 5;

                txt.TextAlign = HorizontalAlignment.Center;
                txt.Size = new Size(90, 20);
                txt.BackColor = Color.Silver;
                Font negrito = new Font(txt.Font, FontStyle.Bold);
                txt.Font = negrito;
                txt.Text = RetornarTituloGrid(numGrid);

                grid.AllowUserToResizeColumns = false;
                grid.AllowUserToResizeRows = false;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                grid.Columns.Clear();
                grid.Size = new Size(larguraGrid, alturaGrid);

                if ((numGrid >= 1) && (numGrid <= 4))
                {
                    grid.Location = new Point(((larguraGrid + espacoHor) * (numGrid - 1) + xPos + 5), yPos);
                }
                if ((numGrid >= 5) && (numGrid <= 8))
                {
                    grid.Location = new Point(((larguraGrid + espacoHor) * (numGrid - 5) + xPos + 5), alturaGrid + espacoVer + yPos);
                }
                if ((numGrid >= 9) && (numGrid <= 12))
                {
                    grid.Location = new Point(((larguraGrid + espacoHor) * (numGrid - 9) + xPos + 5), (alturaGrid + espacoVer) * 2 + yPos);
                }
                txt.Location = new Point(grid.Location.X + ((larguraGrid-90) / 2), grid.Location.Y - 20);

                // var col =                         Header,  Name, width      ,readOnly,visible,alinhamento,format,fonte.size,Sortable
                var col01 = Class.Geral.retColTextBox("DOM", "c01", larguraCelula, true, true, "direita", "#0", fontSize, false);
                var col02 = Class.Geral.retColTextBox("SEG", "c02", larguraCelula, true, true, "direita", "#0", fontSize, false);
                var col03 = Class.Geral.retColTextBox("TER", "c03", larguraCelula, true, true, "direita", "#0", fontSize, false);
                var col04 = Class.Geral.retColTextBox("QUA", "c04", larguraCelula, true, true, "direita", "#0", fontSize, false);
                var col05 = Class.Geral.retColTextBox("QUI", "c05", larguraCelula, true, true, "direita", "#0", fontSize, false);
                var col06 = Class.Geral.retColTextBox("SEX", "c06", larguraCelula, true, true, "direita", "#0", fontSize, false);
                var col07 = Class.Geral.retColTextBox("SAB", "c07", larguraCelula, true, true, "direita", "#0", fontSize, false);

                grid.Columns.AddRange(new DataGridViewColumn[] { col01, col02, col03, col04, col05, col06, col07 });
                grid.AllowUserToAddRows = false;
                grid.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
                grid.RowHeadersVisible = false;
                grid.EditMode = DataGridViewEditMode.EditOnEnter;

                grid.ColumnHeadersHeight = columnHeadersHeight;

                grid.RowTemplate.Height = rowHeight;

                grid.Rows.Add();
                grid.Rows.Add();
                grid.Rows.Add();
                grid.Rows.Add();
                grid.Rows.Add();
                grid.Rows.Add();

                for (int i = 1; i < 7; i++)
                {
                    grid.Rows[i - 1].ReadOnly = true;
                    for (int j = 1; j < 8; j++)
                    {
                        numDia = RetornarDia(i, j, numGrid);
                        if (numDia > 0)
                        {
                            grid.Rows[i-1].Cells[j-1].Value = numDia.ToString();
                            grid.Rows[i-1].Cells[j-1].ToolTipText = "";

                            if (periodicidade > 1)
                            {
                                if (IsDiaEntrega(numDia, numGrid))
                                {
                                    dataAux = new DateTime(ano, numGrid, numDia);
                                    PintarDia(dataAux, "entrega");
                                }
                            }
                        }
                    }
                }

                grid.CurrentCell.Selected = false;
            }
            catch (Exception ex)
            {
                s = numDia.ToString();
                throw ex;
            }
        }

        //==================================================================================================================================//
        private void ConstruirMatrizFeriados()
        //==================================================================================================================================//
        {
            string sql, sDataIni, sDataFim;
            DataTable oDT;
            int i;

            try
            {
                feriados = new int[0, 0];
                sDataIni = ano.ToString().PadLeft(4, '0') + "01" + "01";
                sDataFim = ano.ToString().PadLeft(4, '0') + "12" + "31";

                sql = string.Format(@"SELECT ""StrDate"", ""EndDate"" FROM HLD1 WHERE ""StrDate"" BETWEEN '{0}' AND '{1}'", sDataIni, sDataFim);
                oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                if (oDT.Rows.Count > 0)
                {
                    i = 0;
                    feriados = new int[oDT.Rows.Count, 2];
                    foreach (DataRow linha in oDT.Rows)
                    {
                        feriados[i, 0] = Convert.ToInt32(Convert.ToDateTime(linha["StrDate"].ToString()).ToString("yyyyMMdd"));
                        feriados[i, 1] = Convert.ToInt32(Convert.ToDateTime(linha["EndDate"].ToString()).ToString("yyyyMMdd"));
                        i++;
                    }
                }
            }
            catch { }
        }

        //==================================================================================================================================//
        private void PintarFeriados()
        //==================================================================================================================================//
        {
            DateTime dataAux;
            int nDataIni, nDataFim, nDatAux, mes, dia, row = 0;
            string col = "";
            Color corCelula;

            try
            {
                for (int i = 0; i <= feriados.Length / 2; i++)
                {
                    nDataIni = feriados[i, 0];
                    nDataFim = feriados[i, 1];

                    nDatAux = nDataIni;
                    while (nDatAux <= nDataFim)
                    {
                        mes = Convert.ToInt32(nDatAux.ToString().Substring(4, 2));
                        dia = Convert.ToInt32(nDatAux.ToString().Substring(6, 2));
                        dataAux = new DateTime(ano, mes, dia);

                        DataGridView grid = grid01;

                        if (mes == 2)
                            grid = grid02;
                        if (mes == 3)
                            grid = grid03;
                        if (mes == 4)
                            grid = grid04;
                        if (mes == 5)
                            grid = grid05;
                        if (mes == 6)
                            grid = grid06;
                        if (mes == 7)
                            grid = grid07;
                        if (mes == 8)
                            grid = grid08;
                        if (mes == 9)
                            grid = grid09;
                        if (mes == 10)
                            grid = grid10;
                        if (mes == 11)
                            grid = grid11;
                        if (mes == 12)
                            grid = grid12;

                        RetornarRowCol(dataAux, ref row, ref col);

                        corCelula = grid.Rows[row].Cells[col].Style.BackColor;

                        if (corCelula == corDtEntrega)
                            PintarDia(dataAux, "ambos");
                        else
                            PintarDia(dataAux, "feriado");

                        nDatAux++;
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        //==================================================================================================================================//
        private bool IsFeriado(int mes, int dia)
        //==================================================================================================================================//
        {
            bool retorno = false;
            int nDataIni, nDataFim, nDatAux, nTeste;

            try
            {
                nTeste = ano * 10000 + mes * 100 + dia;

                for (int i = 0; i <= feriados.Length / 2; i++)
                {
                    nDataIni = feriados[i, 0];
                    nDataFim = feriados[i, 1];

                    nDatAux = nDataIni;
                    while (nDatAux <= nDataFim)
                    {
                        if (nTeste == nDatAux)
                        {
                            retorno = true;
                            break;
                        }

                        nDatAux++;
                    }
                }
            }
            catch { }

            return retorno;
        }

        //==================================================================================================================================//
        private void PintarDiasEntrega()
        //==================================================================================================================================//
        {
            DateTime dataAux;
            string sql, sDataIni, sDataFim;
            DataTable oDT;

            try
            {
                if (periodicidade == 0)
                {
                    sDataIni = ano.ToString() + "0101";
                    sDataFim = ano.ToString() + "1231";

                    sql = string.Format(@"
SELECT T1.""U_CVA_Data"" 
FROM ""@CVA_CAR_CAL"" T0
    INNER JOIN ""@CVA_CAR_CAL1"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
WHERE T0.""U_BPLId"" = {0} AND T1.""U_CVA_Data"" >= '{1}' AND T1.""U_CVA_Data"" <= '{2}' AND T0.""U_Categoria"" = {3}
", bplId, sDataIni, sDataFim, categoria);
                    oDT = Class.Conexao.ExecuteSqlDataTable(sql);

                    if (oDT.Rows.Count > 0)
                    {
                        foreach (DataRow linha in oDT.Rows)
                        {
                            DateTime.TryParse(linha["U_CVA_Data"].ToString(), out dataAux);
                            PintarDia(dataAux, "entrega");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //==================================================================================================================================//
        private void PintarDia(DateTime data, string caso)   // caso -> feriado / entrega / ambos / normal
        //==================================================================================================================================//
        {
            int month, row = 0;
            string col = "";

            month = data.Month;

            DataGridView grid = grid01;

            if (month == 2)
                grid = grid02;
            if (month == 3)
                grid = grid03;
            if (month == 4)
                grid = grid04;
            if (month == 5)
                grid = grid05;
            if (month == 6)
                grid = grid06;
            if (month == 7)
                grid = grid07;
            if (month == 8)
                grid = grid08;
            if (month == 9)
                grid = grid09;
            if (month == 10)
                grid = grid10;
            if (month == 11)
                grid = grid11;
            if (month == 12)
                grid = grid12;

            RetornarRowCol(data, ref row, ref col);

            if (caso == "feriado")
            {
                grid.Rows[row].Cells[col].Style = styleFeriado;
                grid.Rows[row].Cells[col].ToolTipText = RetornarFeriado(data);
            }
            else if (caso == "entrega")
            {
                grid.Rows[row].Cells[col].Style = styleDtEntrega;
            }
            else if (caso == "normal")
            {
                grid.Rows[row].Cells[col].Style = styleNormal;
            }
            else if (caso == "ambos")
            {
                grid.Rows[row].Cells[col].Style = styleAmbos;
                grid.Rows[row].Cells[col].ToolTipText = RetornarFeriado(data);
            }

        }

        //==================================================================================================================================//
        private string RetornarFeriado(DateTime data)
        //==================================================================================================================================//
        {
            string retorno = "", sql, sData;
            int year, month, day;

            try
            {
                year = data.Year;
                month = data.Month;
                day = data.Day;
                sData = year.ToString().PadLeft(4, '0') + month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0');
                sql = string.Format(@"SELECT ""Rmrks"" FROM HLD1 WHERE '{0}' BETWEEN ""StrDate"" AND ""EndDate""", sData);
                retorno = Class.Conexao.ExecuteSqlScalar(sql).ToString();
            }
            catch { }

            return retorno;
        }


        //==================================================================================================================================//
        private bool IsDiaEntrega(int dia, int mes)
        //==================================================================================================================================//
        {
            bool retorno = false;
            DateTime dataAux;

            if (periodicidade > 1)
            {
                dataAux = new DateTime(ano, mes, dia);

                // Toda segunda-feira
                if (periodicidade == 2)
                {
                    if ((int)dataAux.DayOfWeek == 1)
                        retorno = true;
                }
                // Toda terça-feira
                else if (periodicidade == 3)
                {
                    if ((int)dataAux.DayOfWeek == 2)
                        retorno = true;
                }
                // Toda quarta-feira
                else if (periodicidade == 4)
                {
                    if ((int)dataAux.DayOfWeek == 3)
                        retorno = true;
                }
                // Toda quinta-feira
                else if (periodicidade == 5)
                {
                    if ((int)dataAux.DayOfWeek == 4)
                        retorno = true;
                }
                // Toda sexta-feira
                else if (periodicidade == 6)
                {
                    if ((int)dataAux.DayOfWeek == 5)
                        retorno = true;
                }
                // Todo sábado
                else if (periodicidade == 7)
                {
                    if ((int)dataAux.DayOfWeek == 6)
                        retorno = true;
                }
                // Todo domingo
                else if (periodicidade == 8)
                {
                    if ((int)dataAux.DayOfWeek == 0)
                        retorno = true;
                }
            }

            return retorno;
        }

        //==================================================================================================================================//
        private void RetornarRowCol(DateTime data, ref int row, ref string col)
        //==================================================================================================================================//
        {
            int primDiaSemana, ultDiaMes, year, month, day, k = 0;
            DateTime dataAux;

            year = data.Year;
            month = data.Month;
            day = data.Day;

            dataAux = new DateTime(year, month, 1);
            primDiaSemana = (int)dataAux.DayOfWeek;
            ultDiaMes = new DateTime(dataAux.Year, dataAux.Month, DateTime.DaysInMonth(dataAux.Year, dataAux.Month)).Day;

            for (int i = 1; i <= 6; i++)
            {
                if (day <= (7 * i) - primDiaSemana)
                {
                    row = i - 1;
                    k = 7 - (((7 * i) - primDiaSemana) - day);
                    break;
                }
            }

            if (k == 1)
                col = "c01";
            else if (k == 2)
                col = "c02";
            else if (k == 3)
                col = "c03";
            else if (k == 4)
                col = "c04";
            else if (k == 5)
                col = "c05";
            else if (k == 6)
                col = "c06";
            else if (k == 7)
                col = "c07";
            else
                col = "";
        }


        //==================================================================================================================================//
        private int RetornarDia(int i, int j, int mes)
        //==================================================================================================================================//
        {
            int retorno = 0, primDiaSemana, ultDiaMes;
            DateTime dataAux;

            dataAux = new DateTime(ano, mes, 1);
            primDiaSemana = (int)dataAux.DayOfWeek;
            ultDiaMes = new DateTime(dataAux.Year, dataAux.Month, DateTime.DaysInMonth(dataAux.Year, dataAux.Month)).Day;

            retorno = (7 * (i - 1) + j) - primDiaSemana;

            if (retorno > ultDiaMes)
                retorno = 0;

            return retorno;
        }

        //==================================================================================================================================//
        private string RetornarTituloGrid(int mes)
        //==================================================================================================================================//
        {
            string retorno;
            DateTime dataAux;

            dataAux = new DateTime(ano, mes, 1);

            //retorno = dataAux.ToString(@"MMM / yyyy", new System.Globalization.CultureInfo("en-US")).ToUpper();
            retorno = dataAux.ToString(@"MMM / yyyy");

            return retorno;
        }


        //==================================================================================================================================//
        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //==================================================================================================================================//
        {
            string s;
            int month, day;
            DateTime dataAux, ultimoProc;

            try
            {
                string headerText = (sender as DataGridView).Columns[e.ColumnIndex].Name;
                int linha = e.RowIndex;
                Color corCelula;

                s = (sender as DataGridView).Rows[linha].Cells[headerText].Value.ToString();
                corCelula = (sender as DataGridView).Rows[linha].Cells[headerText].Style.BackColor;

                if (!string.IsNullOrEmpty(s))
                {
                    Int32.TryParse((sender as DataGridView).Name, out month);
                    Int32.TryParse(s, out day);
                    ultimoProc = RetornaUltimoProcessamento();
                    dataAux = new DateTime(ano, month, day);

                    if (ultimoProc >= dataAux)
                        MessageBox.Show("Essa data é anterior à última consolidação processada para esta filial");
                    else
                    {
                        if (corCelula == corAmbos)
                        {
                            (sender as DataGridView).Rows[linha].Cells[headerText].Style = styleFeriado;
                        }
                        else if (corCelula == corDtEntrega)
                        {
                            (sender as DataGridView).Rows[linha].Cells[headerText].Style = styleNormal;
                        }
                        else if (corCelula == corFeriado)
                        {
                            var result = MessageBox.Show(this, "Esta data é um feriado, confirma data de entrega?", "Confirmação", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                (sender as DataGridView).Rows[linha].Cells[headerText].Style = styleAmbos;
                            }
                        }
                        else
                        {
                            if (((int)dataAux.DayOfWeek == 0) || ((int)dataAux.DayOfWeek == 6))
                            {
                                var result = MessageBox.Show(this, "Esta data é em final de semana, confirma data de entrega?", "Confirmação", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    (sender as DataGridView).Rows[linha].Cells[headerText].Style = styleDtEntrega;
                                }
                            }
                            else
                            {
                                (sender as DataGridView).Rows[linha].Cells[headerText].Style = styleDtEntrega;
                            }
                        }

                        if (btnOk.Text == "OK")
                            btnOk.Text = "Atualizar";
                    }
                }

                (sender as DataGridView).CurrentCell.Selected = false;
            }
            catch (Exception ex)
            {

            }
        }

        //==================================================================================================================================//
        private DateTime RetornaUltimoProcessamento()
        //==================================================================================================================================//
        {
            string sql;
            DateTime dt;

            sql = string.Format(@"SELECT MAX(""U_DataAte"") FROM ""@CVA_CAR_LOTE"" WHERE ""U_ID_Filial"" = {0}", bplId);
            DateTime.TryParse(Class.Conexao.ExecuteSqlScalar(sql).ToString(), out dt);

            return dt;
        }

        //==================================================================================================================================//
        private void btnPesquisar_Click(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            string msg = "", sql, sDataIni, sDataFim;
            int resultado;
            bool prosseguir = true;

            if (btnOk.Text != "OK")
            {
                var result = MessageBox.Show(this, "Calendário de entrega foi alterado. Sair sem salvar?", "Confirmação", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    prosseguir = false;
                }
            }

            if (prosseguir)
            {
                try
                {
                    Int32.TryParse(txtAno.Text, out ano);
                    periodicidade = 0;
                    categoria = 0;
                    if (cbPeriodicidade.SelectedValue != null)
                        Int32.TryParse(cbPeriodicidade.SelectedValue.ToString(), out periodicidade);
                    Int32.TryParse(cbFilial.SelectedValue.ToString(), out bplId);
                    Int32.TryParse(cbCategoria.SelectedValue.ToString(), out categoria);

                    if (cbFilial.SelectedIndex <= 0)
                        msg = msg + "Nenhuma filial selecionada." + Environment.NewLine;

                    if (ano == 0)
                        msg = msg + "Ano em branco ou inválido." + Environment.NewLine;
                    else if (ano < 2017)
                        msg = msg + "Ano deve ser maior que 2017." + Environment.NewLine;
                    if (ano > (DateTime.Now.Year + 5))
                        msg = msg + "Ano deve ser menor que " + (DateTime.Now.Year + 5).ToString() + Environment.NewLine;

                    if (cbCategoria.SelectedIndex <= 0)
                        msg = msg + "Nenhuma categoria selecionada." + Environment.NewLine;


                    /*
                    if (string.IsNullOrEmpty(msg))
                    {
                        sDataIni = ano.ToString() + "0101";
                        sDataFim = ano.ToString() + "1231";

                        sql = string.Format(@"
SELECT COUNT(1)
FROM ""HLD1"" 
WHERE ""StrDate"" >= '{0}' AND ""StrDate"" <= '{1}'
", sDataIni, sDataFim);
                        resultado = (int)Class.Conexao.ExecuteSqlScalar(sql);

                        if (resultado == 0)
                        {
                            msg = msg + "Ano ainda sem feriados cadastrados" + Environment.NewLine;
                        }
                    }
                    */

                    if (string.IsNullOrEmpty(msg))
                    {
                        sDataIni = ano.ToString() + "0101";
                        sDataFim = ano.ToString() + "1231";

                        sql = string.Format(@"
SELECT COUNT(1) 
FROM ""@CVA_CAR_CAL"" T0
    INNER JOIN ""@CVA_CAR_CAL1"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
WHERE 
    T0.""U_BPLId"" = {0} 
    AND T0.""U_Categoria"" = {1}
    AND T1.""U_CVA_Data"" >= '{2}' 
    AND T1.""U_CVA_Data"" <= '{3}'
", bplId, categoria, sDataIni, sDataFim);
                        resultado = (int)Class.Conexao.ExecuteSqlScalar(sql);

                        if (resultado == 0)
                        {
                            if (periodicidade <= 0)
                                msg = msg + "Filal/Ano/Categoria ainda não cadastrada - Nenhuma periodicidade selecionada." + Environment.NewLine;
                        }
                        else
                        {
                            cbPeriodicidade.SelectedIndex = 0;
                            cbPeriodicidade.Text = "";
                            periodicidade = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

                btnOk.Text = "OK";
                if (string.IsNullOrEmpty(msg))
                {
                    MontarTela();
                }
                else
                {
                    ZerarTela();
                    MessageBox.Show(msg);
                }
            }
        }

        //==================================================================================================================================//
        private void ZerarTela()
        //==================================================================================================================================//
        {
            try
            {
                txt01.Text = "";
                txt02.Text = "";
                txt03.Text = "";
                txt04.Text = "";
                txt05.Text = "";
                txt06.Text = "";
                txt07.Text = "";
                txt08.Text = "";
                txt09.Text = "";
                txt10.Text = "";
                txt11.Text = "";
                txt12.Text = "";

                for (int i = 0; i <= 5; i++)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        grid01.Rows[i].Cells[j].Style = styleNormal;
                        grid01.Rows[i].Cells[j].Value = "";
                        grid01.Rows[i].Cells[j].ToolTipText = "";

                        grid02.Rows[i].Cells[j].Style = styleNormal;
                        grid02.Rows[i].Cells[j].Value = "";
                        grid02.Rows[i].Cells[j].ToolTipText = "";

                        grid03.Rows[i].Cells[j].Style = styleNormal;
                        grid03.Rows[i].Cells[j].Value = "";
                        grid03.Rows[i].Cells[j].ToolTipText = "";

                        grid04.Rows[i].Cells[j].Style = styleNormal;
                        grid04.Rows[i].Cells[j].Value = "";
                        grid04.Rows[i].Cells[j].ToolTipText = "";

                        grid05.Rows[i].Cells[j].Style = styleNormal;
                        grid05.Rows[i].Cells[j].Value = "";
                        grid05.Rows[i].Cells[j].ToolTipText = "";

                        grid06.Rows[i].Cells[j].Style = styleNormal;
                        grid06.Rows[i].Cells[j].Value = "";
                        grid06.Rows[i].Cells[j].ToolTipText = "";

                        grid07.Rows[i].Cells[j].Style = styleNormal;
                        grid07.Rows[i].Cells[j].Value = "";
                        grid07.Rows[i].Cells[j].ToolTipText = "";

                        grid08.Rows[i].Cells[j].Style = styleNormal;
                        grid08.Rows[i].Cells[j].Value = "";
                        grid08.Rows[i].Cells[j].ToolTipText = "";

                        grid09.Rows[i].Cells[j].Style = styleNormal;
                        grid09.Rows[i].Cells[j].Value = "";
                        grid09.Rows[i].Cells[j].ToolTipText = "";

                        grid10.Rows[i].Cells[j].Style = styleNormal;
                        grid10.Rows[i].Cells[j].Value = "";
                        grid10.Rows[i].Cells[j].ToolTipText = "";

                        grid11.Rows[i].Cells[j].Style = styleNormal;
                        grid11.Rows[i].Cells[j].Value = "";
                        grid11.Rows[i].Cells[j].ToolTipText = "";

                        grid12.Rows[i].Cells[j].Style = styleNormal;
                        grid12.Rows[i].Cells[j].Value = "";
                        grid12.Rows[i].Cells[j].ToolTipText = "";
                    }
                }
            }
            catch
            {

            }
        }

        //==================================================================================================================================//
        private void btnOk_Click(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            string msg = "", acao = btnOk.Text;
            DataGridView grid = null;
            int dia;
            Color cor;
            DateTime dDataAux;

            if (acao != "OK")
            {
                try
                {
                    Class.Conexao.oCompany.StartTransaction();

                    btnOk.Text = "Processando..."; btnOk.Refresh();

                    if (acao == "Atualizar")
                        DeletarDatas();

                    for (int nGrid = 1; nGrid <= 12; nGrid++)
                    {
                        if (nGrid == 1)
                            grid = grid01;
                        else if (nGrid == 2)
                            grid = grid02;
                        else if (nGrid == 3)
                            grid = grid03;
                        else if (nGrid == 4)
                            grid = grid04;
                        else if (nGrid == 5)
                            grid = grid05;
                        else if (nGrid == 6)
                            grid = grid06;
                        else if (nGrid == 7)
                            grid = grid07;
                        else if (nGrid == 8)
                            grid = grid08;
                        else if (nGrid == 9)
                            grid = grid09;
                        else if (nGrid == 10)
                            grid = grid10;
                        else if (nGrid == 11)
                            grid = grid11;
                        else if (nGrid == 12)
                            grid = grid12;

                        for (int i = 0; i <= 5; i++)
                        {
                            for (int j = 0; j <= 6; j++)
                            {
                                cor = grid.Rows[i].Cells[j].Style.BackColor;

                                if ((cor == corAmbos) || (cor == corDtEntrega))
                                {
                                    dia = RetornarDia(i + 1, j + 1, nGrid);
                                    dDataAux = new DateTime(ano, nGrid, dia);
                                    msg = Processar(dDataAux);
                                    if (!string.IsNullOrEmpty(msg))
                                        throw new Exception(msg);
                                }
                            }
                        }
                    }
                    Class.Conexao.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    if (Class.Conexao.oCompany.InTransaction) Class.Conexao.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                finally
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        btnOk.Text = "OK";
                        msg = "Calendário de entregas salvo com sucesso";
                    }
                    else
                        btnOk.Text = acao;
                    MessageBox.Show(msg);
                }
            }
        }

        //==================================================================================================================================//
        private void DeletarDatas()
        //==================================================================================================================================//
        {
            string sql = "", sDataIni, sDataFim;
            int docEntry;

            try
            {
                sDataIni = ano.ToString().PadLeft(4, '0') + "01" + "01";
                sDataFim = ano.ToString().PadLeft(4, '0') + "12" + "31";

                sql = string.Format(@"SELECT ""DocEntry"" FROM ""@CVA_CAR_CAL"" WHERE ""U_BPLId"" = {0} AND ""U_Categoria"" = {1} ", bplId, categoria);
                Int32.TryParse(Class.Conexao.ExecuteSqlScalar(sql).ToString(), out docEntry);

                if (docEntry > 0)
                {
                    sql = string.Format(@"
DELETE FROM ""@CVA_CAR_CAL1"" 
WHERE ""LineId"" IN 
    (
        SELECT T1.""LineId""
        FROM ""@CVA_CAR_CAL"" T0
            INNER JOIN ""@CVA_CAR_CAL1"" T1 ON T1.""DocEntry"" = T0.""DocEntry""
        WHERE T0.""U_BPLId"" = {0} AND T1.""U_CVA_Data"" >= '{1}' AND T1.""U_CVA_Data"" <= '{2}' AND T0.""U_Categoria"" = {3}
    )
AND ""DocEntry"" = {4}
", bplId, sDataIni, sDataFim, categoria, docEntry);
                    Class.Conexao.ExecuteSqlScalar(sql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================================================================================================================================//
        private string Processar(DateTime data)
        //==================================================================================================================================//
        {
            string sql = "", msg = "", acao = btnOk.Text, sData;
            int docEntry, lineId, year, month, day;

            try
            {
                year = data.Year;
                month = data.Month;
                day = data.Day;
                sData = year.ToString() + month.ToString().PadLeft(2, '0') + day.ToString().PadLeft(2, '0');

                docEntry = RetornaDocEntry(bplId);
                lineId = RetornaNextLineId(docEntry);

                sql = string.Format(@"INSERT INTO ""@CVA_CAR_CAL1"" (""DocEntry"", ""LineId"", ""U_CVA_Data"") VALUES ({0}, {1}, '{2}' )", docEntry, lineId, sData);
                Class.Conexao.ExecuteSqlScalar(sql);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }

        //==================================================================================================================================//
        private void FormCalendario_FormClosing(object sender, FormClosingEventArgs e)
        //==================================================================================================================================//
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (btnOk.Text != "OK")
                {
                    var result = MessageBox.Show(this, "Dados alterados, sair sem gravar?", "Confirmação", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes)
                    {
                        e.Cancel = true;
                    }
                }
            }

        }

        //==================================================================================================================================//
        private void pctFilial_Click(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            try
            {
                Class.CFL.retorno = "";
                Class.CFL.tabela = "OBPL";
                Class.CFL.chave = "BPLId";
                Class.CFL.tituloChave = "Cód Filial";
                Class.CFL.campo1 = "BPLName";
                Class.CFL.tituloCampo1 = "Nome da Filial";
                Class.CFL.campo2 = "BPLFrName";
                Class.CFL.tituloCampo2 = "Nome Fantasia";
                Class.CFL.titulo = "Lista de Filiais";

                if ((Class.CFL.tabela != "") && (Class.CFL.chave != "") && (Class.CFL.campo1 != ""))
                {
                    FormCFL cfl = new FormCFL();
                    cfl.ShowDialog();
                }

                if (!string.IsNullOrEmpty(Class.CFL.retorno))
                    cbFilial.SelectedValue = Convert.ToInt32(Class.CFL.retorno);
            }
            catch { }
        }

        //==================================================================================================================================//
        private void btnCancelar_Click(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            this.Close();
        }

        //==================================================================================================================================//
        private int RetornaDocEntry(int bplId)
        //==================================================================================================================================//
        {
            int retorno = 0, nAux;
            string sql, s;

            try
            {
                sql = string.Format(@"SELECT COUNT(1) FROM ""@CVA_CAR_CAL"" WHERE ""U_BPLId"" = {0} AND ""U_Categoria"" = {1} ", bplId, categoria);
                nAux = (int)Class.Conexao.ExecuteSqlScalar(sql);

                if (nAux == 0)
                {
                    sql = string.Format(@"SELECT MAX(""DocEntry"") FROM ""@CVA_CAR_CAL"" ");
                    s = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                    if (string.IsNullOrEmpty(s))
                        s = "1";
                    else
                        s = (Convert.ToInt32(s) + 1).ToString();

                    sql = string.Format(@"INSERT INTO ""@CVA_CAR_CAL"" (""DocEntry"", ""U_BPLId"", ""U_Period"", ""U_Categoria"") VALUES ({0}, {1}, {2}, {3} ) ", s, bplId, periodicidade, categoria);
                    Class.Conexao.ExecuteSqlScalar(sql);
                }
                else
                {
                    sql = string.Format(@"SELECT ""DocEntry"" FROM ""@CVA_CAR_CAL"" WHERE ""U_BPLId"" = {0} AND ""U_Categoria"" = {1} ", bplId, categoria);
                    s = Class.Conexao.ExecuteSqlScalar(sql).ToString();
                }

                retorno = Convert.ToInt32(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }

        //==================================================================================================================================//
        private int RetornaNextLineId(int docEntry)
        //==================================================================================================================================//
        {
            int retorno = 0, nAux;
            string sql;

            try
            {
                sql = string.Format(@"SELECT COUNT(1) FROM ""@CVA_CAR_CAL1"" WHERE ""DocEntry"" = {0} ", docEntry);
                nAux = (int)Class.Conexao.ExecuteSqlScalar(sql);

                if (nAux > 0)
                {
                    sql = string.Format(@"SELECT MAX(""LineId"") FROM ""@CVA_CAR_CAL1"" WHERE ""DocEntry"" = {0} ", docEntry);
                    nAux = (int)Class.Conexao.ExecuteSqlScalar(sql);
                }

                retorno = nAux + 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retorno;
        }


    }
}
