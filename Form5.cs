using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LR4.Interfaces;

namespace LR4
{
    public partial class Form5 : Form
    {
        private readonly string userData;
        private readonly DateTime voteStartTime;
        private readonly List<CheckBox> candidateCheckBoxes = new List<CheckBox>();
        private readonly IVoteService _voteService;

        public Form5(string userLine, IVoteService voteService)
        {
            InitializeComponent();
            this.userData = userLine;
            _voteService = voteService;
            voteStartTime = DateTime.Now;
            ShowUserInfo();
            LoadCandidates();
        }

        private void ShowUserInfo()
        {
            string[] parts = userData.Split(';');
            if (parts.Length >= 3)
            {
                labelInfo.Text = $"Ім'я: {parts[0]} | Прізвище: {parts[1]} | Номер паспорта: {parts[2]} | Час початку: {voteStartTime:HH:mm:ss}";
            }
        }

        private void LoadCandidates()
        {
            try
            {
                var candidates = _voteService.GetCandidates();
                int y = 10;

                foreach (var candidate in candidates)
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.Text = candidate;
                    checkBox.Location = new System.Drawing.Point(10, y);
                    checkBox.AutoSize = true;
                    checkBox.CheckedChanged += CandidateCheckBox_CheckedChanged;

                    panelCandidates.Controls.Add(checkBox);
                    candidateCheckBoxes.Add(checkBox);

                    y += 25;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при завантаженні кандидатів: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CandidateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox changed = sender as CheckBox;

            if (changed.Checked)
            {
                foreach (var cb in candidateCheckBoxes)
                {
                    if (cb != changed)
                        cb.Checked = false;
                }
            }
        }

        private void buttonVote_Click(object sender, EventArgs e)
        {
            var selected = candidateCheckBoxes.FirstOrDefault(c => c.Checked);
            if (selected == null)
            {
                MessageBox.Show("Будь ласка, оберіть кандидата!", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedCandidate = selected.Text;

            try
            {
                _voteService.ProcessVote(userData, selectedCandidate);
                MessageBox.Show("Голос зафіксовано!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при обробці голосу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
