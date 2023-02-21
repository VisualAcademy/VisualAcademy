CREATE TABLE [dbo].[CandidatesNames] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]  NVARCHAR (50)  NOT NULL,
    [LastName]   NVARCHAR (50)  NOT NULL,
    [MiddleName] NVARCHAR (50)  NULL,
    [UserId]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CandidatesNames] PRIMARY KEY CLUSTERED ([Id] ASC)
);

