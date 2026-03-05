using Settings;
using Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Plugin;

namespace Services.PluginRunnerService {

  public class PluginRunnerService {

    AppSettings appSettings;

    public PluginRunnerService(AppSettings appSettings) {
      this.appSettings = appSettings;
    }

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    /// <exception cref="PluginResponseException"></exception>
    ///
    public InfoResponse RequestInfo(String pluginName) {
      Request request = new Request
      {
          Version = Schema.SchemaGenerator.VERSION,
          Type = Request.RequestType.Info,
      };
      Console.WriteLine(Request.SerializeJson(request));
      return runPlugin(pluginName,Request.SerializeJson(request)).Info;
    }

    /// <exception cref="PluginException"></exception>
    ///
    public MethodsResponse RequestMethods(String pluginName) {
      Request request = new Request
      {
          Version = Schema.SchemaGenerator.VERSION,
          Type = Request.RequestType.Methods,
      };
      Console.WriteLine(Request.SerializeJson(request));
      return runPlugin(pluginName,Request.SerializeJson(request)).Methods;
    }

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    /// <exception cref="PluginResponseException"></exception>
    ///
    private Response runPlugin(String pluginName, String json) {

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

      process.Start();
      process.StandardInput.Write(json);
      process.StandardInput.Close();
      String stdout = process.StandardOutput.ReadToEnd();
      process.WaitForExit();

      //try parse response
      Response response;
      try {
        response = Response.DeserializeJson(stdout);
      }
      catch ( Exception ex) {
        throw new PluginResponseSerializationException("plugin response is not understood",ex);
      } 

      return response!;

    }


  }


}
