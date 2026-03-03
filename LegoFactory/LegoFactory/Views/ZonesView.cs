using System;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class ZonesView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView grid;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public ZonesView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = "Gestion des zones",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            panelHeader.Controls.Add(title);

            // Toolbar
            var panelToolbar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 46,
                BackColor = BgColor,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 4, 0, 4)
            };

            btnAdd = CreateButton("Ajouter");
            btnEdit = CreateButton("Modifier");
            btnDelete = CreateButton("Supprimer");
            btnDelete.BackColor = Color.FromArgb(180, 50, 50);

            panelToolbar.Controls.Add(btnAdd);
            panelToolbar.Controls.Add(btnEdit);
            panelToolbar.Controls.Add(btnDelete);

            // Grid
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            StyleGrid(grid);

            Controls.Add(grid);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            Load += ZonesView_Load;
        }

        private void ZonesView_Load(object? sender, EventArgs e) => RefreshGrid();

        private void RefreshGrid()
        {
            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT z.id, z.nom AS Nom, " +
                    "(SELECT COUNT(*) FROM Emplacement WHERE zone_id = z.id) AS 'Nb emplacements' " +
                    "FROM Zone z ORDER BY z.nom", conn);
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                grid.DataSource = table;
                if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur chargement zones: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            using var dlg = new AddZoneForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    // Vérifier doublon
                    var fullName = $"Zone {dlg.NomZone}";
                    using (var cmdCheck = new MySqlCommand("SELECT COUNT(*) FROM Zone WHERE UPPER(nom) = UPPER(@nom)", conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@nom", fullName);
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show($"La zone '{fullName}' existe deja.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    using var cmd = new MySqlCommand("INSERT INTO Zone (nom, entrepot_id) VALUES (@nom, 1)", conn);
                    cmd.Parameters.AddWithValue("@nom", fullName);
                    cmd.ExecuteNonQuery();
                    HistoriqueHelper.Log("Ajout zone", $"Zone '{fullName}' ajoutee");
                    RefreshGrid();
                    MessageBox.Show($"Zone '{fullName}' ajoutee avec succes.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    MessageBox.Show("Cette zone existe deja.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur ajout zone: {ex.Message}");
                }
            }
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Selectionnez une zone a modifier.");
                return;
            }
            var id = grid.CurrentRow.Cells["id"].Value;
            var nom = grid.CurrentRow.Cells["Nom"].Value?.ToString() ?? "";
            // Extraire juste la lettre (enlever "Zone " si present)
            var lettreActuelle = nom.StartsWith("Zone ", StringComparison.OrdinalIgnoreCase) ? nom.Substring(5) : nom;

            using var dlg = new EditZoneForm(lettreActuelle);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fullName = $"Zone {dlg.NomZone}";
                    using var conn = _db.GetConnection();
                    using var cmd = new MySqlCommand("UPDATE Zone SET nom = @nom WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@nom", fullName);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    HistoriqueHelper.Log("Modification zone", $"Zone '{nom}' renommee en '{fullName}'");
                    RefreshGrid();
                    MessageBox.Show($"Zone modifiee avec succes.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    MessageBox.Show("Ce nom de zone existe deja.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur modification: {ex.Message}");
                }
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Selectionnez une zone a supprimer.");
                return;
            }
            var id = grid.CurrentRow.Cells["id"].Value;
            var nom = grid.CurrentRow.Cells["Nom"].Value?.ToString() ?? "";

            try
            {
                using var conn = _db.GetConnection();

                // Verifier s'il y a des emplacements dans cette zone
                using (var cmdCheck = new MySqlCommand("SELECT COUNT(*) FROM Emplacement WHERE zone_id = @id", conn))
                {
                    cmdCheck.Parameters.AddWithValue("@id", id);
                    int nbEmpl = Convert.ToInt32(cmdCheck.ExecuteScalar());
                    if (nbEmpl > 0)
                    {
                        MessageBox.Show(
                            $"Impossible de supprimer la zone '{nom}'.\n\n" +
                            $"Elle contient encore {nbEmpl} emplacement(s). Veuillez d'abord supprimer ou deplacer les emplacements.",
                            "Suppression impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (MessageBox.Show($"Supprimer la zone '{nom}' ?\n\nCette action est irreversible.",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using var cmd = new MySqlCommand("DELETE FROM Zone WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    HistoriqueHelper.Log("Suppression zone", $"Zone '{nom}' supprimee");
                    RefreshGrid();
                    MessageBox.Show($"Zone '{nom}' supprimee.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur suppression: {ex.Message}");
            }
        }

        private static Button CreateButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Width = 120,
                Height = 34,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                BackColor = Color.FromArgb(30, 60, 114),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 2, 8, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private static void StyleGrid(DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6);
            g.ColumnHeadersHeight = 38;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            g.DefaultCellStyle.Padding = new Padding(4);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 220, 255);
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 253);
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = Color.FromArgb(230, 235, 242);
        }
    }

    public class AddZoneForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private TextBox tbNom;

        public string NomZone => tbNom.Text.Trim();

        public AddZoneForm()
        {
            Text = "Ajouter une zone";
            Size = new Size(380, 200);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);

            var lbl = new Label
            {
                Text = "Nom de la zone :",
                Location = new Point(20, 24),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = PrimaryColor
            };
            tbNom = new TextBox
            {
                Location = new Point(170, 22),
                Width = 170,
                Font = new Font("Segoe UI", 10F),
                MaxLength = 2,
                CharacterCasing = CharacterCasing.Upper,
                PlaceholderText = "Ex: AB"
            };
            tbNom.KeyPress += (s, e) =>
            {
                if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            };

            var lblInfo = new Label
            {
                Text = "2 lettres max, majuscules uniquement",
                Location = new Point(170, 46),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            var btnOk = new Button
            {
                Text = "Ajouter",
                Location = new Point(170, 80),
                Width = 80,
                Height = 34,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                Text = "Annuler",
                Location = new Point(260, 70),
                Width = 80,
                Height = 34,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderColor = PrimaryColor;

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(NomZone))
                {
                    MessageBox.Show("Le nom de la zone est obligatoire.", "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(NomZone, @"^[A-Z]{1,2}$"))
                {
                    MessageBox.Show("Le nom doit contenir 1 ou 2 lettres majuscules uniquement.", "Format invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.AddRange(new Control[] { lbl, tbNom, lblInfo, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }

    public class EditZoneForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private TextBox tbNom;

        public string NomZone => tbNom.Text.Trim();

        public EditZoneForm(string currentNom)
        {
            Text = "Modifier une zone";
            Size = new Size(380, 200);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);

            var lbl = new Label
            {
                Text = "Nom de la zone :",
                Location = new Point(20, 24),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = PrimaryColor
            };
            tbNom = new TextBox
            {
                Location = new Point(170, 22),
                Width = 170,
                Font = new Font("Segoe UI", 10F),
                MaxLength = 2,
                CharacterCasing = CharacterCasing.Upper,
                Text = currentNom
            };
            tbNom.KeyPress += (s, e) =>
            {
                if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            };

            var lblInfo = new Label
            {
                Text = "2 lettres max, majuscules uniquement",
                Location = new Point(170, 46),
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            var btnOk = new Button
            {
                Text = "Enregistrer",
                Location = new Point(160, 80),
                Width = 90,
                Height = 34,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                Text = "Annuler",
                Location = new Point(260, 80),
                Width = 80,
                Height = 34,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.FlatAppearance.BorderColor = PrimaryColor;

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(NomZone))
                {
                    MessageBox.Show("Le nom de la zone est obligatoire.", "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(NomZone, @"^[A-Z]{1,2}$"))
                {
                    MessageBox.Show("Le nom doit contenir 1 ou 2 lettres majuscules uniquement.", "Format invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.AddRange(new Control[] { lbl, tbNom, lblInfo, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}
