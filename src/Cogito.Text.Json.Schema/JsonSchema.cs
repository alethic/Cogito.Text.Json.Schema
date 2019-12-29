using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cogito.Text.Json.Schema
{

    [JsonConverter(typeof(JsonSchemaConverter))]
    public class JsonSchema
    {

        public static JsonSchema Parse(string value, JsonSerializerOptions options = null, JsonSchemaResolver resolver = null)
        {
            return JsonSerializer.Deserialize<JsonSchema>(value, options);
        }

        public static JsonSchema Load(ref Utf8JsonReader reader, JsonSerializerOptions options = null, JsonSchemaResolver resolver = null)
        {
            return JsonSerializer.Deserialize<JsonSchema>(ref reader, options);
        }

        public static async ValueTask<JsonSchema> LoadAsync(StreamReader reader, JsonSerializerOptions options = null, JsonSchemaResolver resolver = null)
        {
            return JsonSerializer.Deserialize<JsonSchema>(await reader.ReadToEndAsync(), options);
        }

        public static JsonSchema Load(StreamReader reader, JsonSerializerOptions options = null, JsonSchemaResolver resolver = null)
        {
            return JsonSerializer.Deserialize<JsonSchema>(reader.ReadToEnd(), options);
        }

        public static ValueTask<JsonSchema> LoadAsync(Stream stream, JsonSerializerOptions options = null, JsonSchemaResolver resolver = null)
        {
            return JsonSerializer.DeserializeAsync<JsonSchema>(stream, options);
        }

        public static JsonSchema Load(Stream stream, JsonSerializerOptions options = null, JsonSchemaResolver resolver = null)
        {
            return Load(new StreamReader(stream), options, resolver);
        }

        JsonSchemaCollection allOf;
        JsonSchemaCollection anyOf;
        JsonSchemaCollection oneOf;
        JsonSchemaDependencyDictionary dependencies;
        List<JsonElement> @enum;
        IDictionary<string, JsonElement> extensionData;
        JsonSchemaCollection items;
        JsonSchemaDictionary patternProperties;
        JsonSchemaDictionary properties;
        List<string> required;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public JsonSchema()
        {
            AllowAdditionalProperties = true;
            AllowAdditionalItems = true;
        }

        /// <summary>
        /// Gets the given target value or creates it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="make"></param>
        /// <returns></returns>
        T GetOrCreate<T>(ref T target, Func<T> make)
        {
            if (target == null)
                target = make();

            return target;
        }

        public JsonSchema AdditionalItems { get; set; }

        public JsonSchema AdditionalProperties { get; set; }

        public IList<JsonSchema> AllOf => GetOrCreate(ref allOf, () => new JsonSchemaCollection(this));

        public bool AllowAdditionalItems { get; set; }

        public bool AllowAdditionalProperties { get; set; }

        public IList<JsonSchema> AnyOf => GetOrCreate(ref anyOf, () => new JsonSchemaCollection(this));

        public JsonElement? Const { get; set; }

        public JsonSchema Contains { get; set; }

        public string ContentEncoding { get; set; }

        public string ContentMediaType { get; set; }

        public JsonElement? Default { get; set; }

        public IDictionary<string, object> Dependencies => GetOrCreate(ref dependencies, () => new JsonSchemaDependencyDictionary(this));

        bool DeprecatedRequired { get; set; }

        public string Description { get; set; }

        public JsonSchema Else { get; set; }

        public IList<JsonElement> Enum => GetOrCreate(ref @enum, () => new List<JsonElement>());

        public bool ExclusiveMaximum { get; set; }

        public bool ExclusiveMinimum { get; set; }

        public IDictionary<string, JsonElement> ExtensionData => GetOrCreate(ref extensionData, () => new Dictionary<string, JsonElement>(StringComparer.Ordinal));

        public string Format { get; set; }

        public Uri Id { get; set; }

        public JsonSchema If { get; set; }

        public IList<JsonSchema> Items => GetOrCreate(ref items, () => new JsonSchemaCollection(this));

        public bool ItemsPositionValidation { get; set; }

        public double? Maximum { get; set; }

        public long? MaximumItems { get; set; }

        public long? MaximumLength { get; set; }

        public long? MaximumProperties { get; set; }

        public double? Minimum { get; set; }

        public long? MinimumItems { get; set; }

        public long? MinimumLength { get; set; }

        public long? MinimumProperties { get; set; }

        public double? MultipleOf { get; set; }

        public JsonSchema Not { get; set; }

        public IList<JsonSchema> OneOf => GetOrCreate(ref oneOf, () => new JsonSchemaCollection(this));

        public string Pattern { get; set; }

        public IDictionary<string, JsonSchema> PatternProperties => GetOrCreate(ref patternProperties, () => new JsonSchemaDictionary(this));

        public IDictionary<string, JsonSchema> Properties => GetOrCreate(ref properties, () => new JsonSchemaDictionary(this));

        public JsonSchema PropertyNames { get; set; }

        public bool? ReadOnly { get; set; }

        public Uri Reference { get; set; }

        public IList<string> Required => GetOrCreate(ref required, () => new List<string>());

        public Uri SchemaVersion { get; set; }

        public JsonSchema Then { get; set; }

        public string Title { get; set; }

        public JsonSchemaType? Type { get; set; }

        public bool UniqueItems { get; set; }

        public bool? Valid { get; set; }

        public bool? WriteOnly { get; set; }

    }

}
