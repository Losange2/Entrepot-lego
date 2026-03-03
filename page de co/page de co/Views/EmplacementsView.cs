using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class EmplacementsView : UserControl
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView grid;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public EmplacementsView()
        {
            var title = new Label { Text = "Gestion des emplacements", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            btnAdd = new Button { Text = "Ajouter", Location = new Point(20, 60), Width = 100 };
            btnEdit = new Button { Text = "Modifier", Location = new Point(130, 60), Width = 100 };
            btnDelete = new Button { Text = "Supprimer", Location = new Point(240, 60), Width = 100 };
            grid = new DataGridView { Location = new Point(20, 100), Width = 1000, Height = 560, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Controls.Add(title);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(grid);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            Load += EmplacementsView_Load;
        }

        private void EmplacementsView_Load(object? sender, System.EventArgs e)
        {
            RefreshGrid();
        }

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

        // Génération de code: Ét. lettre, étage en centaines, rangée en unités (A103)
        private string GenerateCode(char etagere, int etage, int rangee)
        {
            int etageCode = etage * 100;
            int codeNum = etageCode + rangee;
            return $"{char.ToUpper(etagere)}{codeNum}";
        }

        private void BtnEdit_Click(object? sender, System.EventArgs e)
        {
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez un emplacement à modifier.");
                return;
            }
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
            if (grid.CurrentRow == null)
            {
                MessageBox.Show("Sélectionnez un emplacement à supprimer.");
                return;
            }
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
    }
}