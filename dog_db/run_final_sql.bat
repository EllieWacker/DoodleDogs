echo off

rem batch file to run a sql script with sqlexpress

sqlcmd -S localhost -E -i dog_db.sql

echo .
echo if no error messages appear, the DB was created (but check)
pause