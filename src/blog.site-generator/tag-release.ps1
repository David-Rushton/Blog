[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]
    $BuildNumber
)

Set-StrictMode -Version 'Latest'

Import-Module ./build-module.psm1 -Force


$versionNumber = Get-VersionNumber -BuildNumber $BuildNumber

Write-Verbose "Tagging release with version number $versionNumber"
git tag -a $versionNumber -m "Release version $versionNumber"
