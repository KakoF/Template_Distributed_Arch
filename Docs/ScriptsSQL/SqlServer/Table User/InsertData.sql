use PerformRetriveData

DECLARE @Counter INT = 1;

WHILE @Counter <= 20000
BEGIN
    INSERT INTO [dbo].[User] (Name)
    VALUES (
        CONCAT(
            CHAR(65 + ROUND(RAND() * 25, 0)), -- Primeira letra maiúscula (A-Z)
            LOWER(CONCAT(
                CHAR(65 + ROUND(RAND() * 25, 0)), -- Segunda letra (a-z)
                CHAR(65 + ROUND(RAND() * 25, 0)), -- Terceira letra (a-z)
                CHAR(65 + ROUND(RAND() * 25, 0)), -- Quarta letra (a-z)
                CHAR(65 + ROUND(RAND() * 25, 0)), -- Quinta letra (a-z)
                CHAR(65 + ROUND(RAND() * 25, 0))  -- Sexta letra (a-z)
            ))
        )
    );

SET @Counter = @Counter + 1;

END;