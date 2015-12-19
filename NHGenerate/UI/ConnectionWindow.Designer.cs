namespace NHGenerate.UI
{
    partial class ConnectionWindow
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
            this.lblServerName = new System.Windows.Forms.Label();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.gbLogOn = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.rbAuthSql = new System.Windows.Forms.RadioButton();
            this.rbAuthWin = new System.Windows.Forms.RadioButton();
            this.lblDefaultName = new System.Windows.Forms.Label();
            this.gbConnectToDatabase = new System.Windows.Forms.GroupBox();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbDefaultName = new System.Windows.Forms.ComboBox();
            this.gbLogOn.SuspendLayout();
            this.gbConnectToDatabase.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(9, 49);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(70, 13);
            this.lblServerName.TabIndex = 0;
            this.lblServerName.Text = "Server name:";
            // 
            // cmbServer
            // 
            this.cmbServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(12, 65);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(370, 21);
            this.cmbServer.TabIndex = 1;
            this.cmbServer.SelectedIndexChanged += new System.EventHandler(this.ServerUpdate);
            this.cmbServer.TextUpdate += new System.EventHandler(this.ServerUpdate);
            // 
            // gbLogOn
            // 
            this.gbLogOn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLogOn.Controls.Add(this.txtPassword);
            this.gbLogOn.Controls.Add(this.txtUserName);
            this.gbLogOn.Controls.Add(this.lblPassword);
            this.gbLogOn.Controls.Add(this.lblUserName);
            this.gbLogOn.Controls.Add(this.rbAuthSql);
            this.gbLogOn.Controls.Add(this.rbAuthWin);
            this.gbLogOn.Location = new System.Drawing.Point(12, 91);
            this.gbLogOn.Name = "gbLogOn";
            this.gbLogOn.Size = new System.Drawing.Size(370, 138);
            this.gbLogOn.TabIndex = 2;
            this.gbLogOn.TabStop = false;
            this.gbLogOn.Text = "Log on to the server";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(103, 100);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.CredentialUpdate);
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(103, 74);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(250, 20);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.TextChanged += new System.EventHandler(this.CredentialUpdate);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(41, 103);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(36, 77);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(61, 13);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "User name:";
            // 
            // rbAuthSql
            // 
            this.rbAuthSql.AutoSize = true;
            this.rbAuthSql.Location = new System.Drawing.Point(17, 48);
            this.rbAuthSql.Name = "rbAuthSql";
            this.rbAuthSql.Size = new System.Drawing.Size(173, 17);
            this.rbAuthSql.TabIndex = 1;
            this.rbAuthSql.Text = "Use SQL Server Authentication";
            this.rbAuthSql.UseVisualStyleBackColor = true;
            // 
            // rbAuthWin
            // 
            this.rbAuthWin.AutoSize = true;
            this.rbAuthWin.Checked = true;
            this.rbAuthWin.Location = new System.Drawing.Point(17, 25);
            this.rbAuthWin.Name = "rbAuthWin";
            this.rbAuthWin.Size = new System.Drawing.Size(162, 17);
            this.rbAuthWin.TabIndex = 0;
            this.rbAuthWin.TabStop = true;
            this.rbAuthWin.Text = "Use Windows Authentication";
            this.rbAuthWin.UseVisualStyleBackColor = true;
            this.rbAuthWin.CheckedChanged += new System.EventHandler(this.AuthenticationTypeChanged);
            // 
            // lblDefaultName
            // 
            this.lblDefaultName.AutoSize = true;
            this.lblDefaultName.Location = new System.Drawing.Point(9, 9);
            this.lblDefaultName.Name = "lblDefaultName";
            this.lblDefaultName.Size = new System.Drawing.Size(64, 13);
            this.lblDefaultName.TabIndex = 3;
            this.lblDefaultName.Text = "Connection:";
            // 
            // gbConnectToDatabase
            // 
            this.gbConnectToDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConnectToDatabase.Controls.Add(this.cmbDatabase);
            this.gbConnectToDatabase.Location = new System.Drawing.Point(12, 235);
            this.gbConnectToDatabase.Name = "gbConnectToDatabase";
            this.gbConnectToDatabase.Size = new System.Drawing.Size(370, 74);
            this.gbConnectToDatabase.TabIndex = 3;
            this.gbConnectToDatabase.TabStop = false;
            this.gbConnectToDatabase.Text = "Connect to a database";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.Enabled = false;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(17, 27);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(336, 21);
            this.cmbDatabase.TabIndex = 0;
            this.cmbDatabase.DropDown += new System.EventHandler(this.CmbDatabaseDropDown);
            this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.DatabaseUpdate);
            this.cmbDatabase.TextUpdate += new System.EventHandler(this.DatabaseUpdate);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Enabled = false;
            this.btnTestConnection.Location = new System.Drawing.Point(12, 315);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(111, 23);
            this.btnTestConnection.TabIndex = 6;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.BtnTestConnectionClick);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 315);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(226, 315);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // cmbDefaultName
            // 
            this.cmbDefaultName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDefaultName.FormattingEnabled = true;
            this.cmbDefaultName.Location = new System.Drawing.Point(12, 25);
            this.cmbDefaultName.Name = "cmbDefaultName";
            this.cmbDefaultName.Size = new System.Drawing.Size(370, 21);
            this.cmbDefaultName.TabIndex = 7;
            this.cmbDefaultName.SelectedIndexChanged += new System.EventHandler(this.DefaultNameChanged);
            this.cmbDefaultName.TextUpdate += new System.EventHandler(this.DefaultNameChanged);
            // 
            // ConnectionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(394, 350);
            this.Controls.Add(this.cmbDefaultName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.gbConnectToDatabase);
            this.Controls.Add(this.lblDefaultName);
            this.Controls.Add(this.gbLogOn);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.lblServerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConnectionWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect to database";
            this.Load += new System.EventHandler(this.ConnectionWindowLoad);
            this.gbLogOn.ResumeLayout(false);
            this.gbLogOn.PerformLayout();
            this.gbConnectToDatabase.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServerName;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.GroupBox gbLogOn;
        private System.Windows.Forms.RadioButton rbAuthSql;
        private System.Windows.Forms.RadioButton rbAuthWin;
        private System.Windows.Forms.Label lblDefaultName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.GroupBox gbConnectToDatabase;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbDefaultName;
    }
}