-- phpMyAdmin SQL Dump
-- version 5.1.3
-- https://www.phpmyadmin.net/
--
-- Servidor: mysql5046.site4now.net
-- Tiempo de generación: 06-05-2023 a las 04:34:22
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

--
-- Volcado de datos para la tabla `codes`
--

INSERT INTO `codes` (`Key`, `Code`, `Id`) VALUES
('FaceBookUserID', '', 4),
('FacebookApiSecret', '', 5),
('FacebookApiId', '', 6),
('FacebookLongLifeToken', '', 7),
('LinkedIntRefreashToken', '', 10),
('LinkedInClientId', '', 11),
('LinkedInClientSecret', '', 12),
('TwitterApiToken', '', 13),
('TwitterApiSecret', '', 14),
('TwitterAccessToken', '', 15),
('TwitterAccessTokenSecret', '', 16),
('TelegramTokenBot', '', 17),
('TelegramTokenOEPBot', '', 19);

--
-- Volcado de datos para la tabla `empleo`
--

INSERT INTO `empleo` (`id`, `titulo`, `descripcion`, `salario`, `url`, `telefono`, `email`, `fechapub`, `provincia`, `categoria`, `fechastring`, `publicado`, `modo`) VALUES
(1, 'Vigilante de Seguridad', '\n\n<ul class=\"list-default\">\n<li>\n<h3 class=\"list-default-title\">Número de vacantes</h3>\n<p><span id=\"prefijoVacantes\" class=\"list-default-text\">1</span></p></li>\n<li>\n<h3 class=\"list-default-title\">Horario</h3>\n<p><span class=\"list-default-text\">De L-D 6 a 14 hs o 14 a 22 hs</span></p></li>\n</ul>\n<p><a href=\"https://www.infojobs.net/barcelona/vigilante-seguridad-hotel/of-i2440039b5745bebecb966cf4c1c553?navOrigen=android&navOrigen=caInscExt%7Candroid&navOrigen=caAltaInsc%7Candroid\" target=\"_blank\" rel=\"noopener\">infojobs-aqui.</a></p>\n<p> </p>\n<p> </p>\n<p> </p>\n<p> </p>\n', 0, 'https://www.infojobs.net/barcelona/vigilante-seguridad-hotel/of-i2440039b5745bebecb966cf4c1c553?navOrigen=android&navOrigen=caInscExt%7Candroid&navOrigen=caAltaInsc%7Candroid', '', '', '2021-01-20', 'Barcelona', 'infojobs', '20/01/2021 08:00:02 AM', 0, 'B'),

--
-- Volcado de datos para la tabla `empleo_filter`
--

INSERT INTO `empleo_filter` (`words`, `Id`) VALUES
('palabra que no quiero en mi oferta', 1);

--
-- Volcado de datos para la tabla `empresas`
--

INSERT INTO `empresas` (`Id`, `Nombre`) VALUES
(1, 'EMPRESA SL');


/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
