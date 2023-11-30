CREATE TABLE `employes` (
  `id_employe` int PRIMARY KEY AUTO_INCREMENT,
  `nom` varchar(255),
  `prenom` varchar(255),
  `mot_de_passe` varchar(255),
  `mail` varchar(255),
  `role` varchar(255)
);

CREATE TABLE `articles` (
  `id_article` int PRIMARY KEY AUTO_INCREMENT,
  `nom` varchar(255),
  `famille` varchar(255),
  `prix_ht` int,
  `prix_ttc` int,
  `prix_achat` int,
  `code_barre` varchar(255),
  `description` varchar(255),
  `quantite` int,
  `date_ajout` timestamp,
  `image` jpg
);

CREATE TABLE `fournisseur` (
  `id_fournisseur` int PRIMARY KEY AUTO_INCREMENT,
  `id_produit` int,
  `nom` varchar(255),
  `prenom` varchar(255),
  `numero` int,
  `mail` varchar(255),
  `adresse` varchar(255)
);

CREATE TABLE `historique` (
  `id_historique` int PRIMARY KEY AUTO_INCREMENT,
  `nom_article` int,
  `prix_ht` int,
  `prix_ttc` int,
  `type` varchar(255),
  `code_barre` int,
  `quantite_avant` int,
  `quantite_actuelle` int,
  `total` int,
  `date` timestamp,
  `nom_employes` int
);

CREATE TABLE `ventes` (
  `id_ventes` int PRIMARY KEY AUTO_INCREMENT,
  `prix_ventes` int,
  `id_produit` int,
  `id_employe` int,
  `date` timestamp
);

ALTER TABLE `fournisseur` ADD FOREIGN KEY (`id_produit`) REFERENCES `articles` (`id_article`);

ALTER TABLE `historique` ADD FOREIGN KEY (`nom_article`) REFERENCES `articles` (`nom`);

ALTER TABLE `historique` ADD FOREIGN KEY (`quantite_actuelle`) REFERENCES `articles` (`quantite`);

ALTER TABLE `historique` ADD FOREIGN KEY (`nom_employes`) REFERENCES `employes` (`nom`);

ALTER TABLE `ventes` ADD FOREIGN KEY (`id_produit`) REFERENCES `articles` (`id_article`);

ALTER TABLE `ventes` ADD FOREIGN KEY (`id_employe`) REFERENCES `employes` (`id_employe`);

ALTER TABLE `historique` ADD FOREIGN KEY (`prix_ht`) REFERENCES `articles` (`prix_ht`);

ALTER TABLE `historique` ADD FOREIGN KEY (`prix_ttc`) REFERENCES `articles` (`prix_ttc`);

ALTER TABLE `historique` ADD FOREIGN KEY (`code_barre`) REFERENCES `articles` (`code_barre`);
