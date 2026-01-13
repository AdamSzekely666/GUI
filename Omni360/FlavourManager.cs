using MatroxLDS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace OmniCheck_360
{
    public partial class FlavourManagerForm : Form
    {
        private IniManager iniManager = new IniManager();
        private string iniPath = "app.ini"; // Change as needed

        private DataGridView dgv;
        private Button btnAdd;
        private Button btnClose;
        private MainForm mainForm;
        public FlavourManagerForm(MainForm mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            iniManager.LoadFromFile(iniPath);
            InitializeTable();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Font = new Font("Segoe UI", 16);

            // DataGridView
            this.dgv = new DataGridView();
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(12, 12);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(820, 420);
            this.dgv.TabIndex = 0;
            this.dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.Font = this.Font;
            this.dgv.RowTemplate.Height = 48;
            this.dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.dgv.ColumnHeadersHeight = 54;
            this.dgv.CellContentClick += new DataGridViewCellEventHandler(dgv_CellContentClick);
            this.dgv.CellClick += dgv_CellClick; // For custom keypad popup

            // Add button
            this.btnAdd = new Button();
            this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnAdd.Location = new System.Drawing.Point(712, 448);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 54);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add Recipe";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Font = this.Font;
            this.btnAdd.Click += new EventHandler(btnAdd_Click);

            // Close button (with image)
            this.btnClose = new Button();
            this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnClose.Location = new System.Drawing.Point(12, 448);
            this.btnClose.Size = new System.Drawing.Size(54, 54);
            this.btnClose.BackColor = Color.Transparent;
            try
            {
                // Use a relative path or resources for portability in production!
                this.btnClose.BackgroundImage = Image.FromFile("C:\\Users\\ZUser\\Desktop\\OmniCheck 5x\\Omni360\\Resources\\Exit48x48.png");
            }
            catch { /* ignore if image not found */ }
            this.btnClose.BackgroundImageLayout = ImageLayout.Stretch;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Click += (s, e) => this.Close();

            // Form settings
            this.ClientSize = new System.Drawing.Size(844, 514);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = false;
            this.Name = "FlavourManagerForm";
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.Text = "";

            // Add controls
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgv);

            this.ResumeLayout(false);
        }

        private void InitializeTable()
        {
            dgv.Columns.Clear();
            dgv.Rows.Clear();

            dgv.Columns.Add("Size", "Size");
            dgv.Columns.Add("Flavour", "Flavour");
            dgv.Columns.Add("RecipeType", "Recipe Type");
            dgv.Columns["Flavour"].ReadOnly = true;

            var editBtn = new DataGridViewButtonColumn();
            editBtn.Name = "Edit";
            editBtn.HeaderText = "";
            editBtn.Text = "Edit";
            editBtn.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(editBtn);

            var delBtn = new DataGridViewButtonColumn();
            delBtn.Name = "Delete";
            delBtn.HeaderText = "";
            delBtn.Text = "Delete";
            delBtn.UseColumnTextForButtonValue = true;
            dgv.Columns.Add(delBtn);

            // Optionally set preferred widths for larger font
            dgv.Columns["Size"].Width = 150;
            dgv.Columns["Flavour"].Width = 260;
            dgv.Columns["RecipeType"].Width = 200;

            foreach (var size in iniManager.SizeFlavourMap.Keys)
            {
                foreach (var flavour in iniManager.SizeFlavourMap[size])
                {
                    string key = $"{size}|{flavour}";
                    string type = iniManager.RecipeTypeMap.ContainsKey(key) ? iniManager.RecipeTypeMap[key] : "Default";
                    dgv.Rows.Add(size, flavour, type);
                }
            }
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv.Columns[e.ColumnIndex].Name == "Flavour")
            {
                using (var keypad = new MatroxLDS.PasswordForm())
                {
                    keypad.StartPosition = FormStartPosition.CenterParent;
                    if (keypad.ShowDialog(this) == DialogResult.OK)
                    {
                        string entered = keypad.Password;
                        if (!string.IsNullOrEmpty(entered))
                        {
                            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = entered;
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new EditMappingDialog(iniManager.Sizes);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string size = dlg.SelectedSize;
                string flavour = dlg.FlavourName;
                string type = dlg.RecipeType;

                // Add to data and update
                if (!iniManager.SizeFlavourMap.ContainsKey(size))
                    iniManager.SizeFlavourMap[size] = new List<string>();

                if (iniManager.SizeFlavourMap[size].Contains(flavour))
                {
                    MessageBox.Show("This flavour already exists for this size!", "Duplicate Recipe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    iniManager.SizeFlavourMap[size].Add(flavour);
                }
                iniManager.RecipeTypeMap[$"{size}|{flavour}"] = type;

                // --- Handle Green Bottle, Nitrous, or Special Bottle ---
                if (type == "Green Bottle" || type == "Nitrous" || type == "Special Bottle")
                {
                    string recipeType, mappingSection;
                    if (type == "Green Bottle")
                    {
                        recipeType = "GreenBottleRecipe";
                        mappingSection = "GreenBottle";
                    }
                    else if (type == "Nitrous")
                    {
                        recipeType = "NitrousRecipe";
                        mappingSection = "NitrousRecipes";
                    }
                    else
                    {
                        recipeType = "SpecialBottleRecipe";
                        mappingSection = "SpecialBottle";
                    }
                    string sectionName = $"{recipeType}_{size}_{flavour.Replace(" ", "_")}";

                    // 1. Ask user for template
                    var existingRecipes = GetRecipeSectionNames(recipeType);
                    var templateDlg = new RecipeTemplateSelectorDialog(existingRecipes);
                    if (templateDlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Dictionary<string, string> values;
                        if (!string.IsNullOrEmpty(templateDlg.SelectedSectionName))
                        {
                            values = LoadIniSection(templateDlg.SelectedSectionName);
                        }
                        else
                        {
                            values = new Dictionary<string, string>();
                            values["C1ExposureTxt"] = "";
                        }

                        // 2. Write special mapping section ([GreenBottle], [NitrousRecipes], or [SpecialBottle])
                        AddToSpecialMappingSection(mappingSection, size, flavour);

                        // 3. Write the recipe section itself
                        WriteIniSection(sectionName, values);
                    }
                }

                iniManager.SaveToFile(iniPath);
                iniManager.LoadFromFile(iniPath);
                InitializeTable();
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string size = dgv.Rows[e.RowIndex].Cells["Size"].Value.ToString();
            string flavour = dgv.Rows[e.RowIndex].Cells["Flavour"].Value.ToString();
            string key = $"{size}|{flavour}";

            if (dgv.Columns[e.ColumnIndex].Name == "Edit")
            {
                string oldType = iniManager.RecipeTypeMap.ContainsKey(key) ? iniManager.RecipeTypeMap[key] : "Default";

                var dlg = new EditMappingDialog(iniManager.Sizes, size, flavour, oldType);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Remove old mapping if size/flavour changed
                    if (size != dlg.SelectedSize || flavour != dlg.FlavourName)
                    {
                        iniManager.SizeFlavourMap[size].Remove(flavour);
                        if (!iniManager.SizeFlavourMap[dlg.SelectedSize].Contains(dlg.FlavourName))
                            iniManager.SizeFlavourMap[dlg.SelectedSize].Add(dlg.FlavourName);
                        iniManager.RecipeTypeMap.Remove(key);
                    }

                    // Remove old mapping if type changed (Green/Nitrous/Special Bottle)
                    if (oldType != dlg.RecipeType && (oldType == "Green Bottle" || oldType == "Nitrous" || oldType == "Special Bottle"))
                    {
                        iniManager.RemoveSpecialRecipeSections(iniPath, dlg.SelectedSize, dlg.FlavourName);
                        string oldRecipeSection =
                            (oldType == "Green Bottle" ? "GreenBottleRecipe" :
                             oldType == "Nitrous" ? "NitrousRecipe" :
                             oldType == "Special Bottle" ? "SpecialBottleRecipe" : "") +
                            $"_{dlg.SelectedSize}_{dlg.FlavourName.Replace(" ", "_")}";
                        RemoveIniSection(iniPath, oldRecipeSection);
                    }

                    // Add/update mapping for new type if needed
                    iniManager.RecipeTypeMap[$"{dlg.SelectedSize}|{dlg.FlavourName}"] = dlg.RecipeType;

                    // If new type is Green/Nitrous/Special Bottle, handle as before (add mapping, create section, etc)
                    if (dlg.RecipeType == "Green Bottle" || dlg.RecipeType == "Nitrous" || dlg.RecipeType == "Special Bottle")
                    {
                        string recipeType =
                            dlg.RecipeType == "Green Bottle" ? "GreenBottleRecipe" :
                            dlg.RecipeType == "Nitrous" ? "NitrousRecipe" :
                            dlg.RecipeType == "Special Bottle" ? "SpecialBottleRecipe" : "";
                        string mappingSection =
                            dlg.RecipeType == "Green Bottle" ? "GreenBottle" :
                            dlg.RecipeType == "Nitrous" ? "NitrousRecipes" :
                            dlg.RecipeType == "Special Bottle" ? "SpecialBottle" : "";
                        string sectionName = $"{recipeType}_{dlg.SelectedSize}_{dlg.FlavourName.Replace(" ", "_")}";

                        var existingRecipes = GetRecipeSectionNames(recipeType);
                        var templateDlg = new RecipeTemplateSelectorDialog(existingRecipes);
                        if (templateDlg.ShowDialog(this) == DialogResult.OK)
                        {
                            Dictionary<string, string> values;
                            if (!string.IsNullOrEmpty(templateDlg.SelectedSectionName))
                            {
                                values = LoadIniSection(templateDlg.SelectedSectionName);
                            }
                            else
                            {
                                values = new Dictionary<string, string>();
                                values["C1ExposureTxt"] = "";
                            }
                            AddToSpecialMappingSection(mappingSection, dlg.SelectedSize, dlg.FlavourName);
                            WriteIniSection(sectionName, values);
                        }
                    }
                    iniManager.SaveToFile(iniPath);
                    iniManager.LoadFromFile(iniPath);
                    InitializeTable();
                }
            }
            else if (dgv.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (mainForm != null
                     && string.Equals(size, mainForm.currentSize, StringComparison.OrdinalIgnoreCase)
                     && string.Equals(flavour, mainForm.currentFlavour, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("You cannot delete the currently active recipe. Please switch to another recipe before deleting.", "Cannot Delete Active Recipe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // ADD THIS BLOCK:
                if (MessageBox.Show("Are you sure you want to delete this flavour?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                iniManager.SizeFlavourMap[size].Remove(flavour);
                iniManager.RecipeTypeMap.Remove(key);
                if (iniManager.SizeFlavourMap[size].Count == 0)
                    iniManager.SizeFlavourMap.Remove(size);

                iniManager.RemoveSpecialRecipeSections(iniPath, size, flavour);

                iniManager.SaveToFile(iniPath);
                iniManager.LoadFromFile(iniPath);
                InitializeTable();
            }
        }

        // Returns all section names of the requested type from the INI (e.g. "GreenBottleRecipe" or "NitrousRecipe" or "SpecialBottleRecipe")
        private List<string> GetRecipeSectionNames(string recipeType)
        {
            var sectionNames = new List<string>();
            foreach (var line in File.ReadLines(iniPath))
            {
                var match = Regex.Match(line.Trim(), @"^\[(" + recipeType + @"_[^\]]+)\]$");
                if (match.Success)
                {
                    sectionNames.Add(match.Groups[1].Value);
                }
            }
            return sectionNames;
        }

        // Loads all key-value pairs from a section in the INI
        private Dictionary<string, string> LoadIniSection(string sectionName)
        {
            var dict = new Dictionary<string, string>();
            bool inSection = false;
            foreach (var line in File.ReadLines(iniPath))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    inSection = trimmed.Equals("[" + sectionName + "]");
                    continue;
                }
                if (inSection)
                {
                    if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith(";") || trimmed.StartsWith("#")) continue;
                    if (trimmed.StartsWith("[") && !trimmed.Equals("[" + sectionName + "]")) break;
                    var kv = trimmed.Split(new[] { '=' }, 2);
                    if (kv.Length == 2) dict[kv[0].Trim()] = kv[1].Trim();
                }
            }
            return dict;
        }

        // Writes a section (with key/value pairs) to the INI file. Appends or replaces existing section.
        private void WriteIniSection(string sectionName, Dictionary<string, string> values)
        {
            var lines = File.Exists(iniPath) ? File.ReadAllLines(iniPath).ToList() : new List<string>();

            int startIdx = lines.FindIndex(l => l.Trim().Equals("[" + sectionName + "]", StringComparison.OrdinalIgnoreCase));
            int endIdx = -1;
            if (startIdx >= 0)
            {
                endIdx = startIdx + 1;
                while (endIdx < lines.Count && !lines[endIdx].Trim().StartsWith("[")) endIdx++;
                lines.RemoveRange(startIdx, endIdx - startIdx);
            }

            int insertAt = lines.Count;
            if (sectionName.StartsWith("GreenBottleRecipe", StringComparison.OrdinalIgnoreCase))
            {
                insertAt = GetInsertAfterSection(lines, "GreenBottle");
            }
            else if (sectionName.StartsWith("NitrousRecipe", StringComparison.OrdinalIgnoreCase))
            {
                insertAt = GetInsertAfterSection(lines, "NitrousRecipes");
            }
            else if (sectionName.StartsWith("SpecialBottleRecipe", StringComparison.OrdinalIgnoreCase))
            {
                insertAt = GetInsertAfterSection(lines, "SpecialBottle");
            }

            lines.Insert(insertAt, "[" + sectionName + "]");
            foreach (var kv in values)
                lines.Insert(++insertAt, kv.Key + " = " + kv.Value);
            lines.Insert(++insertAt, "");

            File.WriteAllLines(iniPath, lines);
        }

        // Helper: Get the line index just after the end of a section
        private int GetInsertAfterSection(List<string> lines, string sectionName)
        {
            int idx = lines.FindIndex(l => l.Trim().Equals("[" + sectionName + "]", StringComparison.OrdinalIgnoreCase));
            if (idx < 0) return lines.Count;
            idx++;
            while (idx < lines.Count && !lines[idx].Trim().StartsWith("[")) idx++;
            return idx;
        }

        // Adds a Name entry to [GreenBottle], [NitrousRecipes], or [SpecialBottle] section
        private void AddToSpecialMappingSection(string section, string size, string flavour)
        {
            var lines = File.Exists(iniPath) ? File.ReadAllLines(iniPath).ToList() : new List<string>();
            int idx = lines.FindIndex(l => l.Trim().Equals("[" + section + "]", StringComparison.OrdinalIgnoreCase));
            string entry = $"Name={size}_{flavour.Replace(" ", "_")}";
            if (idx < 0)
            {
                lines.Add("");
                lines.Add("[" + section + "]");
                lines.Add(entry);
                lines.Add("");
            }
            else
            {
                int insertAt = idx + 1;
                while (insertAt < lines.Count && !lines[insertAt].Trim().StartsWith("[")) insertAt++;
                if (!lines.Skip(idx + 1).Take(insertAt - (idx + 1)).Any(l => l.Trim().Equals(entry, StringComparison.OrdinalIgnoreCase)))
                {
                    lines.Insert(insertAt, entry);
                }
            }
            File.WriteAllLines(iniPath, lines);
        }

        private void RemoveIniSection(string iniPath, string sectionName)
        {
            var lines = File.Exists(iniPath) ? File.ReadAllLines(iniPath).ToList() : new List<string>();
            int startIdx = lines.FindIndex(l => l.Trim().Equals("[" + sectionName + "]", StringComparison.OrdinalIgnoreCase));
            if (startIdx >= 0)
            {
                int endIdx = startIdx + 1;
                while (endIdx < lines.Count && !lines[endIdx].Trim().StartsWith("[")) endIdx++;
                lines.RemoveRange(startIdx, endIdx - startIdx);
                File.WriteAllLines(iniPath, lines);
            }
        }
    }

    public class RecipeTemplateSelectorDialog : Form
    {
        public string SelectedSectionName { get; private set; }

        public RecipeTemplateSelectorDialog(List<string> sectionNames)
        {
            this.Text = "Select Recipe Template";
            this.Width = 400;
            this.Height = 180;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 16);

            Label lbl = new Label() { Text = "Copy settings from existing recipe (optional):", Left = 20, Top = 20, Width = 340 };
            ComboBox cbSections = new ComboBox() { Left = 20, Top = 50, Width = 340, DropDownStyle = ComboBoxStyle.DropDownList, Font = this.Font };
            cbSections.Items.Add("(Blank/No Template)");
            foreach (var s in sectionNames) cbSections.Items.Add(s);
            cbSections.SelectedIndex = 0;

            Button btnOK = new Button() { Text = "OK", Left = 200, Width = 80, Top = 90, Height = 54, DialogResult = DialogResult.OK, Font = this.Font };
            Button btnCancel = new Button() { Text = "Cancel", Left = 290, Width = 80, Top = 90, Height = 54, DialogResult = DialogResult.Cancel, Font = this.Font };

            btnOK.Click += (s, e) =>
            {
                SelectedSectionName = cbSections.SelectedIndex <= 0 ? null : cbSections.SelectedItem.ToString();
                this.Close();
            };
            btnCancel.Click += (s, e) => { this.Close(); };

            this.Controls.Add(lbl);
            this.Controls.Add(cbSections);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
        }
    }

    // Edit dialog for mapping
    public class EditMappingDialog : Form
    {
        public string SelectedSize { get; private set; }
        public string FlavourName { get; private set; }
        public string RecipeType { get; private set; }

        private ComboBox cbSize;
        private TextBox tbFlavour;
        private ComboBox cbType;
        private Button btnOK;
        private Button btnCancel;

        public EditMappingDialog(List<string> sizes, string size = "", string flavour = "", string recipeType = "Default")
        {
            this.Text = "Edit Mapping";
            this.Width = 420;
            this.Height = 290;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 16);

            Label lblSize = new Label() { Text = "Size:", Left = 20, Top = 30, Width = 120, Font = this.Font };
            cbSize = new ComboBox() { Left = 150, Top = 26, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, Font = this.Font };
            cbSize.Items.AddRange(sizes.ToArray());
            cbSize.SelectedItem = string.IsNullOrEmpty(size) ? (cbSize.Items.Count > 0 ? cbSize.Items[0] : null) : size;

            Label lblFlavour = new Label() { Text = "Flavour:", Left = 20, Top = 80, Width = 120, Font = this.Font };
            tbFlavour = new TextBox() { Left = 150, Top = 76, Width = 220, Text = flavour, ReadOnly = true, Font = this.Font, TabStop = false };
            tbFlavour.Click += (s, e) =>
            {
                using (var keypad = new MatroxLDS.PasswordForm())
                {
                    keypad.StartPosition = FormStartPosition.CenterParent;
                    if (keypad.ShowDialog(this) == DialogResult.OK)
                    {
                        tbFlavour.Text = keypad.Password;
                    }
                }
            };

            Label lblType = new Label() { Text = "Recipe Type:", Left = 20, Top = 130, Width = 120, Font = this.Font };
            cbType = new ComboBox() { Left = 150, Top = 126, Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, Font = this.Font };
            cbType.Items.AddRange(new string[] { "Default", "Green Bottle", "Nitrous", "Special Bottle" });
            cbType.SelectedItem = string.IsNullOrEmpty(recipeType) ? "Default" : recipeType;

            btnOK = new Button() { Text = "OK", Left = 150, Width = 100, Top = 185, Height = 54, DialogResult = DialogResult.OK, Font = this.Font };
            btnCancel = new Button() { Text = "Cancel", Left = 270, Width = 100, Top = 185, Height = 54, DialogResult = DialogResult.Cancel, Font = this.Font };

            btnOK.Click += (s, e) =>
            {
                SelectedSize = cbSize.SelectedItem?.ToString() ?? "";
                FlavourName = tbFlavour.Text.Trim();
                RecipeType = cbType.SelectedItem?.ToString() ?? "Default";
                if (string.IsNullOrWhiteSpace(SelectedSize) || string.IsNullOrWhiteSpace(FlavourName))
                {
                    MessageBox.Show("Please enter both Size and Flavour.");
                    this.DialogResult = DialogResult.None;
                    return;
                }
                this.Close();
            };

            btnCancel.Click += (s, e) => { this.Close(); };

            this.Controls.Add(lblSize);
            this.Controls.Add(cbSize);
            this.Controls.Add(lblFlavour);
            this.Controls.Add(tbFlavour);
            this.Controls.Add(lblType);
            this.Controls.Add(cbType);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
        }
    }
}