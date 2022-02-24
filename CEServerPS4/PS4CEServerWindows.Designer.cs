namespace CEServerPS4
{
    partial class PS4CEServerWindows
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.ConnectBtn = new System.Windows.Forms.Button();
            this.CustomCEToggle = new CEServerPS4.RJToggleButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(91, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "CustonCheatEngine";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.label2.Location = new System.Drawing.Point(12, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(423, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Custome Cheat Engine Means it is build additional support for Ps4 Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkGreen;
            this.label3.Location = new System.Drawing.Point(37, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "PS4 IP";
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(95, 30);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(157, 20);
            this.IPTextBox.TabIndex = 5;
            // 
            // ConnectBtn
            // 
            this.ConnectBtn.ForeColor = System.Drawing.Color.Indigo;
            this.ConnectBtn.Location = new System.Drawing.Point(278, 31);
            this.ConnectBtn.Name = "ConnectBtn";
            this.ConnectBtn.Size = new System.Drawing.Size(146, 23);
            this.ConnectBtn.TabIndex = 6;
            this.ConnectBtn.Text = "Connect";
            this.ConnectBtn.UseVisualStyleBackColor = true;
            this.ConnectBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // CustomCEToggle
            // 
            this.CustomCEToggle.AutoSize = true;
            this.CustomCEToggle.Location = new System.Drawing.Point(40, 61);
            this.CustomCEToggle.MinimumSize = new System.Drawing.Size(45, 22);
            this.CustomCEToggle.Name = "CustomCEToggle";
            this.CustomCEToggle.OffBackColor = System.Drawing.Color.Gray;
            this.CustomCEToggle.OffToggleColor = System.Drawing.Color.Gainsboro;
            this.CustomCEToggle.OnBackColor = System.Drawing.Color.MediumSlateBlue;
            this.CustomCEToggle.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.CustomCEToggle.Size = new System.Drawing.Size(45, 22);
            this.CustomCEToggle.TabIndex = 1;
            this.CustomCEToggle.UseVisualStyleBackColor = true;
            this.CustomCEToggle.CheckedChanged += new System.EventHandler(this.CustomCEToggle_CheckedChanged);
            // 
            // PS4CEServerWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 166);
            this.Controls.Add(this.ConnectBtn);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CustomCEToggle);
            this.MaximizeBox = false;
            this.Name = "PS4CEServerWindows";
            this.Text = "PS4CEServerWindows";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RJToggleButton CustomCEToggle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Button ConnectBtn;
    }
}