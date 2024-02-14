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

--or--

* * sql-start.ps1

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


Licensed under the [MIT license](LICENSE).

## About me ##

**Stuart Williams**

* I Cloud. I Code. 
* <a href="mailto:stuart.t.williams@outlook.com" target="_blank">stuart.t.williams@outlook.com</a> (e-mail)
* LinkedIn: <a href="http://lnkd.in/P35kVT" target="_blank">http://lnkd.in/P35kVT</a>
* YouTube: <a href="https://www.youtube.com/user/spookdejur1962/videos" target="_blank">https://www.youtube.com/user/spookdejur1962/videos</a> 
