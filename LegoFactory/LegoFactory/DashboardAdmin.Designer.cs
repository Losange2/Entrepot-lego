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
            panelMenu = new Panel();
            btnStats = new Button();
            btnUsersRoles = new Button();
            btnSync = new Button();
            btnImportExport = new Button();
            btnSets = new Button();
            btnEmplacements = new Button();
            btnHistorique = new Button();
            btnEntrepot = new Button();
            lblUserInfo = new Label();
            panelHeader = new Panel();
            lblLogo = new Label();
            btnLogout = new Button();
            panelContent = new Panel();
            panelSidebar.SuspendLayout();
            panelMenu.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.Controls.Add(panelMenu);
            panelSidebar.Controls.Add(lblUserInfo);
            panelSidebar.Controls.Add(panelHeader);
            panelSidebar.Controls.Add(btnLogout);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Margin = new Padding(3, 4, 3, 4);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Size = new Size(297, 933);
            panelSidebar.TabIndex = 1;
            panelSidebar.Paint += panelSidebar_Paint;
            // 
            // panelMenu
            // 
            panelMenu.Controls.Add(btnStats);
            panelMenu.Controls.Add(btnUsersRoles);
            panelMenu.Controls.Add(btnSync);
            panelMenu.Controls.Add(btnImportExport);
            panelMenu.Controls.Add(btnSets);
            panelMenu.Controls.Add(btnEmplacements);
            panelMenu.Controls.Add(btnHistorique);
            panelMenu.Controls.Add(btnEntrepot);
            panelMenu.Location = new Point(0, 0);
            panelMenu.Margin = new Padding(3, 4, 3, 4);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(229, 133);
            panelMenu.TabIndex = 0;
            panelMenu.Paint += panelMenu_Paint;
            // 
            // btnStats
            // 
            btnStats.Location = new Point(0, 0);
            btnStats.Margin = new Padding(3, 4, 3, 4);
            btnStats.Name = "btnStats";
            btnStats.Size = new Size(86, 31);
            btnStats.TabIndex = 0;
            btnStats.Click += btnStats_Click;
            // 
            // btnUsersRoles
            // 
            btnUsersRoles.Location = new Point(0, 0);
            btnUsersRoles.Margin = new Padding(3, 4, 3, 4);
            btnUsersRoles.Name = "btnUsersRoles";
            btnUsersRoles.Size = new Size(86, 31);
            btnUsersRoles.TabIndex = 1;
            btnUsersRoles.Click += btnUsersRoles_Click;
            // 
            // btnSync
            // 
            btnSync.Location = new Point(0, 0);
            btnSync.Margin = new Padding(3, 4, 3, 4);
            btnSync.Name = "btnSync";
            btnSync.Size = new Size(86, 31);
            btnSync.TabIndex = 2;
            btnSync.Click += btnSync_Click;
            // 
            // btnImportExport
            // 
            btnImportExport.Location = new Point(0, 0);
            btnImportExport.Margin = new Padding(3, 4, 3, 4);
            btnImportExport.Name = "btnImportExport";
            btnImportExport.Size = new Size(86, 31);
            btnImportExport.TabIndex = 3;
            btnImportExport.Click += btnImportExport_Click;
            // 
            // btnSets
            // 
            btnSets.Location = new Point(0, 0);
            btnSets.Margin = new Padding(3, 4, 3, 4);
            btnSets.Name = "btnSets";
            btnSets.Size = new Size(86, 31);
            btnSets.TabIndex = 4;
            btnSets.Click += btnSets_Click;
            // 
            // btnEmplacements
            // 
            btnEmplacements.Location = new Point(0, 0);
            btnEmplacements.Margin = new Padding(3, 4, 3, 4);
            btnEmplacements.Name = "btnEmplacements";
            btnEmplacements.Size = new Size(86, 31);
            btnEmplacements.TabIndex = 5;
            btnEmplacements.Click += btnEmplacements_Click;
            // 
            // btnHistorique
            // 
            btnHistorique.Location = new Point(0, 0);
            btnHistorique.Margin = new Padding(3, 4, 3, 4);
            btnHistorique.Name = "btnHistorique";
            btnHistorique.Size = new Size(86, 31);
            btnHistorique.TabIndex = 6;
            btnHistorique.Click += btnHistorique_Click;
            // 
            // btnEntrepot
            // 
            btnEntrepot.Location = new Point(0, 0);
            btnEntrepot.Margin = new Padding(3, 4, 3, 4);
            btnEntrepot.Name = "btnEntrepot";
            btnEntrepot.Size = new Size(86, 31);
            btnEntrepot.TabIndex = 7;
            btnEntrepot.Click += btnEntrepot_Click;
            // 
            // lblUserInfo
            // 
            lblUserInfo.Dock = DockStyle.Top;
            lblUserInfo.Font = new Font("Segoe UI", 9F);
            lblUserInfo.ForeColor = Color.FromArgb(160, 180, 220);
            lblUserInfo.Location = new Point(0, 107);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Size = new Size(297, 47);
            lblUserInfo.TabIndex = 1;
            lblUserInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(lblLogo);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(3, 4, 3, 4);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(297, 107);
            panelHeader.TabIndex = 2;
            // 
            // lblLogo
            // 
            lblLogo.Dock = DockStyle.Fill;
            lblLogo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(0, 0);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(297, 107);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "🏭  LegoFactory";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.FromArgb(180, 50, 50);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Dock = DockStyle.Bottom;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 60, 60);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.Location = new Point(0, 869);
            btnLogout.Margin = new Padding(3, 4, 3, 4);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(297, 64);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "🚪   Déconnexion";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(297, 0);
            panelContent.Margin = new Padding(3, 4, 3, 4);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(960, 933);
            panelContent.TabIndex = 0;
            // 
            // DashboardAdmin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1257, 933);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(912, 651);
            Name = "DashboardAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LegoFactory - Admin";
            Load += DashboardAdmin_Load;
            panelSidebar.ResumeLayout(false);
            panelMenu.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
        private Panel panelHeader;
        private Label lblLogo;
        private Label lblUserInfo;
        private Panel panelContent;
        private Button btnEntrepot;
        private Button btnHistorique;
        private Button btnEmplacements;
        private Button btnSets;
        private Button btnImportExport;
        private Button btnSync;
        private Button btnUsersRoles;
        private Button btnStats;
        private Button btnLogout;
        private Panel panelMenu;
    }
}
