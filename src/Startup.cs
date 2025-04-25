using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Event;
using UI.View.Game;
using UI.View.Game.Clues;
using Settings.Theme;
using Microsoft.Extensions.Configuration;
using Settings;
using UI.KeyMaping;
using Services.CrosswordService;
using Services.PluginService;
using Services.CommandServices;

public class Startup() {

  public static void AddServices(IServiceCollection services) {
    services.AddSingleton<DatabaseContext, DatabaseContext>();

    services.AddSingleton<ListCommandService, ListCommandService>();
    services.AddSingleton<StartCommandService, StartCommandService>();

    services.AddSingleton<CrosswordService, CrosswordService>();
    services.AddSingleton<PluginService, PluginService>();
    services.AddSingleton<EventBus, EventBus>();

    services.AddSingleton<KeyMaps, KeyMaps>();
    services.AddSingleton<Theme, Theme>();
    services.AddSingleton<GameView, GameView>();
    services.AddSingleton<GridView, GridView>();
    services.AddSingleton<StatusView, StatusView>();
    services.AddSingleton<KeyBindsView, KeyBindsView>();
    services.AddSingleton<CluesView, CluesView>();
    services.AddSingleton<CluesSingleView, CluesSingleView>();
    services.AddSingleton<CluesSplitView, CluesSplitView>();
    services.AddSingleton<AppSettings, AppSettings>();
  }

  public static void AddConfigs(IConfigurationBuilder builder) {
    builder.AddJsonFile("appsettings.json");
    builder.AddEnvironmentVariables();
  }

  public static void UpdateOrCreateDB(DatabaseContext dbContext) {
    dbContext.Database.Migrate();
  }

  public static void InitializeLogger() {
    // log location should depend on release type
    Trace.Listeners.Add(new TextWriterTraceListener("./logs/enigma.log"));
    Trace.AutoFlush = true;
  }

}
