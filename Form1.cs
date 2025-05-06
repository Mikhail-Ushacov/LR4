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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text.Trim();
            string surname = textBoxSurname.Text.Trim();
            string passport = textBoxPassport.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (name == "" || surname == "" || passport == "" || password == "")
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userData = $"{name};{surname};{passport};{password}";

            File.AppendAllText("user.txt", userData + Environment.NewLine);

            MessageBox.Show("Реєстрація успішна!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Очистити поля після реєстрації
            textBoxName.Clear();
            textBoxSurname.Clear();
            textBoxPassport.Clear();
            textBoxPassword.Clear();
        }
    }
}