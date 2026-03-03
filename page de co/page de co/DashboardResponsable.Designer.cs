namespace LegoFactory
{
    partial class DashboardResponsable
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
            Name = "DashboardResponsable";
            Text = "LegoFactory — Responsable";
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ContentBg;
            Load += DashboardResponsable_Load;
            ResumeLayout(false);
        }

        #endregion

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
    }
}
