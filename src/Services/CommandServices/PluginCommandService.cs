namespace Services.CommandServices {

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

    switch ( command ) {
      case "list":
        List<String> pluginStrings = pluginService.GetInstalledPlugins();
        Console.WriteLine($"{pluginStrings.Count()} plugins installed");
        pluginStrings.ForEach( p => Console.WriteLine(p));
        break;
      case "info":
        throw new NotImplementedException("plugin info not implemented");
        break;
      case "methods":
        throw new NotImplementedException("plugin methods not implemented");
        break;
      case "method":
        throw new NotImplementedException("plugin method not implemented");
        break;
      default :
        throw new NotImplementedException($"no such plugin sub command {command}");

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

