using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LR4.Interfaces;

namespace LR4.Core
{
    public class AdminPanelService
    {
        private readonly IAdminService _adminService;

        public AdminPanelService(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public IEnumerable<string> GetPendingUsers()
        {
            return _adminService.GetPendingRegistrations();
        }

        public void ApproveUser(string userData)
        {
            _adminService.ApproveRegistration(userData);
        }

        public IEnumerable<string> GetCandidates()
        {
            if (!File.Exists("candidates.txt"))
                return Enumerable.Empty<string>();
                
            return File.ReadAllLines("candidates.txt");
        }

        public void AddCandidate(string candidate)
        {
            if (!string.IsNullOrWhiteSpace(candidate))
            {
                File.AppendAllText("candidates.txt", candidate + Environment.NewLine);
            }
        }

        public void RemoveCandidate(string candidate)
        {
            if (!string.IsNullOrWhiteSpace(candidate))
            {
                var candidates = File.ReadAllLines("candidates.txt")
                    .Where(c => c != candidate)
                    .ToArray();
                File.WriteAllLines("candidates.txt", candidates);
            }
        }

        public Dictionary<string, int> GetVotingResults()
        {
            if (!File.Exists("result.txt"))
                return new Dictionary<string, int>();

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
            return results;
        }

        public void SaveVotingTime(DateTime time)
        {
            string timeStr = $"{time.Year},{time.Month},{time.Day}," +
                           $"{time.Hour},{time.Minute},{time.Second}";
            File.WriteAllText("time.txt", timeStr);
        }

        public DateTime? GetCurrentVotingTime()
        {
            if (!File.Exists("time.txt"))
                return null;

            string[] timeParts = File.ReadAllText("time.txt").Split(',');
            if (timeParts.Length == 6)
            {
                int[] timeValues = Array.ConvertAll(timeParts, int.Parse);
                return new DateTime(timeValues[0], timeValues[1], timeValues[2],
                                  timeValues[3], timeValues[4], timeValues[5]);
            }
            return null;
        }

        public void ClearStages()
        {
            File.WriteAllText("stages.txt", "Етапи: 0\n");
        }
    }
}
