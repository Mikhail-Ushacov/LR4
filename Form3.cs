using System;
using System.Windows.Forms;
using LR4.Interfaces;

namespace LR4
{
    public partial class Form3 : Form
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly IVotingService _votingService;
        private readonly IVoteService _voteService;

        public Form3(IUserService userService, IAdminService adminService, IVotingService votingService, IVoteService voteService)
        {
            InitializeComponent();
            _userService = userService;
            _adminService = adminService;
            _votingService = votingService;
            _voteService = voteService;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string passportInput = textBoxPassport.Text.Trim();
            string passwordInput = textBoxPassword.Text.Trim();

            if (passportInput == "" || passwordInput == "")
            {
                MessageBox.Show("Будь ласка, заповніть обидва поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_userService.AuthenticateAdmin(passportInput, passwordInput))
            {
                MessageBox.Show("Вхід як адміністратор успішний!", "Адмін", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form2 form2 = new Form2(_adminService);
                form2.Show();
                this.Hide();
                return;
            }

            if (_userService.AuthenticateUser(passportInput, passwordInput))
            {
                _userService.ActivateUser(passportInput);
                MessageBox.Show("Вхід виконано!", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Невірний номер паспорта або пароль.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            textBoxPassport.Clear();
            textBoxPassword.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(_userService);
            form1.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = '*';
        }
    }
}
