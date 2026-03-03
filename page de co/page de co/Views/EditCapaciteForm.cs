using System.Drawing;
using System.Windows.Forms;

namespace page_de_co
{
    public class EditCapaciteForm : Form
    {
        private NumericUpDown nudCap;
        private Button btnOk;
        private Button btnCancel;
        public int CapaciteMax => (int)nudCap.Value;

        public EditCapaciteForm(string code, int capacite)
        {
            Text = $"Modifier capacité - {code}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 360;
            Height = 180;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lbl = new Label { Text = "Capacité max", Location = new Point(20, 20), AutoSize = true };
            nudCap = new NumericUpDown { Location = new Point(140, 18), Width = 120, Minimum = 1, Maximum = 100000, Value = capacite > 0 ? capacite : 100 };

            btnOk = new Button { Text = "Enregistrer", Location = new Point(140, 70), Width = 90 };
            btnCancel = new Button { Text = "Annuler", Location = new Point(240, 70), Width = 90 };

            btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lbl, nudCap, btnOk, btnCancel });
        }
    }
}