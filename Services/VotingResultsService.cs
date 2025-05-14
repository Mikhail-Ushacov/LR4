using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using LR4.Interfaces;

namespace LR4.Services
{
    public class VotingResultsService : IVotingResultsService
    {
        public IEnumerable<string> GetCandidates()
        {
            if (!File.Exists("candidates.txt"))
                throw new FileNotFoundException("Candidates file not found");

            return File.ReadAllLines("candidates.txt");
        }

        public Dictionary<string, int> GetVoteCounts()
        {
            if (!File.Exists("result.txt"))
                throw new FileNotFoundException("Results file not found");

            var results = new Dictionary<string, int>();
            foreach (string line in File.ReadAllLines("result.txt"))
            {
                string[] parts = line.Split(';');
                if (parts.Length >= 4)
                {
                    string candidate = parts[3].Trim();
                    if (results.ContainsKey(candidate))
                        results[candidate]++;
                    else
                        results[candidate] = 1;
                }
            }
            return results.OrderByDescending(r => r.Value)
                         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public void SaveStageResults(int stage, Dictionary<string, int> votes, int totalVotes)
        {
            string stageResults = $"Stage {stage} Results:\n";
            foreach (var kvp in votes)
            {
                double percentage = (double)kvp.Value / totalVotes * 100;
                stageResults += $"{kvp.Key}: {kvp.Value} votes ({percentage:F1}%)\n";
            }
            File.AppendAllText("stages.txt", stageResults);
        }

        public void LoadResultsToChart(Chart chart)
        {
            var votes = GetVoteCounts();
            int totalVotes = votes.Values.Sum();

            chart.Series["Series1"].Points.Clear();
            foreach (var kvp in votes)
            {
                double percentage = ((double)kvp.Value / totalVotes) * 100;
                string label = $"{kvp.Key} ({percentage:F1}%)";

                DataPoint point = new DataPoint
                {
                    AxisLabel = kvp.Key,
                    YValues = new double[] { kvp.Value },
                    Label = label
                };

                chart.Series["Series1"].Points.Add(point);
            }

            chart.Series["Series1"].IsValueShownAsLabel = true;
            chart.Series["Series1"]["PieLabelStyle"] = "Outside";
        }
    }
}
