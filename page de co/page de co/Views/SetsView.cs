using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class SetsView : UserControl
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView gridSets;
        private DataGridView gridEmplacements;
        private TextBox tbSearch;
        private Button btnSearch;

        public SetsView()
        {
            var title = new Label { Text = "Gestion des sets", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            
            var lblSearch = new Label { Text = "Rechercher:", Location = new Point(20, 60), AutoSize = true };
            tbSearch = new TextBox { Location = new Point(110, 58), Width = 300, PlaceholderText = "Référence ou nom" };
            btnSearch = new Button { Text = "Rechercher", Location = new Point(420, 56), Width = 100 };
            
            var lblSets = new Label { Text = "Sets:", Location = new Point(20, 100), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            gridSets = new DataGridView { Location = new Point(20, 130), Width = 1050, Height = 280, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
            
            var lblEmpl = new Label { Text = "Emplacements du set sélectionné:", Location = new Point(20, 430), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            gridEmplacements = new DataGridView { Location = new Point(20, 460), Width = 1050, Height = 200, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Controls.Add(title);
            Controls.Add(lblSearch);
            Controls.Add(tbSearch);
            Controls.Add(btnSearch);
            Controls.Add(lblSets);
            Controls.Add(gridSets);
            Controls.Add(lblEmpl);
            Controls.Add(gridEmplacements);

            btnSearch.Click += BtnSearch_Click;
            gridSets.SelectionChanged += GridSets_SelectionChanged;
            Load += SetsView_Load;
        }

        private void SetsView_Load(object? sender, System.EventArgs e)
        {
            RefreshSets();
        }

        private void RefreshSets(string search = "")
        {
            try
            {
                using var conn = _db.GetConnection();
                string query = "SELECT id, Reference, nom, AgeCible, NombresPieces, quantiter FROM LegoSet";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query += " WHERE Reference LIKE @search OR nom LIKE @search";
                }
                query += " ORDER BY Reference";
                using var cmd = new MySqlCommand(query, conn);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    cmd.Parameters.AddWithValue("@search", $"%{search}%");
                }
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

        private void BtnSearch_Click(object? sender, System.EventArgs e)
        {
            RefreshSets(tbSearch.Text.Trim());
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
    }
}