using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_Battle_client
{
    public partial class Tetris_Battle_connet : Form
    {
        public Tetris_Battle_connet()
        {
            InitializeComponent();
        }

        private void btn_connet_Click(object sender, EventArgs e)
        {
            Tetris_Battle_tpm_room_ tetris_Battle_Tpm_Room = new Tetris_Battle_tpm_room_();
            tetris_Battle_Tpm_Room.ip = IPAddress.Parse(cbbIP.Text);
            tetris_Battle_Tpm_Room._port = int.Parse(tbPort.Text);
            tetris_Battle_Tpm_Room.Show();
        }

        private void Tetris_Battle_connet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
