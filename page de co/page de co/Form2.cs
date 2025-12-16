using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace page_de_co
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var currentUser = CurrentUser.Instance;
            
            if (currentUser == null)
            {
                MessageBox.Show("Erreur: Utilisateur non identifié", "Erreur");
                this.Close();
                return;
            }
            
            Text = $"LegoFactory - {currentUser.Login} ({currentUser.Role})";
            
            // Par défaut, tout est visible et activé
            btnEntrepot.Visible = true;
            btnHistorique.Visible = true;
            btnStats.Visible = true;
            btnEmplacements.Visible = true;
            btnSets.Visible = true;
            btnImportExport.Visible = true;
            btnSync.Visible = true;
            btnUsersRoles.Visible = true;
            
            // Contrôle d'accès par rôle
            if (currentUser.Role == UserRole.Employe)
            {
                // Employé : consultation uniquement
                // Cache les menus de gestion
                btnEmplacements.Visible = false;
                btnSets.Visible = false;
                btnImportExport.Visible = false;
                btnSync.Visible = false;
                btnUsersRoles.Visible = false;
            }
            else if (currentUser.Role == UserRole.Responsable)
            {
                // Responsable : gestion opérationnelle mais pas les utilisateurs
                btnUsersRoles.Visible = false; // Cache la gestion des utilisateurs
            }
            // Admin : tout est visible (aucun cache)
            
            ShowView(new DashboardWelcome());
        }

        private void ShowView(Control view)
        {
            panelContent.Controls.Clear();
            view.Dock = DockStyle.Fill;
            panelContent.Controls.Add(view);
        }

        private void btnEntrepot_Click(object? sender, EventArgs e) => ShowView(new EntrepotView());
        private void btnHistorique_Click(object? sender, EventArgs e) => ShowView(new HistoriqueView());
        private void btnEmplacements_Click(object? sender, EventArgs e) => ShowView(new EmplacementsView());
        private void btnSets_Click(object? sender, EventArgs e) => ShowView(new SetsView());
        private void btnImportExport_Click(object? sender, EventArgs e) => ShowView(new ImportExportView());
        private void btnSync_Click(object? sender, EventArgs e) => ShowView(new SyncView());
        private void btnUsersRoles_Click(object? sender, EventArgs e) => ShowView(new UsersRolesView());
        private void btnStats_Click(object? sender, EventArgs e) => ShowView(new StatsView());
        
        private void btnLogout_Click(object? sender, EventArgs e)
        {
            // Réinitialiser l'utilisateur courant
            CurrentUser.Instance = null;
            
            // Afficher la page de connexion
            Form1 loginForm = new Form1();
            loginForm.Show();
            
            // Fermer le dashboard
            this.Close();
        }

    }

}
