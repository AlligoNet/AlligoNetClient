namespace AlligoClient
{
    partial class Form1
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.userName_txt = new System.Windows.Forms.TextBox();
            this.password_txt = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.serverPickerCB = new System.Windows.Forms.ComboBox();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.updateBtn = new System.Windows.Forms.Button();
            this.currVersionLbl = new System.Windows.Forms.Label();
            this.updateLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnConnect.Location = new System.Drawing.Point(436, 405);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(295, 45);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // userName_txt
            // 
            this.userName_txt.BackColor = System.Drawing.SystemColors.ControlLight;
            this.userName_txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userName_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userName_txt.ForeColor = System.Drawing.Color.White;
            this.userName_txt.Location = new System.Drawing.Point(436, 60);
            this.userName_txt.Name = "userName_txt";
            this.userName_txt.Size = new System.Drawing.Size(295, 35);
            this.userName_txt.TabIndex = 1;
            // 
            // password_txt
            // 
            this.password_txt.BackColor = System.Drawing.SystemColors.ControlLight;
            this.password_txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.password_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password_txt.ForeColor = System.Drawing.Color.White;
            this.password_txt.Location = new System.Drawing.Point(436, 123);
            this.password_txt.Name = "password_txt";
            this.password_txt.Size = new System.Drawing.Size(295, 35);
            this.password_txt.TabIndex = 2;
            this.password_txt.UseSystemPasswordChar = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnDisconnect.Location = new System.Drawing.Point(436, 405);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(295, 45);
            this.btnDisconnect.TabIndex = 5;
            this.btnDisconnect.TabStop = false;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // serverPickerCB
            // 
            this.serverPickerCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverPickerCB.FormattingEnabled = true;
            this.serverPickerCB.Items.AddRange(new object[] {
            "Quentin (EU)",
            "Sinon (US West)",
            "Eugeo (US East)",
            "Insignia (Oceanic)"});
            this.serverPickerCB.Location = new System.Drawing.Point(436, 185);
            this.serverPickerCB.Name = "serverPickerCB";
            this.serverPickerCB.Size = new System.Drawing.Size(295, 21);
            this.serverPickerCB.TabIndex = 3;
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.rememberCheckBox.ForeColor = System.Drawing.Color.White;
            this.rememberCheckBox.Location = new System.Drawing.Point(436, 222);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(95, 17);
            this.rememberCheckBox.TabIndex = 4;
            this.rememberCheckBox.Text = "Remember Me";
            this.rememberCheckBox.UseVisualStyleBackColor = false;
            // 
            // updateBtn
            // 
            this.updateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateBtn.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.updateBtn.Location = new System.Drawing.Point(43, 405);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(150, 45);
            this.updateBtn.TabIndex = 6;
            this.updateBtn.Text = "Update";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // currVersionLbl
            // 
            this.currVersionLbl.AutoSize = true;
            this.currVersionLbl.BackColor = System.Drawing.Color.Transparent;
            this.currVersionLbl.Location = new System.Drawing.Point(13, 13);
            this.currVersionLbl.Name = "currVersionLbl";
            this.currVersionLbl.Size = new System.Drawing.Size(45, 13);
            this.currVersionLbl.TabIndex = 7;
            this.currVersionLbl.Text = "Version:";
            // 
            // updateLbl
            // 
            this.updateLbl.AutoSize = true;
            this.updateLbl.BackColor = System.Drawing.Color.Transparent;
            this.updateLbl.Location = new System.Drawing.Point(13, 30);
            this.updateLbl.Name = "updateLbl";
            this.updateLbl.Size = new System.Drawing.Size(17, 13);
            this.updateLbl.TabIndex = 8;
            this.updateLbl.Text = "nil";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AlligoClient.Properties.Resources.bg;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.updateLbl);
            this.Controls.Add(this.currVersionLbl);
            this.Controls.Add(this.updateBtn);
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.serverPickerCB);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.password_txt);
            this.Controls.Add(this.userName_txt);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alligo Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Closed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox userName_txt;
        private System.Windows.Forms.TextBox password_txt;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.ComboBox serverPickerCB;
        private System.Windows.Forms.CheckBox rememberCheckBox;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.Label currVersionLbl;
        private System.Windows.Forms.Label updateLbl;
    }
}

