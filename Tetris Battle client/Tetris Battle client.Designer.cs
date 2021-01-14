namespace Tetris_Battle_client
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_tpm = new System.Windows.Forms.Button();
            this.btn_spm = new System.Windows.Forms.Button();
            this.label_title = new System.Windows.Forms.Label();
            this.btn_setting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.Silver;
            this.btn_exit.Font = new System.Drawing.Font("標楷體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_exit.Location = new System.Drawing.Point(268, 358);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(263, 59);
            this.btn_exit.TabIndex = 11;
            this.btn_exit.Text = "退出";
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btn_tpm
            // 
            this.btn_tpm.BackColor = System.Drawing.Color.Silver;
            this.btn_tpm.Font = new System.Drawing.Font("標楷體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_tpm.Location = new System.Drawing.Point(268, 204);
            this.btn_tpm.Name = "btn_tpm";
            this.btn_tpm.Size = new System.Drawing.Size(263, 59);
            this.btn_tpm.TabIndex = 10;
            this.btn_tpm.Text = "雙人模式";
            this.btn_tpm.UseVisualStyleBackColor = false;
            this.btn_tpm.Click += new System.EventHandler(this.btn_tpm_Click);
            // 
            // btn_spm
            // 
            this.btn_spm.BackColor = System.Drawing.Color.Silver;
            this.btn_spm.Font = new System.Drawing.Font("標楷體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_spm.Location = new System.Drawing.Point(268, 128);
            this.btn_spm.Name = "btn_spm";
            this.btn_spm.Size = new System.Drawing.Size(263, 59);
            this.btn_spm.TabIndex = 9;
            this.btn_spm.Text = "單人模式";
            this.btn_spm.UseVisualStyleBackColor = false;
            this.btn_spm.Click += new System.EventHandler(this.btn_spm_Click);
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.BackColor = System.Drawing.Color.Silver;
            this.label_title.Font = new System.Drawing.Font("Ruach LET", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_title.ForeColor = System.Drawing.Color.Blue;
            this.label_title.Location = new System.Drawing.Point(227, 24);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(347, 77);
            this.label_title.TabIndex = 8;
            this.label_title.Text = "Tetris Battle";
            this.label_title.Click += new System.EventHandler(this.label_title_Click);
            // 
            // btn_setting
            // 
            this.btn_setting.BackColor = System.Drawing.Color.Silver;
            this.btn_setting.Font = new System.Drawing.Font("標楷體", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_setting.Location = new System.Drawing.Point(268, 282);
            this.btn_setting.Name = "btn_setting";
            this.btn_setting.Size = new System.Drawing.Size(263, 59);
            this.btn_setting.TabIndex = 12;
            this.btn_setting.Text = "設置";
            this.btn_setting.UseVisualStyleBackColor = false;
            this.btn_setting.Click += new System.EventHandler(this.btn_setting_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 453);
            this.Controls.Add(this.btn_setting);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_tpm);
            this.Controls.Add(this.btn_spm);
            this.Controls.Add(this.label_title);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Tetris Battle client";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_tpm;
        private System.Windows.Forms.Button btn_spm;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Button btn_setting;
    }
}

