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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (!File.Exists("user.txt"))
            {
                MessageBox.Show("Файл user.txt не знайдено.");
                return;
            }

            string[] lines = File.ReadAllLines("user.txt");
            int y = 10;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(';');
                if (parts.Length < 3) continue;

                string formattedText = $"Ім'я: {parts[0]} | Прізвище: {parts[1]} | Номер паспорта: {parts[2]}";

                Label lbl = new Label();
                lbl.Text = formattedText;
                lbl.AutoSize = true;
                lbl.Location = new System.Drawing.Point(10, y);

                Button btn = new Button();
                btn.Text = "Підтвердити";
                btn.Tag = line; // зберігаємо оригінальний рядок
                btn.Location = new System.Drawing.Point(400, y - 3);
                btn.Click += BtnConfirm_Click;

                this.panel1.Controls.Add(lbl);
                this.panel1.Controls.Add(btn);

                y += 30;
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string line = btn.Tag.ToString();

            // Додаємо у verified.txt
            File.AppendAllText("verified.txt", line + Environment.NewLine);

            // Видаляємо з user.txt
            var allLines = File.ReadAllLines("user.txt").ToList();
            allLines.Remove(line);
            File.WriteAllLines("user.txt", allLines);

            // Видаляємо з інтерфейсу
            Label labelToRemove = null;
            foreach (Control control in this.panel1.Controls)
            {
                if (control is Label label && label.Text == GetFormattedLine(line))
                {
                    labelToRemove = label;
                    break;
                }
            }

            this.panel1.Controls.Remove(labelToRemove);
            this.panel1.Controls.Remove(btn);

            MessageBox.Show("Дані підтверджено і видалено з очікування!", "Готово");

            // Якщо всі підтверджені — повідомити або закрити
            if (this.panel1.Controls.Count == 0)
            {
                MessageBox.Show("Усі записи оброблено.");
                this.Close();
            }
        }

        private string GetFormattedLine(string line)
        {
            string[] parts = line.Split(';');
            if (parts.Length < 3) return "";

            return $"Ім'я: {parts[0]} | Прізвище: {parts[1]} | Номер паспорта: {parts[2]}";
        }
    }
}
