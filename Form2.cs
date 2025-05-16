using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using LR4.Interfaces;
using LR4.Core;

namespace LR4
{
    public partial class Form2 : Form
    {
        private readonly AdminPanelService _adminPanelService;
        private TabControl _tabControl;

        public Form2(IAdminService adminService)
        {
            _adminPanelService = new AdminPanelService(adminService);
            InitializeComponent();
            this.Text = "Адміністративна панель";
            this.Size = new Size(600, 400);
            CreateAdminTabs();
        }

        private void CreateAdminTabs()
        {
            _tabControl = new TabControl { Dock = DockStyle.Fill };
            
            _tabControl.Controls.Add(CreateUsersTab());
            _tabControl.Controls.Add(CreateCandidatesTab());
            _tabControl.Controls.Add(CreateResultsTab());
            _tabControl.Controls.Add(CreateSettingsTab());

            this.Controls.Add(_tabControl);
        }

        private TabPage CreateUsersTab()
        {
            var tab = new TabPage("Користувачі");
            var panel = new Panel { Dock = DockStyle.Fill };
            
            var pendingUsers = _adminPanelService.GetPendingUsers();
            int y = 10;

            foreach (string line in pendingUsers)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(';');
                if (parts.Length < 3) continue;

                string formattedText = $"Ім'я: {parts[0]} | Прізвище: {parts[1]} | Номер паспорта: {parts[2]}";

                var lbl = new Label
                {
                    Text = formattedText,
                    AutoSize = true,
                    Location = new Point(10, y)
                };

                var btn = new Button
                {
                    Text = "Підтвердити",
                    Tag = line,
                    Location = new Point(400, y - 3)
                };
                btn.Click += (s, e) => {
                    _adminPanelService.ApproveUser(line);
                    panel.Controls.Remove(lbl);
                    panel.Controls.Remove(btn);
                };

                panel.Controls.Add(lbl);
                panel.Controls.Add(btn);
                y += 30;
            }

            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateCandidatesTab()
        {
            var tab = new TabPage("Кандидати");
            var panel = new Panel { Dock = DockStyle.Fill };

            var listBox = new ListBox
            {
                Width = 300,
                Height = 200,
                Location = new Point(10, 10)
            };

            foreach (var candidate in _adminPanelService.GetCandidates())
            {
                listBox.Items.Add(candidate);
            }

            var txtNewCandidate = new TextBox
            {
                Width = 200,
                Location = new Point(10, 220)
            };

            var btnAdd = new Button
            {
                Text = "Додати",
                Location = new Point(220, 220),
                Width = 80
            };
            btnAdd.Click += (s, e) => {
                _adminPanelService.AddCandidate(txtNewCandidate.Text);
                listBox.Items.Add(txtNewCandidate.Text);
                txtNewCandidate.Clear();
            };

            var btnRemove = new Button
            {
                Text = "Видалити",
                Location = new Point(10, 250),
                Width = 80
            };
            btnRemove.Click += (s, e) => {
                if (listBox.SelectedItem != null)
                {
                    _adminPanelService.RemoveCandidate(listBox.SelectedItem.ToString());
                    listBox.Items.Remove(listBox.SelectedItem);
                }
            };

            panel.Controls.Add(listBox);
            panel.Controls.Add(txtNewCandidate);
            panel.Controls.Add(btnAdd);
            panel.Controls.Add(btnRemove);

            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateResultsTab()
        {
            var tab = new TabPage("Результати");
            var panel = new Panel { Dock = DockStyle.Fill };

            var listBox = new ListBox
            {
                Width = 400,
                Height = 300,
                Location = new Point(10, 10)
            };

            var lblStats = new Label
            {
                AutoSize = true,
                Location = new Point(10, 320),
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold)
            };

            var results = _adminPanelService.GetVotingResults();
            int totalVotes = results.Values.Sum();

            foreach (var kvp in results.OrderByDescending(r => r.Value))
            {
                double percentage = (double)kvp.Value / totalVotes * 100;
                listBox.Items.Add($"{kvp.Key}: {kvp.Value} голосів ({percentage:F1}%)");
            }

            lblStats.Text = $"Всього голосів: {totalVotes}";
            panel.Controls.Add(listBox);
            panel.Controls.Add(lblStats);
            tab.Controls.Add(panel);
            return tab;
        }

        private TabPage CreateSettingsTab()
        {
            var tab = new TabPage("Налаштування");
            var panel = new Panel { Dock = DockStyle.Fill };

            var lblTime = new Label
            {
                Text = "Редагувати час:",
                AutoSize = true,
                Location = new Point(10, 10)
            };

            var dateTimePicker = new DateTimePicker
            {
                Location = new Point(10, 40),
                Width = 200,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy, MM, dd, HH, mm, ss"
            };

            var currentTime = _adminPanelService.GetCurrentVotingTime();
            if (currentTime.HasValue)
            {
                dateTimePicker.Value = currentTime.Value;
            }

            var btnSaveTime = new Button
            {
                Text = "Зберегти час",
                Location = new Point(220, 40),
                Width = 100
            };
            btnSaveTime.Click += (s, e) => {
                _adminPanelService.SaveVotingTime(dateTimePicker.Value);
                MessageBox.Show("Час збережено!", "Успіх");
            };

            var lblStages = new Label
            {
                Text = "Управління етапами:",
                AutoSize = true,
                Location = new Point(10, 80)
            };

            var btnClearStages = new Button
            {
                Text = "Очистити всі етапи",
                Location = new Point(10, 110),
                Width = 150
            };
            btnClearStages.Click += (s, e) => {
                if (MessageBox.Show("Ви впевнені, що хочете очистити всі етапи?", "Підтвердження",
                                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _adminPanelService.ClearStages();
                    MessageBox.Show("Всі етапи очищено!", "Успіх");
                }
            };

            panel.Controls.Add(lblTime);
            panel.Controls.Add(dateTimePicker);
            panel.Controls.Add(btnSaveTime);
            panel.Controls.Add(lblStages);
            panel.Controls.Add(btnClearStages);

            tab.Controls.Add(panel);
            return tab;
    }
}
}
