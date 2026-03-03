using System;
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
        private TextBox tbSearch;
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

            // ====== Barre de filtrage simple ======
            var panelToolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.White,
                Padding = new Padding(16, 8, 16, 8)
            };

            var lblFrom = new Label
            {
                Text = "Du",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(16, 16)
            };
            dtpFrom = new DateTimePicker
            {
                Width = 120,
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Value = DateTime.Now.AddDays(-30),
                Location = new Point(42, 12)
            };
            var lblTo = new Label
            {
                Text = "au",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(172, 16)
            };
            dtpTo = new DateTimePicker
            {
                Width = 120,
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                Value = DateTime.Now,
                Location = new Point(196, 12)
            };

            tbSearch = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "🔍 Rechercher...",
                Location = new Point(340, 12)
            };

            btnFilter = new Button
            {
                Text = "Filtrer",
                Width = 75,
                Height = 28,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;

            var btnReset = new Button
            {
                Text = "Reset",
                Width = 65,
                Height = 28,
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnReset.FlatAppearance.BorderColor = PrimaryColor;
            btnReset.Click += (s, e) =>
            {
                dtpFrom.Value = DateTime.Now.AddDays(-30);
                dtpTo.Value = DateTime.Now;
                tbSearch.Text = "";
                RefreshGrid();
            };

            // Positionnement responsive via Resize
            panelToolbar.Resize += (s, e) =>
            {
                int w = panelToolbar.ClientSize.Width;
                int right = w - 16;
                int dateEndX = 326; // fin de la zone dates

                bool twoRows = (w < 620); // seuil pour passer en 2 lignes

                if (twoRows)
                {
                    // Ligne 1 : dates + boutons
                    btnReset.Location = new Point(right - btnReset.Width, 11);
                    btnFilter.Location = new Point(btnReset.Left - btnFilter.Width - 6, 11);

                    // Ligne 2 : recherche pleine largeur
                    panelToolbar.Height = 90;
                    tbSearch.Visible = true;
                    tbSearch.Location = new Point(16, 50);
                    tbSearch.Width = w - 32;
                }
                else
                {
                    // Tout sur 1 ligne
                    panelToolbar.Height = 50;
                    btnReset.Location = new Point(right - btnReset.Width, 11);
                    btnFilter.Location = new Point(btnReset.Left - btnFilter.Width - 6, 11);

                    int searchLeft = dateEndX + 14;
                    int searchRight = btnFilter.Left - 12;
                    tbSearch.Visible = true;
                    tbSearch.Location = new Point(searchLeft, 12);
                    tbSearch.Width = Math.Max(searchRight - searchLeft, 80);
                }
            };

            panelToolbar.Controls.AddRange(new Control[] { lblFrom, dtpFrom, lblTo, dtpTo, tbSearch, btnFilter, btnReset });

            // Grid
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            StyleGrid(grid);

            Controls.Add(grid);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);

            btnFilter.Click += BtnFilter_Click;
            tbSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { BtnFilter_Click(s, e); e.SuppressKeyPress = true; } };
            Load += HistoriqueView_Load;
        }

        private void HistoriqueView_Load(object? sender, EventArgs e)
        {
            // S'assurer que la table existe
            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();
                using var cmd = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Historique (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        action VARCHAR(100) NOT NULL,
                        description TEXT,
                        date DATETIME DEFAULT CURRENT_TIMESTAMP,
                        utilisateur_id INT,
                        FOREIGN KEY (utilisateur_id) REFERENCES Utilisateur(id) ON DELETE SET NULL
                    )", conn);
                cmd.ExecuteNonQuery();
            }
            catch { }

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            try
            {
                using var conn = _db.GetConnection();
                string query =
                    "SELECT h.id, h.action AS Action, h.description AS Description, " +
                    "h.date AS Date, COALESCE(u.login, '—') AS Utilisateur " +
                    "FROM Historique h " +
                    "LEFT JOIN Utilisateur u ON u.id = h.utilisateur_id " +
                    "WHERE h.date BETWEEN @from AND @to";

                string search = tbSearch.Text.Trim();
                if (!string.IsNullOrEmpty(search))
                    query += " AND (h.action LIKE @search OR h.description LIKE @search OR u.login LIKE @search)";

                query += " ORDER BY h.date DESC, h.id DESC";

                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@from", dtpFrom.Value.Date);
                cmd.Parameters.AddWithValue("@to", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));
                if (!string.IsNullOrEmpty(search))
                    cmd.Parameters.AddWithValue("@search", $"%{search}%");

                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                grid.DataSource = table;
                if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false;

                // Formater la colonne Date
                if (grid.Columns.Contains("Date"))
                    grid.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur chargement historique: {ex.Message}");
            }
        }

        private void BtnFilter_Click(object? sender, EventArgs e) => RefreshGrid();

        private static Button CreateButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Width = 100,
                Height = 34,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(30, 60, 114),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 2, 0, 0)
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