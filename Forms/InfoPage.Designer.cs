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
            pictureBox1 = new PictureBox();
            label1 = new Label();
            JobListBox = new ListBox();
            jobTypes = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(18, 14);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 60);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(84, 44);
            label1.Name = "label1";
            label1.Size = new Size(194, 30);
            label1.TabIndex = 1;
            label1.Text = "Command Executer";
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
            JobListBox.Size = new Size(802, 288);
            JobListBox.TabIndex = 2;
            JobListBox.DrawItem += JobListBox_DrawItem;
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
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "InfoPage";
            Size = new Size(831, 455);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private ListBox JobListBox;
        private ComboBox jobTypes;
    }
}
