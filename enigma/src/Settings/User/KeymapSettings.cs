using Logging;
using Serilog;

namespace Settings.User.Keymap {

  public class KeymapSettings {

    private static ILogger _logger = Logger.For<KeymapSettings>();

    private void ReadKeymapSettings() {

      _logger.Information("Reading UI Settings");

    }

  }

}
