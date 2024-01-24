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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunTestsDialog));
            panelTop = new System.Windows.Forms.Panel();
            textBoxExport = new System.Windows.Forms.TextBox();
            buttonExport = new System.Windows.Forms.Button();
            buttonRun = new System.Windows.Forms.Button();
            comboBoxSelectTab = new System.Windows.Forms.ComboBox();
            labelExport = new System.Windows.Forms.Label();
            labelSelectTab = new System.Windows.Forms.Label();
            tabControlResults = new System.Windows.Forms.TabControl();
            tabPageRun = new System.Windows.Forms.TabPage();
            listView = new System.Windows.Forms.ListView();
            columnNone = new System.Windows.Forms.ColumnHeader();
            columnNumber = new System.Windows.Forms.ColumnHeader();
            columnFileName = new System.Windows.Forms.ColumnHeader();
            columnException = new System.Windows.Forms.ColumnHeader();
            columnPercent = new System.Windows.Forms.ColumnHeader();
            columnRefException = new System.Windows.Forms.ColumnHeader();
            columnRefDiff = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            copySelectedFileName = new System.Windows.Forms.ToolStripMenuItem();
            copySelectedFilePath = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            viewSelectedItem = new System.Windows.Forms.ToolStripMenuItem();
            tabPageLog = new System.Windows.Forms.TabPage();
            richTextBox = new System.Windows.Forms.RichTextBox();
            panelBottom = new System.Windows.Forms.Panel();
            labelSelected = new System.Windows.Forms.Label();
            buttonSelectedCopy = new System.Windows.Forms.Button();
            buttonSelectedView = new System.Windows.Forms.Button();
            panelTop.SuspendLayout();
            tabControlResults.SuspendLayout();
            tabPageRun.SuspendLayout();
            contextMenuStrip.SuspendLayout();
            tabPageLog.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(textBoxExport);
            panelTop.Controls.Add(buttonExport);
            panelTop.Controls.Add(buttonRun);
            panelTop.Controls.Add(comboBoxSelectTab);
            panelTop.Controls.Add(labelExport);
            panelTop.Controls.Add(labelSelectTab);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(3, 3);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(878, 48);
            panelTop.TabIndex = 0;
            // 
            // textBoxExport
            // 
            textBoxExport.Location = new System.Drawing.Point(579, 11);
            textBoxExport.Name = "textBoxExport";
            textBoxExport.Size = new System.Drawing.Size(152, 25);
            textBoxExport.TabIndex = 7;
            // 
            // buttonExport
            // 
            buttonExport.Location = new System.Drawing.Point(737, 8);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new System.Drawing.Size(95, 32);
            buttonExport.TabIndex = 6;
            buttonExport.Text = "Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += OnClickExport;
            // 
            // buttonRun
            // 
            buttonRun.Enabled = false;
            buttonRun.Location = new System.Drawing.Point(348, 8);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new System.Drawing.Size(95, 32);
            buttonRun.TabIndex = 6;
            buttonRun.Text = "Run";
            buttonRun.UseVisualStyleBackColor = true;
            buttonRun.Click += OnClickRun;
            // 
            // comboBoxSelectTab
            // 
            comboBoxSelectTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxSelectTab.FormattingEnabled = true;
            comboBoxSelectTab.Location = new System.Drawing.Point(140, 11);
            comboBoxSelectTab.Name = "comboBoxSelectTab";
            comboBoxSelectTab.Size = new System.Drawing.Size(198, 25);
            comboBoxSelectTab.TabIndex = 3;
            comboBoxSelectTab.SelectedIndexChanged += OnSelectedTabChanged;
            // 
            // labelExport
            // 
            labelExport.Location = new System.Drawing.Point(493, 12);
            labelExport.Name = "labelExport";
            labelExport.Size = new System.Drawing.Size(86, 24);
            labelExport.TabIndex = 2;
            labelExport.Text = "Name Suffix";
            // 
            // labelSelectTab
            // 
            labelSelectTab.Location = new System.Drawing.Point(23, 12);
            labelSelectTab.Name = "labelSelectTab";
            labelSelectTab.Size = new System.Drawing.Size(110, 24);
            labelSelectTab.TabIndex = 2;
            labelSelectTab.Text = "Select Tests Tab";
            labelSelectTab.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tabControlResults
            // 
            tabControlResults.Controls.Add(tabPageRun);
            tabControlResults.Controls.Add(tabPageLog);
            tabControlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlResults.ItemSize = new System.Drawing.Size(150, 28);
            tabControlResults.Location = new System.Drawing.Point(3, 51);
            tabControlResults.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            tabControlResults.Name = "tabControlResults";
            tabControlResults.SelectedIndex = 0;
            tabControlResults.Size = new System.Drawing.Size(878, 525);
            tabControlResults.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            tabControlResults.TabIndex = 1;
            // 
            // tabPageRun
            // 
            tabPageRun.Controls.Add(listView);
            tabPageRun.Location = new System.Drawing.Point(4, 32);
            tabPageRun.Name = "tabPageRun";
            tabPageRun.Padding = new System.Windows.Forms.Padding(3);
            tabPageRun.Size = new System.Drawing.Size(870, 489);
            tabPageRun.TabIndex = 0;
            tabPageRun.Text = "Test Results";
            tabPageRun.UseVisualStyleBackColor = true;
            // 
            // listView
            // 
            listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnNone, columnNumber, columnFileName, columnException, columnPercent, columnRefException, columnRefDiff });
            listView.ContextMenuStrip = contextMenuStrip;
            listView.Dock = System.Windows.Forms.DockStyle.Fill;
            listView.FullRowSelect = true;
            listView.HideSelection = false;
            listView.Location = new System.Drawing.Point(3, 3);
            listView.MultiSelect = false;
            listView.Name = "listView";
            listView.Size = new System.Drawing.Size(864, 483);
            listView.TabIndex = 0;
            listView.UseCompatibleStateImageBehavior = false;
            listView.View = System.Windows.Forms.View.Details;
            listView.ItemActivate += OnListItemClick;
            listView.SelectedIndexChanged += OnSelectedListIndexChanged;
            // 
            // columnNone
            // 
            columnNone.Text = "i";
            columnNone.Width = 4;
            // 
            // columnNumber
            // 
            columnNumber.Text = "#";
            columnNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            columnNumber.Width = 50;
            // 
            // columnFileName
            // 
            columnFileName.Text = "File name";
            columnFileName.Width = 360;
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
            // columnRefException
            // 
            columnRefException.Text = "Exception (Ref)";
            columnRefException.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            columnRefException.Width = 100;
            // 
            // columnRefDiff
            // 
            columnRefDiff.Text = "Diff Percent (Ref)";
            columnRefDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            columnRefDiff.Width = 120;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { copySelectedFileName, copySelectedFilePath, toolStripSeparator1, viewSelectedItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(206, 76);
            contextMenuStrip.Opening += OnMenuOpening;
            contextMenuStrip.ItemClicked += OnMenuItemClick;
            // 
            // copySelectedFileName
            // 
            copySelectedFileName.Name = "copySelectedFileName";
            copySelectedFileName.Size = new System.Drawing.Size(205, 22);
            copySelectedFileName.Text = "Copy Selected File Name";
            // 
            // copySelectedFilePath
            // 
            copySelectedFilePath.Name = "copySelectedFilePath";
            copySelectedFilePath.Size = new System.Drawing.Size(205, 22);
            copySelectedFilePath.Text = "Copy Selected File Path";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(202, 6);
            // 
            // viewSelectedItem
            // 
            viewSelectedItem.Name = "viewSelectedItem";
            viewSelectedItem.Size = new System.Drawing.Size(205, 22);
            viewSelectedItem.Text = "View Selected Item";
            // 
            // tabPageLog
            // 
            tabPageLog.Controls.Add(richTextBox);
            tabPageLog.Location = new System.Drawing.Point(4, 32);
            tabPageLog.Name = "tabPageLog";
            tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            tabPageLog.Size = new System.Drawing.Size(870, 489);
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
            richTextBox.Size = new System.Drawing.Size(864, 483);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            richTextBox.WordWrap = false;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = System.Drawing.SystemColors.Control;
            panelBottom.Controls.Add(labelSelected);
            panelBottom.Controls.Add(buttonSelectedCopy);
            panelBottom.Controls.Add(buttonSelectedView);
            panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelBottom.Location = new System.Drawing.Point(3, 576);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new System.Drawing.Size(878, 32);
            panelBottom.TabIndex = 2;
            // 
            // labelSelected
            // 
            labelSelected.BackColor = System.Drawing.SystemColors.Window;
            labelSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            labelSelected.Location = new System.Drawing.Point(75, 0);
            labelSelected.Margin = new System.Windows.Forms.Padding(3);
            labelSelected.Name = "labelSelected";
            labelSelected.Size = new System.Drawing.Size(728, 32);
            labelSelected.TabIndex = 2;
            labelSelected.Text = "(selected)";
            labelSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSelectedCopy
            // 
            buttonSelectedCopy.Dock = System.Windows.Forms.DockStyle.Left;
            buttonSelectedCopy.Enabled = false;
            buttonSelectedCopy.Location = new System.Drawing.Point(0, 0);
            buttonSelectedCopy.Name = "buttonSelectedCopy";
            buttonSelectedCopy.Size = new System.Drawing.Size(75, 32);
            buttonSelectedCopy.TabIndex = 1;
            buttonSelectedCopy.Text = "Copy";
            buttonSelectedCopy.UseVisualStyleBackColor = true;
            buttonSelectedCopy.Click += OnClickSelectedCopy;
            // 
            // buttonSelectedView
            // 
            buttonSelectedView.Dock = System.Windows.Forms.DockStyle.Right;
            buttonSelectedView.Enabled = false;
            buttonSelectedView.Location = new System.Drawing.Point(803, 0);
            buttonSelectedView.Name = "buttonSelectedView";
            buttonSelectedView.Size = new System.Drawing.Size(75, 32);
            buttonSelectedView.TabIndex = 0;
            buttonSelectedView.Text = "View";
            buttonSelectedView.UseVisualStyleBackColor = true;
            buttonSelectedView.Click += OnClickSelectedView;
            // 
            // RunTestsDialog
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(884, 611);
            Controls.Add(tabControlResults);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimumSize = new System.Drawing.Size(900, 600);
            Name = "RunTestsDialog";
            Padding = new System.Windows.Forms.Padding(3);
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Run Tests";
            Load += OnLoadDialog;
            Shown += OnShownDialog;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            tabControlResults.ResumeLayout(false);
            tabPageRun.ResumeLayout(false);
            contextMenuStrip.ResumeLayout(false);
            tabPageLog.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
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
        private System.Windows.Forms.ColumnHeader columnNone;
        private System.Windows.Forms.Button buttonExport;

        private System.Windows.Forms.Label labelExport;
        private System.Windows.Forms.TextBox textBoxExport;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonSelectedView;
        private System.Windows.Forms.Label labelSelected;
        private System.Windows.Forms.Button buttonSelectedCopy;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copySelectedFileName;
        private System.Windows.Forms.ToolStripMenuItem copySelectedFilePath;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem viewSelectedItem;
        private System.Windows.Forms.ColumnHeader columnRefException;
        private System.Windows.Forms.ColumnHeader columnRefDiff;
    }
}