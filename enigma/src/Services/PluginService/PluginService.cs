using Settings;
using Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Models.Plugin.V1;
using Logging;
using Serilog;

namespace Services.PluginService {

  public class PluginService {

    private static ILogger _logger = Logger.For<PluginService>();
    AppSettings appSettings;

    public PluginService(AppSettings appSettings) {
      this.appSettings = appSettings;
    }

    /// <summary>
    /// Return list of the aliases of installed plugins
    /// </summary>
    /// <exception cref="PluginException"></exception>
    public List<String> GetInstalledPlugins() {
      List<String> pluginNames = appSettings.UserSettings.Plugins
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

      Console.WriteLine($"syncing {appSettings.UserSettings.Plugins.Count()} plugins");
      _logger.Information($"syncing {appSettings.UserSettings.Plugins.Count()} plugins");

      //We need an plugin directory
      if ( !Directory.Exists(appSettings.PluginPath) ) {
        _logger.Information($"creating plugin directory {appSettings.PluginPath}");
        Directory.CreateDirectory(appSettings.PluginPath);
        Directory.CreateDirectory(appSettings.PluginSrcPath);
        Directory.CreateDirectory(appSettings.PluginDeployPath);
      }

      //check if any plugins need to be installed 
      foreach( PluginSetting pluginSetting in (appSettings.UserSettings.Plugins.FindAll(p => p.Enabled))) {
        String inspectedPluginPath = Path.Join(appSettings.PluginSrcPath,pluginSetting.Repo);
        if ( !Directory.Exists(inspectedPluginPath)) {
          Console.WriteLine($"installing plugin {pluginSetting.Src}");
          _logger.Information($"installing plugin {pluginSetting.Src}");
          downloadPlugin(pluginSetting.Src);
          buildPlugin(pluginSetting.Repo);
        }
      }

      //check if any plugins need to removed (no longer in configuration)
      foreach( String path in Directory.GetDirectories(appSettings.PluginSrcPath))  {
        String pluginName = Path.GetFileName(path);
        if ( !appSettings.UserSettings.Plugins.Any( ps => ps.Enabled && (ps.Repo == pluginName) ) ) {
          Console.WriteLine($" the plugin src {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          _logger.Information($" the plugin src {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          Directory.Delete(path, recursive:true);
        }
      }
      //check if any plugins need to removed (no longer in configuration)
      foreach( String path in Directory.GetDirectories(appSettings.PluginDeployPath))  {
        String pluginName = Path.GetFileName(path);
        if ( !appSettings.UserSettings.Plugins.Any( ps => ps.Enabled && (ps.Repo == pluginName) ) ) {
          Console.WriteLine($" the plugin build {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          _logger.Information($" the plugin build {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          Directory.Delete(path, recursive:true);
        }
      }

      //check if any plugins need to be updated

    }


    /// <exception cref="PluginCloneException"></exception>
    private void downloadPlugin(String src) {

      Console.WriteLine($"cloning plugin {src} to {appSettings.PluginPath}");
      _logger.Information($"cloning plugin {src} to {appSettings.PluginPath}");

      Process process = new Process {
        StartInfo= new ProcessStartInfo {
          FileName = "git",
          Arguments = $"clone {src}",
          WorkingDirectory = appSettings.PluginSrcPath
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
      _logger.Information($"building plugin {repoName}");

      ProcessStartInfo startInfo= new ProcessStartInfo {
          FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "sh" : "/bin/sh",
          Arguments = "build.sh",
          WorkingDirectory = Path.Join(appSettings.PluginSrcPath,repoName)
      };
      startInfo.EnvironmentVariables["BUILD_DIR"] = Path.Join(appSettings.PluginDeployPath,repoName);
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
