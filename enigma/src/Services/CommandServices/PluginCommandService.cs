namespace Services.CommandServices {

using Models.Plugin.V1;
using Services.CommandServices.Exceptions;
using Services.PluginService;
using Services.PluginRunnerService;
using global::Exceptions;
using Services.CrosswordInstaller;

    public class PluginCommandService {

  private PluginService pluginService;
  private PluginRunnerService pluginRunnerService;
  private CrosswordInstallerService crosswordInstallerService;

  public PluginCommandService(PluginService pluginService,PluginRunnerService pluginRunnerService,CrosswordInstallerService crosswordInstallerService) {
    this.pluginService = pluginService;
    this.pluginRunnerService = pluginRunnerService;
    this.crosswordInstallerService = crosswordInstallerService;
  }

  /// <exception cref="BadArgsException"></exception>
  public void Process(string[] args) {

    if ( args.Count() < 2) {
      throw new BadArgsException($"invalid command arguments {args}");
    }

    string command = args[1];

    //note to self {} is a scope redeclaration, without this
    //multiple declerations of Type body would conflict

    try {


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
              Response response = pluginRunnerService.RequestInfo(args[2]);

              if ( response.Error == null ) {
                Console.WriteLine(response.ToString()); // Place Holder
              } else {
                handleErrorResponse(response.Error);
              }
          }
          break;
        case "methods":
          {
              Response response = pluginRunnerService.RequestMethods(args[2]);

              if ( response.Error == null ) {
                Console.WriteLine(response.ToString()); // Place Holder
              } else {
                handleErrorResponse(response.Error);
              }
          }
          break;
        case "install":
          {
               String method = args[3];
               String[] installArgs = (args.Length > 4) ?  args[4..] : Array.Empty<String>();
               Response response = pluginRunnerService.RequestFetch(args[2],method,installArgs);

              if ( response.Error == null ) {
                crosswordInstallerService.Install(response.Fetch);
              } else {
                handleErrorResponse(response.Error);
              }
          }
          break;
        default :
          throw new BadArgsException($"no such plugin sub command {command}");

      }

    } catch (PluginNotFoundException ex) {

      Console.WriteLine($"Plugin Not Found");

    } catch (PluginResponseSerializationException ex) {

      Console.WriteLine($"Plugin Response Could Not Be Serialized");
      Trace.WriteLine(ex.ToString());

    }
 

  }

  private void handleErrorResponse(ErrorResponse errorResponse) {
    Console.WriteLine($"Plugin Command Failed");
    Trace.WriteLine(errorResponse.ToString());
  }

} 

}

