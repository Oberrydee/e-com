[CmdletBinding()]
param(
    [string]$PostgresBinPath = "C:\Program Files\PostgreSQL\17\bin",
    [int]$Port = 5433,
    [string]$DatabaseName = "ECommerceDb",
    [string]$Username = "postgres"
)

$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptRoot
$clusterRoot = Join-Path $projectRoot ".local\postgres-dev"
$dataDir = Join-Path $clusterRoot "data"
$logFile = Join-Path $clusterRoot "postgres.log"

$initDb = Join-Path $PostgresBinPath "initdb.exe"
$pgCtl = Join-Path $PostgresBinPath "pg_ctl.exe"
$psql = Join-Path $PostgresBinPath "psql.exe"
$createdb = Join-Path $PostgresBinPath "createdb.exe"

foreach ($tool in @($initDb, $pgCtl, $psql, $createdb)) {
    if (-not (Test-Path $tool)) {
        throw "PostgreSQL tool not found: $tool"
    }
}

function Invoke-NativeCommand {
    param(
        [string]$FilePath,
        [string[]]$Arguments
    )

    $process = Start-Process -FilePath $FilePath -ArgumentList $Arguments -NoNewWindow -Wait -PassThru
    if ($process.ExitCode -ne 0) {
        throw "Command failed: $FilePath $($Arguments -join ' ')"
    }
}

New-Item -ItemType Directory -Force -Path $clusterRoot | Out-Null

if (-not (Test-Path (Join-Path $dataDir "PG_VERSION"))) {
    Invoke-NativeCommand -FilePath $initDb -Arguments @(
        "-D", $dataDir,
        "-U", $Username,
        "-A", "trust",
        "--encoding=UTF8",
        "--locale-provider=libc"
    )
}

$isRunning = (Test-NetConnection -ComputerName localhost -Port $Port -WarningAction SilentlyContinue).TcpTestSucceeded

if (-not $isRunning) {
    & $pgCtl -D $dataDir -l $logFile -o "`"-p $Port`"" -w start
    if ($LASTEXITCODE -ne 0) {
        throw "Command failed: $pgCtl -D $dataDir -l $logFile -o `"-p $Port`" -w start"
    }
}

$dbExists = & $psql -h localhost -p $Port -U $Username -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname = '$DatabaseName'"
if (-not ($dbExists | ForEach-Object { $_.Trim() } | Where-Object { $_ -eq "1" })) {
    & $createdb -h localhost -p $Port -U $Username $DatabaseName | Out-Host
}

Write-Host "PostgreSQL dev instance ready on localhost:$Port (database: $DatabaseName)."
