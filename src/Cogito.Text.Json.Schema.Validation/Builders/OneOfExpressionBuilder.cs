using System.Linq;
using System.Linq.Expressions;

namespace Cogito.Text.Json.Schema.Validation.Builders
{

    public class OneOfExpressionBuilder : ExpressionBuilderBase
    {

        public override Expression Build(JsonSchemaExpressionBuilder builder, JsonSchema schema, Expression token)
        {
            if (schema.OneOf.Count == 0)
                return null;

            return OneOf(schema.OneOf.Select(i => builder.Eval(i, token)));
        }

    }

}
