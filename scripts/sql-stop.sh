#!/bin/bash
export CNAME=d-sql
docker stop ${CNAME}
docker rm ${CNAME}
