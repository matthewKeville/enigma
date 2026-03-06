namespace Services.CommandServices {

using Models.Plugin.V1;
using Services.CommandServices.Exceptions;
using Services.PluginService;
using Services.PluginRunnerService;

public class PluginCommandService {

  private PluginService pluginService;
  private PluginRunnerService pluginRunnerService;

  public PluginCommandService(PluginService pluginService,PluginRunnerService pluginRunnerService) {
    this.pluginService = pluginService;
    this.pluginRunnerService = pluginRunnerService;
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
          InfoResponse info = pluginRunnerService.RequestInfo(args[2]);
          Console.WriteLine(info.ToString());
        }
        break;
      case "methods":
        {
          MethodsResponse info = pluginRunnerService.RequestMethods(args[2]);
          Console.WriteLine(info.ToString());
        }
        break;
      default :
        throw new BadArgsException($"no such plugin sub command {command}");

    }

  }

} 

}

