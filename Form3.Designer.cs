namespace LR4
{
    partial class Form3
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelPassport;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassport;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonLogin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelPassport = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassport = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // labelPassport
            this.labelPassport.AutoSize = true;
            this.labelPassport.Location = new System.Drawing.Point(30, 30);
            this.labelPassport.Name = "labelPassport";
            this.labelPassport.Size = new System.Drawing.Size(92, 13);
            this.labelPassport.Text = "Номер паспорта:";

            // textBoxPassport
            this.textBoxPassport.Location = new System.Drawing.Point(150, 27);
            this.textBoxPassport.Name = "textBoxPassport";
            this.textBoxPassport.Size = new System.Drawing.Size(160, 20);

            // labelPassword
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(30, 70);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(45, 13);
            this.labelPassword.Text = "Пароль:";

            // textBoxPassword
            this.textBoxPassword.Location = new System.Drawing.Point(150, 67);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(160, 20);
            this.textBoxPassword.UseSystemPasswordChar = true;

            // buttonLogin
            this.buttonLogin.Location = new System.Drawing.Point(110, 110);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(100, 30);
            this.buttonLogin.Text = "Увійти";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);

            // Form3
            this.ClientSize = new System.Drawing.Size(340, 170);
            this.Controls.Add(this.labelPassport);
            this.Controls.Add(this.textBoxPassport);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.buttonLogin);
            this.Name = "Form3";
            this.Text = "Вхід користувача";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
