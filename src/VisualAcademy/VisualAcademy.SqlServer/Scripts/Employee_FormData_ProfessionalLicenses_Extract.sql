-- 특정 Employee의 전문 면허(professional_licenses) 펼치기
SELECT e.ID AS EmployeeID, p.*
FROM dbo.Employees AS e
CROSS APPLY OPENJSON(e.FormData, '$.professional_licenses')
WITH
(
    name                                NVARCHAR(255)   '$.name',
    [type]                              NVARCHAR(255)   '$.type',                -- Application Type
    license_type                        NVARCHAR(255)   '$.license_type',
    license_number                      NVARCHAR(255)   '$.license_number',

    app_date                            NVARCHAR(50)    '$.app_date',
    issued_date                         NVARCHAR(50)    '$.issued_date',
    expire_date                         NVARCHAR(50)    '$.expire_date',
    expiration_date                     NVARCHAR(50)    '$.expiration_date',

    status                              NVARCHAR(255)   '$.status',
    issuing_agency                      NVARCHAR(255)   '$.issuing_agency',
    address                             NVARCHAR(MAX)   '$.address',
    city                                NVARCHAR(255)   '$.city',
    state                               NVARCHAR(50)    '$.state',
    county                              NVARCHAR(255)   '$.county',
    postal_code                         NVARCHAR(50)    '$.postal_code',
    country                             NVARCHAR(255)   '$.country',

    adverse_license                     NVARCHAR(50)    '$.adverse_license',         -- 문자열로 수집
    action_description                  NVARCHAR(MAX)   '$.action_description',
    action_date                         NVARCHAR(50)    '$.action_date',
    action_disposition                  NVARCHAR(255)   '$.action_disposition',

    tribe                               NVARCHAR(255)   '$.tribe',
    explanation                         NVARCHAR(MAX)   '$.explanation',
    from_date                           NVARCHAR(50)    '$.from_date',
    to_date                             NVARCHAR(50)    '$.to_date',

    agency_phone                        NVARCHAR(50)    '$.agency_phone',
    agency_fax                          NVARCHAR(50)    '$.agency_fax',
    share_record_explanation            NVARCHAR(MAX)   '$.share_record_explanation',
    adverse_license_actions_explanation NVARCHAR(MAX)   '$.adverse_license_actions_explanation',
    alcohol_license_application         NVARCHAR(50)    '$.alcohol_license_application',
    gaming_license_explanation          NVARCHAR(MAX)   '$.gaming_license_explanation',

    phone                               NVARCHAR(50)    '$.phone',
    approved_or_denied                  NVARCHAR(50)    '$.approved_or_denied'
) AS p
WHERE e.ID = 170003;  -- 필요 시 변수로 교체
