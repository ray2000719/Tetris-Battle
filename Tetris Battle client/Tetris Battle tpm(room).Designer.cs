namespace Tetris_Battle_client
{
    partial class Tetris_Battle_tpm_room_
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
            this.components = new System.ComponentModel.Container();
            this.flp_cubenext = new System.Windows.Forms.FlowLayoutPanel();
            this.rtb_gameScreen = new System.Windows.Forms.RichTextBox();
            this.rtb_msg = new System.Windows.Forms.RichTextBox();
            this.tb_msg = new System.Windows.Forms.TextBox();
            this.btn_msgsend = new System.Windows.Forms.Button();
            this.rtb_gameScreen2 = new System.Windows.Forms.RichTextBox();
            this.btn_ready = new System.Windows.Forms.Button();
            this.label_ID = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // flp_cubenext
            // 
            this.flp_cubenext.Location = new System.Drawing.Point(370, 39);
            this.flp_cubenext.Name = "flp_cubenext";
            this.flp_cubenext.Size = new System.Drawing.Size(100, 350);
            this.flp_cubenext.TabIndex = 6;
            // 
            // rtb_gameScreen
            // 
            this.rtb_gameScreen.Enabled = false;
            this.rtb_gameScreen.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.rtb_gameScreen.Location = new System.Drawing.Point(90, 39);
            this.rtb_gameScreen.Name = "rtb_gameScreen";
            this.rtb_gameScreen.ShowSelectionMargin = true;
            this.rtb_gameScreen.Size = new System.Drawing.Size(275, 525);
            this.rtb_gameScreen.TabIndex = 99;
            this.rtb_gameScreen.Text = "";
            // 
            // rtb_msg
            // 
            this.rtb_msg.Location = new System.Drawing.Point(90, 570);
            this.rtb_msg.Name = "rtb_msg";
            this.rtb_msg.Size = new System.Drawing.Size(785, 107);
            this.rtb_msg.TabIndex = 7;
            this.rtb_msg.Text = "";
            // 
            // tb_msg
            // 
            this.tb_msg.Font = new System.Drawing.Font("新細明體", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tb_msg.Location = new System.Drawing.Point(93, 701);
            this.tb_msg.Name = "tb_msg";
            this.tb_msg.Size = new System.Drawing.Size(703, 35);
            this.tb_msg.TabIndex = 8;
            // 
            // btn_msgsend
            // 
            this.btn_msgsend.Location = new System.Drawing.Point(823, 700);
            this.btn_msgsend.Name = "btn_msgsend";
            this.btn_msgsend.Size = new System.Drawing.Size(97, 35);
            this.btn_msgsend.TabIndex = 9;
            this.btn_msgsend.Text = "送出訊息";
            this.btn_msgsend.UseVisualStyleBackColor = true;
            this.btn_msgsend.Click += new System.EventHandler(this.btn_msgsend_Click);
            // 
            // rtb_gameScreen2
            // 
            this.rtb_gameScreen2.Enabled = false;
            this.rtb_gameScreen2.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.rtb_gameScreen2.Location = new System.Drawing.Point(600, 39);
            this.rtb_gameScreen2.Name = "rtb_gameScreen2";
            this.rtb_gameScreen2.ShowSelectionMargin = true;
            this.rtb_gameScreen2.Size = new System.Drawing.Size(275, 525);
            this.rtb_gameScreen2.TabIndex = 10;
            this.rtb_gameScreen2.Text = "";
            // 
            // btn_ready
            // 
            this.btn_ready.Font = new System.Drawing.Font("標楷體", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_ready.ForeColor = System.Drawing.Color.Black;
            this.btn_ready.Location = new System.Drawing.Point(416, 426);
            this.btn_ready.Name = "btn_ready";
            this.btn_ready.Size = new System.Drawing.Size(125, 100);
            this.btn_ready.TabIndex = 100;
            this.btn_ready.Text = "準備\r\n";
            this.btn_ready.UseVisualStyleBackColor = true;
            this.btn_ready.Click += new System.EventHandler(this.btn_ready_Click);
            // 
            // label_ID
            // 
            this.label_ID.AutoSize = true;
            this.label_ID.Font = new System.Drawing.Font("新細明體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_ID.Location = new System.Drawing.Point(95, 44);
            this.label_ID.Name = "label_ID";
            this.label_ID.Size = new System.Drawing.Size(47, 33);
            this.label_ID.TabIndex = 101;
            this.label_ID.Text = "11";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Tetris_Battle_tpm_room_
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(982, 753);
            this.Controls.Add(this.label_ID);
            this.Controls.Add(this.btn_ready);
            this.Controls.Add(this.rtb_gameScreen2);
            this.Controls.Add(this.btn_msgsend);
            this.Controls.Add(this.tb_msg);
            this.Controls.Add(this.rtb_msg);
            this.Controls.Add(this.flp_cubenext);
            this.Controls.Add(this.rtb_gameScreen);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Tetris_Battle_tpm_room_";
            this.Text = "Tetris_Battle_tpm_room_";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tetris_Battle_tpm_room__FormClosing);
            this.Load += new System.EventHandler(this.Tetris_Battle_tpm_room__Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetris_Battle_tpm_room__KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flp_cubenext;
        private System.Windows.Forms.RichTextBox rtb_gameScreen;
        private System.Windows.Forms.TextBox tb_msg;
        private System.Windows.Forms.Button btn_msgsend;
        private System.Windows.Forms.RichTextBox rtb_msg;
        private System.Windows.Forms.RichTextBox rtb_gameScreen2;
        private System.Windows.Forms.Button btn_ready;
        private System.Windows.Forms.Label label_ID;
        private System.Windows.Forms.Timer timer1;
    }
}