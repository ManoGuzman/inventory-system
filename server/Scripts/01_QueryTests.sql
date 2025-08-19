-- InventorySystem Database Test Scripts
-- Run these scripts against the SQL Server database to test functionality

-- 1. View all products with their current stock
SELECT
    p.Id,
    p.Code,
    p.Name,
    p.Category,
    p.Quantity AS CurrentStock,
    p.Location,
    p.RegistrationDate,
    COUNT(m.Id) AS TotalMovements
FROM Products p
    LEFT JOIN Movements m ON p.Id = m.ProductId
GROUP BY p.Id, p.Code, p.Name, p.Category, p.Quantity, p.Location, p.RegistrationDate
ORDER BY p.Name;

-- 2. View all movements with product details
SELECT
    m.Id,
    p.Code AS ProductCode,
    p.Name AS ProductName,
    m.MovementType,
    m.Quantity,
    m.Date,
    p.Quantity AS CurrentStock
FROM Movements m
    INNER JOIN Products p ON m.ProductId = p.Id
ORDER BY m.Date DESC;

-- 3. Stock movement summary by product
SELECT
    p.Code,
    p.Name,
    SUM(CASE WHEN m.MovementType = 'In' THEN m.Quantity ELSE 0 END) AS TotalStockIn,
    SUM(CASE WHEN m.MovementType = 'Out' THEN m.Quantity ELSE 0 END) AS TotalStockOut,
    p.Quantity AS CurrentStock
FROM Products p
    LEFT JOIN Movements m ON p.Id = m.ProductId
GROUP BY p.Id, p.Code, p.Name, p.Quantity
ORDER BY p.Name;

-- 4. Products by category with stock levels
SELECT
    Category,
    COUNT(*) AS ProductCount,
    SUM(Quantity) AS TotalStock,
    AVG(CAST(Quantity AS FLOAT)) AS AverageStock
FROM Products
GROUP BY Category
ORDER BY Category;

-- 5. Recent movements (last 30 days)
SELECT
    p.Code,
    p.Name,
    m.MovementType,
    m.Quantity,
    m.Date
FROM Movements m
    INNER JOIN Products p ON m.ProductId = p.Id
WHERE m.Date >= DATEADD(DAY, -30, GETDATE())
ORDER BY m.Date DESC;

-- 6. Low stock alert (products with quantity < 10)
SELECT
    Code,
    Name,
    Category,
    Quantity AS LowStock,
    Location
FROM Products
WHERE Quantity < 10
ORDER BY Quantity ASC;

-- 7. Users and their roles
SELECT
    Id,
    Username,
    Role,
    CreatedAt,
    LastLoginAt,
    IsActive
FROM Users
ORDER BY Username;
