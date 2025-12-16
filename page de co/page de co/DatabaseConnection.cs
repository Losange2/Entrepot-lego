using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DotNetEnv;
using System.IO;


namespace page_de_co
{
    public class DatabaseConnection
    {
        private readonly string connectionString;

        public DatabaseConnection()
        {
            // Charge les variables du fichier .env depuis le répertoire du projet
            string envPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".env");
            envPath = Path.GetFullPath(envPath);
            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
            }

            string? server = Environment.GetEnvironmentVariable("DB_SERVER");
            string? database = Environment.GetEnvironmentVariable("DB_NAME");
            string? user = Environment.GetEnvironmentVariable("DB_USER");
            string? password = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(database) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Variables d'environnement DB_SERVER, DB_NAME, DB_USER ou DB_PASSWORD manquantes.");
            }

            connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
        }

        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}