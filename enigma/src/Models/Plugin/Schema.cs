using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Plugin;
using System.IO;

namespace Schema {

public static class SchemaGenerator {

    public const String VERSION = @"v1";
    public const String SCHEMA_HOME = @"https://raw.githubusercontent.com/org/repo/main/schemas/" + VERSION;

    public static String SchemaFolder = @"src/Models/Generated/";
    public static String RequestSchema = SchemaFolder + @"RequestSchema.json";
    public static String ResponseSchema = SchemaFolder + @"ResponseSchema.json";

    public static void Generate() {
      Console.WriteLine("Generating Request Schema at " + RequestSchema);
      WriteSchemaToFile(RequestSchema,Request.Generate().ToString());
      Console.WriteLine("Generating Response Schema at " + ResponseSchema);
      WriteSchemaToFile(ResponseSchema,Response.Generate().ToString());
    }

    public static void WriteSchemaToFile(String path,String json) {
      File.WriteAllText(path, json);
    }
  }

}
