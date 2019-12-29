using System;
using System.Text.RegularExpressions;

namespace Cogito.Text.Json.Schema.Internal
{

    class PatternSchema
    {

        Regex patternRegex;
        string patternError;

        public string Pattern { get; }

        public JsonSchema Schema { get; }

        public PatternSchema(string pattern, JsonSchema schema)
        {
            Pattern = pattern;
            Schema = schema;
        }

        internal bool TryGetPatternRegex(TimeSpan? matchTimeout, out Regex regex, out string errorMessage)
        {
            regex = null;
            errorMessage = null;
            return false;

            //bool flag = RegexHelpers.TryGetPatternRegex(Pattern, matchTimeout, ref patternRegex, ref patternError);
            //regex = patternRegex;
            //errorMessage = patternError;
            //return flag;
        }

    }

}
