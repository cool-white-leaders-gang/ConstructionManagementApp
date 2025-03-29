-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 29, 2025 at 08:45 PM
-- Wersja serwera: 10.4.32-MariaDB
-- Wersja PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `construction_management`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `budgets`
--

CREATE TABLE `budgets` (
  `Id` int(11) NOT NULL,
  `TotalAmount` decimal(10,2) NOT NULL,
  `SpentAmount` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `equipment`
--

CREATE TABLE `equipment` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Status` enum('Available','InUse','UnderMaintenance','Broken') NOT NULL,
  `ProjectId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `issues`
--

CREATE TABLE `issues` (
  `Id` int(11) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Content` text NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `CreatedByUserId` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL,
  `Priority` enum('Low','Medium','High') NOT NULL,
  `Status` enum('Open','InProgress','Resolved') NOT NULL,
  `ResolvedAt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `materials`
--

CREATE TABLE `materials` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Quantity` int(11) NOT NULL,
  `Unit` varchar(50) NOT NULL,
  `ProjectId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `progressreports`
--

CREATE TABLE `progressreports` (
  `Id` int(11) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Content` text NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `CreatedByUserId` int(11) NOT NULL,
  `ProjectId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `projects`
--

CREATE TABLE `projects` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Description` text NOT NULL,
  `TeamId` int(11) DEFAULT NULL,
  `BudgetId` int(11) DEFAULT NULL,
  `ClientId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `taskassignments`
--

CREATE TABLE `taskassignments` (
  `TaskId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `tasks`
--

CREATE TABLE `tasks` (
  `Id` int(11) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Description` text NOT NULL,
  `Priority` enum('Low','Medium','High') NOT NULL,
  `Progress` enum('New','InProgress','Completed') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `teammembers`
--

CREATE TABLE `teammembers` (
  `TeamId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `teams`
--

CREATE TABLE `teams` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `ManagerId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `Username` varchar(30) NOT NULL,
  `Email` varchar(80) NOT NULL,
  `PasswordHash` char(64) NOT NULL,
  `Role` enum('Admin','Manager','Worker','Client') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `budgets`
--
ALTER TABLE `budgets`
  ADD PRIMARY KEY (`Id`);

--
-- Indeksy dla tabeli `equipment`
--
ALTER TABLE `equipment`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ProjectId` (`ProjectId`);

--
-- Indeksy dla tabeli `issues`
--
ALTER TABLE `issues`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `CreatedByUserId` (`CreatedByUserId`),
  ADD KEY `ProjectId` (`ProjectId`);

--
-- Indeksy dla tabeli `materials`
--
ALTER TABLE `materials`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ProjectId` (`ProjectId`);

--
-- Indeksy dla tabeli `progressreports`
--
ALTER TABLE `progressreports`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `CreatedByUserId` (`CreatedByUserId`),
  ADD KEY `ProjectId` (`ProjectId`);

--
-- Indeksy dla tabeli `projects`
--
ALTER TABLE `projects`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `TeamId` (`TeamId`),
  ADD KEY `BudgetId` (`BudgetId`),
  ADD KEY `ClientId` (`ClientId`);

--
-- Indeksy dla tabeli `taskassignments`
--
ALTER TABLE `taskassignments`
  ADD PRIMARY KEY (`TaskId`,`UserId`),
  ADD KEY `UserId` (`UserId`);

--
-- Indeksy dla tabeli `tasks`
--
ALTER TABLE `tasks`
  ADD PRIMARY KEY (`Id`);

--
-- Indeksy dla tabeli `teammembers`
--
ALTER TABLE `teammembers`
  ADD PRIMARY KEY (`TeamId`,`UserId`),
  ADD KEY `UserId` (`UserId`);

--
-- Indeksy dla tabeli `teams`
--
ALTER TABLE `teams`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ManagerId` (`ManagerId`);

--
-- Indeksy dla tabeli `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `budgets`
--
ALTER TABLE `budgets`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `equipment`
--
ALTER TABLE `equipment`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `issues`
--
ALTER TABLE `issues`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `materials`
--
ALTER TABLE `materials`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `progressreports`
--
ALTER TABLE `progressreports`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `projects`
--
ALTER TABLE `projects`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tasks`
--
ALTER TABLE `tasks`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `teams`
--
ALTER TABLE `teams`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `equipment`
--
ALTER TABLE `equipment`
  ADD CONSTRAINT `equipment_ibfk_1` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `issues`
--
ALTER TABLE `issues`
  ADD CONSTRAINT `issues_ibfk_1` FOREIGN KEY (`CreatedByUserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `issues_ibfk_2` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `materials`
--
ALTER TABLE `materials`
  ADD CONSTRAINT `materials_ibfk_1` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `progressreports`
--
ALTER TABLE `progressreports`
  ADD CONSTRAINT `progressreports_ibfk_1` FOREIGN KEY (`CreatedByUserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `progressreports_ibfk_2` FOREIGN KEY (`ProjectId`) REFERENCES `projects` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `projects`
--
ALTER TABLE `projects`
  ADD CONSTRAINT `projects_ibfk_1` FOREIGN KEY (`TeamId`) REFERENCES `teams` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `projects_ibfk_2` FOREIGN KEY (`BudgetId`) REFERENCES `budgets` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `projects_ibfk_3` FOREIGN KEY (`ClientId`) REFERENCES `users` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `taskassignments`
--
ALTER TABLE `taskassignments`
  ADD CONSTRAINT `taskassignments_ibfk_1` FOREIGN KEY (`TaskId`) REFERENCES `tasks` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `taskassignments_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `teammembers`
--
ALTER TABLE `teammembers`
  ADD CONSTRAINT `teammembers_ibfk_1` FOREIGN KEY (`TeamId`) REFERENCES `teams` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `teammembers_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `teams`
--
ALTER TABLE `teams`
  ADD CONSTRAINT `teams_ibfk_1` FOREIGN KEY (`ManagerId`) REFERENCES `users` (`Id`) ON DELETE SET NULL ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
