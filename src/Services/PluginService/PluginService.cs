using Settings;
using Exceptions;
using Utils.Exceptions;
using Utils;

namespace Services.PluginService {

  public class PluginService {

    AppSettings appSettings;

    public PluginService(AppSettings appSettings) {
      this.appSettings = appSettings;
    }

    public List<String> GetInstalledPlugins() {
      List<String> pluginNames = appSettings.UserSettings.Plugins
        .FindAll(p => p.Enabled)
        .Select( p => $"{p.Repo} as {p.As}")
        .ToList();
      return pluginNames;
    }

    public void GetPluginInfo() {}
    public void GetPluginMethods() {}
    public void GetPluginMethod() {}
    public void Install() {}

    /// <summary>
    /// Install or Delete Plugins based on user configuration
    /// </summary>
    /// <exception cref="PluginException"></exception>
    public void Sync() {

      Console.WriteLine($"syncing {appSettings.UserSettings.Plugins.Count()} plugins");

      //We need an plugin directory
      if ( !Directory.Exists(appSettings.PluginPath) ) {
        Trace.WriteLine($"creating plugin directory {appSettings.PluginPath}");
        Directory.CreateDirectory(appSettings.PluginPath);
      }

      //check if any plugins need to be installed 
      foreach( PluginSetting pluginSetting in (appSettings.UserSettings.Plugins.FindAll(p => p.Enabled))) {
        String inspectedPluginPath = Path.Join(appSettings.PluginPath,pluginSetting.Repo);
        if ( !Directory.Exists(inspectedPluginPath)) {
          Console.WriteLine($"installing plugin {pluginSetting.Src}");
          installPlugin(pluginSetting.Src,appSettings.PluginPath);
        }
      }

      //check if any plugins need to removed (no longer in configuration)
      foreach( String path in Directory.GetDirectories(appSettings.PluginPath))  {
        String pluginName = Path.GetFileName(path);
        if ( !appSettings.UserSettings.Plugins.Any( ps => ps.Enabled && (ps.Repo == pluginName) ) ) {
          Console.WriteLine($" the plugin {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          Directory.Delete(path, recursive:true);
        }
      }

      //check if any plugins need to be updated

    }


    /// <exception cref="PluginException"></exception>
    private void installPlugin(String src,String installationPath) {

      Console.WriteLine($"cloning plugin {src} to {installationPath}");

      Process process = new Process {
        StartInfo= new ProcessStartInfo {
          FileName = "git",
          Arguments = $"clone {src}",
          WorkingDirectory = installationPath
        }
      };
      process.Start();
      process.WaitForExit();
      if (process.ExitCode != 0) {
        //throw an error
        Console.WriteLine("error cloning plugin repository");
        throw new PluginException($"unable to clone repository {src}");
      }

      Console.WriteLine($"building plugin {src}");
      //do build...
    }

  }


}
