-- Sample Data Insertion Scripts
-- Use these to add test data to your database

-- Insert additional test products
INSERT INTO Products
    (Code, Name, Category, Quantity, Location, RegistrationDate)
VALUES
    ('COMP001', 'Desktop Computer', 'Electronics', 15, 'Warehouse A - Shelf 2', GETDATE()),
    ('PHONE001', 'Smartphone', 'Electronics', 50, 'Warehouse A - Shelf 3', GETDATE()),
    ('DESK001', 'Standing Desk', 'Furniture', 8, 'Warehouse B - Section 1', GETDATE()),
    ('PEN001', 'Ballpoint Pen (Box)', 'Stationery', 200, 'Storage Room - Cabinet 1', GETDATE()),
    ('MONITOR001', '24" Monitor', 'Electronics', 25, 'Warehouse A - Shelf 4', GETDATE());

-- Insert additional test users
INSERT INTO Users
    (Username, PasswordHash, Role, CreatedAt, LastLoginAt, IsActive)
VALUES
    ('manager1', '$2a$11$UWZcVnBVjZ8ChxMiPO8eqOqGJTKJzJBGSzC/v6D4qKJ8cYH4jOBX.', 'Manager', GETDATE(), GETDATE(), 1),
    ('employee1', '$2a$11$UWZcVnBVjZ8ChxMiPO8eqOqGJTKJzJBGSzC/v6D4qKJ8cYH4jOBX.', 'Employee', GETDATE(), GETDATE(), 1),
    ('employee2', '$2a$11$UWZcVnBVjZ8ChxMiPO8eqOqGJTKJzJBGSzC/v6D4qKJ8cYH4jOBX.', 'Employee', GETDATE(), GETDATE(), 1);

-- Note: All test passwords are "admin123" (hashed with BCrypt)

-- Insert some test movements for the new products
DECLARE @CompId INT = (SELECT Id
FROM Products
WHERE Code = 'COMP001');
DECLARE @PhoneId INT = (SELECT Id
FROM Products
WHERE Code = 'PHONE001');
DECLARE @DeskId INT = (SELECT Id
FROM Products
WHERE Code = 'DESK001');

INSERT INTO Movements
    (ProductId, MovementType, Quantity, Date)
VALUES
    (@CompId, 'In', 20, DATEADD(DAY, -10, GETDATE())),
    (@CompId, 'Out', 5, DATEADD(DAY, -5, GETDATE())),
    (@PhoneId, 'In', 60, DATEADD(DAY, -15, GETDATE())),
    (@PhoneId, 'Out', 10, DATEADD(DAY, -3, GETDATE())),
    (@DeskId, 'In', 10, DATEADD(DAY, -20, GETDATE())),
    (@DeskId, 'Out', 2, DATEADD(DAY, -1, GETDATE()));

-- Update product quantities to match movements
UPDATE Products SET Quantity = 15 WHERE Code = 'COMP001';
-- 20 in - 5 out
UPDATE Products SET Quantity = 50 WHERE Code = 'PHONE001';
-- 60 in - 10 out  
UPDATE Products SET Quantity = 8 WHERE Code = 'DESK001';   -- 10 in - 2 out
