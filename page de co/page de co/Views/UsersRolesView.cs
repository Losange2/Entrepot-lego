using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class UsersRolesView : UserControl
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView grid;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public UsersRolesView()
        {
            var title = new Label { Text = "Utilisateurs et rôles", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            btnAdd = new Button { Text = "Ajouter", Location = new Point(20, 60), Width = 100 };
            btnEdit = new Button { Text = "Modifier rôle", Location = new Point(130, 60), Width = 120 };
            btnDelete = new Button { Text = "Supprimer", Location = new Point(260, 60), Width = 100 };
            grid = new DataGridView { Location = new Point(20, 100), Width = 1000, Height = 540, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
            
            Controls.Add(title);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(grid);

            btnEdit.Click += BtnEdit_Click;
            Load += UsersRolesView_Load;
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