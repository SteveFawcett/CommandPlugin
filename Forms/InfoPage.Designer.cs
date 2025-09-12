using System.Windows.Forms;

namespace CommandPlugin.Forms
{
    partial class InfoPage
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
            LogoPictureBox = new PictureBox();
            TitleLabel = new Label();
            JobListBox = new ListBox();
            jobTypes = new ComboBox();
            StdioTextBox = new RichTextBox();
            OutputTabControl = new TabControl();
            stdout = new TabPage();
            StderrTextBox = new RichTextBox();
            stderr = new TabPage();
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).BeginInit();
            OutputTabControl.SuspendLayout();
            stdout.SuspendLayout();
            stderr.SuspendLayout();
            SuspendLayout();
            // 
            // LogoPictureBox
            // 
            LogoPictureBox.Location = new Point(18, 14);
            LogoPictureBox.Name = "LogoPictureBox";
            LogoPictureBox.Size = new Size(60, 60);
            LogoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            LogoPictureBox.TabIndex = 0;
            LogoPictureBox.TabStop = false;
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TitleLabel.Location = new Point(84, 44);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(194, 30);
            TitleLabel.TabIndex = 1;
            TitleLabel.Text = "Command Executer";
            // 
            // JobListBox
            // 
            JobListBox.BackColor = Color.White;
            JobListBox.BorderStyle = BorderStyle.None;
            JobListBox.DrawMode = DrawMode.OwnerDrawFixed;
            JobListBox.Font = new Font("Segoe UI", 12F);
            JobListBox.FormattingEnabled = true;
            JobListBox.ItemHeight = 72;
            JobListBox.Location = new Point(18, 147);
            JobListBox.Margin = new Padding(0);
            JobListBox.Name = "JobListBox";
            JobListBox.Size = new Size(406, 288);
            JobListBox.TabIndex = 2;
            JobListBox.DrawItem += JobListBox_DrawItem;
            JobListBox.SelectedIndexChanged += display_results;
            // 
            // jobTypes
            // 
            jobTypes.DropDownStyle = ComboBoxStyle.DropDownList;
            jobTypes.FormattingEnabled = true;
            jobTypes.Items.AddRange(new object[] { "All", "Active", "Completed", "Failed" });
            jobTypes.Location = new Point(18, 111);
            jobTypes.Name = "jobTypes";
            jobTypes.Size = new Size(223, 23);
            jobTypes.TabIndex = 3;
            jobTypes.SelectedIndexChanged += jobTypes_SelectedIndexChanged;
            // 
            // StdioTextBox
            // 
            StdioTextBox.BorderStyle = BorderStyle.None;
            StdioTextBox.Location = new Point(0, 0);
            StdioTextBox.Name = "StdioTextBox";
            StdioTextBox.ReadOnly = true;
            StdioTextBox.Size = new Size(390, 292);
            StdioTextBox.TabIndex = 4;
            StdioTextBox.Text = "";
            // 
            // OutputTabControl
            // 
            OutputTabControl.Controls.Add(stdout);
            OutputTabControl.Controls.Add(stderr);
            OutputTabControl.Location = new Point(427, 122);
            OutputTabControl.Name = "OutputTabControl";
            OutputTabControl.SelectedIndex = 0;
            OutputTabControl.Size = new Size(394, 320);
            OutputTabControl.TabIndex = 5;
            // 
            // stdout
            // 
            stdout.BackColor = Color.IndianRed;
            stdout.Controls.Add(StdioTextBox);
            stdout.Location = new Point(4, 24);
            stdout.Name = "stdout";
            stdout.Padding = new Padding(3);
            stdout.Size = new Size(386, 292);
            stdout.TabIndex = 0;
            stdout.Text = "Standard Output";
            // 
            // StderrTextBox
            // 
            StderrTextBox.BorderStyle = BorderStyle.None;
            StderrTextBox.Location = new Point(0, 0);
            StderrTextBox.Name = "StderrTextBox";
            StderrTextBox.ReadOnly = true;
            StderrTextBox.Size = new Size(388, 296);
            StderrTextBox.TabIndex = 0;
            StderrTextBox.Text = "";
            // 
            // stderr
            // 
            stderr.BackColor = Color.PaleGreen;
            stderr.Controls.Add(StderrTextBox);
            stderr.Location = new Point(4, 24);
            stderr.Name = "stderr";
            stderr.Padding = new Padding(3);
            stderr.Size = new Size(386, 292);
            stderr.TabIndex = 1;
            stderr.Text = "Errors";
            // 
            // InfoPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(OutputTabControl);
            Controls.Add(jobTypes);
            Controls.Add(JobListBox);
            Controls.Add(TitleLabel);
            Controls.Add(LogoPictureBox);
            Name = "InfoPage";
            Size = new Size(831, 455);
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).EndInit();
            OutputTabControl.ResumeLayout(false);
            stdout.ResumeLayout(false);
            stderr.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox LogoPictureBox;
        private Label TitleLabel;
        private ListBox JobListBox;
        private ComboBox jobTypes;
        private RichTextBox StdioTextBox;
        private TabControl OutputTabControl;
        private TabPage stdout;
        private TabPage stderr;
        private RichTextBox StderrTextBox;
    }
}
