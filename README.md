# Resiliancy

A demo of how to be resilient

## Demo Web Service

1. We have controllers demoing how to do resiliancy in ADO.NET, Entity-Framework, and HttpClient

2. Run the webapi and look at the demos

3. Read the comments that start with `// SEE:` to see resiliancy in action

## SQL Scripts

Please have `Docker` Running.

In `scripts/` there are the following scripts

* sql-start.sh - starts sql server and mounts the demo `data` directory as `/data` and restores the "Bicycle" database
* sql-stop.sh - stops sql server and tears container down
* sql-cmd.sh - Opens a SQL prompt on the container  
* sh-cmd.sh  - Opens a BASH shell on the container

## Bicylce SQL Database

The SQL Demos use the Blitzkrieg Bicycle Demo. 

![Bicycle](./Bicycle_DB_Diagram.png)

## Restore SQL Linux

You don't need to run this but it is here for completeness, see: 
* `data/Bicycle.bak` - The actual backup of the `Bicycle` database for SQL Server
* `data/Restore.sql` - SQL Server Restore Script
* `data/Restore-Bicycle.sh` - Bash script to run `sqlcmd` to in turn run the `sql` script, which restores the `bak` file

## Demo REST endpoint to call

This exellent endpoint is just the thing to demo how to use `HttpClientFactory` properly.

http://dummy.restapiexample.com/


## About

Stuart Williams

* Cloud/DevOps Practice Lead

* Magenic Technologies Inc.
* Office of the CTO

* [e-mail](mailto:stuartw@magenic.com)

* [Blog](https://blitzkriegsoftware.azurewebsites.net/Blog)

* [LinkedIn](http://lnkd.in/P35kVT)

* [YouTube](https://www.youtube.com/user/spookdejur1962/videos)

