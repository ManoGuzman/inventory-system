-- Database Maintenance and Utility Scripts

-- 1. Reset all test data (use with caution!)
/*
DELETE FROM Movements;
DELETE FROM Products WHERE Code LIKE 'TEST%' OR Code LIKE 'COMP%' OR Code LIKE 'PHONE%' OR Code LIKE 'DESK%' OR Code LIKE 'PEN%' OR Code LIKE 'MONITOR%';
DELETE FROM Users WHERE Username != 'admin';
*/

-- 2. Backup seed data
SELECT 'Products' AS TableName;
SELECT *
FROM Products
WHERE Code IN ('ELEC001', 'FURN001', 'STAT001');

SELECT 'Movements' AS TableName;
SELECT *
FROM Movements
WHERE ProductId IN (SELECT Id
FROM Products
WHERE Code IN ('ELEC001', 'FURN001', 'STAT001'));

SELECT 'Users' AS TableName;
SELECT *
FROM Users
WHERE Username = 'admin';

-- 3. Check data integrity
-- Verify product quantities match movement calculations
WITH
    ProductMovements
    AS
    (
        SELECT
            p.Id,
            p.Code,
            p.Name,
            p.Quantity AS RecordedQuantity,
            COALESCE(SUM(CASE WHEN m.MovementType = 'In' THEN m.Quantity ELSE -m.Quantity END), 0) AS CalculatedQuantity
        FROM Products p
            LEFT JOIN Movements m ON p.Id = m.ProductId
        GROUP BY p.Id, p.Code, p.Name, p.Quantity
    )
SELECT
    Code,
    Name,
    RecordedQuantity,
    CalculatedQuantity,
    CASE 
        WHEN RecordedQuantity = CalculatedQuantity THEN 'OK'
        ELSE 'MISMATCH'
    END AS Status
FROM ProductMovements
ORDER BY Code;

-- 4. Generate inventory report
SELECT
    'Inventory Summary Report - ' + CAST(GETDATE() AS VARCHAR(20)) AS ReportTitle;

SELECT
    Category,
    COUNT(*) AS ProductCount,
    SUM(Quantity) AS TotalItems,
    AVG(CAST(Quantity AS FLOAT)) AS AvgQuantity,
    MIN(Quantity) AS MinQuantity,
    MAX(Quantity) AS MaxQuantity
FROM Products
GROUP BY Category
ORDER BY Category;

-- 5. Create database indexes for better performance (run once)
/*
CREATE INDEX IX_Products_Code ON Products (Code);
CREATE INDEX IX_Products_Category ON Products (Category);
CREATE INDEX IX_Movements_ProductId ON Movements (ProductId);
CREATE INDEX IX_Movements_Date ON Movements (Date);
CREATE INDEX IX_Users_Username ON Users (Username);
*/
