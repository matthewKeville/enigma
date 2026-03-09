using Settings;
using Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Models.Plugin.V1;

namespace Services.PluginRunnerService {

  public class PluginRunnerService {

    AppSettings appSettings;

    public PluginRunnerService(AppSettings appSettings) {
      this.appSettings = appSettings;
    }

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    public Response RequestInfo(String pluginName) {
      Request request = new Request
      {
          Type = Request.RequestType.Info,
      };
      Console.WriteLine(Request.SerializeJson(request)); //DBG
      return runPlugin(pluginName,Request.SerializeJson(request));
    }

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    public Response RequestMethods(String pluginName) {
      Request request = new Request
      {
          Type = Request.RequestType.Methods,
      };
      Console.WriteLine(Request.SerializeJson(request)); //DBG
      return runPlugin(pluginName,Request.SerializeJson(request));
    }

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
    public Response RequestFetch(String pluginName,String method,String[] args) {
      Request request = new Request
      {
          Type = Request.RequestType.Fetch,
          Fetch = new FetchRequest {
            Method = method,
            Args = args
          }
      };
      Console.WriteLine(Request.SerializeJson(request)); //DBG
      Response response =  runPlugin(pluginName,Request.SerializeJson(request));
      return response;
    }

    /// <exception cref="PluginNotFoundException"></exception>
    /// <exception cref="PluginResponseSerializationException"></exception>
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
        throw new PluginResponseSerializationException("plugin response is not understood\n"+stdout+"\n",ex);
      } 

      return response!;

    }


  }


}
