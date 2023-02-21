CREATE TABLE [dbo].[Garnishes] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (25) NOT NULL,
    [NoodleId] INT           NULL,
    CONSTRAINT [PK_Garnishes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Garnishes_Noodles_NoodleId] FOREIGN KEY ([NoodleId]) REFERENCES [dbo].[Noodles] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Garnishes_NoodleId]
    ON [dbo].[Garnishes]([NoodleId] ASC);

