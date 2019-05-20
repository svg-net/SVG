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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.picSaveLoad = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picSvg = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.picSVGPNG = new System.Windows.Forms.PictureBox();
            this.picPng = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bottomTabBox = new System.Windows.Forms.TabControl();
            this.outputTab = new System.Windows.Forms.TabPage();
            this.boxConsoleLog = new System.Windows.Forms.RichTextBox();
            this.descriptionTab = new System.Windows.Forms.TabPage();
            this.boxDescription = new System.Windows.Forms.RichTextBox();
            this.fileTabBox = new System.Windows.Forms.TabControl();
            this.passW3CTabPage = new System.Windows.Forms.TabPage();
            this.lstW3CFilesPassing = new System.Windows.Forms.ListBox();
            this.failW3CTabPage = new System.Windows.Forms.TabPage();
            this.lstW3CFilesFailing = new System.Windows.Forms.ListBox();
            this.passOtherTabPage = new System.Windows.Forms.TabPage();
            this.lstFilesOtherPassing = new System.Windows.Forms.ListBox();
            this.failOtherTabPage = new System.Windows.Forms.TabPage();
            this.lstFilesOtherFailing = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSaveLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSvg)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSVGPNG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPng)).BeginInit();
            this.bottomTabBox.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.descriptionTab.SuspendLayout();
            this.fileTabBox.SuspendLayout();
            this.passW3CTabPage.SuspendLayout();
            this.failW3CTabPage.SuspendLayout();
            this.passOtherTabPage.SuspendLayout();
            this.failOtherTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.bottomTabBox, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1279, 865);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(183, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.splitContainer1.Size = new System.Drawing.Size(1093, 789);
            this.splitContainer1.SplitterDistance = 563;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.picSaveLoad, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.picSvg, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(563, 789);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // picSaveLoad
            // 
            this.picSaveLoad.BackColor = System.Drawing.Color.White;
            this.picSaveLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picSaveLoad.Location = new System.Drawing.Point(0, 407);
            this.picSaveLoad.Margin = new System.Windows.Forms.Padding(0);
            this.picSaveLoad.Name = "picSaveLoad";
            this.picSaveLoad.Size = new System.Drawing.Size(563, 382);
            this.picSaveLoad.TabIndex = 2;
            this.picSaveLoad.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SVG Render";
            // 
            // picSvg
            // 
            this.picSvg.BackColor = System.Drawing.Color.White;
            this.picSvg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picSvg.Location = new System.Drawing.Point(0, 13);
            this.picSvg.Margin = new System.Windows.Forms.Padding(0);
            this.picSvg.Name = "picSvg";
            this.picSvg.Size = new System.Drawing.Size(563, 381);
            this.picSvg.TabIndex = 1;
            this.picSvg.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 394);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Save and Load";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.picSVGPNG, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.picPng, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(526, 789);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // picSVGPNG
            // 
            this.picSVGPNG.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picSVGPNG.BackColor = System.Drawing.Color.White;
            this.picSVGPNG.Location = new System.Drawing.Point(0, 407);
            this.picSVGPNG.Margin = new System.Windows.Forms.Padding(0);
            this.picSVGPNG.Name = "picSVGPNG";
            this.picSVGPNG.Size = new System.Drawing.Size(526, 370);
            this.picSVGPNG.TabIndex = 3;
            this.picSVGPNG.TabStop = false;
            // 
            // picPng
            // 
            this.picPng.BackColor = System.Drawing.Color.White;
            this.picPng.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPng.Location = new System.Drawing.Point(0, 13);
            this.picPng.Margin = new System.Windows.Forms.Padding(0);
            this.picPng.Name = "picPng";
            this.picPng.Size = new System.Drawing.Size(526, 381);
            this.picPng.TabIndex = 2;
            this.picPng.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Reference PNG";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 394);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "SVG vs PNG";
            // 
            // bottomTabBox
            // 
            this.bottomTabBox.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.bottomTabBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomTabBox.Controls.Add(this.outputTab);
            this.bottomTabBox.Controls.Add(this.descriptionTab);
            this.bottomTabBox.Location = new System.Drawing.Point(183, 798);
            this.bottomTabBox.Multiline = true;
            this.bottomTabBox.Name = "bottomTabBox";
            this.bottomTabBox.SelectedIndex = 0;
            this.bottomTabBox.Size = new System.Drawing.Size(1093, 64);
            this.bottomTabBox.TabIndex = 1;
            // 
            // outputTab
            // 
            this.outputTab.Controls.Add(this.boxConsoleLog);
            this.outputTab.Location = new System.Drawing.Point(42, 4);
            this.outputTab.Name = "outputTab";
            this.outputTab.Padding = new System.Windows.Forms.Padding(3);
            this.outputTab.Size = new System.Drawing.Size(1047, 56);
            this.outputTab.TabIndex = 0;
            this.outputTab.Text = "Output";
            this.outputTab.UseVisualStyleBackColor = true;
            // 
            // boxConsoleLog
            // 
            this.boxConsoleLog.BackColor = System.Drawing.Color.White;
            this.boxConsoleLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.boxConsoleLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.boxConsoleLog.Location = new System.Drawing.Point(3, 3);
            this.boxConsoleLog.Name = "boxConsoleLog";
            this.boxConsoleLog.ReadOnly = true;
            this.boxConsoleLog.Size = new System.Drawing.Size(1041, 50);
            this.boxConsoleLog.TabIndex = 1;
            this.boxConsoleLog.Text = "";
            this.boxConsoleLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.boxConsoleLog_MouseDown);
            // 
            // descriptionTab
            // 
            this.descriptionTab.Controls.Add(this.boxDescription);
            this.descriptionTab.Location = new System.Drawing.Point(42, 4);
            this.descriptionTab.Name = "descriptionTab";
            this.descriptionTab.Padding = new System.Windows.Forms.Padding(3);
            this.descriptionTab.Size = new System.Drawing.Size(1047, 56);
            this.descriptionTab.TabIndex = 1;
            this.descriptionTab.Text = "Description";
            this.descriptionTab.UseVisualStyleBackColor = true;
            // 
            // boxDescription
            // 
            this.boxDescription.BackColor = System.Drawing.Color.White;
            this.boxDescription.Location = new System.Drawing.Point(-2, -2);
            this.boxDescription.Name = "boxDescription";
            this.boxDescription.ReadOnly = true;
            this.boxDescription.Size = new System.Drawing.Size(1053, 60);
            this.boxDescription.TabIndex = 1;
            this.boxDescription.Text = "";
            // 
            // fileTabBox
            // 
            this.fileTabBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fileTabBox.Controls.Add(this.passW3CTabPage);
            this.fileTabBox.Controls.Add(this.failW3CTabPage);
            this.fileTabBox.Controls.Add(this.passOtherTabPage);
            this.fileTabBox.Controls.Add(this.failOtherTabPage);
            this.fileTabBox.Location = new System.Drawing.Point(0, 24);
            this.fileTabBox.Name = "fileTabBox";
            this.fileTabBox.SelectedIndex = 0;
            this.fileTabBox.Size = new System.Drawing.Size(180, 841);
            this.fileTabBox.TabIndex = 3;
            this.fileTabBox.ItemSize = new System.Drawing.Size(40, 40);
            this.fileTabBox.SelectedIndexChanged += new System.EventHandler(this.fileTabBox_TabIndexChanged);
            // 
            // passW3CTabPage
            // 
            this.passW3CTabPage.Controls.Add(this.lstW3CFilesPassing);
            this.passW3CTabPage.Location = new System.Drawing.Point(4, 22);
            this.passW3CTabPage.Name = "passW3CTabPage";
            this.passW3CTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.passW3CTabPage.Size = new System.Drawing.Size(172, 815);
            this.passW3CTabPage.TabIndex = 0;
            this.passW3CTabPage.Text = "Pass\nW3C";
            this.passW3CTabPage.UseVisualStyleBackColor = true;
            // 
            // lstW3CFilesPassing
            // 
            this.lstW3CFilesPassing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstW3CFilesPassing.FormattingEnabled = true;
            this.lstW3CFilesPassing.Location = new System.Drawing.Point(3, 3);
            this.lstW3CFilesPassing.Name = "lstW3CFilesPassing";
            this.lstW3CFilesPassing.Size = new System.Drawing.Size(166, 797);
            this.lstW3CFilesPassing.TabIndex = 1;
            this.lstW3CFilesPassing.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            // 
            // failW3CTabPage
            // 
            this.failW3CTabPage.Controls.Add(this.lstW3CFilesFailing);
            this.failW3CTabPage.Location = new System.Drawing.Point(4, 22);
            this.failW3CTabPage.Name = "failW3CTabPage";
            this.failW3CTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.failW3CTabPage.Size = new System.Drawing.Size(172, 815);
            this.failW3CTabPage.TabIndex = 1;
            this.failW3CTabPage.Text = "Fail\nW3C";
            this.failW3CTabPage.UseVisualStyleBackColor = true;
            // 
            // lstW3CFilesFailing
            // 
            this.lstW3CFilesFailing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstW3CFilesFailing.FormattingEnabled = true;
            this.lstW3CFilesFailing.Location = new System.Drawing.Point(3, 4);
            this.lstW3CFilesFailing.Name = "lstW3CFilesFailing";
            this.lstW3CFilesFailing.Size = new System.Drawing.Size(169, 810);
            this.lstW3CFilesFailing.TabIndex = 0;
            this.lstW3CFilesFailing.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            // 
            // passOtherTabPage
            // 
            this.passOtherTabPage.Controls.Add(this.lstFilesOtherPassing);
            this.passOtherTabPage.Location = new System.Drawing.Point(4, 22);
            this.passOtherTabPage.Name = "passOtherTabPage";
            this.passOtherTabPage.Size = new System.Drawing.Size(172, 815);
            this.passOtherTabPage.TabIndex = 2;
            this.passOtherTabPage.Text = "Pass\nOther";
            this.passOtherTabPage.UseVisualStyleBackColor = true;
            // 
            // lstFilesOtherPassing
            // 
            this.lstFilesOtherPassing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstFilesOtherPassing.FormattingEnabled = true;
            this.lstFilesOtherPassing.Location = new System.Drawing.Point(0, -2);
            this.lstFilesOtherPassing.Name = "lstFilesOtherPassing";
            this.lstFilesOtherPassing.Size = new System.Drawing.Size(172, 810);
            this.lstFilesOtherPassing.TabIndex = 0;
            this.lstFilesOtherPassing.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            // 
            // failOtherTabPage
            // 
            this.failOtherTabPage.Controls.Add(this.lstFilesOtherFailing);
            this.failOtherTabPage.Location = new System.Drawing.Point(4, 22);
            this.failOtherTabPage.Name = "failOtherTabPage";
            this.failOtherTabPage.Size = new System.Drawing.Size(172, 815);
            this.failOtherTabPage.TabIndex = 2;
            this.failOtherTabPage.Text = "Fail\nOther";
            this.failOtherTabPage.UseVisualStyleBackColor = true;
            // 
            // lstFilesOtherFailing
            // 
            this.lstFilesOtherFailing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstFilesOtherFailing.FormattingEnabled = true;
            this.lstFilesOtherFailing.Location = new System.Drawing.Point(0, -2);
            this.lstFilesOtherFailing.Name = "lstFilesOtherFailing";
            this.lstFilesOtherFailing.Size = new System.Drawing.Size(172, 810);
            this.lstFilesOtherFailing.TabIndex = 0;
            this.lstFilesOtherFailing.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 865);
            this.Controls.Add(this.fileTabBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "View";
            this.Text = "SVG W3C Test Runner";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSaveLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSvg)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSVGPNG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPng)).EndInit();
            this.bottomTabBox.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.descriptionTab.ResumeLayout(false);
            this.fileTabBox.ResumeLayout(false);
            this.passW3CTabPage.ResumeLayout(false);
            this.failW3CTabPage.ResumeLayout(false);
            this.passOtherTabPage.ResumeLayout(false);
            this.failOtherTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picSvg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.PictureBox picPng;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picSaveLoad;
        private System.Windows.Forms.PictureBox picSVGPNG;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
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
    }
}

