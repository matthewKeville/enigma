using Microsoft.Extensions.Configuration;

namespace Settings {

  public class AppSettings {

    public String DbPath;

    public AppSettings(IConfiguration cm) {
      String? build = cm.GetValue<String>("build");
      if ( build == null ) {
        Console.WriteLine("Failed to create Settings, no build found");
        Environment.Exit(1);
      }
      if ( build.Equals("local")) {
        DbPath = GetLocalDBPath();
      } else {
        DbPath = GetReleaseDBPath();
      }
    }

    public String GetLocalDBPath() {
      return "enigma.db";
    }

    public String GetReleaseDBPath() {

      bool windows = false;

      // defaults
      String appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      String enigmaAppData = appData + ( windows ? "\\enigma" : "/enigma");

      // config override ?
      //Environment.SpecialFolder.ApplicationData;

      //ensure data dir exists
      Directory.CreateDirectory(enigmaAppData);
      String enigmaDbPath = enigmaAppData + ( windows ? "\\enigma.db" : "/enigma.db");
      return enigmaDbPath;

    }


  }
}
