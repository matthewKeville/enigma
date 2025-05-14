#r "nuget:System.Net.Http"
#r "nuget:System.IO"
#r "nuget:System.ValueTuple"
#r "nuget:Newtonsoft.Json,13.0.3"
#r "nuget:NJsonSchema,11.3.2"
#r "nuget:NJsonSchema.CodeGeneration.CSharp,11.3.2"

using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

// Path.Combine is not exposed to csx, so we must use Path.Combine instead .. interesting
await GenerateSchemaClasses.Main();

public class GenerateSchemaClasses {

  readonly static string SCHEMAS_DIR = "./Schemas/";
  readonly static string MODELS_DIR = "./src/Models/Generated/";

  public static async Task Main() {

    string cwd = Directory.GetCurrentDirectory();
    if ( Directory.GetFiles(cwd,"enigma.csproj").Length == 0 ) {
      System.Console.Error.WriteLine("execute this from engima project root");
      Environment.Exit(1);
    }

    Dictionary<string/*name*/, string/*url*/> schemaUrls = new Dictionary<string, string>();
    schemaUrls.Add("fetch/fetch-request-body-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/fetch/fetch-request-body-schema.json");
    schemaUrls.Add("fetch/fetch-response-body-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/fetch/fetch-response-body-schema.json");
    schemaUrls.Add("info/info-response-body-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/info/info-response-body-schema.json");
    schemaUrls.Add("methods/methods-response-body-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/methods/methods-response-body-schema.json");
    schemaUrls.Add("methods/method-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/methods/method-schema.json");
    schemaUrls.Add("error-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/error-schema.json");
    schemaUrls.Add("puzzle-data-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/puzzle-data-schema.json");
    schemaUrls.Add("request-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/request-schema.json");
    schemaUrls.Add("response-schema.json","https://raw.githubusercontent.com/matthewKeville/enigma-puzzle-fetcher-schema/refs/heads/main/schemas/response-schema.json");

    async Task DownloadSchema(string url, string path) {
      HttpClient client = new HttpClient();
      HttpResponseMessage result = await client.GetAsync(url);
      String content = await result.Content.ReadAsStringAsync();
      String directory = Path.GetDirectoryName(Path.Combine(SCHEMAS_DIR,path));
      String filePath = Path.Combine(SCHEMAS_DIR,path);
      Directory.CreateDirectory(directory);
      File.WriteAllText(filePath,content);
    }

    //download schemas to filesystem
    foreach ( var entry in schemaUrls ) {
      await DownloadSchema(entry.Value,entry.Key);
    }

    //load root schemas into JsonSchema
    JsonSchema requestSchema  = await JsonSchema.FromFileAsync(Path.Combine(SCHEMAS_DIR,"request-schema.json"));
    JsonSchema responseSchema  = await JsonSchema.FromFileAsync(Path.Combine(SCHEMAS_DIR,"response-schema.json"));

    //load schema roots into synthetic root
    JsonSchema syntheticRoot = new JsonSchema {
      Type = JsonObjectType.Object,
    };
    syntheticRoot.Properties.Add("request", new JsonSchemaProperty { Reference = requestSchema } );
    syntheticRoot.Properties.Add("response", new JsonSchemaProperty { Reference = responseSchema } );

    //generate cs file
    var generator = new CSharpGenerator(syntheticRoot, new CSharpGeneratorSettings
    {
        Namespace = "Fetcher.Models"
    });
    var file = generator.GenerateFile();
    String csFilePath = MODELS_DIR+"Models.cs";
    Directory.CreateDirectory(Path.GetDirectoryName(csFilePath));
    File.WriteAllText(csFilePath, file);

    //cleanup json schemas
    Directory.Delete(SCHEMAS_DIR,true);

  }

}

