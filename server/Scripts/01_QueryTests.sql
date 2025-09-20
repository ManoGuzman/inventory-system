-- Query Tests for Inventory Management System
-- This script contains verification and testing queries

-- Test 1: Verify database structure
SELECT
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_CATALOG = 'InventorySystemDb'
ORDER BY TABLE_NAME;

-- Test 2: Check user accounts
SELECT
    Id,
    Username,
    Role,
    CreatedAt,
    IsActive
FROM Users
ORDER BY CreatedAt;

-- Test 3: Verify products
SELECT
    Id,
    Code,
    Name,
    Category,
    Quantity,
    Location,
    RegistrationDate
FROM Products
ORDER BY RegistrationDate;

-- Test 4: Check inventory movements
SELECT
    m.Id,
    p.Name AS ProductName,
    m.MovementType,
    m.Quantity,
    m.Date
FROM Movements m
    JOIN Products p ON m.ProductId = p.Id
ORDER BY m.Date DESC;

-- Test 5: Product quantities verification
SELECT
    p.Id,
    p.Name,
    p.Quantity AS CurrentStock,
    ISNULL(SUM(CASE WHEN m.MovementType = 'In' THEN m.Quantity ELSE 0 END), 0) AS TotalIn,
    ISNULL(SUM(CASE WHEN m.MovementType = 'Out' THEN m.Quantity ELSE 0 END), 0) AS TotalOut,
    ISNULL(SUM(CASE WHEN m.MovementType = 'In' THEN m.Quantity ELSE -m.Quantity END), 0) AS CalculatedStock
FROM Products p
    LEFT JOIN Movements m ON p.Id = m.ProductId
GROUP BY p.Id, p.Name, p.Quantity
ORDER BY p.Name;

-- Test 6: Low stock products (quantity < 10)
SELECT
    Code,
    Name,
    Category,
    Quantity,
    Location
FROM Products
WHERE Quantity < 10
ORDER BY Quantity;

-- Test 7: Recent movements (last 30 days)
SELECT
    p.Name AS ProductName,
    m.MovementType,
    m.Quantity,
    m.Date,
    m.Notes
FROM Movements m
    JOIN Products p ON m.ProductId = p.Id
WHERE m.Date >= DATEADD(day, -30, GETDATE())
ORDER BY m.Date DESC;

-- Test 8: Products by category
SELECT
    Category,
    COUNT(*) AS ProductCount,
    SUM(Quantity) AS TotalQuantity
FROM Products
GROUP BY Category
ORDER BY ProductCount DESC;

-- Test 9: User activity summary
SELECT
    Role,
    COUNT(*) AS UserCount,
    COUNT(CASE WHEN IsActive = 1 THEN 1 END) AS ActiveUsers,
    COUNT(CASE WHEN LastLoginAt IS NOT NULL THEN 1 END) AS UsersWithLogin
FROM Users
GROUP BY Role
ORDER BY Role;

-- Test 10: Data integrity checks
-- Check for orphaned movements
SELECT COUNT(*) AS OrphanedMovements
FROM Movements m
    LEFT JOIN Products p ON m.ProductId = p.Id
WHERE p.Id IS NULL;

-- Check for duplicate product codes
SELECT
    Code,
    COUNT(*) AS DuplicateCount
FROM Products
GROUP BY Code
HAVING COUNT(*) > 1;

-- Performance test queries
-- Test 11: Index usage analysis
SELECT
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.indexes i
    LEFT JOIN sys.dm_db_index_usage_stats s ON i.object_id = s.object_id AND i.index_id = s.index_id
WHERE OBJECT_NAME(i.object_id) IN ('Users', 'Products', 'Movements')
ORDER BY TableName, IndexName;
