using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Cogito.Text.Json.Schema.Internal;

namespace Cogito.Text.Json.Schema
{

    public class JsonSchemaPreloadedResolver : JsonSchemaResolver
    {

        readonly Dictionary<Uri, byte[]> preloadedData;
        readonly JsonSchemaResolver resolver;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="resolver"></param>
        public JsonSchemaPreloadedResolver(JsonSchemaResolver resolver) :
            this()
        {
            this.resolver = resolver;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public JsonSchemaPreloadedResolver()
        {
            preloadedData = new Dictionary<Uri, byte[]>(UriComparer.Instance);
        }

        public override Stream GetSchemaResource(JsonSchemaResolveContext context, JsonSchemaReference reference)
        {
            return preloadedData.TryGetValue(reference.BaseUri, out var buffer) ? new MemoryStream(buffer) : resolver?.GetSchemaResource(context, reference);
        }

        public void Add(Uri uri, byte[] value)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            preloadedData[uri] = value;
        }

        public void Add(Uri uri, Stream value)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            var s = new MemoryStream();
            value.CopyTo(s);
            Add(uri, s.ToArray());
        }

        public void Add(Uri uri, string value)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("message", nameof(value));

            Add(uri, Encoding.UTF8.GetBytes(value));
        }

    }

}
