using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LR4.Interfaces;

namespace LR4.Services
{
    public class VotingService : IVotingService
    {
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int CurrentStage { get; private set; }

        public VotingService()
        {
            LoadStages();
        }

        public void LoadCandidates(ListBox listBox)
        {
            if (!File.Exists("candidates.txt")) return;
            
            listBox.Items.Clear();
            listBox.Items.AddRange(File.ReadAllLines("candidates.txt"));
        }

        public void LoadStages()
        {
            if (File.Exists("time.txt"))
            {
                string[] timeParts = File.ReadAllText("time.txt").Split(',');
                if (timeParts.Length == 6)
                {
                    int[] timeValues = Array.ConvertAll(timeParts, int.Parse);
                    StartTime = new DateTime(timeValues[0], timeValues[1], timeValues[2],
                                          timeValues[3], timeValues[4], timeValues[5]);
                    EndTime = StartTime.AddDays(1);
                }
            }
            else
            {
                StartTime = DateTime.Now.AddDays(1);
                EndTime = StartTime.AddDays(1);
            }

            if (File.Exists("stages.txt"))
            {
                string content = File.ReadAllText("stages.txt");
                if (!string.IsNullOrWhiteSpace(content))
                {
                    try 
                    {
                        CurrentStage = int.Parse(content.Split(':')[1].Trim());
                    }
                    catch
                    {
                        CurrentStage = 0;
                        File.WriteAllText("stages.txt", "Етапи: 0");
                    }
                }
                else
                {
                    CurrentStage = 0;
                    File.WriteAllText("stages.txt", "Етапи: 0");
                }
            }
            else
            {
                CurrentStage = 0;
                File.WriteAllText("stages.txt", "Етапи: 0");
            }
        }

        public void ShiftStartTimeByOneDay()
        {
            StartTime = StartTime.AddDays(1);
            EndTime = StartTime.AddDays(1);
            File.WriteAllText("time.txt", 
                $"{StartTime.Year}, {StartTime.Month}, {StartTime.Day}, " +
                $"{StartTime.Hour}, {StartTime.Minute}, {StartTime.Second}");
        }

        public bool CheckIfCanVote(CheckBox agreementCheckBox, Button voteButton, 
                                 Button resultsButton, ListBox candidatesList)
        {
            bool canVote = agreementCheckBox.Checked &&
                         DateTime.Now >= StartTime && 
                         DateTime.Now < EndTime;

            voteButton.Enabled = canVote;
            resultsButton.Enabled = DateTime.Now >= EndTime;
            return canVote;
        }

        public List<string> GetActiveVoters()
        {
            return File.Exists("active.txt") ? 
                File.ReadAllLines("active.txt").ToList() : 
                new List<string>();
        }

        public void ProcessVote(string userData, string selectedCandidate)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string resultLine = $"{userData};{selectedCandidate};{timestamp}";
            File.AppendAllText("result.txt", resultLine + Environment.NewLine);

            var activeVoters = GetActiveVoters();
            activeVoters.Remove(userData);
            File.WriteAllLines("active.txt", activeVoters);
        }

        public void SaveVotingTime(DateTime time)
        {
            string timeStr = $"{time.Year}, {time.Month}, {time.Day}, " +
                           $"{time.Hour}, {time.Minute}, {time.Second}";
            File.WriteAllText("time.txt", timeStr);
        }

        public string GetVotingRules()
        {
            return File.Exists("rules.txt") ? 
                File.ReadAllText("rules.txt") : 
                "Правила голосування не знайдено.";
        }
    }
}
