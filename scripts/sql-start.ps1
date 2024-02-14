<# Start SQL in Docker #>
$PASS='pass@word1'
$CNAME='d-sql'
$PORT=1433
$IMAGENAME='mcr.microsoft.com/mssql/server:2017-latest'
$parentdir=(Split-Path $PSScriptRoot -Parent)
$data="${parentdir}\data"
$vol="${data}:/data"
#write-output $vol

docker stop ${CNAME}
docker rm ${CNAME}
docker pull ${IMAGENAME}

$RCMD="docker run --name ${CNAME} --hostname ${CNAME} -v ${vol} -e 'ACCEPT_EULA=Y' -e 'MSSQL_PID=Evaluation' -e MSSQL_SA_PASSWORD=${PASS} -p ${PORT}:${PORT} -d '${IMAGENAME}'" 
Write-Output $RCMD
Invoke-Expression $RCMD

Start-Sleep -Seconds 15

$DCMD="docker exec -it ${CNAME} //opt/mssql-tools/bin/sqlcmd -S tcp:localhost,${PORT} -U sa -P ${PASS}  -i //data/Restore.sql -o //tmp/restore.log"
Write-Output $DCMD
Invoke-Expression $DCMD
