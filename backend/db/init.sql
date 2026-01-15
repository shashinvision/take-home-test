/*
 Navicat Premium Data Transfer

 Source Server         : Docker SQL Server
 Source Server Type    : SQL Server
 Source Server Version : 16004135 (16.00.4135)
 Source Host           : localhost:1433
 Source Catalog        : loandb
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 16004135 (16.00.4135)
 File Encoding         : 65001

 Date: 15/01/2026 18:43:01
*/


-- ----------------------------
-- Table structure for APPLICANT
-- ----------------------------
--
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'loandb')
BEGIN
    CREATE DATABASE loandb;
END
GO

USE loandb;
GO
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[APPLICANT]') AND type IN ('U'))
	DROP TABLE [dbo].[APPLICANT]
GO

CREATE TABLE [dbo].[APPLICANT] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [FULL_NAME] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [DNI] varchar(12) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[APPLICANT] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for LOAN
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[LOAN]') AND type IN ('U'))
	DROP TABLE [dbo].[LOAN]
GO

CREATE TABLE [dbo].[LOAN] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [AMOUNT] decimal(18)  NULL,
  [CURRENT_BALANCE] decimal(18)  NULL,
  [IS_ACTIVE] int  NULL,
  [CREATED_AT] datetime  NULL,
  [UPDATE_AT] datetime  NULL,
  [ID_APPLICANT] int  NULL
)
GO

ALTER TABLE [dbo].[LOAN] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for PAYMENT
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[PAYMENT]') AND type IN ('U'))
	DROP TABLE [dbo].[PAYMENT]
GO

CREATE TABLE [dbo].[PAYMENT] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [AMOUNT] decimal(18)  NULL,
  [ID_LOAN] int  NULL,
  [CREATED_AT] datetime  NULL
)
GO

ALTER TABLE [dbo].[PAYMENT] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for USER
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[USER]') AND type IN ('U'))
	DROP TABLE [dbo].[USER]
GO

CREATE TABLE [dbo].[USER] (
  [ID] int  IDENTITY(1,1) NOT NULL,
  [EMAIL] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [FULL_NAME] varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [PASSWORD_HASH] varchar(255) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [CREATED_AT] datetime  NULL,
  [IS_ACTIVE] int  NULL
)
GO

ALTER TABLE [dbo].[USER] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Auto increment value for APPLICANT
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[APPLICANT]', RESEED, 24)
GO


-- ----------------------------
-- Primary Key structure for table APPLICANT
-- ----------------------------
ALTER TABLE [dbo].[APPLICANT] ADD CONSTRAINT [PK__APPLICAN__3214EC27001816FF] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for LOAN
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[LOAN]', RESEED, 45)
GO


-- ----------------------------
-- Primary Key structure for table LOAN
-- ----------------------------
ALTER TABLE [dbo].[LOAN] ADD CONSTRAINT [PK__Loan__3214EC2762009076] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for PAYMENT
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[PAYMENT]', RESEED, 83)
GO


-- ----------------------------
-- Primary Key structure for table PAYMENT
-- ----------------------------
ALTER TABLE [dbo].[PAYMENT] ADD CONSTRAINT [PK__PAYMENT__3214EC27DEF8199B] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Auto increment value for USER
-- ----------------------------
DBCC CHECKIDENT ('[dbo].[USER]', RESEED, 3)
GO


-- ----------------------------
-- Uniques structure for table USER
-- ----------------------------
ALTER TABLE [dbo].[USER] ADD CONSTRAINT [UK_EMAIL] UNIQUE NONCLUSTERED ([EMAIL] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table USER
-- ----------------------------
ALTER TABLE [dbo].[USER] ADD CONSTRAINT [PK__USER__3214EC27FE454F02] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table LOAN
-- ----------------------------
ALTER TABLE [dbo].[LOAN] ADD CONSTRAINT [LOAN_APPLICANT] FOREIGN KEY ([ID_APPLICANT]) REFERENCES [dbo].[APPLICANT] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table PAYMENT
-- ----------------------------
ALTER TABLE [dbo].[PAYMENT] ADD CONSTRAINT [FK_LOAN_PAYMENT] FOREIGN KEY ([ID_LOAN]) REFERENCES [dbo].[LOAN] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Insert initial admin user (if not exists)
-- ----------------------------
IF NOT EXISTS (SELECT 1 FROM [USER] WHERE EMAIL = 'admin@admin.com')
BEGIN
    INSERT INTO [USER] (EMAIL, FULL_NAME, PASSWORD_HASH, CREATED_AT, IS_ACTIVE)
    VALUES (
        'admin@admin.com',
        'Admin',
        '$2a$11$VRCL./etYp6kF0eVdeQAiufrC2Jq67wJtbXLBhhsmEIaiSBkBS/w2',
        GETUTCDATE(),
        1
    )
    PRINT 'Admin user created: admin@admin.com'
END
ELSE
BEGIN
    PRINT 'Admin user already exists'
END
GO
