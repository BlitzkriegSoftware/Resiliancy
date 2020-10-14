#!/bin/bash
export CNAME=d-sql
export PASS=pass@word1
export PORT=1433
export DCMD="docker exec -it ${CNAME} //opt/mssql-tools/bin/sqlcmd -S tcp:localhost,${PORT} -U sa -P ${PASS}"
winpty $DCMD
