using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Cogito.Text.Json.Schema
{

    public class JsonSchemaReader
    {

        readonly JsonSchemaReaderSettings settings;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="settings"></param>
        public JsonSchemaReader(JsonSchemaReaderSettings settings = null)
        {
            this.settings = settings ?? JsonSchemaReaderSettings.Default;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="settings"></param>
        public JsonSchemaReader(JsonSchemaResolver resolver) :
            this(new JsonSchemaReaderSettings() { Resolver = resolver })
        {

        }

        /// <summary>
        /// Reads a schema starting at the specified element.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public JsonSchema Read(JsonElement source)
        {
            var top = new JsonSchema();
            var ctx = new JsonSchemaReaderContext(this, top);
            Load(ctx, top, source);
            return top;
        }

        /// <summary>
        /// Reads a schema from the specified document.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public JsonSchema Read(JsonDocument source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Read(source.RootElement);
        }

        /// <summary>
        /// Reads a schema from the specified reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public JsonSchema Read(ref Utf8JsonReader reader)
        {
            return Read(JsonDocument.ParseValue(ref reader).RootElement);
        }

        /// <summary>
        /// Reads a schema from the specified span of bytes.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public JsonSchema Read(ReadOnlySpan<byte> source, Encoding encoding)
        {
            if (encoding == Encoding.UTF8)
            {
                var rdr = new Utf8JsonReader(source);
                return Read(ref rdr);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Reads a schema from the specified span of bytes.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public JsonSchema Read(ReadOnlySequence<byte> source, Encoding encoding)
        {
            if (encoding == Encoding.UTF8)
            {
                var rdr = new Utf8JsonReader(source);
                return Read(ref rdr);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Reads a schema from the specified <see cref="Stream"/> encoded with the specified encoding.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public JsonSchema Read(Stream source, Encoding encoding)
        {
            if (encoding == Encoding.UTF8)
            {
                var buf = new MemoryStream();
                source.CopyTo(buf);
                var rdr = new Utf8JsonReader(buf.ToArray().AsSpan());
                return Read(ref rdr);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Reads a schema from the specified <see cref="StreamReader"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public JsonSchema Read(StreamReader source)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a new schema within the same context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        JsonSchema Read(JsonSchemaReaderContext context, JsonElement source)
        {
            var schema = new JsonSchema();
            Load(context, schema, source);
            return schema;
        }

        /// <summary>
        /// Reads a new schema within the same context.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        JsonSchema ReadOrNull(JsonSchemaReaderContext context, JsonElement source)
        {
            return source.ValueKind switch
            {
                JsonValueKind.Null => null,
                JsonValueKind.Object => Read(context, source),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        /// <summary>
        /// Loads a <see cref="JsonSchema"/> starting at the given element.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="schema"></param>
        /// <param name="source"></param>
        void Load(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source)
        {
            switch (source.ValueKind)
            {
                case JsonValueKind.True:
                    schema.Valid = true;
                    return;
                case JsonValueKind.False:
                    schema.Valid = false;
                    return;
                case JsonValueKind.Object:
                    foreach (var property in source.EnumerateObject())
                        LoadProperty(context, schema, source, property);
                    break;
                default:
                    throw new JsonSchemaReaderException("Error reading schema.");
            }
        }

        /// <summary>
        /// Processes a schema object property and loads the schema object.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="schema"></param>
        /// <param name="source"></param>
        /// <param name="property"></param>
        void LoadProperty(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonProperty property)
        {
            switch (property.Name)
            {
                case "additionalItems":
                    LoadAdditionalItems(context, schema, source, property.Value);
                    break;
                case "additionalProperties":
                    LoadAdditionalProperties(context, schema, source, property.Value);
                    break;
                case "allOf":
                    LoadAllOf(context, schema, source, property.Value);
                    break;
                case "anyOf":
                    LoadAnyOf(context, schema, source, property.Value);
                    break;
                case "const":
                    LoadConst(context, schema, source, property.Value);
                    break;
                case "contains":
                    LoadContains(context, schema, source, property.Value);
                    break;
                case "contentEncoding":
                    LoadContentEncoding(context, schema, source, property.Value);
                    break;
                case "contentMediaType":
                    LoadContentMediaType(context, schema, source, property.Value);
                    break;
                case "default":
                    LoadDefault(context, schema, source, property.Value);
                    break;
                case "dependencies":
                    LoadDependencies(context, schema, source, property.Value);
                    break;
                case "description":
                    LoadDescription(context, schema, source, property.Value);
                    break;
                case "disallow":
                    LoadDisallow(context, schema, source, property.Value);
                    break;
                case "divisibleBy":
                    LoadDivisibleBy(context, schema, source, property.Value);
                    break;
                case "else":
                    LoadElse(context, schema, source, property.Value);
                    break;
                case "enum":
                    LoadEnum(context, schema, source, property.Value);
                    break;
                case "exclusiveMaximum":
                    LoadExclusiveMaximum(context, schema, source, property.Value);
                    break;
                case "exclusiveMinimum":
                    LoadExclusiveMinimum(context, schema, source, property.Value);
                    break;
                case "format":
                    LoadFormat(context, schema, source, property.Value);
                    break;
                case "$id":
                    LoadId(context, schema, source, property.Value);
                    break;
                case "if":
                    LoadIf(context, schema, source, property.Value);
                    break;
                case "items":
                    LoadItems(context, schema, source, property.Value);
                    break;
                case "maximum":
                    LoadMaximum(context, schema, source, property.Value);
                    break;
                case "maximumItems":
                    LoadMaximumItems(context, schema, source, property.Value);
                    break;
                case "maximumLength":
                    LoadMaximumLength(context, schema, source, property.Value);
                    break;
                case "maximumProperties":
                    LoadMaximumProperties(context, schema, source, property.Value);
                    break;
                case "minimum":
                    LoadMinimum(context, schema, source, property.Value);
                    break;
                case "minimumItems":
                    LoadMinimumItems(context, schema, source, property.Value);
                    break;
                case "minimumLength":
                    LoadMinimumLength(context, schema, source, property.Value);
                    break;
                case "minimumProperties":
                    LoadMinimumProperties(context, schema, source, property.Value);
                    break;
                case "multipleOf":
                    LoadMultipleOf(context, schema, source, property.Value);
                    break;
                case "not":
                    LoadNot(context, schema, source, property.Value);
                    break;
                case "oneOf":
                    LoadOneOf(context, schema, source, property.Value);
                    break;
                case "pattern":
                    LoadPattern(context, schema, source, property.Value);
                    break;
                case "patternProperties":
                    LoadPatternProperties(context, schema, source, property.Value);
                    break;
                case "properties":
                    LoadProperties(context, schema, source, property.Value);
                    break;
                case "propertyNames":
                    LoadPropertyNames(context, schema, source, property.Value);
                    break;
                case "readOnly":
                    LoadReadOnly(context, schema, source, property.Value);
                    break;
                case "$ref":
                    LoadRef(context, schema, source, property.Value);
                    break;
                case "required":
                    LoadRequired(context, schema, source, property.Value);
                    break;
                case "$schema":
                    LoadSchema(context, schema, source, property.Value);
                    break;
                case "then":
                    LoadThen(context, schema, source, property.Value);
                    break;
                case "title":
                    LoadTitle(context, schema, source, property.Value);
                    break;
                case "type":
                    LoadType(context, schema, source, property.Value);
                    break;
                case "uniqueItems":
                    LoadUniqueItems(context, schema, source, property.Value);
                    break;
                case "valid":
                    LoadValid(context, schema, source, property.Value);
                    break;
                case "writeOnly":
                    LoadWriteOnly(context, schema, source, property.Value);
                    break;
                default:
                    LoadExtensionData(context, schema, source, property.Name, property.Value);
                    break;
            }
        }

        /// <summary>
        /// Loads a list of schemas into the specified array.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="list"></param>
        /// <param name="source"></param>
        void LoadSchemaListFromArray(JsonSchemaReaderContext context, IList<JsonSchema> list, JsonElement source)
        {
            list.Clear();

            switch (source.ValueKind)
            {
                case JsonValueKind.Array:
                    foreach (var i in source.EnumerateArray())
                        list.Add(Read(context, i));
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadAdditionalItems(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.AdditionalItems = null;

            switch (value.ValueKind)
            {
                case JsonValueKind.True:
                case JsonValueKind.False:
                    schema.AllowAdditionalItems = value.GetBoolean();
                    break;
                case JsonValueKind.Object:
                    schema.AllowAdditionalItems = true;
                    schema.AdditionalItems = Read(context, value);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadAdditionalProperties(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.AdditionalProperties = null;

            switch (value.ValueKind)
            {
                case JsonValueKind.True:
                case JsonValueKind.False:
                    schema.AllowAdditionalProperties = value.GetBoolean();
                    break;
                case JsonValueKind.Object:
                    schema.AllowAdditionalProperties = true;
                    schema.AdditionalProperties = Read(context, value);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadAllOf(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            LoadSchemaListFromArray(context, schema.AllOf, value);
        }

        void LoadAnyOf(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            LoadSchemaListFromArray(context, schema.AnyOf, value);
        }

        void LoadConst(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Const = value;
        }

        void LoadContains(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Contains = Read(context, value);
        }

        void LoadContentEncoding(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.ContentEncoding = value.ValueKind switch
            {
                JsonValueKind.String => value.GetString(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadContentMediaType(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.ContentMediaType = value.ValueKind switch
            {
                JsonValueKind.String => value.GetString(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadDefault(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Default = value;
        }

        void LoadDependencies(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Dependencies.Clear();

            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in value.EnumerateObject())
                        LoadDependency(context, schema, value, property);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadDependency(JsonSchemaReaderContext context, JsonSchema schema, JsonElement value, JsonProperty property)
        {
            switch (property.Value.ValueKind)
            {
                case JsonValueKind.String:
                    schema.Dependencies[property.Name] = new List<string>() { property.Value.GetString() };
                    break;
                case JsonValueKind.Array:
                    LoadPropertyDependency(context, schema, value, property.Name, property.Value);
                    break;
                case JsonValueKind.Object:
                    LoadSchemaDependency(context, schema, value, property.Name, property.Value);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadPropertyDependency(JsonSchemaReaderContext context, JsonSchema schema, JsonElement value, string name, JsonElement item)
        {
            schema.Dependencies[name] = item.EnumerateArray().Select(i => i.GetString()).ToList();
        }

        void LoadSchemaDependency(JsonSchemaReaderContext context, JsonSchema schema, JsonElement value1, string name, JsonElement item)
        {
            schema.Dependencies[name] = Read(context, item);
        }

        void LoadDescription(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Description = value.ValueKind switch
            {
                JsonValueKind.String => value.GetString(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadDisallow(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            throw new NotImplementedException();
        }

        void LoadDivisibleBy(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            LoadMultipleOf(context, schema, source, value);
        }

        void LoadElse(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Else = Read(context, value);
        }

        void LoadEnum(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Enum.Clear();

            switch (value.ValueKind)
            {
                case JsonValueKind.Array:
                    foreach (var i in value.EnumerateArray())
                        schema.Enum.Add(i);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadExclusiveMaximum(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.True:
                case JsonValueKind.False:
                    schema.ExclusiveMaximum = value.GetBoolean();
                    break;
                case JsonValueKind.Number:
                    schema.Maximum = value.GetDouble();
                    schema.ExclusiveMaximum = true;
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadExclusiveMinimum(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.True:
                case JsonValueKind.False:
                    schema.ExclusiveMinimum = value.GetBoolean();
                    break;
                case JsonValueKind.Number:
                    schema.Minimum = value.GetDouble();
                    schema.ExclusiveMinimum = true;
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadFormat(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Format = value.ValueKind switch
            {
                JsonValueKind.String => value.GetString(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadId(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Id = value.ValueKind switch
            {
                JsonValueKind.String => new Uri(value.GetString(), UriKind.RelativeOrAbsolute),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadIf(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.If = Read(context, value);
        }

        void LoadItems(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Items.Clear();

            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    schema.ItemsPositionValidation = false;
                    schema.Items.Add(Read(context, value));
                    break;
                case JsonValueKind.Array:
                    schema.ItemsPositionValidation = true;
                    foreach (var item in value.EnumerateArray())
                        schema.Items.Add(Read(context, item));
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadMaximum(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Maximum = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetDouble(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMaximumItems(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.MaximumItems = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetInt64(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMaximumLength(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.MaximumLength = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetInt64(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMaximumProperties(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.MaximumProperties = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetInt64(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMinimum(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Minimum = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetDouble(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMinimumItems(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.MinimumItems = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetInt64(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMinimumLength(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.MinimumLength = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetInt64(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMinimumProperties(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.MinimumProperties = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetInt64(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadMultipleOf(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.MultipleOf = value.ValueKind switch
            {
                JsonValueKind.Number => value.GetDouble(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadNot(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Not = Read(context, value);
        }

        void LoadOneOf(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            LoadSchemaListFromArray(context, schema.OneOf, value);
        }

        void LoadPattern(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Pattern = value.ValueKind switch
            {
                JsonValueKind.String => value.GetString(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadPatternProperties(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.PatternProperties.Clear();

            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in value.EnumerateObject())
                        schema.PatternProperties[property.Name] = Read(context, property.Value);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadProperties(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Properties.Clear();

            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var property in value.EnumerateObject())
                        schema.Properties[property.Name] = Read(context, property.Value);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadPropertyNames(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            throw new NotImplementedException();
        }

        void LoadReadOnly(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.ReadOnly = value.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadRef(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Reference = value.ValueKind switch
            {
                JsonValueKind.String => new Uri(value.GetString(), UriKind.RelativeOrAbsolute),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadRequired(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Required.Clear();

            switch (value.ValueKind)
            {
                case JsonValueKind.Array:
                    foreach (var i in value.EnumerateArray())
                        LoadRequiredItem(context, schema, i);
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadRequiredItem(JsonSchemaReaderContext context, JsonSchema schema, JsonElement value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.String:
                    schema.Required.Add(value.GetString());
                    break;
                default:
                    throw new JsonSchemaReaderException();
            }
        }

        void LoadSchema(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.SchemaVersion = value.ValueKind switch
            {
                JsonValueKind.Null => null,
                JsonValueKind.String => new Uri(value.GetString()),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadThen(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Then = Read(context, value);
        }

        void LoadTitle(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Title = value.ValueKind switch
            {
                JsonValueKind.Null => null,
                JsonValueKind.String => value.GetString(),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadType(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Type = GetTypeFromElement(value);
        }

        JsonSchemaType? GetTypeFromElement(JsonElement value)
        {
            return value.ValueKind switch
            {
                JsonValueKind.Null => null,
                JsonValueKind.String => GetTypeFromString(value.GetString()),
                JsonValueKind.Array => GetTypeFromArrayElement(value),
                _ => throw new JsonSchemaReaderException(),
            };
        }

        JsonSchemaType? GetTypeFromArrayElement(JsonElement value)
        {
            var t = (JsonSchemaType?)JsonSchemaType.None;

            foreach (var o in value.EnumerateArray())
                t |= o.ValueKind switch
                {
                    JsonValueKind.String => GetTypeFromString(o.GetString()),
                    _ => throw new JsonSchemaReaderException(),
                };

            return t;
        }

        JsonSchemaType? GetTypeFromString(string value)
        {
            return value switch
            {
                "array" => JsonSchemaType.Array,
                "boolean" => JsonSchemaType.Boolean,
                "integer" => JsonSchemaType.Integer,
                "none" => JsonSchemaType.None,
                "null" => JsonSchemaType.Null,
                "number" => JsonSchemaType.Number,
                "object" => JsonSchemaType.Object,
                "string" => JsonSchemaType.String,
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadUniqueItems(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.UniqueItems = value.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadValid(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.Valid = value.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadWriteOnly(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, JsonElement value)
        {
            schema.WriteOnly = value.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => throw new JsonSchemaReaderException(),
            };
        }

        void LoadExtensionData(JsonSchemaReaderContext context, JsonSchema schema, JsonElement source, string propertyName, JsonElement value)
        {
            schema.ExtensionData[propertyName] = value;
        }

    }

}
