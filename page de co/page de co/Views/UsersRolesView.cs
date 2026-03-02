using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class UsersRolesView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView grid;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public UsersRolesView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = "👥  Utilisateurs et rôles",
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
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = BgColor,
                Padding = new Padding(0, 6, 0, 6)
            };

            btnEdit = CreateButton("✏️ Modifier rôle");
            btnEdit.Width = 160;

            panelToolbar.Controls.Add(btnEdit);

            // Grid
            grid = new DataGridView
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
            StyleGrid(grid);

            Controls.Add(grid);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);

            btnEdit.Click += BtnEdit_Click;
            Load += UsersRolesView_Load;
        }

        private static Button CreateButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Width = 130,
                Height = 36,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 0)
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

        private void UsersRolesView_Load(object? sender, System.EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand("SELECT id, nom, login, role FROM Utilisateur ORDER BY role, login", conn);
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                grid.DataSource = table;
                if (grid.Columns.Contains("id")) grid.Columns["id"].Visible = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement utilisateurs: {ex.Message}");
            }
        }

        private void BtnEdit_Click(object? sender, System.EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez un utilisateur.");
                return;
            }
            var id = grid.CurrentRow.Cells["id"].Value;
            var login = grid.CurrentRow.Cells["login"].Value?.ToString();
            var currentRole = grid.CurrentRow.Cells["role"].Value?.ToString() ?? "Employe";

            using var dlg = new EditRoleForm(login ?? "", currentRole);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    using var cmd = new MySqlCommand("UPDATE Utilisateur SET role = @role WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@role", dlg.SelectedRole);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    RefreshGrid();
                    MessageBox.Show("Rôle mis à jour. Reconnexion nécessaire pour appliquer.");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur modification: {ex.Message}");
                }
            }
        }
    }
}