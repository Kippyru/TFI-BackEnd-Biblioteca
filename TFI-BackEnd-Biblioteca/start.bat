@echo off
echo ========================================
echo   Iniciando TFI-BackEnd-Biblioteca
echo ========================================
echo.
echo Cerrando instancias previas...
taskkill /F /IM TFI-BackEnd-Biblioteca.exe 2>nul

echo.
echo Compilando proyecto...
dotnet build --verbosity quiet

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Fallo en la compilacion
    pause
    exit /b 1
)

echo.
echo Iniciando aplicacion en HTTPS...
echo.
echo URLs disponibles:
echo - Scalar: https://localhost:7063/scalar/v1
echo - API: https://localhost:7063/api/libros
echo.
echo Presiona Ctrl+C para detener
echo.

dotnet run --launch-profile https
