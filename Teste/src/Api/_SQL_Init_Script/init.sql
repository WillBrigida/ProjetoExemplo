-- Active: 1696561972145@@172.19.0.2@3306@CompetDB
CREATE DATABASE TesteDB;
USE TesteDB;



CREATE TABLE IF NOT EXISTS AspNetRoles (
  `Id` varchar(128) NOT NULL,
  `Name` varchar(256) NOT NULL,
  `NormalizedName` varchar(256)  NULL,
  `ConcurrencyStamp` longtext NULL,
  constraint PK_AspNetRoles PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS AspNetUserClaims (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(128) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  constraint PK_AspNetUserClaims PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  KEY `UserId` (`UserId`)) 
  ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS TrocaMateriasPorPontos (
  `TrocaMateriasPorPontosId` int(11) NOT NULL AUTO_INCREMENT,
  `MaterialId` int(11) NOT NULL,
  constraint PK_TrocaMateriasPorPontosId PRIMARY KEY (`TrocaMateriasPorPontosId`))
  ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1;

  CREATE TABLE IF NOT EXISTS TrocaPontosPorProdutos (
  `TrocaPontosPorProdutosId` int(11) NOT NULL AUTO_INCREMENT,
  `ProdutoId` int(11) NOT NULL,
  constraint PK_TrocaPontosPorProdutosId PRIMARY KEY (`TrocaPontosPorProdutosId`))
  ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS AspNetUserLogins (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext,
  `UserId` varchar(128) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
  KEY `ApplicationUser_Logins` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS AspNetUserRoles (
  `UserId` varchar(128) NOT NULL,
  `RoleId` varchar(128) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IdentityRole_Users` (`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS AspNetUsers (
  `Id` varchar(128) NOT NULL,
  `ConcurrencyStamp` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `PhoneNumber` varchar(256) NOT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEndDateUtc` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  `FullName` varchar(256) NOT NULL,
  `LockoutEnd` datetime DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

CREATE TABLE IF NOT EXISTS AspNetUserTokens (
`UserId` varchar(128) NOT NULL,
`LoginProvider` varchar(127) NOT NULL,
Name varchar(127) NOT NULL,
Value longtext,
CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId, LoginProvider, Name)
);

ALTER TABLE AspNetUserClaims
ADD CONSTRAINT `ApplicationUser_Claims` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE AspNetUserLogins
ADD CONSTRAINT `ApplicationUser_Logins` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION;

ALTER TABLE AspNetUserRoles
ADD CONSTRAINT `ApplicationUser_Roles` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
ADD CONSTRAINT `IdentityRole_Users` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION;

INSERT INTO AspNetRoles(Id,ConcurrencyStamp,Name,NormalizedName) 
 VALUES( '97bf2353-56bc-4917-90e5-1a3468459257', '1924ef29-465d-4674-bd62-4ec3423d5c9b ', 'Admin', 'ADMIN');

 INSERT INTO AspNetRoles(Id,ConcurrencyStamp,Name,NormalizedName) 
 VALUES( '962022a2-26fd-4d10-9471-2293795c6d44', '19c07450-6328-4c31-b0a9-c4aa303e2cd0', 'User', 'USER');

 INSERT INTO AspNetRoles(Id,ConcurrencyStamp,Name,NormalizedName) 
 VALUES( 'c5549db5-7a45-488a-a03d-2e6608a256e5', '03dab9ac-d6c7-4f4b-96f3-616bd571f93a', 'Employee', 'EMPLOYEE');