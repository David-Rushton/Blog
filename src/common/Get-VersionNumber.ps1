Set-StrictMode -Version 'Latest'

$versionPath = Join-Path -Path $PSScriptRoot -ChildPath 'version.xml'
$versionXml = Select-Xml -Path $versionPath -XPath '/version'

$major = $versionXml.Node.major
$minor = $versionXml.Node.minor
$buildNumber = $env:GITHUB_RUN_ID
$shortSha = git rev-parse --short HEAD

return "$major.$minor.$($buildNumber):$shortSha"
