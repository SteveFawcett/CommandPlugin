using BroadcastPluginSDK.Classes;
using Command.Renderers;
using CyberDog.Controls;

namespace Command.Forms
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
            JobListBox = new ListPanel<CommandItem>( new CommandRenderer<CommandItem>() );
            jobTypes = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).BeginInit();
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
            TitleLabel.Text = "Value Executer";
            // 
            // JobListBox
            // 
            JobListBox.BackColor = Color.White;
            JobListBox.Location = new Point(18, 147);
            JobListBox.Margin = new Padding(0);
            JobListBox.Name = "JobListBox";
            JobListBox.Size = new Size(793, 288);
            JobListBox.TabIndex = 2;
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
            // InfoPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(jobTypes);
            Controls.Add(JobListBox);
            Controls.Add(TitleLabel);
            Controls.Add(LogoPictureBox);
            Name = "InfoPage";
            Size = new Size(831, 455);
            ((System.ComponentModel.ISupportInitialize)LogoPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox LogoPictureBox;
        private Label TitleLabel;
        private ListPanel<CommandItem> JobListBox;
        private ComboBox jobTypes;
    }
}
