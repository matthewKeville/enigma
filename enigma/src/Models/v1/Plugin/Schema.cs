namespace Models.Plugin.V1 {

public static class SchemaGenerator {

    public const String VERSION = @"v1";
    public const String REPO_HOME = @"https://raw.githubusercontent.com/matthewKeville/enigma/master/enigma/";
    public const String SCHEMA_FOLDER = @"src/Models/"+VERSION+"/Plugin/Generated";
    public const String SCHEMA_HOME = REPO_HOME + SCHEMA_FOLDER;

    public static String RequestSchema = SCHEMA_FOLDER + @"/RequestSchema.json";
    public static String ResponseSchema = SCHEMA_FOLDER + @"/ResponseSchema.json";

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
