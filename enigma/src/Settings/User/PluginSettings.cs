using Logging;
using Newtonsoft.Json;
using Serilog;

namespace Settings.User.Plugin {

  public class PluginSettings {

    public class PluginConfig {

      [JsonProperty("src")]
      public String Src { get; set; } = String.Empty;

      [JsonProperty("as")]
      public String? As { get; set; }

      [JsonProperty("enabled")]
      public bool Enabled { get; set; } = false;

      //name of Repo, not url
      public String Repo { 
        get {
          return Utils.RepoParser.GetRepoName(Src);
        }
      }

    }

    /// <value> Absolute path to plugin directory </value>
    public String PluginPath = Path.GetFullPath("./plugins");
    /// <value> Absolute path to plugin src directory </value>
    public String PluginSrcPath = Path.GetFullPath("./plugins/src");
    /// <value> Absolute path to plugin deployment directory </value>
    public String PluginDeployPath = Path.GetFullPath("./plugins/deploy");

    [JsonProperty("plugins")]
    public List<PluginConfig> Plugins { get; set; } = new();

    private static ILogger _logger = Logger.For<PluginSettings>();

    private void Validate() {

      _logger.Information("Validating Plugin Settings");

        //validate Plugins Configs
      
        /**
      
        List<PluginSetting> validatedPluginSettings = new ();
        foreach ( PluginSetting pluginSetting in userSettings.Plugins ) {

          //valid repo src?
          if (!Utils.RepoParser.IsRepoUrl(pluginSetting.Src)) {

            _logger.Error($"Bad Plugin Configuration, not a git repo {pluginSetting.Src}");
            Console.Error.WriteLine($"Bad Plugin Configuration, not a git repo {pluginSetting.Src}");

            continue;

          } 
      
          validatedPluginSettings.Add(new PluginSetting() {
              Src = pluginSetting.Src,
              Enabled = true,
              As = ( pluginSetting.As ?? pluginSetting.Repo )
          });

        }

        UserSettings.Plugins = validatedPluginSettings;

        */

    }

  }

}
