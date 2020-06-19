---
slug: Why column aliases are available to the order by clause but not the group by?
tags: sql-server
date: 2020-06-19
image: /articles/media/person-writing-on-notebook-669615.jpg
---

# SQL Server and the Logical Processing Order

Have you ever wondered why you can use a column alias in the `ORDER BY` clause but not the `GROUP BY`?  This is a
consequence of how SQL Server processes your queries.  Each query clause is executed in a set order.  This has a catchy
moniker; the logical processing order of the `SELECT` statement (I’d bet good money that an engineer devised the name).
MS Docs defines the order in an [article on the SELECT statement](https://docs.microsoft.com/en-us/sql/t-sql/queries/select-transact-sql?view=sql-server-2017):

1. FROM
1. ON
1. JOIN
1. WHERE
1. GROUP BY
1. WITH CUBE or WITH ROLLUP
1. HAVING
1. SELECT
1. DISTINCT
1. ORDER BY
1. TOP

The rows returned by each step are available to all subsequent steps.  For this reason a column alias defined by the
`SELECT` clause (step 8) is available to the `ORDER BY` (step 10) but not the `GROUP BY` (step 5).

As you might expect; it is not quite that simple.  It’s called the logical processing order for a reason.  In reality
SQL Server will process your query however it sees fit.  But you can be assured that no matter how your query is
physically processed the logical order will be respected.

We’ve answered the question and learnt something along the way.  Job done.  Well not quite.  Now that we know what’s
going behind the scenes we can exploit it.  This query uses a column alias in both the `ORDER BY` and `GROUP BY`.

```sql
-- ObjectType is a column alias used by the GROUP BY clause.
SELECT
    ca.ObjectType,
    COUNT(*)
FROM
    sys.objects AS so
    CROSS APPLY
        (
            -- Provide type with an alias of ObjectType.
            SELECT
                so.[type] AS ObjectType
        ) AS ca
GROUP BY
    ca.ObjectType
ORDER BY
    ca.ObjectType
;
```

This works because the column alias is defined within the `FROM` clause (step 1).  From here it is available to all of
the other steps including the `GROUP BY` (step 5).  I’ve used the [APPLY operator](https://docs.microsoft.com/en-us/sql/t-sql/queries/from-transact-sql?view=sql-server-2017#using-apply) to work this witchcraft.  APPLYs are insanely powerful and well worth your time
getting to grips with.  I’ll blog about these another time.
