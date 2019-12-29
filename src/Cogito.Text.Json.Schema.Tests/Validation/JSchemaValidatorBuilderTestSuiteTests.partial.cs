using System;
using System.IO;
using System.Text;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace Cogito.Text.Json.Schema.Tests.Validation
{

    public partial class JSchemaValidatorBuilderTestSuiteTests
    {

        /// <summary>
        /// Resolves from the test suite remote directory
        /// </summary>
        class JSchemaRemoteResolver : JsonSchemaResolver
        {

            readonly string baseDir;
            readonly JsonSchemaPreloadedResolver nested;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="baseDir"></param>
            public JSchemaRemoteResolver(string baseDir)
            {
                this.baseDir = baseDir ?? throw new ArgumentNullException(nameof(baseDir));

                nested = new JsonSchemaPreloadedResolver();
                nested.Add(new Uri("http://json-schema.org/draft-03/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-03.json")));
                nested.Add(new Uri("http://json-schema.org/draft-04/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-04.json")));
                nested.Add(new Uri("http://json-schema.org/draft-06/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-06.json")));
                nested.Add(new Uri("http://json-schema.org/draft-07/schema"), File.ReadAllText(Path.Combine(baseDir, @"schema-draft-07.json")));
            }

            public override Stream GetSchemaResource(JsonSchemaResolveContext context, JsonSchemaReference reference)
            {
                if (reference.BaseUri.Host == "localhost")
                    return File.OpenRead(Path.Combine(baseDir, @"JSON-Schema-Test-Suite", "remotes", reference.BaseUri.LocalPath.Trim('/').Replace('/', '\\')));

                return nested.GetSchemaResource(context, reference);
            }

        }

        /// <summary>
        /// Gets the test context.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static JSchemaValidatorBuilderTestSuiteTests()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                DateParseHandling = DateParseHandling.None,
            };
        }

        /// <summary>
        /// Resolves from the remote directory.
        /// </summary>
        static JsonSchemaResolver Resolver { get; } = new JSchemaRemoteResolver(Path.Combine(Path.GetDirectoryName(typeof(JSchemaValidatorBuilderTestSuiteTests).Assembly.Location), "Validation"));

        /// <summary>
        /// Parses the given token string value from base64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static JsonElement ParseElement(string value)
        {
            return JsonDocument.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(value))).RootElement;
        }

        /// <summary>
        /// Parses the given schema string value from base64.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static JsonSchema ParseSchema(string value)
        {
            return JsonSchema.Load(new MemoryStream(Convert.FromBase64String(value)), resolver: Resolver);
        }

    }

}
