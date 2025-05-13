using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LR4.Interfaces;

namespace LR4.Services
{
    public class VoteService : IVoteService
    {
        public List<string> GetCandidates()
        {
            if (!File.Exists("candidates.txt"))
            {
                throw new FileNotFoundException("Candidates file not found");
            }
            return File.ReadAllLines("candidates.txt").ToList();
        }

        public void ProcessVote(string userData, string selectedCandidate)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string[] parts = userData.Split(';');
            if (parts.Length < 3)
            {
                throw new ArgumentException("Invalid user data format");
            }

            string resultLine = $"{parts[0]};{parts[1]};{parts[2]};{selectedCandidate};{timestamp}";
            File.AppendAllText("result.txt", resultLine + Environment.NewLine);

            var activeVoters = File.Exists("active.txt") ? 
                File.ReadAllLines("active.txt").ToList() : 
                new List<string>();
            activeVoters.Remove(userData);
            File.WriteAllLines("active.txt", activeVoters);
        }
    }
}
