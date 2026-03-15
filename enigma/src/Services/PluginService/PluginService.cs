using Logging;
using Serilog;
using Settings.User.Plugin;
using static Settings.User.Plugin.PluginSettings;

namespace Services.PluginService {

  public class PluginService {

    private static ILogger _logger = Logger.For<PluginService>();
    private PluginSettings pluginSettings;

    public PluginService(Settings.Settings settings) {
      this.pluginSettings = settings.userSettings.pluginSettings;
    }

    /// <summary>
    /// Return list of the aliases of installed plugins
    /// </summary>
    /// <exception cref="PluginServiceException"></exception>
    public List<String> GetInstalledPlugins() {
      List<String> pluginNames = pluginSettings.Plugins
        .FindAll(p => p.Enabled)
        .Select( p => $"{p.Repo} as {p.As}")
        .ToList();
      return pluginNames;
    }


    /// <summary>
    /// Update the plugins managed under enigma, cloning & building
    /// them if not installed, but configured in enigma.json.
    /// Remove any plugins, not specified under configuration or
    /// disabled.
    /// </summary>
    /// <exception cref="PluginException"></exception>
    public void Sync() {

      Console.WriteLine($"syncing {pluginSettings.Plugins.Count()} plugins");

      //We need an plugin directory
      if ( !Directory.Exists(pluginSettings.PluginPath) ) {
        _logger.Information($"creating plugin directory {pluginSettings.PluginPath}");
        Directory.CreateDirectory(pluginSettings.PluginPath);
        Directory.CreateDirectory(pluginSettings.PluginSrcPath);
        Directory.CreateDirectory(pluginSettings.PluginDeployPath);
      }

      //check if any plugins need to be installed 
      foreach( PluginConfig pluginConfig in (pluginSettings.Plugins.FindAll(p => p.Enabled))) {
        String inspectedPluginPath = Path.Join(pluginSettings.PluginSrcPath,pluginConfig.Repo);
        if ( !Directory.Exists(inspectedPluginPath)) {
          Console.WriteLine($"installing plugin {pluginConfig.Src}");
          downloadPlugin(pluginConfig.Src);
          buildPlugin(pluginConfig.Repo);
        }
      }

      //check if any plugins need to removed (no longer in configuration)
      foreach( String path in Directory.GetDirectories(pluginSettings.PluginSrcPath))  {
        String pluginName = Path.GetFileName(path);
        if ( !pluginSettings.Plugins.Any( ps => ps.Enabled && (ps.Repo == pluginName) ) ) {
          Console.WriteLine($" the plugin src {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          Directory.Delete(path, recursive:true);
        }
      }
      //check if any plugins need to removed (no longer in configuration)
      foreach( String path in Directory.GetDirectories(pluginSettings.PluginDeployPath))  {
        String pluginName = Path.GetFileName(path);
        if ( !pluginSettings.Plugins.Any( ps => ps.Enabled && (ps.Repo == pluginName) ) ) {
          Console.WriteLine($" the plugin build {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          Directory.Delete(path, recursive:true);
        }
      }

      //check if any plugins need to be updated

    }


    /// <exception cref="PluginCloneException"></exception>
    private void downloadPlugin(String src) {

      Console.WriteLine($"cloning plugin {src} to {pluginSettings.PluginPath}");

      Process process = new Process {
        StartInfo= new ProcessStartInfo {
          FileName = "git",
          Arguments = $"clone {src}",
          WorkingDirectory = pluginSettings.PluginSrcPath
        }
      };
      process.Start();
      process.WaitForExit();
      if (process.ExitCode != 0) {
        throw new PluginCloneException($"unable to clone plugin repository {src}");
      }

    }

    /// <exception cref="PluginBuildException"></exception>
    private void buildPlugin(String repoName) {

      Console.WriteLine($"building plugin {repoName}");

      ProcessStartInfo startInfo= new ProcessStartInfo {
          FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "sh" : "/bin/sh",
          Arguments = "build.sh",
          WorkingDirectory = Path.Join(pluginSettings.PluginSrcPath,repoName)
      };
      startInfo.EnvironmentVariables["BUILD_DIR"] = Path.Join(pluginSettings.PluginDeployPath,repoName);
      Process process = new Process {
        StartInfo = startInfo
      };
      process.Start();
      process.WaitForExit();
      if (process.ExitCode != 0) {
        throw new PluginBuildException($"unable to build plugin {repoName}");
      }

    }

  }

}
