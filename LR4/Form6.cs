// Form6.cs
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace LR4
{
    public partial class Form6 : Form
    {
        private VotingResultsManager votingResultsManager;

        public Form6()
        {
            InitializeComponent();
            votingResultsManager = new VotingResultsManager();

            try
            {
                LoadCandidates();
                LoadResultsToChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCandidates()
        {
            listBoxCandidates.Items.Clear();
            foreach (var candidate in votingResultsManager.Candidates)
            {
                listBoxCandidates.Items.Add(candidate);
            }
        }

        private void LoadResultsToChart()
        {
            try
            {
                votingResultsManager.LoadResultsToChart();

                var sortedVotes = votingResultsManager.GetSortedVoteCounts();
                int totalVotes = sortedVotes.Sum(v => v.Value);

                chart1.Series["Series1"].Points.Clear();

                foreach (var kvp in sortedVotes)
                {
                    double percentage = ((double)kvp.Value / totalVotes) * 100;
                    string label = $"{kvp.Key} ({percentage:F1}%)";

                    DataPoint point = new DataPoint
                    {
                        AxisLabel = kvp.Key,
                        YValues = new double[] { kvp.Value },
                        Label = label
                    };

                    chart1.Series["Series1"].Points.Add(point);
                }

                chart1.Series["Series1"].IsValueShownAsLabel = true;
                chart1.Series["Series1"]["PieLabelStyle"] = "Outside";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonSaveStageResults_Click(object sender, EventArgs e)
        {
            try
            {
                int stage = 1;
                var sortedVotes = votingResultsManager.GetSortedVoteCounts();
                int totalVotes = sortedVotes.Sum(v => v.Value);

                votingResultsManager.SaveStageResults(stage, sortedVotes, totalVotes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
