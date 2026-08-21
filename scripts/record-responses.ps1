<#
.SYNOPSIS
    Records Discogs API requests and responses by proxying to the real Discogs API.

.DESCRIPTION
    Sets DISCOGS_RECORD, cleans existing recordings, runs all test batches
    with rate-limit cooldowns and unsets the env var on completion.

    Must be run from the repository root (F:\DiscogsApiClient\).
    Requires DiscogsApiClient.Tests.testconfig.json to be configured with a valid PAT.
#>

$ErrorActionPreference = 'Stop'

$repoRoot = $PSScriptRoot | Split-Path
$srcDir = Join-Path $repoRoot 'src'
$recordingsDir = [System.IO.Path]::Combine($srcDir, 'DiscogsApiClient.Tests', 'Fixtures', 'Recording', 'Recordings')

$cooldownSeconds = 65

function Invoke-TestBatch {
    param(
        [Parameter(Mandatory)]
        [string]$Filter
    )

    Write-Host "Running: $Filter"

    Push-Location $srcDir
    try {
        dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "$Filter"
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "Filter had test failures (exit code $LASTEXITCODE) - continuing. Failing tests will still produce recordings."
        }
    }
    finally {
        Pop-Location
    }
}

function Wait-RateLimit {
    Write-Host "--- Cooling down for $cooldownSeconds seconds (rate limit) ---"
    Start-Sleep -Seconds $cooldownSeconds
}

try {
    $env:DISCOGS_RECORD = 'true'

    Write-Host '--- Cleaning recording directory ---'
    if (Test-Path $recordingsDir) {
        Remove-Item -Recurse -Force "$recordingsDir\*" -ErrorAction SilentlyContinue
    }

    Write-Host "`n=== BATCH 1/7: User Tests ==="
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.User/**'
    Write-Host "=== BATCH 1/7 COMPLETE ==="
    Wait-RateLimit

    Write-Host "`n=== BATCH 2/7: Database Tests (Part 1) ==="
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Database/MasterReleaseTests/**'
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Database/ArtistsTests/**'
    Write-Host "=== BATCH 2/7 COMPLETE ==="
    Wait-RateLimit

    Write-Host "`n=== BATCH 3/7: Database Tests (Part 2) ==="
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Database/LabelsTests/**'
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Database/SearchTests/**'
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Database/ReleasesTests/**'
    Write-Host "=== BATCH 3/7 COMPLETE ==="
    Wait-RateLimit

    Write-Host "`n=== BATCH 4/7: Collection Folders Tests ==="
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionFoldersTests/**'
    Write-Host "=== BATCH 4/7 COMPLETE ==="
    Wait-RateLimit

    Write-Host "`n=== BATCH 5/7: Collection Folder Releases Tests ==="
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionFolderReleasesTests/**'
    Write-Host "=== BATCH 5/7 COMPLETE ==="
    Wait-RateLimit

    Write-Host "`n=== BATCH 6/7: Collection Value Tests ==="
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionValueTests/**'
    Write-Host "=== BATCH 6/7 COMPLETE ==="
    Wait-RateLimit

    Write-Host "`n=== BATCH 7/7: Wantlist Tests ==="
    Invoke-TestBatch -Filter '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/WantlistTests/**'
    Write-Host "=== BATCH 7/7 COMPLETE ==="

    Write-Host "`n=== RECORDING COMPLETE ==="
}
finally {
    Remove-Item Env:\DISCOGS_RECORD -ErrorAction SilentlyContinue
}
