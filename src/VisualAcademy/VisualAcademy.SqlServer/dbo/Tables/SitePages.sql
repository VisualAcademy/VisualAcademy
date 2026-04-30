CREATE TABLE [dbo].[SitePages]
(
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [RoutePattern] NVARCHAR(300) NOT NULL,
    [HttpMethod] NVARCHAR(50),
    [DisplayName] NVARCHAR(300),
    [PageTitle] NVARCHAR(200),
    [PageNumber] INT,
    [SortOrder] INT DEFAULT 0,
    [IsPublic] BIT DEFAULT 1,
    [IsVisibleInDashboard] BIT DEFAULT 1,
    [RequiredRoles] NVARCHAR(500),
    [RequiredPolicy] NVARCHAR(200),
    [AllowAnonymous] BIT DEFAULT 0,
    [IsEndpointActive] BIT DEFAULT 1,
    [LastSyncedAtUtc] DATETIME2,
    [CreatedAtUtc] DATETIME2 DEFAULT SYSUTCDATETIME(),
    [UpdatedAtUtc] DATETIME2
);

--CREATE UNIQUE INDEX UX_SitePages_RoutePattern_HttpMethod
--ON SitePages(RoutePattern, HttpMethod);
