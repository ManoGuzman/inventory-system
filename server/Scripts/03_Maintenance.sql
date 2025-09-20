-- Maintenance Scripts for Inventory Management System
-- These scripts handle database maintenance, cleanup, and optimization

-- 1. Data Cleanup Procedures

-- Remove old movement records (older than 2 years)
DELETE FROM Movements 
WHERE Date < DATEADD(year, -2, GETDATE());

-- Clean up inactive users (not logged in for 1 year and not admin)
UPDATE Users 
SET IsActive = 0 
WHERE LastLoginAt < DATEADD(year, -1, GETDATE())
    AND Role != 'Admin';

-- Remove products with zero quantity and no recent movements
DELETE p FROM Products p
    LEFT JOIN Movements m ON p.Id = m.ProductId AND m.Date > DATEADD(month, -6, GETDATE())
WHERE p.Quantity = 0 AND m.Id IS NULL;

-- 2. Index Maintenance

-- Rebuild fragmented indexes
DECLARE @TableName NVARCHAR(128);
DECLARE @IndexName NVARCHAR(128);
DECLARE @SQL NVARCHAR(MAX);

DECLARE index_cursor CURSOR FOR
SELECT
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName
FROM sys.indexes i
    JOIN sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'DETAILED') ps
    ON i.object_id = ps.object_id AND i.index_id = ps.index_id
WHERE ps.avg_fragmentation_in_percent > 30
    AND i.name IS NOT NULL
    AND OBJECT_NAME(i.object_id) IN ('Users', 'Products', 'Movements');

OPEN index_cursor;
FETCH NEXT FROM index_cursor INTO @TableName, @IndexName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = 'ALTER INDEX [' + @IndexName + '] ON [' + @TableName + '] REBUILD;';
    PRINT 'Rebuilding index: ' + @SQL;
    EXEC sp_executesql @SQL;

    FETCH NEXT FROM index_cursor INTO @TableName, @IndexName;
END;

CLOSE index_cursor;
DEALLOCATE index_cursor;

-- 3. Statistics Update
UPDATE STATISTICS Users;
UPDATE STATISTICS Products;
UPDATE STATISTICS Movements;

-- 4. Performance Optimization Queries

-- Check for missing indexes
SELECT
    CONVERT(decimal(18,2), user_seeks * avg_total_user_cost * (avg_user_impact * 0.01)) AS index_advantage,
    migs.last_user_seek,
    mid.statement AS table_name,
    mid.equality_columns,
    mid.inequality_columns,
    mid.included_columns
FROM sys.dm_db_missing_index_group_stats AS migs
    JOIN sys.dm_db_missing_index_groups AS mig ON migs.group_handle = mig.index_group_handle
    JOIN sys.dm_db_missing_index_details AS mid ON mig.index_handle = mid.index_handle
WHERE CONVERT(decimal(18,2), user_seeks * avg_total_user_cost * (avg_user_impact * 0.01)) > 10
ORDER BY index_advantage DESC;

-- 5. Database Size Analysis
SELECT
    name AS 'Database Name',
    CAST((size * 8.0 / 1024) AS DECIMAL(10,2)) AS 'Size (MB)',
    CAST((FILEPROPERTY(name, 'SpaceUsed') * 8.0 / 1024) AS DECIMAL(10,2)) AS 'Used (MB)',
    CAST(((size - FILEPROPERTY(name, 'SpaceUsed')) * 8.0 / 1024) AS DECIMAL(10,2)) AS 'Free (MB)'
FROM sys.database_files;

-- 6. Table Size Analysis
SELECT
    t.NAME AS TableName,
    s.Name AS SchemaName,
    p.rows AS RowCounts,
    CAST(ROUND(((SUM(a.total_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS TotalSpaceMB,
    CAST(ROUND(((SUM(a.used_pages) * 8) / 1024.00), 2) AS NUMERIC(36, 2)) AS UsedSpaceMB,
    CAST(ROUND(((SUM(a.total_pages) - SUM(a.used_pages)) * 8) / 1024.00, 2) AS NUMERIC(36, 2)) AS UnusedSpaceMB
FROM sys.tables t
    INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
    INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
    INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
    LEFT OUTER JOIN sys.schemas s ON t.schema_id = s.schema_id
WHERE t.NAME IN ('Users', 'Products', 'Movements')
    AND t.is_ms_shipped = 0
    AND i.OBJECT_ID > 255
GROUP BY t.Name, s.Name, p.Rows
ORDER BY TotalSpaceMB DESC;

-- 7. Connection and Lock Analysis
SELECT
    session_id,
    login_time,
    host_name,
    program_name,
    login_name,
    status,
    cpu_time,
    memory_usage,
    total_scheduled_time,
    reads,
    writes,
    logical_reads
FROM sys.dm_exec_sessions
WHERE is_user_process = 1
ORDER BY cpu_time DESC;

-- 8. Long Running Queries
SELECT
    r.session_id,
    r.start_time,
    r.status,
    r.command,
    SUBSTRING(qt.text, (r.statement_start_offset/2)+1,
        ((CASE r.statement_end_offset
            WHEN -1 THEN DATALENGTH(qt.text)
            ELSE r.statement_end_offset
        END - r.statement_start_offset)/2)+1) AS statement_text,
    r.cpu_time,
    r.total_elapsed_time,
    r.reads,
    r.writes,
    r.logical_reads
FROM sys.dm_exec_requests r
CROSS APPLY sys.dm_exec_sql_text(r.sql_handle) qt
WHERE r.session_id > 50
    AND r.total_elapsed_time > 5000
-- 5 seconds
ORDER BY r.total_elapsed_time DESC;

-- 9. Backup Verification
SELECT
    database_name,
    backup_start_date,
    backup_finish_date,
    type,
    CASE type
        WHEN 'D' THEN 'Full Backup'
        WHEN 'I' THEN 'Differential Backup'
        WHEN 'L' THEN 'Log Backup'
    END AS backup_type,
    backup_size / 1024 / 1024 AS backup_size_mb
FROM msdb.dbo.backupset
WHERE database_name = DB_NAME()
ORDER BY backup_start_date DESC;

-- 10. Data Integrity Checks
DBCC CHECKDB WITH NO_INFOMSGS;

-- Check for data consistency
    SELECT
        'Users' AS TableName,
        COUNT(*) AS TotalRecords,
        COUNT(CASE WHEN Username IS NULL OR Username = '' THEN 1 END) AS InvalidUsernames,
        COUNT(CASE WHEN PasswordHash IS NULL OR PasswordHash = '' THEN 1 END) AS InvalidPasswords
    FROM Users

UNION ALL

    SELECT
        'Products',
        COUNT(*),
        COUNT(CASE WHEN Code IS NULL OR Code = '' THEN 1 END),
        COUNT(CASE WHEN Name IS NULL OR Name = '' THEN 1 END)
    FROM Products

UNION ALL

    SELECT
        'Movements',
        COUNT(*),
        COUNT(CASE WHEN ProductId NOT IN (SELECT Id
        FROM Products) THEN 1 END),
        COUNT(CASE WHEN Quantity <= 0 THEN 1 END)
    FROM Movements;

-- 11. Cleanup Completed Message
PRINT 'Database maintenance completed successfully at ' + CONVERT(VARCHAR, GETDATE(), 120);
