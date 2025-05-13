using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LR4
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
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

            // Перевірка адміністратора
            if (passportInput == "admin" && passwordInput == "1234")
            {
                MessageBox.Show("Вхід як адміністратор успішний!", "Адмін", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide(); // Можна замінити на this.Close(); якщо потрібно закрити поточну форму
                return;
            }

            // Перевірка звичайного користувача
            if (!File.Exists("verified.txt"))
            {
                MessageBox.Show("Файл verified.txt не знайдено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines("verified.txt");
            bool isMatch = false;

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length < 4) continue;

                string name = parts[0];
                string surname = parts[1];
                string passport = parts[2];
                string password = parts[3];

                if (passport == passportInput && password == passwordInput)
                {
                    File.AppendAllText("active.txt", line + Environment.NewLine);
                    MessageBox.Show($"Вхід виконано! Вітаємо, {name} {surname}", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    isMatch = true;
                    break;
                }
            }

            if (!isMatch)
            {
                MessageBox.Show("Невірний номер паспорта або пароль.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            textBoxPassport.Clear();
            textBoxPassword.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Робить поле пароля прихованим
            textBoxPassword.PasswordChar = '*';
        }
    }
}
