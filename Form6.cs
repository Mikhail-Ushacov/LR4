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
        }

        // Обробник події для збереження результатів
        private void ButtonSaveStageResults_Click(object sender, EventArgs e)
        {
            int stage = 1;  // Оновіть це значення залежно від вашої логіки
            List<KeyValuePair<string, int>> sortedVotes = voteCounts.OrderByDescending(v => v.Value).ToList();
            int totalVotes = voteCounts.Values.Sum();

            SaveStageResults(stage, sortedVotes, totalVotes);
        }

        private void SaveStageResults(int stage, List<KeyValuePair<string, int>> sortedVotes, int totalVotes)
        {
            // Відсіяти кандидатів з менше ніж 15%
            List<KeyValuePair<string, int>> filteredVotes = sortedVotes
                .Where(v => ((double)v.Value / totalVotes) * 100 >= 15)
                .ToList();

            // Оновити candidates.txt – залишити тільки тих, хто має >= 15%
            if (filteredVotes.Count > 0)
            {
                File.WriteAllLines("candidates.txt", filteredVotes.Select(v => v.Key));
            }
            else
            {
                MessageBox.Show("Всі кандидати набрали менше 15% голосів. Завершення голосування.");
                return; // Не продовжуємо, якщо немає кандидатів.
            }

            // Формування результатів для етапу
            string stageResult = $"//Етап {stage}\n";
            int rank = 1;

            foreach (var kvp in filteredVotes)
            {
                double percentage = ((double)kvp.Value / totalVotes) * 100;
                string formattedPercentage = percentage.ToString("F1").Replace('.', ','); // Український формат
                stageResult += $"{rank}. {kvp.Key}: {formattedPercentage}%\n";
                rank++;
            }

            string path = "stages.txt";
            List<string> lines;

            // Читання поточного вмісту файлу stages.txt
            if (File.Exists(path))
            {
                lines = File.ReadAllLines(path).ToList();
            }
            else
            {
                lines = new List<string>();
            }

            // Визначаємо поточний номер етапу
            int currentStage = 1;
            var stagesLine = lines.FirstOrDefault(l => l.StartsWith("Етапи:"));
            if (stagesLine != null)
            {
                currentStage = int.Parse(stagesLine.Split(':')[1].Trim());
            }

            // Якщо кандидатів більше одного, збільшуємо номер етапу
            if (filteredVotes.Count > 1)
            {
                currentStage++;
            }

            // Оновлення "Етапи: N" в першому рядку
            if (stagesLine != null)
            {
                lines[lines.IndexOf(stagesLine)] = $"Етапи: {currentStage}";
            }
            else
            {
                lines.Insert(0, $"Етапи: {currentStage}");
            }

            // Додавання нових результатів етапу
            lines.Add(""); // Пустий рядок для розділення
            lines.Add(stageResult.TrimEnd());

            // Запис назад у файл stages.txt
            File.WriteAllLines(path, lines);

            // Повернення до Form4
            //this.Close(); // Закриття поточної форми
            Form4 form4 = new Form4();
            form4.Show();
            
        }
    }
}
