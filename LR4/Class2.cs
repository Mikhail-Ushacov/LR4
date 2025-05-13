// Class2.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LR4
{
    public class VotingResultsManager
    {
        private Dictionary<string, int> voteCounts = new Dictionary<string, int>();

        public List<string> Candidates { get; private set; }

        public VotingResultsManager()
        {
            Candidates = new List<string>();
            LoadCandidates();
        }

        public void LoadCandidates()
        {
            if (File.Exists("candidates.txt"))
            {
                string[] candidatesArray = File.ReadAllLines("candidates.txt");
                Candidates.AddRange(candidatesArray);
            }
            else
            {
                throw new FileNotFoundException("Файл candidates.txt не знайдено.");
            }
        }

        public void LoadResultsToChart()
        {
            voteCounts.Clear();

            if (!File.Exists("result.txt"))
            {
                throw new FileNotFoundException("Файл result.txt не знайдено.");
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
        }

        public List<KeyValuePair<string, int>> GetSortedVoteCounts()
        {
            return voteCounts.OrderByDescending(v => v.Value).ToList();
        }

        public void SaveStageResults(int stage, List<KeyValuePair<string, int>> sortedVotes, int totalVotes)
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

            int currentStage = 1;
            if (lines.Count > 0 && lines[0].StartsWith("Етапи:"))
            {
                var parts = lines[0].Split(':');
                if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out int stageFromFile))
                    currentStage = stageFromFile;
            }

            string stageResult = $"//Етап {currentStage}\n";
            int rank = 1;

            foreach (var kvp in sortedVotes.Where(v => passedCandidates.Contains(v.Key)))
            {
                double percentage = ((double)kvp.Value / totalVotes) * 100;
                string formattedPercentage = percentage.ToString("F1").Replace('.', ',');
                stageResult += $"{rank}. {kvp.Key}: {formattedPercentage}%\n";
                rank++;
            }

            if (passedCandidates.Count > 1)
                currentStage++;

            if (lines.Count > 0 && lines[0].StartsWith("Етапи:"))
                lines[0] = $"Етапи: {currentStage}";
            else
                lines.Insert(0, $"Етапи: {currentStage}");

            lines.Add("");
            lines.Add(stageResult.TrimEnd());

            File.WriteAllLines(path, lines);

            UpdateTimeForNextStage();
            File.WriteAllText("result.txt", string.Empty); // Очищення файлу результатів
        }

        private HashSet<string> FilterCandidates(List<KeyValuePair<string, int>> sortedVotes, int totalVotes)
        {
            List<KeyValuePair<string, int>> filteredVotes = sortedVotes
                .Where(v => ((double)v.Value / totalVotes) * 100 >= 15)
                .ToList();

            var passedCandidates = filteredVotes.Select(v => v.Key).ToHashSet();

            if (filteredVotes.Count == 0 && passedCandidates.Count > 1)
            {
                throw new InvalidOperationException("Всі кандидати набрали менше 15% голосів. Завершення голосування.");
            }

            File.WriteAllLines("candidates.txt", passedCandidates);

            UpdateCandidatesCompanyFile(passedCandidates);

            return passedCandidates;
        }

        private void UpdateCandidatesCompanyFile(HashSet<string> passedCandidates)
        {
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
        }

        private void UpdateTimeForNextStage()
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
                    DateTime parsedTime = new DateTime(year, month, day, hour, minute, second);
                    DateTime newTime = parsedTime.AddDays(1);
                    string newTimeText = $"{newTime.Year}, {newTime.Month}, {newTime.Day}, {newTime.Hour}, {newTime.Minute}, {newTime.Second}";
                    File.WriteAllText("time.txt", newTimeText);
                }
            }
        }
    }
}
