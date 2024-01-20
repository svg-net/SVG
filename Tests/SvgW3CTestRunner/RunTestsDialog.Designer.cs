namespace SvgW3CTestRunner
{
    partial class RunTestsDialog
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
            panelTop = new System.Windows.Forms.Panel();
            buttonRun = new System.Windows.Forms.Button();
            comboBoxSelectTab = new System.Windows.Forms.ComboBox();
            labelSelectTab = new System.Windows.Forms.Label();
            tabControlResults = new System.Windows.Forms.TabControl();
            tabPageRun = new System.Windows.Forms.TabPage();
            listView = new System.Windows.Forms.ListView();
            columnNumber = new System.Windows.Forms.ColumnHeader();
            columnFileName = new System.Windows.Forms.ColumnHeader();
            columnException = new System.Windows.Forms.ColumnHeader();
            columnPercent = new System.Windows.Forms.ColumnHeader();
            tabPageLog = new System.Windows.Forms.TabPage();
            richTextBox = new System.Windows.Forms.RichTextBox();
            panelTop.SuspendLayout();
            tabControlResults.SuspendLayout();
            tabPageRun.SuspendLayout();
            tabPageLog.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(buttonRun);
            panelTop.Controls.Add(comboBoxSelectTab);
            panelTop.Controls.Add(labelSelectTab);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(3, 3);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(878, 48);
            panelTop.TabIndex = 0;
            // 
            // buttonRun
            // 
            buttonRun.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonRun.Enabled = false;
            buttonRun.Location = new System.Drawing.Point(621, 8);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new System.Drawing.Size(95, 32);
            buttonRun.TabIndex = 6;
            buttonRun.Text = "Run";
            buttonRun.UseVisualStyleBackColor = true;
            buttonRun.Click += OnClickRun;
            // 
            // comboBoxSelectTab
            // 
            comboBoxSelectTab.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBoxSelectTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxSelectTab.FormattingEnabled = true;
            comboBoxSelectTab.Location = new System.Drawing.Point(240, 11);
            comboBoxSelectTab.Name = "comboBoxSelectTab";
            comboBoxSelectTab.Size = new System.Drawing.Size(369, 25);
            comboBoxSelectTab.TabIndex = 3;
            // 
            // labelSelectTab
            // 
            labelSelectTab.AutoSize = true;
            labelSelectTab.Location = new System.Drawing.Point(128, 15);
            labelSelectTab.Name = "labelSelectTab";
            labelSelectTab.Size = new System.Drawing.Size(100, 17);
            labelSelectTab.TabIndex = 2;
            labelSelectTab.Text = "Select Tests Tab";
            // 
            // tabControlResults
            // 
            tabControlResults.Controls.Add(tabPageRun);
            tabControlResults.Controls.Add(tabPageLog);
            tabControlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlResults.ItemSize = new System.Drawing.Size(120, 28);
            tabControlResults.Location = new System.Drawing.Point(3, 51);
            tabControlResults.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            tabControlResults.Name = "tabControlResults";
            tabControlResults.SelectedIndex = 0;
            tabControlResults.Size = new System.Drawing.Size(878, 557);
            tabControlResults.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            tabControlResults.TabIndex = 1;
            // 
            // tabPageRun
            // 
            tabPageRun.Controls.Add(listView);
            tabPageRun.Location = new System.Drawing.Point(4, 32);
            tabPageRun.Name = "tabPageRun";
            tabPageRun.Padding = new System.Windows.Forms.Padding(3);
            tabPageRun.Size = new System.Drawing.Size(870, 521);
            tabPageRun.TabIndex = 0;
            tabPageRun.Text = "Test Results";
            tabPageRun.UseVisualStyleBackColor = true;
            // 
            // listView
            // 
            listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnNumber, columnFileName, columnException, columnPercent });
            listView.Dock = System.Windows.Forms.DockStyle.Fill;
            listView.FullRowSelect = true;
            listView.HideSelection = false;
            listView.Location = new System.Drawing.Point(3, 3);
            listView.Name = "listView";
            listView.Size = new System.Drawing.Size(864, 515);
            listView.TabIndex = 0;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = System.Windows.Forms.View.Details;
            // 
            // columnNumber
            // 
            columnNumber.Text = "#";
            // 
            // columnFileName
            // 
            columnFileName.Text = "File name";
            columnFileName.Width = 500;
            // 
            // columnException
            // 
            columnException.Text = "Exception";
            columnException.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            columnException.Width = 80;
            // 
            // columnPercent
            // 
            columnPercent.Text = "Diff Percent (%)";
            columnPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            columnPercent.Width = 120;
            // 
            // tabPageLog
            // 
            tabPageLog.Controls.Add(richTextBox);
            tabPageLog.Location = new System.Drawing.Point(4, 32);
            tabPageLog.Name = "tabPageLog";
            tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            tabPageLog.Size = new System.Drawing.Size(870, 521);
            tabPageLog.TabIndex = 1;
            tabPageLog.Text = "Logs";
            tabPageLog.UseVisualStyleBackColor = true;
            // 
            // richTextBox
            // 
            richTextBox.BackColor = System.Drawing.SystemColors.Window;
            richTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            richTextBox.Location = new System.Drawing.Point(3, 3);
            richTextBox.Name = "richTextBox";
            richTextBox.ReadOnly = true;
            richTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            richTextBox.ShowSelectionMargin = true;
            richTextBox.Size = new System.Drawing.Size(864, 515);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            richTextBox.WordWrap = false;
            // 
            // RunTestsDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(884, 611);
            Controls.Add(tabControlResults);
            Controls.Add(panelTop);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(900, 600);
            Name = "RunTestsDialog";
            Padding = new System.Windows.Forms.Padding(3);
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Run Tests";
            Load += OnLoadDialog;
            Shown += OnShownDialog;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            tabControlResults.ResumeLayout(false);
            tabPageRun.ResumeLayout(false);
            tabPageLog.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TabControl tabControlResults;
        private System.Windows.Forms.TabPage tabPageRun;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.ComboBox comboBoxSelectTab;
        private System.Windows.Forms.Label labelSelectTab;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnNumber;
        private System.Windows.Forms.ColumnHeader columnFileName;
        private System.Windows.Forms.ColumnHeader columnPercent;
        private System.Windows.Forms.ColumnHeader columnException;
    }
}