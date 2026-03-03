namespace page_de_co
{
    partial class Form2
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
            btnEntrepot = new Button();
            btnHistorique = new Button();
            btnEmplacements = new Button();
            btnSets = new Button();
            btnImportExport = new Button();
            btnSync = new Button();
            btnUsersRoles = new Button();
            btnStats = new Button();
            btnLogout = new Button();
            panelContent = new Panel();
            panelSidebar.SuspendLayout();
            SuspendLayout();
            // 
            // panelSidebar
            // 
            panelSidebar.BackColor = Color.FromArgb(44, 62, 80);
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = 280;
            panelSidebar.Controls.Add(btnLogout);
            panelSidebar.Controls.Add(btnStats);
            panelSidebar.Controls.Add(btnUsersRoles);
            panelSidebar.Controls.Add(btnSync);
            panelSidebar.Controls.Add(btnImportExport);
            panelSidebar.Controls.Add(btnSets);
            panelSidebar.Controls.Add(btnEmplacements);
            panelSidebar.Controls.Add(btnHistorique);
            panelSidebar.Controls.Add(btnEntrepot);
            // 
            // Common button style helper
            // 
            void StyleMenuButton(Button b, string text, int top)
            {
                b.Text = text;
                b.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                b.ForeColor = Color.White;
                b.BackColor = Color.FromArgb(52, 73, 94);
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Width = panelSidebar.Width - 30;
                b.Height = 40;
                b.Left = 15;
                b.Top = top;
            }
            // 
            // Buttons
            // 
            StyleMenuButton(btnEntrepot, "Consulter l'entrepôt", 20);
            StyleMenuButton(btnHistorique, "Historique des actions", 70);
            StyleMenuButton(btnEmplacements, "Gérer les emplacements", 120);
            StyleMenuButton(btnSets, "Gérer les sets", 170);
            StyleMenuButton(btnImportExport, "Importer / Exporter", 220);
            StyleMenuButton(btnSync, "Synchroniser stock", 270);
            StyleMenuButton(btnUsersRoles, "Utilisateurs et rôles", 320);
            StyleMenuButton(btnStats, "Statistiques & reporting", 370);
            // Bouton de déconnexion en bas
            btnLogout.Text = "Déconnexion";
            btnLogout.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = Color.FromArgb(192, 57, 43);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Width = panelSidebar.Width - 30;
            btnLogout.Height = 40;
            btnLogout.Left = 15;
            btnLogout.Top = 730;
            // 
            // Events
            // 
            btnEntrepot.Click += btnEntrepot_Click;
            btnHistorique.Click += btnHistorique_Click;
            btnEmplacements.Click += btnEmplacements_Click;
            btnSets.Click += btnSets_Click;
            btnImportExport.Click += btnImportExport_Click;
            btnSync.Click += btnSync_Click;
            btnUsersRoles.Click += btnUsersRoles_Click;
            btnStats.Click += btnStats_Click;
            btnLogout.Click += btnLogout_Click;
            // 
            // panelContent
            // 
            panelContent.Dock = DockStyle.Fill;
            panelContent.BackColor = Color.WhiteSmoke;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 800);
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            Name = "Form2";
            Text = "LegoFactory - Tableau de bord";
            StartPosition = FormStartPosition.CenterScreen;
            panelSidebar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSidebar;
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
    }
}