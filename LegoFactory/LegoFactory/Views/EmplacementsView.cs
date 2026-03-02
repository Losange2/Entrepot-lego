using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class EmplacementsView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView grid;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public EmplacementsView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = ">  Gestion des emplacements",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            panelHeader.Controls.Add(title);

            // Toolbar
            var panelToolbar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = BgColor, Padding = new Padding(0, 6, 0, 6) };
            btnAdd = CreateButton("+  Ajouter", 0);
            btnEdit = CreateButton("  Modifier", 130);
            btnDelete = CreateButton("  Supprimer", 260);
            btnDelete.BackColor = Color.FromArgb(180, 50, 50);
            panelToolbar.Controls.AddRange(new Control[] { btnAdd, btnEdit, btnDelete });

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
            Load += EmplacementsView_Load;
        }

        private void EmplacementsView_Load(object? sender, System.EventArgs e) => RefreshGrid();

        private void RefreshGrid()
        {
            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand("SELECT id, code, capaciteMax, DateEntree, DateSorti FROM Emplacement ORDER BY id DESC", conn);
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                grid.DataSource = table;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement emplacements: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object? sender, System.EventArgs e)
        {
            using var dlg = new AddEmplacementForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string code = GenerateCode(dlg.Etagere, dlg.Etage, dlg.Rangee);
                    using var conn = _db.GetConnection();
                    using var cmd = new MySqlCommand("INSERT INTO Emplacement(code, capaciteMax, DateEntree) VALUES(@code, @cap, CURRENT_DATE())", conn);
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@cap", dlg.CapaciteMax);
                    cmd.ExecuteNonQuery();
                    RefreshGrid();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur ajout emplacement: {ex.Message}");
                }
            }
        }

        private string GenerateCode(char etagere, int etage, int rangee)
        {
            int etageCode = etage * 100;
            int codeNum = etageCode + rangee;
            return $"{char.ToUpper(etagere)}{codeNum}";
        }

        private void BtnEdit_Click(object? sender, System.EventArgs e)
        {
            if (grid.CurrentRow == null) { MessageBox.Show("Sélectionnez un emplacement à modifier."); return; }
            var id = grid.CurrentRow.Cells["id"].Value;
            var code = grid.CurrentRow.Cells["code"].Value?.ToString();
            var capStr = grid.CurrentRow.Cells["capaciteMax"].Value?.ToString();
            int cap = 0; int.TryParse(capStr, out cap);

            using var dlg = new EditCapaciteForm(code ?? "", cap);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    using var cmd = new MySqlCommand("UPDATE Emplacement SET capaciteMax = @cap WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@cap", dlg.CapaciteMax);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    RefreshGrid();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur modification: {ex.Message}");
                }
            }
        }

        private void BtnDelete_Click(object? sender, System.EventArgs e)
        {
            if (grid.CurrentRow == null) { MessageBox.Show("Sélectionnez un emplacement à supprimer."); return; }
            var id = grid.CurrentRow.Cells["id"].Value;
            var code = grid.CurrentRow.Cells["code"].Value?.ToString();
            if (MessageBox.Show($"Supprimer l'emplacement {code} ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    using var cmd = new MySqlCommand("DELETE FROM Emplacement WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    RefreshGrid();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur suppression: {ex.Message}");
                }
            }
        }

        private static Button CreateButton(string text, int x)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, 3),
                Width = 120,
                Height = 34,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
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