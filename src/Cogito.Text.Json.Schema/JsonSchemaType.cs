using System;

namespace Cogito.Text.Json.Schema
{

    [Flags]
    public enum JsonSchemaType
    {

        None = 0,
        String = 1,
        Number = 2,
        Integer = 4,
        Boolean = 8,
        Object = 16,
        Array = 32,
        Null = 64

    }

}
