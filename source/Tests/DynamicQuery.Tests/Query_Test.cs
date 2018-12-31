using DotFramework.DynamicQuery.Oracle;
using DotFramework.DynamicQuery.SqlServer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DotFramework.DynamicQuery.Tests
{
    [TestClass]
    public class Query_Test
    {
        [TestMethod]
        public void Select_Test()
        {
            short s = 1;

            var builder = SelectQueryBuilder
                .Initialize()
                .From<Organization>()
                .InnerJoin(o => o.COST_CENTER_ID, (CostCenter c) => c.iD)
                //.Select((o, c) => new { o.ORG_NODE_NAME, c.COST_CENTER_NAME })
                .Sum((o, c) => o.CALENDAR_ID)
                .Where((o, c) => o.STATUS == s && (o.ORG_NODE_NAME.Contains("Ali") || o.ORG_NODE_NAME.Contains("Reza") || o.ORG_NODE_NAME.In(new List<String> { "R", "X" })));

            var evaluator_Oracle = new OracleSelectQueryEvaluator(builder.Query);
            var strQuery_Oracle = evaluator_Oracle.ToString();

            var evaluator_SqlServer = new SqlServerSelectQueryEvaluator(builder.Query);
            var strQuery_SqlServer = evaluator_SqlServer.ToString();

            //Assert.AreEqual(evaluator.ToString(), "SELECT *\r\nFROM \"MyOrganization\"\r\nWHERE \"MyOrganization\".\"STATUS\" = 1");
        }

        [TestMethod]
        public void Update_Test()
        {
            short s = 1;

            var builder = UpdateQueryBuilder<Organization>
                .Initialize()
                .Set(o => new Organization { iD = 1, ORG_NODE_NAME = "Ali", USED = o.USED + 1 })
                .Where((o) => o.STATUS == s && (o.ORG_NODE_NAME.Contains("Ali") || o.ORG_NODE_NAME.Contains("Reza") || o.ORG_NODE_NAME.In(new List<String> { "R", "X" })));

            var evaluator_Oracle = new OracleUpdateQueryEvaluator(builder.Query);
            var strQuery_Oracle = evaluator_Oracle.ToString();

            var evaluator_SqlServer = new SqlServerUpdateQueryEvaluator(builder.Query);
            var strQuery_SqlServer = evaluator_SqlServer.ToString();
        }
    }
}
