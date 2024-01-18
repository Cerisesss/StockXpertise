-- Drop tables
DROP TABLE IF EXISTS `achat`;
DROP TABLE IF EXISTS `vente`;
DROP TABLE IF EXISTS `stockage`;
DROP TABLE IF EXISTS `produit`;
DROP TABLE IF EXISTS `articles`;
DROP TABLE IF EXISTS `emplacement`;
DROP TABLE IF EXISTS `fournisseur`;
DROP TABLE IF EXISTS `mouvement`;  
DROP TABLE IF EXISTS `employes`;

CREATE TABLE fournisseur (
  id_fournisseur INT AUTO_INCREMENT PRIMARY KEY,
  nom VARCHAR(255),
  prenom VARCHAR(255),
  numero INT,
  mail VARCHAR(255),
  adresse VARCHAR(255)
);

CREATE TABLE employes (
  id_employes INT AUTO_INCREMENT PRIMARY KEY,
  nom VARCHAR(255),
  prenom VARCHAR(255),
  mot_de_passe VARCHAR(255),
  mail VARCHAR(255),
  role VARCHAR(255),
  date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE emplacement (
  id_emplacement INT AUTO_INCREMENT PRIMARY KEY,
  code VARCHAR(255),
  capacite INT,
  code_reel VARCHAR(255)
);

CREATE TABLE articles (
  id_articles INT AUTO_INCREMENT PRIMARY KEY,
  id_fournisseur INT,
  nom VARCHAR(255),
  famille VARCHAR(255),
  prix_ht INT,
  prix_ttc INT,
  prix_vente INT,
  prix_achat INT,
  description TEXT,
  date_ajout TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  code_barre VARCHAR(255),
  image VARCHAR(255),
  FOREIGN KEY (id_fournisseur) REFERENCES fournisseur(id_fournisseur)
);

CREATE TABLE produit (
  id_produit INT AUTO_INCREMENT PRIMARY KEY,
  id_articles INT,
  quantite_stock INT,
  quantite_stock_reel INT,
  id_emplacement INT,
  FOREIGN KEY (id_articles) REFERENCES articles(id_articles),
  FOREIGN KEY (`id_emplacement`) REFERENCES `emplacement` (`id_emplacement`)
);

CREATE TABLE mouvement (
  id_mouvement INT AUTO_INCREMENT PRIMARY KEY,
  id_employes INT,
  date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (id_employes) REFERENCES employes(id_employes)
);

CREATE TABLE vente (
  id_vente INT AUTO_INCREMENT PRIMARY KEY,
  id_produit INT,
  prix_ventes INT,
  date_vente TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  facture VARCHAR(255),
  id_mouvement INT,
  FOREIGN KEY (id_produit) REFERENCES produit(id_produit),
  FOREIGN KEY (id_mouvement) REFERENCES mouvement(id_mouvement)
);

CREATE TABLE achat (
  id_achat INT AUTO_INCREMENT PRIMARY KEY,
  id_produit INT,
  prix_achat INT,
  date_achat TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  facture VARCHAR(255),
  id_mouvement INT,
  FOREIGN KEY (id_produit) REFERENCES produit(id_produit),
  FOREIGN KEY (id_mouvement) REFERENCES mouvement(id_mouvement)
);