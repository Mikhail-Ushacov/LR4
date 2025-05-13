using System;
using System.Windows.Forms;
using LR4.Interfaces;
using LR4.Models;
using LR4.Services;

namespace LR4
{
    public partial class Form1 : Form
    {
        private readonly IUserService _userService;

        public Form1() : this(new UserService())
        {
        }

        public Form1(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            var user = new Models.User
            {
                Name = textBoxName.Text.Trim(),
                Surname = textBoxSurname.Text.Trim(),
                Passport = textBoxPassport.Text.Trim(),
                Password = textBoxPassword.Text.Trim()
            };

            if (!_userService.RegisterUser(user))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!", "Помилка", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Реєстрація успішна!", "Готово", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear fields after registration
            textBoxName.Clear();
            textBoxSurname.Clear();
            textBoxPassport.Clear();
            textBoxPassword.Clear();
        }
    }
}
