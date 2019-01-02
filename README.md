[![Build status](https://ci.appveyor.com/api/projects/status/s20w31tl8v7uomw8?svg=true)](https://ci.appveyor.com/project/dotFramework/dynamic-query)
[![NuGet Release](https://img.shields.io/nuget/vpre/DotFramework.DynamicQuery.svg)](https://www.nuget.org/packages/DotFramework.DynamicQuery)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DotFramework.DynamicQuery.svg)](https://www.nuget.org/packages/DotFramework.DynamicQuery)
[![License](https://img.shields.io/badge/license-apache%202.0-60C060.svg)](https://github.com/dotFramework/dynamic-query/blob/master/LICENSE)

# Dynamic Query

This library is for generating Sql Server or Oracle queries using Expressions in strong named format. For creating Sql server or Oracle queries you do not need to change any of your expressions but only QueryEvaluator type.

## Installation:

  Install DotFramework.DynamicQuery nuget package
  
  For Sql Install DotFramework.DynamicQuery.SqlServer nuget package
  
  For Oracle Install DotFramework.DynamicQuery.Oracle nuget package
  
## Simpe Select:

```bash
   var builder = SelectQueryBuilder
                .Initialize()
                .From<TestEntity>();
  
  var SqlSimpleSelect = new SqlServerSelectQueryEvaluator(builder.Query);
  var SqlSimpleSelectQuery = SqlSimpleSelect.toString();
  
  var OracleSimpleSelect = new OracleSelectQueryEvaluator(builder.Query);
  var OracleSimpleSelectQuery = OracleSimpleSelect.toString();
```

## Select with Where

```bash
  var builder = SelectQueryBuilder
                .Initialize()
                .From<TestEntity>()
                .Where(n => n.Name.Contains("Yes"));
```
Result: 

SELECT *
FROM [TestEntity]
WHERE [TestEntity].[Name] LIKE N'%Yes%'

## Simple Join:

```bash
  var builder = SelectQueryBuilder
                .Initialize()
                .From<StudentEntity>()
                .InnerJoin(s => s.TestEntitytID, (TestEntity t) => t.ID)
                .Select((s, t) => new { s.ID, t.Name });
```                

Result:

SELECT [StudentEntity].[ID], [TestEntity].[Name]
FROM [StudentEntity] INNER JOIN [TestEntity] ON [StudentEntity].[TestEntitytID] = [TestEntity].[ID] 

## Simple Delete

```bash
  var builder = DeleteQueryBuilder<StudentEntity>
                .Initialize()
                .Where(s => s.ID == 1);

            var res = new SqlServerDeleteQueryEvaluator(builder.Query);
```

Result:

DELETE FROM [StudentEntity]
WHERE [StudentEntity].[ID] = 1

## Simple Update

```bash
  var builder = UpdateQueryBuilder<TestEntity>
                .Initialize()
                .Set(s => new TestEntity { Name = "test" })
                .Where(s => s.ID == 1);

            var res = new SqlServerUpdateQueryEvaluator(builder.Query);
```

Result:
  
  UPDATE ["TestEntity"]
SET [Name] = N'test'
WHERE [TestEntity].[ID] = 1
