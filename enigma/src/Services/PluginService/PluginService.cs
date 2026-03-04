using Settings;
using Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Services.PluginService {

  public class PluginService {

    AppSettings appSettings;

    public PluginService(AppSettings appSettings) {
      this.appSettings = appSettings;
    }

    /// <summary>
    /// Return the info response JSON as a dictionary for
    /// the provided plugin
    /// </summary>
    /// <param name="pluginName"> plugin alias </param>
    /// <returns>
    ///   InfoResponseBody
    /// </returns>
    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    /// <exception cref="PluginResponseException"></exception>
    /**
    public InfoResponseBody RequestPluginInfo(String pluginName) {
      return (InfoResponseBody) runPlugin(pluginName,PluginInfoRequest.AsJsonString());
    }
    */

    /// <summary>
    /// Return the methods available for the plugin
    /// </summary>
    /// <param name="pluginName"> plugin alias </param>
    /// <returns>
    ///   MethodsResponseBody 
    /// </returns>
    /// <exception cref="PluginException"></exception>
    /**
    public MethodsResponseBody GetPluginMethods(String pluginName) {
      return (MethodsResponseBody) runPlugin(pluginName,MethodsRequest.AsJsonString());
    }
    */

    //public void Install() {}

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

      //We need an plugin directory
      if ( !Directory.Exists(appSettings.PluginPath) ) {
        Trace.WriteLine($"creating plugin directory {appSettings.PluginPath}");
        Directory.CreateDirectory(appSettings.PluginPath);
        Directory.CreateDirectory(appSettings.PluginSrcPath);
        Directory.CreateDirectory(appSettings.PluginDeployPath);
      }

      //check if any plugins need to be installed 
      foreach( PluginSetting pluginSetting in (appSettings.UserSettings.Plugins.FindAll(p => p.Enabled))) {
        String inspectedPluginPath = Path.Join(appSettings.PluginSrcPath,pluginSetting.Repo);
        if ( !Directory.Exists(inspectedPluginPath)) {
          Console.WriteLine($"installing plugin {pluginSetting.Src}");
          downloadPlugin(pluginSetting.Src);
          buildPlugin(pluginSetting.Repo);
        }
      }

      //check if any plugins need to removed (no longer in configuration)
      foreach( String path in Directory.GetDirectories(appSettings.PluginSrcPath))  {
        String pluginName = Path.GetFileName(path);
        if ( !appSettings.UserSettings.Plugins.Any( ps => ps.Enabled && (ps.Repo == pluginName) ) ) {
          Console.WriteLine($" the plugin src {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          Directory.Delete(path, recursive:true);
        }
      }
      //check if any plugins need to removed (no longer in configuration)
      foreach( String path in Directory.GetDirectories(appSettings.PluginDeployPath))  {
        String pluginName = Path.GetFileName(path);
        if ( !appSettings.UserSettings.Plugins.Any( ps => ps.Enabled && (ps.Repo == pluginName) ) ) {
          Console.WriteLine($" the plugin build {pluginName} does not exist in config or is disabled, {pluginName} will be deleted");
          Directory.Delete(path, recursive:true);
        }
      }

      //check if any plugins need to be updated

    }


    /// <exception cref="PluginCloneException"></exception>
    private void downloadPlugin(String src) {

      Console.WriteLine($"cloning plugin {src} to {appSettings.PluginPath}");

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

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    /// <exception cref="PluginResponseException"></exception>
    /**
    private ResponseBody runPlugin(String pluginName, String json) {
      PluginSetting? pluginSetting = appSettings.UserSettings.Plugins.Find( p => p.As.Equals(pluginName));
      if (pluginSetting is null) {
        throw new PluginNotFoundException($"no such plugin with alias {pluginName}");
      }

      Process process = new Process { 
        StartInfo = new ProcessStartInfo {
          FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "sh" : "/bin/sh",
          Arguments = "run.sh",
          WorkingDirectory = Path.Join(appSettings.PluginDeployPath,pluginSetting.Repo),
          RedirectStandardOutput = true,
          RedirectStandardInput = true
        }
      };

      Trace.WriteLine($"writing this to stdin \n {json}");
      process.Start();
      process.StandardInput.Write(json);
      process.StandardInput.Close();
      String stdout = process.StandardOutput.ReadToEnd();
      process.WaitForExit();
      Trace.WriteLine($"process finished stdout is \n {stdout}");

      //try parse response
      ResponseBody? responseBody;
      try {
        JsonDocument doc = JsonDocument.Parse(stdout);
        String responseType = doc.RootElement.GetProperty("responseType").GetString() ?? "";
        switch ( responseType ) {
          case "info":
            {
              JsonElement body = doc.RootElement.GetProperty("body");
              responseBody = JsonSerializer.Deserialize<InfoResponseBody>(body.GetRawText());
            }
            break;
          case "methods":
            { 
              JsonElement body = doc.RootElement.GetProperty("body");
              responseBody = JsonSerializer.Deserialize<MethodsResponseBody>(body.GetRawText());
            }
            break;
          case "fetch":
            {
              throw new NotImplemented("fetch response parsing not implemented");
              break;
            }
          default:
            throw new PluginResponseException("Unexpected plugin response, missing responseType");
        }
      }
      catch ( Exception ex) {
        throw new PluginResponseSerializationException("plugin response is not understood",ex);
      } 

      return responseBody!;

    }
    */

    public class PluginInfoRequest {
      [JsonPropertyName("requestType")]
      [JsonInclude]
      public readonly String? requestType = "info";
      public static String AsJsonString() {
        return JsonSerializer.Serialize<PluginInfoRequest>(new PluginInfoRequest());
      }
    }

    public class MethodsRequest {
      [JsonPropertyName("requestType")]
      [JsonInclude]
      public readonly String? requestType = "methods";
      public static String AsJsonString() {
        return JsonSerializer.Serialize<MethodsRequest>(new MethodsRequest());
      }
    }

  }


}
