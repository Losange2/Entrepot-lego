using System;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public static class HistoriqueHelper
    {
        private static bool _tableChecked = false;

        /// <summary>
        /// Enregistre une action dans la table Historique.
        /// </summary>
        public static void Log(string action, string description)
        {
            try
            {
                var db = new DatabaseConnection();
                using var conn = db.GetConnection();

                if (!_tableChecked)
                {
                    EnsureTable(conn);
                    _tableChecked = true;
                }

                var userId = CurrentUser.Instance?.Id ?? 0;
                using var cmd = new MySqlCommand(
                    "INSERT INTO Historique (action, description, date, utilisateur_id) VALUES (@action, @desc, NOW(), @uid)", conn);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // Silencieux — ne pas bloquer l'application si le log échoue
            }
        }

        private static void EnsureTable(MySqlConnection conn)
        {
            try
            {
                using var cmd = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Historique (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        action VARCHAR(100) NOT NULL,
                        description TEXT,
                        date DATETIME DEFAULT CURRENT_TIMESTAMP,
                        utilisateur_id INT,
                        FOREIGN KEY (utilisateur_id) REFERENCES Utilisateur(id) ON DELETE SET NULL
                    )", conn);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
    }
}
