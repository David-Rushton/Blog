<#
    .SYNOPSIS
    Build helper functions

    .DESCRIPTION
    Functions required by the build process
#>

Set-StrictMode -Version 'Latest'


# Returns the current version number
# Format: Major.Minor.Build
function Get-VersionNumber {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string]
        $BuildNumber
    )

    $versionPath = Join-Path -Path $PSScriptRoot -ChildPath '..' 'common' 'version.xml'
    $version = Select-Xml -Path $versionPath -XPath '/version'

    return "$($version.Node.major).$($version.Node.minor).$($BuildNumber)"
}
Export-ModuleMember -Function 'Get-VersionNumber'


# Replaces variables within a template file with a value
function Set-ContentVariableValues {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory,ParameterSetName='ByPath')]
        [ValidateScript({ Test-Path -Path $_ })]
        [string]
        $Path,

        [Parameter(Mandatory,ParameterSetName='ByString')]
        [ValidateNotNullOrEmpty()]
        [string]
        $Content,

        [Parameter(Mandatory,ParameterSetName='ByPath')]
        [Parameter(Mandatory,ParameterSetName='ByString')]
        [ValidateScript({ $_.Count -ge 1 })]
        [hashtable[]]
        $KeyValuePairs
    )

    if ($Path) {
        $Content = Get-Content -Path $Path -Raw
    }

    foreach ($keyValue in $KeyValuePairs) {
        $Content = $Content.Replace($keyValue.Key, $keyValue.Value.ToString().Trim())
    }

    if ($Path) {
        Set-Content -Path $Path -Value $Content -Force
    }
    else {
        return $Content
    }
}
Export-ModuleMember -Function Set-ContentVariableValues


# Returns each article
function Get-Articles {
    [CmdletBinding()]
    param()

    return Get-ArticlePaths | Get-Article | Sort-Object { $_.MetaData.date } -Descending
}
Export-ModuleMember -Function Get-Articles


# Returns the path to each article
function Get-ArticlePaths {
    [CmdletBinding()]
    param()

    $articlesPath = Join-Path -Path $PSScriptRoot '..' 'blog' 'blog.articles'
    if (-not (Test-Path -Path $articlesPath)) {
        throw "Cannot find articles"
    }


    Write-Verbose "Searching for articles: $articlesPath"
    return Get-ChildItem -Path $articlesPath -Filter '*.md'
}


# Returns an article
function Get-Article {
    [CmdletBinding()]
    param(
        [Parameter(
            Mandatory,
            ValueFromPipeline
        )]
        [string]
        $Path
    )

    process {

        Write-Verbose "Processing article: $Path"

        # Ignore any errors here, the test-article cmdlet will pick these up
        $metaData = Get-YamlFrontMatter -Path $Path | ConvertFrom-YamlFrontMatter -ErrorAction 'SilentlyContinue'
        $markdownArticle = Get-MarkdownContent -Path $Path -ErrorAction 'SilentlyContinue'

        if(-not (Test-Article -MetaData $metaData -MarkdownArticle $markdownArticle)) {
            throw "Article does not conform to the standard: $Path"
        }


        return @{
            MetaData = $metaData
            ArticlePath = [System.IO.Path]::ChangeExtension($Path, '.html')
            ArticleUrl = "./blog.articles/$([System.IO.Path]::GetFileNameWithoutExtension($Path)).html"
            Article = New-Article -MetaData $metaData -MarkdownArticle $markdownArticle
        }
    }
}


# Injects meta data and article into template
function New-Article {
    [CmdletBinding()]
    param(
        [Hashtable]
        $MetaData,

        [string]
        $MarkdownArticle
    )


    $template = Get-Content -Path "$PSScriptRoot/../blog/blog.articles/article.template.html" -Raw


    # Inject meta
    $meta = ""
    foreach($key in $MetaData.Keys) {
        $meta += "<meta name=`"$key`" content=`"$($MetaData[$key].Trim())`">`n`t`t"
    }
    $template = $template.Replace('$(meta-tags)', $meta)
    $template = $template.Replace('<meta name="slug" content="', '<meta name="description" content="')
    $template = $template.Replace('<meta name="tags" content="', '<meta name="keywords" content="')


    # Inject content
    $htmlArticle = (ConvertFrom-Markdown -InputObject $MarkdownArticle).Html
    $template = $template.Replace('$(article-title)', $MetaData['title'])
    $template = $template.Replace('$(article-slug)', $MetaData['slug'])
    $template = $template.Replace('$(article-posted)', $MetaData['date'])
    $template = $template.Replace('$(article-author)', $MetaData['author'])
    $template = $template.Replace('$(article-content)', $htmlArticle)
    $template = $template.Replace('$(article-image)', $MetaData.Image)
    $template = $template.Replace('$(article-image-credit)', $MetaData['image-credit'])


    return $template
}


# Tests if the article meets the minimum standard
function Test-Article {
    [CmdletBinding()]
    param(
        [hashtable]
        $MetaData,

        [string]
        $MarkdownArticle
    )

    [int]$errorsFound = 0


    if ($MetaData.Count -eq 0) {
        Write-Error 'Required Yaml front matter is missing'
        $errorsFound++
    }

    # Check for required tags in the front matter
    foreach($metaElement in @('slug', 'tags', 'date', 'title')) {
        if (-not ($MetaData[$metaElement])) {
            Write-Error "Required meta data '$metaElement' is missing"
            $errorsFound++
        }
    }

    if (-not $MarkdownArticle) {
        Write-Error 'Required article is missing'
        $errorsFound++
    }

    return ($errorsFound -eq 0)
}


# Reads the markdown article from disk
function Get-MarkdownContent {
    [CmdletBinding()]
    param (
        [string]
        $Path
    )

    $yamlMatches = Select-String -Path $Path -Pattern '^---(\s)*$'
    if ($yamlMatches.Count -ge 2) {
        $markdown = (Get-Content -Path $Path | Select-Object -Skip ($yamlMatches[1].LineNumber)) -join "`n"
    }


    return $markdown.Trim()
}


# Reads the Yaml front matter from disk
function Get-YamlFrontMatter {
    [CmdletBinding()]
    param(
        [string]
        $Path
    )

    $yamlMatches = Select-String -Path $Path -Pattern '^---(\s)*$'
    if ($yamlMatches.Count -ge 2) {
        if($yamlMatches[0].LineNumber -eq 1) {
            return (Get-Content -Path $Path | Select-Object -First $yamlMatches[1].LineNumber) -join "`n"
        }
    }


    # Yaml front matter not included
    return $null
}


# Converts the Yaml front matter into a hashtable
function ConvertFrom-YamlFrontMatter {
    [CmdletBinding()]
    param(
        [parameter(
            Mandatory,
            ValueFromPipeline
        )]
        [string]
        $YamlFrontMatter
    )

    $metaData = @{}

    # This could be much better
    # This is not a proper Yaml parser
    foreach($line in ($yamlFrontMatter -split "`n")) {
        if($line -like '*:*') {
            $elements = $line -split ':'
            $metaData[$elements[0]] = $elements[1]
        }
    }

    # Append default values, is missing
    if (-not ($metaData.ContainsKey('author'))) {
        $metaData['author'] = 'David Rushton'
    }


    return $metaData
}
