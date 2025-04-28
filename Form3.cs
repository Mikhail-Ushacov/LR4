using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LR4
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 registrationForm = new Form1();
            registrationForm.ShowDialog();
            this.Show();
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

            if (!File.Exists("verified.txt"))
            {
                MessageBox.Show("Файл verified.txt не знайдено.");
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
                    
                    // Check role and open appropriate form
                    this.Hide();
                    if (name == "John" && surname == "Doe") // Admin
                    {
                        new Form2().ShowDialog();
                    }
                    else // Regular user
                    {
                        new Form5(line).ShowDialog();
                    }
                    this.Close();
                    break;
                }
            }

            if (!isMatch)
            {
                MessageBox.Show("Невірний номер паспорта або пароль.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxPassport.Clear();
                textBoxPassword.Clear();
            }
        }
    }
}
