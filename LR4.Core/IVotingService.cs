using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LR4.Interfaces
{
    public interface IVotingService
    {
        DateTime StartTime { get; }
        DateTime EndTime { get; }
        int CurrentStage { get; }
        
        void LoadCandidates(ListBox listBox);
        void LoadCandidatesCompany(ListBox listBox);
        void LoadStages();
        void ShiftStartTimeByOneDay();
        bool CheckIfCanVote(CheckBox agreementCheckBox, Button voteButton, Button resultsButton, ListBox candidatesList);
        List<string> GetActiveVoters();
        void ProcessVote(string userData, string selectedCandidate);
        void SaveVotingTime(DateTime time);
        string GetVotingRules();
    }
}
