using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class SetsView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView gridSets;
        private DataGridView gridEmplacements;
        private TextBox tbSearch;
        private Button btnSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public SetsView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = "🧱  Gestion des sets",
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

            tbSearch = new TextBox
            {
                Width = 260,
                Height = 34,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "🔍  Référence ou nom...",
                Margin = new Padding(0, 2, 8, 0)
            };
            btnSearch = CreateButton("Rechercher");
            btnAdd = CreateButton("➕  Ajouter");
            btnEdit = CreateButton("Modifier");
            btnDelete = CreateButton("🗑️  Supprimer");
            btnDelete.BackColor = Color.FromArgb(180, 50, 50);

            panelToolbar.Controls.Add(tbSearch);
            panelToolbar.Controls.Add(btnSearch);
            panelToolbar.Controls.Add(btnAdd);
            panelToolbar.Controls.Add(btnEdit);
            panelToolbar.Controls.Add(btnDelete);

            // SplitContainer pour les 2 grilles
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 300,
                SplitterWidth = 8,
                BackColor = BgColor,
                BorderStyle = BorderStyle.None
            };

            // Grille sets
            var lblSets = new Label { Text = "Sets", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = PrimaryColor, Dock = DockStyle.Top, Height = 28 };
            gridSets = new DataGridView
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
            StyleGrid(gridSets);
            split.Panel1.BackColor = Color.White;
            split.Panel1.Padding = new Padding(0, 0, 0, 4);
            split.Panel1.Controls.Add(gridSets);
            split.Panel1.Controls.Add(lblSets);

            // Grille emplacements
            var lblEmpl = new Label { Text = "Emplacements du set sélectionné", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = PrimaryColor, Dock = DockStyle.Top, Height = 28 };
            gridEmplacements = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            StyleGrid(gridEmplacements);
            split.Panel2.BackColor = Color.White;
            split.Panel2.Controls.Add(gridEmplacements);
            split.Panel2.Controls.Add(lblEmpl);

            Controls.Add(split);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);

            btnSearch.Click += BtnSearch_Click;
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            tbSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { BtnSearch_Click(s, e); e.SuppressKeyPress = true; } };
            gridSets.SelectionChanged += GridSets_SelectionChanged;
            Load += SetsView_Load;
        }

        private void SetsView_Load(object? sender, System.EventArgs e) => RefreshSets();

        private void RefreshSets(string search = "")
        {
            try
            {
                using var conn = _db.GetConnection();
                string query = "SELECT id, Reference AS Référence, nom AS Nom, AgeCible AS 'Âge cible', NombresPieces AS 'Nb pièces', quantiter AS Quantité FROM LegoSet";
                if (!string.IsNullOrWhiteSpace(search))
                    query += " WHERE Reference LIKE @search OR nom LIKE @search";
                query += " ORDER BY Reference";
                using var cmd = new MySqlCommand(query, conn);
                if (!string.IsNullOrWhiteSpace(search))
                    cmd.Parameters.AddWithValue("@search", $"%{search}%");
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                gridSets.DataSource = table;
                if (gridSets.Columns.Contains("id")) gridSets.Columns["id"].Visible = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement sets: {ex.Message}");
            }
        }

        private void BtnSearch_Click(object? sender, System.EventArgs e) => RefreshSets(tbSearch.Text.Trim());

        private void BtnAdd_Click(object? sender, System.EventArgs e)
        {
            using var dlg = new AddSetForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var conn = _db.GetConnection();

                    // Insérer le set
                    using var cmd = new MySqlCommand(
                        "INSERT INTO LegoSet (Reference, nom, AgeCible, NombresPieces, quantiter) VALUES (@ref, @nom, @age, @pieces, @qty)", conn);
                    cmd.Parameters.AddWithValue("@ref", dlg.Reference);
                    cmd.Parameters.AddWithValue("@nom", dlg.Nom);
                    cmd.Parameters.AddWithValue("@age", dlg.AgeCible);
                    cmd.Parameters.AddWithValue("@pieces", dlg.NombresPieces);
                    cmd.Parameters.AddWithValue("@qty", dlg.Quantite);
                    cmd.ExecuteNonQuery();

                    // Récupérer l'id du set inséré
                    long setId;
                    using (var cmdId = new MySqlCommand("SELECT LAST_INSERT_ID()", conn))
                        setId = Convert.ToInt64(cmdId.ExecuteScalar());

                    // Associer le set à l'emplacement choisi
                    using (var cmdStock = new MySqlCommand(
                        "INSERT INTO stocker (legoset_id, emplacement_id, quantiter) VALUES (@setId, @emplId, @qty2)", conn))
                    {
                        cmdStock.Parameters.AddWithValue("@setId", setId);
                        cmdStock.Parameters.AddWithValue("@emplId", dlg.SelectedEmplacementId);
                        cmdStock.Parameters.AddWithValue("@qty2", dlg.Quantite);
                        cmdStock.ExecuteNonQuery();
                    }

                    HistoriqueHelper.Log("Ajout set", $"Set '{dlg.Reference} — {dlg.Nom}' ajouté (qté: {dlg.Quantite})");
                    RefreshSets();
                    MessageBox.Show($"Set '{dlg.Reference}' ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    MessageBox.Show("Cette référence existe déjà.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur ajout: {ex.Message}");
                }
            }
        }

        private void BtnEdit_Click(object? sender, System.EventArgs e)
        {
            if (gridSets.CurrentRow == null)
            {
                MessageBox.Show("Selectionnez un set a modifier.");
                return;
            }
            var id = gridSets.CurrentRow.Cells["id"].Value;
            var reference = gridSets.CurrentRow.Cells["Référence"].Value?.ToString() ?? "";
            var nom = gridSets.CurrentRow.Cells["Nom"].Value?.ToString() ?? "";
            int age = 0; int.TryParse(gridSets.CurrentRow.Cells["Âge cible"].Value?.ToString(), out age);
            int pieces = 0; int.TryParse(gridSets.CurrentRow.Cells["Nb pièces"].Value?.ToString(), out pieces);
            int qty = 0; int.TryParse(gridSets.CurrentRow.Cells["Quantité"].Value?.ToString(), out qty);

            // Récupérer l'emplacement actuel
            int currentEmplId = 0;
            try
            {
                using var conn2 = _db.GetConnection();
                using var cmdEmpl = new MySqlCommand("SELECT emplacement_id FROM stocker WHERE legoset_id = @id LIMIT 1", conn2);
                cmdEmpl.Parameters.AddWithValue("@id", id);
                var result = cmdEmpl.ExecuteScalar();
                if (result != null) currentEmplId = System.Convert.ToInt32(result);
            }
            catch { }

            using var dlg = new EditSetForm(reference, nom, age, pieces, qty, currentEmplId);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var conn = _db.GetConnection();

                    // Mettre à jour le set
                    using (var cmd = new MySqlCommand(
                        "UPDATE LegoSet SET Reference = @ref, nom = @nom, AgeCible = @age, NombresPieces = @pieces, quantiter = @qty WHERE id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@ref", dlg.Reference);
                        cmd.Parameters.AddWithValue("@nom", dlg.Nom);
                        cmd.Parameters.AddWithValue("@age", dlg.AgeCible);
                        cmd.Parameters.AddWithValue("@pieces", dlg.NombresPieces);
                        cmd.Parameters.AddWithValue("@qty", dlg.Quantite);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    // Mettre à jour l'emplacement dans stocker
                    using (var cmdDel = new MySqlCommand("DELETE FROM stocker WHERE legoset_id = @id", conn))
                    {
                        cmdDel.Parameters.AddWithValue("@id", id);
                        cmdDel.ExecuteNonQuery();
                    }
                    using (var cmdIns = new MySqlCommand(
                        "INSERT INTO stocker (legoset_id, emplacement_id, quantiter) VALUES (@setId, @emplId, @qty2)", conn))
                    {
                        cmdIns.Parameters.AddWithValue("@setId", id);
                        cmdIns.Parameters.AddWithValue("@emplId", dlg.SelectedEmplacementId);
                        cmdIns.Parameters.AddWithValue("@qty2", dlg.Quantite);
                        cmdIns.ExecuteNonQuery();
                    }

                    HistoriqueHelper.Log("Modification set", $"Set '{dlg.Reference} \u2014 {dlg.Nom}' modifie");
                    RefreshSets();
                    MessageBox.Show($"Set '{dlg.Reference}' modifie avec succes.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    MessageBox.Show("Cette reference existe deja.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur modification: {ex.Message}");
                }
            }
        }

        private void BtnDelete_Click(object? sender, System.EventArgs e)
        {
            if (gridSets.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez un set à supprimer.");
                return;
            }
            var id = gridSets.CurrentRow.Cells["id"].Value;
            var reference = gridSets.CurrentRow.Cells["Référence"].Value?.ToString() ?? "";
            var nom = gridSets.CurrentRow.Cells["Nom"].Value?.ToString() ?? "";

            if (MessageBox.Show($"Supprimer le set '{reference} — {nom}' ?\n\nCette action est irréversible.",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    // Supprimer d'abord les liens dans la table stocker
                    using (var cmdStock = new MySqlCommand("DELETE FROM stocker WHERE legoset_id = @id", conn))
                    {
                        cmdStock.Parameters.AddWithValue("@id", id);
                        cmdStock.ExecuteNonQuery();
                    }
                    using var cmd = new MySqlCommand("DELETE FROM LegoSet WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    HistoriqueHelper.Log("Suppression set", $"Set '{reference} — {nom}' supprimé");
                    RefreshSets();
                    MessageBox.Show($"Set '{reference}' supprimé.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur suppression: {ex.Message}");
                }
            }
        }

        private void GridSets_SelectionChanged(object? sender, System.EventArgs e)
        {
            if (gridSets.CurrentRow == null) return;
            var setId = gridSets.CurrentRow.Cells["id"].Value;
            LoadEmplacements(setId);
        }

        private void LoadEmplacements(object setId)
        {
            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT e.code AS Emplacement, s.quantiter AS Quantité " +
                    "FROM stocker s " +
                    "JOIN Emplacement e ON e.id = s.emplacement_id " +
                    "WHERE s.legoset_id = @setId " +
                    "ORDER BY e.code", conn);
                cmd.Parameters.AddWithValue("@setId", setId);
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                gridEmplacements.DataSource = table;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement emplacements: {ex.Message}");
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
}