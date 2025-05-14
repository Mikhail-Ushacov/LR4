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

using LR4.Interfaces;
using System;

namespace LR4
{
    public partial class Form2 : Form
    {
        private TabControl tabControl1;
        private TabPage tabUsers;
        private TabPage tabCandidates;
        private TabPage tabResults;
        private TabPage tabSettings;
        private readonly IAdminService _adminService;

        public Form2(IAdminService adminService)
        {
            _adminService = adminService;
            InitializeComponent();
            CreateAdminTabs();
            LoadUserData();
        }

        private void CreateAdminTabs()
        {
            this.Text = "Адміністративна панель";
            this.Size = new Size(600, 400);

            tabControl1 = new TabControl { Dock = DockStyle.Fill };

            tabUsers = new TabPage("Користувачі");
            tabUsers.Controls.Add(panel1);
            tabControl1.Controls.Add(tabUsers);

            tabCandidates = new TabPage("Кандидати");
            tabCandidates.Controls.Add(CreateCandidatesTabContent());
            tabControl1.Controls.Add(tabCandidates);

            tabResults = new TabPage("Результати");
            tabResults.Controls.Add(CreateResultsTabContent());
            tabControl1.Controls.Add(tabResults);

            tabSettings = new TabPage("Налаштування");
            tabSettings.Controls.Add(CreateSettingsTabContent());
            tabControl1.Controls.Add(tabSettings);

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
            var pendingUsers = _adminService.GetPendingRegistrations();
            int y = 10;

            foreach (string line in pendingUsers)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(';');
                if (parts.Length < 3) continue;

                string formattedText = $"Ім'я: {parts[0]} | Прізвище: {parts[1]} | Номер паспорта: {parts[2]}";

            Label lbl = new Label
            {
                Text = formattedText,
                AutoSize = true,
                Location = new System.Drawing.Point(10, y)
            };

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

            // Approve registration using service
            _adminService.ApproveRegistration(line);

            // Remove from UI
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

            // If all approved - notify or close
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

        private Control CreateSettingsTabContent()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill };

            Label lblTime = new Label
            {
                Text = "Редагувати час:",
                AutoSize = true,
                Location = new Point(10, 10)
            };

            DateTimePicker dateTimePicker = new DateTimePicker
            {
                Location = new Point(10, 40),
                Width = 200,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy, MM, dd, HH, mm, ss"
            };

            if (File.Exists("time.txt"))
            {
                string[] timeParts = File.ReadAllText("time.txt").Split(',');
                if (timeParts.Length == 6)
                {
                    int[] timeValues = Array.ConvertAll(timeParts, int.Parse);
                    dateTimePicker.Value = new DateTime(timeValues[0], timeValues[1], timeValues[2],
                                                      timeValues[3], timeValues[4], timeValues[5]);
                }
            }

            Button btnSaveTime = new Button
            {
                Text = "Зберегти час",
                Location = new Point(220, 40),
                Width = 100
            };

            btnSaveTime.Click += (s, e) => {
                string timeStr = $"{dateTimePicker.Value.Year}, {dateTimePicker.Value.Month}, {dateTimePicker.Value.Day}, " +
                               $"{dateTimePicker.Value.Hour}, {dateTimePicker.Value.Minute}, {dateTimePicker.Value.Second}";
                File.WriteAllText("time.txt", timeStr);
                MessageBox.Show("Час збережено!", "Успіх");
            };

            Label lblStages = new Label
            {
                Text = "Управління етапами:",
                AutoSize = true,
                Location = new Point(10, 80)
            };

            Button btnClearStages = new Button
            {
                Text = "Очистити всі етапи",
                Location = new Point(10, 110),
                Width = 150
            };

            btnClearStages.Click += (s, e) => {
                if (MessageBox.Show("Ви впевнені, що хочете очистити всі етапи?", "Підтвердження",
                                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.WriteAllText("stages.txt", "Етапи: 0\n");
                    MessageBox.Show("Всі етапи очищено!", "Успіх");
                }
            };

            panel.Controls.Add(lblTime);
            panel.Controls.Add(dateTimePicker);
            panel.Controls.Add(btnSaveTime);
            panel.Controls.Add(lblStages);
            panel.Controls.Add(btnClearStages);

            return panel;
        }
    }
}
