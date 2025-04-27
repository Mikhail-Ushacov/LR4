using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

    namespace LR4
    {
        public partial class Form6 : Form
        {
            public Form6()
            {
                InitializeComponent();
                LoadCandidates();
                LoadResultsToChart();
            }

            private void LoadCandidates()
            {
                if (File.Exists("candidates.txt"))
                {
                    string[] candidates = File.ReadAllLines("candidates.txt");
                    listBoxCandidates.Items.AddRange(candidates);
                }
                else
                {
                    listBoxCandidates.Items.Add("Файл candidates.txt не знайдено.");
                }
            }

            private void LoadResultsToChart()
            {
            chart1.Series["Series1"].Points.AddXY("1", "20");

            Dictionary<string, int> voteCounts = new Dictionary<string, int>();

            if (!File.Exists("result.txt"))
            {
                MessageBox.Show("Файл result.txt не знайдено.");
                return;
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

            chart1.Series["Series1"].Points.Clear();

            foreach (var kvp in voteCounts)
            {
                chart1.Series["Series1"].Points.AddXY(kvp.Key, kvp.Value);
            }

            chart1.Series["Series1"].IsValueShownAsLabel = true;
            chart1.Series["Series1"]["PieLabelStyle"] = "Outside";
            }
        }
    }