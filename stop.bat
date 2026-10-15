@echo off
setlocal EnableExtensions
title ChertenkoWebServer - stop

echo Stopping ChertenkoWebServer...
taskkill /f /im ChertenkoWebServer.exe

rem fallback: anything still holding port 5172
for /f "tokens=5" %%p in ('netstat -ano ^| findstr ":5172 " ^| findstr /i "LISTENING"") do (
    taskkill /f /pid %%p
)

echo Done. Server stopped.
pause
