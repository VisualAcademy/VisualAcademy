CREATE TABLE [dbo].[Noodles] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (25) NOT NULL,
    [BrothId] INT           NOT NULL,
    CONSTRAINT [PK_Noodles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Noodles_Broths_BrothId] FOREIGN KEY ([BrothId]) REFERENCES [dbo].[Broths] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Noodles_BrothId]
    ON [dbo].[Noodles]([BrothId] ASC);

