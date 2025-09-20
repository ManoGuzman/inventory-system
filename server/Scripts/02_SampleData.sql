-- Sample Data for Inventory Management System
-- This script populates the database with test data for development and testing

-- Insert sample users (passwords are BCrypt hashed)
-- Note: These are the same users created by the application, this is just for reference

-- Sample Products
INSERT INTO Products
    (Code, Name, Category, Quantity, Location, RegistrationDate)
VALUES
    ('LAPTOP001', 'Business Laptop Dell', 'Electronics', 25, 'Warehouse A', GETDATE()),
    ('LAPTOP002', 'Gaming Laptop ASUS', 'Electronics', 15, 'Warehouse A', GETDATE()),
    ('MOUSE001', 'Wireless Mouse Logitech', 'Accessories', 100, 'Warehouse B', GETDATE()),
    ('KEYBOARD001', 'Mechanical Keyboard', 'Accessories', 50, 'Warehouse B', GETDATE()),
    ('MONITOR001', '24 inch LED Monitor', 'Electronics', 30, 'Warehouse A', GETDATE()),
    ('CHAIR001', 'Office Chair Ergonomic', 'Furniture', 20, 'Warehouse C', GETDATE()),
    ('DESK001', 'Standing Desk', 'Furniture', 10, 'Warehouse C', GETDATE()),
    ('PRINTER001', 'Color Laser Printer', 'Electronics', 5, 'Warehouse A', GETDATE()),
    ('CABLE001', 'HDMI Cable 6ft', 'Accessories', 200, 'Warehouse B', GETDATE()),
    ('SPEAKER001', 'Bluetooth Speaker', 'Electronics', 35, 'Warehouse A', GETDATE()),
    ('TABLET001', 'Android Tablet', 'Electronics', 40, 'Warehouse A', GETDATE()),
    ('PHONE001', 'Business Smartphone', 'Electronics', 60, 'Warehouse A', GETDATE()),
    ('HEADSET001', 'Noise Cancelling Headset', 'Accessories', 75, 'Warehouse B', GETDATE()),
    ('WEBCAM001', 'HD Webcam', 'Accessories', 45, 'Warehouse B', GETDATE()),
    ('ROUTER001', 'Wireless Router', 'Electronics', 20, 'Warehouse A', GETDATE());

-- Sample inventory movements
DECLARE @ProductIds TABLE (Id INT,
    Code NVARCHAR(50));
INSERT INTO @ProductIds
    (Id, Code)
SELECT Id, Code
FROM Products
WHERE Code IN ('LAPTOP001', 'MOUSE001', 'MONITOR001', 'CHAIR001', 'PRINTER001');

-- Stock in movements
INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'In', 100, DATEADD(day, -30, GETDATE()), 'Initial stock purchase'
FROM @ProductIds
WHERE Code = 'LAPTOP001';

INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'In', 200, DATEADD(day, -25, GETDATE()), 'Bulk order received'
FROM @ProductIds
WHERE Code = 'MOUSE001';

INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'In', 50, DATEADD(day, -20, GETDATE()), 'Monthly restock'
FROM @ProductIds
WHERE Code = 'MONITOR001';

-- Stock out movements
INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'Out', 75, DATEADD(day, -15, GETDATE()), 'Sales order SO-001'
FROM @ProductIds
WHERE Code = 'LAPTOP001';

INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'Out', 100, DATEADD(day, -10, GETDATE()), 'Bulk sale to corporate client'
FROM @ProductIds
WHERE Code = 'MOUSE001';

INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'Out', 20, DATEADD(day, -5, GETDATE()), 'Office setup project'
FROM @ProductIds
WHERE Code = 'MONITOR001';

INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'Out', 10, DATEADD(day, -3, GETDATE()), 'Department allocation'
FROM @ProductIds
WHERE Code = 'CHAIR001';

-- Recent movements for testing
INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'In', 25, GETDATE(), 'Emergency restock'
FROM Products
WHERE Code = 'LAPTOP001';

INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
SELECT Id, 'Out', 5, GETDATE(), 'Manager requisition'
FROM Products
WHERE Code = 'PRINTER001';

-- Additional movements for various products
INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date, Notes)
VALUES
    ((SELECT Id
        FROM Products
        WHERE Code = 'CABLE001'), 'In', 500, DATEADD(day, -28, GETDATE()), 'Large shipment received'),
    ((SELECT Id
        FROM Products
        WHERE Code = 'CABLE001'), 'Out', 300, DATEADD(day, -14, GETDATE()), 'Distributed to departments'),
    ((SELECT Id
        FROM Products
        WHERE Code = 'SPEAKER001'), 'In', 100, DATEADD(day, -21, GETDATE()), 'New product line'),
    ((SELECT Id
        FROM Products
        WHERE Code = 'SPEAKER001'), 'Out', 65, DATEADD(day, -7, GETDATE()), 'Conference room setup'),
    ((SELECT Id
        FROM Products
        WHERE Code = 'TABLET001'), 'In', 80, DATEADD(day, -18, GETDATE()), 'Quarterly purchase'),
    ((SELECT Id
        FROM Products
        WHERE Code = 'TABLET001'), 'Out', 40, DATEADD(day, -12, GETDATE()), 'Sales team allocation'),
    ((SELECT Id
        FROM Products
        WHERE Code = 'HEADSET001'), 'In', 150, DATEADD(day, -26, GETDATE()), 'Remote work equipment'),
    ((SELECT Id
        FROM Products
        WHERE Code = 'HEADSET001'), 'Out', 75, DATEADD(day, -8, GETDATE()), 'Employee distribution');

-- Update product quantities to reflect movements
UPDATE p
SET Quantity = ISNULL(movements.NetQuantity, 0)
FROM Products p
    LEFT JOIN (
    SELECT
        ProductId,
        SUM(CASE WHEN MovementType = 'In' THEN Quantity ELSE -Quantity END) AS NetQuantity
    FROM Movements
    GROUP BY ProductId
) movements ON p.Id = movements.ProductId;

-- Verification query
SELECT
    p.Code,
    p.Name,
    p.Quantity,
    ISNULL(SUM(CASE WHEN m.MovementType = 'In' THEN m.Quantity ELSE -m.Quantity END), 0) AS CalculatedQuantity
FROM Products p
    LEFT JOIN Movements m ON p.Id = m.ProductId
GROUP BY p.Id, p.Code, p.Name, p.Quantity
ORDER BY p.Code;
