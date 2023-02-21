CREATE TABLE [dbo].[CandidatesIncomes] (
    [Id]     INT             IDENTITY (1, 1) NOT NULL,
    [Source] NVARCHAR (50)   NOT NULL,
    [Amount] DECIMAL (18, 2) NULL,
    [UserId] NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_CandidatesIncomes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

