using System.Runtime.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;

namespace Models.Plugin.V1 {

public class  Response {

    [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
    public enum ResponseType {
      [EnumMember(Value="info")]
      Info,
      [EnumMember(Value="methods")]
      Methods,
      [EnumMember(Value="fetch")]
      Fetch,
      [EnumMember(Value="error")]
      Error
    }

    [JsonProperty(Required = Required.Always)]
    public required String ApiVersion { get; set; }

    [JsonProperty(Required = Required.Always)]
    public ResponseType Type { get; set; }

    [JsonProperty(Required = Required.Default)]
    public FetchResponse? Fetch { get; set; }

    [JsonProperty(Required = Required.Default)]
    public MethodsResponse? Methods { get; set; }

    [JsonProperty(Required = Required.Default)]
    public InfoResponse? Info{ get; set; }

    [JsonProperty(Required = Required.Default)]
    public ErrorResponse? Error { get; set; }

    public override string ToString()
    {
      return $"Response(Version={SchemaGenerator.VERSION}, Type={Type}, Fetch={Fetch}, Methods={Methods}, Info={Info})";
    }

    public static JSchema Generate() {

          JSchemaGenerator generator = new JSchemaGenerator();
          generator.GenerationProviders.Add(new StringEnumGenerationProvider());
          generator.ContractResolver = new CamelCasePropertyNamesContractResolver();
          JSchema schema = generator.Generate(typeof(Response));

          schema.AllOf.Add(new JSchema
          {
              If = new JSchema
              {
                  Properties = { ["type"] = new JSchema { Enum = { JToken.FromObject(Response.ResponseType.Fetch) } } }
              },
              Then = new JSchema
              {
                  Required = { "fetch" }
              }
          });
          schema.AllOf.Add(new JSchema
          {
              If = new JSchema
              {
                  Properties = { ["type"] = new JSchema { Enum = { JToken.FromObject(Response.ResponseType.Methods) } } }
              },
              Then = new JSchema
              {
                  Required = { "methods" }
              }
          });
          schema.AllOf.Add(new JSchema
          {
              If = new JSchema
              {
                  Properties = { ["type"] = new JSchema { Enum = { JToken.FromObject(Response.ResponseType.Info) } } }
              },
              Then = new JSchema
              {
                  Required = { "info" }
              }
          });


          schema.Properties["type"].Description = "The Type of Response";
          schema.Properties["fetch"].Description = "Fetch object";
          schema.Properties["methods"].Description = "Methods object";
          schema.Properties["info"].Description = "Info object";

          schema.Id = new Uri(SchemaGenerator.SCHEMA_HOME + "/ResponseSchema.json");

          return schema;

        }

      public static Response? DeserializeJson(String json) {
        return JsonConvert.DeserializeObject<Response>(json);
      }

  }

  public class FetchResponse {

    public class MetaData {

      [JsonProperty(Required = Required.Always)]
      public required String Plugin { get; set; }

      [JsonProperty(Required = Required.Always)]
      public required String PluginVersion { get; set; }

      [JsonProperty(Required = Required.Always)]
      public DateTime FetchDate { get; set; } //YYYY-MM-DDTHH:mm:ssZ

    }

    public class Clue {

      [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
      public enum Direction {
        [EnumMember(Value="across")]
        Across,
        [EnumMember(Value="down")]
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
      public required String Prompt { get; set; }

      [JsonProperty(Required = Required.Always)]
      public required String Answer { get; set; }

    }

    [JsonProperty(Required = Required.Always)]
    public required MetaData Meta { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int Columns { get; set; }

    [JsonProperty(Required = Required.Always)]
    public int Rows { get; set; }

    [JsonProperty(Required = Required.Always)]
    public required Clue[] Clues { get; set; }

    [JsonProperty(Required = Required.Always)]
    public required String Title { get; set; }

    [JsonProperty(Required = Required.Default)]
    public required String Author { get; set; }

    [JsonProperty(Required = Required.Default)]
    public DateTime ReleaseDate { get; set; }

  }

  public class MethodsResponse {

    public class Method {

      [JsonProperty(Required = Required.Always)]
      public required String Name { get; set; }

      [JsonProperty(Required = Required.Always)]
      public required String Description { get; set; }

      [JsonProperty(Required = Required.Always)]
      public required Argument[] Arguments { get; set; }

      public override string ToString()
      {
        String argumentsString = "";
        foreach ( Argument a in Arguments ) {
          argumentsString = argumentsString + "\n" + a.ToString();
        }
        return $"Method(Name={Name}, Description={Description}, Arguments={argumentsString}";
      }

    }

    public class Argument {

      [JsonProperty(Required = Required.Always)]
      public required String Name { get; set; }

      [JsonProperty(Required = Required.Always)]
      public required String Description { get; set; }

      [JsonProperty(Required = Required.Always)]
      public required String[] Constraints;  //maybe can just be part of description

      public override string ToString()
      {
        return $"Argument(Name={Name}, Description={Description}, Constraints={Constraints})";
      }

    }

    [JsonProperty(Required = Required.Default)]
    public required Method[] Methods { get; set; }

    public override string ToString()
    {
      String methodsString = "";
      foreach ( Method m in Methods ) {
        methodsString = methodsString + "\n" + m.ToString();
      }
      return $"MethodResponse(Methods={methodsString})";
    }

  }

  public class InfoResponse {

    [JsonProperty(Required = Required.Always)]
    public required String Name { get; set; }

    [JsonProperty(Required = Required.Always)]
    public required String Description { get; set; }

    [JsonProperty(Required = Required.Always)]
    public required String Version { get; set; }

    public override string ToString()
    {
      return $"InfoResponse(Name={Name}, Description={Description}, Version={Version}";
    }

  }

  public class ErrorResponse {

    [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
    public enum ErrorType {
      [EnumMember(Value="badRequest")]
      BadRequest,
      [EnumMember(Value="invalidArgs")]
      InvalidArgs,
      [EnumMember(Value="fetchFailed")]
      FetchFailed,
      [EnumMember(Value="criticalFailure")]
      CriticalFailure
    }

    [JsonProperty(Required = Required.Always)]
    public ErrorType Type { get; set; }

    [JsonProperty(Required = Required.Default)]
    public required String Message { get; set; }


    public override string ToString()
    {
      return $"ErrorResponse(Type={Type}, Message={Message}";
    }

  }

}
