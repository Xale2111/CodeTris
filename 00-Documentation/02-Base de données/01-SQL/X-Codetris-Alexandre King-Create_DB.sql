-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Hôte : localhost
-- Généré le : mer. 22 mai 2024 à 06:46
-- Version du serveur :  5.7.11
-- Version de PHP : 8.0.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `db_codetris`
--
DROP DATABASE IF EXISTS db_codetris;
CREATE DATABASE db_codetris;
USE db_codetris;
-- --------------------------------------------------------

--
-- Structure de la table `t_difficulty`
--

CREATE TABLE `t_difficulty` (
  `idDifficulty` int(11) NOT NULL,
  `name` varchar(25) NOT NULL,
  `level` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `t_game`
--

CREATE TABLE `t_game` (
  `idGame` int(11) NOT NULL,
  `score` int(10) NOT NULL,
  `gameDate` date NOT NULL,
  `fkUser` int(11) NOT NULL,
  `fkDifficulty` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `t_question`
--

CREATE TABLE `t_question` (
  `idQuestion` int(11) NOT NULL,
  `question` varchar(250) NOT NULL,
  `answer` varchar(250) NOT NULL,
  `creationDate` date NOT NULL,
  `fkDifficulty` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `t_user`
--

CREATE TABLE `t_user` (
  `idUser` int(11) NOT NULL,
  `nickname` varchar(50) NOT NULL,
  `creationDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `t_difficulty`
--
ALTER TABLE `t_difficulty`
  ADD PRIMARY KEY (`idDifficulty`);

--
-- Index pour la table `t_game`
--
ALTER TABLE `t_game`
  ADD PRIMARY KEY (`idGame`),
  ADD KEY `fkDifficulty` (`fkDifficulty`),
  ADD KEY `fkUser` (`fkUser`);

--
-- Index pour la table `t_question`
--
ALTER TABLE `t_question`
  ADD PRIMARY KEY (`idQuestion`),
  ADD KEY `fkDifficulty` (`fkDifficulty`);

--
-- Index pour la table `t_user`
--
ALTER TABLE `t_user`
  ADD PRIMARY KEY (`idUser`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `t_difficulty`
--
ALTER TABLE `t_difficulty`
  MODIFY `idDifficulty` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `t_game`
--
ALTER TABLE `t_game`
  MODIFY `idGame` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `t_question`
--
ALTER TABLE `t_question`
  MODIFY `idQuestion` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT pour la table `t_user`
--
ALTER TABLE `t_user`
  MODIFY `idUser` int(11) NOT NULL AUTO_INCREMENT;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `t_game`
--
ALTER TABLE `t_game`
  ADD CONSTRAINT `t_game_ibfk_1` FOREIGN KEY (`fkDifficulty`) REFERENCES `t_difficulty` (`idDifficulty`),
  ADD CONSTRAINT `t_game_ibfk_2` FOREIGN KEY (`fkUser`) REFERENCES `t_user` (`idUser`);

--
-- Contraintes pour la table `t_question`
--
ALTER TABLE `t_question`
  ADD CONSTRAINT `t_question_ibfk_1` FOREIGN KEY (`fkDifficulty`) REFERENCES `t_difficulty` (`idDifficulty`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
