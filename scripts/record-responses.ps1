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
$recordingsDir = Join-Path (Join-Path (Join-Path $srcDir 'DiscogsApiClient.Tests') 'Fixtures') (Join-Path 'Recording' 'Recordings')
$testProject = Join-Path $srcDir 'DiscogsApiClient.Tests'

# Test batches: namespace filters executed in order with rate-limit sleeps between them
$batches = @(
    '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.User/**'
    '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Database/**'
    '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionFoldersTests/**'
    '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionFolderReleasesTests/**'
    '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/CollectionValueTests/**'
    '/DiscogsApiClient.Tests/DiscogsApiClient.Tests.Collection/WantlistTests/**'
)

$cooldownSeconds = 65

try {
    # Enable recording mode
    $env:DISCOGS_RECORD = 'true'

    # Clean existing recordings
    Write-Host '--- Cleaning recording directory ---'
    if (Test-Path $recordingsDir) {
        Remove-Item -Recurse -Force "$recordingsDir\*" -ErrorAction SilentlyContinue
    }

    # Run each test batch with a cooldown between them
    for ($i = 0; $i -lt $batches.Count; $i++) {
        $filter = $batches[$i]
        Write-Host "`n--- Running batch $($i + 1)/$($batches.Count): $filter ---"

        Push-Location $srcDir
        try {
            dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "$filter"
            if ($LASTEXITCODE -ne 0) {
                Write-Warning "Batch $($i + 1) had test failures (exit code $LASTEXITCODE) - continuing. Failing tests will still produce recordings."
            }
        }
        finally {
            Pop-Location
        }

        # Cooldown between batches (skip after the last one)
        if ($i -lt $batches.Count - 1) {
            Write-Host "--- Cooling down for $cooldownSeconds seconds (rate limit) ---"
            Start-Sleep -Seconds $cooldownSeconds
        }
    }

    Write-Host "`n=== RECORDING COMPLETE ==="
}
finally {
    # Always unset recording mode
    Remove-Item Env:\DISCOGS_RECORD -ErrorAction SilentlyContinue
}
