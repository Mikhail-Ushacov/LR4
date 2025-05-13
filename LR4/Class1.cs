// Class1.cs
using System;
using System.IO;
using System.Windows.Forms;

namespace LR4
{
    public class VotingManager
    {
        private DateTime startTime;
        private DateTime endTime;
        private string currentStage;

        public VotingManager()
        {
            LoadStartTime();
            endTime = startTime.AddHours(1);  // Кінець голосування
            currentStage = "Не визначено";   // За замовчуванням
        }

        public DateTime StartTime => startTime;
        public DateTime EndTime => endTime;
        public string CurrentStage => currentStage;

        public void LoadStartTime()
        {
            if (File.Exists("time.txt"))
            {
                string timeText = File.ReadAllText("time.txt").Trim();
                string[] parts = timeText.Split(',');

                if (parts.Length == 6 &&
                    int.TryParse(parts[0], out int year) &&
                    int.TryParse(parts[1], out int month) &&
                    int.TryParse(parts[2], out int day) &&
                    int.TryParse(parts[3], out int hour) &&
                    int.TryParse(parts[4], out int minute) &&
                    int.TryParse(parts[5], out int second))
                {
                    startTime = new DateTime(year, month, day, hour, minute, second);
                }
                else
                {
                    MessageBox.Show("Неправильний формат у time.txt. Використано поточний час.");
                    startTime = DateTime.Now;
                }
            }
            else
            {
                MessageBox.Show("Файл time.txt не знайдено. Використано поточний час.");
                startTime = DateTime.Now;
            }
        }

        public void LoadCandidates(ListBox listBox)
        {
            if (File.Exists("candidates_company.txt"))
            {
                string[] lines = File.ReadAllLines("candidates_company.txt");
                listBox.Items.AddRange(lines);
            }
            else
            {
                listBox.Items.Add("Файл candidates_company.txt не знайдено.");
            }
        }

        public void LoadStages()
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
                    currentStage = "Не визначено";
                }
            }
            else
            {
                currentStage = "Файл stages.txt не знайдено.";
            }
        }

        public void CheckIfCanVote(CheckBox checkBoxAgree, Button buttonVote, Button buttonResults, ListBox listBoxCandidates)
        {
            DateTime now = DateTime.Now;
            if (checkBoxAgree.Checked && now >= startTime && now < endTime && listBoxCandidates.Items.Count > 1)
                buttonVote.Enabled = true;
            else
            {
                buttonVote.Enabled = false;
                buttonResults.Enabled = true;

                string candidatesFile = "candidates.txt";
                if (File.Exists(candidatesFile))
                {
                    string[] remainingCandidates = File.ReadAllLines(candidatesFile);
                    if (remainingCandidates.Length == 1)
                    {
                        string winner = remainingCandidates[0].Trim();
                        MessageBox.Show($"Голосування завершено! Переміг {winner}");
                    }
                }
            }
        }

        public void ShiftStartTimeByOneDay()
        {
            startTime = startTime.AddDays(1);
        }
    }
}
