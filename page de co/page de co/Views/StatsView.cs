using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class StatsView : UserControl
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private Label lblTotalSets;
        private Label lblTotalEmplacements;
        private Label lblEmplacementsVides;
        private DataGridView gridSetsByZone;

        public StatsView()
        {
            var title = new Label { Text = "Statistiques & reporting", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            
            var lblStats = new Label { Text = "Résumé:", Location = new Point(20, 70), AutoSize = true, Font = new Font("Segoe UI", 12F, FontStyle.Bold) };
            lblTotalSets = new Label { Text = "Total sets: ...", Location = new Point(40, 110), AutoSize = true, Font = new Font("Segoe UI", 11F) };
            lblTotalEmplacements = new Label { Text = "Total emplacements: ...", Location = new Point(40, 140), AutoSize = true, Font = new Font("Segoe UI", 11F) };
            lblEmplacementsVides = new Label { Text = "Emplacements vides: ...", Location = new Point(40, 170), AutoSize = true, Font = new Font("Segoe UI", 11F) };
            
            var lblGrid = new Label { Text = "Sets par zone:", Location = new Point(20, 220), AutoSize = true, Font = new Font("Segoe UI", 12F, FontStyle.Bold) };
            gridSetsByZone = new DataGridView { Location = new Point(20, 250), Width = 800, Height = 400, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Controls.Add(title);
            Controls.Add(lblStats);
            Controls.Add(lblTotalSets);
            Controls.Add(lblTotalEmplacements);
            Controls.Add(lblEmplacementsVides);
            Controls.Add(lblGrid);
            Controls.Add(gridSetsByZone);

            Load += StatsView_Load;
        }

        private void StatsView_Load(object? sender, System.EventArgs e)
        {
            LoadStats();
        }

        private void LoadStats()
        {
            try
            {
                using var conn = _db.GetConnection();
                
                using var cmdSets = new MySqlCommand("SELECT COUNT(*) FROM LegoSet", conn);
                lblTotalSets.Text = $"Total sets: {cmdSets.ExecuteScalar()}";
                
                using var cmdEmpl = new MySqlCommand("SELECT COUNT(*) FROM Emplacement", conn);
                lblTotalEmplacements.Text = $"Total emplacements: {cmdEmpl.ExecuteScalar()}";
                
                using var cmdVides = new MySqlCommand("SELECT COUNT(*) FROM Emplacement e LEFT JOIN stocker s ON s.emplacement_id = e.id WHERE s.emplacement_id IS NULL", conn);
                lblEmplacementsVides.Text = $"Emplacements vides: {cmdVides.ExecuteScalar()}";
                
                using var cmdZones = new MySqlCommand(
                    "SELECT z.nom AS Zone, COUNT(DISTINCT s.legoset_id) AS 'Nombre de sets différents', SUM(s.quantiter) AS 'Quantité totale' " +
                    "FROM Zone z " +
                    "JOIN Emplacement e ON e.zone_id = z.id " +
                    "LEFT JOIN stocker s ON s.emplacement_id = e.id " +
                    "GROUP BY z.id, z.nom " +
                    "ORDER BY z.nom", conn);
                using var reader = cmdZones.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                gridSetsByZone.DataSource = table;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement stats: {ex.Message}");
            }
        }
    }
}