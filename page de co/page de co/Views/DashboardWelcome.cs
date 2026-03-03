using System.Windows.Forms;
using System.Drawing;

namespace page_de_co
{
    public class DashboardWelcome : UserControl
    {
        public DashboardWelcome()
        {
            var title = new Label
            {
                Text = "Bienvenue sur LegoFactory",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(40, 30)
            };
            var sub = new Label
            {
                Text = "Choisissez une action dans le menu à gauche.",
                Font = new Font("Segoe UI", 12F),
                AutoSize = true,
                Location = new Point(42, 80)
            };
            Controls.Add(title);
            Controls.Add(sub);
        }
    }
}