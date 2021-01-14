using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_Battle_client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_spm_Click(object sender, EventArgs e)
        {
            Tetris_Battle_spc tetris_Battle_Spc = new Tetris_Battle_spc();
            DialogResult  result = tetris_Battle_Spc.ShowDialog();
        }

        private void btn_tpm_Click(object sender, EventArgs e)
        {
            Tetris_Battle_tpm tetris_Battle_Tpm = new Tetris_Battle_tpm();
            DialogResult result = tetris_Battle_Tpm.ShowDialog();
        }

        private void btn_setting_Click(object sender, EventArgs e)
        {
            MessageBox.Show("W=順時鐘翻轉\nS=逆時鐘翻轉\nA=向左移動\n" +
                "D=向右移動\nE=向下移動\nJ=快速放置\nL=退出", "操作",MessageBoxButtons.OK);
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label_title_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                this.Close();
            }
        }
    }
}
