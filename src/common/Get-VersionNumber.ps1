[CmdletBinding()]
param(
    [switch]$IncludeSha
)

Set-StrictMode -Version 'Latest'

$versionPath = Join-Path -Path $PSScriptRoot -ChildPath 'version.xml'
$versionXml = Select-Xml -Path $versionPath -XPath '/version'


if(-not $env:GITHUB_RUN_ID) {
    $env:GITHUB_RUN_ID = "local-build"
}


$major = $versionXml.Node.major
$minor = $versionXml.Node.minor
$buildNumber = $env:GITHUB_RUN_ID
$shortSha = git rev-parse --short HEAD

$versionNumber = "v$major.$minor.$($buildNumber)"

if($IncludeSha) {
    $versionNumber = "$versionNumber $shortSha"
}


return $versionNumber
