namespace Services.CommandServices {
    using System.Text.Json;
    using Services.CommandServices.Exceptions;
using Services.PluginService;

public class PluginCommandService {

  private PluginService pluginService;

  public PluginCommandService(PluginService pluginService) {
    this.pluginService = pluginService;
  }

  /// <exception cref="BadArgsException"></exception>
  public void Process(string[] args) {

    if ( args.Count() < 2) {
      throw new BadArgsException($"invalid command arguments {args}");
    }

    string command = args[1];

    //note to self {} is a scope redeclaration, without this
    //multiple declerations of Type body would conflict

    switch ( command ) {
      case "list":
        {
          List<String> pluginStrings = pluginService.GetInstalledPlugins();
          Console.WriteLine($"{pluginStrings.Count()} plugins installed");
          pluginStrings.ForEach( p => Console.WriteLine(p));
        }
        break;
      case "info":
        {
          InfoResponseBody body = pluginService.RequestPluginInfo(args[2]);
          Console.WriteLine($" plugin : {body.name}");
          Console.WriteLine(new String('-',80));
          Console.WriteLine(body.description);
        }
        break;
      case "methods":
        {
          MethodsResponseBody body = pluginService.GetPluginMethods(args[2]);
          Console.WriteLine(JsonSerializer.Serialize<MethodsResponseBody>(body));
        }
        break;
      default :
        throw new BadArgsException($"no such plugin sub command {command}");

    }

    //plugin list
    //plugin info <pluginName>
    //plugin methods <pluginName>
    //plugin method <pluginName> <methodName>
  }

  private void list() {

  }

} 

}

