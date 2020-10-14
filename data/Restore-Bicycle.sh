#!/bin/bash
/opt/mssql-tools/bin/sqlcmd -S tcp:localhost,1433 -U sa -P pass@word1  -i /data/Restore.sql -o /tmp/restore.log