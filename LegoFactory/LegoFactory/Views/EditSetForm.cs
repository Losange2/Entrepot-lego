using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class EditSetForm : Form
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

        public EditSetForm(string reference, string nom, int ageCible, int nombresPieces, int quantite, int currentEmplacementId)
        {
            Text = "Modifier un set";
            Size = new Size(420, 430);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);

            int y = 20;
            int lblX = 20, fieldX = 160, fieldW = 220;

            AddLabel("Reference :", lblX, y);
            tbReference = new TextBox { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Text = reference };
            Controls.Add(tbReference);
            y += 42;

            AddLabel("Nom :", lblX, y);
            tbNom = new TextBox { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Text = nom };
            Controls.Add(tbNom);
            y += 42;

            AddLabel("Age cible :", lblX, y);
            nudAge = new NumericUpDown { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Minimum = 1, Maximum = 99, Value = Math.Max(1, ageCible) };
            Controls.Add(nudAge);
            y += 42;

            AddLabel("Nombre de pieces :", lblX, y);
            nudPieces = new NumericUpDown { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Minimum = 1, Maximum = 99999, Value = Math.Max(1, nombresPieces) };
            Controls.Add(nudPieces);
            y += 42;

            AddLabel("Quantite :", lblX, y);
            nudQuantite = new NumericUpDown { Location = new Point(fieldX, y - 2), Width = fieldW, Font = new Font("Segoe UI", 10F), Minimum = 0, Maximum = 99999, Value = Math.Max(0, quantite) };
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
            LoadEmplacements(currentEmplacementId);
            y += 52;

            var btnOk = new Button
            {
                Text = "Enregistrer",
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

        private void LoadEmplacements(int currentEmplacementId)
        {
            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();
                using var cmd = new MySqlCommand("SELECT id, code FROM Emplacement ORDER BY code", conn);
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

                // Sélectionner l'emplacement actuel
                if (currentEmplacementId > 0)
                    cbEmplacement.SelectedValue = currentEmplacementId;
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
                    MessageBox.Show("La reference est obligatoire.", "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Veuillez selectionner un emplacement.", "Champ manquant", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }
            base.OnFormClosing(e);
        }
    }
}
