using System.Windows.Forms;
using System.Drawing;

namespace LegoFactory
{
    public class SyncView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        public SyncView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = ">  Synchronisation",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            panelHeader.Controls.Add(title);

            // Card info
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(32)
            };
            card.Paint += (s, e) =>
            {
                using var path = RoundedRect(card.ClientRectangle, 12);
                card.Region = new Region(path);
            };

            var lblInfo = new Label
            {
                Text = "Cette fonctionnalité nécessite la configuration d'un outil de stock externe.\n\n" +
                       "Selon le cahier des charges (section 3.4), la synchronisation peut se faire via :\n" +
                       "• API REST (si l'outil externe expose une API)\n" +
                       "• Import/Export planifié (CSV/Excel)\n" +
                       "• Liaison par clé unique (code set)\n\n" +
                       "Configuration requise :\n" +
                       "1. URL de l'API ou chemin du fichier d'échange\n" +
                       "2. Identifiants d'accès (si API)\n" +
                       "3. Mapping des champs (Reference ↔ code unique)\n" +
                       "4. Fréquence de synchronisation",
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 250,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            var panelBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                AutoSize = false,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 8, 0, 0),
                BackColor = Color.White
            };

            var btnConfig = new Button
            {
                Text = "  Configurer",
                Width = 180,
                Height = 38,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(0, 0, 12, 0)
            };
            btnConfig.FlatAppearance.BorderColor = PrimaryColor;

            var btnSync = new Button
            {
                Text = "  Lancer la sync",
                Width = 180,
                Height = 38,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.White,
                ForeColor = PrimaryColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnSync.FlatAppearance.BorderColor = PrimaryColor;

            panelBtns.Controls.Add(btnConfig);
            panelBtns.Controls.Add(btnSync);

            var lblStatus = new Label
            {
                Text = "/!\\ Aucun outil de stock configure. Utilisez Import/Export pour l'instant.",
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 40,
                ForeColor = Color.DarkOrange,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                Padding = new Padding(0, 14, 0, 0)
            };

            // Ajout en ordre inverse (Dock Top)
            card.Controls.Add(lblStatus);
            card.Controls.Add(panelBtns);
            card.Controls.Add(lblInfo);

            Controls.Add(card);
            Controls.Add(panelHeader);
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
