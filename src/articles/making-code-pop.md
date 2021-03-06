---
title: Making Code Pop
slug: I spend a lot of time writing my code, I want to look its best
tags: [intro]
date: 2020-06-14
image: /articles/media/blur-close-up-code-computer-546819.jpg
image-credit: luis gomes
image-provider: Pexels
---

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

> How are quotes formatted?

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
