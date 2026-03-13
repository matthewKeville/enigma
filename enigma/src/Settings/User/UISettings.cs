using Logging;
using Serilog;

namespace Settings.User.UI {

  public class UISettings {

    private static ILogger _logger = Logger.For<UISettings>();

    private void ReadUISettings() {

      _logger.Information("Reading UI Settings");

    }

  }

}
