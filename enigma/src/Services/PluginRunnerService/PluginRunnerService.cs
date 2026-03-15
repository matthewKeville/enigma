using Models.Plugin.V1;
using Settings.User.Plugin;
using static Settings.User.Plugin.PluginSettings;

namespace Services.PluginRunnerService {

  public class PluginRunnerService {

    PluginSettings pluginSettings;

    public PluginRunnerService(Settings.Settings settings) {
      this.pluginSettings = settings.userSettings.pluginSettings;
    }

    /// <exception cref="PluginRunnerServiceException"></exception>
    public Response RequestInfo(String pluginName) {
      Request request = new Request
      {
          Type = Request.RequestType.Info,
      };
      return runPlugin(pluginName,Request.SerializeJson(request));
    }

    /// <exception cref="PluginRunnerServiceException"></exception>
    public Response RequestMethods(String pluginName) {
      Request request = new Request
      {
          Type = Request.RequestType.Methods,
      };
      return runPlugin(pluginName,Request.SerializeJson(request));
    }

    /// <exception cref="PluginRunnerServiceException"></exception>
    public Response RequestFetch(String pluginName,String method,String[] args) {
      Request request = new Request
      {
          Type = Request.RequestType.Fetch,
          Fetch = new FetchRequest {
            Method = method,
            Args = args
          }
      };
      Response response =  runPlugin(pluginName,Request.SerializeJson(request));
      return response;
    }

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    private Response runPlugin(String pluginName, String json) {

      PluginConfig? pluginConfig = pluginSettings.Plugins.Find( p => (p?.As ?? "").Equals(pluginName));
      if (pluginConfig is null) {
        throw new PluginNotFoundException($"no such plugin with alias {pluginName}");
      }

      Process process = new Process { 
        StartInfo = new ProcessStartInfo {
          FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "sh" : "/bin/sh",
          Arguments = "run.sh",
          WorkingDirectory = Path.Join(pluginSettings.PluginDeployPath,pluginConfig.Repo),
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
      Response? response;
      try {
        response = Response.DeserializeJson(stdout);
      }
      catch ( Exception ex) {
        throw new PluginResponseSerializationException("plugin response is not understood\n"+stdout+"\n",ex);
      } 

      if (response == null) {
        throw new PluginResponseSerializationException("deserailization returned null without error");
      }

      return response!;

    }


  }


}
