using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
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

            btnAdd = CreateButton("➕ Ajouter");
            btnEdit = CreateButton("✏️ Modifier");
            btnEdit.Width = 140;
            btnDelete = CreateButton("🗑️ Supprimer");
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
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            StyleGrid(grid);

            Controls.Add(grid);
            Controls.Add(panelToolbar);
            Controls.Add(panelHeader);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
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
                using var cmd = new MySqlCommand("SELECT id, nom AS Nom, login AS Login, role AS Rôle FROM Utilisateur ORDER BY role, login", conn);
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

        private void BtnAdd_Click(object? sender, System.EventArgs e)
        {
            using var dlg = new AddUserForm();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    using var cmd = new MySqlCommand(
                        "INSERT INTO Utilisateur (nom, login, motDePasse, role) VALUES (@nom, @login, @mdp, @role)", conn);
                    cmd.Parameters.AddWithValue("@nom", dlg.Nom);
                    cmd.Parameters.AddWithValue("@login", dlg.Login);
                    cmd.Parameters.AddWithValue("@mdp", Security.PasswordHasher.HashPassword(dlg.Password));
                    cmd.Parameters.AddWithValue("@role", dlg.SelectedRole);
                    cmd.ExecuteNonQuery();
                    HistoriqueHelper.Log("Ajout utilisateur", $"Utilisateur '{dlg.Login}' créé (rôle: {dlg.SelectedRole})");
                    RefreshGrid();
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    MessageBox.Show("Ce login existe déjà. Veuillez en choisir un autre.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur création: {ex.Message}");
                }
            }
        }

        private void BtnEdit_Click(object? sender, System.EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez un utilisateur à modifier.");
                return;
            }
            var id = grid.CurrentRow.Cells["id"].Value;
            var nom = grid.CurrentRow.Cells["Nom"].Value?.ToString() ?? "";
            var login = grid.CurrentRow.Cells["Login"].Value?.ToString() ?? "";
            var currentRole = grid.CurrentRow.Cells["Rôle"].Value?.ToString() ?? "Employe";

            using var dlg = new EditUserForm(nom, login, currentRole);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    string sql;
                    if (!string.IsNullOrWhiteSpace(dlg.Password))
                    {
                        sql = "UPDATE Utilisateur SET nom = @nom, login = @login, motDePasse = @mdp, role = @role WHERE id = @id";
                    }
                    else
                    {
                        sql = "UPDATE Utilisateur SET nom = @nom, login = @login, role = @role WHERE id = @id";
                    }
                    using var cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@nom", dlg.Nom);
                    cmd.Parameters.AddWithValue("@login", dlg.UserLogin);
                    cmd.Parameters.AddWithValue("@role", dlg.SelectedRole);
                    cmd.Parameters.AddWithValue("@id", id);
                    if (!string.IsNullOrWhiteSpace(dlg.Password))
                    {
                        cmd.Parameters.AddWithValue("@mdp", Security.PasswordHasher.HashPassword(dlg.Password));
                    }
                    cmd.ExecuteNonQuery();
                    HistoriqueHelper.Log("Modification utilisateur", $"Utilisateur '{dlg.UserLogin}' modifié (rôle: {dlg.SelectedRole})");
                    RefreshGrid();
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    MessageBox.Show("Ce login existe déjà. Veuillez en choisir un autre.", "Doublon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur modification: {ex.Message}");
                }
            }
        }

        private void BtnDelete_Click(object? sender, System.EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez un utilisateur à supprimer.");
                return;
            }
            var id = grid.CurrentRow.Cells["id"].Value;
            var login = grid.CurrentRow.Cells["Login"].Value?.ToString() ?? "";

            // Empêcher la suppression de son propre compte
            var currentUser = CurrentUser.Instance;
            if (currentUser != null && currentUser.Login == login)
            {
                MessageBox.Show("Vous ne pouvez pas supprimer votre propre compte.", "Interdit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Supprimer l'utilisateur '{login}' ?\n\nCette action est irréversible.",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    using var cmd = new MySqlCommand("DELETE FROM Utilisateur WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    HistoriqueHelper.Log("Suppression utilisateur", $"Utilisateur '{login}' supprimé");
                    RefreshGrid();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Erreur suppression: {ex.Message}");
                }
            }
        }
    }
}