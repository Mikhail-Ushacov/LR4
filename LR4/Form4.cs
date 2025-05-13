// Form4.cs
using System;
using System.IO;
using System.Windows.Forms;

namespace LR4
{
    public partial class Form4 : Form
    {
        private VotingManager votingManager;

        public Form4()
        {
            InitializeComponent();
            votingManager = new VotingManager();

            votingManager.LoadCandidates(listBoxCandidates);
            votingManager.LoadStages(); // Загрузка етапів з файлу

            labelStartDate.Text = "Дата початку: " + votingManager.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
            labelEndDate.Text = "Дата завершення: " + votingManager.EndTime.ToString("dd.MM.yyyy HH:mm:ss");
            labelStage.Text = "Поточний етап: " + votingManager.CurrentStage;

            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            buttonResults.Enabled = false;

            if (now < votingManager.StartTime)
            {
                TimeSpan untilStart = votingManager.StartTime - now;
                labelTimeLeft.Text = $"Залишилось до початку: {untilStart:dd\\:hh\\:mm\\:ss}";
            }
            else if (now >= votingManager.StartTime && now < votingManager.EndTime)
            {
                TimeSpan untilEnd = votingManager.EndTime - now;
                labelTimeLeft.Text = $"До завершення голосування: {untilEnd:dd\\:hh\\:mm\\:ss}";
            }
            else
            {
                labelTimeLeft.Text = "Голосування завершено!";
                buttonVote.Enabled = false;
                buttonResults.Enabled = true;
            }

            // Перевірка: якщо залишився один кандидат — оголосити переможця
            string candidatesFile = "candidates.txt";
            if (File.Exists(candidatesFile))
            {
                string[] remainingCandidates = File.ReadAllLines(candidatesFile);
                if (remainingCandidates.Length == 1)
                {
                    string winner = remainingCandidates[0].Trim();
                    labelTimeLeft.Text = $"Голосування завершено! Переміг {winner}";
                    buttonVote.Enabled = false;
                    buttonResults.Enabled = true;
                    timer1.Stop(); // Зупиняємо таймер, оскільки голосування завершене
                }
            }

            votingManager.CheckIfCanVote(checkBoxAgree, buttonVote, buttonResults, listBoxCandidates);
        }

        private void ButtonResults_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form6"] == null)
            {
                Form6 form6 = new Form6();
                form6.Show();
            }
        }

        private void checkBoxAgree_CheckedChanged(object sender, EventArgs e)
        {
            votingManager.CheckIfCanVote(checkBoxAgree, buttonVote, buttonResults, listBoxCandidates);
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        public void ShiftStartTimeByOneDay()
        {
            votingManager.ShiftStartTimeByOneDay();
            labelStartDate.Text = "Дата початку: " + votingManager.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}
