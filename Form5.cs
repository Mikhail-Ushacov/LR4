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
    public partial class Form5 : Form
    {
        private string userData;
        private DateTime voteStartTime;
        private List<CheckBox> candidateCheckBoxes = new List<CheckBox>();

        public Form5(string userLine)
        {
            InitializeComponent();
            this.userData = userLine;
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
            if (!File.Exists("candidates.txt"))
            {
                MessageBox.Show("Файл candidates.txt не знайдено.");
                return;
            }

            string[] candidates = File.ReadAllLines("candidates.txt");
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
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string[] parts = userData.Split(';');
            if (parts.Length < 3)
            {
                MessageBox.Show("Некоректні дані користувача.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string firstName = parts[0];
            string lastName = parts[1];
            string passport = parts[2];

            // Формати:
            string voteRaw = $"{passport};{selectedCandidate};{timestamp}"; // votes.txt
            string resultLine = $"{firstName};{lastName};{passport};{selectedCandidate};{timestamp}"; // result.txt

            File.AppendAllText("votes.txt", voteRaw + Environment.NewLine);
            File.AppendAllText("result.txt", resultLine + Environment.NewLine);

            // Видалення з active.txt
            var allLines = File.ReadAllLines("active.txt").ToList();
            allLines.Remove(userData);
            File.WriteAllLines("active.txt", allLines);

            MessageBox.Show("Голос зафіксовано!\nІнформація записана в result.txt", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}