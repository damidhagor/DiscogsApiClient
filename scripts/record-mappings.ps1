<#
.SYNOPSIS
    Records WireMock mappings by proxying to the real Discogs API.

.DESCRIPTION
    Sets WIREMOCK_RECORD, cleans existing mappings, runs all test batches
    with rate-limit cooldowns, verifies no tokens leaked into mappings,
    and unsets the env var on completion.

    Must be run from the repository root (F:\DiscogsApiClient\).
    Requires DiscogsApiClient.Tests.testconfig.json to be configured with a valid PAT.
#>

$ErrorActionPreference = 'Stop'

$repoRoot = $PSScriptRoot | Split-Path
$srcDir = Join-Path $repoRoot 'src'
$mappingsDir = Join-Path (Join-Path (Join-Path $srcDir 'DiscogsApiClient.Tests') 'Fixtures') (Join-Path 'WireMock' 'Mappings')
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
    $env:WIREMOCK_RECORD = 'true'

    # Clean existing mappings
    Write-Host '--- Cleaning mappings directory ---'
    if (Test-Path $mappingsDir) {
        Remove-Item -Recurse -Force "$mappingsDir\*" -ErrorAction SilentlyContinue
    }

    # Run each test batch with a cooldown between them
    for ($i = 0; $i -lt $batches.Count; $i++) {
        $filter = $batches[$i]
        Write-Host "`n--- Running batch $($i + 1)/$($batches.Count): $filter ---"

        Push-Location $srcDir
        try {
            dotnet run --project DiscogsApiClient.Tests -f net10.0 -- --treenode-filter "$filter"
            if ($LASTEXITCODE -ne 0) {
                Write-Warning "Batch $($i + 1) had test failures (exit code $LASTEXITCODE) - continuing. Failing tests will still produce mappings."
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

    # Verify no real tokens leaked into mappings
    Write-Host "`n--- Verifying token cleanup ---"
    $adminMappings = Join-Path (Join-Path $mappingsDir '__admin') 'mappings'
    if (Test-Path $adminMappings) {
        $leaks = Select-String -Path "$adminMappings\*.json" -Pattern 'Discogs token=' |
            Where-Object { $_ -notmatch 'Discogs token=\*' }

        if ($leaks) {
            Write-Error "TOKEN LEAK DETECTED - do not commit these mappings:`n$($leaks | Out-String)"
            exit 1
        }
    }

    Write-Host "`n=== RECORDING COMPLETE - no token leaks detected ==="
}
finally {
    # Always unset recording mode
    Remove-Item Env:\WIREMOCK_RECORD -ErrorAction SilentlyContinue
}
