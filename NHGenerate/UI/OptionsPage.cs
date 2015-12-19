using System.Windows.Forms;
using NHGenerate.Core;

namespace NHGenerate.UI
{
    public class OptionsPage : UserControl
    {
        private PageProperties _properties = new PageProperties();
        private TextBox _txtDefaultNamespace;
        private Label _lblDefaultNamespace;
        private CheckBox _cbUseShortTypeName;
        private CheckBox _cbIncludeColumnName;
        private CheckBox _cbIncludeNotNull;
        private CheckBox _cbIncludeColumnSize;

        public OptionsPage()
        {
            InitializeComponent();
        }

        private void OptionsPageLoad(object sender, System.EventArgs e)
        {
            _txtDefaultNamespace.Text = _properties.DefaultNamespace;
            _cbIncludeColumnName.Checked = _properties.IncludeColumnName;
            _cbIncludeColumnSize.Checked = _properties.IncludeColumnSize;
            _cbIncludeNotNull.Checked = _properties.IncludeNotNull;
            _cbUseShortTypeName.Checked = _properties.UseShortTypeName;
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(_txtDefaultNamespace.Text))
            {
                _properties.DefaultNamespace = _txtDefaultNamespace.Text;
            }

            _properties.IncludeColumnName = _cbIncludeColumnName.Checked;
            _properties.IncludeColumnSize = _cbIncludeColumnSize.Checked;
            _properties.IncludeNotNull = _cbIncludeNotNull.Checked;
            _properties.UseShortTypeName = _cbUseShortTypeName.Checked;
        }


        #region priv

        private void InitializeComponent()
        {
            this._txtDefaultNamespace = new System.Windows.Forms.TextBox();
            this._lblDefaultNamespace = new System.Windows.Forms.Label();
            this._cbUseShortTypeName = new System.Windows.Forms.CheckBox();
            this._cbIncludeColumnName = new System.Windows.Forms.CheckBox();
            this._cbIncludeColumnSize = new System.Windows.Forms.CheckBox();
            this._cbIncludeNotNull = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // _txtDefaultNamespace
            // 
            this._txtDefaultNamespace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtDefaultNamespace.Location = new System.Drawing.Point(18, 32);
            this._txtDefaultNamespace.Name = "_txtDefaultNamespace";
            this._txtDefaultNamespace.Size = new System.Drawing.Size(344, 20);
            this._txtDefaultNamespace.TabIndex = 0;
            // 
            // _lblDefaultNamespace
            // 
            this._lblDefaultNamespace.AutoSize = true;
            this._lblDefaultNamespace.Location = new System.Drawing.Point(15, 16);
            this._lblDefaultNamespace.Name = "_lblDefaultNamespace";
            this._lblDefaultNamespace.Size = new System.Drawing.Size(102, 13);
            this._lblDefaultNamespace.TabIndex = 1;
            this._lblDefaultNamespace.Text = "Default namespace:";
            // 
            // cbUseShortTypeName
            // 
            this._cbUseShortTypeName.AutoSize = true;
            this._cbUseShortTypeName.Location = new System.Drawing.Point(18, 58);
            this._cbUseShortTypeName.Name = "_cbUseShortTypeName";
            this._cbUseShortTypeName.Size = new System.Drawing.Size(123, 17);
            this._cbUseShortTypeName.TabIndex = 2;
            this._cbUseShortTypeName.Text = "Use short type name";
            this._cbUseShortTypeName.UseVisualStyleBackColor = true;
            // 
            // cbIncludeColumnName
            // 
            this._cbIncludeColumnName.AutoSize = true;
            this._cbIncludeColumnName.Location = new System.Drawing.Point(18, 81);
            this._cbIncludeColumnName.Name = "_cbIncludeColumnName";
            this._cbIncludeColumnName.Size = new System.Drawing.Size(127, 17);
            this._cbIncludeColumnName.TabIndex = 3;
            this._cbIncludeColumnName.Text = "Include column name";
            this._cbIncludeColumnName.UseVisualStyleBackColor = true;
            // 
            // cbIncludeColumnSize
            // 
            this._cbIncludeColumnSize.AutoSize = true;
            this._cbIncludeColumnSize.Location = new System.Drawing.Point(18, 104);
            this._cbIncludeColumnSize.Name = "_cbIncludeColumnSize";
            this._cbIncludeColumnSize.Size = new System.Drawing.Size(119, 17);
            this._cbIncludeColumnSize.TabIndex = 4;
            this._cbIncludeColumnSize.Text = "Include column size";
            this._cbIncludeColumnSize.UseVisualStyleBackColor = true;
            // 
            // cbIncludeNotNull
            // 
            this._cbIncludeNotNull.AutoSize = true;
            this._cbIncludeNotNull.Location = new System.Drawing.Point(18, 127);
            this._cbIncludeNotNull.Name = "_cbIncludeNotNull";
            this._cbIncludeNotNull.Size = new System.Drawing.Size(99, 17);
            this._cbIncludeNotNull.TabIndex = 5;
            this._cbIncludeNotNull.Text = "Include NotNull";
            this._cbIncludeNotNull.UseVisualStyleBackColor = true;
            // 
            // OptionsPage
            // 
            this.Controls.Add(this._cbIncludeNotNull);
            this.Controls.Add(this._cbIncludeColumnSize);
            this.Controls.Add(this._cbIncludeColumnName);
            this.Controls.Add(this._cbUseShortTypeName);
            this.Controls.Add(this._lblDefaultNamespace);
            this.Controls.Add(this._txtDefaultNamespace);
            this.Name = "OptionsPage";
            this.Size = new System.Drawing.Size(380, 213);
            this.Load += new System.EventHandler(this.OptionsPageLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
