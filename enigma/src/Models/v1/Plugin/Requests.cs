using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;

namespace Models.Plugin.V1 {

  public class Request {
  
    [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
    public enum RequestType {
      [EnumMember(Value="info")]
      Info,
      [EnumMember(Value="methods")]
      Methods,
      [EnumMember(Value="fetch")]
      Fetch
    }

    [JsonProperty(Required = Required.Always)]
    public String ApiVersion { get; } = SchemaGenerator.VERSION;

    [JsonProperty(Required = Required.Always)]
    public RequestType Type { get; set; }

    [JsonProperty(Required = Required.Default)]
    public FetchRequest? Fetch { get; set; }

    public static JSchema Generate() {

      JSchemaGenerator generator = new JSchemaGenerator();
      generator.GenerationProviders.Add(new StringEnumGenerationProvider());
      generator.ContractResolver = new CamelCasePropertyNamesContractResolver();
      JSchema schema = generator.Generate(typeof(Request));

      // Use allOf to combine multiple conditional rules
      schema.AllOf.Add(new JSchema
      {
          If = new JSchema
          {
              Properties = { ["type"] = new JSchema { Enum = { JToken.FromObject(Request.RequestType.Fetch) } } }
          },
          Then = new JSchema
          {
              Required = { "fetch" }
          }
      });

      schema.Properties["type"].Description = "The Type of Request";
      schema.Properties["fetch"].Description = "Fetch Request object";

      schema.Id = new Uri(SchemaGenerator.SCHEMA_HOME + "/RequestSchema.json");
      return schema;

    }

    public static String SerializeJson(Request request) {

      var settings = new JsonSerializerSettings
      {
          ContractResolver = new DefaultContractResolver
          {
              NamingStrategy = new CamelCaseNamingStrategy()
          }
      };
    
      return JsonConvert.SerializeObject(request,settings);
    }

  }

  public class FetchRequest {

    [JsonProperty(Required = Required.Always)]
    public required String Method { get; set; }

    [JsonProperty(Required = Required.Always)]
    public required String[] Args { get; set; }

  }

}
