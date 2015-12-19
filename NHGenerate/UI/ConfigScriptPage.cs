using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using NHGenerate.Core;
using NHGenerate.Properties;

namespace NHGenerate.UI
{
    public class ConfigScriptPage : UserControl
    {
        private ConfigScriptProperties _properties = new ConfigScriptProperties();
        private Label _lblDefaultConnection;
        private Button _btnNew;
        private TextBox _txtTemplate;
        private Label _lblTemplate;
        private ComboBox _cmbDefaultConnection;
        private Label _lblSchema;
        private TextBox _txtSchema;
        private Label _lblTableName;
        private TextBox _txtTableName;
        private Label _lblUserName;
        private TextBox _txtUserName;
        private List<DbConnection> _connections = new List<DbConnection>();

        public ConfigScriptPage()
        {
            InitializeComponent();
        }

        private void ConfigScriptPageLoad(object sender, System.EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            _cmbDefaultConnection.Items.Clear();

            StringCollection list = Settings.Default.Connections;

            if (list != null)
            {
                foreach (string val in list)
                {
                    if (!string.IsNullOrEmpty(val))
                    {
                        var conn = new DbConnection(val);
                        if (!_cmbDefaultConnection.Items.Contains(conn.DefaultName))
                        {
                            _connections.Add(conn);
                            _cmbDefaultConnection.Items.Add(conn.DefaultName);
                        }
                    }
                }
            }

            if (_cmbDefaultConnection.Items.Contains(_properties.DefaultScriptDatabase))
            {
                int index = _cmbDefaultConnection.Items.IndexOf(_properties.DefaultScriptDatabase);
                _cmbDefaultConnection.SelectedIndex = index;
            }

            _txtUserName.Text = _properties.UpdateScriptUserName;
            _txtTemplate.Text = _properties.UpdateScriptTemplate;
            _txtSchema.Text = _properties.UpdateScriptSchemaName;
            _txtTableName.Text = _properties.UpdateScriptTableName;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(_txtUserName.Text))
            {
                MessageBox.Show("User name is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _properties.UpdateScriptUserName = _txtUserName.Text;
            _properties.UpdateScriptTemplate = _txtTemplate.Text;

            if (_cmbDefaultConnection.SelectedIndex != -1)
            {
                _properties.DefaultScriptDatabase = _cmbDefaultConnection.Items[_cmbDefaultConnection.SelectedIndex].ToString();
            }

            _properties.UpdateScriptSchemaName = _txtSchema.Text;
            _properties.UpdateScriptTableName = _txtTableName.Text;

            return true;
        }

        private void NewClick(object sender, System.EventArgs e)
        {
            var win = new ConnectionWindow();
            win.ShowDialog();

            var connection = win.Connection;
            if (connection != null)
            {
                _properties.DefaultScriptDatabase = connection.DefaultName;
                LoadData();
            }
        }

        #region priv

        private void InitializeComponent()
        {
            this._lblDefaultConnection = new System.Windows.Forms.Label();
            this._btnNew = new System.Windows.Forms.Button();
            this._cmbDefaultConnection = new System.Windows.Forms.ComboBox();
            this._txtTemplate = new System.Windows.Forms.TextBox();
            this._lblTemplate = new System.Windows.Forms.Label();
            this._lblSchema = new System.Windows.Forms.Label();
            this._txtSchema = new System.Windows.Forms.TextBox();
            this._lblTableName = new System.Windows.Forms.Label();
            this._txtTableName = new System.Windows.Forms.TextBox();
            this._lblUserName = new System.Windows.Forms.Label();
            this._txtUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _lblDefaultConnection
            // 
            this._lblDefaultConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblDefaultConnection.AutoSize = true;
            this._lblDefaultConnection.Location = new System.Drawing.Point(13, 16);
            this._lblDefaultConnection.Name = "_lblDefaultConnection";
            this._lblDefaultConnection.Size = new System.Drawing.Size(100, 13);
            this._lblDefaultConnection.TabIndex = 1;
            this._lblDefaultConnection.Text = "Default connection:";
            // 
            // _btnNew
            // 
            this._btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnNew.Location = new System.Drawing.Point(315, 30);
            this._btnNew.Name = "_btnNew";
            this._btnNew.Size = new System.Drawing.Size(63, 23);
            this._btnNew.TabIndex = 6;
            this._btnNew.Text = "New";
            this._btnNew.UseVisualStyleBackColor = true;
            this._btnNew.Click += new System.EventHandler(this.NewClick);
            // 
            // _cmbDefaultConnection
            // 
            this._cmbDefaultConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbDefaultConnection.FormattingEnabled = true;
            this._cmbDefaultConnection.Location = new System.Drawing.Point(16, 32);
            this._cmbDefaultConnection.Name = "_cmbDefaultConnection";
            this._cmbDefaultConnection.Size = new System.Drawing.Size(293, 21);
            this._cmbDefaultConnection.TabIndex = 7;
            // 
            // _txtTemplate
            // 
            this._txtTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtTemplate.Location = new System.Drawing.Point(16, 121);
            this._txtTemplate.Name = "_txtTemplate";
            this._txtTemplate.Size = new System.Drawing.Size(362, 20);
            this._txtTemplate.TabIndex = 8;
            // 
            // _lblTemplate
            // 
            this._lblTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblTemplate.AutoSize = true;
            this._lblTemplate.Location = new System.Drawing.Point(13, 105);
            this._lblTemplate.Name = "_lblTemplate";
            this._lblTemplate.Size = new System.Drawing.Size(54, 13);
            this._lblTemplate.TabIndex = 9;
            this._lblTemplate.Text = "Template:";
            // 
            // _lblSchema
            // 
            this._lblSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblSchema.AutoSize = true;
            this._lblSchema.Location = new System.Drawing.Point(13, 149);
            this._lblSchema.Name = "_lblSchema";
            this._lblSchema.Size = new System.Drawing.Size(49, 13);
            this._lblSchema.TabIndex = 11;
            this._lblSchema.Text = "Schema:";
            // 
            // _txtSchema
            // 
            this._txtSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtSchema.Location = new System.Drawing.Point(16, 165);
            this._txtSchema.Name = "_txtSchema";
            this._txtSchema.Size = new System.Drawing.Size(362, 20);
            this._txtSchema.TabIndex = 10;
            // 
            // _lblTableName
            // 
            this._lblTableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblTableName.AutoSize = true;
            this._lblTableName.Location = new System.Drawing.Point(13, 193);
            this._lblTableName.Name = "_lblTableName";
            this._lblTableName.Size = new System.Drawing.Size(66, 13);
            this._lblTableName.TabIndex = 13;
            this._lblTableName.Text = "Table name:";
            // 
            // _txtTableName
            // 
            this._txtTableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtTableName.Location = new System.Drawing.Point(16, 209);
            this._txtTableName.Name = "_txtTableName";
            this._txtTableName.Size = new System.Drawing.Size(362, 20);
            this._txtTableName.TabIndex = 12;
            // 
            // _lblUserName
            // 
            this._lblUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblUserName.AutoSize = true;
            this._lblUserName.Location = new System.Drawing.Point(13, 61);
            this._lblUserName.Name = "_lblUserName";
            this._lblUserName.Size = new System.Drawing.Size(61, 13);
            this._lblUserName.TabIndex = 15;
            this._lblUserName.Text = "User name:";
            // 
            // _txtUserName
            // 
            this._txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtUserName.Location = new System.Drawing.Point(16, 77);
            this._txtUserName.Name = "_txtUserName";
            this._txtUserName.Size = new System.Drawing.Size(362, 20);
            this._txtUserName.TabIndex = 14;
            // 
            // ConfigScriptPage
            // 
            this.Controls.Add(this._lblUserName);
            this.Controls.Add(this._txtUserName);
            this.Controls.Add(this._lblTableName);
            this.Controls.Add(this._txtTableName);
            this.Controls.Add(this._lblSchema);
            this.Controls.Add(this._txtSchema);
            this.Controls.Add(this._lblTemplate);
            this.Controls.Add(this._txtTemplate);
            this.Controls.Add(this._cmbDefaultConnection);
            this.Controls.Add(this._btnNew);
            this.Controls.Add(this._lblDefaultConnection);
            this.Name = "ConfigScriptPage";
            this.Size = new System.Drawing.Size(394, 280);
            this.Load += new System.EventHandler(this.ConfigScriptPageLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
    }
}
