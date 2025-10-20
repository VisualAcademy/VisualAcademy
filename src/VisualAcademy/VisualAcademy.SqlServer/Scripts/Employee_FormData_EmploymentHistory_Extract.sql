-- 특정 Employee의 employment_history 펼치기
SELECT e.ID AS EmployeeID, j.*
FROM dbo.Employees AS e
CROSS APPLY OPENJSON(e.FormData, '$.employment_history')
WITH
(
    employer                    NVARCHAR(255)   '$.employer',
    title                       NVARCHAR(255)   '$.title',
    future_title                NVARCHAR(255)   '$.future_title',
    address                     NVARCHAR(MAX)   '$.address',
    city                        NVARCHAR(255)   '$.city',
    state                       NVARCHAR(50)    '$.state',
    country                     NVARCHAR(255)   '$.country',
    postal_code                 NVARCHAR(50)    '$.postal_code',

    phone_number                NVARCHAR(50)    '$.phone_number',

    supervisor_name             NVARCHAR(255)   '$.supervisor_name',
    supervisor_phone            NVARCHAR(50)    '$.supervisor_phone',
    supervisor_email            NVARCHAR(255)   '$.supervisor_email',
    supervisor_title            NVARCHAR(255)   '$.supervisor_title',

    start_date                  NVARCHAR(50)    '$.start_date',
    end_date                    NVARCHAR(50)    '$.end_date',

    duties                      NVARCHAR(MAX)   '$.duties',
    future_duties               NVARCHAR(MAX)   '$.future_duties',
    reason_for_leaving          NVARCHAR(MAX)   '$.reason_for_leaving',

    gaming_related              NVARCHAR(255)   '$.gaming_related',
    gaming_details              NVARCHAR(MAX)   '$.gaming_details',

    has_corrective_action       NVARCHAR(255)   '$.has_corrective_action',
    corrective_action_details   NVARCHAR(MAX)   '$.corrective_action_details',

    do_not_contact              bit             '$.do_not_contact',

    gaps                        NVARCHAR(MAX)   '$.gaps',
    employment_category         NVARCHAR(255)   '$.employment_category',
    [type]                      NVARCHAR(255)   '$.type',
    coworkers                   NVARCHAR(MAX)   '$.coworkers',
    names_used                  NVARCHAR(MAX)   '$.names_used',

    fax_number                  NVARCHAR(50)    '$.fax_number',

    salary                      NVARCHAR(50)    '$.salary',
    hours                       NVARCHAR(50)    '$.hours',

    different_last_name         NVARCHAR(255)   '$.different_last_name',
    problem_date                NVARCHAR(50)    '$.problem_date',
    problem_explanation         NVARCHAR(MAX)   '$.problem_explanation'
) AS j
WHERE e.ID = 170003;  -- 필요 시 파라미터로
