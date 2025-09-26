namespace Command.Forms
{
    partial class Results
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
            OutputText = new RichTextBox();
            button1 = new Button();
            SuspendLayout();
            // 
            // OutputText
            // 
            OutputText.BorderStyle = BorderStyle.None;
            OutputText.Location = new Point(7, 5);
            OutputText.Name = "OutputText";
            OutputText.ReadOnly = true;
            OutputText.Size = new Size(790, 397);
            OutputText.TabIndex = 0;
            OutputText.Text = "";
            // 
            // button1
            // 
            button1.Location = new Point(662, 408);
            button1.Name = "button1";
            button1.Size = new Size(135, 30);
            button1.TabIndex = 1;
            button1.Text = "Close";
            button1.UseVisualStyleBackColor = true;
            // 
            // Results
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(OutputText);
            Name = "Results";
            Text = "Results";
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        public RichTextBox OutputText;
    }
}