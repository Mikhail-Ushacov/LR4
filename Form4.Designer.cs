namespace LR4
{
    partial class Form4
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBoxCandidates;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.Label labelTimeLeft;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.Button buttonRules;
        private System.Windows.Forms.CheckBox checkBoxAgree;
        private System.Windows.Forms.Button buttonVote;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBoxCandidates = new System.Windows.Forms.ListBox();
            this.buttonResults = new System.Windows.Forms.Button();
            this.labelStage = new System.Windows.Forms.Label();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.labelTimeLeft = new System.Windows.Forms.Label();
            this.buttonRules = new System.Windows.Forms.Button();
            this.checkBoxAgree = new System.Windows.Forms.CheckBox();
            this.buttonVote = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxCandidates);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.buttonResults);
            this.splitContainer1.Panel2.Controls.Add(this.labelStage);
            this.splitContainer1.Panel2.Controls.Add(this.labelStartDate);
            this.splitContainer1.Panel2.Controls.Add(this.labelEndDate);
            this.splitContainer1.Panel2.Controls.Add(this.labelTimeLeft);
            this.splitContainer1.Panel2.Controls.Add(this.buttonRules);
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxAgree);
            this.splitContainer1.Panel2.Controls.Add(this.buttonVote);
            this.splitContainer1.Panel2.Controls.Add(this.button1);
            this.splitContainer1.Size = new System.Drawing.Size(600, 400);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 0;
            // 
            // listBoxCandidates
            // 
            this.listBoxCandidates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCandidates.FormattingEnabled = true;
            this.listBoxCandidates.Location = new System.Drawing.Point(0, 0);
            this.listBoxCandidates.Name = "listBoxCandidates";
            this.listBoxCandidates.Size = new System.Drawing.Size(300, 400);
            this.listBoxCandidates.TabIndex = 0;
            // 
            // buttonResults
            // 
            this.buttonResults.Location = new System.Drawing.Point(13, 349);
            this.buttonResults.Name = "buttonResults";
            this.buttonResults.Size = new System.Drawing.Size(115, 35);
            this.buttonResults.TabIndex = 8;
            this.buttonResults.Text = "Результати";
            this.buttonResults.UseVisualStyleBackColor = true;
            this.buttonResults.Click += new System.EventHandler(this.ButtonResults_Click);
            // 
            // labelStage
            // 
            this.labelStage.AutoSize = true;
            this.labelStage.Location = new System.Drawing.Point(10, 4);
            this.labelStage.Name = "labelStage";
            this.labelStage.Size = new System.Drawing.Size(35, 13);
            this.labelStage.TabIndex = 7;
            this.labelStage.Text = "label1";
            // 
            // labelStartDate
            // 
            this.labelStartDate.AutoSize = true;
            this.labelStartDate.Location = new System.Drawing.Point(10, 20);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(0, 13);
            this.labelStartDate.TabIndex = 0;
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(10, 35);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(0, 13);
            this.labelEndDate.TabIndex = 6;
            // 
            // labelTimeLeft
            // 
            this.labelTimeLeft.AutoSize = true;
            this.labelTimeLeft.Location = new System.Drawing.Point(10, 50);
            this.labelTimeLeft.Name = "labelTimeLeft";
            this.labelTimeLeft.Size = new System.Drawing.Size(0, 13);
            this.labelTimeLeft.TabIndex = 1;
            // 
            // buttonRules
            // 
            this.buttonRules.Location = new System.Drawing.Point(10, 90);
            this.buttonRules.Name = "buttonRules";
            this.buttonRules.Size = new System.Drawing.Size(130, 30);
            this.buttonRules.TabIndex = 2;
            this.buttonRules.Text = "Правила";
            this.buttonRules.UseVisualStyleBackColor = true;
            this.buttonRules.Click += new System.EventHandler(this.buttonRules_Click);
            // 
            // checkBoxAgree
            // 
            this.checkBoxAgree.Location = new System.Drawing.Point(10, 140);
            this.checkBoxAgree.Name = "checkBoxAgree";
            this.checkBoxAgree.Size = new System.Drawing.Size(220, 20);
            this.checkBoxAgree.TabIndex = 3;
            this.checkBoxAgree.Text = "Погоджуюсь з умовами";
            this.checkBoxAgree.CheckedChanged += new System.EventHandler(this.checkBoxAgree_CheckedChanged);
            // 
            // buttonVote
            // 
            this.buttonVote.Enabled = false;
            this.buttonVote.Location = new System.Drawing.Point(10, 180);
            this.buttonVote.Name = "buttonVote";
            this.buttonVote.Size = new System.Drawing.Size(150, 30);
            this.buttonVote.TabIndex = 4;
            this.buttonVote.Text = "Почати голосування";
            this.buttonVote.UseVisualStyleBackColor = true;
            this.buttonVote.Click += new System.EventHandler(this.buttonVote_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(153, 349);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 35);
            this.button1.TabIndex = 5;
            this.button1.Text = "Увійти";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form4
            // 
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form4";
            this.Text = "Голосування - Підготовка";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label labelStage;
        private System.Windows.Forms.Button buttonResults;
    }
}
