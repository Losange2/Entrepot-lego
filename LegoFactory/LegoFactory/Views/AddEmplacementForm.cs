using System;
using System.Drawing;
using System.Windows.Forms;

namespace LegoFactory
{
    public class AddEmplacementForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);

        private TextBox tbEtagere;
        private NumericUpDown nudEtage;
        private NumericUpDown nudRangee;
        private NumericUpDown nudCapacite;
        private Button btnOk;
        private Button btnCancel;

        public char Etagere => string.IsNullOrWhiteSpace(tbEtagere.Text) ? 'A' : tbEtagere.Text.Trim()[0];
        public int Etage => (int)nudEtage.Value;
        public int Rangee => (int)nudRangee.Value;
        public int CapaciteMax => (int)nudCapacite.Value;

        public AddEmplacementForm()
        {
            Text = "> Ajouter un emplacement";
            StartPosition = FormStartPosition.CenterParent;
            Width = 440;
            Height = 320;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);
            Font = new Font("Segoe UI", 10F);

            var lblEtagere = new Label { Text = "Étagère (lettre)", Location = new Point(24, 24), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            tbEtagere = new TextBox { Location = new Point(200, 22), Width = 190, PlaceholderText = "Ex: A", Font = new Font("Segoe UI", 10F) };

            var lblEtage = new Label { Text = "Étage (1..n)", Location = new Point(24, 64), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            nudEtage = new NumericUpDown { Location = new Point(200, 62), Width = 100, Minimum = 1, Maximum = 50, Value = 1, Font = new Font("Segoe UI", 10F) };

            var lblRangee = new Label { Text = "Rangée (1..99)", Location = new Point(24, 104), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            nudRangee = new NumericUpDown { Location = new Point(200, 102), Width = 100, Minimum = 1, Maximum = 99, Value = 1, Font = new Font("Segoe UI", 10F) };

            var lblCap = new Label { Text = "Capacité max", Location = new Point(24, 144), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            nudCapacite = new NumericUpDown { Location = new Point(200, 142), Width = 120, Minimum = 1, Maximum = 100000, Value = 100, Font = new Font("Segoe UI", 10F) };

            btnOk = new Button
            {
                Text = "  Ajouter",
                Location = new Point(200, 200),
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
                Location = new Point(310, 200),
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

            Controls.AddRange(new Control[] { lblEtagere, tbEtagere, lblEtage, nudEtage, lblRangee, nudRangee, lblCap, nudCapacite, btnOk, btnCancel });
        }
    }
}