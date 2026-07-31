@echo off
title CocinaManager
echo Starting CocinaManager...

:: Arrancar la aplicación en segundo plano
start "" dotnet run --project CocinaManager.API --urls "http://localhost:5000"

:: Esperar 4 segundos a que levante
timeout /t 4 /nobreak >nul

:: Abrir el navegador con la URL de la aplicación
start "" http://localhost:5000

echo CocinaManager is running. Press any key to exit.