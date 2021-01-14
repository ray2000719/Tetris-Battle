namespace Tetris_Battle_client
{
    partial class Tetris_Battle_spc
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // flp_cubenext
            // 
            this.flp_cubenext.Location = new System.Drawing.Point(454, 40);
            this.flp_cubenext.Name = "flp_cubenext";
            this.flp_cubenext.Size = new System.Drawing.Size(100, 350);
            this.flp_cubenext.TabIndex = 4;
            // 
            // rtb_gameScreen
            // 
            this.rtb_gameScreen.Enabled = false;
            this.rtb_gameScreen.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.rtb_gameScreen.Location = new System.Drawing.Point(173, 40);
            this.rtb_gameScreen.Name = "rtb_gameScreen";
            this.rtb_gameScreen.ShowSelectionMargin = true;
            this.rtb_gameScreen.Size = new System.Drawing.Size(275, 525);
            this.rtb_gameScreen.TabIndex = 3;
            this.rtb_gameScreen.Text = "";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Tetris_Battle_spc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 603);
            this.Controls.Add(this.flp_cubenext);
            this.Controls.Add(this.rtb_gameScreen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Tetris_Battle_spc";
            this.Text = "Tetris_Battle_spc";
            this.Load += new System.EventHandler(this.Tetris_Battle_spc_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Tetris_Battle_spc_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flp_cubenext;
        private System.Windows.Forms.RichTextBox rtb_gameScreen;
        private System.Windows.Forms.Timer timer1;
    }
}