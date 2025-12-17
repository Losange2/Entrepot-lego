using MySql.Data.MySqlClient;

namespace page_de_co
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

        }

        private void ltitre_Click(object sender, EventArgs e)
        {

        }

        private void pblogo_Click(object sender, EventArgs e)
        {

        }

        private void tbutil_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnconnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbutil.Text) || string.IsNullOrWhiteSpace(tbmdp.Text))
            {
                MessageBox.Show("Merci de remplir le nom d'utilisateur et le mot de passe.");
                return;
            }

            try
            {
                if (UserExists(tbutil.Text.Trim(), tbmdp.Text))
                {
                    MessageBox.Show("Connexion réussie !");
                    
                    // Ouvrir le bon dashboard selon le rôle
                    Form dashboard = null;
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
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la connexion à la base : {ex.Message}");
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
                
                // Normaliser le rôle
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

            // Normaliser : trim et première lettre majuscule
            var normalized = roleStr.Trim();
            if (normalized.Length > 0)
            {
                normalized = char.ToUpper(normalized[0]) + normalized.Substring(1).ToLower();
            }

            // Essayer de parser l'enum
            if (Enum.TryParse<UserRole>(normalized, ignoreCase: true, out var role))
            {
                return role;
            }

            // Fallback si rien ne marche
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
            tbmdp.PasswordChar = cbShowPassword.Checked ? '\0' : '*';
        }
    }
}
