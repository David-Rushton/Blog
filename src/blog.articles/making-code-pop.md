---
slug: I spend a lot of time writing my code, I want to look its best
tags: intro
date: 2020-06-14
image: /blog.articles/media/person-pointing-numeric-print-1342460.jpg
---

# Making Code Pop

I want my code to look good.  I used `Highlight.js` to archive this.

## SQL

```sql
-- What's going on here?
SELECT
    this
FROM
    that
;
```

## C#

```csharp
class PocoExample
{
    public string Name => 'Plain code class code example';

    public int Size { get; get; }


    public override string ToString
      => $"This class has a name.  It is ${this.Name}\nAnd a size of ${Size.ToString("000")}.";
}
```

## PowerShell

```powershell
Get-Process | Out-File -Path '/some/path/some-file.txt'
```

## Results

| Percentile | Execution Time (ms) |
| ---------- | ------------------- |
|         99 |               1,245 |
|         95 |                 997 |
|         90 |                 870 |
|         80 |                  50 |
