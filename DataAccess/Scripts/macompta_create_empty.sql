-- --------------------------------------------------------
-- Hôte:                         127.0.0.1
-- Version du serveur:           5.5.29-MariaDB - mariadb.org binary distribution
-- SE du serveur:                Win32
-- HeidiSQL Version:             11.3.0.6295
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Listage de la structure de la base pour macompta_test
CREATE DATABASE IF NOT EXISTS `macompta_test` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci */;
USE `macompta_test`;

-- Listage de la structure de la table macompta_test. banques
CREATE TABLE IF NOT EXISTS `banques` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `designation` varchar(30) DEFAULT NULL,
  `adresse` text,
  `code_postal` varchar(5) DEFAULT NULL,
  `ville` varchar(30) DEFAULT NULL,
  `notes` text,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. budgets
CREATE TABLE IF NOT EXISTS `budgets` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `compte_id` int(11) DEFAULT NULL,
  `designation` varchar(30) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. budget_lignes
CREATE TABLE IF NOT EXISTS `budget_lignes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `budget_id` int(11) DEFAULT NULL,
  `rubrique_id` int(11) DEFAULT NULL,
  `sousrubrique_id` int(11) DEFAULT NULL,
  `jan` decimal(10,2) DEFAULT NULL,
  `fev` decimal(10,2) DEFAULT NULL,
  `mar` decimal(10,2) DEFAULT NULL,
  `avr` decimal(10,2) DEFAULT NULL,
  `mai` decimal(10,2) DEFAULT NULL,
  `jun` decimal(10,2) DEFAULT NULL,
  `jul` decimal(10,2) DEFAULT NULL,
  `aou` decimal(10,2) DEFAULT NULL,
  `sep` decimal(10,2) DEFAULT NULL,
  `oct` decimal(10,2) DEFAULT NULL,
  `nov` decimal(10,2) DEFAULT NULL,
  `dez` decimal(10,2) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `type_ligne` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. cheques
CREATE TABLE IF NOT EXISTS `cheques` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `numero` varchar(7) DEFAULT NULL,
  `operation_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `depot_cheque_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. comptas
CREATE TABLE IF NOT EXISTS `comptas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `description` varchar(50) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. comptes
CREATE TABLE IF NOT EXISTS `comptes` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `numero` varchar(20) DEFAULT NULL,
  `designation` varchar(50) DEFAULT NULL,
  `compta_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `banque_id` int(11) DEFAULT NULL,
  `carte_bancaire` varchar(16) DEFAULT NULL,
  `actif` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. comptes_utilisateurs
CREATE TABLE IF NOT EXISTS `comptes_utilisateurs` (
  `utilisateur_id` int(11) NOT NULL,
  `compte_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. details
CREATE TABLE IF NOT EXISTS `details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `montant` decimal(10,2) DEFAULT NULL,
  `commentaire` text,
  `operation_id` int(11) DEFAULT NULL,
  `rubrique_id` int(11) DEFAULT NULL,
  `sousrubrique_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `montant_budget` decimal(10,2) DEFAULT '0.00',
  `lien_detail_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. operations
CREATE TABLE IF NOT EXISTS `operations` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date_operation` datetime DEFAULT NULL,
  `date_validation` datetime DEFAULT NULL,
  `ordre` varchar(35) DEFAULT NULL,
  `compte_id` int(11) DEFAULT NULL,
  `type_paiement` varchar(3) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `date_validation_partielle` datetime DEFAULT NULL,
  `numero_cheque` varchar(7) DEFAULT NULL,
  `report` tinyint(1) NOT NULL,
  `virement_auto` tinyint(1) NOT NULL,
  `montant_budget` decimal(10,2) DEFAULT NULL,
  `lien_operation_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. predefinis
CREATE TABLE IF NOT EXISTS `predefinis` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ordre` varchar(35) DEFAULT NULL,
  `type_paiement` varchar(3) DEFAULT NULL,
  `commentaire` text,
  `rubrique_id` int(11) DEFAULT NULL,
  `sousrubrique_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. roles
CREATE TABLE IF NOT EXISTS `roles` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. roles_utilisateurs
CREATE TABLE IF NOT EXISTS `roles_utilisateurs` (
  `role_id` int(11) DEFAULT NULL,
  `utilisateur_id` int(11) DEFAULT NULL,
  KEY `index_roles_utilisateurs_on_role_id` (`role_id`),
  KEY `index_roles_utilisateurs_on_utilisateur_id` (`utilisateur_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. rubriques
CREATE TABLE IF NOT EXISTS `rubriques` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `designation` varchar(30) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. schema_info
CREATE TABLE IF NOT EXISTS `schema_info` (
  `version` int(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. sousrubriques
CREATE TABLE IF NOT EXISTS `sousrubriques` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `designation` varchar(30) DEFAULT NULL,
  `rubrique_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. stats
CREATE TABLE IF NOT EXISTS `stats` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `compte_id` int(11) DEFAULT NULL,
  `ordre` varchar(255) DEFAULT NULL,
  `date_debut` date DEFAULT NULL,
  `date_fin` date DEFAULT NULL,
  `rubrique_id` int(11) DEFAULT NULL,
  `sousrubrique_id` int(11) DEFAULT NULL,
  `type_paiement` varchar(255) DEFAULT NULL,
  `group1` varchar(30) DEFAULT NULL,
  `group2` varchar(30) DEFAULT NULL,
  `group3` varchar(30) DEFAULT NULL,
  `group4` varchar(30) DEFAULT NULL,
  `group5` varchar(30) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `date_validation_debut` date DEFAULT NULL,
  `date_validation_fin` date DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. utilisateurs
CREATE TABLE IF NOT EXISTS `utilisateurs` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nom` varchar(255) DEFAULT NULL,
  `prenom` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `login` varchar(40) DEFAULT NULL,
  `crypted_password` varchar(40) DEFAULT NULL,
  `salt` varchar(40) DEFAULT NULL,
  `remember_token` varchar(40) DEFAULT NULL,
  `remember_token_expires_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `index_utilisateurs_on_login` (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. virements
CREATE TABLE IF NOT EXISTS `virements` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `duree` tinyint(4) DEFAULT NULL,
  `jour` tinyint(4) DEFAULT NULL,
  `compte_src_id` int(11) DEFAULT NULL,
  `compte_dst_id` int(11) DEFAULT NULL,
  `ordre` varchar(35) DEFAULT NULL,
  `date_dernier` date DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `description` varchar(35) DEFAULT NULL,
  `frequence` tinyint(4) DEFAULT '1' COMMENT '1=mensuel, 2=annuel, 3=hebdomadaire',
  `montant` decimal(10,2) DEFAULT '0.00',
  `type_paiement` varchar(3) DEFAULT 'VRT',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. virement_details
CREATE TABLE IF NOT EXISTS `virement_details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `montant` decimal(10,2) DEFAULT NULL,
  `commentaire` text,
  `virement_id` int(11) DEFAULT NULL,
  `rubrique_id` int(11) DEFAULT NULL,
  `sousrubrique_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `compte_src_only` tinyint(1) DEFAULT NULL,
  `compte_dst_only` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

-- Listage de la structure de la table macompta_test. virement_montants
CREATE TABLE IF NOT EXISTS `virement_montants` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `montant` decimal(10,2) DEFAULT NULL,
  `mois` int(11) DEFAULT NULL,
  `virementdetail_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- Les données exportées n'étaient pas sélectionnées.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
