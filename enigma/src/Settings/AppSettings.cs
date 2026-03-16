using Logging;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Settings.App {

  public class AppSettings {

    private static ILogger _logger = Logger.For<AppSettings>();

    public bool IsDev = false;
    public String DbPath = "";

    public override String ToString() {
      return $"(IsDev) ={IsDev}, (DbPath) ={DbPath}";
    }

    public AppSettings(IConfiguration cm) {

      String? build = cm.GetValue<String>("build");
      if ( build == null ) {
        Console.Error.WriteLine("Failed to create App Settings, no build found");
        Environment.Exit(1);
      }
      IsDev = build == "local";
      SetReleaseDBPath(IsDev);

    }

    private void SetReleaseDBPath(bool isdev) {

      if ( IsDev ) {
        DbPath = "enigma.db";
        return;
      } 

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
