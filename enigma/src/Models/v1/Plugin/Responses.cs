using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace Models.Plugin.V1 {

public class  Response {

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResponseType {
      Info,
      Methods,
      Fetch
    }

    [JsonProperty(Required = Required.Always)]
    public String APIVersion { get; set; }

    [JsonProperty(Required = Required.Always)]
    public ResponseType Type { get; set; }

    [JsonProperty(Required = Required.Default)]
    public FetchResponse Fetch { get; set; }

    [JsonProperty(Required = Required.Default)]
    public MethodsResponse Methods { get; set; }

    [JsonProperty(Required = Required.Default)]
    public InfoResponse Info{ get; set; }

    [JsonProperty(Required = Required.Default)]
    public ErrorResponse Error { get; set; }

    public override string ToString()
    {
      return $"Response(Version={SchemaGenerator.VERSION}, Type={Type}, Fetch={Fetch}, Methods={Methods}, Info={Info})";
    }

    public static JSchema Generate() {

          JSchemaGenerator generator = new JSchemaGenerator();
          generator.GenerationProviders.Add(new StringEnumGenerationProvider());
          JSchema schema = generator.Generate(typeof(Response));

          schema.AllOf.Add(new JSchema
          {
              If = new JSchema
              {
                  Properties = { ["Type"] = new JSchema { Enum = { JToken.FromObject(Response.ResponseType.Fetch) } } }
              },
              Then = new JSchema
              {
                  Required = { "Fetch" }
              }
          });
          schema.AllOf.Add(new JSchema
          {
              If = new JSchema
              {
                  Properties = { ["Type"] = new JSchema { Enum = { JToken.FromObject(Response.ResponseType.Methods) } } }
              },
              Then = new JSchema
              {
                  Required = { "Methods" }
              }
          });
          schema.AllOf.Add(new JSchema
          {
              If = new JSchema
              {
                  Properties = { ["Type"] = new JSchema { Enum = { JToken.FromObject(Response.ResponseType.Info) } } }
              },
              Then = new JSchema
              {
                  Required = { "Info" }
              }
          });


          schema.Properties["Type"].Description = "The Type of Response";
          schema.Properties["Fetch"].Description = "Fetch object";
          schema.Properties["Methods"].Description = "Methods object";
          schema.Properties["Info"].Description = "Info object";

          schema.Id = new Uri(SchemaGenerator.SCHEMA_HOME + "/ResponseSchema.json");

          return schema;

        }

      public static Response DeserializeJson(String json) {
        //Trace.WriteLine("");
        return JsonConvert.DeserializeObject<Response>(json);
      }

  }

  public class FetchResponse {

    public class MetaData {

      [JsonProperty(Required = Required.Always)]
      public String Plugin { get; set; }

      [JsonProperty(Required = Required.Always)]
      public String PluginVersion { get; set; }

      [JsonProperty(Required = Required.Always)]
      public DateTime FetchDate { get; set; } //YYYY-MM-DDTHH:mm:ssZ

    }

    public class Clue {

      [JsonConverter(typeof(StringEnumConverter))]
      public enum Direction {
        Across,
        Down,
      }

      [JsonProperty(Required = Required.Always)]
      public int X { get; set; }

      [JsonProperty(Required = Required.Always)]
      public int Y { get; set; }

      [JsonProperty(Required = Required.Always)]
      public int I { get; set; }

      [JsonProperty(Required = Required.Always)]
      public Direction D { get; set; }

      [JsonProperty(Required = Required.Always)]
      public String Prompt { get; set; }

      [JsonProperty(Required = Required.Always)]
      public String Answer { get; set; }

    }

    [JsonProperty(Required = Required.Always)]
    public MetaData Meta { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int Columns { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int Rows { get; set; }

    [JsonProperty(Required = Required.Always)]
    public Clue[] Clues { get; set; }

    [JsonProperty(Required = Required.Always)]
    public String Title { get; set; }

    [JsonProperty(Required = Required.Default)]
    public String Author { get; set; }

    [JsonProperty(Required = Required.Always)]
    public String ReleaseDate { get; set; }

  }

  public class MethodsResponse {

    public class Method {

      [JsonProperty(Required = Required.Always)]
      public String Name { get; set; }

      [JsonProperty(Required = Required.Always)]
      public String Description { get; set; }

      [JsonProperty(Required = Required.Always)]
      public Argument[] Arguments { get; set; }

    }

    public class Argument {

      [JsonProperty(Required = Required.Always)]
      public String Name { get; set; }

      [JsonProperty(Required = Required.Always)]
      public String Description { get; set; }

      [JsonProperty(Required = Required.Always)]
      public String[] Constraints;  //maybe can just be part of description

    }

    [JsonProperty(Required = Required.Default)]
    public Method[] Methods { get; set; }

  }

  public class InfoResponse {

    [JsonProperty(Required = Required.Always)]
    public String Name { get; set; }

    [JsonProperty(Required = Required.Always)]
    public String Description { get; set; }

    [JsonProperty(Required = Required.Always)]
    public String Version { get; set; }

  }

  public class ErrorResponse {

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ErrorType {
      BadRequest,
      InvalidArgs,
      FetchFailed,
      CriticalFailure
    }

    [JsonProperty(Required = Required.Always)]
    public ErrorType Type { get; set; }

    [JsonProperty(Required = Required.Default)]
    public String errorMessage { get; set; }

  }

}
