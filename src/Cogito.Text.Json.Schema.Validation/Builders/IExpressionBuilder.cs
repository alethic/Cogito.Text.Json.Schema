using System.Linq.Expressions;

namespace Cogito.Text.Json.Schema.Validation.Builders
{

    /// <summary>
    /// Provides <see cref="Expression"/> instances that contribute to validation.
    /// </summary>
    public interface IExpressionBuilder
    {

        /// <summary>
        /// Builds and returns an <see cref="Expression"/> that should be a condition of validity of the schema
        /// against the specified token.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="schema"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Expression Build(JsonSchemaExpressionBuilder builder, JsonSchema schema, Expression token);

    }

}