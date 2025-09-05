Select 
SUBSTRING(UserText, PATINDEX('%OP_%', UserText) + 3, 4) ÒP,
UserText
FROM OALR
WHERE UserSign = 1
AND ([Subject] LIKE 'SKA x SAP%' OR [Subject] LIKE 'SAP x SKA%') 
AND UserText LIKE '%OP_%'