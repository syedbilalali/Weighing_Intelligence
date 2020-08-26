@echo off
SQLCMD -U  "sa" -P "Avery@123" -S 70031worndw10\sqlexpress2008 -Q "EXEC SP_BACKUPDATABASES @databaseName='WIWEB_AveryDB_New' , @backupType='F',@backupLocation='C:\\'"
