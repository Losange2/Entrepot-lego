using System;
using System.Drawing;
using System.Windows.Forms;

namespace LegoFactory
{
    public partial class DashboardAdmin : Form
    {
        private Button? _activeButton;
        private bool _stockExpanded = true;
        private bool _suiviExpanded = true;
        private bool _adminExpanded = true;

        private static readonly Color SidebarDark = Color.FromArgb(20, 40, 80);
        private static readonly Color SidebarMain = Color.FromArgb(30, 60, 114);
        private static readonly Color SidebarHover = Color.FromArgb(45, 85, 150);
        private static readonly Color SidebarActive = Color.FromArgb(55, 100, 170);
        private static readonly Color SubBg = Color.FromArgb(26, 52, 98);
        private static readonly Color ContentBg = Color.FromArgb(245, 247, 251);

        public DashboardAdmin()
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
                else if (btn == btnUsersRoles)
                { if (!_adminExpanded) btnGroupAdmin_Click(null, EventArgs.Empty); }
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

        private void btnGroupAdmin_Click(object? sender, EventArgs e)
        {
            _adminExpanded = !_adminExpanded;
            btnGroupAdmin.Text = (_adminExpanded ? "▾" : "▸") + "  Administration";
            btnUsersRoles.Visible = _adminExpanded;
        }

        private void DashboardAdmin_Load(object sender, EventArgs e)
        {
            var currentUser = CurrentUser.Instance;

            if (currentUser == null)
            {
                MessageBox.Show("Erreur: Utilisateur non identifi�", "Erreur");
                this.Close();
                return;
            }

            Text = $"LegoFactory — {currentUser.Login} ({currentUser.Role})";
            lblUserInfo.Text = $"👤 {currentUser.Login} — {currentUser.Role}";
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
        private void btnUsersRoles_Click(object? sender, EventArgs e) { SetActiveButton(btnUsersRoles); ShowView(new UsersRolesView()); }
        private void btnStats_Click(object? sender, EventArgs e) { SetActiveButton(btnStats); ShowView(new StatsView()); }

        private void btnLogout_Click(object? sender, EventArgs e)
        {
            CurrentUser.Instance = null;
            new LoginForm().Show();
            this.Close();
        }
    }
}
