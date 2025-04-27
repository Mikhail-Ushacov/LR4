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
            }
            else
            {
                listBoxCandidates.Items.Add("Файл candidates.txt не знайдено.");
            }
        }

        private void LoadResultsToChart()
        {
            voteCounts.Clear(); // Очистка попередніх результатів

            if (!File.Exists("result.txt"))
            {
                MessageBox.Show("Файл result.txt не знайдено.");
                return;
            }

            string[] lines = File.ReadAllLines("result.txt");

            // Підрахунок голосів
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
            double percentage;

            // Відображення на діаграмі
            foreach (var kvp in sortedVotes)
            {
                percentage = ((double)kvp.Value / totalVotes) * 100;
                string label = $"{kvp.Key} ({percentage:F1}%)";

                DataPoint point = new DataPoint
                {
                    AxisLabel = kvp.Key,
                    YValues = new double[] { kvp.Value }
                };

                point.Label = label;
                chart1.Series["Series1"].Points.Add(point);
            }

            chart1.Series["Series1"].IsValueShownAsLabel = true;
            chart1.Series["Series1"]["PieLabelStyle"] = "Outside";

            // Перехід на форму 3
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void SaveStageResults(int stage, List<KeyValuePair<string, int>> sortedVotes, int totalVotes)
        {
            // Етап 1: збереження результатів для першого етапу
            SaveStageResults(1, sortedVotes, totalVotes);

            // Видалення кандидатів, що набрали менше 15%
            sortedVotes = sortedVotes.Where(v => ((double)v.Value / totalVotes) * 100 >= 15).ToList();

            // Оновлення candidates.txt для етапу 1
            File.WriteAllLines("candidates.txt", sortedVotes.Select(v => v.Key).Take(5));

            // Формування результатів для етапу
            string stageResult = $"//Етап {stage}\n";
            int rank = 1;

            foreach (var kvp in sortedVotes)
            {
                double percentage = ((double)kvp.Value / totalVotes) * 100;
                stageResult += $"{rank}. {kvp.Key}: {percentage:F1}%\n";
                rank++;
            }

            // Запис результатів в файл stages.txt
            File.AppendAllText("stages.txt", stageResult + "\n");
        }
    }
}
