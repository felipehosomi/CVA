namespace Conferencia
{
    partial class FrmInicial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInicial));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_GestaoPicking = new System.Windows.Forms.Button();
            this.btn_2Conf = new System.Windows.Forms.Button();
            this.btn_1Conf = new System.Windows.Forms.Button();
            this.btn_listaPedidos = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(368, 39);
            this.label1.TabIndex = 4;
            this.label1.Text = "Operações Disponiveis";
            // 
            // btn_GestaoPicking
            // 
            this.btn_GestaoPicking.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btn_GestaoPicking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_GestaoPicking.FlatAppearance.BorderSize = 0;
            this.btn_GestaoPicking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_GestaoPicking.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_GestaoPicking.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_GestaoPicking.Image = global::Conferencia.Properties.Resources.Picking;
            this.btn_GestaoPicking.Location = new System.Drawing.Point(164, 87);
            this.btn_GestaoPicking.Name = "btn_GestaoPicking";
            this.btn_GestaoPicking.Size = new System.Drawing.Size(135, 142);
            this.btn_GestaoPicking.TabIndex = 6;
            this.btn_GestaoPicking.Text = "Gestão de Picking";
            this.btn_GestaoPicking.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_GestaoPicking.UseVisualStyleBackColor = false;
            this.btn_GestaoPicking.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_GestaoPicking_MouseClick);
            // 
            // btn_2Conf
            // 
            this.btn_2Conf.FlatAppearance.BorderSize = 0;
            this.btn_2Conf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_2Conf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_2Conf.Image = ((System.Drawing.Image)(resources.GetObject("btn_2Conf.Image")));
            this.btn_2Conf.Location = new System.Drawing.Point(438, 104);
            this.btn_2Conf.Name = "btn_2Conf";
            this.btn_2Conf.Size = new System.Drawing.Size(115, 125);
            this.btn_2Conf.TabIndex = 3;
            this.btn_2Conf.Text = "2º Conferencia";
            this.btn_2Conf.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_2Conf.UseVisualStyleBackColor = true;
            this.btn_2Conf.Click += new System.EventHandler(this.button3_Click);
            // 
            // btn_1Conf
            // 
            this.btn_1Conf.FlatAppearance.BorderSize = 0;
            this.btn_1Conf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_1Conf.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_1Conf.Image = ((System.Drawing.Image)(resources.GetObject("btn_1Conf.Image")));
            this.btn_1Conf.Location = new System.Drawing.Point(305, 104);
            this.btn_1Conf.Name = "btn_1Conf";
            this.btn_1Conf.Size = new System.Drawing.Size(127, 125);
            this.btn_1Conf.TabIndex = 2;
            this.btn_1Conf.Text = "1º Conferência";
            this.btn_1Conf.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_1Conf.UseVisualStyleBackColor = true;
            this.btn_1Conf.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_listaPedidos
            // 
            this.btn_listaPedidos.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btn_listaPedidos.FlatAppearance.BorderSize = 0;
            this.btn_listaPedidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_listaPedidos.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_listaPedidos.Image = ((System.Drawing.Image)(resources.GetObject("btn_listaPedidos.Image")));
            this.btn_listaPedidos.Location = new System.Drawing.Point(32, 104);
            this.btn_listaPedidos.Name = "btn_listaPedidos";
            this.btn_listaPedidos.Size = new System.Drawing.Size(126, 125);
            this.btn_listaPedidos.TabIndex = 1;
            this.btn_listaPedidos.Text = "Lista de Pedidos";
            this.btn_listaPedidos.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_listaPedidos.UseVisualStyleBackColor = false;
            this.btn_listaPedidos.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Conferencia.Properties.Resources._21botaoSair;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Maroon;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(1188, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "SAIR";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FrmInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1268, 495);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_GestaoPicking);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_2Conf);
            this.Controls.Add(this.btn_1Conf);
            this.Controls.Add(this.btn_listaPedidos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmInicial";
            this.Text = "Tela Inicial";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmInicial_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmInicial_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_listaPedidos;
        private System.Windows.Forms.Button btn_1Conf;
        private System.Windows.Forms.Button btn_2Conf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_GestaoPicking;
        private System.Windows.Forms.Button button1;
    }
}