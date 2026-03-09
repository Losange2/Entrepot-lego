using MySql.Data.MySqlClient;
using System.IO;

namespace LegoFactory
{
    public partial class LoginForm : Form
    {
        private readonly DatabaseConnection _database;

        public LoginForm()
        {
            InitializeComponent();
            _database = new DatabaseConnection();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Charger le logo
            LoadLogo();

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

        private void LoadLogo()
        {
            try
            {
                string logoPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "img", "logo.png");
                logoPath = Path.GetFullPath(logoPath);

                if (File.Exists(logoPath))
                {
                    pblogo.Image = Image.FromFile(logoPath);
                }
            }
            catch
            {
                // Si le logo ne peut pas être chargé, on continue sans
            }
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
                        dashboard = new DashboardEmploye();
                    }
                    else if (CurrentUser.Instance.Role == UserRole.Responsable)
                    {
                        dashboard = new DashboardResponsable();
                    }
                    else if (CurrentUser.Instance.Role == UserRole.Admin)
                    {
                        dashboard = new DashboardAdmin();
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
                "SELECT id, role, motDePasse FROM Utilisateur WHERE login = @username LIMIT 1",
                connection);

            command.Parameters.AddWithValue("@username", username);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                int userId = (int)reader["id"];
                string roleStr = reader["role"]?.ToString() ?? "Employe";
                string storedPassword = reader["motDePasse"]?.ToString() ?? "";

                bool isAuthenticated = false;
                bool needsMigration = false;

                // Vérifier si c'est un hash BCrypt (commence par $2a$, $2b$, ou $2y$)
                if (storedPassword.StartsWith("$2a$") || storedPassword.StartsWith("$2b$") || storedPassword.StartsWith("$2y$"))
                {
                    // Mot de passe déjà haché - vérifier avec BCrypt
                    isAuthenticated = Security.PasswordHasher.VerifyPassword(password, storedPassword);
                }
                else
                {
                    // Ancien format en clair - comparaison directe
                    isAuthenticated = storedPassword == password;
                    needsMigration = isAuthenticated; // Si authentifié, on doit migrer
                }

                if (isAuthenticated)
                {
                    UserRole role = ParseRole(roleStr);
                    CurrentUser.Instance = new CurrentUser { Id = userId, Login = username, Role = role };

                    // Migration automatique si nécessaire
                    if (needsMigration)
                    {
                        reader.Close();
                        MigrateUserPassword(userId, password);
                    }

                    return true;
                }
            }
            return false;
        }

        // Migre automatiquement un mot de passe en clair vers BCrypt
        private void MigrateUserPassword(int userId, string plainPassword)
        {
            try
            {
                using var conn = _database.GetConnection();
                string hashedPassword = Security.PasswordHasher.HashPassword(plainPassword);
                using var cmd = new MySqlCommand(
                    "UPDATE Utilisateur SET motDePasse = @hash WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@hash", hashedPassword);
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Échec silencieux - l'utilisateur peut se connecter quand même
            }
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
