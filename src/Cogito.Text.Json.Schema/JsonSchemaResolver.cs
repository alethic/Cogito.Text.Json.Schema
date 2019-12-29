using System;
using System.IO;

namespace Cogito.Text.Json.Schema
{

    public abstract class JsonSchemaResolver
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected JsonSchemaResolver()
        {

        }

        public virtual JsonSchemaReference ResolveSchemaReference(JsonSchemaResolveContext context)
        {
            return new JsonSchemaReference(
                ResolveBaseUri(context, out var str),
                str != null ? new Uri(str, UriKind.RelativeOrAbsolute) : null);
        }

        public abstract Stream GetSchemaResource(JsonSchemaResolveContext context, JsonSchemaReference reference);

        public virtual JsonSchema GetSubschema(JsonSchemaReference reference, JsonSchema root)
        {
            throw new NotImplementedException();
        }

        Uri RemoveFragment(Uri uri, out string fragment)
        {
            if (uri is null)
                throw new ArgumentNullException(nameof(uri));

            if (uri.IsAbsoluteUri && string.IsNullOrEmpty(uri.Fragment))
            {
                fragment = null;
                return uri;
            }

            var num = uri.OriginalString.IndexOf('#');
            string originalString;

            if (num == -1)
            {
                originalString = uri.OriginalString;
                fragment = null;
            }
            else
            {
                originalString = uri.OriginalString.Substring(0, num);
                fragment = uri.OriginalString.Substring(num);
            }

            return new Uri(originalString, UriKind.RelativeOrAbsolute);
        }

        Uri ResolveBaseUri(JsonSchemaResolveContext context, out string fragment)
        {
            Uri resolverBaseUri = context.ResolverBaseUri;
            Uri resolvedSchemaId = context.ResolvedSchemaId;
            if (!resolvedSchemaId.IsAbsoluteUri && resolvedSchemaId.OriginalString.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                fragment = resolvedSchemaId.OriginalString;
                return resolverBaseUri;
            }
            if (resolverBaseUri == null || !resolverBaseUri.IsAbsoluteUri && resolverBaseUri.OriginalString.Length == 0)
            {
                return RemoveFragment(resolvedSchemaId, out fragment);
            }
            resolvedSchemaId = RemoveFragment(resolvedSchemaId, out fragment);
            throw new NotImplementedException();
            //resolvedSchemaId = SchemaDiscovery.ResolveSchemaId(resolverBaseUri, resolvedSchemaId);
            return resolvedSchemaId;
        }

    }

}
