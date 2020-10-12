RESTORE DATABASE Bicycle FROM DISK = '/data/Bicycle.bak' WITH MOVE 'Bicycle' TO '/var/opt/mssql/data/Bicycle.mdf', MOVE 'Bicycle_Log' TO '/var/opt/mssql/data/Bicycle.ldf'
GO