using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services.CommandServices;
using Services.CommandServices.Exceptions;
using Exceptions;

IHost? host = null;

try {
  HostApplicationBuilder builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
  Startup.AddServices(builder.Services);
  Startup.AddConfigs(builder.Configuration);
  Startup.InitializeLogger();
  host = builder.Build();
  DatabaseContext dbContext = host.Services.GetRequiredService<DatabaseContext>();
  Startup.UpdateOrCreateDB(dbContext);
  host.Start();

} catch (ConfigurationException ex) {
  Console.Error.WriteLine("bad configuration file\n" + ex.Message);
} catch (Exception ex) {
  Console.Error.WriteLine("unknown startup error\n" + ex.Message);
}

if (host == null) {
  Environment.Exit(1);
}

if ( args.Count() == 0 ) {
  Console.Error.WriteLine("invalid arguments");
  Console.Error.WriteLine($"see help for available commands");
  return;
}

String command = args[0];
try {
  switch ( command ) {
    case "list":
      host.Services.GetRequiredService<ListCommandService>().List(args);
      break;
    case "sync":
      host.Services.GetRequiredService<SyncCommandService>().Sync(args);
      break;
    case "install":
      host.Services.GetRequiredService<InstallCommandService>().Install(args);
      break;
    case "plugin":
      host.Services.GetRequiredService<PluginCommandService>().Process(args);
      break;
    case "help":
      HelpCommandService.Help();
      break;
    case "start":
      host.Services.GetRequiredService<StartCommandService>().Start(args);
      break;
    default:
      Console.Error.WriteLine($"unknown command {command}");
      Console.WriteLine($"see help for available commands");
      break;

  }
} catch (CommandServiceException ex) {
  if ( ex is BadArgsException ) {
    Console.Error.WriteLine("invalid arguments\n" + ex.Message);
  } else if ( ex is NotFoundException ) {
    Console.Error.WriteLine(ex.Message);
  } else {
    Console.Error.WriteLine(ex.Message);
  }
} catch (Exception ex) {
  Console.Error.WriteLine("unexpected error \n" + ex.Message);
}
