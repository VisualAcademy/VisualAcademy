CREATE TABLE [dbo].[TextMessages]
(
	[ID] BIGINT NOT NULL PRIMARY KEY IDENTITY,
	[EmployeeID] BIGINT NOT NULL,
	[Message] NVARCHAR(MAX) NOT NULL,
	[DateSent] DateTimeOffset(7) NOT NULL,
	[TextMessageType] int null
)
