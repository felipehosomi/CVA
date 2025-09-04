namespace CVA.View.AdicionaTabelaUsuario
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxLicenceServer = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.cbDbType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtPasswSAP = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtUserSAP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPasswDB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUserDB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 298);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Adicionar";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 17);
            this.label8.TabIndex = 24;
            this.label8.Text = "Servidor Licença";
            // 
            // tbxLicenceServer
            // 
            this.tbxLicenceServer.Location = new System.Drawing.Point(138, 16);
            this.tbxLicenceServer.Margin = new System.Windows.Forms.Padding(4);
            this.tbxLicenceServer.Name = "tbxLicenceServer";
            this.tbxLicenceServer.Size = new System.Drawing.Size(132, 22);
            this.tbxLicenceServer.TabIndex = 25;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(13, 50);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(61, 17);
            this.Label1.TabIndex = 10;
            this.Label1.Text = "Servidor";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(138, 46);
            this.txtServer.Margin = new System.Windows.Forms.Padding(4);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(132, 22);
            this.txtServer.TabIndex = 11;
            // 
            // cbDbType
            // 
            this.cbDbType.FormattingEnabled = true;
            this.cbDbType.Items.AddRange(new object[] {
            "MSSQL",
            "DB_2",
            "SYBASE",
            "MSSQL2005",
            "MAXDB",
            "MSSQL2008",
            "MSSQL2012"});
            this.cbDbType.Location = new System.Drawing.Point(138, 78);
            this.cbDbType.Margin = new System.Windows.Forms.Padding(4);
            this.cbDbType.Name = "cbDbType";
            this.cbDbType.Size = new System.Drawing.Size(132, 24);
            this.cbDbType.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 82);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "Tipo de Servidor";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(13, 147);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(80, 17);
            this.Label3.TabIndex = 16;
            this.Label3.Text = "Usuário BD";
            // 
            // txtPasswSAP
            // 
            this.txtPasswSAP.Location = new System.Drawing.Point(138, 239);
            this.txtPasswSAP.Margin = new System.Windows.Forms.Padding(4);
            this.txtPasswSAP.Name = "txtPasswSAP";
            this.txtPasswSAP.Size = new System.Drawing.Size(132, 22);
            this.txtPasswSAP.TabIndex = 22;
            this.txtPasswSAP.UseSystemPasswordChar = true;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(13, 179);
            this.Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(72, 17);
            this.Label4.TabIndex = 18;
            this.Label4.Text = "Senha BD";
            // 
            // txtUserSAP
            // 
            this.txtUserSAP.Location = new System.Drawing.Point(138, 207);
            this.txtUserSAP.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserSAP.Name = "txtUserSAP";
            this.txtUserSAP.Size = new System.Drawing.Size(132, 22);
            this.txtUserSAP.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 211);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 17);
            this.label5.TabIndex = 21;
            this.label5.Text = "Usuário SAP";
            // 
            // txtPasswDB
            // 
            this.txtPasswDB.Location = new System.Drawing.Point(138, 175);
            this.txtPasswDB.Margin = new System.Windows.Forms.Padding(4);
            this.txtPasswDB.Name = "txtPasswDB";
            this.txtPasswDB.Size = new System.Drawing.Size(132, 22);
            this.txtPasswDB.TabIndex = 19;
            this.txtPasswDB.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 243);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 17);
            this.label6.TabIndex = 23;
            this.label6.Text = "Senha SAP";
            // 
            // txtUserDB
            // 
            this.txtUserDB.Location = new System.Drawing.Point(138, 143);
            this.txtUserDB.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserDB.Name = "txtUserDB";
            this.txtUserDB.Size = new System.Drawing.Size(132, 22);
            this.txtUserDB.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 347);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbxLicenceServer);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.cbDbType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.txtPasswSAP);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.txtUserSAP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPasswDB);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtUserDB);
            this.Controls.Add(this.btnAdd);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbxLicenceServer;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.ComboBox cbDbType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.TextBox txtPasswSAP;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.TextBox txtUserSAP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPasswDB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUserDB;
    }
}

