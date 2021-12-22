# Components Web.JS

This directory contains the source files for Blazor's client-side JavaScript.

## Type Declarations

We leverage the `@microsoft/api-extractor` node package to create the public type declaration files for Blazor. The `api-extractor` is also responsible for "rolling-in" dependencies like `@microsoft/dotnet-js-interop` into our declarations, and ensuring `@internal` marked types are excluded from the declarations we ship publicly.

The `api-extractor` is configured within the `api-extractor.json` file and is run via:

```bash
api-extractor run --local --verbose
```

note, the `yarn build` script also runs the `api-extractor`.

More details on the `api-extractor` can be found [here](https://api-extractor.com/).
