using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

using Cogito.Text.Json.Schema.Validation;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cogito.Text.Json.Schema.Tests.Validation
{

    [TestClass]
    public class JSchemaValidatorBuilderTests
    {

        JsonElement FromObject(object o)
        {
            return JsonDocument.Parse(JsonSerializer.Serialize(o)).RootElement;
        }

        [TestMethod]
        public void Should_validate_const_integer()
        {
            var s = JsonSchema.Parse(@"{ ""const"": 1 }");
            var o = FromObject(1);
            var r = JsonSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Should_fail_to_validate_const_integer()
        {
            var s = JsonSchema.Parse(@"{ ""const"": 1 }");
            var o = FromObject(2);
            var r = JsonSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeFalse();
        }

        [TestMethod]
        public void Should_validate_single_property_with_const()
        {
            var s = JsonSchema.Parse(@"{ ""properties"": { ""Prop"": { ""const"": 1 }  } }");
            var o = FromObject(new { Prop = 1 });
            var r = JsonSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Should_fail_to_validate_single_property_with_const()
        {
            var s = JsonSchema.Parse(@"{ ""properties"": { ""Prop"": { ""const"": 1 }  } }");
            var o = FromObject(new { Prop = 2 });
            var r = JsonSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeFalse();
        }

        [TestMethod]
        public void Should_skip_validating_single_property_with_const()
        {
            var s = JsonSchema.Parse(@"{ ""properties"": { ""Prop1"": { ""const"": 1 }  } }");
            var o = FromObject(new { Prop2 = 2 });
            var r = JsonSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Should_validate_recursive_ref()
        {
            var s = JsonSchema.Parse(@"{ ""properties"": { ""Prop1"": { ""$ref"": ""#"" }, ""Prop2"": { ""const"": ""value"" } } }");
            var o = FromObject(new { Prop1 = new { Prop1 = (string)null } });
            var r = JsonSchemaExpressionBuilder.CreateDefault().Build(s).Compile().Invoke(o);
            r.Should().BeTrue();
        }

        [TestMethod]
        public void Can_load_really_big_schema()
        {
            var s = JsonSchema.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(JSchemaValidatorBuilderTests).Assembly.Location), "Validation", "ecourt_com_151.json")));
            var o = JsonDocument.Parse(File.ReadAllText(Path.Combine(Path.GetDirectoryName(typeof(JSchemaValidatorBuilderTests).Assembly.Location), "Validation", "efm.json"))).RootElement;
            var v = JsonSchemaExpressionBuilder.CreateDefault().Build(s);

            var a = v.Compile();
            a.Invoke(o);
            var sw = new Stopwatch();

            var t = TimeSpan.Zero;
            for (var i = 0; i < 1000; i++)
            {
                sw.Start();
                var r = a.Invoke(o);
                sw.Stop();
                t += sw.Elapsed;
                sw.Reset();
            }
            Console.WriteLine("Average on Fast Validator: " + new TimeSpan((long)(t.Ticks / 1000d)));
        }

    }

}
