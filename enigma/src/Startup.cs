using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Logging;
using Event;
using UI.View.Game;
using UI.View.Game.Clues;
using UI.KeyMaping;
using Settings.Theme;
using Services.CrosswordService;
using Services.PluginService;
using Services.CommandServices;
using Services.PluginRunnerService;
using Services.CrosswordInstaller;
using Settings.App;
using Settings.User;

public class Startup() {

  public static void AddServices(IServiceCollection services) {

    services.AddSingleton<DatabaseContext, DatabaseContext>();

    services.AddSingleton<ListCommandService, ListCommandService>();
    services.AddSingleton<StartCommandService, StartCommandService>();
    services.AddSingleton<PluginCommandService, PluginCommandService>();
    services.AddSingleton<PluginRunnerService, PluginRunnerService>();
    services.AddSingleton<SyncCommandService, SyncCommandService>();

    services.AddSingleton<CrosswordInstallerService, CrosswordInstallerService>();
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

    services.AddSingleton<AppSettings,AppSettings>();
    services.AddSingleton<UserSettings,UserSettings>();
    services.AddSingleton<Settings.Settings, Settings.Settings>();

  }

  public static void AddConfigs(IConfigurationBuilder builder) {
    builder.AddJsonFile("appsettings.json");
    builder.AddEnvironmentVariables();
  }

  public static void UpdateOrCreateDB(DatabaseContext dbContext) {
    dbContext.Database.Migrate();
  }

  public static void AddStreamTee() {
    var originalOut = Console.Out;
    var originalErr = Console.Error;
    Console.SetOut(new TeeTextWriter(originalOut, Logger.For<Object>(), TeeTextWriter.StreamType.OUT));
    Console.SetError(new TeeTextWriter(originalErr, Logger.For<Object>(), TeeTextWriter.StreamType.ERR));
  }

}
