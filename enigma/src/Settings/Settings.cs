using Exceptions;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using Logging;

namespace Settings {


  public class PluginSetting {

    [JsonPropertyName("src")]
    public String Src { get; set; } = String.Empty;

    [JsonPropertyName("as")]
    public String? As { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = false;

    //name of Repo, not url
    public String Repo { 
      get {
        return Utils.RepoParser.GetRepoName(Src);
      }
    }

  }

  public class UserSettings {
    [JsonPropertyName("plugins")]
    public List<PluginSetting> Plugins { get; set; } = new();
  }


  // Annoyed I want a better Path abstraction, but Path is a String Utility
  // class, .NET 8 has FileInfo which is pretty much what I want.
  public class AppSettings {

    private static ILogger _logger = Logger.For<AppSettings>();

    public bool IsDev = false;
    public String DbPath = "";
    /// <value> Absolute path to plugin directory </value>
    public String PluginPath = Path.GetFullPath("./plugins");
    /// <value> Absolute path to plugin src directory </value>
    public String PluginSrcPath = Path.GetFullPath("./plugins/src");
    /// <value> Absolute path to plugin deployment directory </value>
    public String PluginDeployPath = Path.GetFullPath("./plugins/deploy");
    public UserSettings UserSettings = new UserSettings();

    //TD UI Settings

    public AppSettings(IConfiguration cm) {

      // Build

      String? build = cm.GetValue<String>("build");
      if ( build == null ) {
        Console.Error.WriteLine("Failed to create Settings, no build found");
        Environment.Exit(1);
      }
      IsDev = build == "local";

      // DB

      if ( IsDev ) {
        DbPath = "enigma.db";
      } else {
        SetReleaseDBPath();
      }

      // Settings

      ReadUserSettings();


    }

    /// <summary>
    /// Override Enigma App Settings
    /// </summary>
    /// <exception cref="ConfigurationException"></exception>
    private void ReadUserSettings() {

      _logger.Information("Reading User Settings");
      UserSettings userSettings;

      if (File.Exists("./enigma.json")) {

        String text = File.ReadAllText("./enigma.json");
        userSettings = JsonSerializer.Deserialize<UserSettings>(text);

        //config non-empty?

        if ( text == "") {
            _logger.Warning("Configuration file is empty");
            throw new ConfigurationException("Configuration file is empty");
        }

        //parse settings
      
        try {
          userSettings = JsonSerializer.Deserialize<UserSettings>(text);
        } catch ( JsonException exception ) {
          Console.Error.WriteLine("invalid user settings, unserializable");
          _logger.Error("invalid user settings, unserializable");
          throw new ConfigurationException("invalid user settings, unserializable",exception);
        }

        if (userSettings == null ) {
          _logger.Error("invalid user settings, unserializable");
          Console.Error.WriteLine("invalid user settings, unserializable");
          throw new ConfigurationException("invalid user settings, empty");
        }

        //validate Plugins
      
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

      } 
    }

    private void SetReleaseDBPath() {

      bool windows = false;

      // defaults
      String appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      String enigmaAppData = appData + ( windows ? "\\enigma" : "/enigma");

      // config override ?
      //Environment.SpecialFolder.ApplicationData;

      //ensure data dir exists
      Directory.CreateDirectory(enigmaAppData);
      String enigmaDbPath = enigmaAppData + ( windows ? "\\enigma.db" : "/enigma.db");

      DbPath = enigmaDbPath;

    }


  }
}
