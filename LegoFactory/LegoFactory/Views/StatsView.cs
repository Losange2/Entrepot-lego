using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class StatsView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private Label lblTotalSets;
        private Label lblTotalEmplacements;
        private Label lblEmplacementsVides;
        private DataGridView gridSetsByZone;

        public StatsView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = "📊  Statistiques & reporting",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            panelHeader.Controls.Add(title);

            // Stat cards row
            var panelCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 100,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = BgColor,
                Padding = new Padding(0, 8, 0, 8)
            };

            lblTotalSets = CreateStatCard("🧱", "Total sets", "...");
            lblTotalEmplacements = CreateStatCard("📍", "Emplacements", "...");
            lblEmplacementsVides = CreateStatCard("📭", "Vides", "...");

            // Grid
            var panelGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 8, 0, 0) };
            var lblGrid = new Label
            {
                Text = "Sets par zone",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = PrimaryColor
            };
            gridSetsByZone = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            StyleGrid(gridSetsByZone);
            panelGrid.Controls.Add(gridSetsByZone);
            panelGrid.Controls.Add(lblGrid);

            // Find parent panels of stat cards
            panelCards.Controls.Add(lblTotalSets.Parent);
            panelCards.Controls.Add(lblTotalEmplacements.Parent);
            panelCards.Controls.Add(lblEmplacementsVides.Parent);

            Controls.Add(panelGrid);
            Controls.Add(panelCards);
            Controls.Add(panelHeader);

            Load += StatsView_Load;
        }

        private Label CreateStatCard(string icon, string label, string value)
        {
            var card = new Panel
            {
                Width = 200,
                Height = 80,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 16, 0)
            };
            card.Paint += (s, e) =>
            {
                using var path = RoundedRect(card.ClientRectangle, 10);
                card.Region = new Region(path);
            };
            var lblIcon = new Label
            {
                Text = icon,
                Font = new Font("Segoe UI", 13F),
                Location = new Point(14, 20),
                AutoSize = true
            };
            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(50, 12),
                AutoSize = true
            };
            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Location = new Point(50, 34),
                AutoSize = true
            };
            card.Controls.Add(lblIcon);
            card.Controls.Add(lblLabel);
            card.Controls.Add(lblValue);
            return lblValue;
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
                lblTotalSets.Text = cmdSets.ExecuteScalar()?.ToString() ?? "0";
                
                using var cmdEmpl = new MySqlCommand("SELECT COUNT(*) FROM Emplacement", conn);
                lblTotalEmplacements.Text = cmdEmpl.ExecuteScalar()?.ToString() ?? "0";
                
                using var cmdVides = new MySqlCommand("SELECT COUNT(*) FROM Emplacement e LEFT JOIN stocker s ON s.emplacement_id = e.id WHERE s.emplacement_id IS NULL", conn);
                lblEmplacementsVides.Text = cmdVides.ExecuteScalar()?.ToString() ?? "0";
                
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

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}