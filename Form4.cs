using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LR4
{
    public partial class Form4 : Form
    {
        private DateTime startTime; // Початок голосування
        private DateTime endTime;   // Кінець голосування
        private string currentStage; // Поточний етап

        public Form4()
        {
            InitializeComponent();
            LoadCandidates();
            LoadStages(); // Загрузка етапів з файлу

            startTime = new DateTime(2025, 4, 1, 12, 0, 0); // Початок голосування
            endTime = startTime.AddHours(1);   // Кінець голосування

            labelStartDate.Text = "Дата початку: " + startTime.ToString("dd.MM.yyyy HH:mm:ss");
            labelEndDate.Text = "Дата завершення: " + endTime.ToString("dd.MM.yyyy HH:mm:ss");

            labelStage.Text = "Поточний етап: " + currentStage; // Вивести поточний етап на лейблі

            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        private void LoadCandidates()
        {
            if (File.Exists("candidates_company.txt"))
            {
                string[] lines = File.ReadAllLines("candidates_company.txt");
                listBoxCandidates.Items.AddRange(lines);
            }
            else
            {
                listBoxCandidates.Items.Add("Файл candidates_company.txt не знайдено.");
            }
        }

        private void LoadStages()
        {
            if (File.Exists("stages.txt"))
            {
                string[] lines = File.ReadAllLines("stages.txt");
                if (lines.Length > 0)
                {
                    currentStage = lines[0];
                }
                else
                {
                    currentStage = "Не визначено"; // Якщо файл порожній або неправильний формат
                }
            }
            else
            {
                currentStage = "Файл stages.txt не знайдено."; // Якщо файл не знайдений
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            if (now < startTime)
            {
                TimeSpan untilStart = startTime - now;
                labelTimeLeft.Text = $"Залишилось до початку: {untilStart:dd\\:hh\\:mm\\:ss}";
            }
            else if (now >= startTime && now < endTime)
            {
                TimeSpan untilEnd = endTime - now;
                labelTimeLeft.Text = $"До завершення голосування: {untilEnd:dd\\:hh\\:mm\\:ss}";
            }
            else
            {
                labelTimeLeft.Text = "Голосування завершено!";
                buttonVote.Enabled = false;

                OpenForm6();
            }

            CheckIfCanVote();
        }

        private void OpenForm6()
        {
            // Перевіряємо, чи форма ще не відкрита
            if (Application.OpenForms["Form6"] == null)
            {
                Form6 form6 = new Form6();
                form6.Show();
            }
        }

        private void CheckIfCanVote()
        {
            DateTime now = DateTime.Now;
            if (checkBoxAgree.Checked && now >= startTime && now < endTime)
                buttonVote.Enabled = true;
            else
                buttonVote.Enabled = false;
        }

        private void buttonRules_Click(object sender, EventArgs e)
        {
            if (File.Exists("rules.txt"))
            {
                string rules = File.ReadAllText("rules.txt");
                MessageBox.Show(rules, "Правила голосування", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Файл rules.txt не знайдено.");
            }
        }

        private void checkBoxAgree_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfCanVote();
        }

        private void buttonVote_Click(object sender, EventArgs e)
        {
            if (!File.Exists("active.txt"))
            {
                MessageBox.Show("Файл active.txt не знайдено.");
                return;
            }

            string[] users = File.ReadAllLines("active.txt");
            foreach (string userLine in users)
            {
                if (!string.IsNullOrWhiteSpace(userLine))
                {
                    Form5 voteForm = new Form5(userLine);
                    voteForm.ShowDialog();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }
}
