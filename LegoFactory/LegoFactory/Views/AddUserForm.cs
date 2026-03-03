using System;
using System.Drawing;
using System.Windows.Forms;

namespace LegoFactory
{
    public class AddUserForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);

        private TextBox tbNom;
        private TextBox tbLogin;
        private TextBox tbPassword;
        private ComboBox cbRole;
        private Button btnOk;
        private Button btnCancel;

        public string Nom => tbNom.Text.Trim();
        public string Login => tbLogin.Text.Trim();
        public string Password => tbPassword.Text;
        public string SelectedRole => cbRole.SelectedItem?.ToString() ?? "Employe";

        public AddUserForm()
        {
            Text = "➕ Ajouter un utilisateur";
            StartPosition = FormStartPosition.CenterParent;
            Width = 440;
            Height = 340;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);
            Font = new Font("Segoe UI", 10F);

            var lblNom = new Label { Text = "Nom", Location = new Point(24, 24), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            tbNom = new TextBox { Location = new Point(160, 22), Width = 240, Font = new Font("Segoe UI", 10F), PlaceholderText = "Nom complet" };

            var lblLogin = new Label { Text = "Login", Location = new Point(24, 64), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            tbLogin = new TextBox { Location = new Point(160, 62), Width = 240, Font = new Font("Segoe UI", 10F), PlaceholderText = "Identifiant unique" };

            var lblPassword = new Label { Text = "Mot de passe", Location = new Point(24, 104), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            tbPassword = new TextBox { Location = new Point(160, 102), Width = 240, Font = new Font("Segoe UI", 10F), PasswordChar = '*', PlaceholderText = "Mot de passe" };

            var lblRole = new Label { Text = "Rôle", Location = new Point(24, 144), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            cbRole = new ComboBox { Location = new Point(160, 142), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F) };
            cbRole.Items.AddRange(new object[] { "Employe", "Responsable", "Admin" });
            cbRole.SelectedIndex = 0;

            btnOk = new Button
            {
                Text = "✅  Créer",
                Location = new Point(160, 210),
                Width = 120,
                Height = 38,
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
                Location = new Point(290, 210),
                Width = 110,
                Height = 38,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = PrimaryColor;

            btnOk.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(tbNom.Text) || string.IsNullOrWhiteSpace(tbLogin.Text) || string.IsNullOrWhiteSpace(tbPassword.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs.", "Champs manquants", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
                Close();
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblNom, tbNom, lblLogin, tbLogin, lblPassword, tbPassword, lblRole, cbRole, btnOk, btnCancel });
        }
    }
}
