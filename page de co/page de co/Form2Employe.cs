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
    public partial class Form2Employe : Form
    {
        private Panel panelSidebar;
        private Panel panelContent;
        private Button btnEntrepot;
        private Button btnHistorique;
        private Button btnStats;
        private Button btnLogout;

        public Form2Employe()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panelSidebar = new Panel();
            this.btnEntrepot = new Button();
            this.btnHistorique = new Button();
            this.btnStats = new Button();
            this.btnLogout = new Button();
            this.panelContent = new Panel();

            // panelSidebar
            this.panelSidebar.BackColor = Color.FromArgb(44, 62, 80);
            this.panelSidebar.Dock = DockStyle.Left;
            this.panelSidebar.Width = 280;
            this.panelSidebar.Controls.Add(this.btnLogout);
            this.panelSidebar.Controls.Add(this.btnStats);
            this.panelSidebar.Controls.Add(this.btnHistorique);
            this.panelSidebar.Controls.Add(this.btnEntrepot);

            // Boutons de menu
            StyleMenuButton(btnEntrepot, "Consulter l'entrepôt", 20);
            StyleMenuButton(btnHistorique, "Historique des actions", 70);
            StyleMenuButton(btnStats, "Statistiques & reporting", 120);

            // Bouton déconnexion
            btnLogout.Text = "Déconnexion";
            btnLogout.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogout.ForeColor = Color.White;
            btnLogout.BackColor = Color.FromArgb(192, 57, 43);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Width = panelSidebar.Width - 30;
            btnLogout.Height = 40;
            btnLogout.Left = 15;
            btnLogout.Top = 720;

            // Events
            btnEntrepot.Click += btnEntrepot_Click;
            btnHistorique.Click += btnHistorique_Click;
            btnStats.Click += btnStats_Click;
            btnLogout.Click += btnLogout_Click;

            // panelContent
            this.panelContent.Dock = DockStyle.Fill;
            this.panelContent.BackColor = Color.WhiteSmoke;

            // Form2Employe
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1200, 800);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSidebar);
            this.Name = "Form2Employe";
            this.Text = "LegoFactory - Employé";
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void StyleMenuButton(Button b, string text, int top)
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

        private void Form2Employe_Load(object sender, EventArgs e)
        {
            var currentUser = CurrentUser.Instance;
            if (currentUser != null)
            {
                Text = $"LegoFactory - {currentUser.Login} ({currentUser.Role}) - LECTURE SEULE";
            }
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
        private void btnStats_Click(object? sender, EventArgs e) => ShowView(new StatsView());

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            CurrentUser.Instance = null;
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Close();
        }
    }
}
