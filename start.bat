@echo off
setlocal
title ChertenkoWebServer
cd /d "%~dp0"

echo ==================================================
echo   ChertenkoWebServer
echo   http://localhost:5172/
echo   Browser opens automatically.
echo   To stop: close this window or run stop.bat
echo ==================================================
echo.

dotnet run --project ChertenkoWebServer.csproj --launch-profile http

echo.
echo Server stopped.
pause
