using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class HistoriqueView : UserControl
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private DataGridView grid;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnFilter;

        public HistoriqueView()
        {
            var title = new Label { Text = "Historique des actions", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            
            var lblFrom = new Label { Text = "Du:", Location = new Point(20, 60), AutoSize = true };
            dtpFrom = new DateTimePicker { Location = new Point(70, 58), Width = 150, Value = System.DateTime.Now.AddDays(-30) };
            
            var lblTo = new Label { Text = "Au:", Location = new Point(240, 60), AutoSize = true };
            dtpTo = new DateTimePicker { Location = new Point(280, 58), Width = 150, Value = System.DateTime.Now };
            
            btnFilter = new Button { Text = "Filtrer", Location = new Point(450, 56), Width = 100 };
            
            grid = new DataGridView { Location = new Point(20, 100), Width = 1050, Height = 560, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Controls.Add(title);
            Controls.Add(lblFrom);
            Controls.Add(dtpFrom);
            Controls.Add(lblTo);
            Controls.Add(dtpTo);
            Controls.Add(btnFilter);
            Controls.Add(grid);

            btnFilter.Click += BtnFilter_Click;
            Load += HistoriqueView_Load;
        }

        private void HistoriqueView_Load(object? sender, System.EventArgs e)
        {
            RefreshGrid();
        }

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

        private void BtnFilter_Click(object? sender, System.EventArgs e)
        {
            RefreshGrid();
        }
    }
}