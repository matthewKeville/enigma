using Logging;
using Newtonsoft.Json;
using Serilog;
using Settings.User.Keymap;
using Settings.User.Plugin;
using Settings.User.UI;

namespace Settings.User {

  public class UserSettings {

    private static ILogger _logger = Logger.For<UserSettings>();

    [JsonProperty("plugins", Required = Required.Default)]
    public PluginSettings pluginSettings = new PluginSettings();

    [JsonProperty("keymaps", Required = Required.Default)]
    public KeymapSettings keymapSettings = new KeymapSettings();

    [JsonProperty("ui", Required = Required.Default)]
    public UISettings uiSettings = new UISettings();

    public override String ToString() {
      return string.Join("\n", new List<String>() { pluginSettings?.ToString()??"none", keymapSettings?.ToString()??"none", uiSettings?.ToString()??"none" });
    }

    public static UserSettings ReadSettings() {

      _logger.Information("reading enigma.json");

      if (!File.Exists("./enigma.json")) {
        _logger.Warning("enigma.json not found, using default settings");
        return new UserSettings();
      } 

      String fileText = File.ReadAllText("./enigma.json");

      if (String.IsNullOrEmpty(fileText)) {
        _logger.Warning("enigma.json found, but empty, using default settings");
        return new UserSettings();
      } 

      _logger.Information("enigma.json found");
      UserSettings? userSettings = null;

      try {
        userSettings = JsonConvert.DeserializeObject<UserSettings>(fileText);
        //TBD call settings.validate
        //TBD call settings.validate
        //TBD call settings.validate
        //TBD call settings.validate
        //TBD call settings.validate
      } catch (JsonReaderException ex) {
        Console.Error.WriteLine("unable to parse enigma.json, see logs, using default settings");
        _logger.Error(ex.ToString());
      } catch (JsonSerializationException ex) {
        Console.Error.WriteLine("unable to parse enigma.json, see logs, using default settings");
        _logger.Error(ex.ToString());
      }

      userSettings ??= new UserSettings();

      return userSettings;

    }

  }

}
