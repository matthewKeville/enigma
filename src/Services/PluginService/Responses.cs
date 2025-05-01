using System.Text.Json.Serialization;
using System.Text.Json;

namespace Services.PluginService {

  //Note to self about Json parsing in C# SDK,
  //members need to be properties i.e. Type Name { encaps }

  public interface ResponseBody {}

  //////////////////////////////////////////////////////////////////////
  // InfoResponseBody

  //this is frustrating, I want to annotate this such
  //that the fields that musn't be null will throw
  //when deserialized, but no such annotation exists.
  //and I i need provide default values for name and desc,
  //otherwise the compiler will nag at me..
  public class InfoResponseBody : ResponseBody {
    [JsonPropertyName("name")]
    public String name { get; set; } =  "";
    [JsonPropertyName("description")]
    public String description { get; set;} = "";
    [JsonPropertyName("version")]
    public String? version { get; set; }
  }

  //////////////////////////////////////////////////////////////////////
  // MethodsResponseBody

  public class ArgsJson {
    [JsonPropertyName("name")]
    public String name { get; set; } = "";
    [JsonPropertyName("description")]
    public String description { get; set; } = "";
    [JsonPropertyName("constraints")]
    public List<String> constraints { get; set; } = new();
  }

  public class MethodJson {
    [JsonPropertyName("name")]
    public String name { get; set; } = "";
    [JsonPropertyName("description")]
    public String description { get; set; } = "";
    [JsonPropertyName("args")]
    public List<ArgsJson> args { get; set; } = new();
  }

  public class MethodsResponseBody : ResponseBody {
    [JsonPropertyName("methods")]
    public List<MethodJson> methods { get; set; } = new(); 
  }

}
