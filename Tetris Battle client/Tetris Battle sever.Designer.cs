namespace Tetris_Battle_client
{
    partial class Tetris_Battle_sever
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
            this.rtb_msg = new System.Windows.Forms.RichTextBox();
            this.gb = new System.Windows.Forms.GroupBox();
            this.btn_connet = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.cbbIP = new System.Windows.Forms.ComboBox();
            this.label_Port = new System.Windows.Forms.Label();
            this.label_IP = new System.Windows.Forms.Label();
            this.gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtb_msg
            // 
            this.rtb_msg.Location = new System.Drawing.Point(101, 241);
            this.rtb_msg.Name = "rtb_msg";
            this.rtb_msg.Size = new System.Drawing.Size(531, 304);
            this.rtb_msg.TabIndex = 5;
            this.rtb_msg.Text = "";
            // 
            // gb
            // 
            this.gb.Controls.Add(this.btn_connet);
            this.gb.Controls.Add(this.tbPort);
            this.gb.Controls.Add(this.cbbIP);
            this.gb.Controls.Add(this.label_Port);
            this.gb.Controls.Add(this.label_IP);
            this.gb.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.gb.Location = new System.Drawing.Point(101, 57);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(531, 165);
            this.gb.TabIndex = 4;
            this.gb.TabStop = false;
            this.gb.Text = "連線資訊";
            // 
            // btn_connet
            // 
            this.btn_connet.Location = new System.Drawing.Point(392, 48);
            this.btn_connet.Name = "btn_connet";
            this.btn_connet.Size = new System.Drawing.Size(100, 75);
            this.btn_connet.TabIndex = 4;
            this.btn_connet.Text = "啟動";
            this.btn_connet.UseVisualStyleBackColor = true;
            this.btn_connet.Click += new System.EventHandler(this.btn_connet_Click);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(154, 92);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(192, 43);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "8885";
            // 
            // cbbIP
            // 
            this.cbbIP.FormattingEnabled = true;
            this.cbbIP.Location = new System.Drawing.Point(153, 41);
            this.cbbIP.Name = "cbbIP";
            this.cbbIP.Size = new System.Drawing.Size(193, 38);
            this.cbbIP.TabIndex = 2;
            this.cbbIP.Text = "127.0.0.1";
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(29, 104);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(93, 30);
            this.label_Port.TabIndex = 1;
            this.label_Port.Text = "PORT:";
            // 
            // label_IP
            // 
            this.label_IP.AutoSize = true;
            this.label_IP.Location = new System.Drawing.Point(29, 48);
            this.label_IP.Name = "label_IP";
            this.label_IP.Size = new System.Drawing.Size(46, 30);
            this.label_IP.TabIndex = 0;
            this.label_IP.Text = "IP:";
            // 
            // Tetris_Battle_sever
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 603);
            this.Controls.Add(this.rtb_msg);
            this.Controls.Add(this.gb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Tetris_Battle_sever";
            this.Text = "Tetris_Battle_sever";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tetris_Battle_sever_FormClosing);
            this.Load += new System.EventHandler(this.Tetris_Battle_sever_Load);
            this.Shown += new System.EventHandler(this.Tetris_Battle_sever_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetris_Battle_sever_KeyDown);
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_msg;
        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.Button btn_connet;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.ComboBox cbbIP;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.Label label_IP;
    }
}