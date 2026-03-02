using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public partial class Form1 : Form
    {
        private readonly DatabaseConnection _database;

        public Form1()
        {
            InitializeComponent();
            _database = new DatabaseConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbutil.Focus();

            // Centrer la carte de login dans le panneau droit
            CenterLoginBox();
            panelRight.Resize += (s, ev) => CenterLoginBox();

            // Effet hover sur le bouton
            btnconnect.MouseEnter += (s, ev) => btnconnect.BackColor = Color.FromArgb(40, 80, 150);
            btnconnect.MouseLeave += (s, ev) => btnconnect.BackColor = Color.FromArgb(30, 60, 114);

            // Cacher l'erreur quand on tape
            tbutil.TextChanged += (s, ev) => HideError();
            tbmdp.TextChanged += (s, ev) => HideError();

            // Arrondir la carte (coins arrondis via Region)
            panelLoginBox.Paint += (s, ev) =>
            {
                using var path = RoundedRect(panelLoginBox.ClientRectangle, 16);
                panelLoginBox.Region = new Region(path);
            };
        }

        private void CenterLoginBox()
        {
            panelLoginBox.Location = new Point(
                (panelRight.Width - panelLoginBox.Width) / 2,
                (panelRight.Height - panelLoginBox.Height) / 2
            );
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private void HideError()
        {
            lblError.Visible = false;
            lblError.Text = "";
        }

        private void btnconnect_Click(object sender, EventArgs e)
        {
            HideError();

            if (string.IsNullOrWhiteSpace(tbutil.Text) || string.IsNullOrWhiteSpace(tbmdp.Text))
            {
                ShowError("Merci de remplir le nom d'utilisateur et le mot de passe.");
                return;
            }

            try
            {
                if (UserExists(tbutil.Text.Trim(), tbmdp.Text))
                {
                    // Pas de popup — on ouvre directement le dashboard
                    Form? dashboard = null;
                    if (CurrentUser.Instance.Role == UserRole.Employe)
                    {
                        dashboard = new Form2Employe();
                    }
                    else if (CurrentUser.Instance.Role == UserRole.Responsable)
                    {
                        dashboard = new Form2Responsable();
                    }
                    else if (CurrentUser.Instance.Role == UserRole.Admin)
                    {
                        dashboard = new Form2Admin();
                    }

                    if (dashboard != null)
                    {
                        dashboard.Show();
                        this.Hide();
                    }
                }
                else
                {
                    ShowError("Nom d'utilisateur ou mot de passe incorrect.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Erreur de connexion : {ex.Message}");
            }
        }

        // Vérifie dans la base si l'utilisateur/mot de passe existe.
        private bool UserExists(string username, string password)
        {
            using MySqlConnection connection = _database.GetConnection();
            using MySqlCommand command = new MySqlCommand(
                "SELECT id, role FROM Utilisateur WHERE login = @username AND motDePasse = @password LIMIT 1",
                connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int userId = (int)reader["id"];
                string roleStr = reader["role"]?.ToString() ?? "Employe";

                UserRole role = ParseRole(roleStr);
                CurrentUser.Instance = new CurrentUser { Id = userId, Login = username, Role = role };

                return true;
            }
            return false;
        }

        // Convertit une chaîne de rôle en enum UserRole
        private UserRole ParseRole(string roleStr)
        {
            if (string.IsNullOrWhiteSpace(roleStr))
                return UserRole.Employe;

            var normalized = roleStr.Trim();
            if (normalized.Length > 0)
            {
                normalized = char.ToUpper(normalized[0]) + normalized.Substring(1).ToLower();
            }

            if (Enum.TryParse<UserRole>(normalized, ignoreCase: true, out var role))
            {
                return role;
            }

            return normalized.ToLower() switch
            {
                "admin" => UserRole.Admin,
                "administrateur" => UserRole.Admin,
                "responsable" => UserRole.Responsable,
                "resp" => UserRole.Responsable,
                "employe" => UserRole.Employe,
                "employee" => UserRole.Employe,
                _ => UserRole.Employe
            };
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            tbmdp.PasswordChar = cbShowPassword.Checked ? '\0' : '●';
        }
    }
}
