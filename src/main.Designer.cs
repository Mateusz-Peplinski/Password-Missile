namespace PasswordCracker
{
    partial class main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.locateFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.hashTextBox = new System.Windows.Forms.TextBox();
            this.passwordFoundTextBox = new System.Windows.Forms.TextBox();
            this.selectedFileTextBox = new System.Windows.Forms.TextBox();
            this._launch = new System.Windows.Forms.PictureBox();
            this._abort = new System.Windows.Forms.PictureBox();
            this.progressConsole = new System.Windows.Forms.TextBox();
            this._launchOnLoad = new System.Windows.Forms.PictureBox();
            this._abortOnLoad = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusBox = new System.Windows.Forms.PictureBox();
            this.loadingGIF = new System.Windows.Forms.PictureBox();
            this.algorithComboBox = new PasswordCracker.customDropDown();
            ((System.ComponentModel.ISupportInitialize)(this._launch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._abort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._launchOnLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._abortOnLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingGIF)).BeginInit();
            this.SuspendLayout();
            // 
            // locateFile
            // 
            this.locateFile.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.locateFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.locateFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.locateFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.locateFile.ForeColor = System.Drawing.Color.Chartreuse;
            this.locateFile.Location = new System.Drawing.Point(266, 57);
            this.locateFile.Margin = new System.Windows.Forms.Padding(2);
            this.locateFile.Name = "locateFile";
            this.locateFile.Size = new System.Drawing.Size(102, 26);
            this.locateFile.TabIndex = 0;
            this.locateFile.Text = "Locate File";
            this.locateFile.UseVisualStyleBackColor = false;
            this.locateFile.Click += new System.EventHandler(this.locateFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // hashTextBox
            // 
            this.hashTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hashTextBox.BackColor = System.Drawing.Color.Black;
            this.hashTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hashTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hashTextBox.ForeColor = System.Drawing.Color.Chartreuse;
            this.hashTextBox.Location = new System.Drawing.Point(266, 16);
            this.hashTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.hashTextBox.Name = "hashTextBox";
            this.hashTextBox.Size = new System.Drawing.Size(379, 26);
            this.hashTextBox.TabIndex = 4;
            // 
            // passwordFoundTextBox
            // 
            this.passwordFoundTextBox.BackColor = System.Drawing.Color.Black;
            this.passwordFoundTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordFoundTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.passwordFoundTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordFoundTextBox.ForeColor = System.Drawing.Color.Chartreuse;
            this.passwordFoundTextBox.Location = new System.Drawing.Point(288, 276);
            this.passwordFoundTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.passwordFoundTextBox.Name = "passwordFoundTextBox";
            this.passwordFoundTextBox.ReadOnly = true;
            this.passwordFoundTextBox.Size = new System.Drawing.Size(228, 26);
            this.passwordFoundTextBox.TabIndex = 10;
            // 
            // selectedFileTextBox
            // 
            this.selectedFileTextBox.BackColor = System.Drawing.Color.Black;
            this.selectedFileTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectedFileTextBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.selectedFileTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedFileTextBox.ForeColor = System.Drawing.Color.Chartreuse;
            this.selectedFileTextBox.Location = new System.Drawing.Point(372, 55);
            this.selectedFileTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.selectedFileTextBox.Name = "selectedFileTextBox";
            this.selectedFileTextBox.ReadOnly = true;
            this.selectedFileTextBox.Size = new System.Drawing.Size(272, 26);
            this.selectedFileTextBox.TabIndex = 11;
            // 
            // _launch
            // 
            this._launch.BackColor = System.Drawing.Color.Transparent;
            this._launch.Cursor = System.Windows.Forms.Cursors.Hand;
            this._launch.Image = global::PasswordCracker.Properties.Resources._launchButton;
            this._launch.Location = new System.Drawing.Point(329, 169);
            this._launch.Name = "_launch";
            this._launch.Size = new System.Drawing.Size(129, 46);
            this._launch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._launch.TabIndex = 13;
            this._launch.TabStop = false;
            this._launch.Click += new System.EventHandler(this._launch_Click);
            this._launch.MouseLeave += new System.EventHandler(this._launch_MouseLeave);
            // 
            // _abort
            // 
            this._abort.BackColor = System.Drawing.Color.Transparent;
            this._abort.Cursor = System.Windows.Forms.Cursors.Hand;
            this._abort.Image = global::PasswordCracker.Properties.Resources._abortButton;
            this._abort.Location = new System.Drawing.Point(495, 169);
            this._abort.Name = "_abort";
            this._abort.Size = new System.Drawing.Size(129, 42);
            this._abort.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._abort.TabIndex = 14;
            this._abort.TabStop = false;
            this._abort.Click += new System.EventHandler(this._abort_Click);
            this._abort.MouseLeave += new System.EventHandler(this._abort_MouseLeave);
            // 
            // progressConsole
            // 
            this.progressConsole.BackColor = System.Drawing.Color.Black;
            this.progressConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.progressConsole.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.progressConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressConsole.ForeColor = System.Drawing.Color.Chartreuse;
            this.progressConsole.Location = new System.Drawing.Point(8, 349);
            this.progressConsole.Margin = new System.Windows.Forms.Padding(2);
            this.progressConsole.Multiline = true;
            this.progressConsole.Name = "progressConsole";
            this.progressConsole.ReadOnly = true;
            this.progressConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.progressConsole.Size = new System.Drawing.Size(645, 89);
            this.progressConsole.TabIndex = 15;
            // 
            // _launchOnLoad
            // 
            this._launchOnLoad.BackColor = System.Drawing.Color.Transparent;
            this._launchOnLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this._launchOnLoad.Image = global::PasswordCracker.Properties.Resources._launchButton;
            this._launchOnLoad.Location = new System.Drawing.Point(335, 169);
            this._launchOnLoad.Name = "_launchOnLoad";
            this._launchOnLoad.Size = new System.Drawing.Size(117, 42);
            this._launchOnLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._launchOnLoad.TabIndex = 16;
            this._launchOnLoad.TabStop = false;
            this._launchOnLoad.MouseEnter += new System.EventHandler(this._launchOnLoad_MouseEnter);
            // 
            // _abortOnLoad
            // 
            this._abortOnLoad.BackColor = System.Drawing.Color.Transparent;
            this._abortOnLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this._abortOnLoad.Image = global::PasswordCracker.Properties.Resources._abortButton;
            this._abortOnLoad.Location = new System.Drawing.Point(499, 169);
            this._abortOnLoad.Name = "_abortOnLoad";
            this._abortOnLoad.Size = new System.Drawing.Size(121, 42);
            this._abortOnLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._abortOnLoad.TabIndex = 17;
            this._abortOnLoad.TabStop = false;
            this._abortOnLoad.MouseEnter += new System.EventHandler(this._abortOnLoad_MouseEnter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PasswordCracker.Properties.Resources.logoWithName;
            this.pictureBox1.Location = new System.Drawing.Point(43, 138);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(201, 113);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // statusBox
            // 
            this.statusBox.BackColor = System.Drawing.Color.Transparent;
            this.statusBox.Image = global::PasswordCracker.Properties.Resources.errorStatus;
            this.statusBox.Location = new System.Drawing.Point(146, 307);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(98, 24);
            this.statusBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.statusBox.TabIndex = 19;
            this.statusBox.TabStop = false;
            // 
            // loadingGIF
            // 
            this.loadingGIF.BackColor = System.Drawing.Color.Transparent;
            this.loadingGIF.Image = global::PasswordCracker.Properties.Resources.loadingLoop;
            this.loadingGIF.Location = new System.Drawing.Point(430, 221);
            this.loadingGIF.Margin = new System.Windows.Forms.Padding(2);
            this.loadingGIF.Name = "loadingGIF";
            this.loadingGIF.Size = new System.Drawing.Size(94, 41);
            this.loadingGIF.TabIndex = 21;
            this.loadingGIF.TabStop = false;
            // 
            // algorithComboBox
            // 
            this.algorithComboBox.BackColor = System.Drawing.Color.Black;
            this.algorithComboBox.BorderColor = System.Drawing.Color.LawnGreen;
            this.algorithComboBox.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.algorithComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.algorithComboBox.ForeColor = System.Drawing.Color.LawnGreen;
            this.algorithComboBox.FormattingEnabled = true;
            this.algorithComboBox.Items.AddRange(new object[] {
            "MD5",
            "SHA1",
            "SHA256",
            "NTLM"});
            this.algorithComboBox.Location = new System.Drawing.Point(266, 95);
            this.algorithComboBox.Name = "algorithComboBox";
            this.algorithComboBox.Size = new System.Drawing.Size(121, 21);
            this.algorithComboBox.TabIndex = 20;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::PasswordCracker.Properties.Resources.main;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(662, 449);
            this.Controls.Add(this.loadingGIF);
            this.Controls.Add(this.algorithComboBox);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressConsole);
            this.Controls.Add(this._abort);
            this.Controls.Add(this._launch);
            this.Controls.Add(this.selectedFileTextBox);
            this.Controls.Add(this.passwordFoundTextBox);
            this.Controls.Add(this.hashTextBox);
            this.Controls.Add(this.locateFile);
            this.Controls.Add(this._launchOnLoad);
            this.Controls.Add(this._abortOnLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Password Missile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.main_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this._launch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._abort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._launchOnLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._abortOnLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loadingGIF)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button locateFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox hashTextBox;
        private System.Windows.Forms.TextBox passwordFoundTextBox;
        private System.Windows.Forms.TextBox selectedFileTextBox;
        private System.Windows.Forms.PictureBox _launch;
        private System.Windows.Forms.PictureBox _abort;
        private System.Windows.Forms.TextBox progressConsole;
        private System.Windows.Forms.PictureBox _launchOnLoad;
        private System.Windows.Forms.PictureBox _abortOnLoad;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox statusBox;
        private customDropDown algorithComboBox;
        private System.Windows.Forms.PictureBox loadingGIF;
    }
}

