namespace CVA.DB.Query
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
            this.Label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtPasswDB = new System.Windows.Forms.TextBox();
            this.txtUserDB = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxQuery = new System.Windows.Forms.TextBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.ofdQuery = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(11, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(46, 13);
            this.Label1.TabIndex = 41;
            this.Label1.Text = "Servidor";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(105, 12);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(100, 20);
            this.txtServer.TabIndex = 42;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(11, 41);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(61, 13);
            this.Label3.TabIndex = 43;
            this.Label3.Text = "Usuário BD";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(11, 67);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(56, 13);
            this.Label4.TabIndex = 45;
            this.Label4.Text = "Senha BD";
            // 
            // txtPasswDB
            // 
            this.txtPasswDB.Location = new System.Drawing.Point(105, 64);
            this.txtPasswDB.Name = "txtPasswDB";
            this.txtPasswDB.Size = new System.Drawing.Size(100, 20);
            this.txtPasswDB.TabIndex = 46;
            this.txtPasswDB.UseSystemPasswordChar = true;
            // 
            // txtUserDB
            // 
            this.txtUserDB.Location = new System.Drawing.Point(105, 38);
            this.txtUserDB.Name = "txtUserDB";
            this.txtUserDB.Size = new System.Drawing.Size(100, 20);
            this.txtUserDB.TabIndex = 44;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(14, 218);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 22);
            this.btnOK.TabIndex = 53;
            this.btnOK.Text = "Processar";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 54;
            this.label5.Text = "Query";
            // 
            // tbxQuery
            // 
            this.tbxQuery.Location = new System.Drawing.Point(105, 90);
            this.tbxQuery.Name = "tbxQuery";
            this.tbxQuery.Size = new System.Drawing.Size(100, 20);
            this.tbxQuery.TabIndex = 55;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(210, 88);
            this.btnQuery.Margin = new System.Windows.Forms.Padding(2);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(32, 22);
            this.btnQuery.TabIndex = 56;
            this.btnQuery.Text = "...";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // ofdQuery
            // 
            this.ofdQuery.Filter = "SQL|*.sql";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbxQuery);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.txtPasswDB);
            this.Controls.Add(this.txtUserDB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.TextBox txtPasswDB;
        private System.Windows.Forms.TextBox txtUserDB;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxQuery;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.OpenFileDialog ofdQuery;
    }
}

