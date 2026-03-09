using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace LegoFactory.Utils
{
    /// <summary>
    /// Outil de migration pour hacher les mots de passe existants en clair dans la base de données.
    /// À exécuter UNE SEULE FOIS après l'implémentation du système de hachage.
    /// </summary>
    public static class PasswordMigrationTool
    {
        /// <summary>
        /// Migre tous les mots de passe en clair vers des hashes BCrypt.
        /// </summary>
        public static void MigrateAllPasswords()
        {
            var result = MessageBox.Show(
                "⚠️ Cette opération va hacher tous les mots de passe en base de données.\n\n" +
                "Attention : Cette action est IRRÉVERSIBLE.\n\n" +
                "Les mots de passe actuels en clair seront remplacés par des hashes sécurisés.\n\n" +
                "Voulez-vous continuer ?",
                "Migration des mots de passe",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();

                // 1. Récupérer tous les utilisateurs avec leurs mots de passe actuels
                var usersToMigrate = new System.Collections.Generic.List<(int id, string password)>();

                using (var cmdSelect = new MySqlCommand(
                    "SELECT id, motDePasse FROM Utilisateur WHERE motDePasse IS NOT NULL AND motDePasse != ''", conn))
                {
                    using var reader = cmdSelect.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = (int)reader["id"];
                        string currentPassword = reader["motDePasse"]?.ToString() ?? "";

                        // Vérifier si c'est déjà un hash BCrypt (commence par $2a$, $2b$, $2y$)
                        if (!currentPassword.StartsWith("$2a$") && 
                            !currentPassword.StartsWith("$2b$") && 
                            !currentPassword.StartsWith("$2y$"))
                        {
                            usersToMigrate.Add((id, currentPassword));
                        }
                    }
                }

                // 2. Hacher et mettre à jour chaque mot de passe
                int migratedCount = 0;
                foreach (var (id, plainPassword) in usersToMigrate)
                {
                    string hashedPassword = Security.PasswordHasher.HashPassword(plainPassword);

                    using var cmdUpdate = new MySqlCommand(
                        "UPDATE Utilisateur SET motDePasse = @hashedPassword WHERE id = @id", conn);
                    cmdUpdate.Parameters.AddWithValue("@hashedPassword", hashedPassword);
                    cmdUpdate.Parameters.AddWithValue("@id", id);
                    cmdUpdate.ExecuteNonQuery();

                    migratedCount++;
                }

                MessageBox.Show(
                    $"✅ Migration réussie !\n\n" +
                    $"Nombre d'utilisateurs migrés : {migratedCount}\n\n" +
                    $"Les mots de passe sont maintenant sécurisés avec BCrypt.",
                    "Migration terminée",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                HistoriqueHelper.Log("Migration sécurité", $"Migration de {migratedCount} mots de passe vers BCrypt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"❌ Erreur lors de la migration :\n\n{ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Génère un hash BCrypt pour un mot de passe donné (utile pour les tests).
        /// </summary>
        public static string GenerateHash(string plainPassword)
        {
            return Security.PasswordHasher.HashPassword(plainPassword);
        }
    }
}
