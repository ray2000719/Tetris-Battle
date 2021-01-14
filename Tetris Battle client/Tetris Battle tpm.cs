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
    public partial class Tetris_Battle_tpm : Form
    {
        public Tetris_Battle_tpm()
        {
            InitializeComponent();
        }

        private void btn_createroom_Click(object sender, EventArgs e)
        {
            Tetris_Battle_sever tetris_Battle_Sever = new Tetris_Battle_sever();

            tetris_Battle_Sever.ShowDialog();
        }

        private void btn_connet_Click(object sender, EventArgs e)
        {
            Tetris_Battle_connet tetris_Battle_Connet = new Tetris_Battle_connet();

            tetris_Battle_Connet.ShowDialog();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Tetris_Battle_tpm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                this.Close();
            }
        }
    }
}
