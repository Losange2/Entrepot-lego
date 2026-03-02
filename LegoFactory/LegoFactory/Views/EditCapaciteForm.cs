using System.Drawing;
using System.Windows.Forms;

namespace LegoFactory
{
    public class EditCapaciteForm : Form
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);

        private NumericUpDown nudCap;
        private Button btnOk;
        private Button btnCancel;
        public int CapaciteMax => (int)nudCap.Value;

        public EditCapaciteForm(string code, int capacite)
        {
            Text = $" Modifier capacité - {code}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 400;
            Height = 200;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);
            Font = new Font("Segoe UI", 10F);

            var lbl = new Label { Text = "Capacité max", Location = new Point(24, 24), AutoSize = true, ForeColor = PrimaryColor, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            nudCap = new NumericUpDown { Location = new Point(160, 22), Width = 140, Minimum = 1, Maximum = 100000, Value = capacite > 0 ? capacite : 100, Font = new Font("Segoe UI", 10F) };

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

            Controls.AddRange(new Control[] { lbl, nudCap, btnOk, btnCancel });
        }
    }
}