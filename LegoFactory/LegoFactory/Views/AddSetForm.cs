using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class AddSetForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);

        private TextBox tbReference;
        private TextBox tbNom;
        private NumericUpDown nudAge;
        private NumericUpDown nudPieces;
        private NumericUpDown nudQuantite;
        private ComboBox cbEmplacement;

        public string Reference => tbReference.Text.Trim();
        public string Nom => tbNom.Text.Trim();
        public int AgeCible => (int)nudAge.Value;
        public int NombresPieces => (int)nudPieces.Value;
        public int Quantite => (int)nudQuantite.Value;
        public int SelectedEmplacementId => cbEmplacement.SelectedValue is int id ? id : -1;

        public AddSetForm()
        {
            Text = "Ajouter un set";
            Size = new Size(420, 430);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);

            int y = 20;
            int lblX = 20, fieldX = 160, fieldW = 220;

            AddLabel("Référence :", lblX, y);
            tbReference = new TextBox { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F) };
            Controls.Add(tbReference);
            y += 42;

            AddLabel("Nom :", lblX, y);
            tbNom = new TextBox { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F) };
            Controls.Add(tbNom);
            y += 42;

            AddLabel("Âge cible :", lblX, y);
            nudAge = new NumericUpDown { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Minimum = 1, Maximum = 99, Value = 6 };
            Controls.Add(nudAge);
            y += 42;

            AddLabel("Nombre de pièces :", lblX, y);
            nudPieces = new NumericUpDown { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Minimum = 1, Maximum = 99999, Value = 100 };
            Controls.Add(nudPieces);
            y += 42;

            AddLabel("Quantité :", lblX, y);
            nudQuantite = new NumericUpDown { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Minimum = 0, Maximum = 99999, Value = 1 };
            Controls.Add(nudQuantite);
            y += 42;

            AddLabel("Emplacement :", lblX, y);
            cbEmplacement = new ComboBox
            {
                Location = new Point(fieldX, y - 2),
                Width = fieldW,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            Controls.Add(cbEmplacement);
            LoadEmplacements();
            y += 52;

            var btnOk = new Button
            {
                Text = "Ajouter",
                DialogResult = DialogResult.OK,
                Location = new Point(fieldX, y),
                Width = 105,
                Height = 36,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderSize = 0;

            var btnCancel = new Button
            {
                Text = "Annuler",
                DialogResult = DialogResult.Cancel,
                Location = new Point(fieldX + 115, y),
                Width = 105,
                Height = 36,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(180, 185, 195),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private void LoadEmplacements()
        {
            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT id, code FROM Emplacement ORDER BY code", conn);
                using var reader = cmd.ExecuteReader();

                var items = new System.Collections.Generic.List<EmplacementItem>();
                while (reader.Read())
                {
                    items.Add(new EmplacementItem
                    {
                        Id = reader.GetInt32("id"),
                        Code = reader.GetString("code")
                    });
                }
                cbEmplacement.DataSource = items;
                cbEmplacement.DisplayMember = "Code";
                cbEmplacement.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible de charger les emplacements : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class EmplacementItem
        {
            public int Id { get; set; }
            public string Code { get; set; } = "";
        }

        private void AddLabel(string text, int x, int y)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = PrimaryColor
            });
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(Reference))
                {
                    MessageBox.Show("La référence est obligatoire.", "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                if (string.IsNullOrWhiteSpace(Nom))
                {
                    MessageBox.Show("Le nom est obligatoire.", "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                if (SelectedEmplacementId < 0)
                {
                    MessageBox.Show("Veuillez sélectionner un emplacement.", "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }
            base.OnFormClosing(e);
        }
    }
}
