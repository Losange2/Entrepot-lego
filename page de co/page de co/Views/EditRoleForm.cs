using System;
using System.Drawing;
using System.Windows.Forms;

namespace page_de_co
{
    public class EditRoleForm : Form
    {
        private ComboBox cbRole;
        private Button btnOk;
        private Button btnCancel;
        public string SelectedRole => cbRole.SelectedItem?.ToString() ?? "Employe";

        public EditRoleForm(string login, string currentRole)
        {
            Text = $"Modifier rôle - {login}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 360;
            Height = 180;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lbl = new Label { Text = "Rôle:", Location = new Point(20, 20), AutoSize = true };
            cbRole = new ComboBox { Location = new Point(140, 18), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cbRole.Items.AddRange(new object[] { "Employe", "Responsable", "Admin" });
            cbRole.SelectedItem = currentRole;

            btnOk = new Button { Text = "Enregistrer", Location = new Point(140, 70), Width = 90 };
            btnCancel = new Button { Text = "Annuler", Location = new Point(240, 70), Width = 90 };

            btnOk.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lbl, cbRole, btnOk, btnCancel });
        }
    }
}
