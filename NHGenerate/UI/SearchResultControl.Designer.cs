namespace NHGenerate.UI
{
    partial class SearchResultControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scContent = new System.Windows.Forms.SplitContainer();
            this.resultView = new System.Windows.Forms.TreeView();
            this.txtDeleteSql = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.scContent)).BeginInit();
            this.scContent.Panel1.SuspendLayout();
            this.scContent.Panel2.SuspendLayout();
            this.scContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // scContent
            // 
            this.scContent.BackColor = System.Drawing.Color.Transparent;
            this.scContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scContent.Location = new System.Drawing.Point(0, 0);
            this.scContent.Name = "scContent";
            this.scContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scContent.Panel1
            // 
            this.scContent.Panel1.Controls.Add(this.resultView);
            // 
            // scContent.Panel2
            // 
            this.scContent.Panel2.Controls.Add(this.txtDeleteSql);
            this.scContent.Size = new System.Drawing.Size(454, 432);
            this.scContent.SplitterDistance = 264;
            this.scContent.TabIndex = 2;
            // 
            // resultView
            // 
            this.resultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultView.Location = new System.Drawing.Point(0, 0);
            this.resultView.Name = "resultView";
            this.resultView.Size = new System.Drawing.Size(454, 264);
            this.resultView.TabIndex = 0;
            this.resultView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.resultView_NodeMouseDoubleClick);
            // 
            // txtDeleteSql
            // 
            this.txtDeleteSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeleteSql.Location = new System.Drawing.Point(0, 0);
            this.txtDeleteSql.Multiline = true;
            this.txtDeleteSql.Name = "txtDeleteSql";
            this.txtDeleteSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDeleteSql.Size = new System.Drawing.Size(454, 164);
            this.txtDeleteSql.TabIndex = 0;
            // 
            // SearchResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scContent);
            this.Name = "SearchResultControl";
            this.Size = new System.Drawing.Size(454, 432);
            this.scContent.Panel1.ResumeLayout(false);
            this.scContent.Panel2.ResumeLayout(false);
            this.scContent.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scContent)).EndInit();
            this.scContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scContent;
        private System.Windows.Forms.TreeView resultView;
        private System.Windows.Forms.TextBox txtDeleteSql;
    }
}
