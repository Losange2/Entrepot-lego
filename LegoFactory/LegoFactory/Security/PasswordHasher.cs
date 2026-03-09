namespace LegoFactory.Security
{
    /// <summary>
    /// Utilitaire pour hacher et vérifier les mots de passe de manière sécurisée.
    /// Utilise BCrypt avec un work factor de 12 pour une sécurité optimale.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hache un mot de passe en clair avec BCrypt.
        /// </summary>
        /// <param name="plainPassword">Le mot de passe en clair à hacher</param>
        /// <returns>Le hash BCrypt du mot de passe</returns>
        public static string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: 12);
        }

        /// <summary>
        /// Vérifie qu'un mot de passe en clair correspond au hash stocké.
        /// </summary>
        /// <param name="plainPassword">Le mot de passe en clair à vérifier</param>
        /// <param name="hashedPassword">Le hash BCrypt stocké en base</param>
        /// <returns>True si le mot de passe correspond, False sinon</returns>
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
