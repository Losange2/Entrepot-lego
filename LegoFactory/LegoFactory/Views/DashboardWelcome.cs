using System.Windows.Forms;
using System.Drawing;

namespace LegoFactory
{
    public class DashboardWelcome : UserControl
    {
        public DashboardWelcome()
        {
            BackColor = Color.FromArgb(245, 247, 251);
            Dock = DockStyle.Fill;

            var panelCard = new Panel
            {
                BackColor = Color.White,
                Size = new Size(500, 220),
                Anchor = AnchorStyles.None
            };
            panelCard.Paint += (s, e) =>
            {
                using var path = RoundedRect(panelCard.ClientRectangle, 14);
                panelCard.Region = new Region(path);
            };

            var iconLabel = new Label
            {
                Text = "[LF]",
                Font = new Font("Segoe UI Emoji", 36F),
                AutoSize = true,
                Location = new Point(30, 30),
                BackColor = Color.White
            };

            var title = new Label
            {
                Text = "Bienvenue sur LegoFactory",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 60, 114),
                AutoSize = true,
                Location = new Point(120, 40),
                BackColor = Color.White
            };

            var sub = new Label
            {
                Text = "Sélectionnez une action dans le menu à gauche\npour commencer à travailler.",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(100, 110, 130),
                AutoSize = true,
                Location = new Point(120, 90),
                BackColor = Color.White
            };

            var user = CurrentUser.Instance;
            var userLabel = new Label
            {
                Text = user != null ? $"Connecté en tant que {user.Login} ({user.Role})" : "",
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                ForeColor = Color.FromArgb(150, 160, 175),
                AutoSize = true,
                Location = new Point(120, 160),
                BackColor = Color.White
            };

            panelCard.Controls.Add(iconLabel);
            panelCard.Controls.Add(title);
            panelCard.Controls.Add(sub);
            panelCard.Controls.Add(userLabel);
            Controls.Add(panelCard);

            // Centrer la carte
            Resize += (s, e) =>
            {
                panelCard.Location = new Point(
                    (Width - panelCard.Width) / 2,
                    (Height - panelCard.Height) / 2
                );
            };
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}