<#dr
    .SYNOPSIS
    Generates release notes

    .DESCRIPTION
    Release notes are generated from the git logs
#>
[CmdletBinding()]
param(
)


Set-StrictMode -Version 'Latest'


# We are only interested in key messages
$keyLogEntryPattern = '(^tag:\sv\d\.\d\.\d+$|^Feature:|^Fix:\s|^Docs:\s|^Internal:\s)'

# Extract raw data from Git log
$rawLog = (git log --pretty=format:"%D%+s%n" | Select-String -Pattern $keyLogEntryPattern)


# The log is read into a dictionary, using the the version number as a key
# This allows us to process and sort the content later
$currentVersion = 'vNext'
$versionLog = @{}
$versionLog[$currentVersion] = @()

foreach($line in $rawLog) {

    # Version numbers are encoded as tags
    if($line -match '^tag:\s') {
        $currentVersion = ($line -replace @('tag: ', ''))
        $versionLog[$currentVersion] = @()
    }
    else {
        $versionLog[$currentVersion] += $line
    }

}


# The dictionary is converted into a markdown document
$mdLog = "# Change Log`n"
$keys = $versionLog.Keys | Sort-Object -Descending
foreach($key in $keys) {

    if($versionLog[$key].Count -gt 0) {

        $mdLog += "`n## $key`n`n"
        foreach($change in ($versionLog[$key] | Sort-Object)) {
            $mdLog += "- $change`n"
        }

    }

}


## The markdown doc is saved to the root of this project
$outArgs = @{
    FilePath = Join-Path -Path $PSScriptRoot -ChildPath '..' 'release-notes.md'
    Encoding = 'utf8'
}
$mdLog | Out-File @outArgs
