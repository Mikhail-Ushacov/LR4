using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace LR4.Interfaces
{
    public interface IVotingResultsService
    {
        IEnumerable<string> GetCandidates();
        Dictionary<string, int> GetVoteCounts();
        void SaveStageResults(int stage, Dictionary<string, int> votes, int totalVotes);
        void LoadResultsToChart(Chart chart);
    }
}
