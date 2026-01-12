CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                   NVARCHAR (450)     NOT NULL,
    [UserName]             NVARCHAR (256)     NULL,
    [NormalizedUserName]   NVARCHAR (256)     NULL,
    [Email]                NVARCHAR (256)     NULL,
    [NormalizedEmail]      NVARCHAR (256)     NULL,
    [EmailConfirmed]       BIT                NOT NULL,
    [PasswordHash]         NVARCHAR (MAX)     NULL,
    [SecurityStamp]        NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX)     NULL,
    [PhoneNumber]          NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed] BIT                NOT NULL,
    [TwoFactorEnabled]     BIT                NOT NULL,
    [LockoutEnd]           DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]       BIT                NOT NULL,
    [AccessFailedCount]    INT                NOT NULL,

    [Address]              NVARCHAR (MAX)     NULL,
    [FirstName]            NVARCHAR (MAX)     NULL,
    [Gender]               NVARCHAR (MAX)     NULL,
    [LastName]             NVARCHAR (MAX)     NULL,
    [TenantId]             BIGINT             DEFAULT (CONVERT([bigint],(0))) NOT NULL,

    [TenantName]               NVARCHAR (MAX)     Default('VisualAcademy'),

    RegistrationDate DATETIMEOFFSET  NULL DEFAULT (SYSDATETIMEOFFSET()),

    [Timezone]             NVARCHAR (MAX)     NULL,

    -- Change of Information (정보 변경 사유 및 내역)
    [MaritalStatus] NVARCHAR(50) NULL, -- 혼인 상태
    [NewEmail] NVARCHAR(254) NULL, -- 변경 요청된 새 이메일
    [BadgeName] NVARCHAR(255) NULL, -- 배지에 표시할 이름
    [ReasonForChange] NVARCHAR(MAX) NULL, -- 정보 변경 사유
    [SpousesName] NVARCHAR(255) NULL, -- 배우자 이름
    [RoommateName1] NVARCHAR(255) NULL, -- 동거인 이름 1
    [RoommateName2] NVARCHAR(255) NULL, -- 동거인 이름 2
    [RelationshipDisclosureName] NVARCHAR(255) NULL, -- 관계 공개 대상 이름
    [RelationshipDisclosurePosition] NVARCHAR(255) NULL, -- 관계 공개 대상 직위
    [RelationshipDisclosure] NVARCHAR(MAX) NULL, -- 관계 공개 내용
    [AdditionalEmploymentBusinessName] NVARCHAR(255) NULL, -- 추가 근무 사업장명
    [AdditionalEmploymentStartDate] DATETIME2(7) NULL, -- 추가 근무 시작일
    [AdditionalEmploymentEndDate] DATETIME2(7) NULL, -- 추가 근무 종료일
    [AdditionalEmploymentLocation] NVARCHAR(255) NULL, -- 추가 근무 장소


    -- Profile Picture(PFP, Persona Avatar)
    [ProfilePicture]          VARBINARY(MAX)    NULL,

    -- Signature image (user handwritten signature, JPG binary)
    [SignatureImage] VARBINARY(MAX) NULL,

    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [EmailIndex]
    ON [dbo].[AspNetUsers]([NormalizedEmail] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers]([NormalizedUserName] ASC) WHERE ([NormalizedUserName] IS NOT NULL);
