namespace LegoFactory
{
    partial class DashboardAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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
            btnGroupAdmin = new Button();
            btnUsersRoles = new Button();
            btnMigratePasswords = new Button();
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
            btnUsersRoles.Name = "btnUsersRoles";
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

            // Accueil (standalone)
            StyleMenuButton(btnAccueil, "🏠   Accueil");

            // Group headers
            StyleGroupHeader(btnGroupStock, "▾  Gestion stock");
            StyleGroupHeader(btnGroupSuivi, "▾  Suivi & Outils");
            StyleGroupHeader(btnGroupAdmin, "▾  Administration");

            // Sub-buttons
            StyleSubButton(btnEntrepot, "      📦  Entrepôt");
            StyleSubButton(btnZones, "      🗺️  Zones");
            StyleSubButton(btnEmplacements, "      📍  Emplacements");
            StyleSubButton(btnSets, "      🧱  Sets");
            StyleSubButton(btnHistorique, "      📋  Historique");
            StyleSubButton(btnImportExport, "      📁  Import / Export");
            StyleSubButton(btnStats, "      📊  Statistiques");
            StyleSubButton(btnUsersRoles, "      👥  Utilisateurs");

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
            btnGroupAdmin.Click += btnGroupAdmin_Click;
            btnUsersRoles.Click += btnUsersRoles_Click;
            btnMigratePasswords.Click += btnMigratePasswords_Click;
            btnLogout.Click += btnLogout_Click;

            // Assemblage sidebar (ordre inversé car Dock=Top)
            var panelMenu = new Panel { Dock = DockStyle.Fill, BackColor = SidebarMain, AutoScroll = true };
            panelMenu.Controls.Add(btnUsersRoles);
            panelMenu.Controls.Add(btnGroupAdmin);
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
            Name = "DashboardAdmin";
            Text = "LegoFactory - Admin";
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ContentBg;
            Load += DashboardAdmin_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Panel panelHeader;
        private Label lblLogo;
        private Label lblUserInfo;
        private Panel panelContent;
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
        private Button btnGroupAdmin;
        private Button btnUsersRoles;
        private Button btnMigratePasswords;
        private Button btnLogout;
    }
}
