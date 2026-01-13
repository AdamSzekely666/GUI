using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace MatroxLDS
{
    public partial class UserManagerForm : BaseForm
    {
        private Dictionary<string, Dictionary<string, int>> buttonPermissions;
        private List<User> users;
        public MainMenu mainMenu;
        private bool isDirty = false; // Tracks if access level changed

        public UserManagerForm(MainMenu _mainMenu)
        {
            InitializeComponent();
            this.Load += UserManagerForm_Load;
            ButtonAnimator.InitializeAnimation(MainMenuFormBtn, "blue");
            mainMenu = _mainMenu;
            userdataGridView.SelectionChanged += userdataGridView_SelectionChanged;
            userdataGridView.CellContentClick += userdataGridView_CellContentClick;
        }

        private void UserManagerForm_Load(object sender, EventArgs e)
        {
            SyncButtonsWithJson();
            LoadConfig();
            LoadUsers();
            LoadButtons();
            AddCustomColumnsToUserGrid();

            CurrentUserTxt.Text = mainMenu.mainForm.CurrentUserTxt.Text;
            DateTimeLabel.Text = mainMenu.mainForm.DateTimeLabel.Text;

            if (userdataGridView.Rows.Count > 0)
                userdataGridView.Rows[0].Selected = true;

            UpdateButtonLabelsForSelectedUser();
        }

        private void LoadConfig()
        {
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            var json = File.ReadAllText(configFilePath);
            var config = JsonConvert.DeserializeObject<Configuration>(json);
            users = config.Users ?? new List<User>();
            buttonPermissions = config.Buttons ?? new Dictionary<string, Dictionary<string, int>>();
        }

        /// <summary>
        /// Load users, add action columns, and append "Add" row.
        /// </summary>
        private void LoadUsers()
        {
            userdataGridView.DataSource = null;
            userdataGridView.AutoGenerateColumns = false;
            userdataGridView.Rows.Clear();
            userdataGridView.Columns.Clear();

            // Add user columns explicitly
            userdataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Username",
                Name = "Username",
                HeaderText = "Username",
                Width = 200
            });
            userdataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Password",
                Name = "Password",
                HeaderText = "Password",
                Width = 160
            });
            userdataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SecurityLevel",
                Name = "SecurityLevel",
                HeaderText = "Access Level",
                Width = 90

            });

            AddCustomColumnsToUserGrid();

            // Add all users
            foreach (var user in users)
            {
                int rowIdx = userdataGridView.Rows.Add();
                var row = userdataGridView.Rows[rowIdx];
                row.Cells["Username"].Value = user.Username;
                row.Cells["Password"].Value = user.Password;
                row.Cells["SecurityLevel"].Value = user.SecurityLevel;
            }

            // Add the "Add" button row (empty row)
            int addRowIdx = userdataGridView.Rows.Add();
            var addRow = userdataGridView.Rows[addRowIdx];
            addRow.Cells["Add"].Value = "Add";
            addRow.DefaultCellStyle.BackColor = Color.LightGreen;
        }

        /// <summary>
        /// Add Save, Edit, Delete, Add columns if not present and set their order.
        /// </summary>
        private void AddCustomColumnsToUserGrid()
        {
            if (!userdataGridView.Columns.Contains("Save"))
            {
                var saveBtnCol = new DataGridViewButtonColumn
                {
                    Name = "Save",
                    HeaderText = "Save",
                    Text = "",
                    UseColumnTextForButtonValue = false,
                    Width = 120
                };
                userdataGridView.Columns.Add(saveBtnCol);
            }
            if (!userdataGridView.Columns.Contains("Edit"))
            {
                var editBtnCol = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 70
                };
                userdataGridView.Columns.Add(editBtnCol);
            }
            if (!userdataGridView.Columns.Contains("Delete"))
            {
                var deleteBtnCol = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    Width = 70
                };
                userdataGridView.Columns.Add(deleteBtnCol);
            }
            if (!userdataGridView.Columns.Contains("Add"))
            {
                var addBtnCol = new DataGridViewButtonColumn
                {
                    Name = "Add",
                    HeaderText = "Add",
                    Text = "",
                    UseColumnTextForButtonValue = false,
                    Width = 70
                };
                userdataGridView.Columns.Add(addBtnCol);
            }

            int colCount = userdataGridView.Columns.Count;
            userdataGridView.Columns["Save"].DisplayIndex = colCount - 4;
            userdataGridView.Columns["Edit"].DisplayIndex = colCount - 3;
            userdataGridView.Columns["Delete"].DisplayIndex = colCount - 2;
            userdataGridView.Columns["Add"].DisplayIndex = colCount - 1;
        }

        /// <summary>
        /// Instead of checkboxes, display each button and its required level.
        /// Highlight accessible ones.
        /// </summary>
        private void LoadButtons()
        {
            flowLayoutPanelButtons.Controls.Clear();

            // Info label at top
            var infoLbl = new Label
            {
                Text = "Access to buttons is determined by the user's overall Access Level (not per-button).",
                AutoSize = true,
                Font = new Font(DefaultFont, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Padding = new Padding(2),
                Margin = new Padding(2)
            };
            flowLayoutPanelButtons.Controls.Add(infoLbl);

            // --- MainForm group ---
            flowLayoutPanelButtons.Controls.Add(new Label()
            {
                Text = "MainForm Buttons",
                AutoSize = true,
                Font = new Font(DefaultFont, FontStyle.Bold),
                ForeColor = Color.Blue
            });

            var mainFormButtons = typeof(MainForm).GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(Button))
                .Select(f => f.Name);

            foreach (var buttonName in mainFormButtons)
            {
                int requiredLevel = buttonPermissions.TryGetValue("MainForm", out var formBtns) && formBtns.TryGetValue(buttonName, out var lvl)
                    ? lvl : 0;

                Label lbl = new Label
                {
                    Text = $"MainForm - {buttonName} (Level: {requiredLevel})",
                    AutoSize = true,
                    Margin = new Padding(4, 2, 4, 2),
                    ForeColor = (SelectedUser != null && SelectedUser.SecurityLevel >= requiredLevel) ? Color.DarkGreen : Color.Gray,
                    Font = (SelectedUser != null && SelectedUser.SecurityLevel >= requiredLevel)
                        ? new Font(DefaultFont, FontStyle.Bold)
                        : DefaultFont
                };
                flowLayoutPanelButtons.Controls.Add(lbl);
            }

            // --- Separator ---
            flowLayoutPanelButtons.Controls.Add(new Panel()
            {
                Height = 2,
                Width = flowLayoutPanelButtons.Width - 10,
                BackColor = Color.Gray,
                Margin = new Padding(3, 8, 3, 8)
            });

            // --- MainMenu group ---
            flowLayoutPanelButtons.Controls.Add(new Label()
            {
                Text = "MainMenu Buttons",
                AutoSize = true,
                Font = new Font(DefaultFont, FontStyle.Bold),
                ForeColor = Color.Green
            });

            var mainMenuButtons = typeof(MainMenu).GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.FieldType == typeof(Button))
                .Select(f => f.Name);

            foreach (var buttonName in mainMenuButtons)
            {
                int requiredLevel = buttonPermissions.TryGetValue("MainMenu", out var menuBtns) && menuBtns.TryGetValue(buttonName, out var lvl)
                    ? lvl : 0;

                Label lbl = new Label
                {
                    Text = $"MainMenu - {buttonName} (Level: {requiredLevel})",
                    AutoSize = true,
                    Margin = new Padding(4, 2, 4, 2),
                    ForeColor = (SelectedUser != null && SelectedUser.SecurityLevel >= requiredLevel) ? Color.DarkGreen : Color.Gray,
                    Font = (SelectedUser != null && SelectedUser.SecurityLevel >= requiredLevel)
                        ? new Font(DefaultFont, FontStyle.Bold)
                        : DefaultFont
                };
                flowLayoutPanelButtons.Controls.Add(lbl);
            }
        }

        private User SelectedUser
        {
            get
            {
                if (userdataGridView.SelectedRows.Count == 0) return null;
                var selectedRow = userdataGridView.SelectedRows[0];
                int idx = selectedRow.Index;
                // If selected row is the Add row, ignore
                if (idx >= users.Count) return null;
                return users[idx];
            }
        }

        /// <summary>
        /// Refresh button labels with proper highlighting for the selected user.
        /// </summary>
        private void UpdateButtonLabelsForSelectedUser()
        {
            // Remove all labels except the top info and section headings
            LoadButtons();
            isDirty = false;
            ResetSaveButtonCell();
        }

        // Only show SAVE button if isDirty and for selected row
        private void ShowSaveButtonCell()
        {
            if (userdataGridView.SelectedRows.Count == 1)
            {
                var row = userdataGridView.SelectedRows[0];
                // Only show for user rows, not Add row
                if (row.Index < users.Count)
                    row.Cells["Save"].Value = "Save";
            }
        }

        private void ResetSaveButtonCell()
        {
            foreach (DataGridViewRow row in userdataGridView.Rows)
            {
                if (row.Index < users.Count)
                    row.Cells["Save"].Value = "";
            }
        }

        private void userdataGridView_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonLabelsForSelectedUser();
        }

        /// <summary>
        /// Save the edited user access level.
        /// </summary>
        private void SaveUserAccessLevel(int rowIndex)
        {
            if (rowIndex >= users.Count) return;
            var user = users[rowIndex];

            // Get updated access level from grid
            var cellValue = userdataGridView.Rows[rowIndex].Cells["SecurityLevel"].Value;
            if (cellValue != null && int.TryParse(cellValue.ToString(), out int newLevel))
            {
                user.SecurityLevel = newLevel;
                SaveToJson();
                LoadUsers();
                ResetSaveButtonCell();
                isDirty = false;
                UpdateButtonLabelsForSelectedUser();
            }
        }

        private void EditUser(int rowIndex)
        {
            if (rowIndex >= users.Count) return;
            var user = users[rowIndex];

            string newUsername = ShowKeyboardInput("Edit Username:", user.Username);
            user.Username = newUsername;

            string newPassword = ShowKeyboardInput("Edit Password:", user.Password);
            user.Password = newPassword;

            string newAccess = ShowKeyboardInput("Edit Access Level (1-5):", user.SecurityLevel.ToString());
            if (int.TryParse(newAccess, out int accessLevel)) user.SecurityLevel = accessLevel;

            SaveToJson();
            LoadUsers();
            UpdateButtonLabelsForSelectedUser();
        }

        // Replace DeleteUser(int rowIndex) in UserManagerForm with this version
        private void DeleteUser(int rowIndex)
        {
            if (rowIndex >= users.Count) return;
            var user = users[rowIndex];
            var confirm = MessageBox.Show($"Are you sure you want to delete user '{user.Username}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // 1) Remove any card mappings for this username from UsersManager (so deleted user cannot login)
                try
                {
                    if (mainMenu != null && mainMenu.mainForm != null)
                    {
                        var mgr = mainMenu.mainForm.GetUsersManager();
                        if (mgr != null)
                        {
                            var mappings = mgr.GetAllMappings()
                                              .Where(kv => string.Equals(kv.Value, user.Username, StringComparison.OrdinalIgnoreCase))
                                              .ToList();

                            foreach (var kv in mappings)
                            {
                                // remove the mapping
                                mgr.RemoveCardFromUser(user.Username, kv.Key);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log but continue with deletion
                    Debug.WriteLine("Warning: failed to remove card mappings for deleted user: " + ex.Message);
                }

                // 2) Remove user from the in-memory list and persist
                users.RemoveAt(rowIndex);
                SaveToJson();

                // 3) Refresh the user grid and UI
                LoadUsers();
                UpdateButtonLabelsForSelectedUser();

                // 4) If the deleted user was currently logged in, force logout immediately
                try
                {
                    if (mainMenu != null && mainMenu.mainForm != null)
                    {
                        var mf = mainMenu.mainForm;
                        if (string.Equals(mf.currentUserName, user.Username, StringComparison.OrdinalIgnoreCase))
                        {
                            mf.LogOutToOperatingUser();
                            // Also notify subscribers (MainForm.LogOutToOperatingUser will call OnUserStateChanged if configured)
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Warning: failed to force logout after deleting user: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AddUser()
        {
            string username = ShowKeyboardInput("Enter Username:", "");
            if (string.IsNullOrWhiteSpace(username)) return;

            string password = ShowKeyboardInput("Enter Password:", "");
            if (string.IsNullOrWhiteSpace(password)) return;

            string accessStr = ShowKeyboardInput("Enter Access Level (1-5):", "1");
            if (!int.TryParse(accessStr, out int accessLevel) || accessLevel < 1 || accessLevel > 5)
            {
                MessageBox.Show("Invalid access level.");
                return;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                SecurityLevel = accessLevel
            };
            users.Add(newUser);
            SaveToJson();
            LoadUsers();
            UpdateButtonLabelsForSelectedUser();
        }

        /// <summary>
        /// Handles Save/Edit/Delete/Add button clicks in grid.
        /// </summary>
        private void userdataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var colName = userdataGridView.Columns[e.ColumnIndex].Name;

            if (colName == "Save")
            {
                if (e.RowIndex < users.Count && userdataGridView.Rows[e.RowIndex].Cells["Save"].Value?.ToString() == "Save")
                {
                    SaveUserAccessLevel(e.RowIndex);
                }
            }
            else if (colName == "Edit")
            {
                if (e.RowIndex < users.Count)
                    EditUser(e.RowIndex);
            }
            else if (colName == "Delete")
            {
                if (e.RowIndex < users.Count)
                    DeleteUser(e.RowIndex);
            }
            else if (colName == "Add")
            {
                // Only allow Add on the last row
                if (e.RowIndex == userdataGridView.Rows.Count - 1)
                {
                    AddUser();
                }
            }
        }

        private void SaveToJson()
        {
            try
            {
                var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

                // Build a JObject representing the configuration (Users, Buttons, Cards)
                var root = new JObject();

                // Users
                var usersArray = JArray.FromObject(users);
                root["Users"] = usersArray;

                // Buttons (permissions)
                root["Buttons"] = JObject.FromObject(buttonPermissions ?? new Dictionary<string, Dictionary<string, int>>());

                // Cards: try to get current mappings from main form's UsersManager if available
                try
                {
                    Dictionary<string, string> cardsDict = new Dictionary<string, string>();
                    if (mainMenu != null && mainMenu.mainForm != null)
                    {
                        var mgr = mainMenu.mainForm.GetUsersManager();
                        if (mgr != null)
                        {
                            cardsDict = mgr.GetAllMappings().ToDictionary(k => k.Key, v => v.Value);
                        }
                    }
                    root["Cards"] = JObject.FromObject(cardsDict);
                }
                catch
                {
                    // if something goes wrong retrieving cards, skip cards to avoid overwrite
                }

                // Atomic write
                var json = root.ToString(Formatting.Indented);
                var tmp = configFilePath + ".tmp";
                File.WriteAllText(tmp, json);
                File.Copy(tmp, configFilePath, true);
                File.Delete(tmp);

                // NEW: notify MainForm to reload users and permissions so the running app sees the change immediately
                try
                {
                    if (mainMenu != null && mainMenu.mainForm != null)
                    {
                        mainMenu.mainForm.ReloadUsersFromConfig();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to notify main form to reload users: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving JSON: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public int SecurityLevel { get; set; }
        }

        private class Configuration
        {
            public List<User> Users { get; set; }
            public Dictionary<string, Dictionary<string, int>> Buttons { get; set; }
        }

        private void SyncButtonsWithJson()
        {
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            var json = File.ReadAllText(configFilePath);
            var config = JsonConvert.DeserializeObject<Configuration>(json);

            var existingButtons = config.Buttons;
            var buttonNames = new List<(string formName, string buttonName)>();
            var formTypes = new[] { typeof(MainForm), typeof(MainMenu) };

            foreach (var formType in formTypes)
            {
                var buttons = formType.GetFields(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.FieldType == typeof(Button))
                    .Select(f => (formType.Name, f.Name));
                buttonNames.AddRange(buttons);
            }

            foreach (var (formName, buttonName) in buttonNames)
            {
                if (!existingButtons.ContainsKey(formName))
                {
                    existingButtons[formName] = new Dictionary<string, int>();
                }

                if (!existingButtons[formName].ContainsKey(buttonName))
                {
                    existingButtons[formName][buttonName] = 0;
                }
            }

            var updatedJson = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(configFilePath, updatedJson);
        }

        private string ShowKeyboardInput(string prompt, string defaultValue = "")
        {
            PasswordForm inputForm = new PasswordForm();
            inputForm.Text = prompt;
            inputForm.activeTextBox.Text = defaultValue;
            var result = inputForm.ShowDialog();
            return result == DialogResult.OK ? inputForm.Password : defaultValue;
        }

        private void MainMenuFormBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        // Paste this method into UserManagerForm class. Call it from your Enroll button click event.
        private void btnEnrollCard_Click(object sender, EventArgs e)
        {
            // Ensure a user row is selected
            if (userdataGridView.SelectedRows.Count == 0) return;
            int idx = userdataGridView.SelectedRows[0].Index;
            if (idx >= users.Count) return;

            var username = users[idx].Username;

            // Get the shared UsersManager instance from MainForm (must exist)
            UsersManager mgr = null;
            try
            {
                if (mainMenu != null && mainMenu.mainForm != null)
                {
                    mgr = mainMenu.mainForm.GetUsersManager();
                }
            }
            catch { mgr = null; }

            if (mgr == null)
            {
                MessageBox.Show("Cannot access UsersManager. Ensure the main form has initialized the card manager.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Use the singleton CardReaderService instance (no cast to ICardReader)
            var readerService = CardReaderService.Instance;
            if (readerService == null)
            {
                MessageBox.Show("Card reader service is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var dlg = new EnrollCardForm(mgr, readerService, username))
            {
                dlg.ShowDialog(this);
            }
        }
    }
}