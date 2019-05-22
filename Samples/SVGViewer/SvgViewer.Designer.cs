namespace SVGViewer
{
    partial class SVGViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVGViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.open = new System.Windows.Forms.ToolStripButton();
            this.svgImage = new System.Windows.Forms.PictureBox();
            this.openSvgFile = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.svgImage)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1060, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // open
            // 
            this.open.Image = ((System.Drawing.Image)(resources.GetObject("open.Image")));
            this.open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(56, 22);
            this.open.Text = "Open";
            this.open.Click += new System.EventHandler(this.Open_Click);
            // 
            // svgImage
            // 
            this.svgImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.svgImage.Location = new System.Drawing.Point(0, 25);
            this.svgImage.Name = "svgImage";
            this.svgImage.Size = new System.Drawing.Size(735, 449);
            this.svgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.svgImage.TabIndex = 1;
            this.svgImage.TabStop = false;
            // 
            // openSvgFile
            // 
            this.openSvgFile.Filter = "Vector Graphics (*.svg)|*.svg";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(741, 25);
            this.textBox1.MaxLength = 327670;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(307, 446);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox1_KeyDown);
            // 
            // SVGViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 474);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.svgImage);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SVGViewer";
            this.Text = "SVG Viewer";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.svgImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton open;
        private System.Windows.Forms.PictureBox svgImage;
        private System.Windows.Forms.OpenFileDialog openSvgFile;
        private System.Windows.Forms.TextBox textBox1;
    }
}

