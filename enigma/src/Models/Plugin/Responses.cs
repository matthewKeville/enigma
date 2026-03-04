using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace Plugin {

public class  Response {

    public enum ResponseType {
      Info,
      Methods,
      Fetch
    }

    [JsonProperty(Required = Required.Always)]
    public String Version { get; set; }

    [JsonProperty(Required = Required.Always)]
    public ResponseType Type { get; set; }

    //this should be required if requestType is Fetch
    [JsonProperty(Required = Required.Default)]
    public FetchResponse Fetch { get; set; }

    //this should be required if requestType is Methods
    [JsonProperty(Required = Required.Default)]
    public MethodsResponse Methods { get; set; }

    //this should be required if requestType is Methods
    [JsonProperty(Required = Required.Default)]
    public InfoResponse Info{ get; set; }

    public override string ToString()
    {
      return $"Response(Version={Version}, Type={Type}, Fetch={Fetch}, Methods={Methods}, Info={Info})";
    }

    public static JSchema Generate() {

          JSchemaGenerator generator = new JSchemaGenerator();
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

          schema.Id = new Uri(Schema.SchemaGenerator.SCHEMA_HOME + "/ResponseSchema.json");

          return schema;

        }

      public static Response DeserializeJson(String json) {
        //Trace.WriteLine("");
        return JsonConvert.DeserializeObject<Response>(json);
      }

  }

  /** Puzzle Date Goes In Here **/
  public class FetchResponse {

    [JsonProperty(Required = Required.Always)]
    String PuzzleData;

  }

  public class MethodsResponse {

    public class  Argument {

      [JsonProperty(Required = Required.Always)]
      public String Name;
      [JsonProperty(Required = Required.Always)]
      public String Description;
      //public String[] Constraints;  //maybe can just be part of description

    }

    [JsonProperty(Required = Required.Always)]
    public String Name;
    [JsonProperty(Required = Required.Always)]
    public String Description;
    [JsonProperty(Required = Required.Always)]
    public Argument[] Arguments;

  }

  public class InfoResponse {
    [JsonProperty(Required = Required.Always)]
    public String Name;
    [JsonProperty(Required = Required.Always)]
    public String Description;
    [JsonProperty(Required = Required.Always)]
    public String Version;
  }

  public class Error {

    public enum ErrorType {
      BadRequest,
      InvalidArgs,
      FetchFailed,
      CriticalFailure
    }

    [JsonProperty(Required = Required.Always)]
    public ErrorType Type;

    [JsonProperty(Required = Required.Default)]
    public String errorMessage;

  }

}
