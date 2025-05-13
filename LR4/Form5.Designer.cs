namespace LR4
{
    partial class Form5
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Panel panelCandidates;
        private System.Windows.Forms.Button buttonVote;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelInfo = new System.Windows.Forms.Label();
            this.panelCandidates = new System.Windows.Forms.Panel();
            this.buttonVote = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // labelInfo
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(12, 15);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(320, 16);
            this.labelInfo.Text = "Інформація про користувача";

            // panelCandidates
            this.panelCandidates.AutoScroll = true;
            this.panelCandidates.Location = new System.Drawing.Point(15, 45);
            this.panelCandidates.Name = "panelCandidates";
            this.panelCandidates.Size = new System.Drawing.Size(340, 200);

            // buttonVote
            this.buttonVote.Location = new System.Drawing.Point(120, 260);
            this.buttonVote.Name = "buttonVote";
            this.buttonVote.Size = new System.Drawing.Size(130, 35);
            this.buttonVote.Text = "Проголосувати";
            this.buttonVote.UseVisualStyleBackColor = true;
            this.buttonVote.Click += new System.EventHandler(this.buttonVote_Click);

            // Form5
            this.ClientSize = new System.Drawing.Size(380, 310);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.panelCandidates);
            this.Controls.Add(this.buttonVote);
            this.Name = "Form5";
            this.Text = "Голосування";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
