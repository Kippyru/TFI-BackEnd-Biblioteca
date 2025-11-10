# Script para cerrar la aplicación y compilar limpiamente
# Uso: .\build.ps1

Write-Host "`n?? Cerrando instancias previas..." -ForegroundColor Yellow

# Cerrar todos los procesos de la aplicación
Get-Process -Name "TFI-BackEnd-Biblioteca" -ErrorAction SilentlyContinue | Stop-Process -Force
Start-Sleep -Seconds 2

Write-Host "? Procesos cerrados`n" -ForegroundColor Green

# Compilar
Write-Host "?? Compilando proyecto..." -ForegroundColor Cyan
dotnet build

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n? Compilación exitosa`n" -ForegroundColor Green
} else {
    Write-Host "`n? Error en la compilación`n" -ForegroundColor Red
}
