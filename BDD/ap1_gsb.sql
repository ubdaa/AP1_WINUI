-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : mar. 28 mai 2024 à 09:38
-- Version du serveur : 10.4.28-MariaDB
-- Version de PHP : 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `ap1_gsb`
--

-- --------------------------------------------------------

--
-- Structure de la table `etat`
--

CREATE TABLE `etat` (
  `id_etat` int(11) NOT NULL,
  `type_etat` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `fiche_de_frais`
--

CREATE TABLE `fiche_de_frais` (
  `id_fiche` int(11) NOT NULL,
  `date_fiche` date NOT NULL,
  `utilisateur` int(11) NOT NULL,
  `etat` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `forfait`
--

CREATE TABLE `forfait` (
  `id_forfait` int(11) NOT NULL,
  `quantite` int(11) NOT NULL,
  `date` date NOT NULL,
  `etat` enum('ATTENTE','ACCEPTE','REFUSE') NOT NULL,
  `type_forfait` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `hors_forfait`
--

CREATE TABLE `hors_forfait` (
  `id_hors_forfait` int(11) NOT NULL,
  `nom` text NOT NULL,
  `date` date NOT NULL,
  `cout` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `justificatif`
--

CREATE TABLE `justificatif` (
  `id_justificatif` int(11) NOT NULL,
  `chemin` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `justificatif_forfait`
--

CREATE TABLE `justificatif_forfait` (
  `id_justificatif` int(11) NOT NULL,
  `id_forfait` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `justificatif_hors_forfait`
--

CREATE TABLE `justificatif_hors_forfait` (
  `id_justificatif` int(11) NOT NULL,
  `id_hors_forfait` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `role`
--

CREATE TABLE `role` (
  `id_role` int(11) NOT NULL,
  `role` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `type_frais`
--

CREATE TABLE `type_frais` (
  `id_type_forfait` int(11) NOT NULL,
  `nom` text NOT NULL,
  `cout` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Structure de la table `utilisateur`
--

CREATE TABLE `utilisateur` (
  `id_utilisateur` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `mdp` varchar(50) NOT NULL,
  `id_role` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `etat`
--
ALTER TABLE `etat`
  ADD PRIMARY KEY (`id_etat`);

--
-- Index pour la table `fiche_de_frais`
--
ALTER TABLE `fiche_de_frais`
  ADD PRIMARY KEY (`id_fiche`),
  ADD KEY `utilisateur` (`utilisateur`),
  ADD KEY `etat` (`etat`);

--
-- Index pour la table `forfait`
--
ALTER TABLE `forfait`
  ADD PRIMARY KEY (`id_forfait`),
  ADD KEY `type_forfait` (`type_forfait`);

--
-- Index pour la table `hors_forfait`
--
ALTER TABLE `hors_forfait`
  ADD PRIMARY KEY (`id_hors_forfait`);

--
-- Index pour la table `justificatif`
--
ALTER TABLE `justificatif`
  ADD PRIMARY KEY (`id_justificatif`);

--
-- Index pour la table `justificatif_forfait`
--
ALTER TABLE `justificatif_forfait`
  ADD KEY `id_justificatif` (`id_justificatif`),
  ADD KEY `id_forfait` (`id_forfait`);

--
-- Index pour la table `justificatif_hors_forfait`
--
ALTER TABLE `justificatif_hors_forfait`
  ADD KEY `id_justificatif` (`id_justificatif`),
  ADD KEY `id_hors_forfait` (`id_hors_forfait`);

--
-- Index pour la table `role`
--
ALTER TABLE `role`
  ADD PRIMARY KEY (`id_role`);

--
-- Index pour la table `type_frais`
--
ALTER TABLE `type_frais`
  ADD PRIMARY KEY (`id_type_forfait`);

--
-- Index pour la table `utilisateur`
--
ALTER TABLE `utilisateur`
  ADD PRIMARY KEY (`id_utilisateur`),
  ADD KEY `id_role` (`id_role`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `etat`
--
ALTER TABLE `etat`
  MODIFY `id_etat` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `fiche_de_frais`
--
ALTER TABLE `fiche_de_frais`
  MODIFY `id_fiche` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `forfait`
--
ALTER TABLE `forfait`
  MODIFY `id_forfait` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `hors_forfait`
--
ALTER TABLE `hors_forfait`
  MODIFY `id_hors_forfait` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `justificatif`
--
ALTER TABLE `justificatif`
  MODIFY `id_justificatif` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `role`
--
ALTER TABLE `role`
  MODIFY `id_role` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `type_frais`
--
ALTER TABLE `type_frais`
  MODIFY `id_type_forfait` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `utilisateur`
--
ALTER TABLE `utilisateur`
  MODIFY `id_utilisateur` int(11) NOT NULL AUTO_INCREMENT;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `fiche_de_frais`
--
ALTER TABLE `fiche_de_frais`
  ADD CONSTRAINT `fiche_de_frais_ibfk_1` FOREIGN KEY (`utilisateur`) REFERENCES `utilisateur` (`id_utilisateur`),
  ADD CONSTRAINT `fiche_de_frais_ibfk_2` FOREIGN KEY (`etat`) REFERENCES `etat` (`id_etat`);

--
-- Contraintes pour la table `forfait`
--
ALTER TABLE `forfait`
  ADD CONSTRAINT `forfait_ibfk_1` FOREIGN KEY (`type_forfait`) REFERENCES `type_frais` (`id_type_forfait`);

--
-- Contraintes pour la table `justificatif_forfait`
--
ALTER TABLE `justificatif_forfait`
  ADD CONSTRAINT `justificatif_forfait_ibfk_1` FOREIGN KEY (`id_justificatif`) REFERENCES `justificatif` (`id_justificatif`),
  ADD CONSTRAINT `justificatif_forfait_ibfk_2` FOREIGN KEY (`id_forfait`) REFERENCES `forfait` (`id_forfait`);

--
-- Contraintes pour la table `justificatif_hors_forfait`
--
ALTER TABLE `justificatif_hors_forfait`
  ADD CONSTRAINT `justificatif_hors_forfait_ibfk_1` FOREIGN KEY (`id_justificatif`) REFERENCES `justificatif` (`id_justificatif`),
  ADD CONSTRAINT `justificatif_hors_forfait_ibfk_2` FOREIGN KEY (`id_hors_forfait`) REFERENCES `hors_forfait` (`id_hors_forfait`);

--
-- Contraintes pour la table `utilisateur`
--
ALTER TABLE `utilisateur`
  ADD CONSTRAINT `utilisateur_ibfk_1` FOREIGN KEY (`id_role`) REFERENCES `role` (`id_role`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
