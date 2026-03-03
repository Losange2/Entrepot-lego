using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LegoFactory
{
    public partial class DashboardResponsable : Form
    {
        private Panel panelSidebar;
        private Panel panelContent;
        private Panel panelHeader;
        private Label lblLogo;
        private Label lblUserInfo;
        private Button btnAccueil;
        private Button btnGroupStock;
        private Button btnEntrepot;
        private Button btnZones;
        private Button btnEmplacements;
        private Button btnSets;
        private Button btnGroupSuivi;
        private Button btnHistorique;
        private Button btnImportExport;
        private Button btnStats;
        private Button btnLogout;
        private Button? _activeButton;
        private bool _stockExpanded = true;
        private bool _suiviExpanded = true;

        private static readonly Color SidebarDark = Color.FromArgb(20, 40, 80);
        private static readonly Color SidebarMain = Color.FromArgb(30, 60, 114);
        private static readonly Color SidebarHover = Color.FromArgb(45, 85, 150);
        private static readonly Color SidebarActive = Color.FromArgb(55, 100, 170);
        private static readonly Color SubBg = Color.FromArgb(26, 52, 98);
        private static readonly Color ContentBg = Color.FromArgb(245, 247, 251);

        public DashboardResponsable()
        {
            InitializeComponent();
            this.Load += DashboardResponsable_Load;
        }

        private void InitializeComponent()
        {
            panelSidebar = new Panel();
            panelHeader = new Panel();
            lblLogo = new Label();
            lblUserInfo = new Label();
            btnAccueil = new Button();
            btnGroupStock = new Button();
            btnEntrepot = new Button();
            btnZones = new Button();
            btnEmplacements = new Button();
            btnSets = new Button();
            btnGroupSuivi = new Button();
            btnHistorique = new Button();
            btnImportExport = new Button();
            btnStats = new Button();
            btnLogout = new Button();
            panelContent = new Panel();
            SuspendLayout();

            // Set Name for quick access navigation
            btnAccueil.Name = "btnAccueil";
            btnEntrepot.Name = "btnEntrepot";
            btnZones.Name = "btnZones";
            btnEmplacements.Name = "btnEmplacements";
            btnSets.Name = "btnSets";
            btnHistorique.Name = "btnHistorique";
            btnImportExport.Name = "btnImportExport";
            btnStats.Name = "btnStats";

            // --- Sidebar ---
            panelSidebar.BackColor = SidebarMain;
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 260;

            // Header
            panelHeader.BackColor = SidebarDark;
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 80;

            lblLogo.Text = "🏭  LegoFactory";
            lblLogo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.AutoSize = false;
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            lblLogo.Dock = DockStyle.Fill;
            panelHeader.Controls.Add(lblLogo);

            lblUserInfo.Text = "";
            lblUserInfo.Font = new Font("Segoe UI", 9F);
            lblUserInfo.ForeColor = Color.FromArgb(160, 180, 220);
            lblUserInfo.AutoSize = false;
            lblUserInfo.TextAlign = ContentAlignment.MiddleCenter;
            lblUserInfo.Dock = DockStyle.Top;
            lblUserInfo.Height = 35;
            lblUserInfo.BackColor = SidebarDark;

            // Boutons - Dock/Height de base (styles appliqués dans Load)
            btnAccueil.Dock = DockStyle.Top;
            btnAccueil.Height = 46;
            btnGroupStock.Dock = DockStyle.Top;
            btnGroupStock.Height = 34;
            btnEntrepot.Dock = DockStyle.Top;
            btnEntrepot.Height = 40;
            btnZones.Dock = DockStyle.Top;
            btnZones.Height = 40;
            btnEmplacements.Dock = DockStyle.Top;
            btnEmplacements.Height = 40;
            btnSets.Dock = DockStyle.Top;
            btnSets.Height = 40;
            btnGroupSuivi.Dock = DockStyle.Top;
            btnGroupSuivi.Height = 34;
            btnHistorique.Dock = DockStyle.Top;
            btnHistorique.Height = 40;
            btnImportExport.Dock = DockStyle.Top;
            btnImportExport.Height = 40;
            btnStats.Dock = DockStyle.Top;
            btnStats.Height = 40;

            // Logout
            btnLogout.Text = "🚪   Déconnexion";
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = Color.FromArgb(180, 50, 50);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 60, 60);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.Height = 48;
            btnLogout.TextAlign = ContentAlignment.MiddleCenter;

            // Events
            btnAccueil.Click += btnAccueil_Click;
            btnGroupStock.Click += btnGroupStock_Click;
            btnEntrepot.Click += btnEntrepot_Click;
            btnZones.Click += btnZones_Click;
            btnEmplacements.Click += btnEmplacements_Click;
            btnSets.Click += btnSets_Click;
            btnGroupSuivi.Click += btnGroupSuivi_Click;
            btnHistorique.Click += btnHistorique_Click;
            btnImportExport.Click += btnImportExport_Click;
            btnStats.Click += btnStats_Click;
            btnLogout.Click += btnLogout_Click;

            // Assemblage sidebar (ordre inversé car Dock=Top)
            var panelMenu = new Panel { Dock = DockStyle.Fill, BackColor = SidebarMain, AutoScroll = true };
            panelMenu.Controls.Add(btnStats);
            panelMenu.Controls.Add(btnImportExport);
            panelMenu.Controls.Add(btnHistorique);
            panelMenu.Controls.Add(btnGroupSuivi);
            panelMenu.Controls.Add(btnSets);
            panelMenu.Controls.Add(btnEmplacements);
            panelMenu.Controls.Add(btnZones);
            panelMenu.Controls.Add(btnEntrepot);
            panelMenu.Controls.Add(btnGroupStock);
            panelMenu.Controls.Add(btnAccueil);

            panelSidebar.Controls.Add(panelMenu);
            panelSidebar.Controls.Add(lblUserInfo);
            panelSidebar.Controls.Add(panelHeader);
            panelSidebar.Controls.Add(btnLogout);

            // --- Content ---
            panelContent.Dock = DockStyle.Fill;
            panelContent.BackColor = ContentBg;

            // --- Form ---
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 700);
            MinimumSize = new Size(800, 500);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            Name = "DashboardResponsable";
            Text = "LegoFactory — Responsable";
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ContentBg;
            ResumeLayout(false);
        }

        private void StyleMenuButton(Button b, string text)
        {
            b.Text = text;
            b.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            b.ForeColor = Color.FromArgb(200, 215, 240);
            b.BackColor = SidebarMain;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = SidebarHover;
            b.Height = 46;
            b.Dock = DockStyle.Top;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(20, 0, 0, 0);
            b.Cursor = Cursors.Hand;

            b.MouseEnter += (s, e) => { if (b != _activeButton) b.BackColor = SidebarHover; };
            b.MouseLeave += (s, e) => { if (b != _activeButton) b.BackColor = SidebarMain; };
        }

        private void StyleGroupHeader(Button b, string text)
        {
            b.Text = text;
            b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            b.ForeColor = Color.FromArgb(140, 170, 220);
            b.BackColor = Color.FromArgb(22, 48, 90);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(28, 55, 105);
            b.Height = 34;
            b.Dock = DockStyle.Top;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(14, 0, 0, 0);
            b.Cursor = Cursors.Hand;
        }

        private void StyleSubButton(Button b, string text)
        {
            b.Tag = "sub";
            b.Text = text;
            b.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            b.ForeColor = Color.FromArgb(180, 200, 230);
            b.BackColor = SubBg;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = SidebarHover;
            b.Height = 40;
            b.Dock = DockStyle.Top;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Padding = new Padding(30, 0, 0, 0);
            b.Cursor = Cursors.Hand;

            b.MouseEnter += (s, e) => { if (b != _activeButton) b.BackColor = SidebarHover; };
            b.MouseLeave += (s, e) => { if (b != _activeButton) b.BackColor = SubBg; };
        }

        private void SetActiveButton(Button btn)
        {
            // Auto-expand group if button is hidden
            if (!btn.Visible)
            {
                if (btn == btnEntrepot || btn == btnZones || btn == btnEmplacements || btn == btnSets)
                { if (!_stockExpanded) btnGroupStock_Click(null, EventArgs.Empty); }
                else if (btn == btnHistorique || btn == btnImportExport || btn == btnStats)
                { if (!_suiviExpanded) btnGroupSuivi_Click(null, EventArgs.Empty); }
            }

            if (_activeButton != null)
            {
                _activeButton.BackColor = _activeButton.Tag is "sub" ? SubBg : SidebarMain;
                _activeButton.ForeColor = Color.FromArgb(200, 215, 240);
            }
            _activeButton = btn;
            _activeButton.BackColor = SidebarActive;
            _activeButton.ForeColor = Color.White;
        }

        private void btnGroupStock_Click(object? sender, EventArgs e)
        {
            _stockExpanded = !_stockExpanded;
            btnGroupStock.Text = (_stockExpanded ? "▾" : "▸") + "  Gestion stock";
            btnEntrepot.Visible = _stockExpanded;
            btnZones.Visible = _stockExpanded;
            btnEmplacements.Visible = _stockExpanded;
            btnSets.Visible = _stockExpanded;
        }

        private void btnGroupSuivi_Click(object? sender, EventArgs e)
        {
            _suiviExpanded = !_suiviExpanded;
            btnGroupSuivi.Text = (_suiviExpanded ? "▾" : "▸") + "  Suivi & Outils";
            btnHistorique.Visible = _suiviExpanded;
            btnImportExport.Visible = _suiviExpanded;
            btnStats.Visible = _suiviExpanded;
        }

        private void DashboardResponsable_Load(object? sender, EventArgs e)
        {
            // Appliquer les styles aux boutons
            StyleMenuButton(btnAccueil, "🏠   Accueil");

            StyleGroupHeader(btnGroupStock, "▾  Gestion stock");
            StyleSubButton(btnEntrepot, "      📦  Entrepôt");
            StyleSubButton(btnZones, "      🗺️  Zones");
            StyleSubButton(btnEmplacements, "      📍  Emplacements");
            StyleSubButton(btnSets, "      🧱  Sets");

            StyleGroupHeader(btnGroupSuivi, "▾  Suivi & Outils");
            StyleSubButton(btnHistorique, "      📋  Historique");
            StyleSubButton(btnImportExport, "      📁  Import / Export");
            StyleSubButton(btnStats, "      📊  Statistiques");

            var currentUser = CurrentUser.Instance;
            if (currentUser != null)
            {
                Text = $"LegoFactory — {currentUser.Login} ({currentUser.Role})";
                lblUserInfo.Text = $"👤 {currentUser.Login} — {currentUser.Role}";
            }
            ShowView(new DashboardWelcome());
        }

        private void ShowView(Control view)
        {
            panelContent.Controls.Clear();
            view.Dock = DockStyle.Fill;
            panelContent.Controls.Add(view);
        }

        private void btnAccueil_Click(object? sender, EventArgs e)
        {
            if (_activeButton != null)
            {
                _activeButton.BackColor = _activeButton.Tag is "sub" ? SubBg : SidebarMain;
                _activeButton.ForeColor = Color.FromArgb(200, 215, 240);
                _activeButton = null;
            }
            ShowView(new DashboardWelcome());
        }
        private void btnEntrepot_Click(object? sender, EventArgs e) { SetActiveButton(btnEntrepot); ShowView(new EntrepotView()); }
        private void btnHistorique_Click(object? sender, EventArgs e) { SetActiveButton(btnHistorique); ShowView(new HistoriqueView()); }
        private void btnEmplacements_Click(object? sender, EventArgs e) { SetActiveButton(btnEmplacements); ShowView(new EmplacementsView()); }
        private void btnZones_Click(object? sender, EventArgs e) { SetActiveButton(btnZones); ShowView(new ZonesView()); }
        private void btnSets_Click(object? sender, EventArgs e) { SetActiveButton(btnSets); ShowView(new SetsView()); }
        private void btnImportExport_Click(object? sender, EventArgs e) { SetActiveButton(btnImportExport); ShowView(new ImportExportView()); }
        private void btnStats_Click(object? sender, EventArgs e) { SetActiveButton(btnStats); ShowView(new StatsView()); }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            CurrentUser.Instance = null;
            new LoginForm().Show();
            this.Close();
        }
    }
}
