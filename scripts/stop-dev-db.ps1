[CmdletBinding()]
param(
    [string]$PostgresBinPath = "C:\Program Files\PostgreSQL\17\bin"
)

$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptRoot
$dataDir = Join-Path $projectRoot ".local\postgres-dev\data"
$pgCtl = Join-Path $PostgresBinPath "pg_ctl.exe"
$pidFile = Join-Path $dataDir "postmaster.pid"

if (-not (Test-Path $pgCtl)) {
    throw "PostgreSQL tool not found: $pgCtl"
}

if (-not (Test-Path $dataDir)) {
    Write-Host "No local PostgreSQL cluster found."
    exit 0
}

if (-not (Test-Path $pidFile)) {
    Write-Host "Local PostgreSQL instance is already stopped."
    exit 0
}

& $pgCtl -D $dataDir stop -m fast | Out-Host
