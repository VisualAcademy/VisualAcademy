-- Tenants 테이블: 테넌트 정보를 저장하는 테이블
CREATE TABLE dbo.Tenants (
    Id bigint IDENTITY(1,1) NOT NULL PRIMARY KEY CLUSTERED, -- 테이블의 기본 키 열로 ID 컬럼을 생성하며, IDENTITY(1,1)로 설정하여 자동 증가 값을 지정합니다.
    [Name] nvarchar(max), -- 테넌트 이름을 저장하는 nvarchar(max) 데이터 타입의 컬럼입니다.

    ConnectionString nvarchar(max), -- 연결 문자열을 저장하는 nvarchar(max) 데이터 타입의 컬럼입니다.
    AuthenticationHeader nvarchar(max), -- 인증 헤더를 저장하는 nvarchar(max) 데이터 타입의 컬럼입니다.
    AccountID nvarchar(max), -- 계정 ID를 저장하는 nvarchar(max) 데이터 타입의 컬럼입니다.
    GSConnectionString nvarchar(max), -- Global Search 연결 문자열을 저장하는 nvarchar(max) 데이터 타입의 컬럼입니다.
    ReportWriterURL nvarchar(max), -- 보고서 작성기 URL을 저장하는 nvarchar(max) 데이터 타입의 컬럼입니다.
    BadgePhotoType nvarchar(50), -- 뱃지 사진 유형을 저장하는 nvarchar(50) 데이터 타입의 컬럼입니다.
    PortalName nvarchar(max) CONSTRAINT DF_Tenants_PortalName DEFAULT ('VisualAcademy') -- 테넌트 포털 이름을 저장하는 nvarchar(max) 데이터 타입의 컬럼입니다. 기본값으로 'VisualAcademy'을 지정합니다.
);
