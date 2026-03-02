using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace page_de_co
{
    public partial class Form2Responsable : Form
    {
        private Panel panelSidebar;
        private Panel panelContent;
        private Panel panelHeader;
        private Label lblLogo;
        private Label lblUserInfo;
        private Button btnEntrepot;
        private Button btnHistorique;
        private Button btnEmplacements;
        private Button btnSets;
        private Button btnImportExport;
        private Button btnSync;
        private Button btnStats;
        private Button btnLogout;
        private Button? _activeButton;

        private static readonly Color SidebarDark = Color.FromArgb(20, 40, 80);
        private static readonly Color SidebarMain = Color.FromArgb(30, 60, 114);
        private static readonly Color SidebarHover = Color.FromArgb(45, 85, 150);
        private static readonly Color SidebarActive = Color.FromArgb(55, 100, 170);
        private static readonly Color ContentBg = Color.FromArgb(245, 247, 251);

        public Form2Responsable()
        {
            InitializeComponent();
            this.Load += Form2Responsable_Load;
        }

        private void InitializeComponent()
        {
            panelSidebar = new Panel();
            panelHeader = new Panel();
            lblLogo = new Label();
            lblUserInfo = new Label();
            btnEntrepot = new Button();
            btnHistorique = new Button();
            btnEmplacements = new Button();
            btnSets = new Button();
            btnImportExport = new Button();
            btnSync = new Button();
            btnStats = new Button();
            btnLogout = new Button();
            panelContent = new Panel();
            SuspendLayout();

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

            // Boutons menu (Dock Top, ordre inversé d'ajout)
            StyleMenuButton(btnEntrepot, "📦   Consulter l'entrepôt");
            StyleMenuButton(btnHistorique, "📋   Historique des actions");
            StyleMenuButton(btnEmplacements, "📍   Gérer les emplacements");
            StyleMenuButton(btnSets, "🧱   Gérer les sets");
            StyleMenuButton(btnImportExport, "📁   Importer / Exporter");
            StyleMenuButton(btnSync, "🔄   Synchroniser stock");
            StyleMenuButton(btnStats, "📊   Statistiques & reporting");

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
            btnEntrepot.Click += btnEntrepot_Click;
            btnHistorique.Click += btnHistorique_Click;
            btnEmplacements.Click += btnEmplacements_Click;
            btnSets.Click += btnSets_Click;
            btnImportExport.Click += btnImportExport_Click;
            btnSync.Click += btnSync_Click;
            btnStats.Click += btnStats_Click;
            btnLogout.Click += btnLogout_Click;

            // Assemblage sidebar
            var panelMenu = new Panel { Dock = DockStyle.Fill, BackColor = SidebarMain };
            panelMenu.Controls.Add(btnStats);
            panelMenu.Controls.Add(btnSync);
            panelMenu.Controls.Add(btnImportExport);
            panelMenu.Controls.Add(btnSets);
            panelMenu.Controls.Add(btnEmplacements);
            panelMenu.Controls.Add(btnHistorique);
            panelMenu.Controls.Add(btnEntrepot);

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
            Name = "Form2Responsable";
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

        private void SetActiveButton(Button btn)
        {
            if (_activeButton != null)
            {
                _activeButton.BackColor = SidebarMain;
                _activeButton.ForeColor = Color.FromArgb(200, 215, 240);
            }
            _activeButton = btn;
            _activeButton.BackColor = SidebarActive;
            _activeButton.ForeColor = Color.White;
        }

        private void Form2Responsable_Load(object? sender, EventArgs e)
        {
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

        private void btnEntrepot_Click(object? sender, EventArgs e) { SetActiveButton(btnEntrepot); ShowView(new EntrepotView()); }
        private void btnHistorique_Click(object? sender, EventArgs e) { SetActiveButton(btnHistorique); ShowView(new HistoriqueView()); }
        private void btnEmplacements_Click(object? sender, EventArgs e) { SetActiveButton(btnEmplacements); ShowView(new EmplacementsView()); }
        private void btnSets_Click(object? sender, EventArgs e) { SetActiveButton(btnSets); ShowView(new SetsView()); }
        private void btnImportExport_Click(object? sender, EventArgs e) { SetActiveButton(btnImportExport); ShowView(new ImportExportView()); }
        private void btnSync_Click(object? sender, EventArgs e) { SetActiveButton(btnSync); ShowView(new SyncView()); }
        private void btnStats_Click(object? sender, EventArgs e) { SetActiveButton(btnStats); ShowView(new StatsView()); }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            CurrentUser.Instance = null;
            new Form1().Show();
            this.Close();
        }
    }
}
