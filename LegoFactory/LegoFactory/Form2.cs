using System;
using System.Drawing;
using System.Windows.Forms;

namespace LegoFactory
{
    public partial class Form2Admin : Form
    {
        private Button? _activeButton;

        private static readonly Color SidebarDark = Color.FromArgb(20, 40, 80);
        private static readonly Color SidebarMain = Color.FromArgb(30, 60, 114);
        private static readonly Color SidebarHover = Color.FromArgb(45, 85, 150);
        private static readonly Color SidebarActive = Color.FromArgb(55, 100, 170);
        private static readonly Color ContentBg = Color.FromArgb(245, 247, 251);

        public Form2Admin()
        {
            InitializeComponent();
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

        private void Form2_Load(object sender, EventArgs e)
        {
            var currentUser = CurrentUser.Instance;

            if (currentUser == null)
            {
                MessageBox.Show("Erreur: Utilisateur non identifié", "Erreur");
                this.Close();
                return;
            }

            Text = $"LegoFactory — {currentUser.Login} ({currentUser.Role})";
            lblUserInfo.Text = $"?? {currentUser.Login} — {currentUser.Role}";
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
        private void btnUsersRoles_Click(object? sender, EventArgs e) { SetActiveButton(btnUsersRoles); ShowView(new UsersRolesView()); }
        private void btnStats_Click(object? sender, EventArgs e) { SetActiveButton(btnStats); ShowView(new StatsView()); }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            CurrentUser.Instance = null;
            new Form1().Show();
            this.Close();
        }
    }
}
