using System.Windows.Forms;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class ImportExportView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();

        public ImportExportView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = "📁  Import / Export",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            panelHeader.Controls.Add(title);

            // Contenu avec 2 cartes
            var panelCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(0, 10, 0, 0)
            };

            // Card Import
            var cardImport = CreateCard("📥  Importer des sets depuis CSV",
                "Format CSV : Reference;nom;AgeCible;NombresPieces;quantiter",
                "Importer CSV", BtnImport_Click);

            // Card Export
            var cardExport = CreateCard("📤  Exporter les positions actuelles",
                "Export CSV : tous les sets avec emplacements et quantités",
                "Exporter CSV", BtnExport_Click);

            panelCards.Controls.Add(cardImport);
            panelCards.Controls.Add(cardExport);

            Controls.Add(panelCards);
            Controls.Add(panelHeader);
        }

        private Panel CreateCard(string titleText, string descText, string btnText, System.EventHandler onClick)
        {
            var card = new Panel
            {
                Width = 500,
                Height = 160,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 16),
                Padding = new Padding(24)
            };
            card.Paint += (s, e) =>
            {
                using var path = RoundedRect(card.ClientRectangle, 12);
                card.Region = new Region(path);
            };

            var lbl = new Label
            {
                Text = titleText,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(24, 20)
            };
            var desc = new Label
            {
                Text = descText,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(26, 55)
            };
            var btn = new Button
            {
                Text = btnText,
                Location = new Point(24, 95),
                Width = 180,
                Height = 38,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;

            card.Controls.Add(lbl);
            card.Controls.Add(desc);
            card.Controls.Add(btn);
            return card;
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

                MessageBox.Show($"Import terminé.\nImportés : {imported}\nErreurs : {errors}");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur import : {ex.Message}");
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

                MessageBox.Show($"Export terminé : {count} lignes exportées.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur export : {ex.Message}");
            }
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
