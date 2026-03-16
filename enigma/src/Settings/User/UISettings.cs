using Logging;
using Serilog;

namespace Settings.User.UI {

  public class UISettings {

    private static ILogger _logger = Logger.For<UISettings>();

    public override String ToString() {
      return "(UISettings) = none";
    }

  }

}
