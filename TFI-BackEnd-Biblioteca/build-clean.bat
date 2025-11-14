@echo off
echo ========================================
echo   Limpiando procesos y compilando
echo ========================================

REM Cerrar todas las instancias
taskkill /F /IM TFI-BackEnd-Biblioteca.exe 2>nul
timeout /t 2 /nobreak >nul

REM Compilar
echo.
echo Compilando proyecto...
dotnet build

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo   ? Compilación exitosa
    echo ========================================
) else (
    echo.
echo ========================================
    echo   ? Error en la compilación
    echo ========================================
    pause
)
