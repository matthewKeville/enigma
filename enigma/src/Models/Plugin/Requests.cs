using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace Plugin {


  public class Request {
  
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestType {
      Info,
      Methods,
      Fetch
    }

    [JsonProperty(Required = Required.Always)]
    public String Version { get; set; }

    [JsonProperty(Required = Required.Always)]
    public RequestType Type { get; set; }

    //this should be required if requestType is Fetch
    [JsonProperty(Required = Required.Default)]
    public FetchRequest Fetch { get; set; }

    public static JSchema Generate() {

      JSchemaGenerator generator = new JSchemaGenerator();
      generator.GenerationProviders.Add(new StringEnumGenerationProvider());
      JSchema schema = generator.Generate(typeof(Request));

      // Use allOf to combine multiple conditional rules
      schema.AllOf.Add(new JSchema
      {
          If = new JSchema
          {
              Properties = { ["Type"] = new JSchema { Enum = { JToken.FromObject(Request.RequestType.Fetch) } } }
          },
          Then = new JSchema
          {
              Required = { "Fetch" }
          }
      });

      schema.Properties["Type"].Description = "The Type of Request";
      schema.Properties["Fetch"].Description = "Fetch Request object";

      schema.Id = new Uri(Schema.SchemaGenerator.SCHEMA_HOME + "/RequestSchema.json");
      return schema;

    }

    public static String SerializeJson(Request request) {
      return JsonConvert.SerializeObject(request);
    }

  }

  public class FetchRequest {

    [JsonProperty(Required = Required.Always)]
    public String Method { get; set; }

    [JsonProperty(Required = Required.Always)]
    public String[] Args { get; set; }

  }

  //public class MethodsRequest { }
}
