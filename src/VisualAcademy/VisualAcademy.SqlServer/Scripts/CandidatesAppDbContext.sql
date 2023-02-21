IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Candidates] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [IsEnrollment] bit NOT NULL,
    CONSTRAINT [PK_Candidates] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220829005101_CandidateModelAdd', N'7.0.0-preview.7.22376.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Candidates] ADD [Address] nvarchar(70) NULL;
GO

ALTER TABLE [Candidates] ADD [BirthCity] nvarchar(70) NULL;
GO

ALTER TABLE [Candidates] ADD [BirthCountry] nvarchar(70) NULL;
GO

ALTER TABLE [Candidates] ADD [BirthState] nvarchar(2) NULL;
GO

ALTER TABLE [Candidates] ADD [City] nvarchar(70) NULL;
GO

ALTER TABLE [Candidates] ADD [DOB] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [DriverLicenseNumber] nvarchar(35) NULL;
GO

ALTER TABLE [Candidates] ADD [DriverLicenseState] nvarchar(2) NULL;
GO

ALTER TABLE [Candidates] ADD [Email] nvarchar(254) NULL;
GO

ALTER TABLE [Candidates] ADD [Gender] nvarchar(35) NULL;
GO

ALTER TABLE [Candidates] ADD [LicenseNumber] nvarchar(35) NULL;
GO

ALTER TABLE [Candidates] ADD [MiddleName] nvarchar(35) NULL;
GO

ALTER TABLE [Candidates] ADD [Photo] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [PostalCode] nvarchar(35) NULL;
GO

ALTER TABLE [Candidates] ADD [PrimaryPhone] nvarchar(35) NULL;
GO

ALTER TABLE [Candidates] ADD [SSN] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [SecondaryPhone] nvarchar(35) NULL;
GO

ALTER TABLE [Candidates] ADD [State] nvarchar(2) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220830113207_CandidateColumnAdd', N'7.0.0-preview.7.22376.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Candidates] ADD [Age] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Candidates] ADD [AliasNames] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [BirthCounty] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [BirthPlace] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [BusinessStructure] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [BusinessStructureOther] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [County] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [DriverLicenseExpiration] datetime2 NULL;
GO

ALTER TABLE [Candidates] ADD [EyeColor] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [HairColor] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [Height] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [HeightFeet] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [HeightInches] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [HomePhone] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [MaritalStatus] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [MobilePhone] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [NameSuffix] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [OfficeAddress] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [OfficeCity] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [OfficeState] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [PhysicalMarks] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [UsCitizen] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [Weight] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [WorkFax] nvarchar(max) NULL;
GO

ALTER TABLE [Candidates] ADD [WorkPhone] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220901004602_AddColumn', N'7.0.0-preview.7.22376.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Candidates] ADD [ConcurrencyToken] rowversion NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220906103956_RowVersion', N'7.0.0-preview.7.22376.2');
GO

COMMIT;
GO

