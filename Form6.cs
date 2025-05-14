// Form6.cs
using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LR4.Interfaces;

namespace LR4
{
    public partial class Form6 : Form
    {
        private readonly IVotingResultsService _resultsService;

        public Form6(IVotingResultsService resultsService)
        {
            InitializeComponent();
            _resultsService = resultsService;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                LoadCandidates();
                _resultsService.LoadResultsToChart(chart1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCandidates()
        {
            listBoxCandidates.Items.Clear();
            foreach (var candidate in _resultsService.GetCandidates())
            {
                listBoxCandidates.Items.Add(candidate);
            }
        }

        private void ButtonSaveStageResults_Click(object sender, EventArgs e)
        {
            try
            {
                var votes = _resultsService.GetVoteCounts();
                int totalVotes = votes.Values.Sum();
                _resultsService.SaveStageResults(1, votes, totalVotes);
                MessageBox.Show("Stage results saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving results: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
