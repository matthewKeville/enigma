using Exceptions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;


namespace Settings {


  public class PluginSettingJson {
    [JsonPropertyName("src")]
    public String Src { get; set; }

    [JsonPropertyName("as")]
    public String? As { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
  }

  public class UserSettingsJson {
    [JsonPropertyName("plugins")]
    public List<PluginSettingJson> Plugins { get; set; } = new();
  }

  public class PluginSetting {
    public String Src { get; set; } = String.Empty;
    public String Repo { get; set; } = String.Empty;
    public String As { get; set; } = String.Empty;
    public bool Enabled { get; set; } = false;
  }

  public class UserSettings {
    public List<PluginSetting> Plugins { get; set; } = new();
  }


  // Annoyed I want a better Path abstraction, but Path is a String Utility
  // class, .NET 8 has FileInfo which is pretty much what I want.
  public class AppSettings {

    public bool IsDev = false;
    public String DbPath = "";
    public String PluginPath = "./plugins";
    public UserSettings UserSettings = new UserSettings();

    //TD UI Settings

    public AppSettings(IConfiguration cm) {

      // build?

      String? build = cm.GetValue<String>("build");
      if ( build == null ) {
        Console.WriteLine("Failed to create Settings, no build found");
        Environment.Exit(1);
      }
      IsDev = build == "local";

      // db

      if ( IsDev ) {
        DbPath = "enigma.db";
      } else {
        SetReleaseDBPath();
      }

      // user settings

      ReadUserSettings();


    }

    /// <summary>
    /// Override Enigma App Settings
    /// </summary>
    /// <exception cref="ConfigurationException"></exception>
    private void ReadUserSettings() {

      UserSettings = new UserSettings();

      if (File.Exists("./enigma.json")) {

        String text = File.ReadAllText("./enigma.json");

        //config non-empty?
        if ( text == "") {
            Trace.WriteLine("Configuration file is empty");
            throw new ConfigurationException("Configuration file is empty");
        }

        Trace.WriteLine("found non-empty config file, contents :");
        Trace.WriteLine(text);

        //parse settings
        UserSettingsJson? userSettingsJson;
        try {
          userSettingsJson = JsonSerializer.Deserialize<UserSettingsJson>(text);
        } catch ( JsonException exception ) {
          Console.Error.WriteLine("invalid user settings, unserializable");
          throw new ConfigurationException("invalid user settings, unserializable",exception);
        }

        if (userSettingsJson == null ) {
          Console.Error.WriteLine("invalid user settings, unserializable");
          throw new ConfigurationException("invalid user settings, empty");
        }

        //Validate Plugins
        List<PluginSetting> validatedPluginSettings = new ();
        foreach ( PluginSettingJson pluginSettingJson in userSettingsJson.Plugins ) {


          //valid repo src?
          if (!Utils.RepoParser.IsRepoUrl(pluginSettingJson.Src)) {
            Trace.WriteLine($"Bad Plugin Configuration, not a git repo {pluginSettingJson.Src}");
            Console.WriteLine($"[WARN] : Bad Plugin Configuration, not a git repo {pluginSettingJson.Src}");
            continue;
          } 
      
          validatedPluginSettings.Add(new PluginSetting() {
              Src = pluginSettingJson.Src,
              Repo = Utils.RepoParser.GetRepoName(pluginSettingJson.Src),
              Enabled = true,
              As = ( pluginSettingJson.As ?? Utils.RepoParser.GetRepoName(pluginSettingJson.Src) )
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
