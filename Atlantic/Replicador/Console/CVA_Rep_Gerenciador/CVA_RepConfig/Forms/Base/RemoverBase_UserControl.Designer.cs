namespace CVA_RepConfig.Forms.Base
{
	partial class RemoverBase_UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoverBase_UserControl));
            this.tbCOMP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.cbUSE_TRU = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbSTU = new System.Windows.Forms.ComboBox();
            this.tbPAS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbUNAME = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSRVR = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbUPD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbINS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.tbDB_PAS = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbDB_UNAME = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbDB_TYP = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbDB_SRVR = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbCOMP
            // 
            this.tbCOMP.Location = new System.Drawing.Point(116, 138);
            this.tbCOMP.Name = "tbCOMP";
            this.tbCOMP.Size = new System.Drawing.Size(100, 20);
            this.tbCOMP.TabIndex = 53;
            this.tbCOMP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCOMP_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 60;
            this.label9.Text = "Empresa";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(90, 194);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 59;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(9, 194);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 58;
            this.btnSalvar.Text = "Apagar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // cbUSE_TRU
            // 
            this.cbUSE_TRU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUSE_TRU.FormattingEnabled = true;
            this.cbUSE_TRU.Location = new System.Drawing.Point(313, 138);
            this.cbUSE_TRU.Name = "cbUSE_TRU";
            this.cbUSE_TRU.Size = new System.Drawing.Size(170, 21);
            this.cbUSE_TRU.TabIndex = 54;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(222, 142);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 57;
            this.label8.Text = "Habilitar SSL";
            // 
            // cbSTU
            // 
            this.cbSTU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSTU.FormattingEnabled = true;
            this.cbSTU.Location = new System.Drawing.Point(116, 164);
            this.cbSTU.Name = "cbSTU";
            this.cbSTU.Size = new System.Drawing.Size(100, 21);
            this.cbSTU.TabIndex = 47;
            // 
            // tbPAS
            // 
            this.tbPAS.Location = new System.Drawing.Point(313, 31);
            this.tbPAS.Name = "tbPAS";
            this.tbPAS.PasswordChar = '*';
            this.tbPAS.Size = new System.Drawing.Size(170, 20);
            this.tbPAS.TabIndex = 51;
            this.tbPAS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPAS_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(222, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 56;
            this.label7.Text = "Senha";
            // 
            // tbUNAME
            // 
            this.tbUNAME.Location = new System.Drawing.Point(116, 31);
            this.tbUNAME.Name = "tbUNAME";
            this.tbUNAME.Size = new System.Drawing.Size(100, 20);
            this.tbUNAME.TabIndex = 50;
            this.tbUNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUNAME_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 55;
            this.label4.Text = "Usuário";
            // 
            // tbSRVR
            // 
            this.tbSRVR.Location = new System.Drawing.Point(116, 57);
            this.tbSRVR.Name = "tbSRVR";
            this.tbSRVR.Size = new System.Drawing.Size(365, 20);
            this.tbSRVR.TabIndex = 48;
            this.tbSRVR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSRVR_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Servidor de licenças";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 49;
            this.label6.Text = "Status do cadastro";
            // 
            // tbUPD
            // 
            this.tbUPD.Enabled = false;
            this.tbUPD.Location = new System.Drawing.Point(564, 83);
            this.tbUPD.Name = "tbUPD";
            this.tbUPD.Size = new System.Drawing.Size(100, 20);
            this.tbUPD.TabIndex = 45;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(489, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 46;
            this.label3.Text = "Atualizado";
            // 
            // tbINS
            // 
            this.tbINS.Enabled = false;
            this.tbINS.Location = new System.Drawing.Point(564, 57);
            this.tbINS.Name = "tbINS";
            this.tbINS.Size = new System.Drawing.Size(100, 20);
            this.tbINS.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(489, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Cadastrado";
            // 
            // tbID
            // 
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(564, 31);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(100, 20);
            this.tbID.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(489, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "ID";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(667, 25);
            this.bindingNavigator1.TabIndex = 40;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // tbDB_PAS
            // 
            this.tbDB_PAS.Location = new System.Drawing.Point(313, 112);
            this.tbDB_PAS.Name = "tbDB_PAS";
            this.tbDB_PAS.PasswordChar = '*';
            this.tbDB_PAS.Size = new System.Drawing.Size(170, 20);
            this.tbDB_PAS.TabIndex = 66;
            this.tbDB_PAS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_PAS_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(222, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 68;
            this.label11.Text = "Senha do banco";
            // 
            // tbDB_UNAME
            // 
            this.tbDB_UNAME.Location = new System.Drawing.Point(116, 112);
            this.tbDB_UNAME.Name = "tbDB_UNAME";
            this.tbDB_UNAME.Size = new System.Drawing.Size(100, 20);
            this.tbDB_UNAME.TabIndex = 65;
            this.tbDB_UNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_UNAME_KeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 116);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 67;
            this.label12.Text = "Usuário do banco";
            // 
            // cbDB_TYP
            // 
            this.cbDB_TYP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDB_TYP.FormattingEnabled = true;
            this.cbDB_TYP.Location = new System.Drawing.Point(116, 83);
            this.cbDB_TYP.Name = "cbDB_TYP";
            this.cbDB_TYP.Size = new System.Drawing.Size(100, 21);
            this.cbDB_TYP.TabIndex = 64;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 13);
            this.label10.TabIndex = 63;
            this.label10.Text = "Tipo de conexão";
            // 
            // tbDB_SRVR
            // 
            this.tbDB_SRVR.Location = new System.Drawing.Point(313, 83);
            this.tbDB_SRVR.Name = "tbDB_SRVR";
            this.tbDB_SRVR.Size = new System.Drawing.Size(170, 20);
            this.tbDB_SRVR.TabIndex = 61;
            this.tbDB_SRVR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_SRVR_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(222, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 13);
            this.label13.TabIndex = 62;
            this.label13.Text = "Banco de dados";
            // 
            // RemoverBase_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Salmon;
            this.Controls.Add(this.tbDB_PAS);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbDB_UNAME);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbDB_TYP);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbDB_SRVR);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbCOMP);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.cbUSE_TRU);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbSTU);
            this.Controls.Add(this.tbPAS);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbUNAME);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbSRVR);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbUPD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbINS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "RemoverBase_UserControl";
            this.Size = new System.Drawing.Size(667, 224);
            this.Load += new System.EventHandler(this.RemoverBase_UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbCOMP;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnCancelar;
		private System.Windows.Forms.Button btnSalvar;
		private System.Windows.Forms.ComboBox cbUSE_TRU;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cbSTU;
		private System.Windows.Forms.TextBox tbPAS;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox tbUNAME;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbSRVR;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbUPD;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbINS;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbID;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
		private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
		private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
		private System.Windows.Forms.BindingSource bindingSource1;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
		private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.TextBox tbDB_PAS;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbDB_UNAME;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbDB_TYP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbDB_SRVR;
        private System.Windows.Forms.Label label13;
    }
}
