CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `AboutUs` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NOT NULL,
        CONSTRAINT `PK_AboutUs` PRIMARY KEY (`Id`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `ContactUs` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `FirstName` longtext CHARACTER SET utf8mb4 NULL,
        `LastName` longtext CHARACTER SET utf8mb4 NULL,
        `Email` longtext CHARACTER SET utf8mb4 NULL,
        `Tel` longtext CHARACTER SET utf8mb4 NULL,
        `Title` longtext CHARACTER SET utf8mb4 NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NULL,
        CONSTRAINT `PK_ContactUs` PRIMARY KEY (`Id`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `ProjectState` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `Title` longtext CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NULL,
        CONSTRAINT `PK_ProjectState` PRIMARY KEY (`Id`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `Role` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(256) CHARACTER SET utf8mb4 NULL,
        `NormalizedName` varchar(256) CHARACTER SET utf8mb4 NULL,
        `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Role` PRIMARY KEY (`Id`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `Tag` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `Size` int NOT NULL,
        `Title` varchar(25) CHARACTER SET utf8mb4 NOT NULL,
        `Description` varchar(256) CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Tag` PRIMARY KEY (`Id`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `User` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserName` varchar(256) CHARACTER SET utf8mb4 NULL,
        `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 NULL,
        `Email` varchar(256) CHARACTER SET utf8mb4 NULL,
        `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 NULL,
        `EmailConfirmed` tinyint(1) NOT NULL,
        `PasswordHash` longtext CHARACTER SET utf8mb4 NULL,
        `SecurityStamp` longtext CHARACTER SET utf8mb4 NULL,
        `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 NULL,
        `PhoneNumber` longtext CHARACTER SET utf8mb4 NULL,
        `PhoneNumberConfirmed` tinyint(1) NOT NULL,
        `TwoFactorEnabled` tinyint(1) NOT NULL,
        `LockoutEnd` datetime(6) NULL,
        `LockoutEnabled` tinyint(1) NOT NULL,
        `AccessFailedCount` int NOT NULL,
        `FirstName` varchar(450) CHARACTER SET utf8mb4 NOT NULL,
        `LastName` varchar(450) CHARACTER SET utf8mb4 NOT NULL,
        `PhotoFileName` varchar(450) CHARACTER SET utf8mb4 NULL,
        `BirthDate` datetime(6) NULL,
        `IsEmailPublic` tinyint(1) NOT NULL,
        `Location` longtext CHARACTER SET utf8mb4 NULL,
        `IpAddress` longtext CHARACTER SET utf8mb4 NULL,
        `LoginProvider` longtext CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NOT NULL,
        CONSTRAINT `PK_User` PRIMARY KEY (`Id`)
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `RoleClaim` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `RoleId` int NOT NULL,
        `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
        `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_RoleClaim` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_RoleClaim_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `Article` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `UserId` int NULL,
        `Title` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
        `Summary` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        `DescriptionAsPlainText` longtext CHARACTER SET utf8mb4 NULL,
        `RateCounter` decimal(65,30) NULL,
        `LikeCounter` int NULL,
        `IsActive` tinyint(1) NOT NULL,
        `IsPublished` tinyint(1) NOT NULL,
        `IsActiveNewComment` tinyint(1) NOT NULL,
        `TitleHtmlMetaTag` varchar(70) CHARACTER SET utf8mb4 NULL,
        `DescriptionHtmlMetaTag` varchar(300) CHARACTER SET utf8mb4 NULL,
        `KeywordsHtmlMetaTag` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_Article` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Article_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE RESTRICT
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `Project` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `UserId` int NULL,
        `ProjectStateId` int NULL,
        `TagId` int NULL,
        `Title` longtext CHARACTER SET utf8mb4 NULL,
        `Summary` longtext CHARACTER SET utf8mb4 NULL,
        `Description` longtext CHARACTER SET utf8mb4 NULL,
        `CompanyName` longtext CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NULL,
        `StartDate` datetime(6) NULL,
        `FinishDate` datetime(6) NULL,
        CONSTRAINT `PK_Project` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Project_ProjectState_ProjectStateId` FOREIGN KEY (`ProjectStateId`) REFERENCES `ProjectState` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Project_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE RESTRICT
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `UserClaim` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `ClaimType` longtext CHARACTER SET utf8mb4 NULL,
        `ClaimValue` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_UserClaim` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_UserClaim_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `UserLogin` (
        `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `ProviderKey` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `ProviderDisplayName` longtext CHARACTER SET utf8mb4 NULL,
        `UserId` int NOT NULL,
        CONSTRAINT `PK_UserLogin` PRIMARY KEY (`LoginProvider`, `ProviderKey`),
        CONSTRAINT `FK_UserLogin_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `UserRole` (
        `UserId` int NOT NULL,
        `RoleId` int NOT NULL,
        CONSTRAINT `PK_UserRole` PRIMARY KEY (`UserId`, `RoleId`),
        CONSTRAINT `FK_UserRole_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_UserRole_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `UserToken` (
        `UserId` int NOT NULL,
        `LoginProvider` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `Value` longtext CHARACTER SET utf8mb4 NULL,
        CONSTRAINT `PK_UserToken` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
        CONSTRAINT `FK_UserToken_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `ArticleLike` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `ArticleId` int NULL,
        `UserId` int NULL,
        `IsLiked` tinyint(1) NULL,
        CONSTRAINT `PK_ArticleLike` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_ArticleLike_Article_ArticleId` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_ArticleLike_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE RESTRICT
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `ArticleTag` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `TagId` int NOT NULL,
        `ArticleId` int NOT NULL,
        `Extra` int NULL,
        CONSTRAINT `PK_ArticleTag` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_ArticleTag_Article_ArticleId` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_ArticleTag_Tag_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tag` (`Id`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `Comment` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `ParentId` int NULL,
        `ArticleId` int NULL,
        `UserId` int NULL,
        `Description` varchar(1024) CHARACTER SET utf8mb4 NULL,
        `LikeCounter` int NULL,
        CONSTRAINT `PK_Comment` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Comment_Article_ArticleId` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Comment_Comment_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `Comment` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Comment_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE RESTRICT
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `Rating` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `UserId` int NULL,
        `ArticleId` int NULL,
        `IsLock` tinyint(1) NULL,
        `IsActive` tinyint(1) NULL,
        `Rate` decimal(65,30) NULL,
        CONSTRAINT `PK_Rating` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Rating_Article_ArticleId` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Rating_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE RESTRICT
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `AttachmentFile` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `ArticleId` int NULL,
        `ProjectId` int NULL,
        `Name` varchar(512) CHARACTER SET utf8mb4 NULL,
        `Size` bigint NULL,
        `Extension` varchar(10) CHARACTER SET utf8mb4 NULL,
        `Path` varchar(1024) CHARACTER SET utf8mb4 NULL,
        `IsActive` tinyint(1) NULL,
        CONSTRAINT `PK_AttachmentFile` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AttachmentFile_Article_ArticleId` FOREIGN KEY (`ArticleId`) REFERENCES `Article` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_AttachmentFile_Project_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Project` (`Id`) ON DELETE RESTRICT
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `ProjectTag` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `TagId` int NOT NULL,
        `ProjectId` int NOT NULL,
        `Extra` int NULL,
        CONSTRAINT `PK_ProjectTag` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_ProjectTag_Project_ProjectId` FOREIGN KEY (`ProjectId`) REFERENCES `Project` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_ProjectTag_Tag_TagId` FOREIGN KEY (`TagId`) REFERENCES `Tag` (`Id`) ON DELETE CASCADE
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE TABLE `CommentLike` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `CreatedDate` datetime(6) NULL,
        `ModifiedDate` datetime(6) NULL,
        `CommentId` int NULL,
        `UserId` int NULL,
        `IsLiked` tinyint(1) NULL,
        CONSTRAINT `PK_CommentLike` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_CommentLike_Comment_CommentId` FOREIGN KEY (`CommentId`) REFERENCES `Comment` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_CommentLike_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE RESTRICT
    );

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Article_UserId` ON `Article` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_ArticleLike_ArticleId` ON `ArticleLike` (`ArticleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_ArticleLike_UserId` ON `ArticleLike` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_ArticleTag_ArticleId` ON `ArticleTag` (`ArticleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_ArticleTag_TagId` ON `ArticleTag` (`TagId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_AttachmentFile_ArticleId` ON `AttachmentFile` (`ArticleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_AttachmentFile_ProjectId` ON `AttachmentFile` (`ProjectId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Comment_ArticleId` ON `Comment` (`ArticleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Comment_ParentId` ON `Comment` (`ParentId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Comment_UserId` ON `Comment` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_CommentLike_CommentId` ON `CommentLike` (`CommentId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_CommentLike_UserId` ON `CommentLike` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Project_ProjectStateId` ON `Project` (`ProjectStateId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Project_UserId` ON `Project` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_ProjectTag_ProjectId` ON `ProjectTag` (`ProjectId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_ProjectTag_TagId` ON `ProjectTag` (`TagId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Rating_ArticleId` ON `Rating` (`ArticleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_Rating_UserId` ON `Rating` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE UNIQUE INDEX `RoleNameIndex` ON `Role` (`NormalizedName`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_RoleClaim_RoleId` ON `RoleClaim` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `EmailIndex` ON `User` (`NormalizedEmail`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE UNIQUE INDEX `UserNameIndex` ON `User` (`NormalizedUserName`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_UserClaim_UserId` ON `UserClaim` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_UserLogin_UserId` ON `UserLogin` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    CREATE INDEX `IX_UserRole_RoleId` ON `UserRole` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;


DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20200520115254_MySql_Init') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20200520115254_MySql_Init', '3.1.3');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

