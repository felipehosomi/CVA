using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVA_Rep_DAL;
using CVA_Rep_BLL;

namespace CVA_RepConfig.Forms.AcessoCVA
{
    public partial class AcessoCVA_Form : UserControl
    {
        CVA_TIM_BLL tim_bll = new CVA_TIM_BLL();
        CVA_REG_BLL reg_bll = new CVA_REG_BLL();

        public AcessoCVA_Form()
        {
            InitializeComponent();
            dgCVA_TIM.Visible = false;
            dgCVA_REG.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            bCVA_TIM.Visible = false;
            bCVA_REG.Visible = false;
        }

        private void AcessoCVA_Form_Load(object sender, EventArgs e)
        {
            var listaTIM = tim_bll.GetAll().ToList();

            foreach (var rs in listaTIM)
            {
                dgCVA_TIM.Rows.Add(rs.ID, rs.INS, rs.UPD, rs.STU, rs.TIM, rs.NUM_OBJ);
            }

            var listREG = reg_bll.GetAll().ToList();

            foreach (var rs in listREG)
            {
                dgCVA_REG.Rows.Add(rs.ID, rs.INS, rs.UPD, rs.STU, rs.BAS, rs.CODE, rs.OBJ, rs.FUNC, rs.BAS_ERR);
            }
        }

        //Realiza o UPDATE na tabela CVA_TIM
        private void bCVA_TIM_Click(object sender, EventArgs e)
        {
            try
            {
                CVA_TIM obj;
                int lineIndex;
                int id;

                lineIndex = dgCVA_TIM.CurrentRow.Index;
                id = Convert.ToInt32(dgCVA_TIM.Rows[lineIndex].Cells["ID_TIM"].FormattedValue.ToString());
                obj = tim_bll.GetById(id);

                obj.UPD = DateTime.Now;
                obj.STU = Convert.ToInt32(dgCVA_TIM.Rows[lineIndex].Cells["STU_TIM"].FormattedValue.ToString());
                obj.TIM = Convert.ToInt32(dgCVA_TIM.Rows[lineIndex].Cells["TIM_TIM"].FormattedValue.ToString());
                obj.NUM_OBJ = Convert.ToInt32(dgCVA_TIM.Rows[lineIndex].Cells["NUM_OBJ_TIM"].FormattedValue.ToString());

                tim_bll.Update(obj);
                MessageBox.Show("CVA_TIM atualizada com sucesso.");
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível atualizar CVA_TIM. Verifique os dados informados.");
            }
        }

        //Realiza o UPDATE na tabela CVA_REG
        private void bCVA_REG_Click(object sender, EventArgs e)
        {
            try
            {
                CVA_REG obj;
                int id;

                for (int i = 0; i < dgCVA_REG.RowCount - 1; i++)
                {
                    if (!string.IsNullOrEmpty(dgCVA_REG.Rows[i].Cells["Changed"].FormattedValue.ToString()))
                    {
                        id = Convert.ToInt32(dgCVA_REG.Rows[i].Cells["ID_REG"].FormattedValue.ToString());
                        obj = reg_bll.GetById(id);

                        obj.UPD = DateTime.Now;
                        obj.STU = Convert.ToInt32(dgCVA_REG.Rows[i].Cells["STU_REG"].FormattedValue.ToString());
                        obj.BAS = Convert.ToInt32(dgCVA_REG.Rows[i].Cells["BAS_REG"].FormattedValue.ToString());
                        obj.CODE = dgCVA_REG.Rows[i].Cells["CODE_REG"].FormattedValue.ToString();
                        obj.OBJ = Convert.ToInt32(dgCVA_REG.Rows[i].Cells["OBJ_REG"].FormattedValue.ToString());
                        obj.FUNC = Convert.ToInt32(dgCVA_REG.Rows[i].Cells["FUNC_REG"].FormattedValue.ToString());

                        if (!string.IsNullOrEmpty(dgCVA_REG.Rows[i].Cells["BAS_ERR_REG"].FormattedValue.ToString()))
                        {
                            obj.BAS_ERR = Convert.ToInt32(dgCVA_REG.Rows[i].Cells["BAS_ERR_REG"].FormattedValue.ToString());
                        }

                        reg_bll.Update(obj);
                        dgCVA_REG.Rows[i].Cells["Changed"].Value = null;
                    }
                }
                MessageBox.Show("CVA_REG atualizada com sucesso.");
            }
            catch (Exception)
            {
                MessageBox.Show("Não foi possível atualizar CVA_REG. Verifique os dados informados.");
            }
        }

        private void bInput_Click(object sender, EventArgs e)
        {
            AcessoDAO dao = new AcessoDAO();

            if (dao.Grant_Access(tSenha.Text, tEmail.Text))
            {
                dgCVA_TIM.Visible = true;
                dgCVA_REG.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                bCVA_TIM.Visible = true;
                bCVA_REG.Visible = true;

                tEmail.Visible = false;
                tSenha.Visible = false;
                bInput.Visible = false;
                label3.Visible = false;
                label5.Visible = false;
            }
            else
            {
                MessageBox.Show("Usuário e/ou Senha incorretos", "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tEmail.Text = null;
                tSenha.Text = null;
            }
        }

        private void dgCVA_REG_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int lineIndex;
            lineIndex = dgCVA_REG.CurrentRow.Index;

            dgCVA_REG.Rows[lineIndex].Cells["Changed"].Value = "S";
        }
    }
}

