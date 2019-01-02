[![Build status](https://ci.appveyor.com/api/projects/status/s20w31tl8v7uomw8?svg=true)](https://ci.appveyor.com/project/dotFramework/dynamic-query)
[![NuGet Release](https://img.shields.io/nuget/vpre/DotFramework.DynamicQuery.svg)](https://www.nuget.org/packages/DotFramework.DynamicQuery)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DotFramework.DynamicQuery.svg)](https://www.nuget.org/packages/DotFramework.DynamicQuery)
[![License](https://img.shields.io/badge/license-apache%202.0-60C060.svg)](https://github.com/dotFramework/dynamic-query/blob/master/LICENSE)

# Dynamic Query

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
