using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class AddEmplacementForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private NumericUpDown nudEtage;
        private NumericUpDown nudRangee;
        private NumericUpDown nudCapacite;
        private ComboBox cbZone;
        private Label lblPreview;
        private Label lblLetterValue;
        private Button btnOk;
        private Button btnCancel;

        public string Etagere
        {
            get
            {
                if (cbZone.SelectedItem is ZoneItem z && !string.IsNullOrEmpty(z.Nom))
                {
                    // Si le nom contient un espace (ex: "Zone AB"), prendre la dernière partie
                    string nom = z.Nom.Trim();
                    string[] parts = nom.Split(' ');
                    string lastPart = parts[parts.Length - 1];
                    if (lastPart.Length > 0 && char.IsLetter(lastPart[0]))
                        return lastPart.ToUpper();
                    // Sinon prendre le nom entier en majuscules
                    return nom.ToUpper();
                }
                return "A";
            }
        }
        public int Etage => (int)nudEtage.Value;
        public int Rangee => (int)nudRangee.Value;
        public int CapaciteMax => (int)nudCapacite.Value;
        public int SelectedZoneId => cbZone.SelectedValue is int id ? id : -1;

        public AddEmplacementForm()
        {
            Text = "Ajouter un emplacement";
            StartPosition = FormStartPosition.CenterParent;
            Width = 460;
            Height = 530;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = BgColor;
            Font = new Font("Segoe UI", 10F);

            // Supprimer les doublons de zone au lancement
            CleanDuplicateZones();

            int y = 16;

            // ====== Encadré explication code ======
            var infoPanel = new Panel
            {
                Location = new Point(20, y),
                Size = new Size(400, 130),
                BackColor = Color.FromArgb(235, 242, 255)
            };
            infoPanel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var pen = new Pen(Color.FromArgb(180, 200, 230), 1);
                using var path = RoundedRect(new Rectangle(0, 0, infoPanel.Width - 1, infoPanel.Height - 1), 8);
                infoPanel.Region = new Region(path);
                e.Graphics.DrawPath(pen, path);
            };

            var lblInfoTitle = new Label
            {
                Text = "Generation automatique du code d'emplacement :",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(10, 8),
                BackColor = Color.Transparent
            };

            var lblInfoBody = new Label
            {
                Text = "- L'etagere est representee par une lettre (A, B, C, ...)\n" +
                       "- L'etage est represente en centaines (Etage 1 = 100, Etage 2 = 200, etc.)\n" +
                       "- La rangee est ajoutee en unite (ex. rangee 3 = ...3)",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(60, 80, 120),
                AutoSize = true,
                Location = new Point(10, 30),
                BackColor = Color.Transparent
            };

            var lblInfoExample = new Label
            {
                Text = "-> Exemple : A103 = Etagere A, Etage 1, Rangee 3",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.FromArgb(40, 100, 60),
                AutoSize = true,
                Location = new Point(10, 100),
                BackColor = Color.Transparent
            };

            infoPanel.Controls.Add(lblInfoTitle);
            infoPanel.Controls.Add(lblInfoBody);
            infoPanel.Controls.Add(lblInfoExample);
            Controls.Add(infoPanel);
            y += 145;

            // ====== Champs du formulaire ======
            var lblZone = new Label { Text = "Zone", Location = new Point(24, y), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            cbZone = new ComboBox { Location = new Point(200, y - 2), Width = 210, Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            LoadZones();
            Controls.Add(lblZone);
            Controls.Add(cbZone);
            y += 38;

            var lblEtagere = new Label { Text = "Lettre (auto)", Location = new Point(24, y), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            lblLetterValue = new Label
            {
                Text = "A",
                Location = new Point(200, y),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 100, 60),
                AutoSize = true,
                BackColor = BgColor
            };
            Controls.Add(lblEtagere);
            Controls.Add(lblLetterValue);
            cbZone.SelectedIndexChanged += (s, e) => UpdateLetterAndPreview();
            y += 38;

            var lblEtage = new Label { Text = "Etage (1..n)", Location = new Point(24, y), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            nudEtage = new NumericUpDown { Location = new Point(200, y - 2), Width = 100, Minimum = 1, Maximum = 50, Value = 1, Font = new Font("Segoe UI", 10F) };
            Controls.Add(lblEtage);
            Controls.Add(nudEtage);
            y += 38;

            var lblRangee = new Label { Text = "Rangee (1..99)", Location = new Point(24, y), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            nudRangee = new NumericUpDown { Location = new Point(200, y - 2), Width = 100, Minimum = 1, Maximum = 99, Value = 1, Font = new Font("Segoe UI", 10F) };
            Controls.Add(lblRangee);
            Controls.Add(nudRangee);
            y += 38;

            var lblCap = new Label { Text = "Capacite max", Location = new Point(24, y), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            nudCapacite = new NumericUpDown { Location = new Point(200, y - 2), Width = 120, Minimum = 1, Maximum = 100000, Value = 100, Font = new Font("Segoe UI", 10F) };
            Controls.Add(lblCap);
            Controls.Add(nudCapacite);
            y += 40;

            // ====== Aperçu du code généré ======
            lblPreview = new Label
            {
                Text = "Code genere : A101",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 100, 60),
                AutoSize = true,
                Location = new Point(24, y),
                BackColor = BgColor
            };
            Controls.Add(lblPreview);
            UpdatePreview();
            y += 38;

            // Mettre à jour l'aperçu en temps réel
            nudEtage.ValueChanged += (s, e) => UpdatePreview();
            nudRangee.ValueChanged += (s, e) => UpdatePreview();
            UpdateLetterAndPreview();

            // ====== Boutons ======
            btnOk = new Button
            {
                Text = "Ajouter",
                Location = new Point(200, y),
                Width = 100,
                Height = 36,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            btnCancel = new Button
            {
                Text = "Annuler",
                Location = new Point(310, y),
                Width = 100,
                Height = 36,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = PrimaryColor;

            btnOk.Click += (s, e) =>
            {
                if (SelectedZoneId < 0)
                {
                    MessageBox.Show("Veuillez selectionner une zone.",
                        "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
                Close();
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private void UpdateLetterAndPreview()
        {
            lblLetterValue.Text = Etagere;
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            int code = (int)nudEtage.Value * 100 + (int)nudRangee.Value;
            lblPreview.Text = $"Code genere : {Etagere}{code}";
        }

        private void CleanDuplicateZones()
        {
            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();

                // Réaffecter les emplacements des doublons vers l'original (id le plus petit)
                using (var cmd = new MySqlCommand(
                    "UPDATE Emplacement e " +
                    "JOIN Zone z ON e.zone_id = z.id " +
                    "JOIN (SELECT MIN(id) AS min_id, nom FROM Zone GROUP BY nom) keep ON z.nom = keep.nom " +
                    "SET e.zone_id = keep.min_id " +
                    "WHERE z.id != keep.min_id", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Supprimer les doublons (garder l'id minimum par nom)
                using (var cmd = new MySqlCommand(
                    "DELETE z FROM Zone z " +
                    "JOIN (SELECT MIN(id) AS min_id, nom FROM Zone GROUP BY nom) keep " +
                    "ON z.nom = keep.nom " +
                    "WHERE z.id != keep.min_id", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* Silencieux */ }
        }

        private void LoadZones()
        {
            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();
                using var cmd = new MySqlCommand("SELECT id, nom FROM Zone ORDER BY nom", conn);
                using var reader = cmd.ExecuteReader();

                var items = new System.Collections.Generic.List<ZoneItem>();
                while (reader.Read())
                {
                    items.Add(new ZoneItem
                    {
                        Id = reader.GetInt32("id"),
                        Nom = reader.GetString("nom")
                    });
                }
                cbZone.DataSource = items;
                cbZone.DisplayMember = "Nom";
                cbZone.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible de charger les zones : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private class ZoneItem
        {
            public int Id { get; set; }
            public string Nom { get; set; } = "";
        }
    }
}