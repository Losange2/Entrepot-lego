using System;
using System.Drawing;
using System.Windows.Forms;

namespace LegoFactory
{
    public class EditRoleForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);

        private ComboBox cbRole;
        private Button btnOk;
        private Button btnCancel;
        public string SelectedRole => cbRole.SelectedItem?.ToString() ?? "Employe";

        public EditRoleForm(string login, string currentRole)
        {
            Text = $" Modifier rôle - {login}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 400;
            Height = 200;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);
            Font = new Font("Segoe UI", 10F);

            var lbl = new Label { Text = "Rôle :", Location = new Point(24, 24), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            cbRole = new ComboBox { Location = new Point(160, 22), Width = 190, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 10F) };
            cbRole.Items.AddRange(new object[] { "Employe", "Responsable", "Admin" });
            cbRole.SelectedItem = currentRole;

            btnOk = new Button
            {
                Text = "  Enregistrer",
                Location = new Point(160, 80),
                Width = 120,
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
                Location = new Point(290, 80),
                Width = 80,
                Height = 36,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderColor = PrimaryColor;

            btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lbl, cbRole, btnOk, btnCancel });
        }
    }
}
