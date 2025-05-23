﻿using System;
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
    public partial class Form2 : Form
    {
        private TabControl tabControl1;
        private TabPage tabUsers;
        private TabPage tabCandidates;
        private TabPage tabResults;

        public Form2()
        {
            InitializeComponent();
            CreateAdminTabs();
            LoadUserData();
        }

        private void CreateAdminTabs()
        {
            this.Text = "Адміністративна панель";
            this.Size = new Size(600, 400);

            tabControl1 = new TabControl();
            tabControl1.Dock = DockStyle.Fill;

            tabUsers = new TabPage("Користувачі");
            tabUsers.Controls.Add(panel1); // Move existing panel to first tab
            tabControl1.Controls.Add(tabUsers);

            tabCandidates = new TabPage("Кандидати");
            tabCandidates.Controls.Add(CreateCandidatesTabContent());
            tabControl1.Controls.Add(tabCandidates);

            tabResults = new TabPage("Результати");
            tabResults.Controls.Add(CreateResultsTabContent());
            tabControl1.Controls.Add(tabResults);

            this.Controls.Add(tabControl1);
        }

        private Control CreateCandidatesTabContent()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };

            ListBox listBox = new ListBox
            {
                Width = 300,
                Height = 200,
                Location = new Point(10, 10)
            };

            if (File.Exists("candidates.txt"))
            {
                listBox.Items.AddRange(File.ReadAllLines("candidates.txt"));
            }

            TextBox txtNewCandidate = new TextBox
            {
                Width = 200,
                Location = new Point(10, 220)
            };

            Button btnAdd = new Button
            {
                Text = "Додати",
                Location = new Point(220, 220),
                Width = 80
            };

            Button btnRemove = new Button
            {
                Text = "Видалити",
                Location = new Point(10, 250),
                Width = 80
            };

            btnAdd.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(txtNewCandidate.Text))
                {
                    listBox.Items.Add(txtNewCandidate.Text);
                    File.AppendAllText("candidates.txt", txtNewCandidate.Text + Environment.NewLine);
                    txtNewCandidate.Clear();
                }
            };

            btnRemove.Click += (s, e) => {
                if (listBox.SelectedIndex >= 0)
                {
                    var candidates = File.ReadAllLines("candidates.txt").ToList();
                    candidates.Remove(listBox.SelectedItem.ToString());
                    File.WriteAllLines("candidates.txt", candidates);
                    listBox.Items.Remove(listBox.SelectedItem);
                }
            };

            panel.Controls.Add(listBox);
            panel.Controls.Add(txtNewCandidate);
            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnRemove);

            return panel;
        }

        private Control CreateResultsTabContent()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };

            ListBox listBox = new ListBox
            {
                Width = 400,
                Height = 300,
                Location = new Point(10, 10)
            };

            Label lblStats = new Label
            {
                AutoSize = true,
                Location = new Point(10, 320),
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold)
            };

            if (File.Exists("result.txt"))
            {
                var lines = File.ReadAllLines("result.txt");
                var results = new Dictionary<string, int>();

                foreach (string line in lines)
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

                int totalVotes = results.Values.Sum();

                foreach (var kvp in results.OrderByDescending(r => r.Value))
                {
                    double percentage = (double)kvp.Value / totalVotes * 100;
                    listBox.Items.Add($"{kvp.Key}: {kvp.Value} голосів ({percentage:F1}%)");
                }

                lblStats.Text = $"Всього голосів: {totalVotes}";
            }
            else
            {
                listBox.Items.Add("Файл result.txt не знайдено");
            }

            panel.Controls.Add(listBox);
            panel.Controls.Add(lblStats);

            return panel;
        }

        private void LoadUserData()
        {
            if (!File.Exists("user.txt"))
            {
                MessageBox.Show("Файл user.txt не знайдено.");
                return;
            }

            string[] lines = File.ReadAllLines("user.txt");
            int y = 10;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(';');
                if (parts.Length < 3) continue;

                string formattedText = $"Ім'я: {parts[0]} | Прізвище: {parts[1]} | Номер паспорта: {parts[2]}";

                Label lbl = new Label();
                lbl.Text = formattedText;
                lbl.AutoSize = true;
                lbl.Location = new System.Drawing.Point(10, y);

                Button btn = new Button
                {
                    Text = "Підтвердити",
                    Tag = line,
                    Location = new System.Drawing.Point(400, y - 3)
                };
                btn.Click += BtnConfirm_Click;

                this.panel1.Controls.Add(lbl);
                this.panel1.Controls.Add(btn);

                y += 30;
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string line = btn.Tag.ToString();

            // Додаємо у verified.txt
            File.AppendAllText("verified.txt", line + Environment.NewLine);

            // Видаляємо з user.txt
            var allLines = File.ReadAllLines("user.txt").ToList();
            allLines.Remove(line);
            File.WriteAllLines("user.txt", allLines);

            // Видаляємо з інтерфейсу
            Label labelToRemove = null;
            foreach (Control control in this.panel1.Controls)
            {
                if (control is Label label && label.Text == GetFormattedLine(line))
                {
                    labelToRemove = label;
                    break;
                }
            }

            this.panel1.Controls.Remove(labelToRemove);
            this.panel1.Controls.Remove(btn);

            MessageBox.Show("Дані підтверджено і видалено з очікування!", "Готово");

            // Якщо всі підтверджені — повідомити або закрити
            if (this.panel1.Controls.Count == 0)
            {
                MessageBox.Show("Усі записи оброблено.");
                this.Close();
            }
        }

        private string GetFormattedLine(string line)
        {
            string[] parts = line.Split(';');
            if (parts.Length < 3) return "";

            return $"Ім'я: {parts[0]} | Прізвище: {parts[1]} | Номер паспорта: {parts[2]}";
        }
    }
}

