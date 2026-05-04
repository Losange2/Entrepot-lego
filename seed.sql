-- seed.sql
-- Exemple de fixtures pour LegoFactory
-- Remplacez `your_database_name` par le nom de la base de données utilisée dans votre .env

USE `your_database_name`;

-- Tables de base
CREATE TABLE IF NOT EXISTS Entrepot (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Utilisateur (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    login VARCHAR(50) NOT NULL UNIQUE,
    motDePasse VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS Zone (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nom VARCHAR(100) NOT NULL,
    entrepot_id INT NOT NULL DEFAULT 1
);

CREATE TABLE IF NOT EXISTS Emplacement (
    id INT AUTO_INCREMENT PRIMARY KEY,
    code VARCHAR(20) NOT NULL UNIQUE,
    capaciteMax INT NOT NULL,
    DateEntree DATE NOT NULL,
    DateSorti DATE NULL,
    zone_id INT NOT NULL
);

CREATE TABLE IF NOT EXISTS LegoSet (
    id INT AUTO_INCREMENT PRIMARY KEY,
    Reference VARCHAR(100) NOT NULL UNIQUE,
    nom VARCHAR(255) NOT NULL,
    AgeCible INT NOT NULL,
    NombresPieces INT NOT NULL,
    quantiter INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS stocker (
    id INT AUTO_INCREMENT PRIMARY KEY,
    legoset_id INT NOT NULL,
    emplacement_id INT NOT NULL,
    quantiter INT NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS Mouvement (
    id INT AUTO_INCREMENT PRIMARY KEY,
    type VARCHAR(100) NOT NULL,
    date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    quantite INT NOT NULL,
    utilisateur_id INT NOT NULL,
    legoset_id INT NOT NULL
);

CREATE TABLE IF NOT EXISTS Historique (
    id INT AUTO_INCREMENT PRIMARY KEY,
    action VARCHAR(200) NOT NULL,
    description TEXT,
    date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    utilisateur_id INT NULL
);

-- Données de base
INSERT INTO Entrepot (nom) VALUES
('Entrepôt central')
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

INSERT INTO Utilisateur (nom, login, motDePasse, role) VALUES
('Administrateur Test', 'admin', 'admin123', 'Responsable'),
('Employé Test', 'employe', 'employe123', 'Employe')
ON DUPLICATE KEY UPDATE nom = VALUES(nom), role = VALUES(role);

INSERT INTO Zone (nom, entrepot_id) VALUES
('Zone A', 1),
('Zone B', 1)
ON DUPLICATE KEY UPDATE nom = VALUES(nom);

INSERT INTO Emplacement (code, capaciteMax, DateEntree, zone_id) VALUES
('A101', 20, CURRENT_DATE(), 1),
('A102', 15, CURRENT_DATE(), 1),
('B101', 30, CURRENT_DATE(), 2)
ON DUPLICATE KEY UPDATE capaciteMax = VALUES(capaciteMax), zone_id = VALUES(zone_id);

INSERT INTO LegoSet (Reference, nom, AgeCible, NombresPieces, quantiter) VALUES
('60001', 'Camion de pompiers', 8, 350, 10),
('60100', 'Avion de tourisme', 6, 220, 8),
('60200', 'Château fort', 10, 540, 5)
ON DUPLICATE KEY UPDATE nom = VALUES(nom), AgeCible = VALUES(AgeCible), NombresPieces = VALUES(NombresPieces), quantiter = VALUES(quantiter);

INSERT INTO stocker (legoset_id, emplacement_id, quantiter) VALUES
(1, 1, 5),
(2, 2, 3),
(3, 3, 2)
ON DUPLICATE KEY UPDATE quantiter = VALUES(quantiter);

INSERT INTO Mouvement (type, date, quantite, utilisateur_id, legoset_id) VALUES
('Entrée produit', NOW(), 5, 1, 1),
('Entrée produit', NOW(), 3, 1, 2),
('Entrée produit', NOW(), 2, 1, 3);

INSERT INTO Historique (action, description, utilisateur_id) VALUES
('Création utilisateur', 'Utilisateur admin créé pour tests.', 1),
('Création zone', 'Zones A et B ajoutées pour tests.', 1),
('Création emplacement', 'Emplacements A101, A102 et B101 ajoutés pour tests.', 1);
