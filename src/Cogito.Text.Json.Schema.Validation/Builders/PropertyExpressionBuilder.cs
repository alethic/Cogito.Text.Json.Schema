using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Cogito.Text.Json.Schema.Validation.Builders
{

    public class PropertyExpressionBuilder : ExpressionBuilderBase
    {

        public override Expression Build(JsonSchemaExpressionBuilder builder, JsonSchema schema, Expression token)
        {
            return BuildProperties(builder, schema, token);
        }

        Expression BuildProperties(JsonSchemaExpressionBuilder builder, JsonSchema schema, Expression o)
        {
            return AllOf(BuildPropertiesAll(builder, schema, o).Where(i => i != null));
        }

        IEnumerable<Expression> BuildPropertiesAll(JsonSchemaExpressionBuilder builder, JsonSchema schema, Expression o)
        {
            if (schema.Properties.Count > 0)
                yield return IfThenElseTrue(
                    IsTokenType(o, JsonValueKind.Object),
                    AllOf(schema.Properties.Select(i =>
                        BuildProperty(builder, i.Key, i.Value, Expression.Convert(o, typeof(JsonElement))))));

            if (schema.PatternProperties.Count > 0)
                yield return IfThenElseTrue(
                    IsTokenType(o, JsonValueKind.Object),
                        AllOf(schema.PatternProperties.Select(i =>
                            BuildPatternProperty(builder, i.Key, i.Value, Expression.Convert(o, typeof(JsonElement))))));

            if (schema.AllowAdditionalProperties == false)
            {
                yield return IfThenElseTrue(
                    IsTokenType(o, JsonValueKind.Object),
                    CallThis(
                        nameof(AllowAdditionalProperties),
                        Expression.Constant(schema),
                        Expression.Convert(o, typeof(JsonElement))));
            }
            else if (schema.AdditionalProperties != null)
            {
                var p = Expression.Parameter(typeof(JsonElement));

                yield return IfThenElseTrue(
                    IsTokenType(o, JsonValueKind.Object),
                    CallThis(
                        nameof(AdditionalProperties),
                        Expression.Constant(schema),
                        Expression.Convert(o, typeof(JsonElement)),
                        EvalSchemaFunc(builder, schema.AdditionalProperties)));
            }
        }

        Expression BuildProperty(JsonSchemaExpressionBuilder builder, string propertyName, JsonSchema propertySchema, Expression o)
        {
            return CallThis(nameof(Property), Expression.Constant(propertyName), EvalSchemaFunc(builder, propertySchema), o);
        }

        static bool Property(string propertyName, Func<JsonElement, bool> propertySchema, JsonElement o)
        {
            if (o.TryGetProperty(propertyName, out var p))
                return propertySchema(p);

            return true;
        }

        Expression BuildPatternProperty(JsonSchemaExpressionBuilder builder, string propertyPattern, JsonSchema propertySchema, Expression o)
        {
            return CallThis(nameof(PatternProperty), Expression.Constant(propertyPattern), EvalSchemaFunc(builder, propertySchema), o);
        }

        static bool PatternProperty(string propertyPattern, Func<JsonElement, bool> propertySchema, JsonElement o)
        {
            foreach (var p in o.EnumerateObject())
                if (Regex.IsMatch(p.Name, propertyPattern))
                    if (!propertySchema(p.Value))
                        return false;

            return true;
        }

        static bool AllowAdditionalProperties(JsonSchema schema, JsonElement o)
        {
            foreach (var p in o.EnumerateObject())
                if (schema.Properties.ContainsKey(p.Name) == false &&
                    schema.PatternProperties.Any(i => Regex.IsMatch(p.Name, i.Key)) == false)
                    return false;

            return true;
        }

        static bool AdditionalProperties(JsonSchema schema, JsonElement o, Func<JsonElement, bool> additionalPropertiesSchema)
        {
            foreach (var p in o.EnumerateObject())
                if (schema.Properties.ContainsKey(p.Name) == false &&
                    schema.PatternProperties.Any(i => Regex.IsMatch(p.Name, i.Key)) == false)
                    if (additionalPropertiesSchema(p.Value) == false)
                        return false;

            return true;
        }

    }

}
