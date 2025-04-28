
namespace Services.CommandServices {

public class InstallCommandService {


  public InstallCommandService() {}

  /// <summary>
  /// install a crossword from a plugin
  /// format : 'install' <pluginName> <method> args..
  /// </summary>
  /// <exception cref="BadArgsException"></exception>
  public void Install(string[] args) {

    throw new NotImplementedException();

    if (args.Count() < 2) {
      //throw
    }

    String pluginName = args[1];
    //pluginName installed and loaded?
    //throw PluginNotFoundException

    //method arg exists?
    //throw PluginMethodError

    //try request?
    //throw FetchFailure
   
    //good data?
    //attempt installation
    

  }

} 

}

