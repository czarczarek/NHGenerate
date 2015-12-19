using System;
using System.Windows.Forms;

namespace NHGenerate.UI
{
    public partial class ConfigScriptWindow : Form
    {
        public ConfigScriptWindow()
        {
            InitializeComponent();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            var succeed = Settings.Save();
            if (succeed)
            {
                Close();
            }
        }
    }
}
