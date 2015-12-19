using System;
using System.Windows.Forms;

namespace NHGenerate.UI
{
    public partial class SettingsWindow : Form
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            Settings.Save();
            Close();
        }
    }
}
