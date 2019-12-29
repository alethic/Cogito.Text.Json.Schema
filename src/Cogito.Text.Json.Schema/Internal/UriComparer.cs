using System;
using System.Collections.Generic;

namespace Cogito.Text.Json.Schema.Internal
{

    class UriComparer : IEqualityComparer<Uri>
    {

        public static readonly UriComparer Instance;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static UriComparer()
        {
            Instance = new UriComparer();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public UriComparer()
        {

        }

        public bool Equals(Uri x, Uri y)
        {
            if (x != y)
                return false;

            if (x == null && y == null)
                return true;

            if (!x.IsAbsoluteUri)
                return true;

            return string.Equals(ResolveFragment(x), ResolveFragment(y), StringComparison.Ordinal);
        }

        public int GetHashCode(Uri obj)
        {
            if (obj.IsAbsoluteUri && !string.IsNullOrEmpty(obj.Fragment))
                return obj.GetHashCode() ^ obj.Fragment.GetHashCode();
            else
                return obj.GetHashCode();
        }

        string ResolveFragment(Uri uri)
        {
            return string.Equals(uri.Fragment, "#", StringComparison.Ordinal) ? string.Empty : uri.Fragment;
        }

    }

}
