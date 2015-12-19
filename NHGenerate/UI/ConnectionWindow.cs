using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using NHGenerate.Core;
using NHGenerate.Properties;

namespace NHGenerate.UI
{
    public partial class ConnectionWindow : Form
    {
        const string Pattern = @"(?<table>^\w+)(\s\((?<schema>\w+)\))*";
        private bool _eventEnabled = false;
        private List<DbConnection> _connections = new List<DbConnection>();

        public ConnectionWindow(string defaultName)
        {
            InitializeComponent();
            cmbDefaultName.Text = defaultName;
        }

        public ConnectionWindow()
        {
            InitializeComponent();
        }

        public DbConnection Connection { get; set; }

        private void ConnectionWindowLoad(object sender, System.EventArgs e)
        {
            StringCollection list = Settings.Default.Connections;

            if (list != null)
            {
                foreach (string val in list)
                {
                    if (!string.IsNullOrEmpty(val))
                    {
                        var conn = new DbConnection(val);
                        if (!cmbDefaultName.Items.Contains(conn.DefaultName))
                        {
                            _connections.Add(conn);
                            cmbDefaultName.Items.Add(conn.DefaultName);
                        }
                    }
                }
            }

            StringCollection servers = Settings.Default.Servers;
            if (servers != null)
            {
                foreach (string server in servers)
                {
                    if (!string.IsNullOrEmpty(server))
                    {
                        cmbServer.Items.Add(server);
                    }
                }
            }

            DefaultNameChanged(cmbDefaultName, null);
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbDefaultName.Text))
            {
                MessageBox.Show("Default name is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool connected = TestConnection();

            if (connected)
            {
                Connection = new DbConnection
                {
                    DefaultName = cmbDefaultName.Text,
                    Server = cmbServer.Text,
                    WindowsAuthentication = rbAuthWin.Checked,
                    UserName = txtUserName.Text,
                    Password = txtPassword.Text,
                    Database = cmbDatabase.Text
                };

                var connections = _connections.Where(c => c.DefaultName == Connection.DefaultName).ToList();
                foreach (DbConnection dbConnection in connections)
                {
                    _connections.Remove(dbConnection);
                }
                _connections.Add(Connection);

                if (!Settings.Default.Servers.Contains(cmbServer.Text))
                {
                    Settings.Default.Servers.Add(cmbServer.Text);
                }

                if (Settings.Default.Connections != null)
                {
                    Settings.Default.Connections.Clear();
                }
                foreach (DbConnection c in _connections)
                {
                    if (!Settings.Default.Connections.Contains(c.Encoded))
                    {
                        Settings.Default.Connections.Add(c.Encoded);
                    }
                }

                Settings.Default.Save();

                Close();
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnTestConnectionClick(object sender, EventArgs e)
        {
            bool connected = TestConnection();
            if (connected)
            {
                MessageBox.Show("Connected!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SetVisibility()
        {
            txtPassword.Enabled = txtUserName.Enabled = (rbAuthSql.Checked);
            cmbDatabase.Enabled = !string.IsNullOrEmpty(cmbServer.Text) && (rbAuthWin.Checked || (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text)));
            btnTestConnection.Enabled = btnOK.Enabled = !string.IsNullOrEmpty(cmbDatabase.Text);
        }

        private void AuthenticationTypeChanged(object sender, EventArgs e)
        {
            SetVisibility();
            //txtPassword.Enabled = txtUserName.Enabled = rbAuthSql.Checked;
            if (!rbAuthSql.Checked)
            {
                txtPassword.Text = string.Empty;
                txtUserName.Text = string.Empty;
            }
            _databases = null;
            cmbDatabase.Text = string.Empty;
        }

        private void CmbDatabaseDropDown(object sender, EventArgs e)
        {
            cmbDatabase.Items.Clear();
            foreach (string d in Databases)
            {
                cmbDatabase.Items.Add(d);
            }
        }

        private bool TestConnection()
        {
            bool connected = false;

            try
            {
                string connectionString = rbAuthWin.Checked ?
                    string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Connection Timeout=3", cmbServer.Text, cmbDatabase.Text) :
                    string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Connection Timeout=3", cmbServer.Text, cmbDatabase.Text, txtUserName.Text, txtPassword.Text);

                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    conn.Close();
                    connected = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return connected;
        }

        private List<string> _databases;
        private List<string> Databases
        {
            get
            {
                if (_databases != null) return _databases;

                _databases = new List<string>();
                try
                {
                    string connectionString = rbAuthWin.Checked ?
                        string.Format("Data Source={0};Integrated Security=True;Connection Timeout=3", cmbServer.Text) :
                        string.Format("Data Source={0};User Id={1};Password={2};Connection Timeout=3", cmbServer.Text, txtUserName.Text, txtPassword.Text);

                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        DataTable dt = conn.GetSchema("Databases");
                        conn.Close();
                        _databases.AddRange(from DataRow r in dt.Rows select r["database_name"].ToString());

                        _databases.Sort();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return _databases;
            }
        }

        private void DefaultNameChanged(object sender, EventArgs e)
        {
            var connection = _connections.SingleOrDefault(c => c.DefaultName == cmbDefaultName.Text);
            if (connection != null)
            {
                cmbServer.Text = connection.Server;
                rbAuthWin.Checked = connection.WindowsAuthentication;
                rbAuthSql.Checked = !connection.WindowsAuthentication;
                txtUserName.Text = connection.UserName;
                txtPassword.Text = connection.Password;
                cmbDatabase.Text = connection.Database;
                SetVisibility();
            }
        }

        private void ServerUpdate(object sender, EventArgs e)
        {
            SetVisibility();
            _databases = null;
            cmbDatabase.Text = string.Empty;
        }

        private void CredentialUpdate(object sender, EventArgs e)
        {
            SetVisibility();
            _databases = null;
            cmbDatabase.Text = string.Empty;
        }

        private void DatabaseUpdate(object sender, EventArgs e)
        {
            SetVisibility();
            _databases = null;
            cmbDatabase.Text = string.Empty;
        }
    }
}
