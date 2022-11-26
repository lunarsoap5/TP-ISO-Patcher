namespace RandomizerPatchApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.isoTBox = new System.Windows.Forms.TextBox();
            this.PatchButton = new System.Windows.Forms.Button();
            this.isoLabel = new System.Windows.Forms.Label();
            this.outputTBox = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.isoButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // isoTBox
            // 
            this.isoTBox.Location = new System.Drawing.Point(147, 344);
            this.isoTBox.Name = "isoTBox";
            this.isoTBox.Size = new System.Drawing.Size(454, 23);
            this.isoTBox.TabIndex = 2;
            // 
            // PatchButton
            // 
            this.PatchButton.Location = new System.Drawing.Point(343, 373);
            this.PatchButton.Name = "PatchButton";
            this.PatchButton.Size = new System.Drawing.Size(90, 23);
            this.PatchButton.TabIndex = 3;
            this.PatchButton.Text = "Patch Now!";
            this.PatchButton.UseVisualStyleBackColor = true;
            this.PatchButton.Click += new System.EventHandler(this.PatchButton_Click);
            // 
            // isoLabel
            // 
            this.isoLabel.AutoSize = true;
            this.isoLabel.Location = new System.Drawing.Point(89, 347);
            this.isoLabel.Name = "isoLabel";
            this.isoLabel.Size = new System.Drawing.Size(55, 15);
            this.isoLabel.TabIndex = 6;
            this.isoLabel.Text = "ISO Path:";
            // 
            // outputTBox
            // 
            this.outputTBox.Location = new System.Drawing.Point(89, 37);
            this.outputTBox.Multiline = true;
            this.outputTBox.Name = "outputTBox";
            this.outputTBox.ReadOnly = true;
            this.outputTBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputTBox.Size = new System.Drawing.Size(593, 274);
            this.outputTBox.TabIndex = 7;
            this.outputTBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // isoButton
            // 
            this.isoButton.Location = new System.Drawing.Point(607, 344);
            this.isoButton.Name = "isoButton";
            this.isoButton.Size = new System.Drawing.Size(75, 23);
            this.isoButton.TabIndex = 10;
            this.isoButton.Text = "Browse";
            this.isoButton.UseVisualStyleBackColor = true;
            this.isoButton.Click += new System.EventHandler(this.isoButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.isoButton);
            this.Controls.Add(this.outputTBox);
            this.Controls.Add(this.isoLabel);
            this.Controls.Add(this.PatchButton);
            this.Controls.Add(this.isoTBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "TPR ISO Patcher";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox isoTBox;
        private Button PatchButton;
        private Label isoLabel;
        private TextBox outputTBox;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button isoButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
    }
}