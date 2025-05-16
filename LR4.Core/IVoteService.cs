using System.Collections.Generic;

namespace LR4.Interfaces
{
    public interface IVoteService
    {
        List<string> GetCandidates();
        void ProcessVote(string userData, string selectedCandidate);
    }
}
