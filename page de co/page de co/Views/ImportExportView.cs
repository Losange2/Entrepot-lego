using System.Windows.Forms;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class ImportExportView : UserControl
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();

        public ImportExportView()
        {
            var title = new Label { Text = "Import / Export", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            
            var lblImport = new Label { Text = "Importer des sets depuis CSV:", Location = new Point(20, 70), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            var lblFormatImport = new Label { Text = "Format CSV: Reference;nom;AgeCible;NombresPieces;quantiter", Location = new Point(40, 100), AutoSize = true, ForeColor = Color.Gray };
            var btnImport = new Button { Text = "Importer CSV", Location = new Point(40, 130), Width = 180 };
            
            var lblExport = new Label { Text = "Exporter les positions actuelles:", Location = new Point(20, 200), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            var lblFormatExport = new Label { Text = "Export CSV: tous les sets avec emplacements et quantités", Location = new Point(40, 230), AutoSize = true, ForeColor = Color.Gray };
            var btnExport = new Button { Text = "Exporter positions CSV", Location = new Point(40, 260), Width = 200 };

            Controls.Add(title);
            Controls.Add(lblImport);
            Controls.Add(lblFormatImport);
            Controls.Add(btnImport);
            Controls.Add(lblExport);
            Controls.Add(lblFormatExport);
            Controls.Add(btnExport);

            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
        }

        private void BtnImport_Click(object? sender, System.EventArgs e)
        {
            using var dlg = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv", Title = "Sélectionner fichier CSV sets" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                var lines = File.ReadAllLines(dlg.FileName);
                int imported = 0, errors = 0;
                using var conn = _db.GetConnection();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(';');
                    if (parts.Length < 5) { errors++; continue; }

                    try
                    {
                        using var cmd = new MySqlCommand(
                            "INSERT INTO LegoSet (Reference, nom, AgeCible, NombresPieces, quantiter) VALUES (@ref, @nom, @age, @pieces, @qte) " +
                            "ON DUPLICATE KEY UPDATE nom=@nom, AgeCible=@age, NombresPieces=@pieces, quantiter=quantiter+@qte", conn);
                        cmd.Parameters.AddWithValue("@ref", parts[0].Trim());
                        cmd.Parameters.AddWithValue("@nom", parts[1].Trim());
                        cmd.Parameters.AddWithValue("@age", int.Parse(parts[2].Trim()));
                        cmd.Parameters.AddWithValue("@pieces", int.Parse(parts[3].Trim()));
                        cmd.Parameters.AddWithValue("@qte", int.Parse(parts[4].Trim()));
                        cmd.ExecuteNonQuery();
                        imported++;
                    }
                    catch { errors++; }
                }

                MessageBox.Show($"Import terminé.\nImportés: {imported}\nErreurs: {errors}");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur import: {ex.Message}");
            }
        }

        private void BtnExport_Click(object? sender, System.EventArgs e)
        {
            using var dlg = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", Title = "Exporter positions", FileName = $"export_positions_{System.DateTime.Now:yyyyMMdd}.csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT ls.Reference, ls.nom, e.code AS emplacement, s.quantiter, z.nom AS zone " +
                    "FROM stocker s " +
                    "JOIN LegoSet ls ON ls.id = s.legoset_id " +
                    "JOIN Emplacement e ON e.id = s.emplacement_id " +
                    "JOIN Zone z ON z.id = e.zone_id " +
                    "ORDER BY ls.Reference, e.code", conn);

                using var writer = new StreamWriter(dlg.FileName);
                writer.WriteLine("Reference;Nom;Emplacement;Quantite;Zone");

                using var reader = cmd.ExecuteReader();
                int count = 0;
                while (reader.Read())
                {
                    writer.WriteLine($"{reader["Reference"]};{reader["nom"]};{reader["emplacement"]};{reader["quantiter"]};{reader["zone"]}");
                    count++;
                }

                MessageBox.Show($"Export terminé: {count} lignes exportées vers {dlg.FileName}");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur export: {ex.Message}");
            }
        }
    }
}
