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
    [switch]
    $LocalBuild
)

Set-StrictMode -Version 'Latest'

Import-Module "$PSScriptRoot/build-module.psm1" -Force


# Blog is built from scratch during each build
# Only applies to local builds
Remove-Item -Path "$PSScriptRoot/../src/blog" -ErrorAction 'SilentlyContinue' -Force -Recurse


# Copy template and articles
Copy-Item -Path "$PSScriptRoot/../src/blog.template" -Filter '*.*' -Destination "$PSScriptRoot/../src/blog" -ErrorAction 'SilentlyContinue' -Recurse
Copy-Item -Path "$PSScriptRoot/../src/articles" -Filter '*.*' -Destination "$PSScriptRoot/../src/blog" -ErrorAction 'SilentlyContinue' -Recurse


# Inject content into the website
$recentArticles = ''
$articleNumber = 1
$articles = Get-Articles -Verbose:$LocalBuild -Debug:$LocalBuild
foreach($article in $articles) {

    # Build index page
    if ($articleNumber -eq 1) {
        $articleArgs = @{
            Path = "$PSScriptRoot/../src/blog/index.html"
            KeyValuePairs = @(
                @{ Key = '$(last-updated)'       ; Value = $article.MetaData.Date  }
                @{ Key = '$(lead-article-title)' ; Value = $article.MetaData.Title }
                @{ Key = '$(lead-article-slug)'  ; Value = $article.MetaData.Slug  }
                @{ Key = '$(lead-article-path)'  ; Value = $article.ArticleUrl     }
                @{ Key = '$(lead-article-image)' ; Value = $article.MetaData.Image }
            )
        }
        Set-ContentVariableValues @articleArgs
    }

    if ($articleNumber -in @(2, 4, 6)) {
        $articleArgs = @{
            Content = (Get-Content -Path "$PSScriptRoot/../src/blog/index.article-preview-even.fragment.template.html" -Raw)
            KeyValuePairs = @(
                @{ Key = '$(article-title)'   ; Value = $article.MetaData.Title   }
                @{ Key = '$(article-slug)'    ; Value = $article.MetaData.Slug    }
                @{ Key = '$(article-path)'    ; Value = $article.ArticleUrl       }
                @{ Key = '$(article-preview)' ; Value = $article.MetaData.Preview }
                @{ Key = '$(article-image)'   ; Value = $article.MetaData.Image   }
            )
        }
        $recentArticles += Set-ContentVariableValues @articleArgs
    }

    if ($articleNumber -in @(3, 5)) {
        $articleArgs = @{
            Content = (Get-Content -Path "$PSScriptRoot/../src/blog/index.article-preview-odd.fragment.template.html" -Raw)
            KeyValuePairs = @(
                @{ Key = '$(article-title)'   ; Value = $article.MetaData.Title   }
                @{ Key = '$(article-slug)'    ; Value = $article.MetaData.Slug    }
                @{ Key = '$(article-path)'    ; Value = $article.ArticleUrl       }
                @{ Key = '$(article-preview)' ; Value = $article.MetaData.Preview }
                @{ Key = '$(article-image)'   ; Value = $article.MetaData.Image   }
            )
        }
        $recentArticles += Set-ContentVariableValues @articleArgs
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

Set-ContentVariableValues -Path "$PSScriptRoot/../src/blog/index.html" -KeyValuePairs @( @{ Key = '$(article-previews)' ; Value = $recentArticles } )
