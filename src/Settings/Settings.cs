using Exceptions;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;


namespace Settings {

  public class PluginSetting {

    [JsonPropertyName("src")]
    public String Src { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
    
    public override String ToString() {
      return 
        $"src {Src} enabled {Enabled}";
    }
  }

  public class UserSettings {

    //Plugins
    [JsonPropertyName("plugins")]
    public List<PluginSetting>? Plugins { get; set; } = new();


    public override String ToString() {
      String result = "";
      Plugins.ForEach( p => result+= "\n" + p.ToString() );
      return result;
    }
    
  }


  // Annoyed I want a better Path abstraction, but Path is a String Utility
  // class, .NET 8 has FileInfo which is pretty much what I want.
  public class AppSettings {

    public bool IsDev = false;
    public String DbPath;
    public String PluginPath = "./plugins";
    public UserSettings UserSettings;

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
        try {
          UserSettings = JsonSerializer.Deserialize<UserSettings>(text);
        } catch ( JsonException exception ) {
          Console.Error.WriteLine("invalid user settings, unserializable");
          throw new ConfigurationException("invalid user settings, unserializable",exception);
        }

        if (UserSettings == null ) {
          Console.Error.WriteLine("invalid user settings, unserializable");
          throw new ConfigurationException("invalid user settings, empty");
        }

        //Validate Plugins
        List<PluginSetting> validatedPluginSettings = new ();
        foreach ( PluginSetting pluginSetting in UserSettings.Plugins ) {

          // WARN : begin gpt
          bool isGithubRepo = Regex.IsMatch(pluginSetting.Src,
            @"^(https?:\/\/|git@)github\.com[:\/](?:[\w\-]+\/)+[\w\-]+(?:\.git)?$",
            RegexOptions.IgnoreCase);
          // WARN : end gpt

          //valid repo src?
          if (!isGithubRepo) {
            Trace.WriteLine($"Bad Plugin Configuration, not a git repo {pluginSetting.Src}");
            Console.WriteLine($"[WARN] : Bad Plugin Configuration, not a git repo {pluginSetting.Src}");
            continue;
          } 

          validatedPluginSettings.Add(pluginSetting);

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
