-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Hôte : localhost
-- Généré le : mer. 22 mai 2024 à 06:48
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
USE `db_codetris`;

--
-- Déchargement des données de la table `t_difficulty`
--

INSERT INTO `t_difficulty` (`idDifficulty`, `name`, `level`) VALUES
(1, 'Facile', 0),
(2, 'Moyen', 1),
(3, 'Difficile', 2);

--
-- Déchargement des données de la table `t_question`
--

INSERT INTO `t_question` (`idQuestion`, `question`, `answer`, `creationDate`, `fkDifficulty`) VALUES
(1, 'Quel est la valeur maximum d\'un byte ?', '255', '2024-05-01', 1),
(2, 'Quel est la valeur minimum d\'un byte ?', '0', '2024-05-01', 1),
(3, 'Comment écrire une ligne en C# console ?', 'console.writeline()', '2024-05-02', 1),
(4, 'Comment créer une liste de charactères avec comme nom \"toto\" ?', 'List<char> toto = new List<char>()', '2024-05-02', 2),
(5, 'Quelle nomenclature faut-il utiliser pour les noms de méthode ? ', 'lower camelcase', '2024-05-02', 2),
(6, 'quel est le nom de la classe par défaut d\'un programme C# ?', 'program.cs', '2024-05-02', 2),
(7, 'Qu\'est ce que la POO ?', 'programmation orientée objet', '2024-05-02', 3),
(8, 'Quel est le raccourci clavier pour créer une nouvelle classe ?', 'Ctrl + shift + A', '2024-05-02', 3),
(9, 'Quel diminutif peut être utilisé pour écrire Console.WriteLine() ?', 'cw', '2024-05-02', 3);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
