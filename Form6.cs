using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LR4
{
    public partial class Form6 : Form
    {
        private List<string> candidates = new List<string>();
        private Dictionary<string, int> voteCounts = new Dictionary<string, int>();

        public Form6()
        {
            InitializeComponent();
            LoadCandidates();
            LoadResultsToChart();
        }

        private void LoadCandidates()
        {
            if (File.Exists("candidates.txt"))
            {
                string[] candidatesArray = File.ReadAllLines("candidates.txt");
                candidates.AddRange(candidatesArray);

                listBoxCandidates.Items.Clear();
                listBoxCandidates.Items.AddRange(candidatesArray);
            }
            else
            {
                listBoxCandidates.Items.Add("Файл candidates.txt не знайдено.");
            }
        }

        private void LoadResultsToChart()
        {
            voteCounts.Clear();

            if (!File.Exists("result.txt"))
            {
                MessageBox.Show("Файл result.txt не знайдено.");
                return;
            }

            string[] lines = File.ReadAllLines("result.txt");

            foreach (string line in lines)
            {
                string[] parts = line.Split(';');
                if (parts.Length >= 4)
                {
                    string candidate = parts[3].Trim();
                    if (voteCounts.ContainsKey(candidate))
                        voteCounts[candidate]++;
                    else
                        voteCounts[candidate] = 1;
                }
            }

            int totalVotes = voteCounts.Values.Sum();
            chart1.Series["Series1"].Points.Clear();

            List<KeyValuePair<string, int>> sortedVotes = voteCounts.OrderByDescending(v => v.Value).ToList();

            foreach (var kvp in sortedVotes)
            {
                double percentage = ((double)kvp.Value / totalVotes) * 100;
                string label = $"{kvp.Key} ({percentage:F1}%)";

                DataPoint point = new DataPoint
                {
                    AxisLabel = kvp.Key,
                    YValues = new double[] { kvp.Value },
                    Label = label
                };

                chart1.Series["Series1"].Points.Add(point);
            }

            chart1.Series["Series1"].IsValueShownAsLabel = true;
            chart1.Series["Series1"]["PieLabelStyle"] = "Outside";
        }

        private void ButtonSaveStageResults_Click(object sender, EventArgs e)
        {
            int stage = 1;
            List<KeyValuePair<string, int>> sortedVotes = voteCounts.OrderByDescending(v => v.Value).ToList();
            int totalVotes = voteCounts.Values.Sum();

            SaveStageResults(stage, sortedVotes, totalVotes);
        }

        private void SaveStageResults(int stage, List<KeyValuePair<string, int>> sortedVotes, int totalVotes)
        {
            var passedCandidates = FilterCandidates(sortedVotes, totalVotes);
            if (passedCandidates == null || passedCandidates.Count == 0)
                return;

            string path = "stages.txt";
            List<string> lines;

            if (File.Exists(path))
                lines = File.ReadAllLines(path).ToList();
            else
                lines = new List<string>();

            // Чтение текущего этапа из первой строки
            int currentStage = 1;
            if (lines.Count > 0 && lines[0].StartsWith("Етапи:"))
            {
                var parts = lines[0].Split(':');
                if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int stageFromFile))
                    currentStage = stageFromFile;
            }

            // Формирование результатов для текущего этапа
            string stageResult = $"//Етап {currentStage}\n";
            int rank = 1;

            foreach (var kvp in sortedVotes.Where(v => passedCandidates.Contains(v.Key)))
            {
                double percentage = ((double)kvp.Value / totalVotes) * 100;
                string formattedPercentage = percentage.ToString("F1").Replace('.', ',');
                stageResult += $"{rank}. {kvp.Key}: {formattedPercentage}%\n";
                rank++;
            }

            // Увеличение этапа, если осталось больше одного кандидата
            if (passedCandidates.Count > 1)
                currentStage++;

            // Обновление первой строки с номером этапа
            if (lines.Count > 0 && lines[0].StartsWith("Етапи:"))
                lines[0] = $"Етапи: {currentStage}";
            else
                lines.Insert(0, $"Етапи: {currentStage}");

            // Добавление результатов этапа
            lines.Add("");
            lines.Add(stageResult.TrimEnd());

            File.WriteAllLines(path, lines);

            // Зміщення часу на 1 день вперед
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
                    DateTime parsedTime = new DateTime(year, month, day, hour, minute, second);
                    DateTime newTime = parsedTime.AddDays(1);
                    string newTimeText = $"{newTime.Year}, {newTime.Month}, {newTime.Day}, {newTime.Hour}, {newTime.Minute}, {newTime.Second}";
                    File.WriteAllText("time.txt", newTimeText);
                }
            }

            File.WriteAllText("result.txt", string.Empty); // Очищення файлу результатів

            this.Close();
        }

        private HashSet<string> FilterCandidates(List<KeyValuePair<string, int>> sortedVotes, int totalVotes)
        {
            List<KeyValuePair<string, int>> filteredVotes = sortedVotes
                .Where(v => ((double)v.Value / totalVotes) * 100 >= 15)
                .ToList();

            var passedCandidates = filteredVotes.Select(v => v.Key).ToHashSet();

            if (filteredVotes.Count == 0 && passedCandidates.Count > 1)
            {
                MessageBox.Show("Всі кандидати набрали менше 15% голосів. Завершення голосування.");
                return null;
            }

            File.WriteAllLines("candidates.txt", passedCandidates);

            if (File.Exists("candidates_company.txt"))
            {
                var companyLines = File.ReadAllLines("candidates_company.txt").ToList();
                var updatedLines = new List<string>();

                for (int i = 0; i < companyLines.Count;)
                {
                    string line = companyLines[i].Trim();

                    if (char.IsDigit(line.FirstOrDefault()) && line.Contains('.') && line.IndexOf('.') < line.Length - 2)
                    {
                        string candidateName = line.Substring(line.IndexOf('.') + 1).Trim();

                        var block = new List<string>();
                        while (i < companyLines.Count && !string.IsNullOrWhiteSpace(companyLines[i]))
                        {
                            block.Add(companyLines[i]);
                            i++;
                        }

                        if (i < companyLines.Count)
                        {
                            block.Add("");
                            i++;
                        }

                        if (passedCandidates.Contains(candidateName))
                        {
                            updatedLines.AddRange(block);
                        }
                    }
                    else
                    {
                        i++;
                    }
                }

                File.WriteAllLines("candidates_company.txt", updatedLines);
            }

            return passedCandidates;
        }
    }
}
