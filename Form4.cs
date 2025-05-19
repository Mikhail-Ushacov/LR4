// Form4.cs
using System;
using System.IO;
using System.Windows.Forms;
using LR4.Interfaces;
using LR4.Services;

namespace LR4
{
    public partial class Form4 : Form
    {
        private readonly IVotingService _votingService;
        private readonly IVoteService _voteService;
        private readonly IVotingResultsService _resultsService;

        public Form4(IVotingService votingService, IVoteService voteService, IVotingResultsService resultsService)
        {
            _votingService = votingService;
            _voteService = voteService;
            _resultsService = resultsService;
            InitializeComponent();

            _votingService.LoadCandidatesCompany(listBoxCandidates);
            listBoxCandidates.Enabled = true;
            _votingService.LoadStages();

            labelStartDate.Text = "Дата початку: " + _votingService.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
            labelEndDate.Text = "Дата завершення: " + _votingService.EndTime.ToString("dd.MM.yyyy HH:mm:ss");
            labelStage.Text = "Поточний етап: " + _votingService.CurrentStage;

            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            buttonResults.Enabled = false;

            if (now < _votingService.StartTime)
            {
                TimeSpan untilStart = _votingService.StartTime - now;
                labelTimeLeft.Text = $"Залишилось до початку: {untilStart:dd\\:hh\\:mm\\:ss}";
            }
            else if (now >= _votingService.StartTime && now < _votingService.EndTime)
            {
                TimeSpan untilEnd = _votingService.EndTime - now;
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

            _votingService.CheckIfCanVote(checkBoxAgree, buttonVote, buttonResults, listBoxCandidates);
        }

        private void ButtonResults_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Form6"] == null)
            {
                Form6 form6 = new Form6(_resultsService);
                form6.Show();
            }
        }

        private void CheckBoxAgree_CheckedChanged(object sender, EventArgs e)
        {
            _votingService.CheckIfCanVote(checkBoxAgree, buttonVote, buttonResults, listBoxCandidates);
        }

        private void ButtonVote_Click(object sender, EventArgs e)
        {
            var activeVoters = _votingService.GetActiveVoters();
            if (activeVoters.Count == 0)
            {
                MessageBox.Show("Немає активних виборців.");
                return;
            }

            foreach (string userLine in activeVoters)
            {
                if (!string.IsNullOrWhiteSpace(userLine))
                {
                    Form5 voteForm = new Form5(userLine, _voteService);
                    voteForm.ShowDialog();
                }
            }
        }

        private void ButtonRules_Click(object sender, EventArgs e)
        {
            string rules = _votingService.GetVotingRules();
            MessageBox.Show(rules, "Правила голосування", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            using (Form3 form3 = new Form3(
                new UserService(),
                new AdminService(),
                _votingService,
                _voteService))
            {
                form3.ShowDialog(this);
            }
        }

        public void ShiftStartTimeByOneDay()
        {
            _votingService.ShiftStartTimeByOneDay();
            labelStartDate.Text = "Дата початку: " + _votingService.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
        }
    }
}
