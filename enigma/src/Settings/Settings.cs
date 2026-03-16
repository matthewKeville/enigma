using Serilog;
using Logging;
using Settings.User;
using Settings.App;

namespace Settings {

  public class Settings {

    private static ILogger _logger = Logger.For<Settings>();

    public UserSettings userSettings;
    public AppSettings appSettings;

    public override String ToString() {
      return $"\n(Settings)= \n(userSettings) ={userSettings.ToString()}\n(appSettings) ={appSettings.ToString()}";
    }

    public Settings(AppSettings appSettings) {
      this.appSettings = appSettings;
      this.userSettings = UserSettings.ReadSettings();

      _logger.Debug("Loaded AppSettings");
      _logger.Debug(ToString());
    }

  }
}

