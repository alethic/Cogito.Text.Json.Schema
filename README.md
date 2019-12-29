# Cogito.Json.Schema
JSON schema support for System.Text.Json.

## Cogito.Json.Schema.Validation

Contains a fast validator for JSON schema, on top of `System.Text.Json`. The validator is implemented as an `Expression` tree builder. It takes a `JsonSchema` object, and generates an `Expression` that implements the validation when invoked against a `JsonElement` from `System.Text.Json`.

