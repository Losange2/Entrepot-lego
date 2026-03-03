using System;
using System.Drawing;
using System.Windows.Forms;

namespace page_de_co
{
    public class AddEmplacementForm : Form
    {
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
            Text = "Ajouter un emplacement";
            StartPosition = FormStartPosition.CenterParent;
            Width = 420;
            Height = 280;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lblEtagere = new Label { Text = "Étagère (lettre)", Location = new Point(20, 20), AutoSize = true };
            tbEtagere = new TextBox { Location = new Point(180, 18), Width = 180, PlaceholderText = "Ex: A" };

            var lblEtage = new Label { Text = "Étage (1..n)", Location = new Point(20, 60), AutoSize = true };
            nudEtage = new NumericUpDown { Location = new Point(180, 58), Width = 80, Minimum = 1, Maximum = 50, Value = 1 };

            var lblRangee = new Label { Text = "Rangée (1..99)", Location = new Point(20, 100), AutoSize = true };
            nudRangee = new NumericUpDown { Location = new Point(180, 98), Width = 80, Minimum = 1, Maximum = 99, Value = 1 };

            var lblCap = new Label { Text = "Capacité max", Location = new Point(20, 140), AutoSize = true };
            nudCapacite = new NumericUpDown { Location = new Point(180, 138), Width = 100, Minimum = 1, Maximum = 100000, Value = 100 };

            btnOk = new Button { Text = "Ajouter", Location = new Point(180, 190), Width = 90 };
            btnCancel = new Button { Text = "Annuler", Location = new Point(270, 190), Width = 90 };

            btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblEtagere, tbEtagere, lblEtage, nudEtage, lblRangee, nudRangee, lblCap, nudCapacite, btnOk, btnCancel });
        }
    }
}