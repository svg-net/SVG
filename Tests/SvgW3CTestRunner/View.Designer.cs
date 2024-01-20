using System.Drawing;

namespace SvgW3CTestRunner
{
    partial class View
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
            uniformGrid = new System.Windows.Forms.TableLayoutPanel();
            panelSvg = new System.Windows.Forms.Panel();
            picSvg = new System.Windows.Forms.PictureBox();
            labelSvg = new System.Windows.Forms.Label();
            panelPng = new System.Windows.Forms.Panel();
            picPng = new System.Windows.Forms.PictureBox();
            label1 = new System.Windows.Forms.Label();
            panelSaveLoad = new System.Windows.Forms.Panel();
            picSaveLoad = new System.Windows.Forms.PictureBox();
            labelSaveLoad = new System.Windows.Forms.Label();
            panelSVGPNG = new System.Windows.Forms.Panel();
            picSVGPNG = new System.Windows.Forms.PictureBox();
            labelSVGPNG = new System.Windows.Forms.Label();
            splitContainerBase = new System.Windows.Forms.SplitContainer();
            fileTabBox = new System.Windows.Forms.TabControl();
            passW3CTabPage = new System.Windows.Forms.TabPage();
            lstW3CFilesPassing = new System.Windows.Forms.ListBox();
            failW3CTabPage = new System.Windows.Forms.TabPage();
            lstW3CFilesFailing = new System.Windows.Forms.ListBox();
            passOtherTabPage = new System.Windows.Forms.TabPage();
            lstFilesOtherPassing = new System.Windows.Forms.ListBox();
            failOtherTabPage = new System.Windows.Forms.TabPage();
            lstFilesOtherFailing = new System.Windows.Forms.ListBox();
            panelButtons = new System.Windows.Forms.Panel();
            buttonFind = new System.Windows.Forms.Button();
            buttonRun = new System.Windows.Forms.Button();
            bottomTabBox = new System.Windows.Forms.TabControl();
            outputTab = new System.Windows.Forms.TabPage();
            boxConsoleLog = new System.Windows.Forms.RichTextBox();
            descriptionTab = new System.Windows.Forms.TabPage();
            boxDescription = new System.Windows.Forms.RichTextBox();
            uniformGrid.SuspendLayout();
            panelSvg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSvg).BeginInit();
            panelPng.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPng).BeginInit();
            panelSaveLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSaveLoad).BeginInit();
            panelSVGPNG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSVGPNG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerBase).BeginInit();
            splitContainerBase.Panel1.SuspendLayout();
            splitContainerBase.Panel2.SuspendLayout();
            splitContainerBase.SuspendLayout();
            fileTabBox.SuspendLayout();
            passW3CTabPage.SuspendLayout();
            failW3CTabPage.SuspendLayout();
            passOtherTabPage.SuspendLayout();
            failOtherTabPage.SuspendLayout();
            panelButtons.SuspendLayout();
            bottomTabBox.SuspendLayout();
            outputTab.SuspendLayout();
            descriptionTab.SuspendLayout();
            SuspendLayout();
            // 
            // uniformGrid
            // 
            uniformGrid.BackColor = SystemColors.Control;
            uniformGrid.ColumnCount = 2;
            uniformGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            uniformGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            uniformGrid.Controls.Add(panelSvg, 0, 0);
            uniformGrid.Controls.Add(panelPng, 1, 0);
            uniformGrid.Controls.Add(panelSaveLoad, 0, 1);
            uniformGrid.Controls.Add(panelSVGPNG, 1, 1);
            uniformGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            uniformGrid.Location = new Point(0, 0);
            uniformGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            uniformGrid.Name = "uniformGrid";
            uniformGrid.RowCount = 2;
            uniformGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            uniformGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            uniformGrid.Size = new Size(1203, 918);
            uniformGrid.TabIndex = 1;
            // 
            // panelSvg
            // 
            panelSvg.BackColor = SystemColors.Window;
            panelSvg.Controls.Add(picSvg);
            panelSvg.Controls.Add(labelSvg);
            panelSvg.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSvg.Location = new Point(3, 3);
            panelSvg.Name = "panelSvg";
            panelSvg.Size = new Size(595, 453);
            panelSvg.TabIndex = 0;
            // 
            // picSvg
            // 
            picSvg.Dock = System.Windows.Forms.DockStyle.Fill;
            picSvg.Location = new Point(0, 24);
            picSvg.Name = "picSvg";
            picSvg.Size = new Size(595, 429);
            picSvg.TabIndex = 1;
            picSvg.TabStop = false;
            // 
            // labelSvg
            // 
            labelSvg.BackColor = SystemColors.Control;
            labelSvg.Dock = System.Windows.Forms.DockStyle.Top;
            labelSvg.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSvg.Location = new Point(0, 0);
            labelSvg.Name = "labelSvg";
            labelSvg.Size = new Size(595, 24);
            labelSvg.TabIndex = 0;
            labelSvg.Text = "SVG Render";
            // 
            // panelPng
            // 
            panelPng.BackColor = SystemColors.Window;
            panelPng.Controls.Add(picPng);
            panelPng.Controls.Add(label1);
            panelPng.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPng.Location = new Point(604, 3);
            panelPng.Name = "panelPng";
            panelPng.Size = new Size(596, 453);
            panelPng.TabIndex = 1;
            // 
            // picPng
            // 
            picPng.Dock = System.Windows.Forms.DockStyle.Fill;
            picPng.Location = new Point(0, 24);
            picPng.Name = "picPng";
            picPng.Size = new Size(596, 429);
            picPng.TabIndex = 1;
            picPng.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = SystemColors.Control;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(596, 24);
            label1.TabIndex = 0;
            label1.Text = "Reference PNG";
            // 
            // panelSaveLoad
            // 
            panelSaveLoad.BackColor = SystemColors.Window;
            panelSaveLoad.Controls.Add(picSaveLoad);
            panelSaveLoad.Controls.Add(labelSaveLoad);
            panelSaveLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSaveLoad.Location = new Point(3, 462);
            panelSaveLoad.Name = "panelSaveLoad";
            panelSaveLoad.Size = new Size(595, 453);
            panelSaveLoad.TabIndex = 2;
            // 
            // picSaveLoad
            // 
            picSaveLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            picSaveLoad.Location = new Point(0, 24);
            picSaveLoad.Name = "picSaveLoad";
            picSaveLoad.Size = new Size(595, 429);
            picSaveLoad.TabIndex = 1;
            picSaveLoad.TabStop = false;
            // 
            // labelSaveLoad
            // 
            labelSaveLoad.BackColor = SystemColors.Control;
            labelSaveLoad.Dock = System.Windows.Forms.DockStyle.Top;
            labelSaveLoad.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSaveLoad.Location = new Point(0, 0);
            labelSaveLoad.Name = "labelSaveLoad";
            labelSaveLoad.Size = new Size(595, 24);
            labelSaveLoad.TabIndex = 0;
            labelSaveLoad.Text = "Save and Load";
            // 
            // panelSVGPNG
            // 
            panelSVGPNG.BackColor = SystemColors.Window;
            panelSVGPNG.Controls.Add(picSVGPNG);
            panelSVGPNG.Controls.Add(labelSVGPNG);
            panelSVGPNG.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSVGPNG.Location = new Point(604, 462);
            panelSVGPNG.Name = "panelSVGPNG";
            panelSVGPNG.Size = new Size(596, 453);
            panelSVGPNG.TabIndex = 3;
            // 
            // picSVGPNG
            // 
            picSVGPNG.Dock = System.Windows.Forms.DockStyle.Fill;
            picSVGPNG.Location = new Point(0, 24);
            picSVGPNG.Name = "picSVGPNG";
            picSVGPNG.Size = new Size(596, 429);
            picSVGPNG.TabIndex = 1;
            picSVGPNG.TabStop = false;
            // 
            // labelSVGPNG
            // 
            labelSVGPNG.BackColor = SystemColors.Control;
            labelSVGPNG.Dock = System.Windows.Forms.DockStyle.Top;
            labelSVGPNG.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelSVGPNG.Location = new Point(0, 0);
            labelSVGPNG.Name = "labelSVGPNG";
            labelSVGPNG.Size = new Size(596, 24);
            labelSVGPNG.TabIndex = 0;
            labelSVGPNG.Text = "SVG vs PNG";
            // 
            // splitContainerBase
            // 
            splitContainerBase.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerBase.Location = new Point(3, 3);
            splitContainerBase.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainerBase.Name = "splitContainerBase";
            // 
            // splitContainerBase.Panel1
            // 
            splitContainerBase.Panel1.BackColor = SystemColors.Control;
            splitContainerBase.Panel1.Controls.Add(fileTabBox);
            splitContainerBase.Panel1.Controls.Add(panelButtons);
            // 
            // splitContainerBase.Panel2
            // 
            splitContainerBase.Panel2.Controls.Add(uniformGrid);
            splitContainerBase.Panel2.Controls.Add(bottomTabBox);
            splitContainerBase.Size = new Size(1486, 992);
            splitContainerBase.SplitterDistance = 278;
            splitContainerBase.SplitterWidth = 5;
            splitContainerBase.TabIndex = 0;
            // 
            // fileTabBox
            // 
            fileTabBox.Controls.Add(passW3CTabPage);
            fileTabBox.Controls.Add(failW3CTabPage);
            fileTabBox.Controls.Add(passOtherTabPage);
            fileTabBox.Controls.Add(failOtherTabPage);
            fileTabBox.Dock = System.Windows.Forms.DockStyle.Fill;
            fileTabBox.HotTrack = true;
            fileTabBox.ItemSize = new Size(64, 28);
            fileTabBox.Location = new Point(0, 0);
            fileTabBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            fileTabBox.Name = "fileTabBox";
            fileTabBox.SelectedIndex = 0;
            fileTabBox.Size = new Size(278, 956);
            fileTabBox.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            fileTabBox.TabIndex = 3;
            fileTabBox.SelectedIndexChanged += fileTabBox_TabIndexChanged;
            // 
            // passW3CTabPage
            // 
            passW3CTabPage.Controls.Add(lstW3CFilesPassing);
            passW3CTabPage.Location = new Point(4, 32);
            passW3CTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            passW3CTabPage.Name = "passW3CTabPage";
            passW3CTabPage.Size = new Size(270, 920);
            passW3CTabPage.TabIndex = 0;
            passW3CTabPage.Text = "Pass W3C";
            passW3CTabPage.UseVisualStyleBackColor = true;
            // 
            // lstW3CFilesPassing
            // 
            lstW3CFilesPassing.Dock = System.Windows.Forms.DockStyle.Fill;
            lstW3CFilesPassing.FormattingEnabled = true;
            lstW3CFilesPassing.ItemHeight = 15;
            lstW3CFilesPassing.Location = new Point(0, 0);
            lstW3CFilesPassing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstW3CFilesPassing.Name = "lstW3CFilesPassing";
            lstW3CFilesPassing.Size = new Size(270, 920);
            lstW3CFilesPassing.TabIndex = 1;
            lstW3CFilesPassing.SelectedIndexChanged += OnW3CSelectedIndexChanged;
            // 
            // failW3CTabPage
            // 
            failW3CTabPage.Controls.Add(lstW3CFilesFailing);
            failW3CTabPage.Location = new Point(4, 32);
            failW3CTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            failW3CTabPage.Name = "failW3CTabPage";
            failW3CTabPage.Size = new Size(270, 920);
            failW3CTabPage.TabIndex = 1;
            failW3CTabPage.Text = "Fail W3C";
            failW3CTabPage.UseVisualStyleBackColor = true;
            // 
            // lstW3CFilesFailing
            // 
            lstW3CFilesFailing.Dock = System.Windows.Forms.DockStyle.Fill;
            lstW3CFilesFailing.FormattingEnabled = true;
            lstW3CFilesFailing.ItemHeight = 15;
            lstW3CFilesFailing.Location = new Point(0, 0);
            lstW3CFilesFailing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstW3CFilesFailing.Name = "lstW3CFilesFailing";
            lstW3CFilesFailing.Size = new Size(270, 920);
            lstW3CFilesFailing.TabIndex = 0;
            lstW3CFilesFailing.SelectedIndexChanged += OnW3CSelectedIndexChanged;
            // 
            // passOtherTabPage
            // 
            passOtherTabPage.Controls.Add(lstFilesOtherPassing);
            passOtherTabPage.Location = new Point(4, 32);
            passOtherTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            passOtherTabPage.Name = "passOtherTabPage";
            passOtherTabPage.Size = new Size(270, 920);
            passOtherTabPage.TabIndex = 2;
            passOtherTabPage.Text = "Pass Other";
            passOtherTabPage.UseVisualStyleBackColor = true;
            // 
            // lstFilesOtherPassing
            // 
            lstFilesOtherPassing.Dock = System.Windows.Forms.DockStyle.Fill;
            lstFilesOtherPassing.FormattingEnabled = true;
            lstFilesOtherPassing.ItemHeight = 15;
            lstFilesOtherPassing.Location = new Point(0, 0);
            lstFilesOtherPassing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstFilesOtherPassing.Name = "lstFilesOtherPassing";
            lstFilesOtherPassing.Size = new Size(270, 920);
            lstFilesOtherPassing.TabIndex = 0;
            lstFilesOtherPassing.SelectedIndexChanged += OnIssuesSelectedIndexChanged;
            // 
            // failOtherTabPage
            // 
            failOtherTabPage.Controls.Add(lstFilesOtherFailing);
            failOtherTabPage.Location = new Point(4, 32);
            failOtherTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            failOtherTabPage.Name = "failOtherTabPage";
            failOtherTabPage.Size = new Size(270, 920);
            failOtherTabPage.TabIndex = 2;
            failOtherTabPage.Text = "Fail Other";
            failOtherTabPage.UseVisualStyleBackColor = true;
            // 
            // lstFilesOtherFailing
            // 
            lstFilesOtherFailing.Dock = System.Windows.Forms.DockStyle.Fill;
            lstFilesOtherFailing.FormattingEnabled = true;
            lstFilesOtherFailing.ItemHeight = 15;
            lstFilesOtherFailing.Location = new Point(0, 0);
            lstFilesOtherFailing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstFilesOtherFailing.Name = "lstFilesOtherFailing";
            lstFilesOtherFailing.Size = new Size(270, 920);
            lstFilesOtherFailing.TabIndex = 0;
            lstFilesOtherFailing.SelectedIndexChanged += OnIssuesSelectedIndexChanged;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonFind);
            panelButtons.Controls.Add(buttonRun);
            panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelButtons.Location = new Point(0, 956);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(278, 36);
            panelButtons.TabIndex = 4;
            // 
            // buttonFind
            // 
            buttonFind.Dock = System.Windows.Forms.DockStyle.Right;
            buttonFind.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            buttonFind.Location = new Point(203, 0);
            buttonFind.Name = "buttonFind";
            buttonFind.Size = new Size(75, 36);
            buttonFind.TabIndex = 1;
            buttonFind.Text = "Search";
            buttonFind.UseVisualStyleBackColor = true;
            buttonFind.Click += OnClickSearch;
            // 
            // buttonRun
            // 
            buttonRun.Dock = System.Windows.Forms.DockStyle.Left;
            buttonRun.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            buttonRun.Location = new Point(0, 0);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new Size(75, 36);
            buttonRun.TabIndex = 0;
            buttonRun.Text = "Run Tests";
            buttonRun.UseVisualStyleBackColor = true;
            buttonRun.Click += OnClickRunTests;
            // 
            // bottomTabBox
            // 
            bottomTabBox.Alignment = System.Windows.Forms.TabAlignment.Left;
            bottomTabBox.Controls.Add(outputTab);
            bottomTabBox.Controls.Add(descriptionTab);
            bottomTabBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            bottomTabBox.Location = new Point(0, 918);
            bottomTabBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            bottomTabBox.Multiline = true;
            bottomTabBox.Name = "bottomTabBox";
            bottomTabBox.SelectedIndex = 0;
            bottomTabBox.Size = new Size(1203, 74);
            bottomTabBox.TabIndex = 1;
            // 
            // outputTab
            // 
            outputTab.Controls.Add(boxConsoleLog);
            outputTab.Location = new Point(50, 4);
            outputTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            outputTab.Name = "outputTab";
            outputTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            outputTab.Size = new Size(1149, 66);
            outputTab.TabIndex = 0;
            outputTab.Text = "Output";
            outputTab.UseVisualStyleBackColor = true;
            // 
            // boxConsoleLog
            // 
            boxConsoleLog.BackColor = Color.White;
            boxConsoleLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            boxConsoleLog.Dock = System.Windows.Forms.DockStyle.Fill;
            boxConsoleLog.Location = new Point(4, 3);
            boxConsoleLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            boxConsoleLog.Name = "boxConsoleLog";
            boxConsoleLog.ReadOnly = true;
            boxConsoleLog.Size = new Size(1141, 60);
            boxConsoleLog.TabIndex = 1;
            boxConsoleLog.Text = "";
            boxConsoleLog.MouseDown += boxConsoleLog_MouseDown;
            // 
            // descriptionTab
            // 
            descriptionTab.Controls.Add(boxDescription);
            descriptionTab.Location = new Point(50, 4);
            descriptionTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            descriptionTab.Name = "descriptionTab";
            descriptionTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            descriptionTab.Size = new Size(1149, 66);
            descriptionTab.TabIndex = 1;
            descriptionTab.Text = "Description";
            descriptionTab.UseVisualStyleBackColor = true;
            // 
            // boxDescription
            // 
            boxDescription.BackColor = Color.White;
            boxDescription.Location = new Point(-2, -2);
            boxDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            boxDescription.Name = "boxDescription";
            boxDescription.ReadOnly = true;
            boxDescription.Size = new Size(1228, 69);
            boxDescription.TabIndex = 1;
            boxDescription.Text = "";
            // 
            // View
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(1492, 998);
            Controls.Add(splitContainerBase);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "View";
            Padding = new System.Windows.Forms.Padding(3);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SVG W3C Test Runner - Press [Ctrl + F] to search the lists";
            uniformGrid.ResumeLayout(false);
            panelSvg.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picSvg).EndInit();
            panelPng.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPng).EndInit();
            panelSaveLoad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picSaveLoad).EndInit();
            panelSVGPNG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picSVGPNG).EndInit();
            splitContainerBase.Panel1.ResumeLayout(false);
            splitContainerBase.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerBase).EndInit();
            splitContainerBase.ResumeLayout(false);
            fileTabBox.ResumeLayout(false);
            passW3CTabPage.ResumeLayout(false);
            failW3CTabPage.ResumeLayout(false);
            passOtherTabPage.ResumeLayout(false);
            failOtherTabPage.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            bottomTabBox.ResumeLayout(false);
            outputTab.ResumeLayout(false);
            descriptionTab.ResumeLayout(false);
            ResumeLayout(false);
        }


        #endregion
        private System.Windows.Forms.TableLayoutPanel uniformGrid;
        private System.Windows.Forms.SplitContainer splitContainerBase;
        private System.Windows.Forms.TabControl bottomTabBox;
        private System.Windows.Forms.TabPage outputTab;
        private System.Windows.Forms.RichTextBox boxConsoleLog;
        private System.Windows.Forms.TabPage descriptionTab;
        private System.Windows.Forms.RichTextBox boxDescription;
        private System.Windows.Forms.TabControl fileTabBox;
        private System.Windows.Forms.TabPage passW3CTabPage;
        private System.Windows.Forms.ListBox lstW3CFilesPassing;
        private System.Windows.Forms.TabPage failW3CTabPage;
        private System.Windows.Forms.ListBox lstW3CFilesFailing;
        private System.Windows.Forms.TabPage passOtherTabPage;
        private System.Windows.Forms.ListBox lstFilesOtherPassing;
        private System.Windows.Forms.TabPage failOtherTabPage;
        private System.Windows.Forms.ListBox lstFilesOtherFailing;
        private System.Windows.Forms.Panel panelSvg;
        private System.Windows.Forms.PictureBox picSvg;
        private System.Windows.Forms.Label labelSvg;
        private System.Windows.Forms.Panel panelPng;
        private System.Windows.Forms.PictureBox picPng;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelSaveLoad;
        private System.Windows.Forms.PictureBox picSaveLoad;
        private System.Windows.Forms.Label labelSaveLoad;
        private System.Windows.Forms.Panel panelSVGPNG;
        private System.Windows.Forms.Label labelSVGPNG;
        private System.Windows.Forms.PictureBox picSVGPNG;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.Button buttonRun;
    }
}

