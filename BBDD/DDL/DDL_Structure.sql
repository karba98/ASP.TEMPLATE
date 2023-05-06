-- phpMyAdmin SQL Dump
-- version 5.1.3
-- https://www.phpmyadmin.net/
--
-- Servidor: mysql5046.site4now.net
-- Tiempo de generación: 06-05-2023 a las 04:33:37
-- Versión del servidor: 5.7.33-log
-- Versión de PHP: 7.4.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `db_a7e01e_vyp`
--
CREATE DATABASE IF NOT EXISTS `db_a7e01e_vyp` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `db_a7e01e_vyp`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `codes`
--

DROP TABLE IF EXISTS `codes`;
CREATE TABLE IF NOT EXISTS `codes` (
  `Key` longtext NOT NULL,
  `Code` longtext NOT NULL,
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleo`
--

DROP TABLE IF EXISTS `empleo`;
CREATE TABLE IF NOT EXISTS `empleo` (
  `id` int(11) NOT NULL,
  `titulo` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `descripcion` longtext CHARACTER SET utf8,
  `salario` int(11) DEFAULT NULL,
  `url` varchar(999) CHARACTER SET utf8 DEFAULT NULL,
  `telefono` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `email` varchar(400) CHARACTER SET utf8 DEFAULT NULL,
  `fechapub` date DEFAULT NULL,
  `provincia` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `categoria` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `fechastring` varchar(99) CHARACTER SET utf8 DEFAULT NULL,
  `publicado` int(11) DEFAULT NULL,
  `modo` varchar(3) CHARACTER SET utf8 DEFAULT 'B',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleo_br`
--

DROP TABLE IF EXISTS `empleo_br`;
CREATE TABLE IF NOT EXISTS `empleo_br` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `titulo` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `descripcion` longtext CHARACTER SET utf8,
  `salario` int(11) DEFAULT NULL,
  `url` varchar(999) CHARACTER SET utf8 DEFAULT NULL,
  `telefono` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `email` varchar(400) CHARACTER SET utf8 DEFAULT NULL,
  `fechapub` date DEFAULT NULL,
  `provincia` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `categoria` varchar(255) CHARACTER SET utf8 DEFAULT NULL,
  `fechastring` varchar(99) CHARACTER SET utf8 DEFAULT NULL,
  `publicado` int(11) DEFAULT NULL,
  `modo` varchar(3) CHARACTER SET utf8 DEFAULT 'B',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empleo_filter`
--

DROP TABLE IF EXISTS `empleo_filter`;
CREATE TABLE IF NOT EXISTS `empleo_filter` (
  `words` varchar(500) NOT NULL,
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `empresas`
--

DROP TABLE IF EXISTS `empresas`;
CREATE TABLE IF NOT EXISTS `empresas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(700) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `files`
--

DROP TABLE IF EXISTS `files`;
CREATE TABLE IF NOT EXISTS `files` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FileName` varchar(255) NOT NULL,
  `FilePath` varchar(900) NOT NULL,
  `Descripcion` varchar(900) DEFAULT NULL,
  `Img` varchar(900) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL,
  `username` varchar(100) DEFAULT NULL,
  `password` blob,
  `salt` varchar(900) CHARACTER SET utf8 DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
