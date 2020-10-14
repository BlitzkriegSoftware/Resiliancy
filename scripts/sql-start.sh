#!/bin/bash
export PASS=pass@word1
export CNAME=d-sql
export PORT=1433
export IMAGENAME=mcr.microsoft.com/mssql/server:2017-latest
parentdir="$(dirname "$(pwd)")"
export data="${parentdir}/data"
docker stop ${CNAME}
docker rm ${CNAME}
docker pull ${IMAGENAME}

set -x
docker run --name ${CNAME} -v "/${data}:/data" -e 'ACCEPT_EULA=Y' -e "SA_PASSWORD=${PASS}" -p $PORT:$PORT -d ${IMAGENAME} 
set +x

sleep 15

export DCMD="docker exec -it ${CNAME} //opt/mssql-tools/bin/sqlcmd -S tcp:localhost,${PORT} -U sa -P ${PASS}  -i //data/Restore.sql -o //tmp/restore.log"

set -x
winpty $DCMD
set +x
