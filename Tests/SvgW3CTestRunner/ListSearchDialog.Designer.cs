namespace SvgW3CTestRunner
{
    partial class ListSearchDialog
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
            labelSelectTab = new System.Windows.Forms.Label();
            comboBoxSelectTab = new System.Windows.Forms.ComboBox();
            labelSearch = new System.Windows.Forms.Label();
            textBoxSearch = new System.Windows.Forms.TextBox();
            labelStatus = new System.Windows.Forms.Label();
            buttonSearch = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // labelSelectTab
            // 
            labelSelectTab.AutoSize = true;
            labelSelectTab.Location = new System.Drawing.Point(22, 14);
            labelSelectTab.Name = "labelSelectTab";
            labelSelectTab.Size = new System.Drawing.Size(100, 17);
            labelSelectTab.TabIndex = 0;
            labelSelectTab.Text = "Select Tests Tab";
            // 
            // comboBoxSelectTab
            // 
            comboBoxSelectTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxSelectTab.FormattingEnabled = true;
            comboBoxSelectTab.Location = new System.Drawing.Point(155, 8);
            comboBoxSelectTab.Name = "comboBoxSelectTab";
            comboBoxSelectTab.Size = new System.Drawing.Size(404, 25);
            comboBoxSelectTab.TabIndex = 1;
            // 
            // labelSearch
            // 
            labelSearch.AutoSize = true;
            labelSearch.Location = new System.Drawing.Point(22, 54);
            labelSearch.Name = "labelSearch";
            labelSearch.Size = new System.Drawing.Size(127, 17);
            labelSearch.TabIndex = 2;
            labelSearch.Text = "Enter Test File Name";
            // 
            // textBoxSearch
            // 
            textBoxSearch.Enabled = false;
            textBoxSearch.Location = new System.Drawing.Point(155, 51);
            textBoxSearch.Name = "textBoxSearch";
            textBoxSearch.Size = new System.Drawing.Size(404, 25);
            textBoxSearch.TabIndex = 3;
            // 
            // labelStatus
            // 
            labelStatus.BackColor = System.Drawing.SystemColors.ControlDark;
            labelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            labelStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            labelStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            labelStatus.ForeColor = System.Drawing.Color.White;
            labelStatus.Location = new System.Drawing.Point(0, 92);
            labelStatus.Margin = new System.Windows.Forms.Padding(3);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new System.Drawing.Size(686, 36);
            labelStatus.TabIndex = 4;
            labelStatus.Text = "Quick Help: Searches the selected tests tab for a sepcified file name";
            labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSearch
            // 
            buttonSearch.Enabled = false;
            buttonSearch.Location = new System.Drawing.Point(565, 48);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new System.Drawing.Size(95, 32);
            buttonSearch.TabIndex = 5;
            buttonSearch.Text = "Search";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += OnSearch;
            // 
            // ListSearchDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(686, 128);
            Controls.Add(buttonSearch);
            Controls.Add(labelStatus);
            Controls.Add(textBoxSearch);
            Controls.Add(labelSearch);
            Controls.Add(comboBoxSelectTab);
            Controls.Add(labelSelectTab);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ListSearchDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "List Search Dialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelSelectTab;
        private System.Windows.Forms.ComboBox comboBoxSelectTab;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonSearch;
    }
}