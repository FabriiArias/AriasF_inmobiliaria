-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 22-04-2025 a las 05:07:14
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `arias_inmobiliaria`
--
CREATE DATABASE IF NOT EXISTS `arias_inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `arias_inmobiliaria`;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `id_contrato` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `creado_por` int(11) NOT NULL,
  `anulado_por` int(11) DEFAULT NULL,
  `f_inicio` date NOT NULL,
  `f_fin` date NOT NULL,
  `f_inicio_anulacion` date DEFAULT NULL,
  `f_fin_anulacion` date DEFAULT NULL,
  `monto_mensual` double NOT NULL,
  `estado` enum('Vigente','Finalizado','Anulado','Pendiente_anulacion') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`id_contrato`, `id_inmueble`, `id_inquilino`, `creado_por`, `anulado_por`, `f_inicio`, `f_fin`, `f_inicio_anulacion`, `f_fin_anulacion`, `monto_mensual`, `estado`) VALUES
(2, 1, 9, 1, 1, '2025-04-01', '2025-04-30', '2025-04-21', '2025-04-22', 200000, 'Anulado'),
(3, 2, 1, 1, NULL, '2025-04-05', '2025-04-25', NULL, NULL, 200000, 'Finalizado'),
(6, 8, 7, 1, NULL, '2025-06-01', '2025-06-05', NULL, NULL, 150000, 'Vigente'),
(7, 8, 9, 1, NULL, '2025-08-01', '2025-08-05', NULL, NULL, 150000, 'Vigente'),
(8, 8, 10, 1, NULL, '2025-04-21', '2025-04-22', NULL, NULL, 150000, 'Finalizado'),
(9, 1, 9, 1, NULL, '2025-04-23', '2025-04-25', NULL, NULL, 200000, 'Vigente'),
(10, 2, 1, 1, NULL, '2025-04-22', '2025-04-24', NULL, NULL, 200000, 'Vigente'),
(11, 3, 6, 1, 1, '2025-04-22', '2025-04-23', '2025-04-21', '2025-04-22', 53211.99, 'Anulado'),
(12, 3, 6, 1, NULL, '2025-04-24', '2025-04-25', NULL, NULL, 53211.99, 'Vigente');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagen`
--

CREATE TABLE `imagen` (
  `id` int(11) NOT NULL,
  `url` varchar(1000) NOT NULL,
  `id_inmueble` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `id_inmueble` int(11) NOT NULL,
  `dni_propietario` int(11) NOT NULL,
  `tipo` enum('casa','local','deposito','departamento') NOT NULL,
  `direccion` varchar(250) NOT NULL,
  `ambientes` int(11) NOT NULL,
  `precio` double NOT NULL,
  `estado` enum('disponible','suspendido','ocupado','') NOT NULL,
  `uso` enum('comercial','residencial','otro') NOT NULL,
  `latitud` varchar(250) NOT NULL,
  `longitud` varchar(250) NOT NULL,
  `portada` varchar(500) NOT NULL,
  `activo` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`id_inmueble`, `dni_propietario`, `tipo`, `direccion`, `ambientes`, `precio`, `estado`, `uso`, `latitud`, `longitud`, `portada`, `activo`) VALUES
(1, 2, 'casa', 'Calle Falsa 123', 3, 450000, 'disponible', 'residencial', '12345asd', '54321dsa', '/uploads/7e7b60f7-72b3-4b68-b6d1-8a9d4e5f5f4d.jpeg', 1),
(2, 12382, 'casa', 'si', 1, 230000, 'disponible', 'residencial', '123123', '123123', '', 1),
(3, 2, 'local', 'pringles 1123', 2, 53211.99, 'disponible', 'comercial', '1231aasd', '123asda', '/uploads/75b5f3a3-45ab-4c7e-afe4-25ecaafce7f5.jpeg', 1),
(4, 90, 'deposito', 'sl 421', 2, 1500, 'suspendido', 'otro', '1235412', '111111', '', 1),
(5, 12345, 'departamento', 'av sol', 2, 4201, 'disponible', 'residencial', '1235412', '111111', '', 1),
(6, 12345, 'local', 'av los almendros', 2, 270000, 'disponible', 'residencial', '1235412', '111111', '', 1),
(7, 90, 'casa', 'av los almendros', 2, 270000, 'disponible', 'otro', '1235412', '111111', '', 0),
(8, 1923, 'casa', 'jupiter 227, Tilisarao, San Luis', 3, 150000, 'disponible', 'residencial', '1290492', '0919231', '/uploads/c8fb0812-d4b1-4ab5-987b-74cb403b8099.jpeg', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `dni_inquilino` int(11) NOT NULL,
  `nombre` varchar(250) NOT NULL,
  `apellido` varchar(250) NOT NULL,
  `email` varchar(250) NOT NULL,
  `celular` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `activo` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`dni_inquilino`, `nombre`, `apellido`, `email`, `celular`, `id_inquilino`, `activo`) VALUES
(1, 'Alan', 'Alvarez', 'aa@gmaill', 111, 1, 1),
(2, 'Bruno', 'Benitez', 'bb@gmail', 222, 2, 1),
(3, 'carlos', 'caceres', 'cc@gmial', 333, 6, 1),
(45, 'si', 'no ', 'editado@gmail', 2, 7, 1),
(77712, 'Agustinn', 'Fernandez', 'af@gmailcom', 2319221, 9, 1),
(666, 'nuevo', 'activo', 'active@gmail', 9898, 10, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `nro_pago` int(11) NOT NULL,
  `fecha_pago` date NOT NULL,
  `importe` double NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `detalle` varchar(500) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `DNI_propietario` int(11) NOT NULL,
  `nombre` varchar(200) NOT NULL,
  `apellido` varchar(200) NOT NULL,
  `celular` int(11) NOT NULL,
  `mail` varchar(250) NOT NULL,
  `activo` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`DNI_propietario`, `nombre`, `apellido`, `celular`, `mail`, `activo`) VALUES
(1, 'me faltaba un igual', 'no te lo puedo creer', 312, 'nma@gmail.com', 1),
(2, 'Nicolas', 'De la cruz', 23912, 'ndelacrz@hotmail.com', 1),
(90, 'Silvina', 'Simeone', 2921, 'sisi@gmial.com', 1),
(1727, 'nuevo', 'activo', 12312, 'a@g', 0),
(1923, 'Pedro', 'Marchesin', 2938582, 'pmarchesin@gmail.com', 1),
(12345, 'Pablo', 'Moran', 2718283, 'pablom@gmail.com', 1),
(12382, 'Lucas', 'Palacios', 1928432, 'lpala@gmail.com', 1),
(54321, 'Matias', 'Vaez', 3029381, 'matiasv@gmial.com', 0),
(98212, 'Nuevo', 'new', 1, 'n@gmail', 0),
(98765, 'Carlos', 'Vala', 231231, 'valac@gmial.com', 0),
(99912, 'Edu', 'Lopez', 12312312, 'edul@gmial.com', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `id_usuario` int(11) NOT NULL,
  `email` varchar(200) NOT NULL,
  `pass` varchar(200) NOT NULL,
  `rol` enum('empleado','administrador') NOT NULL,
  `avatar` varchar(1000) DEFAULT NULL,
  `nombre` varchar(250) NOT NULL,
  `apellido` varchar(250) NOT NULL,
  `contacto` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id_usuario`, `email`, `pass`, `rol`, `avatar`, `nombre`, `apellido`, `contacto`) VALUES
(1, 'user1@gmail.com', '123', 'administrador', NULL, 'Ricardo', 'Fort', '2664111111');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id_contrato`),
  ADD KEY `id_inmueble` (`id_inmueble`),
  ADD KEY `contrato_ibfk_3` (`id_inquilino`),
  ADD KEY `creado_por` (`creado_por`),
  ADD KEY `anulado_por` (`anulado_por`);

--
-- Indices de la tabla `imagen`
--
ALTER TABLE `imagen`
  ADD PRIMARY KEY (`id`),
  ADD KEY `id_inmueble` (`id_inmueble`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id_inmueble`),
  ADD KEY `dni_propietario` (`dni_propietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id_inquilino`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`nro_pago`),
  ADD KEY `id_contrato` (`id_contrato`),
  ADD KEY `id_usuario` (`id_usuario`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`DNI_propietario`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id_usuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id_contrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `imagen`
--
ALTER TABLE `imagen`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id_inmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id_inquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id_usuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id_inmueble`),
  ADD CONSTRAINT `contrato_ibfk_3` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id_inquilino`),
  ADD CONSTRAINT `contrato_ibfk_4` FOREIGN KEY (`creado_por`) REFERENCES `usuario` (`id_usuario`),
  ADD CONSTRAINT `contrato_ibfk_5` FOREIGN KEY (`anulado_por`) REFERENCES `usuario` (`id_usuario`);

--
-- Filtros para la tabla `imagen`
--
ALTER TABLE `imagen`
  ADD CONSTRAINT `imagen_ibfk_1` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id_inmueble`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`dni_propietario`) REFERENCES `propietario` (`DNI_propietario`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`id_contrato`) REFERENCES `contrato` (`id_contrato`),
  ADD CONSTRAINT `pago_ibfk_2` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
