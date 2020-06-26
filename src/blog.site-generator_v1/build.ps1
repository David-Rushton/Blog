<#
    .SYNOPSIS
    Builds the blog

    .DESCRIPTION
    The blog is static website.  The HTML is generated at build time form the articles, written in markdown.

    .PARAMETER LocalBuild
    LocalBuild builds are created and hosted within a docker container.

    .EXAMPLE
    # Builds the site
    PS> ./build.ps1

    .EXAMPLE
    # Builds a local copy of the site, for testing
    PS? ./build.ps1 -LocalBuild
#>
#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter()]
    [string]
    $BuildNumber = '0-Dev',

    [Parameter()]
    [string]
    $BuildSHA


)

Set-StrictMode -Version 'Latest'

Import-Module "$PSScriptRoot/build-module.psm1" -Force


if (-not $BuildSHA) {
    $BuildSHA = git rev-parse HEAD
}


# Blog is built from scratch during each build
# Only applies to local builds
Remove-Item -Path "$PSScriptRoot/../blog" -ErrorAction 'SilentlyContinue' -Force -Recurse


# Copy template and articles
Copy-Item -Path "$PSScriptRoot/../blog.template" -Filter '*.*' -Destination "$PSScriptRoot/../blog" -ErrorAction 'SilentlyContinue' -Recurse
Copy-Item -Path "$PSScriptRoot/../blog.articles" -Filter '*.*' -Destination "$PSScriptRoot/../blog" -ErrorAction 'SilentlyContinue' -Recurse


# Inject content into the website
$recentArticles = ''
$articleNumber = 1
$articles = Get-Articles -Verbose
foreach($article in $articles) {

    # Build index page
    if ($articleNumber -eq 1) {
        $articleArgs = @{
            Path = "$PSScriptRoot/../blog/index.html"
            KeyValuePairs = @(
                @{ Key = '$(last-updated)'       ; Value = $article.MetaData.Date  }
                @{ Key = '$(lead-article-title)' ; Value = $article.MetaData.Title }
                @{ Key = '$(lead-article-slug)'  ; Value = $article.MetaData.Slug  }
                @{ Key = '$(lead-article-path)'  ; Value = $article.ArticleUrl     }
                @{ Key = '$(lead-article-image)' ; Value = $article.MetaData.Image }
            )
        }
        Set-ContentVariableValues @articleArgs -Verbose
    }

    if ($articleNumber -in @(2, 4, 6)) {
        $articleArgs = @{
            Content = (Get-Content -Path "$PSScriptRoot/../blog/index.article-preview-even.fragment.template.html" -Raw)
            KeyValuePairs = @(
                @{ Key = '$(article-title)'   ; Value = $article.MetaData.Title   }
                @{ Key = '$(article-slug)'    ; Value = $article.MetaData.Slug    }
                @{ Key = '$(article-path)'    ; Value = $article.ArticleUrl       }
                @{ Key = '$(article-image)'   ; Value = $article.MetaData.Image   }
            )
        }
        $recentArticles += Set-ContentVariableValues @articleArgs -Verbose
    }

    if ($articleNumber -in @(3, 5)) {
        $articleArgs = @{
            Content = (Get-Content -Path "$PSScriptRoot/../blog/index.article-preview-odd.fragment.template.html" -Raw)
            KeyValuePairs = @(
                @{ Key = '$(article-title)'   ; Value = $article.MetaData.Title   }
                @{ Key = '$(article-slug)'    ; Value = $article.MetaData.Slug    }
                @{ Key = '$(article-path)'    ; Value = $article.ArticleUrl       }
                @{ Key = '$(article-image)'   ; Value = $article.MetaData.Image   }
            )
        }
        $recentArticles += Set-ContentVariableValues @articleArgs -Verbose
    }



    # Create an article
    $contentArgs = @{
        Path = $article.ArticlePath
        Value = $article.Article
        Force = $true
    }
    Set-Content @contentArgs


    $articleNumber++
}


$kvPairs = @(
    @{ Key = '$(article-previews)'  ; Value = $recentArticles                               }
    @{ Key = '$(build-number)'      ; Value = Get-VersionNumber -BuildNumber $BuildNumber   }
    @{ Key = '$(build-sha)'         ; Value = $BuildSHA                                     }
)
Set-ContentVariableValues -Path "$PSScriptRoot/../blog/index.html" -KeyValuePairs $kvPairs -Verbose
