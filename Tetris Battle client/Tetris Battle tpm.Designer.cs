namespace Tetris_Battle_client
{
    partial class Tetris_Battle_tpm
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
            this.btn_createroom = new System.Windows.Forms.Button();
            this.btn_connet = new System.Windows.Forms.Button();
            this.btn_back = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_createroom
            // 
            this.btn_createroom.BackColor = System.Drawing.Color.Silver;
            this.btn_createroom.Font = new System.Drawing.Font("標楷體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_createroom.Location = new System.Drawing.Point(221, 124);
            this.btn_createroom.Name = "btn_createroom";
            this.btn_createroom.Size = new System.Drawing.Size(263, 59);
            this.btn_createroom.TabIndex = 0;
            this.btn_createroom.Text = "創建房間";
            this.btn_createroom.UseVisualStyleBackColor = false;
            this.btn_createroom.Click += new System.EventHandler(this.btn_createroom_Click);
            // 
            // btn_connet
            // 
            this.btn_connet.BackColor = System.Drawing.Color.Silver;
            this.btn_connet.Font = new System.Drawing.Font("標楷體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_connet.Location = new System.Drawing.Point(221, 246);
            this.btn_connet.Name = "btn_connet";
            this.btn_connet.Size = new System.Drawing.Size(263, 59);
            this.btn_connet.TabIndex = 1;
            this.btn_connet.Text = "加入房間";
            this.btn_connet.UseVisualStyleBackColor = false;
            this.btn_connet.Click += new System.EventHandler(this.btn_connet_Click);
            // 
            // btn_back
            // 
            this.btn_back.BackColor = System.Drawing.Color.Silver;
            this.btn_back.Font = new System.Drawing.Font("標楷體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_back.Location = new System.Drawing.Point(221, 383);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(263, 59);
            this.btn_back.TabIndex = 12;
            this.btn_back.Text = "返回";
            this.btn_back.UseVisualStyleBackColor = false;
            this.btn_back.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // Tetris_Battle_tpm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(732, 603);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.btn_connet);
            this.Controls.Add(this.btn_createroom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Tetris_Battle_tpm";
            this.Text = "Tetris_Battle_tpm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetris_Battle_tpm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_createroom;
        private System.Windows.Forms.Button btn_connet;
        private System.Windows.Forms.Button btn_back;
    }
}