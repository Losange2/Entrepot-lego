using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class HistoriqueView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView grid;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnFilter;

        public HistoriqueView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = "📋  Historique des actions",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            panelHeader.Controls.Add(title);

            // Toolbar filtres
            var panelToolbar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = BgColor, Padding = new Padding(0, 6, 0, 6) };
            var lblFrom = new Label { Text = "Du :", Font = new Font("Segoe UI", 10F), ForeColor = Color.FromArgb(80, 90, 110), AutoSize = true, Location = new Point(0, 8) };
            dtpFrom = new DateTimePicker { Location = new Point(40, 5), Width = 150, Font = new Font("Segoe UI", 9F), Value = System.DateTime.Now.AddDays(-30) };
            var lblTo = new Label { Text = "Au :", Font = new Font("Segoe UI", 10F), ForeColor = Color.FromArgb(80, 90, 110), AutoSize = true, Location = new Point(210, 8) };
            dtpTo = new DateTimePicker { Location = new Point(245, 5), Width = 150, Font = new Font("Segoe UI", 9F), Value = System.DateTime.Now };
            btnFilter = StyleButton("Filtrer", 415, 3);
            panelToolbar.Controls.AddRange(new Control[] { lblFrom, dtpFrom, lblTo, dtpTo, btnFilter });

            // Grid
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            StyleGrid(grid);

            Controls.Add(grid);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);

            btnFilter.Click += BtnFilter_Click;
            Load += HistoriqueView_Load;
        }

        private void HistoriqueView_Load(object? sender, System.EventArgs e) => RefreshGrid();

        private void RefreshGrid()
        {
            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT m.id, m.type AS Type, m.date AS Date, m.quantite AS Quantité, " +
                    "u.login AS Utilisateur, ls.Reference AS 'Réf Set', ls.nom AS 'Nom Set' " +
                    "FROM Mouvement m " +
                    "JOIN Utilisateur u ON u.id = m.utilisateur_id " +
                    "JOIN LegoSet ls ON ls.id = m.legoset_id " +
                    "WHERE m.date BETWEEN @from AND @to " +
                    "ORDER BY m.date DESC, m.id DESC", conn);
                cmd.Parameters.AddWithValue("@from", dtpFrom.Value.Date);
                cmd.Parameters.AddWithValue("@to", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                grid.DataSource = table;
                if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement historique: {ex.Message}");
            }
        }

        private void BtnFilter_Click(object? sender, System.EventArgs e) => RefreshGrid();

        private static Button StyleButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Width = 110,
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