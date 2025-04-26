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

namespace LR4
{
    public partial class Form4 : Form
    {
        private DateTime startTime = new DateTime(2025, 4, 1, 12, 0, 0); // Дата початку голосування

        public Form4()
        {
            InitializeComponent();
            LoadCandidates();
            labelStartDate.Text = "Дата початку: " + startTime.ToString("dd.MM.yyyy HH:mm:ss");

            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        private void LoadCandidates()
        {
            if (File.Exists("candidates.txt"))
            {
                string[] lines = File.ReadAllLines("candidates.txt");
                listBoxCandidates.Items.AddRange(lines);
            }
            else
            {
                listBoxCandidates.Items.Add("Файл candidates.txt не знайдено.");
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan remaining = startTime - DateTime.Now;

            if (remaining.TotalSeconds > 0)
            {
                labelTimeLeft.Text = $"Залишилось до початку: {remaining:dd\\:hh\\:mm\\:ss}";
            }
            else
            {
                labelTimeLeft.Text = "Голосування почалось!";
            }

            CheckIfCanVote();
        }

        private void CheckIfCanVote()
        {
            if (checkBoxAgree.Checked && DateTime.Now >= startTime)
                buttonVote.Enabled = true;
            else
                buttonVote.Enabled = false;
        }

        private void buttonRules_Click(object sender, EventArgs e)
        {
            if (File.Exists("rules.txt"))
            {
                string rules = File.ReadAllText("rules.txt");
                MessageBox.Show(rules, "Правила голосування", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Файл rules.txt не знайдено.");
            }
        }

        private void checkBoxAgree_CheckedChanged(object sender, EventArgs e)
        {
            CheckIfCanVote();
        }

        private void buttonVote_Click(object sender, EventArgs e)
        {
            if (!File.Exists("active.txt"))
            {
                MessageBox.Show("Файл active.txt не знайдено.");
                return;
            }

            string[] users = File.ReadAllLines("active.txt");
            foreach (string userLine in users)
            {
                if (!string.IsNullOrWhiteSpace(userLine))
                {
                    Form5 voteForm = new Form5(userLine);
                    voteForm.ShowDialog();
                }
            }

            //this.Close(); // Закрити підготовче вікно
        }
    }
}