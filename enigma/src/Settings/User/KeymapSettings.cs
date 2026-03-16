using Logging;
using Newtonsoft.Json;
using Serilog;
using UI.KeyMapping;

namespace Settings.User.Keymap {

  public class KeymapSettings {

    private static ILogger _logger = Logger.For<KeymapSettings>();

    [JsonProperty("keymaps")]
    public List<KeymapConfig> keymaps { get; set; } = new ();

    public override String ToString() {
      if ( keymaps.Count() == 0 ) {
        return "(keymapsettings) = none";
      }
      return "(keymapsettings) =" + string.Join('\n', keymaps.Select( k => k.ToString())) + ")";
    }

    public class KeymapConfig {

      [JsonProperty("command")]
      public required UICommandType Command { get; set; }

      [JsonProperty("keys")]
      public required List<String> Keys { get; set; }

      [JsonProperty("remap")]
      public required bool Override { get; set; } = false;

      public override String ToString() {
        return $"{Command}\t{string.Join('-',Keys)}";
      }

    }

  }

}
