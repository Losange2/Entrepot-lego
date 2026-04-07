# Entrepot-lego

**Entrepot-lego** est une application de gestion d’entrepôt dédiée au rangement et au suivi de pièces LEGO.  
Elle permet de référencer les pièces disponibles, d’organiser leur stockage (boîtes, bacs, emplacements), et de retrouver rapidement où se situe une pièce précise. L’objectif est de simplifier l’inventaire, éviter les pertes de temps lors de la recherche de pièces, et mieux gérer les quantités (ajouts, retraits, mise à jour du stock).

## Base de données (MariaDB)

L’application s’appuie sur une **base de données MariaDB** afin de stocker de manière persistante l’ensemble des informations (pièces, emplacements, quantités, mouvements de stock, etc.).

La connexion à la base est configurée via un fichier **`.env`**, ce qui permet de séparer la configuration du code et de changer facilement d’environnement (local / développement / production) sans modifier le projet.

## Configuration (.env)

1. Crée un fichier `.env` à la racine du projet.
2. Renseigne les variables suivantes :

```env
# MariaDB
DB_HOST=localhost
DB_PORT=3306
DB_NAME=entrepot_lego
DB_USER=root
DB_PASSWORD=motdepasse

# Optionnel (selon le projet) : environnement
# NODE_ENV=development
```
