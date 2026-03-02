using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
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

            // Toolbar recherche
            var panelToolbar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = BgColor, Padding = new Padding(0, 6, 0, 6) };
            var lblSearch = new Label { Text = "🔍", Font = new Font("Segoe UI Emoji", 11F), AutoSize = true, Location = new Point(0, 8) };
            tbSearch = new TextBox { Location = new Point(30, 6), Width = 300, Height = 30, Font = new Font("Segoe UI", 10F), PlaceholderText = "Référence ou nom..." };
            btnSearch = CreateButton("Rechercher", 345, 3);
            panelToolbar.Controls.AddRange(new Control[] { lblSearch, tbSearch, btnSearch });

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
                string query = "SELECT id, Reference, nom, AgeCible, NombresPieces, quantiter FROM LegoSet";
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
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement sets: {ex.Message}");
            }
        }

        private void BtnSearch_Click(object? sender, System.EventArgs e) => RefreshSets(tbSearch.Text.Trim());

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

        private static Button CreateButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Width = 120,
                Height = 34,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(30, 60, 114),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
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