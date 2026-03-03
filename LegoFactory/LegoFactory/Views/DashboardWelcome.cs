using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class DashboardWelcome : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);
        private static readonly Color CardBg = Color.White;

        private Label? lblDate;
        private System.Windows.Forms.Timer? clockTimer;

        public DashboardWelcome()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            AutoScroll = true;

            Load += DashboardWelcome_Load;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                clockTimer?.Stop();
                clockTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DashboardWelcome_Load(object? sender, EventArgs e)
        {
            BuildUI();
            // Timer horloge temps réel
            clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            clockTimer.Tick += (s, ev) =>
            {
                if (lblDate != null)
                    lblDate.Text = $"  {DateTime.Now:dddd d MMMM yyyy — HH:mm:ss}";
            };
            clockTimer.Start();
        }

        private void BuildUI()
        {
            Controls.Clear();

            var user = CurrentUser.Instance;
            string userName = user?.Login ?? "Utilisateur";
            string userRole = user?.Role.ToString() ?? "Inconnu";

            // ====== Conteneur scrollable ======
            var mainPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = BgColor,
                Padding = new Padding(32, 24, 32, 24),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // ====== Welcome Header Card ======
            var headerCard = new Panel
            {
                Size = new Size(700, 150),
                BackColor = CardBg,
                Margin = new Padding(0, 0, 0, 20)
            };
            headerCard.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedRect(new Rectangle(0, 0, headerCard.Width - 1, headerCard.Height - 1), 12);
                headerCard.Region = new Region(path);
                using var shadow = new Pen(Color.FromArgb(20, 0, 0, 0), 1);
                e.Graphics.DrawPath(shadow, path);
            };

            var iconWelcome = CreateIconPanel("L", Color.FromArgb(30, 60, 114), 50);
            iconWelcome.Location = new Point(24, 28);
            headerCard.Controls.Add(iconWelcome);

            var lblWelcome = new Label
            {
                Text = $"Bienvenue, {userName} !",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(90, 22),
                BackColor = CardBg
            };

            var lblRole = new Label
            {
                Text = $"Role : {userRole}",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(100, 115, 140),
                AutoSize = true,
                Location = new Point(90, 62),
                BackColor = CardBg
            };

            lblDate = new Label
            {
                Text = $"  {DateTime.Now:dddd d MMMM yyyy — HH:mm:ss}",
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.FromArgb(130, 140, 160),
                AutoSize = true,
                Location = new Point(90, 95),
                BackColor = CardBg
            };

            headerCard.Controls.Add(lblWelcome);
            headerCard.Controls.Add(lblRole);
            headerCard.Controls.Add(lblDate);
            mainPanel.Controls.Add(headerCard);

            // ====== Titre Stats ======
            var lblStats = new Label
            {
                Text = "  Tableau de bord",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 10),
                BackColor = BgColor
            };
            mainPanel.Controls.Add(lblStats);

            // Load stats from DB
            int nbSets = 0, nbEmplacements = 0, nbUtilisateurs = 0, nbActions = 0;
            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();

                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM LegoSet", conn))
                    nbSets = Convert.ToInt32(cmd.ExecuteScalar());
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM Emplacement", conn))
                    nbEmplacements = Convert.ToInt32(cmd.ExecuteScalar());
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM Utilisateur", conn))
                    nbUtilisateurs = Convert.ToInt32(cmd.ExecuteScalar());
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM Historique", conn))
                    nbActions = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch { /* Silencieux si DB indisponible */ }

            // ====== Stats Cards Row ======
            var statsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = BgColor,
                Margin = new Padding(0, 0, 0, 12)
            };

            statsPanel.Controls.Add(CreateStatCard("S", "Sets", nbSets.ToString(), Color.FromArgb(52, 152, 219)));
            statsPanel.Controls.Add(CreateStatCard("E", "Emplacements", nbEmplacements.ToString(), Color.FromArgb(46, 204, 113)));
            statsPanel.Controls.Add(CreateStatCard("U", "Utilisateurs", nbUtilisateurs.ToString(), Color.FromArgb(155, 89, 182)));
            statsPanel.Controls.Add(CreateStatCard("H", "Historique", nbActions.ToString(), Color.FromArgb(230, 126, 34)));

            mainPanel.Controls.Add(statsPanel);

            // ====== Titre Accès rapide ======
            var lblActions = new Label
            {
                Text = "  Acces rapide",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Margin = new Padding(0, 8, 0, 10),
                BackColor = BgColor
            };
            mainPanel.Controls.Add(lblActions);

            // ====== Quick Actions Row ======
            var actionsPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                BackColor = BgColor,
                Margin = new Padding(0, 0, 0, 12)
            };

            var btnGoEntrepot = CreateQuickAction("E", "Consulter l'entrepot", "Voir tous les emplacements\net le stock", Color.FromArgb(52, 152, 219));
            btnGoEntrepot.Click += (s, e) => FindAndClickSidebarButton("btnEntrepot");
            actionsPanel.Controls.Add(btnGoEntrepot);

            var btnGoSets = CreateQuickAction("L", "Gestion des sets", "Ajouter, modifier\net supprimer des sets", Color.FromArgb(46, 204, 113));
            btnGoSets.Click += (s, e) => FindAndClickSidebarButton("btnSets");
            actionsPanel.Controls.Add(btnGoSets);

            var btnGoEmpl = CreateQuickAction("P", "Emplacements", "Gerer les emplacements\nde l'entrepot", Color.FromArgb(241, 196, 15));
            btnGoEmpl.Click += (s, e) => FindAndClickSidebarButton("btnEmplacements");
            actionsPanel.Controls.Add(btnGoEmpl);

            var btnGoHistorique = CreateQuickAction("H", "Historique", "Consulter l'historique\ndes actions", Color.FromArgb(230, 126, 34));
            btnGoHistorique.Click += (s, e) => FindAndClickSidebarButton("btnHistorique");
            actionsPanel.Controls.Add(btnGoHistorique);

            var btnGoStats = CreateQuickAction("S", "Statistiques", "Voir les rapports\net graphiques", Color.FromArgb(155, 89, 182));
            btnGoStats.Click += (s, e) => FindAndClickSidebarButton("btnStats");
            actionsPanel.Controls.Add(btnGoStats);

            mainPanel.Controls.Add(actionsPanel);

            // ====== Info footer ======
            var lblInfo = new Label
            {
                Text = "Utilisez le menu a gauche pour naviguer entre les differentes sections.",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(140, 150, 170),
                AutoSize = true,
                Margin = new Padding(0, 12, 0, 0),
                BackColor = BgColor
            };
            mainPanel.Controls.Add(lblInfo);

            Controls.Add(mainPanel);

            // Adapter la largeur du header à la zone visible
            mainPanel.Resize += (s, e) =>
            {
                int w = mainPanel.ClientSize.Width - mainPanel.Padding.Left - mainPanel.Padding.Right;
                if (w > 200) headerCard.Width = w;
            };
            int initW = mainPanel.ClientSize.Width - mainPanel.Padding.Left - mainPanel.Padding.Right;
            if (initW > 200) headerCard.Width = initW;
        }

        private void FindAndClickSidebarButton(string btnName)
        {
            var form = FindForm();
            if (form == null) return;
            // Defer to avoid validation/focus issues with PerformClick
            form.BeginInvoke(new Action(() =>
            {
                var btn = FindControlByName(form, btnName);
                if (btn is Button b)
                    b.PerformClick();
            }));
        }

        private static Control? FindControlByName(Control parent, string name)
        {
            if (parent.Name == name) return parent;
            foreach (Control c in parent.Controls)
            {
                var found = FindControlByName(c, name);
                if (found != null) return found;
            }
            return null;
        }

        /// <summary>
        /// Crée un petit panel circulaire coloré avec une lettre blanche au centre.
        /// Remplace les emojis qui s'affichent mal en WinForms.
        /// </summary>
        private static Panel CreateIconPanel(string letter, Color color, int size)
        {
            var panel = new Panel
            {
                Size = new Size(size, size),
                BackColor = Color.Transparent
            };
            panel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(color);
                e.Graphics.FillEllipse(brush, 0, 0, size - 1, size - 1);
                using var font = new Font("Segoe UI", size * 0.4F, FontStyle.Bold);
                using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(letter, font, Brushes.White, new RectangleF(0, 0, size, size), sf);
            };
            return panel;
        }

        private static Panel CreateStatCard(string iconLetter, string label, string value, Color accentColor)
        {
            var card = new Panel
            {
                Size = new Size(180, 110),
                BackColor = CardBg,
                Margin = new Padding(0, 0, 16, 8)
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 10);
                card.Region = new Region(path);
                // Barre de couleur en haut
                using var brush = new SolidBrush(accentColor);
                e.Graphics.FillRectangle(brush, 0, 0, card.Width, 4);
            };

            var iconPanel = CreateIconPanel(iconLetter, accentColor, 36);
            iconPanel.Location = new Point(14, 20);

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(62, 16),
                BackColor = CardBg
            };

            var lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(100, 115, 140),
                AutoSize = true,
                Location = new Point(14, 80),
                BackColor = CardBg
            };

            card.Controls.Add(iconPanel);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblLabel);
            return card;
        }

        private static ClickablePanel CreateQuickAction(string iconLetter, string title, string description, Color accentColor)
        {
            var card = new ClickablePanel
            {
                Size = new Size(220, 90),
                BackColor = CardBg,
                Margin = new Padding(0, 0, 16, 8),
                Cursor = Cursors.Hand
            };
            card.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = RoundedRect(new Rectangle(0, 0, card.Width - 1, card.Height - 1), 10);
                card.Region = new Region(path);
            };

            var iconPanel = CreateIconPanel(iconLetter, accentColor, 32);
            iconPanel.Location = new Point(12, 14);

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(55, 14),
                BackColor = CardBg
            };

            var lblDesc = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(120, 130, 150),
                Size = new Size(155, 36),
                Location = new Point(55, 40),
                BackColor = CardBg
            };

            // Hover
            var hoverColor = Color.FromArgb(235, 240, 250);
            card.MouseEnter += (s, e) => card.BackColor = hoverColor;
            card.MouseLeave += (s, e) => card.BackColor = CardBg;
            iconPanel.MouseEnter += (s, e) => card.BackColor = hoverColor;
            lblTitle.MouseEnter += (s, e) => card.BackColor = hoverColor;
            lblDesc.MouseEnter += (s, e) => card.BackColor = hoverColor;
            iconPanel.MouseLeave += (s, e) => card.BackColor = CardBg;
            lblTitle.MouseLeave += (s, e) => card.BackColor = CardBg;
            lblDesc.MouseLeave += (s, e) => card.BackColor = CardBg;

            // Propagate click
            iconPanel.Click += (s, e) => card.RaiseClick();
            lblTitle.Click += (s, e) => card.RaiseClick();
            lblDesc.Click += (s, e) => card.RaiseClick();

            card.Controls.Add(iconPanel);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblDesc);
            return card;
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    internal class ClickablePanel : Panel
    {
        public void RaiseClick() => OnClick(EventArgs.Empty);
    }
}