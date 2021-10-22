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

-- Listage des données de la table macompta_test.banques : ~1 rows (environ)
/*!40000 ALTER TABLE `banques` DISABLE KEYS */;
INSERT INTO `banques` (`id`, `designation`, `adresse`, `code_postal`, `ville`, `notes`, `created_at`, `updated_at`) VALUES
	(1, 'banque1', NULL, '35220', 'st jean', NULL, '2017-09-06 15:55:53', '2017-09-06 15:55:53');
/*!40000 ALTER TABLE `banques` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. budgets
CREATE TABLE IF NOT EXISTS `budgets` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `compte_id` int(11) DEFAULT NULL,
  `designation` varchar(30) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.budgets : ~0 rows (environ)
/*!40000 ALTER TABLE `budgets` DISABLE KEYS */;
/*!40000 ALTER TABLE `budgets` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.budget_lignes : ~0 rows (environ)
/*!40000 ALTER TABLE `budget_lignes` DISABLE KEYS */;
/*!40000 ALTER TABLE `budget_lignes` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.cheques : ~0 rows (environ)
/*!40000 ALTER TABLE `cheques` DISABLE KEYS */;
/*!40000 ALTER TABLE `cheques` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. comptas
CREATE TABLE IF NOT EXISTS `comptas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `description` varchar(50) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.comptas : ~2 rows (environ)
/*!40000 ALTER TABLE `comptas` DISABLE KEYS */;
INSERT INTO `comptas` (`id`, `description`, `created_at`, `updated_at`) VALUES
	(1, 'Compta test', NULL, NULL),
	(2, 'compta 2', '2017-10-21 16:09:36', '2017-10-21 16:09:37');
/*!40000 ALTER TABLE `comptas` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.comptes : ~2 rows (environ)
/*!40000 ALTER TABLE `comptes` DISABLE KEYS */;
INSERT INTO `comptes` (`id`, `numero`, `designation`, `compta_id`, `created_at`, `updated_at`, `banque_id`, `carte_bancaire`, `actif`) VALUES
	(2, '456654564', 'Compte Flo', 1, '2017-10-09 07:05:53', '2017-10-09 07:05:53', 1, '5565654545', 1),
	(3, '45455', 'compte 1', 2, '2017-10-21 16:11:22', '2021-10-20 09:01:09', 1, NULL, 1);
/*!40000 ALTER TABLE `comptes` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. comptes_utilisateurs
CREATE TABLE IF NOT EXISTS `comptes_utilisateurs` (
  `utilisateur_id` int(11) NOT NULL,
  `compte_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.comptes_utilisateurs : ~0 rows (environ)
/*!40000 ALTER TABLE `comptes_utilisateurs` DISABLE KEYS */;
/*!40000 ALTER TABLE `comptes_utilisateurs` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.details : ~8 rows (environ)
/*!40000 ALTER TABLE `details` DISABLE KEYS */;
INSERT INTO `details` (`id`, `montant`, `commentaire`, `operation_id`, `rubrique_id`, `sousrubrique_id`, `created_at`, `updated_at`, `montant_budget`, `lien_detail_id`) VALUES
	(9, -50.00, NULL, 4, 3, 4, '2017-10-09 07:19:19', '2017-10-09 07:19:19', 0.00, NULL),
	(10, -7.00, NULL, 4, 3, 5, '2017-10-09 07:19:33', '2017-10-09 07:19:33', 0.00, NULL),
	(11, -100.00, NULL, 5, 4, 7, '2017-10-09 07:22:29', '2017-10-09 07:22:29', 0.00, NULL),
	(12, -20.00, NULL, 5, 1, 1, '2017-10-09 07:22:29', '2017-10-09 07:22:29', 0.00, NULL),
	(13, -5.00, 'sdgsdgsd', 4, 1, 1, '2017-10-09 07:25:06', '2017-10-09 07:25:06', 0.00, NULL),
	(14, -10.00, NULL, 6, 3, 4, '2017-11-08 07:13:50', '2017-11-08 07:13:50', 0.00, NULL),
	(16, -50.00, NULL, 8, 4, 7, '2017-11-22 15:37:51', '2017-11-22 15:37:51', 0.00, NULL),
	(17, -25.00, 'dfghsdfh', 9, 3, 4, '2021-10-17 09:19:02', '2021-10-17 09:45:38', 0.00, NULL);
/*!40000 ALTER TABLE `details` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.operations : ~5 rows (environ)
/*!40000 ALTER TABLE `operations` DISABLE KEYS */;
INSERT INTO `operations` (`id`, `date_operation`, `date_validation`, `ordre`, `compte_id`, `type_paiement`, `created_at`, `updated_at`, `date_validation_partielle`, `numero_cheque`, `report`, `virement_auto`, `montant_budget`, `lien_operation_id`) VALUES
	(4, '2017-10-09 07:06:06', '2017-10-09 18:57:34', 'super u', 2, 'CB', '2017-10-09 07:06:20', '2017-10-15 18:57:34', NULL, NULL, 0, 0, 0.00, NULL),
	(5, '2017-10-09 07:22:00', NULL, 'garage', 2, 'CB', '2017-10-09 07:22:11', '2017-10-09 07:22:11', NULL, NULL, 0, 0, 0.00, NULL),
	(6, '2017-11-08 07:13:25', NULL, 'test cheque', 3, 'CHQ', '2017-11-08 07:13:43', '2017-11-08 07:13:43', NULL, '0123456', 0, 0, 0.00, NULL),
	(8, '2017-11-02 15:37:43', NULL, 'test tri date', 3, 'CB', '2017-11-22 15:37:44', '2019-03-17 08:03:18', '2017-11-02 08:03:13', NULL, 0, 0, 0.00, NULL),
	(9, '2021-10-17 09:18:41', NULL, 'super u', 3, 'CB', '2021-10-17 09:18:52', '2021-10-17 09:45:38', NULL, NULL, 0, 0, 0.00, NULL);
/*!40000 ALTER TABLE `operations` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.predefinis : ~2 rows (environ)
/*!40000 ALTER TABLE `predefinis` DISABLE KEYS */;
INSERT INTO `predefinis` (`id`, `ordre`, `type_paiement`, `commentaire`, `rubrique_id`, `sousrubrique_id`, `created_at`, `updated_at`) VALUES
	(1, 'Super U', 'CB', 'course semaine', 3, 4, '2021-10-17 15:40:06', '2021-10-17 15:40:06'),
	(2, 'Intermarché', 'CB', 'courses inter', 3, 4, '2021-10-17 15:51:08', '2021-10-17 15:51:08');
/*!40000 ALTER TABLE `predefinis` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. roles
CREATE TABLE IF NOT EXISTS `roles` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.roles : ~0 rows (environ)
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. roles_utilisateurs
CREATE TABLE IF NOT EXISTS `roles_utilisateurs` (
  `role_id` int(11) DEFAULT NULL,
  `utilisateur_id` int(11) DEFAULT NULL,
  KEY `index_roles_utilisateurs_on_role_id` (`role_id`),
  KEY `index_roles_utilisateurs_on_utilisateur_id` (`utilisateur_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.roles_utilisateurs : ~0 rows (environ)
/*!40000 ALTER TABLE `roles_utilisateurs` DISABLE KEYS */;
/*!40000 ALTER TABLE `roles_utilisateurs` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. rubriques
CREATE TABLE IF NOT EXISTS `rubriques` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `designation` varchar(30) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.rubriques : ~5 rows (environ)
/*!40000 ALTER TABLE `rubriques` DISABLE KEYS */;
INSERT INTO `rubriques` (`id`, `designation`, `created_at`, `updated_at`) VALUES
	(1, 'rubrique1', '2017-09-06 15:59:41', '2017-09-06 15:59:41'),
	(2, 'rubrique 2', '2017-10-08 08:52:04', '2017-10-08 08:52:04'),
	(3, 'dépenses courantes', '2017-10-09 07:07:04', '2017-10-09 07:07:04'),
	(4, 'voiture', '2017-10-09 07:18:45', '2017-10-09 07:18:45'),
	(5, 'energie', '2021-10-17 10:18:05', '2021-10-17 10:18:05');
/*!40000 ALTER TABLE `rubriques` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. schema_info
CREATE TABLE IF NOT EXISTS `schema_info` (
  `version` int(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.schema_info : 0 rows
/*!40000 ALTER TABLE `schema_info` DISABLE KEYS */;
/*!40000 ALTER TABLE `schema_info` ENABLE KEYS */;

-- Listage de la structure de la table macompta_test. sousrubriques
CREATE TABLE IF NOT EXISTS `sousrubriques` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `designation` varchar(30) DEFAULT NULL,
  `rubrique_id` int(11) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- Listage des données de la table macompta_test.sousrubriques : ~10 rows (environ)
/*!40000 ALTER TABLE `sousrubriques` DISABLE KEYS */;
INSERT INTO `sousrubriques` (`id`, `designation`, `rubrique_id`, `created_at`, `updated_at`) VALUES
	(1, 'ss rub 1', 1, '2017-09-06 15:59:53', '2017-09-06 15:59:53'),
	(2, 'ssrub 2_1', 2, '2017-10-08 08:57:37', '2017-10-08 08:57:37'),
	(3, 'ssrub 2_2', 2, '2017-10-08 08:57:44', '2017-10-08 08:57:44'),
	(4, 'courses', 3, '2017-10-09 07:18:34', '2017-10-09 07:18:34'),
	(5, 'timbres', 3, '2017-10-09 07:18:39', '2017-10-09 07:18:39'),
	(6, 'essence', 4, '2017-10-09 07:18:49', '2017-10-09 07:18:49'),
	(7, 'entretien', 4, '2017-10-09 07:18:59', '2017-10-09 07:18:59'),
	(8, 'electricité', 5, '2021-10-17 10:18:16', '2021-10-17 10:18:16'),
	(9, 'gaz', 5, '2021-10-17 10:18:17', '2021-10-17 10:18:17'),
	(10, 'assurance', 4, '2021-10-17 10:18:37', '2021-10-17 10:18:37');
/*!40000 ALTER TABLE `sousrubriques` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.stats : ~0 rows (environ)
/*!40000 ALTER TABLE `stats` DISABLE KEYS */;
/*!40000 ALTER TABLE `stats` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.utilisateurs : ~0 rows (environ)
/*!40000 ALTER TABLE `utilisateurs` DISABLE KEYS */;
/*!40000 ALTER TABLE `utilisateurs` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.virements : ~4 rows (environ)
/*!40000 ALTER TABLE `virements` DISABLE KEYS */;
INSERT INTO `virements` (`id`, `duree`, `jour`, `compte_src_id`, `compte_dst_id`, `ordre`, `date_dernier`, `created_at`, `updated_at`, `description`, `frequence`, `montant`, `type_paiement`) VALUES
	(1, 0, 10, 3, 0, 'ordrevirement', '2017-11-09', '2017-11-25 19:27:22', '2017-11-25 19:27:22', 'virement 1', 1, 0.00, 'VRT'),
	(2, 0, 0, 0, 2, 'test 1', '2018-03-01', '2018-03-10 09:07:20', '2020-06-07 15:02:49', 'virement 2', 0, 0.00, 'PLV'),
	(3, -1, 0, 2, 3, 'test 2', NULL, '2018-04-14 11:41:40', '2018-04-14 11:41:40', 'virement 3', 1, 150.00, 'VRT'),
	(4, -1, 5, 2, 0, 'toto', '2020-06-07', '2020-06-07 15:13:59', '2021-04-14 18:01:27', 'test modif', 1, 10.00, 'PLV');
/*!40000 ALTER TABLE `virements` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.virement_details : ~12 rows (environ)
/*!40000 ALTER TABLE `virement_details` DISABLE KEYS */;
INSERT INTO `virement_details` (`id`, `montant`, `commentaire`, `virement_id`, `rubrique_id`, `sousrubrique_id`, `created_at`, `updated_at`, `compte_src_only`, `compte_dst_only`) VALUES
	(1, NULL, 'sdgsdgsdg', 1, 3, 4, '2017-11-25 19:27:35', '2018-03-10 09:06:43', 0, 0),
	(2, NULL, 'fhfghgfh', 1, 2, 2, '2018-03-10 09:06:43', '2018-03-10 09:06:43', 0, 0),
	(3, NULL, 'fghgfh', 1, 1, 1, '2018-03-10 09:06:43', '2018-03-10 09:06:43', 0, 0),
	(4, NULL, 'gfhgf', 1, 2, 3, '2018-03-10 09:06:43', '2018-03-10 09:06:43', 0, 0),
	(5, NULL, 'gfhfgh', 1, 3, 5, '2018-03-10 09:06:43', '2018-03-10 09:06:43', 0, 0),
	(6, NULL, 'gfhgfh', 1, 3, 4, '2018-03-10 09:06:43', '2018-03-10 09:06:43', 0, 0),
	(7, NULL, 'xdgsdg', 1, 4, 7, '2018-03-10 09:06:43', '2018-03-10 09:06:43', 0, 0),
	(8, NULL, 'sdgsdg', 1, 4, 6, '2018-03-10 09:06:43', '2018-03-10 09:06:43', 0, 0),
	(9, NULL, 'dfsdf', 2, 3, 4, '2018-03-10 09:07:30', '2018-03-10 09:07:30', 0, 0),
	(10, NULL, 'ddd', 3, 3, 4, '2018-04-14 11:41:48', '2018-04-14 11:41:48', 0, 0),
	(11, NULL, 'ghghgh', 2, 2, 2, '2019-09-04 14:15:46', '2019-09-04 14:15:46', 0, 0),
	(12, NULL, 'qsdfqsdf', 4, 2, 3, '2020-06-07 15:14:07', '2021-04-14 18:01:28', 0, 0);
/*!40000 ALTER TABLE `virement_details` ENABLE KEYS */;

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

-- Listage des données de la table macompta_test.virement_montants : ~4 rows (environ)
/*!40000 ALTER TABLE `virement_montants` DISABLE KEYS */;
INSERT INTO `virement_montants` (`id`, `montant`, `mois`, `virementdetail_id`, `created_at`, `updated_at`) VALUES
	(1, 10.00, 1, 1, '2017-11-25 19:28:01', '2017-11-25 19:28:01'),
	(2, 20.00, 2, 1, '2017-11-25 19:28:01', '2017-11-25 19:28:01'),
	(3, 10.00, 3, 1, '2017-11-25 19:28:01', '2017-11-25 19:28:01'),
	(4, 0.00, 1, 2, '2018-03-10 09:11:32', '2018-03-10 09:11:32');
/*!40000 ALTER TABLE `virement_montants` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
