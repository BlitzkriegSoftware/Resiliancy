#!/bin/bash
export CNAME=d-sql
export DCMD="docker exec -it ${CNAME} //bin/bash"
winpty $DCMD
